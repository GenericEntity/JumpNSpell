using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour 
{
	private GameController controller;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player == null)
			return;

		controller.ExitLevel();
	}
}
