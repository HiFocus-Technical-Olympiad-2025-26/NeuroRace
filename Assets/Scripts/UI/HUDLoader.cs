using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDLoader : MonoBehaviour
{
    [SerializeField] private string hudSceneName = "HUD";
    [SerializeField] private string gameSceneName = "SDKDiscovery";

    void Start()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded &&
            !SceneManager.GetSceneByName(hudSceneName).isLoaded)
        {
            SceneManager.LoadScene(hudSceneName, LoadSceneMode.Additive);
        }
    }
}