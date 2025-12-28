using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{
    [Header("UI Navigation")]
    public GameObject firstButton;

    [Header("UI Inputs")]
    [SerializeField] private TMP_Dropdown CarSettings;
    [SerializeField] private TMP_Dropdown StartPositionDropdown;
    [SerializeField] private Slider StartPositionSlider;
    [SerializeField] private TextMeshProUGUI StartPositionValueText;
    private StartPositionType startPositionType;
    [SerializeField] private Slider NumOfAIsSlider;
    [SerializeField] private TextMeshProUGUI NumOfAIsValueText;
    [SerializeField] private Toggle ShowNeuroObstacleToggle;

    [Header("Settings")]
    [SerializeField] private GameSettingsSO gameSettings;

    private bool hasFocusedBtn = false;

    public enum StartPositionType
    {
        Pole,
        Second,
        Last,
        Random,
        Specific
    }

    private void Awake()
    {
        StartPositionDropdown.ClearOptions();
        StartPositionDropdown.AddOptions(Enum.GetNames(typeof(StartPositionType)).ToList());
    }

    private void Start()
    {
        InputManager.Instance.InputMap_Menu();

        StartPositionDropdown.onValueChanged.AddListener(OnStartPositionDropdownChanged);
        StartPositionSlider.onValueChanged.AddListener(OnStartPositionSliderChanged);
        NumOfAIsSlider.onValueChanged.AddListener(OnNumOfAIsSliderChanged);

        UpdateNumOfAIsUI();
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

    public void BtnSubmit()
    {
        gameSettings.NumOfAIs = Mathf.RoundToInt(NumOfAIsSlider.value);
        gameSettings.StartPosition = Mathf.RoundToInt(StartPositionSlider.value);
        gameSettings.ShowNeuroObstacle = ShowNeuroObstacleToggle.isOn;

        Debug.Log("Game settings saved");
    }

    private void UpdateNumOfAIsUI()
    {
        int numAIs = Mathf.RoundToInt(NumOfAIsSlider.value);
        NumOfAIsValueText.text = numAIs.ToString();

        int maxStartPos = numAIs + 1;

        StartPositionSlider.minValue = 1;
        StartPositionSlider.maxValue = maxStartPos;

        if (StartPositionSlider.value > maxStartPos)
            StartPositionSlider.value = maxStartPos;

        UpdateStartPositionDropdownOptions(numAIs);
    }

    private void UpdateStartPositionDropdownOptions(int numAIs)
    {
        StartPositionDropdown.ClearOptions();

        List<string> options = new List<string>();
        options.Add("Pole");

        if (numAIs >= 1)
            options.Add("Second");

        if (numAIs >= 2)

        if (numAIs > 0)
        {
            options.Add("Last");
            options.Add("Random");
            options.Add("Specific");
        }

        StartPositionDropdown.AddOptions(options);

        StartPositionDropdown.value = 0;
        StartPositionDropdown.RefreshShownValue();

        OnStartPositionDropdownChanged(0);
    }

    public void OnStartPositionDropdownChanged(int value)
    {
        if (StartPositionDropdown.options.Count == 0) return;

        string option = StartPositionDropdown.options[value].text;

        bool allowAdvanced = Mathf.RoundToInt(NumOfAIsSlider.value) > 0;
        bool isSpecific = allowAdvanced && option == "Specific";

        StartPositionSlider.gameObject.SetActive(isSpecific);
        StartPositionValueText.gameObject.SetActive(isSpecific);

        gameSettings.RandomizeStartPosition = allowAdvanced && option == "Random";

        if (option == "Pole")
            StartPositionSlider.SetValueWithoutNotify(1);
        else if (option == "Second")
            StartPositionSlider.SetValueWithoutNotify(2);
        else if (option == "Last")
            StartPositionSlider.SetValueWithoutNotify((int)StartPositionSlider.maxValue);

        if (isSpecific)
            StartPositionValueText.text = Mathf.RoundToInt(StartPositionSlider.value).ToString();
    }

    public void OnStartPositionSliderChanged(float value)
    {
        Debug.Log("OnStartPositionSliderChanged");
        StartPositionValueText.text = Mathf.RoundToInt(value).ToString();
    }

    public void OnNumOfAIsSliderChanged(float value)
    {
        Debug.Log("OnNumOfAIsSliderChanged");
        UpdateNumOfAIsUI();
    }
}
