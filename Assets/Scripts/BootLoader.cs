using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private float delay = 1f; // time for fade/logo
    [SerializeField] private string sceneName = "FirstMenu";

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}