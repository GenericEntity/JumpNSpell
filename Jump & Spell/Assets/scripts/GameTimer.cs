using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
	public long secondsRemaining = 180;
	public Text timeLeftDisplay;
	GameController controller;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void Start()
	{
		timeLeftDisplay.text = string.Format("Die in: {0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);
		InvokeRepeating("TickDown", 1.0F, 1.0F);
	}

	void TickDown()
	{
		--secondsRemaining;
		timeLeftDisplay.text = string.Format("Die in: {0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);

		if(secondsRemaining == 0)
		{
			timeLeftDisplay.color = Color.red;
			CancelInvoke("TickDown");
			controller.KillPlayer();
		}
	}

	/// <summary>
	/// Adds the input number of seconds to the remaining time.
	/// </summary>
	/// <param name="seconds">The number of seconds to add</param>
	public void IncreaseTimeRemaining(int seconds)
	{
		secondsRemaining += seconds;
		timeLeftDisplay.text = string.Format("Die in: {0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);
	}
}