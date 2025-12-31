using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIStuckRespawn : MonoBehaviour
{
    [Header("Stuck Detection")]
    /*[SerializeField]*/ private float minSpeed = 1.0f; // m/s
    [SerializeField] private float stuckTime = 3.0f; // seconds
    [SerializeField] private float upsideDownDot = 0.4f; // how much upside-down is allowed
    //[SerializeField] private bool PrintDot = false;

    private Rigidbody rb;
    private AICar car;
    private AIController carController;
    private float stuckTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        car = GetComponent<AICar>();
        carController = GetComponent<AIController>();

        if (carController != null)
        {
            minSpeed = carController.GetMinSpeed();
        }
    }

    private void FixedUpdate()
    {
        if (rb == null || car.spawner == null)
            return;

        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Running)
        {
            stuckTimer = 0f;
            return;
        }

        var dot = Vector3.Dot(transform.up.normalized, Vector3.up);
        /*if(PrintDot)
            Debug.Log("Dot: " + dot);*/
        bool isUpsideDown = dot < upsideDownDot;
        bool isSlow = rb.velocity.magnitude < minSpeed;

        /*if(isUpsideDown)
            Debug.Log("Car is upside down.");*/

        if (isUpsideDown || isSlow || car.spawner.IsCarGoingWrongDirection(car.transform, 0f))
        {
            stuckTimer += Time.fixedDeltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        if (stuckTimer >= stuckTime || car.spawner.IsCarGoingWrongDirection(car.transform, -0.5f))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        stuckTimer = 0f;
        car.spawner.SpawnCarAtNearestPoint(transform);
    }
}
