using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AICar : Car
{
    [Header("AI")]
    [SerializeField] private AIController controller;

    void FixedUpdate()
    {
        this.ApplyPhysics(controller.carInput);
    }
}
