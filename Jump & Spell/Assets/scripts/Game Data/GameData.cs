using System.Collections;
using UnityEngine;

public class GameData : MonoBehaviour
{
	public static GameData dataHolder;

	public int score;
	// Singleton design - one persistent GameControl instance
	void Awake()
	{
		if (dataHolder == null)
		{
			// If it doesn't exist, create a new one
			DontDestroyOnLoad(gameObject);
			dataHolder = this;
		}
		else if (dataHolder != this)
		{
			// If it exists and is another instance, delete this one because we only want one
			Destroy(gameObject);
		}
	}
}
