using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInput
{
	private InputActions.MenuActions m;

    public bool NextTheme { get; private set; } = false;

    public bool Confirm { get; private set; } = false;
	public bool Back { get; private set; } = false;

    public Vector2 RawDirection { get; private set; }
    public Vector2 Direction { get; private set; }

    public float repeatDelay = 0.3f;
    public float repeatRate = 0.1f;
    private float nextRepeatTime;

    public bool RepeatedDirectionDown { get; private set; }
    public bool RepeatedDirectionUp { get; private set; }
    public bool RepeatedDirectionLeft { get; private set; }
    public bool RepeatedDirectionRight { get; private set; }

    public bool GamepadLeftShoulder { get; private set; } = false;
    public bool GamepadRightShoulder { get; private set; } = false;


    public MenuInput(InputActions.MenuActions m)
	{
		this.m = m;

        m.NextTheme.performed += _ => NextTheme = true;
        m.NextTheme.canceled += _ => NextTheme = false;

        m.Direction.performed += ctx =>
        {
            RawDirection = ctx.ReadValue<Vector2>();
            Direction = NormalizeDirection(RawDirection);

            nextRepeatTime = Time.time + repeatDelay;

            RepeatedDirectionDown = (Direction.y < 0);
            RepeatedDirectionUp = (Direction.y > 0);
            RepeatedDirectionLeft = (Direction.x < 0);
            RepeatedDirectionRight = (Direction.x > 0);
        };
        m.Direction.canceled += _ =>
        {
            RawDirection = Vector2.zero;
            Direction = Vector2.zero;
        };

        m.Confirm.performed += _ => Confirm = true;
		m.Confirm.canceled += _ => Confirm = false;

		m.Back.performed += _ => Back = true;
		m.Back.canceled += _ => Back = false;

        m.Gamepad_LeftShoulder.performed += _ => GamepadLeftShoulder = true;
        m.Gamepad_LeftShoulder.canceled += _ => GamepadLeftShoulder = false;

        m.Gamepad_RightShoulder.performed += _ => GamepadRightShoulder = true;
        m.Gamepad_RightShoulder.canceled += _ => GamepadRightShoulder = false;
    }

    public void Update()
    {
        if (Direction == Vector2.zero || Time.time < nextRepeatTime) 
            return;

        nextRepeatTime = Time.time + repeatRate;

        RepeatedDirectionDown = (Direction.y < 0);
        RepeatedDirectionUp = (Direction.y > 0);
        RepeatedDirectionLeft = (Direction.x < 0);
        RepeatedDirectionRight = (Direction.x > 0);
    }

    private Vector2 NormalizeDirection(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            return new Vector2(Mathf.Sign(v.x), 0);

        return new Vector2(0, Mathf.Sign(v.y));
    }

    public bool ConsumeNextTheme()
    {
        bool value = NextTheme;
        NextTheme = false;
        return value;
    }

    public bool ConsumeGamepadLeftShoulder()
    {
        bool value = GamepadLeftShoulder;
        GamepadLeftShoulder = false;
        return value;
    }

    public bool ConsumeGamepadRightShoulder()
    {
        bool value = GamepadRightShoulder;
        GamepadRightShoulder = false;
        return value;
    }
}
