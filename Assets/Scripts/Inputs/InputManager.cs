using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }


    public float steerValue { get; private set; } = 0;
    public float throttleValue { get; private set; } = 0;
    public bool handbrakePressed { get; private set; } = false;

    public bool spawnPressed { get; private set; } = false;
    private bool spawnPressedPrev = false;

    public bool spawnOnStartPressed { get; private set; } = false;
    private bool spawnOnStartPressedPrev = false;

    public bool nextCamPressed { get; private set; } = false;
    private bool nextCamPressedPrev = false;

    public bool prevCamPressed { get; private set; } = false;
    private bool prevCamPressedPrev = false;

    public float RotationCamValue { get; private set; } = 0f;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // only 1 InputManager
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    float BoolToFloat(bool b) => b ? 1f : 0f;

    void Update()
    {
        steerValue = Input.GetAxis("Horizontal");
        throttleValue = Input.GetAxis("Vertical");
        handbrakePressed = Input.GetKey(KeyCode.Space);

        spawnPressed = CheckPressed(ref spawnPressedPrev, Input.GetKey(KeyCode.R));
        spawnOnStartPressed = CheckPressed(ref spawnOnStartPressedPrev, Input.GetKey(KeyCode.Backspace));

        nextCamPressed = CheckPressed(ref nextCamPressedPrev, Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift));
        prevCamPressed = CheckPressed(ref prevCamPressedPrev, Input.GetKey(KeyCode.C) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));

        RotationCamValue = 0f + BoolToFloat(Input.GetKey(KeyCode.Q)) - BoolToFloat(Input.GetKey(KeyCode.E));
    }

    private bool CheckPressed(ref bool prevState, bool currentState)
    {
        bool pressed = currentState && !prevState;
        prevState = currentState;
        return pressed;
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
