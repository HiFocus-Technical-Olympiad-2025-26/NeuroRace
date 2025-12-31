using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCar : Car
{
    [Header("Speed event")]
    [SerializeField] private FloatEventChannelSO speedEvent;

    [Header("Wrong Direction Detection")]
    [SerializeField] private BoolEventChannelSO wrongDirectionEvent;
    [SerializeField] private float wrongDirCheckInterval = 0.1f;
    [SerializeField] private float wrongDirDotThreshold = 0f;
    [SerializeField] private float wrongDirMinTime = 2.0f;

    private float timer;
    private float wrongDirTimer = 0f;
    private bool isWrongDirActive = false;

    protected override void Start()
    {
        base.Start();

        InputManager.Instance.InputMap_GamePlay();
    }

    void FixedUpdate()
    {
        var inputRaw = InputManager.Instance.GamePlay;

        //spawn
        if (inputRaw.Spawn)
            spawner.SpawnCarAtNearestPoint(this.transform);

        //skin
        if (inputRaw.ConsumeSkin() && skinSwitcher != null)
            skinSwitcher.NextSkin();

        //input
        CarInput input = new CarInput
        {
            Throttle = inputRaw.Throttle,
            Brake = inputRaw.Brake,
            Steer = inputRaw.Steer,
            Spawn = inputRaw.Spawn,
            SpawnOnStart = inputRaw.SpawnOnStart
        };

        // physics
        this.ApplyPhysics(input);

        // speed event
        float speedKmh = Mathf.Abs(rb.velocity.magnitude) * 3.6f;
        speedEvent?.RaiseEvent(speedKmh);

        // wrong direction detection
        timer += Time.deltaTime;
        if (timer >= wrongDirCheckInterval)
        {
            timer = 0f;

            bool isWrongNow = spawner.IsCarGoingWrongDirection(transform, wrongDirDotThreshold);

            if (isWrongNow)
            {
                wrongDirTimer += wrongDirCheckInterval;

                if (!isWrongDirActive && wrongDirTimer >= wrongDirMinTime)
                {
                    isWrongDirActive = true;
                    wrongDirectionEvent.RaiseEvent(true);
                }
            }
            else
            {
                wrongDirTimer = 0f;

                if (isWrongDirActive)
                {
                    isWrongDirActive = false;
                    wrongDirectionEvent.RaiseEvent(false);
                }
            }
        }
    }
}
