using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedometrUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private RectTransform Needle;
    [SerializeField] private float NeedleAngleMinSpeed = 225f;
    [SerializeField] private float NeedleAngleMaxSpeed = -45f;
    [SerializeField] private float MaxSpeed = 360f;

    [SerializeField] private FloatEventChannelSO speedEvent;

    private float currentSpeed;

    private void OnEnable()
    {
        speedEvent.OnEventRaised += OnSpeedChanged;
    }

    private void OnDisable()
    {
        speedEvent.OnEventRaised -= OnSpeedChanged;
    }

    private void OnSpeedChanged(float speed)
    {
        currentSpeed = speed;
    }

    void Update()
    {
        Text.text = ((int)currentSpeed).ToString() + " km/h";

        float t = Mathf.Clamp01(currentSpeed / MaxSpeed);
        float angle = Mathf.Lerp(NeedleAngleMinSpeed, NeedleAngleMaxSpeed, t);
        Needle.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
