using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Camera> cameras = new List<Camera>();
    public int CurrentCameraIndex = 0;

    void Update()
    {
        if (InputManager.Instance.ConsumeNextCamPressed())
            CurrentCameraIndex++;

        if (InputManager.Instance.ConsumePrevCamPressed())
            CurrentCameraIndex--;

        if (CurrentCameraIndex >= cameras.Count)
            CurrentCameraIndex -= cameras.Count;

        if (CurrentCameraIndex < 0)
            CurrentCameraIndex += cameras.Count;

        for(int i = 0; i < cameras.Count; i++)
            cameras[i].enabled = (i == CurrentCameraIndex);

        //Debug.Log("CurrentCameraIndex: " + CurrentCameraIndex);
    }
}
