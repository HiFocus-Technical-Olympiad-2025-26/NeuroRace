using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float steeringDeadZone = 0.1f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private int minimalSpeed = 10;
    [SerializeField] private float brakingThreshold = 15;
    [SerializeField] private int turnClearDistance = 20;

    [Header("Refferences")]
    [SerializeField] private Transform lrTransform;
    [SerializeField] private Transform rrTransform;
    
    public CarInput carInput;

    private AICar car;
    private Checkpoint current_inst;
    private bool follow_direction = false;
    private bool ignoreRules = false;

    private void Start()
    {
        car = GetComponent<AICar>();
    }

    void FixedUpdate()
    {
        carInput.Throttle = 1;

        var frontRay = new Ray(transform.position, transform.forward);
        RaycastHit frHitInfo;
        Physics.Raycast(frontRay, out frHitInfo, Mathf.Infinity, mask.value);

        var leftRay = new Ray(lrTransform.position, lrTransform.forward);
        RaycastHit lrHitInfo;
        Physics.Raycast(leftRay, out lrHitInfo, Mathf.Infinity, mask.value);

        var rightRay = new Ray(rrTransform.position, rrTransform.forward);
        RaycastHit rrHitInfo;
        Physics.Raycast(rightRay, out rrHitInfo, Mathf.Infinity, mask.value);

        carInput.Steer = lrHitInfo.distance / rrHitInfo.distance < steeringDeadZone ? 0 : -Mathf.Clamp(lrHitInfo.distance - rrHitInfo.distance, -1, 1);

        if (frHitInfo.distance > turnClearDistance && follow_direction) 
        {
            follow_direction = false;
        }

        if (follow_direction) 
        {
            carInput.Steer = current_inst.direction == Direction.Left ? -1 : 1;
        }

        if (frHitInfo.distance == 0 || ignoreRules) 
        {
            carInput.Brake = 0;
            return;
        }

        float braking_ratio = frHitInfo.distance / car.speed;

        if (braking_ratio < brakingThreshold && car.speed > minimalSpeed)
        {
            //Debug.Log("Breaking, value: " + frHitInfo.distance / car.speed);
            carInput.Brake = 1 / braking_ratio;
        }
        else 
        {
            carInput.Brake = 0;
        }
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * turnClearDistance, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint") 
        {
            current_inst = other.GetComponent<Checkpoint>();
            follow_direction = !current_inst.removeCheckpointEffects;
            ignoreRules = current_inst.ignoreRules;
        }
    }
}