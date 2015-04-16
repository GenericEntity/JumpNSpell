using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ControllerParameters2D
{
	public enum JumpBehaviour
	{
		CanJumpOnGround,
		CanJumpAnywhere,
		CantJump
	}

	public Vector2 maxVelocity = new Vector2(float.MaxValue, float.MaxValue);
	
	[Range(0, 90)]
	public float slopeLimit = 30F;
	
	public float gravity = -25F;

	public JumpBehaviour jumpRestrictions;

	public float jumpFrequency = 0.25F;

	public float jumpMagnitude = 12F;
}
