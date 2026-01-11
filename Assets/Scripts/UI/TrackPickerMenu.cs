using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TrackPickerMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstUIObject;
    [SerializeField] private string BrnoCircuitSceneName = "BrnoGameScene";
    [SerializeField] private string BasicCircuitSceneName = "BasicGameScene";

    public void OpenBrnoCircuit()
    {
        SceneManager.LoadScene(BrnoCircuitSceneName);
    }
    public void OpenBasicCircuit()
    {
        SceneManager.LoadScene(BasicCircuitSceneName);
    }

    void Update()
    {
        Vector2 dir = InputManager.Instance.Menu.Direction;

        bool isSomethingSelected = EventSystem.current.currentSelectedGameObject != null;
        if (!isSomethingSelected && dir != Vector2.zero)
            EventSystem.current.SetSelectedGameObject(firstUIObject);

        if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f)
            EventSystem.current.SetSelectedGameObject(null);
    }
}
