using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using NextMind;

public class FirstMenu : MonoBehaviour
{
    public GameObject firstButton;

    private bool hasFocusedBtn = false;


    private void Start()
    {
        InputManager.Instance.InputMap_Menu();
    }

    void Update()
    {
        Vector2 dir = InputManager.Instance.Menu.Direction;

        if (!hasFocusedBtn && dir != Vector2.zero)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
            hasFocusedBtn = true;
        }

        if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Calibrate()
    {
        NeuroManager.Instance.SimulateDevice = false;

        Debug.Log("Btn Calibrate clicked");
        SceneManager.LoadScene("Calibration");
    }

    public void ContinueWithoutNextmind()
    {
        NeuroManager.Instance.SimulateDevice = true;

        Debug.Log("Btn ContinueWithoutNextmind clicked");
        SceneManager.LoadScene("SDKDiscovery");
    }
}
