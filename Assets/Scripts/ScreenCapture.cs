using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenCapture : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private string fileName = "CameraCapture.png";
    [SerializeField] private float delaySeconds = 2f;

    void Start()
    {
        StartCoroutine(CaptureAfterDelay());
    }

    IEnumerator CaptureAfterDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        Capture();
    }

    void Capture()
    {
        int width = targetCamera.pixelWidth;
        int height = targetCamera.pixelHeight;

        RenderTexture rt = new RenderTexture(width, height, 24);
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        targetCamera.targetTexture = rt;
        targetCamera.Render();

        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = tex.EncodeToPNG();
        string fullPath = Path.Combine(Application.dataPath, fileName);
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log($"Saved screenshot {width}x{height} to: {fullPath}");
    }
}
