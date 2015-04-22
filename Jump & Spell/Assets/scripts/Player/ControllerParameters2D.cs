using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ControllerParameters2D
{
	public enum JumpBehavior
	{
		CanJumpOnGround,
		CanJumpAnywhere,
		CantJump
	}

	public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);

	[Range(0, 90)]
	public float SlopeLimit = 30F;

	public float Gravity = -40F;

	public JumpBehavior JumpRestrictions;

	public float JumpFrequency = 0.25F;

	public float JumpMagnitude = 16F;
}
