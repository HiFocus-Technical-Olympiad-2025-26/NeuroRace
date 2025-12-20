using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private SpawnSystem spawnSystem;
    [SerializeField] private StartLights startLights;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject aiPrefab;
    [SerializeField] private float LightSequenceInterval = 1f;
    private List<GameObject> AIInstances = new List<GameObject>();

    private void Start()
    {
        ResetGame();
    }

    private void FixedUpdate()
    {
        if (InputManager.Instance.GamePlay.SpawnOnStart)
            ResetGame();
    }

    public void ResetGame()
    {
        if (player == null || aiPrefab == null || gameSettings == null || 
            gameSettings.StartPosition < 0 || gameSettings.NumOfAIs < 0 || gameSettings.StartPosition > gameSettings.NumOfAIs)
            return;

        if(AIInstances.Count > 0)
        {
            foreach (var ai in AIInstances)
            {
                Destroy(ai);
            }
            AIInstances.Clear();
        }

        var PlayerCar = player.GetComponent<Car>();

        PlayerCar.ApplyWheelParameters(gameSettings.playerWheelSetup);

        //PlayerCar.ApplyCarSettings(gameSettings.playerCarSettings);

        spawnSystem.SpawnCarOnSpecificStart(player.transform, gameSettings.StartPosition);
        PlayerCar.isThrottleEnabled = false;
        PlayerCar.spawner = spawnSystem;

        int nextAIStartPosition = 0;
        for (int i = 0; i < gameSettings.NumOfAIs; i++)
        {
            var ai = Instantiate(aiPrefab);
            AIInstances.Add(ai);

            var aiCar = ai.GetComponent<Car>();
            aiCar.ApplyWheelParameters(gameSettings.AIWheelSetup);
            // aiCar.ApplyCarSettings(gameSettings.AICarSettings);
            aiCar.spawner = spawnSystem;

            aiCar.isThrottleEnabled = false;

            if (nextAIStartPosition == gameSettings.StartPosition)
                nextAIStartPosition++;
            spawnSystem.SpawnCarOnSpecificStart(ai.transform, nextAIStartPosition);
            nextAIStartPosition++;
        }

        //startLights.TurnOnUpTo(3);
        StartCoroutine(StartLightSequence());
    }

    IEnumerator StartLightSequence()
    {
        for (int i = 1; i <= startLights.segments.Length; i++)
        {
            startLights.TurnOnUpTo(i);
            yield return new WaitForSeconds(LightSequenceInterval);
        }

        startLights.TurnAllOff();

        EnableThrottle(true);
    }

    void EnableThrottle(bool enable)
    {
        player.GetComponent<Car>().isThrottleEnabled = enable;
        foreach(var AI in AIInstances)
        {
            AI.GetComponent<Car>().isThrottleEnabled = enable;
        }
    }
}