using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarSettings", menuName = "Car/Car settings")]
public class CarSettings : ScriptableObject
{
    [Header("Torque")]
    public float motorTorque = 1500f;
    public float brakeTorque = 3000f;

    [Header("Steer")]
    public float lowSpeedSteer = 20f;
    public float highSpeedSteer = 10f;
    public float steerSpeedThreshold = 25f;
}
