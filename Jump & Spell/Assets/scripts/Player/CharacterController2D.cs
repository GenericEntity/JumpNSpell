using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
	private const float SkinWidth = 0.02F;
	private const int TotalHorizontalRays = 8;
	private const int TotalVerticalRays = 4;

	private static readonly float SlopeLimitTangant = Mathf.Tan(75F * Mathf.Deg2Rad);

	public LayerMask platformMask;
	public ControllerParameters2D defaultParameters;

	public ControllerState2D State { get; private set; }
	public Vector2 Velocity { get { return _velocity; } }
	public bool CanJump 
	{
		get 
		{
			switch(Parameters.jumpRestrictions)
			{
				case ControllerParameters2D.JumpBehaviour.CanJumpAnywhere:
					return _jumpIn <= 0F;
				case ControllerParameters2D.JumpBehaviour.CanJumpOnGround:
					return State.IsGrounded && _jumpIn <= 0F;
				case ControllerParameters2D.JumpBehaviour.CantJump:
					return false;
				default: return false;
			}
		} 
	}
	public bool HandleCollisions { get; set; }
	public ControllerParameters2D Parameters { get { return _overrideParameters ?? defaultParameters; } }
	public GameObject StandingOn { get; private set; }

	// Component aliases / fields
	private Vector2 _velocity;
	private Transform _transform;
	private Vector3 _localScale;
	private BoxCollider2D _boxCollider;
	private ControllerParameters2D _overrideParameters;
	private float _jumpIn;
	private Vector3
		_raycastTopLeft,
		_raycastBottomRight,
		_raycastBottomLeft;

	private float 
		_verticalDistanceBetweenRays,
		_horizontalDistanceBetweenRays;

	public void Awake()
	{
		// 1. Alias out commonly-used components into individual fields
		_transform = transform;
		_localScale = transform.localScale;
		_boxCollider = GetComponent<BoxCollider2D>();

		// 2. Calculate vert and horiz distance between all rays by dividing box collider
		var colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
		_horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

		var _colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
		_verticalDistanceBetweenRays = _colliderHeight / (TotalHorizontalRays - 1);

		State = new ControllerState2D();
		HandleCollisions = true;
	}

	// These AddForce, SetForce, etc. methods are done for making the code neat and to hide the implementation so that future changes can happen.
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
		// TODO: Moving Platform support
		AddForce(new Vector2(0F, Parameters.jumpMagnitude));
		_jumpIn = Parameters.jumpFrequency; // Determines if it is time to let the player jump again
	}

	public void LateUpdate()
	{
		_jumpIn -= Time.deltaTime;

		_velocity.y += Parameters.gravity * Time.deltaTime;
		Move(Velocity * Time.deltaTime);
	}

	private void Move(Vector2 deltaMovement)
	{
		var wasGrounded = State.IsCollidingBelow;
		State.Reset();

		if(HandleCollisions)
		{
			HandlePlatforms();
			CalculateRayOrigins();

			if (deltaMovement.y < 0F && wasGrounded)
				HandleVerticalSlope(ref deltaMovement);

			if (Mathf.Abs(deltaMovement.x) > 0.001F)
				MoveHorizontally(ref deltaMovement);

			// Will always have vertical force (like gravity) so no if
			MoveVertically(ref deltaMovement);
		}

		_transform.Translate(deltaMovement, Space.World);

		// TODO: Additional moving platform code

		if (Time.deltaTime > 0F)
			_velocity = deltaMovement / Time.deltaTime;

		_velocity.x = Mathf.Min(_velocity.x, Parameters.maxVelocity.x);
		_velocity.y = Mathf.Min(_velocity.y, Parameters.maxVelocity.y);

		if (State.IsMovingUpSlope)
			_velocity.y = 0F;
	}

	private void HandlePlatforms()
	{

	}

	private void CalculateRayOrigins()
	{
		var size = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
		var center = new Vector2(_boxCollider.offset.x * _localScale.x, _boxCollider.offset.y * _localScale.y);

		_raycastTopLeft = _transform.position + 
			new Vector3(
			center.x - size.x + SkinWidth, 
			center.y + size.y - SkinWidth);

		_raycastBottomRight = _transform.position +
			new Vector3(
				center.x + size.x - SkinWidth,
				center.y - size.y + SkinWidth);

		_raycastBottomLeft = _transform.position +
			new Vector3(
				center.x - size.x + SkinWidth,
				center.y - size.y + SkinWidth);
	}

	/// <summary>
	/// Cast rays in horizontal direction we are moving in and constrain movement accordingly.
	/// </summary>
	/// <param name="deltaMovement"></param>
	private void MoveHorizontally(ref Vector2 deltaMovement)
	{
		var isGoingRight = deltaMovement.x > 0F;
		var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
		var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
		var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

		for(int i = 0; i < TotalHorizontalRays; ++i)
		{
			var rayVector = new Vector2(
				rayOrigin.x, 
				rayOrigin.y + (i * _verticalDistanceBetweenRays));
			Debug.DrawRay(
				rayVector, 
				rayDirection * rayDistance, 
				Color.red);

			var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask);
			if (!rayCastHit)
				continue;

			if (i == 0 && HandleHorizontalSlope(
				ref deltaMovement, 
				Vector2.Angle(rayCastHit.normal, Vector2.up), 
				isGoingRight))
				break;

			deltaMovement.x = rayCastHit.point.x - rayVector.x; // rayCastHit.point.x - rayVector.x is the furthest distance the player can move without hitting an obstacle.
			rayDistance = Mathf.Abs(deltaMovement.x);

			if(isGoingRight)
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

		rayOrigin.x += deltaMovement.x; // horizontal movement is already handled

		var standingOnDistance = float.MaxValue;
		for(int i = 0; i < TotalVerticalRays; ++i)
		{
			var rayVector = new Vector2(
				rayOrigin.x + (i * _horizontalDistanceBetweenRays), 
				rayOrigin.y);
			Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

			var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, platformMask);
			if (!raycastHit)
				continue;

			// Keep track of what we're standing on
			if(!isGoingUp)
			{
				var verticalDistanceToHit = _transform.position.y - raycastHit.point.y;
				if(verticalDistanceToHit < standingOnDistance)
				{
					standingOnDistance = verticalDistanceToHit; // Find shortest down raycast
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

			if(!isGoingUp && deltaMovement.y > 0.0001F)
				State.IsMovingUpSlope = true;

			if (rayDistance < SkinWidth + 0.0001F)
				break;
		}
	}

	private void HandleVerticalSlope(ref Vector2 deltaMovement)
	{
		var center = (_raycastBottomLeft.x + _raycastBottomRight.x) / 2F;
		var direction = -Vector2.up; // down

		var slopeDistance = SlopeLimitTangant * (_raycastBottomRight.x - center);
		var slopeRayVector = new Vector2(center, _raycastBottomLeft.y);

		Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.green);

	}

	private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
	{
		return false;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{

	}

	public void OnTriggerExit2D(Collider2D other)
	{

	}
}
