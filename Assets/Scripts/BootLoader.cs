using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private float delay = 1f; // time for fade/logo

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadFirstMenu());
    }

    private IEnumerator LoadFirstMenu()
    {
        yield return new WaitForSeconds(delay);
        //SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene("FirstMenu");
    }
}