using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Camera> cameras = new List<Camera>();
    public int currentCameraIndex = 0;

    void Update()
    {
        if (InputManager.Instance.ConsumeNextCamPressed())
            currentCameraIndex++;

        if (InputManager.Instance.ConsumePrevCamPressed())
            currentCameraIndex--;

        if (currentCameraIndex >= cameras.Count)
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;

        if (currentCameraIndex < 0)
            currentCameraIndex = (currentCameraIndex - 1 + cameras.Count) % cameras.Count;

        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled = (i == currentCameraIndex);
            if (cameras[i].enabled)
                NeuroCameraBinder.AssignCamera(cameras[i]);
        }

        //Debug.Log("currentCameraIndex: " + currentCameraIndex);
    }
}
