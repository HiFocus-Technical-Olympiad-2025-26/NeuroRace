using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInput
{
	private InputActions.MenuActions m;

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


    public MenuInput(InputActions.MenuActions m)
	{
		this.m = m;

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
}
