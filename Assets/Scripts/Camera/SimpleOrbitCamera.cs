using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrbitCamera : MonoBehaviour
{
    public Transform target;
    public Transform distanceTarget;
    public float rotationSpeed = 90f; // degrees per second while holding the button

    public float defaultYaw = -90f;
    public float defaultPitch = 20f;

    public float pitchMin = -3f;
    public float pitchMax = 60f;

    private float currentYaw;
    private float currentPitch;

    private bool lastCameraState;

    private void Start()
    {
        ResetOrbit();
    }

    void Update()
    {
        if (target == null || distanceTarget == null)
            return;

        Camera cam = GetComponent<Camera>();
        if (lastCameraState && !cam.enabled)
            ResetOrbit();
        lastCameraState = cam.enabled;

        var input = InputManager.Instance.GamePlay;

        Vector2 rotation = input.CamRotationMouse != Vector2.zero 
                            ? input.CamRotationMouse 
                            : input.CamRotation;

        currentYaw += rotation.x * rotationSpeed * Time.deltaTime;
        currentPitch -= rotation.y * rotationSpeed * Time.deltaTime;

        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);

        Quaternion rot = Quaternion.Euler(currentPitch + target.rotation.eulerAngles.x + 0f, currentYaw + target.eulerAngles.y + 90f, distanceTarget.eulerAngles.z);
        //Quaternion rot = Quaternion.Euler(distanceTarget.eulerAngles.x, currentYaw + target.rotation.eulerAngles.y + 90f, distanceTarget.eulerAngles.z);

        Vector3 offset = rot * Vector3.forward * -distanceTarget.localPosition.magnitude;

        transform.position = target.position + offset;

        transform.LookAt(target.position);
    }

    public void ResetOrbit()
    {
        currentYaw = defaultYaw;
        currentPitch = defaultPitch;
    }
}
