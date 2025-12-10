using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AICar : Car
{
    [Header("AI")]
    [SerializeField] private AIController controller;

    public int StartPosition = 0;

    protected override void Start()
    {
        base.Start();

        spawner.SpawnCarOnSpecificStart(StartPosition);
    }

    void FixedUpdate()
    {
        this.ApplyPhysics(controller.carInput);
    }
}
