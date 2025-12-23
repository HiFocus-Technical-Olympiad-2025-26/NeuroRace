using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCar : Car
{
    [SerializeField] private FloatEventChannelSO speedEvent;

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


        CarInput input = new CarInput
        {
            Throttle = inputRaw.Throttle,
            Brake = inputRaw.Brake,
            Steer = inputRaw.Steer,
            Spawn = inputRaw.Spawn,
            SpawnOnStart = inputRaw.SpawnOnStart
        };

        this.ApplyPhysics(input);

        float speedKmh = Mathf.Abs(rb.velocity.magnitude) * 3.6f;
        speedEvent?.RaiseEvent(speedKmh);
    }
}
