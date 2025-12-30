using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownFollowCamera : MonoBehaviour
{
    public Transform camTarget;  // object to follow
    public float offset = 12f;
    public float positionSmooth = 0.1f;
    public float rotationSmooth = 0.15f;

    private bool wasCamEnabledLastTime = false;

    void Start()
    {
        ResetCameraInstant();
    }

    void Update()
    {
        if (!enabled)
        {
            wasCamEnabledLastTime = false;
            return;
        }
        else if (!wasCamEnabledLastTime)
        {
            ResetCameraInstant();
            wasCamEnabledLastTime = true;
        }

        Vector3 desiredPos = camTarget.position + new Vector3(0f, offset, 0f);
        transform.position = Vector3.Lerp(transform.position, desiredPos, positionSmooth);


        Quaternion targetRot = Quaternion.Euler(90f, camTarget.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth);
    }

    private void ResetCameraInstant()
    {
        transform.position = camTarget.position + new Vector3(0f, offset, 0f);
        transform.rotation = Quaternion.Euler(90f, camTarget.eulerAngles.y, 0f);
    }
}
