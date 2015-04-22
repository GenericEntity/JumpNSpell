using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private bool _isFacingRight;
	private CharacterController2D _controller;
	private float _normalizedHorizontalSpeed;

	public float MaxSpeed = 8F;
	public float SpeedAccelerationOnGround = 10F;
	public float SpeedAccelerationInAir = 5F;

	public bool CanMove { get; set; }

	public void Start()
	{
		CanMove = true;
		_controller = GetComponent<CharacterController2D>();
		_isFacingRight = transform.localScale.x > 0F;
	}

	public void Update()
	{
		HandleInput();

		var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
		_controller.SetHorizontalForce(Mathf.Lerp(
			_controller.Velocity.x, 
			_normalizedHorizontalSpeed * MaxSpeed, 
			Time.deltaTime * movementFactor));
	}

	private void HandleInput()
	{
		if (CanMove)
		{
			if (Input.GetButton("Right"))
			{
				_normalizedHorizontalSpeed = 1F;
				if (!_isFacingRight)
					Flip();
			}
			else if (Input.GetButton("Left"))
			{
				_normalizedHorizontalSpeed = -1F;
				if (_isFacingRight)
					Flip();
			}
			else
			{
				_normalizedHorizontalSpeed = 0F;
			}

			if (_controller.CanJump && Input.GetButtonDown("Jump"))
			{
				_controller.Jump();
			}
		}
		else
			_normalizedHorizontalSpeed = 0F;
	}

	private void Flip()
	{
		transform.localScale = new Vector3(
			-transform.localScale.x,
			transform.localScale.y,
			transform.localScale.z);

		_isFacingRight = transform.localScale.x > 0F;
	}	
}
