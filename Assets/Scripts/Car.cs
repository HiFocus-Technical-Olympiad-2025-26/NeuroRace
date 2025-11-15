using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider wheelColliderLeftFront;
    [SerializeField] private WheelCollider wheelColliderRightFront;
    [SerializeField] private WheelCollider wheelColliderLeftBack;
    [SerializeField] private WheelCollider wheelColliderRightBack;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform wheelLeftFront;
    [SerializeField] private Transform wheelRightFront;
    [SerializeField] private Transform wheelLeftBack;
    [SerializeField] private Transform wheelRightBack;

    [Header("Car Settings")]
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float motorTorque = 1500f;
    [SerializeField] private float brakeTorque = 3000f;
    [SerializeField] private float downforce = 50f;

    [Header("Steer Settings")]
    //[SerializeField] private float maxSteer = 25f;
    [SerializeField] private float lowSpeedSteer = 20f;
    [SerializeField] private float highSpeedSteer = 10f;
    [SerializeField] private float steerSpeedThreshold = 25f;

    private Rigidbody rb;

    private SpawnSystem spawner;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        spawner = GetComponent<SpawnSystem>();
    }

    void FixedUpdate()
    {
        rb.centerOfMass = centerOfMass.localPosition;
        float speed = rb.velocity.magnitude; // current speed in m/s
        //Debug.Log("speed: " + speed);

        float steerFactor = Mathf.Clamp01(speed / steerSpeedThreshold);
        float currentMaxSteer = Mathf.Lerp(lowSpeedSteer, highSpeedSteer, steerFactor);
        //Debug.Log("Steer: " + currentMaxSteer);

        wheelColliderLeftFront.steerAngle = wheelColliderRightFront.steerAngle = InputManager.Instance.steerValue * currentMaxSteer;
        wheelColliderLeftBack.motorTorque = wheelColliderRightBack.motorTorque = InputManager.Instance.throttleValue * motorTorque;
        wheelColliderLeftBack.brakeTorque = wheelColliderRightBack.brakeTorque = wheelColliderLeftFront.brakeTorque = wheelColliderRightFront.brakeTorque = InputManager.Instance.handbrakePressed ? brakeTorque : 0;

        if (InputManager.Instance.spawnPressed)
            spawner.SpawnCarAtNearestPoint();

        if (InputManager.Instance.spawnOnStartPressed)
            spawner.SpawnCarOnStart();

        rb.AddForce(downforce * speed * speed * -transform.up); // downforce

        UpdateWheelPose(wheelColliderLeftFront, wheelLeftFront, true);
        UpdateWheelPose(wheelColliderRightFront, wheelRightFront, false);
        UpdateWheelPose(wheelColliderLeftBack, wheelLeftBack, true);
        UpdateWheelPose(wheelColliderRightBack, wheelRightBack, false);
    }

    void UpdateWheelPose(WheelCollider col, Transform wheel, bool isLeft)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        wheel.position = pos;

        if (isLeft)
            wheel.rotation = rot * Quaternion.Euler(0, 180, 0);
        else
            wheel.rotation = rot;
    }
}
