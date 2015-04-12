using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour
{
	private Vector3 startingPosition;

	void Awake()
	{
		startingPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player == null)
			return;

		player.transform.position = startingPosition;
	}
}
