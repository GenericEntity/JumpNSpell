using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	private GameController controller;

	public Transform player;
	public Vector2 
		margin,
		smoothing;
	public BoxCollider2D bounds;
	private Vector3
		_min,
		_max;

	private Camera thisCam;
	public BoxCollider2D levelViewArea;
	private float originalOrthSize;
	public float zoomSpeed = 10F;
	private bool isInflated = false;

	public LetterInflater inflater;

	public bool IsFollowing { get; set; }

	void Awake()
	{
		thisCam = GetComponent<Camera>();
		originalOrthSize = thisCam.orthographicSize;
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	public void Start()
	{
		_min = bounds.bounds.min;
		_max = bounds.bounds.max;
		IsFollowing = true;
	}

	public void Update()
	{
		float currOrthSize = thisCam.orthographicSize;
		float cameraHalfWidth = thisCam.orthographicSize * ((float)Screen.width / Screen.height);

		if(Input.GetButton("ZoomCam"))
		{
			float newOrthSize;
			float zoomX = transform.position.x;
			float zoomY = transform.position.y;

			var boxAspect = levelViewArea.size.x / levelViewArea.size.y;
			var screenAspect = thisCam.aspect;
			if(boxAspect > screenAspect)
			{
				newOrthSize = (levelViewArea.size.x / screenAspect) / 2;
			}
			else
			{
				newOrthSize = levelViewArea.size.y / 2;
			}

			thisCam.orthographicSize = Mathf.Lerp(currOrthSize, newOrthSize, Time.deltaTime * zoomSpeed);

			zoomX = Mathf.Lerp(zoomX, levelViewArea.transform.position.x, Time.deltaTime * zoomSpeed);
			zoomY = Mathf.Lerp(zoomY, levelViewArea.transform.position.y, Time.deltaTime * zoomSpeed);

			zoomX = Mathf.Clamp(zoomX, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
			zoomY = Mathf.Clamp(zoomY, _min.y + currOrthSize, _max.y - currOrthSize);

			transform.position = new Vector3(zoomX, zoomY, transform.position.z);
			controller.ToggleUserControl(false);

			if(!isInflated)
			{
				inflater.InflateLetters(newOrthSize / originalOrthSize);
				isInflated = true;
			}

			return;
		}
		else if (currOrthSize != originalOrthSize)
		{
			float newOrthSize = originalOrthSize;

			thisCam.orthographicSize = Mathf.Lerp(currOrthSize, newOrthSize, Time.deltaTime * zoomSpeed);
		}

		// User has control as long as not holding down the zoom key (because code will not reach here if so)
		controller.ToggleUserControl(true);
		if(isInflated)
		{
			inflater.DeflateLetters();
			isInflated = false;
		}
			

		var x = transform.position.x;
		var y = transform.position.y;

		if (IsFollowing)
		{
			if (Mathf.Abs(x - player.position.x) > margin.x)
				x = Mathf.Lerp(x, player.position.x, smoothing.x * Time.deltaTime);

			if (Mathf.Abs(y - player.position.y) > margin.y)
				y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);
		}

		x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
		y = Mathf.Clamp(y, _min.y + currOrthSize, _max.y - currOrthSize);

		transform.position = new Vector3(x, y, transform.position.z);
	}
}
