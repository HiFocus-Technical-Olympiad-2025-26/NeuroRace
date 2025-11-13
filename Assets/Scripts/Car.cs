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
    [SerializeField] private float maxSteer = 25f;

    private Rigidbody rb;

    /*InputActions input;
    float throttle;
    float brake;
    float steer;
    bool handbrake;*/

    private SpawnCar spawner;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        /*input.Vehicle.Throttle.performed += ctx => throttle = ctx.ReadValue<float>();
        input.Vehicle.BrakeReverse.performed += ctx => brake = ctx.ReadValue<float>();
        input.Vehicle.Steering.performed += ctx => steer = ctx.ReadValue<float>();
        input.Vehicle.Handbrake.performed += ctx => handbrake = ctx.ReadValue<float>() > 0.5f;
        input.Vehicle.Handbrake.canceled += ctx => handbrake = false;*/

        spawner = GetComponent<SpawnCar>();
    }

    /*void Awake()
    {
        input = new InputActions();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }*/

    void FixedUpdate()
    {
        rb.centerOfMass = centerOfMass.localPosition;

        float steer = Input.GetAxis("Horizontal") * maxSteer;
        float throttle = Input.GetAxis("Vertical");
        //Debug.Log("Throttle: " + throttle);
        //Debug.Log("Steer: " + steer);

        wheelColliderLeftFront.steerAngle = steer;
        wheelColliderRightFront.steerAngle = steer;

        // Pohon zadních kol
        wheelColliderLeftBack.motorTorque = throttle * motorTorque;
        wheelColliderRightBack.motorTorque = throttle * motorTorque;

        // Brzda
        if (Input.GetKey(KeyCode.Space)) //(handbrake)
        {
            wheelColliderLeftBack.brakeTorque = brakeTorque;
            wheelColliderRightBack.brakeTorque = brakeTorque;
        }
        else
        {
            wheelColliderLeftBack.brakeTorque = 0;
            wheelColliderRightBack.brakeTorque = 0;
        }

        if (Input.GetKey(KeyCode.R))
            spawner.SpawnCarAtNearestPoint();

        if (Input.GetKey(KeyCode.Backspace))
            spawner.SpawnCarOnStart();

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
