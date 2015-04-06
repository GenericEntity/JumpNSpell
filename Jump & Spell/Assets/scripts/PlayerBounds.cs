using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour
{
	void OnTriggerExit2D(Collider2D other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player == null)
			return;

		Application.LoadLevel(Application.loadedLevel);
	}
}
