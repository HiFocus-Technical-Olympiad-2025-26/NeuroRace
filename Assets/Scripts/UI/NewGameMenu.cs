using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{
    [Header("UI Navigation")]
    public GameObject firstUIObject;
    [SerializeField] string GameSceneName = "GameScene";

    [Header("UI Inputs")]
    [SerializeField] private TMP_Dropdown CarSettings;
    [SerializeField] private TMP_Dropdown SkyboxDropdown;
    [SerializeField] private TMP_Dropdown StartPositionDropdown;
    [SerializeField] private Slider StartPositionSlider;
    [SerializeField] private TextMeshProUGUI StartPositionValueText;
    [SerializeField] private Slider NumOfAIsSlider;
    [SerializeField] private TextMeshProUGUI NumOfAIsValueText;
    [SerializeField] private Toggle ShowNeuroObstacleToggle;
    [SerializeField] private SkinSwitcher skinSwitcher;

    [Header("Settings")]
    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private List<LevelConfig> levelConfigs;

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

        if(levelConfigs != null && levelConfigs.Count > 0)
        {
            CarSettings.ClearOptions();
            CarSettings.AddOptions(levelConfigs.Select(lc => lc.levelName).ToList());
        }
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
        var input = InputManager.Instance.Menu;

        Vector2 dir = input.Direction;
        bool isSomethingSelected = EventSystem.current.currentSelectedGameObject != null;
        if (!isSomethingSelected && dir != Vector2.zero)
            EventSystem.current.SetSelectedGameObject(firstUIObject);

        if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f)
            EventSystem.current.SetSelectedGameObject(null);


        if (input.ConsumeGamepadLeftShoulder())
            skinSwitcher.NextSkin();

        if (input.ConsumeGamepadRightShoulder())
            skinSwitcher.PreviousSkin();
    }

    public void BtnNewGame()
    {
        StartPositionType startPositionType = (StartPositionType)Enum.Parse(typeof(StartPositionType), StartPositionDropdown.options[StartPositionDropdown.value].text);

        gameSettings.NumOfAIs = Mathf.RoundToInt(NumOfAIsSlider.value);
        int startPos;
        switch(startPositionType)
        {
            case StartPositionType.Pole:
                startPos = 0;
                break;
            case StartPositionType.Second:
                startPos = 1;
                break;
            case StartPositionType.Last:
                startPos = gameSettings.NumOfAIs;
                break;
            case StartPositionType.Random:
                startPos = 0;
                break;
            case StartPositionType.Specific:
                startPos = Mathf.RoundToInt(StartPositionSlider.value) - 1;
                break;
            default:
                startPos = 0;
                break;
        }
        gameSettings.StartPosition = startPos;
        gameSettings.RandomizeStartPosition = startPositionType == StartPositionType.Random;
        gameSettings.ShowNeuroObstacle = ShowNeuroObstacleToggle.isOn;
        gameSettings.skinIndex = skinSwitcher.GetSkinIndex();

        LevelConfig selectedLC = levelConfigs[CarSettings.value];
        gameSettings.playerCarSettings = selectedLC.playerCarSettings;
        gameSettings.playerWheelSetup = selectedLC.playerWheelSetup;
        gameSettings.AICarSettings = selectedLC.AICarSettings;
        gameSettings.AIWheelSetup = selectedLC.AIWheelSetup;

        gameSettings.skyboxIndex = SkyboxDropdown.value;


        SceneManager.LoadScene(GameSceneName);
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

        /*StartPositionSlider.gameObject.SetActive(isSpecific);
        StartPositionValueText.gameObject.SetActive(isSpecific);*/
        StartPositionSlider.interactable = isSpecific;
        //StartPositionSlider.gameObject.GetComponent<CanvasGroup>().alpha = isSpecific ? 1f : 0.2f;

        gameSettings.RandomizeStartPosition = allowAdvanced && option == "Random";

        if (option == "Pole")
            StartPositionSlider.value = 1;
        else if (option == "Second")
            StartPositionSlider.value = 2;
        else if (option == "Last")
            StartPositionSlider.value = (int)StartPositionSlider.maxValue;

        StartPositionValueText.text = Mathf.RoundToInt(StartPositionSlider.value).ToString();
    }

    public void OnStartPositionSliderChanged(float value)
    {
        StartPositionValueText.text = Mathf.RoundToInt(value).ToString();
    }

    public void OnNumOfAIsSliderChanged(float value)
    {
        UpdateNumOfAIsUI();
    }
}
