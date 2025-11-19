using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputActions input;

    public bool quitPressed { get; private set; }
    public float steerValue { get; private set; } = 0;
    public float throttleValue { get; private set; } = 0;
    public float brakeValue { get; private set; } = 0;

    public bool spawnPressed { get; private set; } = false;

    public bool spawnOnStartPressed { get; private set; } = false;

    public bool nextCamPressed { get; private set; } = false;

    public bool prevCamPressed { get; private set; } = false;

    public Vector2 camRotation {  get; private set; }
    public Vector2 camRotationMouse { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // only 1 InputManager
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        input = new InputActions();

        input.GamePlay.Steer.performed += ctx => steerValue = ctx.ReadValue<float>();
        input.GamePlay.Steer.canceled += ctx => steerValue = 0f;

        input.GamePlay.Throttle.performed += ctx => throttleValue = ctx.ReadValue<float>();
        input.GamePlay.Throttle.canceled += ctx => throttleValue = 0f;

        input.GamePlay.Brake.performed += ctx => brakeValue = ctx.ReadValue<float>();
        input.GamePlay.Brake.canceled += ctx => brakeValue = 0f;

        input.GamePlay.CameraRotate.performed += ctx => camRotation = ctx.ReadValue<Vector2>();
        input.GamePlay.CameraRotate.canceled += ctx => camRotation = Vector2.zero;

        input.GamePlay.CameraRotateMouse.performed += ctx => camRotationMouse = ctx.ReadValue<Vector2>();
        input.GamePlay.CameraRotateMouse.canceled += ctx => camRotationMouse = Vector2.zero;


        input.GamePlay.Quit.performed += _ => quitPressed = true;
        input.GamePlay.Quit.canceled += _ => quitPressed = false;

        input.GamePlay.NextCamera.performed += _ => nextCamPressed = true;
        input.GamePlay.NextCamera.canceled += _ => nextCamPressed = false;

        input.GamePlay.PrevCamera.performed += _ => prevCamPressed = true;
        input.GamePlay.PrevCamera.canceled += _ => prevCamPressed = false;

        input.GamePlay.Respawn.performed += _ => spawnPressed = true;
        input.GamePlay.Respawn.canceled += _ => spawnPressed = false;

        input.GamePlay.SpawnStart.performed += _ => spawnOnStartPressed = true;
        input.GamePlay.SpawnStart.canceled += _ => spawnOnStartPressed = false;

        input.Enable();
        input.GamePlay.Enable();
    }

    public bool ConsumeQuitPressed()
    {
        bool value = quitPressed;
        quitPressed = false;
        return value;
    }

    public bool ConsumeSpawnPressed()
    {
        bool value = spawnPressed;
        spawnPressed = false;
        return value;
    }

    public bool ConsumeSpawnOnStartPressed()
    {
        bool value = spawnOnStartPressed;
        spawnOnStartPressed = false;
        return value;
    }


    public bool ConsumeNextCamPressed()
    {
        bool value = nextCamPressed;
        nextCamPressed = false;
        return value;
    }

    public bool ConsumePrevCamPressed()
    {
        bool value = prevCamPressed;
        prevCamPressed = false;
        return value;
    }
}
