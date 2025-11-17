using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public List<Transform> cameraTargets = new List<Transform>();
    public float smoothSpeed = 0f;

    private int currentCameraIndex = 0;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (cameraTargets.Count > 0 && mainCamera != null)
            ResetCameraPosition();
    }

    void Update()
    {
        bool cameraChanged = false;

        if (InputManager.Instance.ConsumeNextCamPressed())
        {
            currentCameraIndex = (currentCameraIndex + 1) % cameraTargets.Count;
            cameraChanged = true;
        }

        if (InputManager.Instance.ConsumePrevCamPressed())
        {
            currentCameraIndex = (currentCameraIndex - 1 + cameraTargets.Count) % cameraTargets.Count;
            cameraChanged = true;
        }

        if (cameraTargets.Count == 0) 
            return;
        
        if (cameraChanged)
            ResetCameraPosition();

        // smooth movement and rotation of camera
        Transform target = cameraTargets[currentCameraIndex];

        // position
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, target.position, ref velocity, smoothSpeed);

        // rotation
        Quaternion targetRotation = Quaternion.LookRotation(target.forward, target.up);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, smoothSpeed);
    }

    private void ResetCameraPosition()
    {
        velocity = Vector3.zero;
        mainCamera.transform.position = cameraTargets[currentCameraIndex].position;
        mainCamera.transform.rotation = Quaternion.LookRotation(cameraTargets[currentCameraIndex].forward, cameraTargets[currentCameraIndex].up);
    }
}
