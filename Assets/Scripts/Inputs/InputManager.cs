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
}
