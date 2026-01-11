using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private string sceneName = "FirstMenu";
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float delayBeforeInFade = 1f;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float delay = 1.5f; // time for fade/logo
    [SerializeField] private float delayAfterOutFade = 0.5f;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(delayBeforeInFade);

        // Fade in
        yield return Fade(0f, 1f);

        yield return new WaitForSeconds(delay);

        // Fade out
        yield return Fade(1f, 0f);

        yield return new WaitForSeconds(delayAfterOutFade);

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        fadeGroup.alpha = to;
    }
}