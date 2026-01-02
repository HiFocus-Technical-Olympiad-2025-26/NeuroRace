using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject menuRoot;

    [Header("Scenes")]
    [SerializeField] private string NewGameSceneName = "NewGame";


    private bool isPaused = false;

    private void Awake()
    {
        if (menuRoot == null)
        {
            Debug.LogError("Menu root is not assigned!", this);
            enabled = false;
            return;
        }

        menuRoot.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if (InputManager.Instance.GamePlay.ConsumeQuit())
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
            return;

        isPaused = true;

        Time.timeScale = 0f;

        menuRoot.SetActive(true);

        InputManager.Instance.InputMap_Menu();
    }

    public void ResumeGame()
    {
        if (!isPaused)
            return;

        isPaused = false;

        Time.timeScale = 1f;

        menuRoot.SetActive(false);

        InputManager.Instance.InputMap_GamePlay();
    }

    public void OnRestartRacePressed()
    {
        ResumeGame();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnNewGamePressed()
    {
        ResumeGame();
        if (!string.IsNullOrEmpty(NewGameSceneName))
            SceneManager.LoadScene(NewGameSceneName);
        else
            Debug.LogError("New game scene name is not set!");
    }

    public void OnQuitPressed()
    {
        GameQuitter.QuitGame();
    }
}