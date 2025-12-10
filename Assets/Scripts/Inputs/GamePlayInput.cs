using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GamePlayInput
{
	private InputActions.GamePlayActions g;

	public bool Quit { get; private set; } = false;

	public float Steer { get; private set; } = 0;
	public float Throttle { get; private set; } = 0;
	public float Brake { get; private set; } = 0;

	public bool Spawn { get; private set; } = false;
	public bool SpawnOnStart { get; private set; } = false;

	public bool NextCam { get; private set; } = false;
	public bool PrevCam { get; private set; } = false;

	public Vector2 CamRotation { get; private set; }
	public Vector2 CamRotationMouse { get; private set; }


	public GamePlayInput(InputActions.GamePlayActions g)
	{
		this.g = g;

		g.Steer.performed += ctx => Steer = ctx.ReadValue<float>();
		g.Steer.canceled += _ => Steer = 0f;

		g.Throttle.performed += ctx => Throttle = ctx.ReadValue<float>();
		g.Throttle.canceled += _ => Throttle = 0f;

		g.Brake.performed += ctx => Brake = ctx.ReadValue<float>();
		g.Brake.canceled += _ => Brake = 0f;

		g.CameraRotate.performed += ctx => CamRotation = ctx.ReadValue<Vector2>();
		g.CameraRotate.canceled += _ => CamRotation = Vector2.zero;

		g.CameraRotateMouse.performed += ctx => CamRotationMouse = ctx.ReadValue<Vector2>();
		g.CameraRotateMouse.canceled += _ => CamRotationMouse = Vector2.zero;

		g.Quit.performed += _ => Quit = true;
		g.Quit.canceled += _ => Quit = false;

		g.NextCamera.performed += ctx =>
		{
			// if input comes from Keyboard, check shift
			if (ctx.control.device is Keyboard)
			{
				if (!Keyboard.current.shiftKey.isPressed)
					NextCam = true;
				else
					NextCam = false;
			}
			else
				NextCam = true;  // gamepad → allow always
		};
		g.NextCamera.canceled += _ => NextCam = false;

		g.PrevCamera.performed += _ => PrevCam = true;
		g.PrevCamera.canceled += _ => PrevCam = false;

		g.Respawn.performed += _ => 
		{
            Spawn = true;
            //Debug.Log("Spawn!");
        };
        g.Respawn.canceled += _ => Spawn = false;

		g.SpawnStart.performed += _ => 
		{
            SpawnOnStart = true;
            //Debug.Log("Spawn on start!");
        };
        g.SpawnStart.canceled += _ => SpawnOnStart = false;	
    }


    public bool ConsumeQuit()
    {
        bool value = Quit;
        Quit = false;
        return value;
    }

    public bool ConsumeSpawn()
    {
        bool value = Spawn;
        Spawn = false;
        return value;
    }

    public bool ConsumeSpawnOnStart()
    {
        bool value = SpawnOnStart;
        SpawnOnStart = false;
        return value;
    }


    public bool ConsumeNextCam()
    {
        bool value = NextCam;
        NextCam = false;
        return value;
    }

    public bool ConsumePrevCam()
    {
        bool value = PrevCam;
        PrevCam = false;
        return value;
    }
}
