using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private bool isFacingRight;
	private CharacterController2D controller;
	private float normalizedHorizontalSpeed; // -1 if moving left, 1 if moving right

	public float maxSpeed = 8F; // maximum units/s that player can move
	// How quickly player's velocity changes
	public float speedAccelerationOnGround = 10F;
	public float speedAccelerationInAir = 5F;

	public void Start()
	{
		controller = GetComponent<CharacterController2D>();
		isFacingRight = transform.localScale.x > 0F; // transform.localScale will be negative if the character is flipped (facing left). Otherwise, it will be positive
	}

	public void Update()
	{
		// 1. Handle input
		HandleInput();

		// 2. Update controller's force
		var movementFactor = controller.State.IsGrounded ? speedAccelerationOnGround : speedAccelerationInAir;
		controller.SetHorizontalForce(Mathf.Lerp(
			controller.Velocity.x, 
			normalizedHorizontalSpeed * maxSpeed, 
			Time.deltaTime * movementFactor));

	}

	/// <summary>
	/// Sets normalizedHorizontalSpeed to 1, -1 or 0 based on user input.
	/// </summary>
	private void HandleInput()
	{
		if(Input.GetKey(KeyCode.D))
		{
			normalizedHorizontalSpeed = 1F;
			if (!isFacingRight)
				Flip();
		}
		else if (Input.GetKey(KeyCode.A))
		{
			normalizedHorizontalSpeed = -1F;
			if (isFacingRight)
				Flip();
		}
		else
		{
			normalizedHorizontalSpeed = 0F;
		}

		if(controller.CanJump && Input.GetKeyDown(KeyCode.Space))
		{
			controller.Jump();
		}
	}

	private void Flip()
	{
		transform.localScale = new Vector3(
			-transform.localScale.x, 
			transform.localScale.y, 
			transform.localScale.z);

		isFacingRight = transform.localScale.x > 0F;
	}
	
}
