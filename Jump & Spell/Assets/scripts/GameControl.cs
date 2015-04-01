using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour 
{
	public static GameControl control;

	public int score;
	// Singleton design - one persistent GameControl instance
	void Awake()
	{
		if(control == null)
		{
			// If it doesn't exist, create a new one
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if(control != this)
		{
			// If it exists and is another instance, delete this one because we only want one
			Destroy(gameObject);
		}
	}
}
