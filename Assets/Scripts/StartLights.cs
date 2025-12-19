using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLights : MonoBehaviour
{
    [SerializeField] public Renderer[] segments;
    [SerializeField] private float emissionIntensityOn = 3f;
    [SerializeField] private float emissionIntensityOff = 0.3f;

    private MaterialPropertyBlock block;

    private void Start()
    {
        block = new MaterialPropertyBlock();
        TurnAllOff();
    }

    void SetSegment(int index, bool on)
    {
        if (index < 0 || index >= segments.Length)
            return;

        if (segments[index] == null)
        {
            Debug.LogWarning("Renderer at index " + index + " is null!");
            return;
        }

        segments[index].GetPropertyBlock(block);

        float intensity = on ? emissionIntensityOn : emissionIntensityOff;
        Color color = Color.red * intensity;

        block.SetColor("_EmissionColor", color);

        segments[index].SetPropertyBlock(block);
    }

    public void TurnAllOff()
    {
        for (int i = 0; i < segments.Length; i++)
            SetSegment(i, false);
    }

    public void TurnOnUpTo(int count)
    {
        for (int i = 0; i < segments.Length; i++)
            SetSegment(i, i < count);
    }
}
