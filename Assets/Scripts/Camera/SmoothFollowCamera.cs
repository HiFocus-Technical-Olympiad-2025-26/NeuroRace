using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform cameraTarget;
    public float smoothSpeed = 0f;

    private Vector3 velocity = Vector3.zero;
    private bool wasCamEnabledLastTime;

    void Start()
    {
        ResetCameraPosition();
    }

    void Update()
    {
        if (!this.enabled)
        {
            wasCamEnabledLastTime = false;
            return; // camera is not visible -> there is no point in setting camera position and rotation
        }
        else if (!wasCamEnabledLastTime)
        {
            ResetCameraPosition();
            wasCamEnabledLastTime = true;
        }

        // position
        this.transform.position = Vector3.SmoothDamp(this.transform.position, cameraTarget.position, ref velocity, smoothSpeed);

        // rotation
        Quaternion targetRotation = Quaternion.LookRotation(cameraTarget.forward, cameraTarget.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, smoothSpeed);
    }

    private void ResetCameraPosition()
    {
        velocity = Vector3.zero;
        this.transform.position = cameraTarget.position;
        this.transform.rotation = Quaternion.LookRotation(cameraTarget.forward, cameraTarget.up);
    }
}
