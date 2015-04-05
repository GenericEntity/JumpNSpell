using UnityEngine;
using System.Collections;

public class LetterPickup : MonoBehaviour 
{
	public char letter;

	public void OnTriggerEnter2D(Collider2D other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player == null)
			return;

		gameObject.SetActive(false);
		Debug.Log(string.Format("{0} was picked up.", letter));

		WordManager let = other.GetComponent<WordManager>();
		let.AddLetter(letter);
	}
}
