using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameQuitter : MonoBehaviour
{
    [Header("ESC handling")]
    public List<string> scenesWithCustomESC = new List<string>();

    void Update()
    {
        if (IsESCHandledByLoadedScene())
            return;

        if (InputManager.Instance.GamePlay.ConsumeQuit())
            QuitGame();
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private bool IsESCHandledByLoadedScene()
    {
        foreach (string sceneName in scenesWithCustomESC)
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
                return true;

        return false;
    }
}