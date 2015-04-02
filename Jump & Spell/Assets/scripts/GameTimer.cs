using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
	GameController controller;

	public long maxSeconds = 180;
	public long startingSeconds = 90;
	long secondsRemaining;

	public Slider timeLeftSlider;
	public Text timeLeftNumberDisplay;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		// Check for invalid values
		if (maxSeconds < 0 ||
			startingSeconds < 0)
			throw new Exception("maxSeconds or startingSeconds is lesser than 0");
		if (startingSeconds > maxSeconds)
			throw new Exception("startingSeconds cannot be larger than maxSeconds.");
	}

	void Start()
	{
		// Init values
		secondsRemaining = startingSeconds;

		// Init number display
		timeLeftNumberDisplay.text = string.Format("{0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);

		// Init slider
		timeLeftSlider.minValue = 0.0F;
		timeLeftSlider.maxValue = maxSeconds;
		timeLeftSlider.value = secondsRemaining;

		// Start countdown
		InvokeRepeating("TickDown", 1.0F, 1.0F);
	}

	void TickDown()
	{
		// Tick down
		--secondsRemaining;

		// Update number display
		timeLeftNumberDisplay.text = string.Format("{0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);

		// Update slider
		timeLeftSlider.value = secondsRemaining;

		// CHeck for timeout
		if(secondsRemaining == 0)
		{
			KillPlayer();
		}
	}

	/// <summary>
	/// Adds the input number of seconds to the remaining time.
	/// </summary>
	/// <param name="seconds">The number of seconds to add</param>
	public void IncreaseTimeRemaining(int seconds)
	{
		// Update value
		secondsRemaining += seconds;

		// Check for timeout
		if (secondsRemaining <= 0)
		{
			secondsRemaining = 0;
			KillPlayer();
		}

		// Check for excess time
		if(secondsRemaining > maxSeconds)
		{
			controller.ChargeTimeTank(secondsRemaining - maxSeconds);
			secondsRemaining = maxSeconds;
		}

		// Update number display
		timeLeftNumberDisplay.text = string.Format("{0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);

		// Update slider
		timeLeftSlider.value = secondsRemaining;
	}

	void KillPlayer()
	{
		timeLeftNumberDisplay.color = Color.red;
		CancelInvoke("TickDown");
		controller.KillPlayer();
	}
}