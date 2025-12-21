using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float steeringDeadZone = 0.1f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask collisions;
    [SerializeField] private int minimalSpeed = 10;
    [SerializeField] private float brakingThreshold = 15;
    [SerializeField] private int turnClearDistance = 20;
    [SerializeField] private int maxSpeed = 25;

    [Header("Refferences")]
    [SerializeField] private Transform lrTransform;
    [SerializeField] private Transform rrTransform;
    
    public CarInput carInput;

    private AICar car;
    private AICheckpoint current_inst;
    private bool follow_direction = false;
    private bool ignoreRules = false;
    [SerializeField] private float laneRatio = 1;

    private float RussianElimination(float target, float replacement, float threshold) {
        if (target <= threshold) {
            target = replacement; // Target found dead under a window
        }

        return target;
    }

    private void Start()
    {
        laneRatio = Random.Range(0.5f, 1.5f);
        car = GetComponent<AICar>();
    }

    void FixedUpdate()
    {
        if (car.speed < maxSpeed)
        {
            carInput.Throttle = 1;
        }
        else {
            carInput.Throttle = 0;
        }

        var frontRay = new Ray(transform.position, transform.forward);
        RaycastHit frHitInfo;
        Physics.Raycast(frontRay, out frHitInfo, Mathf.Infinity, mask.value);

        var leftRay = new Ray(lrTransform.position, lrTransform.forward);
        RaycastHit lrHitInfo;
        Physics.Raycast(leftRay, out lrHitInfo, Mathf.Infinity, mask.value);

        var rightRay = new Ray(rrTransform.position, rrTransform.forward);
        RaycastHit rrHitInfo;
        Physics.Raycast(rightRay, out rrHitInfo, Mathf.Infinity, mask.value);

        var collisionRay = new Ray(transform.position, transform.forward);
        RaycastHit crHitInfo;
        Physics.Raycast(collisionRay, out crHitInfo, Mathf.Infinity, mask.value);

        carInput.Steer = lrHitInfo.distance / rrHitInfo.distance < steeringDeadZone ? 0 : -Mathf.Clamp(lrHitInfo.distance * laneRatio - rrHitInfo.distance, -1, 1);

        if (frHitInfo.distance > turnClearDistance && follow_direction) 
        {
            follow_direction = false;
        }

        if (follow_direction) 
        {
            carInput.Steer = current_inst.direction == AIDirection.Left ? -1 : 1;
        }

        if (frHitInfo.distance == 0 || ignoreRules) 
        {
            carInput.Brake = 0;
            return;
        }

        float braking_ratio = Mathf.Min(frHitInfo.distance / car.speed, RussianElimination(crHitInfo.distance, 10000, 0) / car.speed);

        if (braking_ratio < brakingThreshold && car.speed > minimalSpeed)
        {
            //Debug.Log("Breaking, value: " + frHitInfo.distance / car.speed);
            carInput.Brake = 1 / braking_ratio;
        }
        else 
        {
            carInput.Brake = 0;
        }

        if (car.speed < minimalSpeed) {
            carInput.Spawn = true;
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
            current_inst = other.GetComponent<AICheckpoint>();
            follow_direction = !current_inst.removeCheckpointEffects;
            ignoreRules = current_inst.ignoreRules;
            laneRatio = Random.Range(0.5f, 1.5f);
        }
    }
}