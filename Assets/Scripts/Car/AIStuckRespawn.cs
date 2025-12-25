using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIStuckRespawn : MonoBehaviour
{
    [Header("Stuck Detection")]
    [SerializeField] private float minSpeed = 1.0f; // m/s
    [SerializeField] private float stuckTime = 3.0f; // seconds
    [SerializeField] private float upsideDownDot = 0.4f; // how much upside-down is allowed

    private Rigidbody rb;
    private AICar car;
    private float stuckTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        car = GetComponent<AICar>();
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

        bool isUpsideDown = Vector3.Dot(transform.up, Vector3.up) < upsideDownDot;
        bool isSlow = rb.velocity.magnitude < minSpeed;

        if (isUpsideDown || isSlow)
        {
            stuckTimer += Time.fixedDeltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        if (stuckTimer >= stuckTime)
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
