using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AICar : Car
{
    [Header("AI")]
    [SerializeField] private AIController controller;

    protected override void Start()
    {
        base.Start();

        if(skinSwitcher != null)
            skinSwitcher.SetRandomSkin();
    }

    void FixedUpdate()
    {
        this.ApplyPhysics(controller.carInput);
    }
}
