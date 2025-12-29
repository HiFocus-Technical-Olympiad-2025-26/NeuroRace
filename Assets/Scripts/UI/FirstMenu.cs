using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using NextMind;

public class FirstMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject firstButton;

    [Header("NeuroManager Prefabs")]
    [SerializeField] private GameObject neuroManagerRealPrefab;
    [SerializeField] private GameObject neuroManagerSimulatedPrefab;

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
        //Debug.Log("Btn Calibrate clicked");

        SpawnNeuroManager(neuroManagerRealPrefab);

        SceneManager.LoadScene("Calibration");
    }

    public void ContinueWithoutNextmind()
    {
        //Debug.Log("Btn ContinueWithoutNextmind clicked");

        SpawnNeuroManager(neuroManagerSimulatedPrefab);

        //SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene("NewGame");
    }

    private void SpawnNeuroManager(GameObject prefab)
    {
        if (FindObjectOfType<NeuroManager>() != null)
        {
            Debug.LogWarning("NeuroManager already exists, skipping spawn");
            return;
        }

        GameObject nm = Instantiate(prefab);
        DontDestroyOnLoad(nm);
    }
}
