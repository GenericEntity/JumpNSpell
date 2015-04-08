using UnityEngine;

/// <summary>
/// Handles the game effects of zooming out the camera.
/// </summary>
public class CameraZoom_JnS : MonoBehaviour
{
	private GameController_JnS controller;
	private LetterManager_JnS lManager;
	private Camera thisCam;
	private CameraController camController;

	public BoxCollider2D levelViewArea;
	public float zoomSpeed = 10F;
	private float originalOrthSize;

	private bool willProcessZoom;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_JnS>();
		lManager = GameObject.FindGameObjectWithTag("LetterManager").GetComponent<LetterManager_JnS>();
		thisCam = GetComponent<Camera>();
		camController = GetComponent<CameraController>();
	}

	void Start()
	{
		originalOrthSize = thisCam.orthographicSize;
		camController.IsBounded = true;
		camController.IsFollowing = true;
	}

	void Update()
	{
		if (Input.GetButtonDown("ZoomCam"))
		{
			Debug.Log("ZoomOut");

			float newOrthSize;

			var boxAspect = levelViewArea.size.x / levelViewArea.size.y;
			var screenAspect = thisCam.aspect;
			if (boxAspect > screenAspect)
			{
				newOrthSize = (levelViewArea.size.x / screenAspect) / 2;
			}
			else
			{
				newOrthSize = levelViewArea.size.y / 2;
			}

			camController.Zoom(newOrthSize, zoomSpeed, true);
			camController.MoveCamera(levelViewArea.transform.position, zoomSpeed, true);

			controller.TogglePlayerControl(false);

			lManager.InflateLetterSprites(newOrthSize / originalOrthSize);
		}
		else if (Input.GetButtonUp("ZoomCam"))
		{
			Debug.Log("ZoomIn");
			camController.Zoom(originalOrthSize, zoomSpeed, true);
			camController.IsFollowing = true;
			controller.TogglePlayerControl(true);
			lManager.DeflateLetterSprites();
		}
	}
}
