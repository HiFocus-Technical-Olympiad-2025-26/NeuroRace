using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform camTarget;  // object to follow
    public float smoothY = 0.08f; // vertical smoothing
    public float smoothRotation = 0.1f; // overall rotation smoothing

    private float yVelocity = 0f; // used by SmoothDamp
    private bool wasCamEnabledLastTime = false;

    void Start()
    {
        ResetCameraInstant();
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
            ResetCameraInstant();
            wasCamEnabledLastTime = true;
        }

        // XZ snaps immediately, Y is smoothed
        Vector3 targetPos = camTarget.position;

        float newY = Mathf.SmoothDamp(
            transform.position.y,
            targetPos.y,
            ref yVelocity,
            smoothY
        );

        transform.position = new Vector3(
            targetPos.x,
            newY,
            targetPos.z
        );

        Quaternion targetRot = camTarget.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, smoothRotation);
    }

    private void ResetCameraInstant()
    {
        yVelocity = 0f;
        this.transform.position = camTarget.position;
        this.transform.rotation = camTarget.rotation;
    }
}
