using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
	public static InputManager Instance { get; private set; }

	private InputActions input;

    public GamePlayInput GamePlay { get; private set; }
    public MenuInput Menu { get; private set; }

    /*public bool Quit { get; private set; }

	public float Steer { get; private set; } = 0;
	public float Throttle { get; private set; } = 0;
	public float Brake { get; private set; } = 0;

	public bool Spawn { get; private set; } = false;
	public bool SpawnOnStart { get; private set; } = false;

	public bool NextCam { get; private set; } = false;
	public bool PrevCam { get; private set; } = false;

	public Vector2 CamRotation {  get; private set; }
	public Vector2 CamRotationMouse { get; private set; }*/


	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); // only 1 InputManager
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		input = new InputActions();

        GamePlay = new GamePlayInput(input.GamePlay);
        Menu = new MenuInput(input.Menu);


        /*input.GamePlay.Steer.performed += ctx => Steer = ctx.ReadValue<float>();
		input.GamePlay.Steer.canceled += ctx => Steer = 0f;

		input.GamePlay.Throttle.performed += ctx => Throttle = ctx.ReadValue<float>();
		input.GamePlay.Throttle.canceled += ctx => Throttle = 0f;

		input.GamePlay.Brake.performed += ctx => Brake = ctx.ReadValue<float>();
		input.GamePlay.Brake.canceled += ctx => Brake = 0f;

		input.GamePlay.CameraRotate.performed += ctx => CamRotation = ctx.ReadValue<Vector2>();
		input.GamePlay.CameraRotate.canceled += ctx => CamRotation = Vector2.zero;

		input.GamePlay.CameraRotateMouse.performed += ctx => CamRotationMouse = ctx.ReadValue<Vector2>();
		input.GamePlay.CameraRotateMouse.canceled += ctx => CamRotationMouse = Vector2.zero;


		input.GamePlay.Quit.performed += _ => Quit = true;
		input.GamePlay.Quit.canceled += _ => Quit = false;

		input.GamePlay.NextCamera.performed += ctx =>
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
		input.GamePlay.NextCamera.canceled += _ => NextCam = false;

		input.GamePlay.PrevCamera.performed += _ => PrevCam = true;
		input.GamePlay.PrevCamera.canceled += _ => PrevCam = false;

		input.GamePlay.Respawn.performed += _ => Spawn = true;
		input.GamePlay.Respawn.canceled += _ => Spawn = false;

		input.GamePlay.SpawnStart.performed += _ => SpawnOnStart = true;
		input.GamePlay.SpawnStart.canceled += _ => SpawnOnStart = false;*/

        //input.GamePlay.Enable();
    }

    void Update()
    {
        Menu?.Update();
    }

    public void InputMap_GamePlay()
	{
		input.Disable();
		input.GamePlay.Enable();
        Debug.Log("InputMap-GamePlay");
    }

	public void InputMap_Menu()
	{
		input.Disable();
		input.Menu.Enable();
        Debug.Log("InputMap-Menu");
    }

	/*#region Consume_Pressed
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
	#endregion*/
}
