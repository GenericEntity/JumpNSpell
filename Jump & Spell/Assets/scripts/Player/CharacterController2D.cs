﻿using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
	private const float SkinWidth = 0.02F;
	private const int TotalHorizontalRays = 8;
	private const int TotalVerticalRays = 5;

	private static readonly float SlopeLimitTangent = Mathf.Tan(75F * Mathf.Deg2Rad);

	public LayerMask PlatformMask;
	public ControllerParameters2D DefaultParameters;

	public ControllerState2D State { get; private set; }
	public Vector2 Velocity { get { return _velocity; } }
	public bool CanJump
	{
		get
		{
			if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehavior.CanJumpAnywhere)
				return _jumpIn <= 0F;
			else if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehavior.CanJumpOnGround)
				return State.IsGrounded;
			else
				return false;
		}
	}
	public bool HandleCollisions { get; set; }
	public ControllerParameters2D Parameters
	{
		get { return _overrideParameters ?? DefaultParameters; }
	}
	public GameObject StandingOn { get; private set; }
	public Vector3 PlatformVelocity { get; private set; }

	private Vector2 _velocity;
	private Transform _transform;
	private Vector3 _localScale;
	private BoxCollider2D _boxCollider;
	private ControllerParameters2D _overrideParameters;
	private float _jumpIn;
	private GameObject _lastStandingOn;

	private Vector3
		_activeGlobalPlatformPoint,
		_activeLocalPlatformPoint;

	private Vector3
		_raycastTopLeft,
		_raycastBottomLeft,
		_raycastBottomRight;

	private float
		_verticalDistanceBetweenRays,
		_horizontalDistanceBetweenRays;

	public void Awake()
	{
		HandleCollisions = true;
		State = new ControllerState2D();
		_transform = transform;
		_localScale = transform.localScale;
		_boxCollider = GetComponent<BoxCollider2D>();

		var colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
		_horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

		var colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
		_verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);
	}

	public void AddForce(Vector2 force)
	{
		_velocity += force;
	}

	public void SetForce(Vector2 force)
	{
		_velocity = force;
	}

	public void SetHorizontalForce(float x)
	{
		_velocity.x = x;
	}

	public void SetVerticalForce(float y)
	{
		_velocity.y = y;
	}

	public void Jump()
	{
		AddForce(new Vector2(0F, Parameters.JumpMagnitude));
		_jumpIn = Parameters.JumpFrequency;
	}

	public void LateUpdate()
	{
		_jumpIn -= Time.deltaTime;

		_velocity.y += Parameters.Gravity * Time.deltaTime;

		Move(Velocity * Time.deltaTime);
	}

	private void Move(Vector2 deltaMovement)
	{
		var wasGrounded = State.IsCollidingBelow;
		State.Reset();

		if (HandleCollisions)
		{
			HandlePlatforms();
			CalculateRayOrigins();

			// If moving down
			if (deltaMovement.y < 0F && wasGrounded)
				HandleVerticalSlope(ref deltaMovement);

			if (Mathf.Abs(deltaMovement.x) > 0.001F)
				MoveHorizontally(ref deltaMovement);

			MoveVertically(ref deltaMovement);

			CorrectHorizontalPlacement(ref deltaMovement, true); // Correct right
			CorrectHorizontalPlacement(ref deltaMovement, false); // Correct left
		}

		_transform.Translate(deltaMovement, Space.World);

		if (Time.deltaTime > 0F)
			_velocity = deltaMovement / Time.deltaTime;

		_velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
		_velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);

		if (State.IsMovingUpSlope)
			_velocity.y = 0F;

		if(StandingOn != null)
		{
			_activeGlobalPlatformPoint = transform.position;
			_activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);

			if (_lastStandingOn != StandingOn)
			{
				if (_lastStandingOn != null)
					_lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);

				StandingOn.SendMessage("ControllerEnter2D", this, SendMessageOptions.DontRequireReceiver);
				_lastStandingOn = StandingOn;
			}
			else if (StandingOn != null)
				StandingOn.SendMessage("ControllerStay2D", this, SendMessageOptions.DontRequireReceiver);
		}
		else if(_lastStandingOn != null)
		{
			_lastStandingOn.SendMessage("ControllerExit2D", this, SendMessageOptions.DontRequireReceiver);
			_lastStandingOn = null;
		}
	}

	private void HandlePlatforms()
	{
		if (StandingOn != null)
		{
			var newGlobalPlatformPoint = StandingOn.transform.TransformPoint(_activeLocalPlatformPoint);
			var moveDistance = newGlobalPlatformPoint - _activeGlobalPlatformPoint;

			if (moveDistance != Vector3.zero)
				transform.Translate(moveDistance, Space.World);

			PlatformVelocity = (newGlobalPlatformPoint - _activeGlobalPlatformPoint) / Time.deltaTime;
		}
		else
			PlatformVelocity = Vector3.zero;

		// Reset StandingOn from last frame.
		StandingOn = null;
	}

	private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
	{
		// Half of width of player
		var halfWidth = (_boxCollider.size.x * _localScale.x) / 2F;
		var rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;

		if (isRight)
			rayOrigin.x -= (halfWidth - SkinWidth);
		else
			rayOrigin.x += (halfWidth - SkinWidth);

		var rayDirection = isRight ? Vector2.right : -Vector2.right;
		var offset = 0F;

		for(int i = 1; i < TotalHorizontalRays - 1; ++i)
		{
			var rayVector = new Vector2(deltaMovement.x + rayOrigin.x, deltaMovement.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));
			Debug.DrawRay(rayVector, rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);

			var raycastHit = Physics2D.Raycast(rayVector, rayDirection, halfWidth, PlatformMask);
			if (!raycastHit)
				continue;

			offset = isRight ? 
				((raycastHit.point.x - _transform.position.x) - halfWidth) : 
				(halfWidth - (_transform.position.x - raycastHit.point.x));
		}

		deltaMovement.x += offset;
	}

	private void CalculateRayOrigins()
	{
		var size = new Vector2(
			_boxCollider.size.x * Mathf.Abs(_localScale.x),
			_boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
		var center = new Vector2(
			_boxCollider.offset.x * _localScale.x,
			_boxCollider.offset.y * _localScale.y);

		_raycastTopLeft = _transform.position + new Vector3(
			center.x - size.x + SkinWidth,
			center.y + size.y - SkinWidth);

		_raycastBottomLeft = _transform.position + new Vector3(
			center.x - size.x + SkinWidth,
			center.y - size.y + SkinWidth);

		_raycastBottomRight = _transform.position + new Vector3(
			center.x + size.x - SkinWidth,
			center.y - size.y + SkinWidth);
	}

	private void MoveHorizontally(ref Vector2 deltaMovement)
	{
		var isGoingRight = deltaMovement.x > 0F;
		var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
		var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
		var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

		for (int i = 0; i < TotalHorizontalRays; ++i)
		{
			var rayVector = new Vector2(
				rayOrigin.x,
				rayOrigin.y + (i * _verticalDistanceBetweenRays));
			Debug.DrawRay(
				rayVector,
				rayDirection * rayDistance,
				Color.red);

			var raycastHit = Physics2D.Raycast(
				rayVector,
				rayDirection,
				rayDistance,
				PlatformMask);
			if (!raycastHit)
				continue;

			if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), isGoingRight))
				break;

			deltaMovement.x = raycastHit.point.x - rayVector.x;
			rayDistance = Mathf.Abs(deltaMovement.x);

			if (isGoingRight)
			{
				deltaMovement.x -= SkinWidth;
				State.IsCollidingRight = true;
			}
			else
			{
				deltaMovement.x += SkinWidth;
				State.IsCollidingLeft = true;
			}

			if (rayDistance < SkinWidth + 0.0001F)
				break;
		}

	}

	private void MoveVertically(ref Vector2 deltaMovement)
	{
		var isGoingUp = deltaMovement.y > 0F;
		var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
		var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
		var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;

		rayOrigin.x += deltaMovement.x;

		var standingOnDistance = float.MaxValue;
		for (int i = 0; i < TotalVerticalRays; ++i)
		{
			var rayVector = new Vector2(
				rayOrigin.x + (i * _horizontalDistanceBetweenRays),
				rayOrigin.y);

			Debug.DrawRay(
				rayVector, 
				rayDirection * rayDistance, 
				Color.green);

			var raycastHit = Physics2D.Raycast(
				rayVector,
				rayDirection,
				rayDistance, 
				PlatformMask);

			if (!raycastHit)
				continue;

			if(!isGoingUp)
			{
				var verticalDistanceToHit = _transform.position.y - raycastHit.point.y;
				if(verticalDistanceToHit < standingOnDistance)
				{
					standingOnDistance = verticalDistanceToHit;
					StandingOn = raycastHit.collider.gameObject;
				}
			}

			deltaMovement.y = raycastHit.point.y - rayVector.y;
			rayDistance = Mathf.Abs(deltaMovement.y);

			if(isGoingUp)
			{
				deltaMovement.y -= SkinWidth;
				State.IsCollidingAbove = true;
			}
			else
			{
				deltaMovement.y += SkinWidth;
				State.IsCollidingBelow = true;
			}

			if (!isGoingUp && deltaMovement.y > 0.0001F)
				State.IsMovingUpSlope = true;

			if (rayDistance < SkinWidth + 0.0001F)
				break;
		}
	}

	private void HandleVerticalSlope(ref Vector2 deltaMovement)
	{
		var center = (_raycastBottomLeft.x + _raycastBottomRight.x) / 2F;
		var direction = -Vector2.up;

		var slopeDistance = SlopeLimitTangent * (_raycastBottomRight.x - center);
		var slopeRayVector = new Vector2(center, _raycastBottomLeft.y);

		Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.yellow);

		var raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PlatformMask);
		if (!raycastHit)
			return;

		var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x);
		if (!isMovingDownSlope)
			return;

		var angle = Vector2.Angle(raycastHit.normal, Vector2.up);
		if (Mathf.Abs(angle) < 0.0001F)
			return;

		State.IsMovingDownSlope = true;
		State.SlopeAngle = angle;
		deltaMovement.y = raycastHit.point.y - slopeRayVector.y;
	}

	private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
	{
		if (Mathf.RoundToInt(angle) == 90)
			return false;

		if(angle > Parameters.SlopeLimit)
		{
			deltaMovement.x = 0F;
			return true;
		}

		if (deltaMovement.y > 0.07F)
			return true;

		deltaMovement.x += isGoingRight ? -SkinWidth : SkinWidth;
		deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);
		State.IsMovingUpSlope = true;
		State.IsCollidingBelow = true;
		return true;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		var parameters = other.gameObject.GetComponent<ControllerPhysicsVolume2D>();
		if (parameters == null)
			return;

		_overrideParameters = parameters.Parameters;
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		var parameters = other.gameObject.GetComponent<ControllerPhysicsVolume2D>();
		if (parameters == null)
			return;

		_overrideParameters = null;
	}
}
