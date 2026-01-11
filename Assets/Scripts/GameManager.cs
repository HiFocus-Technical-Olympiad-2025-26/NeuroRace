using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Idle,
        Countdown,
        Running
    }

    [System.Serializable]
    public class SkyboxPreset
    {
        public Material skyboxMaterial;

        [Header("Directional Light Intensities")]
        public float directionalLight1Intensity = 1f;
        public float directionalLight2Intensity = 1f;
    }

    public GameState CurrentState { get; private set; } = GameState.Idle;

    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private SpawnSystem spawnSystem;
    [SerializeField] private LapTimer lapTimer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject aiPrefab;
    [SerializeField] private List<NeuroObstacleController> neuroObstacleControllers;

    [Header("Start lights")]
    [SerializeField] private StartLights startLights;
    [SerializeField] private float LightSequenceInterval = 1f;

    [Header("Skybox")]
    [SerializeField] private List<SkyboxPreset> SkyboxPresets;

    [Header("Directional Lights")]
    [SerializeField] private Light directionalLight1;
    [SerializeField] private Light directionalLight2;

    private List<GameObject> AIInstances = new List<GameObject>();

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ApplySkybox();
        ResetGame();
    }

    private void FixedUpdate()
    {
        if (InputManager.Instance.GamePlay.SpawnOnStart)
            ResetGame();
    }

    public void ResetGame()
    {
        if (player == null || aiPrefab == null)
            return;

        if(gameSettings == null ||
            gameSettings.StartPosition < 0 || 
            gameSettings.NumOfAIs < 0 || 
            gameSettings.StartPosition > gameSettings.NumOfAIs)
            SceneManager.LoadScene("NewGame");


        CurrentState = GameState.Idle;

        if(lapTimer != null)
            lapTimer.ResetTimer();

        if (AIInstances.Count > 0)
        {
            foreach (var ai in AIInstances)
            {
                Destroy(ai);
            }
            AIInstances.Clear();
        }

        var PlayerCar = player.GetComponent<Car>();

        PlayerCar.ApplyWheelParameters(gameSettings.playerWheelSetup);
        PlayerCar.ApplyCarSettings(gameSettings.playerCarSettings);

        int startPosition;
        if(gameSettings.RandomizeStartPosition)
            startPosition = Random.Range(0, gameSettings.NumOfAIs);
        else
            startPosition = gameSettings.StartPosition;

        spawnSystem.SpawnCarOnSpecificStart(player.transform, startPosition);
        PlayerCar.isThrottleEnabled = false;
        PlayerCar.spawner = spawnSystem;
        PlayerCar.SetSkin(gameSettings.skinIndex);

        int nextAIStartPosition = 0;
        for (int i = 0; i < gameSettings.NumOfAIs; i++)
        {
            var ai = Instantiate(aiPrefab);
            AIInstances.Add(ai);

            var aiCar = ai.GetComponent<Car>();
            aiCar.ApplyWheelParameters(gameSettings.AIWheelSetup);
            aiCar.ApplyCarSettings(gameSettings.AICarSettings);
            aiCar.spawner = spawnSystem;

            aiCar.isThrottleEnabled = false;

            if (nextAIStartPosition == startPosition)
                nextAIStartPosition++;
            spawnSystem.SpawnCarOnSpecificStart(ai.transform, nextAIStartPosition);
            nextAIStartPosition++;
        }

        foreach (var neuroObstacleController in neuroObstacleControllers)
        {
            if (neuroObstacleController != null)
            {
                if (gameSettings.ShowNeuroObstacle)
                {
                    neuroObstacleController.gameObject.SetActive(true);
                    neuroObstacleController.ResetObstacle();
                    neuroObstacleController.IgnoreAICollisions();
                }
                else
                    neuroObstacleController.gameObject.SetActive(false);
            }
            else
                Debug.LogError("neuroObstacleController is null", neuroObstacleController);
        }

        //startLights.TurnOnUpTo(3);
        StartCoroutine(StartLightSequence());
    }

    IEnumerator StartLightSequence()
    {
        CurrentState = GameState.Countdown;

        for (int i = 1; i <= startLights.segments.Length; i++)
        {
            startLights.TurnOnUpTo(i);
            yield return new WaitForSeconds(LightSequenceInterval);
        }

        startLights.TurnAllOff();

        EnableThrottle(true);

        CurrentState = GameState.Running;
    }

    void EnableThrottle(bool enable)
    {
        player.GetComponent<Car>().isThrottleEnabled = enable;
        foreach(var AI in AIInstances)
        {
            AI.GetComponent<Car>().isThrottleEnabled = enable;
        }
    }

    private void ApplySkybox()
    {
        if (SkyboxPresets == null || SkyboxPresets.Count == 0)
            return;

        int index = Mathf.Clamp(gameSettings.skyboxIndex, 0, SkyboxPresets.Count - 1);


        SkyboxPreset preset = SkyboxPresets[index];

        if (preset.skyboxMaterial != null)
            RenderSettings.skybox = preset.skyboxMaterial;

        if (directionalLight1 != null)
            directionalLight1.intensity = preset.directionalLight1Intensity;

        if (directionalLight2 != null)
            directionalLight2.intensity = preset.directionalLight2Intensity;

        DynamicGI.UpdateEnvironment();
    }
}