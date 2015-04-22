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

	public void Start()
	{
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
		if(Input.GetKey(KeyCode.D))
		{
			_normalizedHorizontalSpeed = 1F;
			if (!_isFacingRight)
				Flip();
		}
		else if(Input.GetKey(KeyCode.A))
		{
			_normalizedHorizontalSpeed = -1F;
			if (_isFacingRight)
				Flip();
		}
		else
		{
			_normalizedHorizontalSpeed = 0F;
		}

		if(_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
		{
			_controller.Jump();
		}
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
