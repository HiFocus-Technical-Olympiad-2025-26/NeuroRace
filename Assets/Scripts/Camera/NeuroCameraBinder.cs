using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextMind;

public class NeuroCameraBinder : MonoBehaviour
{
    public static NeuroCameraBinder Instance;

    [SerializeField] private Camera defaultCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (defaultCamera != null)
        {
            AssignCamera(defaultCamera);
        }
    }
    

    public static void AssignCamera(Camera cam)
    {
        if (cam == null)
        {
            Debug.LogError("NeuroCameraBinder.AssignCamera: Camera is null!");
            return;
        }

        NeuroManager manager = FindObjectOfType<NeuroManager>();
        if (manager == null)
        {
            Debug.LogError("NeuroCameraBinder.AssignCamera: NeuroManager not found!");
            return;
        }

        manager.TrackingCamera = cam;

        Debug.Log($"NeuroManager tracking camera updated to: {cam.name}");
    }
}