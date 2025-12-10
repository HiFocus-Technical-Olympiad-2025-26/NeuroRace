using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float steeringDeadZone = 0.1f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private int minimalSpeed = 10;
    [SerializeField] private int brakingDistance = 15;

    [Header("Refferences")]
    [SerializeField] private Transform lrTransform;
    [SerializeField] private Transform rrTransform;
    [SerializeField] private List<GameObject> checkpoints;

    private AICar car;
    private Instruction current_inst;

    public CarInput carInput;


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

        if (frHitInfo.distance < brakingDistance / (minimalSpeed / car.speed) && car.speed > minimalSpeed)
        {
            carInput.Brake = 1;
        }
        else 
        {
            carInput.Brake = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint") {
            current_inst = other.GetComponent<Checkpoint>().instruction;
            other.gameObject.SetActive(false);
        }
    }
}