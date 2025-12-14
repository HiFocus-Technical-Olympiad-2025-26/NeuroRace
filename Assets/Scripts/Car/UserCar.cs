using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCar : Car
{
    public int StartPosition = 1;
    [SerializeField] private FloatEventChannelSO speedEvent;

    protected override void Start()
    {
        base.Start();

        InputManager.Instance.InputMap_GamePlay();

        spawner.SpawnCarOnSpecificStart(StartPosition);
    }

    void FixedUpdate()
    {
        var inputRaw = InputManager.Instance.GamePlay;

        //spawn
        if (inputRaw.Spawn)
            spawner.SpawnCarAtNearestPoint();
        if (inputRaw.SpawnOnStart)
            spawner.SpawnCarOnStart();


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
