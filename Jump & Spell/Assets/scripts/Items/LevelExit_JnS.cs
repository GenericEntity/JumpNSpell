using UnityEngine;
using System.Collections;

public class LevelExit_JnS : MonoBehaviour 
{
	private GameController_JnS controller;
	private bool inExit;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_JnS>();
	}

	void Start()
	{
		inExit = false;
	}

	void Update()
	{
		if(inExit && Input.GetButtonDown("Use"))
		{
			controller.CongratulatePlayer();
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player == null)
			return;

		inExit = true;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player == null)
			return;

		inExit = false;
	}
}
