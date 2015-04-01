using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
	public long timeRemaining = 180;
	public Text timerDisplay;

	void Start()
	{
		timerDisplay.text = TimeSpan.FromSeconds(timeRemaining).ToString();
		InvokeRepeating("DecreaseTimeRemaining", 1.0F, 1.0F);
	}

	void DecreaseTimeRemaining()
	{
		--timeRemaining;
		timerDisplay.text = TimeSpan.FromSeconds(timeRemaining).ToString();

		if(timeRemaining == 0)
		{
			timerDisplay.color = Color.red;
			CancelInvoke("DecreaseTimeRemaining");
			GameController.controller.End();
		}
	}
}