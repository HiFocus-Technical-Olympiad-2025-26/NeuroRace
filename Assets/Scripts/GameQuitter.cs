using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    void Update()
    {
        if (InputManager.Instance.GamePlay.ConsumeQuit())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
