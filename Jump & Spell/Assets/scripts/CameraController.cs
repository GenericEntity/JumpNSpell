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

	public bool IsFollowing { get; set; }

	public void Start()
	{
		_min = bounds.bounds.min;
		_max = bounds.bounds.max;
		IsFollowing = true;
	}

	public void Update()
	{
		var x = transform.position.x;
		var y = transform.position.y;

		if (IsFollowing)
		{
			if(Mathf.Abs(x - player.position.x) > margin.x)
				x = Mathf.Lerp(x, player.position.x, smoothing.x * Time.deltaTime);

			if (Mathf.Abs(y - player.position.y) > margin.y)
				y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);
		}

		var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);
		var cameraHalfHeight = GetComponent<Camera>().orthographicSize;

		x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
		y = Mathf.Clamp(y, _min.y + cameraHalfHeight, _max.y - cameraHalfHeight);

		transform.position = new Vector3(x, y, transform.position.z);
	}
}
