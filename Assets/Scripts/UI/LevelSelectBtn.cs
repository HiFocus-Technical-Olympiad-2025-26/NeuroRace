using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private string gameSceneName = "SDKDiscovery";

    public void OnClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}