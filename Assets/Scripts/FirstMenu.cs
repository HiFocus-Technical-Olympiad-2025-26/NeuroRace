using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstMenu : MonoBehaviour
{
    public void Calibrate()
    {
        Debug.Log("Btn Calibrate clicked");
        SceneManager.LoadScene("Calibration");
    }

    public void ContinueWithoutNextmind()
    {
        Debug.Log("Btn ContinueWithoutNextmind clicked");
        SceneManager.LoadScene("SDKDiscovery");
    }
}
