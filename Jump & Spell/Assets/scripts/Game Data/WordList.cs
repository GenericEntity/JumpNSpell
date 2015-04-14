using UnityEngine;
using System.Collections;

public class WordList : MonoBehaviour
{
	public static WordList wordHolder;
	private string[] wordList;

	public string[] List
	{
		get { return wordList; }
	}

	// Singleton design - one persistent GameControl instance
	void Awake()
	{
		if (wordHolder == null)
		{
			// If it doesn't exist, create a new one
			DontDestroyOnLoad(gameObject);
			wordHolder = this;
		}
		else if (wordHolder != this)
		{
			// If it exists and is another instance, delete this one because we only want one
			Destroy(gameObject);
		}

		TextAsset list = Resources.Load<TextAsset>("wordlist");
		wordHolder.wordList = list.text.Split(',');
	}
}
