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
    public WheelSetup defaultWheelSetup;

    [Header("Car Settings")]
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float motorTorque = 4000f;
    [SerializeField] private float brakeTorque = 1500f;
    public bool isThrottleEnabled = true;

    [Header("Downforce")]
    [SerializeField] private float downforceCoefficient = 0.8f; // Downforce coefficient k (F = k * v * v)
    // distribution of downforce between front and rear (sum = 1)
    [Range(0f, 1f)] [SerializeField] private float frontDownforceRatio = 0.4f;
    [Range(0f, 1f)] [SerializeField] private float rearDownforceRatio = 0.6f;
    [SerializeField] private Transform frontDownforcePoint;
    [SerializeField] private Transform rearDownforcePoint;

    [Header("Steer Settings")]
    //[SerializeField] private float maxSteer = 25f;
    [SerializeField] private float lowSpeedSteer = 20f;
    [SerializeField] private float highSpeedSteer = 10f;
    [SerializeField] private float steerSpeedThreshold = 25f;
    
    public float speed { get; private set; } = 0f;

    protected Rigidbody rb;

    public SpawnSystem spawner;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        this.ApplyDefaultWheelParameters();
    }


    protected void ApplyPhysics(CarInput input)
    {
        rb.centerOfMass = centerOfMass.localPosition;

        this.speed = rb.velocity.magnitude; // current speed in m/s

        // steering
        float steerFactor = Mathf.Clamp01(this.speed / steerSpeedThreshold);
        float currentMaxSteer = Mathf.Lerp(lowSpeedSteer, highSpeedSteer, steerFactor);
        //Debug.Log("Steer: " + currentMaxSteer);
        wheelColliderLeftFront.steerAngle = wheelColliderRightFront.steerAngle = input.Steer * currentMaxSteer;

        // brake, reverse, throttle
        float brake = 0;
        float reverse = 0;
        float brakeReverse = input.Brake;
        if (this.speed > 2 || !isThrottleEnabled)
            brake = brakeReverse;
        else
            reverse = brakeReverse;

        float throttle = isThrottleEnabled ? input.Throttle : 0;

        float finalBrakeTorque = brake * brakeTorque;
        float finalMotorTorque;
        if (reverse > 0)
            finalMotorTorque = -1 * reverse * motorTorque;
        else
            finalMotorTorque = throttle * motorTorque;

        wheelColliderLeftBack.motorTorque = wheelColliderRightBack.motorTorque = finalMotorTorque;
        wheelColliderLeftBack.brakeTorque = wheelColliderRightBack.brakeTorque = wheelColliderLeftFront.brakeTorque = wheelColliderRightFront.brakeTorque = finalBrakeTorque;

        //downforce
        float downforceTotal = downforceCoefficient * speed * speed;
        Vector3 down = -transform.up;
        //rb.AddForceAtPosition(down * downforceTotal * frontDownforceRatio, frontDownforcePoint.position, ForceMode.Force);
        //rb.AddForceAtPosition(down * downforceTotal * rearDownforceRatio, rearDownforcePoint.position, ForceMode.Force);

        UpdateWheelPose(wheelColliderLeftFront, wheelLeftFront, true);
        UpdateWheelPose(wheelColliderRightFront, wheelRightFront, false);
        UpdateWheelPose(wheelColliderLeftBack, wheelLeftBack, true);
        UpdateWheelPose(wheelColliderRightBack, wheelRightBack, false);
    }


    #region WheelSetup
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

    public void ApplyDefaultWheelParameters()
    {
        if(defaultWheelSetup != null)
            ApplyWheelParameters(defaultWheelSetup);
    }

    public void ApplyWheelParameters(WheelSetup setup)
    {
        SetWheelSetup(wheelColliderLeftFront, setup.front);
        SetWheelSetup(wheelColliderRightFront, setup.front);

        SetWheelSetup(wheelColliderLeftBack, setup.rear);
        SetWheelSetup(wheelColliderRightBack, setup.rear);
    }

    private void SetWheelSetup(WheelCollider wc, WheelSetup.WheelAxleSetup setup)
    {
        wc.mass = setup.mass;
        wc.wheelDampingRate = setup.wheelDampingRate;
        wc.forceAppPointDistance = setup.forceAppPointDistance;

        wc.suspensionDistance = setup.suspensionDistance;

        JointSpring spring = wc.suspensionSpring;
        spring.spring = setup.spring;
        spring.damper = setup.damper;
        spring.targetPosition = setup.targetPosition;
        wc.suspensionSpring = spring;

        WheelFrictionCurve forward = wc.forwardFriction;
        forward.extremumSlip = setup.forwardExtremumSlip;
        forward.extremumValue = setup.forwardExtremumValue;
        forward.asymptoteSlip = setup.forwardAsymptoteSlip;
        forward.asymptoteValue = setup.forwardAsymptoteValue;
        forward.stiffness = setup.forwardStiffness;
        wc.forwardFriction = forward;

        WheelFrictionCurve sideways = wc.sidewaysFriction;
        sideways.extremumSlip = setup.sidewaysExtremumSlip;
        sideways.extremumValue = setup.sidewaysExtremumValue;
        sideways.asymptoteSlip = setup.sidewaysAsymptoteSlip;
        sideways.asymptoteValue = setup.sidewaysAsymptoteValue;
        sideways.stiffness = setup.sidewaysStiffness;
        wc.sidewaysFriction = sideways;
    }
    #endregion
}