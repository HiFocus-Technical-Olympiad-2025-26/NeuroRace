using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Camera> cameras = new List<Camera>();
    public int currentCamIndex = 0;

    void Start()
    {
        if (cameras.Count == 0)
        {
            Debug.Log("Camera list is empty");
            return;
        }

        if (currentCamIndex < 0 ||  currentCamIndex >= cameras.Count)
            currentCamIndex = (currentCamIndex + cameras.Count) % cameras.Count;

        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled = (i == currentCamIndex);
            if (cameras[i].enabled)
                NeuroCameraBinder.AssignCamera(cameras[i]);
        }
    }

    void Update()
    {
        if (InputManager.Instance.ConsumeNextCamPressed())
            SetCameraIndex(currentCamIndex + 1);

        if (InputManager.Instance.ConsumePrevCamPressed())
            SetCameraIndex(currentCamIndex - 1);
    }

    public void SetCameraIndex(int newIndex)
    {
        if (cameras.Count == 0)
        {
            Debug.Log("Camera list is empty");
            return;
        }

        cameras[currentCamIndex].enabled = false;
        currentCamIndex = (newIndex + cameras.Count) % cameras.Count;
        cameras[currentCamIndex].enabled = true;
        NeuroCameraBinder.AssignCamera(cameras[currentCamIndex]);
    }
}
