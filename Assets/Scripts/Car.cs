using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider wheelColliderLeftFront;
    public WheelCollider wheelColliderRightFront;
    public WheelCollider wheelColliderLeftBack;
    public WheelCollider wheelColliderRightBack;

    [Header("Wheel Meshes")]
    public Transform wheelLeftFront;
    public Transform wheelRightFront;
    public Transform wheelLeftBack;
    public Transform wheelRightBack;

    [Header("Car Settings")]
    public Transform centerOfMass;
    public float motorTorque = 1500f;
    public float brakeTorque = 3000f;
    public float maxSteer = 25f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    void FixedUpdate()
    {
        rb.centerOfMass = centerOfMass.localPosition;

        float steer = Input.GetAxis("Horizontal") * maxSteer;
        float throttle = Input.GetAxis("Vertical");
        Debug.Log("Throttle: " + throttle);
        Debug.Log("Steer: " + steer);

        wheelColliderLeftFront.steerAngle = steer;
        wheelColliderRightFront.steerAngle = steer;

        // Pohon zadních kol
        wheelColliderLeftBack.motorTorque = throttle * motorTorque;
        wheelColliderRightBack.motorTorque = throttle * motorTorque;

        // Brzda (Space)
        if (Input.GetKey(KeyCode.Space))
        {
            wheelColliderLeftBack.brakeTorque = brakeTorque;
            wheelColliderRightBack.brakeTorque = brakeTorque;
        }
        else
        {
            wheelColliderLeftBack.brakeTorque = 0;
            wheelColliderRightBack.brakeTorque = 0;
        }

        UpdateWheelPose(wheelColliderLeftFront, wheelLeftFront, true);
        UpdateWheelPose(wheelColliderRightFront, wheelRightFront, false);
        UpdateWheelPose(wheelColliderLeftBack, wheelLeftBack, true);
        UpdateWheelPose(wheelColliderRightBack, wheelRightBack, false);
    }

    /// <summary>
    /// Synchronizuje fyzikální WheelCollider s vizuálním kolem.
    /// Parametr isLeft urèuje, zda jde o levé kolo — pro otoèení disku.
    /// </summary>
    void UpdateWheelPose(WheelCollider col, Transform wheel, bool isLeft)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        wheel.position = pos;

        // levá kola otoèíme o 180° kolem osy Y, aby disky nebyly zrcadlovì špatnì
        if (isLeft)
            wheel.rotation = rot * Quaternion.Euler(0, 180, 0);
        else
            wheel.rotation = rot;
    }

    /*void FixedUpdate()
    {
        //wheelColliderLeftFront.steerAngle = maxSteer * Input.GetAxis("Horizontal");
        //wheelColliderRightFront.steerAngle = maxSteer * Input.GetAxis("Horizontal");
        if(Input.GetAxis("Vertical") != 0)
        {
            wheelColliderLeftBack.motorTorque = motorTorque * Input.GetAxis("Vertical");
            wheelColliderRightBack.motorTorque = motorTorque * Input.GetAxis("Vertical");
            //wheelColliderLeftFront.motorTorque = motorTorque * Input.GetAxis("Vertical");
            //wheelColliderRightFront.motorTorque = motorTorque * Input.GetAxis("Vertical");
        }
    }

    void Update()
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;

        wheelColliderLeftFront.GetWorldPose(out pos, out rot);
        wheelLeftFront.position = pos;
        wheelLeftFront.rotation = rot * Quaternion.Euler(0, 180, 0);

        wheelColliderRightFront.GetWorldPose(out pos, out rot);
        wheelRightFront.position = pos;
        wheelRightFront.rotation = rot;

        wheelColliderLeftBack.GetWorldPose(out pos, out rot);
        wheelLeftBack.position = pos;
        wheelLeftBack.rotation = rot * Quaternion.Euler(0, 180, 0);

        wheelColliderRightBack.GetWorldPose(out pos, out rot);
        wheelRightBack.position = pos;
        wheelRightBack.rotation = rot;
    }*/
}
