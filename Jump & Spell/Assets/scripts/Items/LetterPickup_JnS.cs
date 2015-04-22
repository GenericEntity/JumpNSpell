using UnityEngine;
using System.Collections;

public class LetterPickup_JnS : MonoBehaviour 
{
	[SerializeField]
	private char letter;

	public char Letter
	{
		get { return letter; }
		set { letter = value; }
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		var player = other.GetComponent<Player>();
		if (player == null)
			return;

		gameObject.SetActive(false);
		Debug.Log(string.Format("{0} was picked up.", letter));

		WordManager_JnS let = other.GetComponent<WordManager_JnS>();
		let.AddLetter(letter);
	}
}
