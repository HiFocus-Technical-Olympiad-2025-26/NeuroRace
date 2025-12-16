using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Car/Wheel Setup")]
public class WheelSetup : ScriptableObject
{
    [System.Serializable]
    public struct WheelAxleSetup
    {
        [Header("General")]
        public float mass;
        public float wheelDampingRate;
        public float forceAppPointDistance;

        [Header("Suspension")]
        public float suspensionDistance;
        public float spring;
        public float damper;
        [Range(0f, 1f)]
        public float targetPosition;

        [Header("Forward Friction")]
        public float forwardExtremumSlip;
        public float forwardExtremumValue;
        public float forwardAsymptoteSlip;
        public float forwardAsymptoteValue;
        public float forwardStiffness;

        [Header("Sideways Friction")]
        public float sidewaysExtremumSlip;
        public float sidewaysExtremumValue;
        public float sidewaysAsymptoteSlip;
        public float sidewaysAsymptoteValue;
        public float sidewaysStiffness;
    }

    [Header("Front Axle")]
    public WheelAxleSetup front;

    [Header("Rear Axle")]
    public WheelAxleSetup rear;
}
