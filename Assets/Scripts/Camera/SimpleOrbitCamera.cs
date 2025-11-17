using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrbitCamera : MonoBehaviour
{
    public Transform target;
    public Transform distanceTarget;
    public float rotationSpeed = 90f; // degrees per second while holding the button

    public float yaw = -90f;

    void Update()
    {
        if (target == null || distanceTarget == null)
            return;
        
        float input = InputManager.Instance.RotationCamValue; // -1,0,1
        yaw += input * rotationSpeed * Time.deltaTime;

        Quaternion rot = Quaternion.Euler(distanceTarget.eulerAngles.x, yaw + target.rotation.eulerAngles.y + 90f, distanceTarget.eulerAngles.z);

        Vector3 offset = rot * Vector3.forward * -distanceTarget.localPosition.magnitude;

        transform.position = target.position + offset;

        transform.LookAt(target.position);
    }
}
