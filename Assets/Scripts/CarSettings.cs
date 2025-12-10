using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarSettings", menuName = "Game/Car settings")]
public class CarSettings : ScriptableObject
{
    [Header("Torque")]
    [SerializeField] private float motorTorque = 1500f;
    [SerializeField] private float brakeTorque = 3000f;

    [Header("Steer")]
    [SerializeField] private float lowSpeedSteer = 20f;
    [SerializeField] private float highSpeedSteer = 10f;
    [SerializeField] private float steerSpeedThreshold = 25f;

    [Header("Downforce")]
    [SerializeField] private float downforceCoefficient = 0.8f; // Downforce coefficient k (F = k * v * v)
    // distribution of downforce between front and rear (sum = 1)
    [Range(0f, 1f)][SerializeField] private float frontDownforceRatio = 0.4f;
    [Range(0f, 1f)][SerializeField] private float rearDownforceRatio = 0.6f;
    //[SerializeField] private Transform frontDownforcePoint;
    //[SerializeField] private Transform rearDownforcePoint;
}
