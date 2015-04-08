using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public Transform player;
	public Vector2 
		margin,
		smoothing;
	public BoxCollider2D bounds;
	private Vector3
		_min,
		_max;

	private Camera thisCam;

	private float currOrthSize;
	private float zoomSpeed;
	private float newOrthSize;
	private Vector3 newPosition;
	private float moveSpeed;

	public bool IsZooming { get; set; }

	public bool IsBounded { get; set; }

	public bool IsFollowing { get; set; }

	void Awake()
	{
		thisCam = GetComponent<Camera>();
	}

	void Start()
	{
		_min = bounds.bounds.min;
		_max = bounds.bounds.max;
	}

	void Update()
	{
		currOrthSize = thisCam.orthographicSize;

		var x = transform.position.x;
		var y = transform.position.y;

		if (IsFollowing)
		{
			if (Mathf.Abs(x - player.position.x) > margin.x)
				x = Mathf.Lerp(
					x, 
					player.position.x, 
					smoothing.x * Time.deltaTime);

			if (Mathf.Abs(y - player.position.y) > margin.y)
				y = Mathf.Lerp(
					y, 
					player.position.y, 
					smoothing.y * Time.deltaTime);
		}
		else
		{
			// We're moving the camera without following the player
			x = Mathf.Lerp(
				x,
				newPosition.x,
				Time.deltaTime * zoomSpeed);
			y = Mathf.Lerp(
				y,
				newPosition.y,
				Time.deltaTime * zoomSpeed);
		}

		if(IsZooming)
		{
			thisCam.orthographicSize = Mathf.Lerp(
				currOrthSize,
				newOrthSize,
				Time.deltaTime * zoomSpeed);
			if (thisCam.orthographicSize == newOrthSize)
				IsZooming = false;
		}

		if (IsBounded)
		{
			float updatedOrthSize = thisCam.orthographicSize;
			float updatedCameraHalfWidth = thisCam.orthographicSize * ((float)Screen.width / Screen.height);

			x = Mathf.Clamp(
				x,
				_min.x + updatedCameraHalfWidth,
				_max.x - updatedCameraHalfWidth);
			y = Mathf.Clamp(
				y,
				_min.y + updatedOrthSize,
				_max.y - updatedOrthSize);
		}

		transform.position = new Vector3(
			x, 
			y, 
			transform.position.z);
	}

	public void MoveCamera(Vector3 newPosition, float moveSpeed, bool keepWithinBounds)
	{
		//Debug.Log("MoveCamera");
		IsFollowing = false;
		this.newPosition = newPosition;
		this.moveSpeed = moveSpeed;
		this.IsBounded = keepWithinBounds;
	}

	public void Zoom(float newOrthSize, float zoomSpeed, bool keepWithinBounds)
	{
		//Debug.Log("Zoom");
		IsZooming = true;
		this.newOrthSize = newOrthSize;
		this.zoomSpeed = zoomSpeed;
		this.IsBounded = keepWithinBounds;
	}
}
