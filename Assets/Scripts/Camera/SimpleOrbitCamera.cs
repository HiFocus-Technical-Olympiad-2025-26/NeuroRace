using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrbitCamera : MonoBehaviour
{
    public Transform target;
    public Transform distanceTarget;
    public float rotationSpeed = 90f; // degrees per second while holding the button

    public float yaw = -90f;
    public float pitch = 20f;

    public float pitchMin = -3f;
    public float pitchMax = 60f;

    void Update()
    {
        if (target == null || distanceTarget == null)
            return;

        var input = InputManager.Instance.GamePlay;

        Vector2 rotation = input.CamRotationMouse != Vector2.zero 
                            ? input.CamRotationMouse 
                            : input.CamRotation;

        yaw += rotation.x * rotationSpeed * Time.deltaTime;
        pitch -= rotation.y * rotationSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Quaternion rot = Quaternion.Euler(pitch + target.rotation.eulerAngles.x + 0f, yaw + target.eulerAngles.y + 90f, distanceTarget.eulerAngles.z);
        //Quaternion rot = Quaternion.Euler(distanceTarget.eulerAngles.x, yaw + target.rotation.eulerAngles.y + 90f, distanceTarget.eulerAngles.z);

        Vector3 offset = rot * Vector3.forward * -distanceTarget.localPosition.magnitude;

        transform.position = target.position + offset;

        transform.LookAt(target.position);
    }
}
