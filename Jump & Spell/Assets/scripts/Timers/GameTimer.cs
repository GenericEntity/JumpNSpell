using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
	private GameController_JnS controller;
	private UIManager_JnS uiManager;

	public long maxSeconds = 180;
	public long startingSeconds = 90;
	private long secondsRemaining;
	private bool gameOver = false;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_JnS>();
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();

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
		uiManager.TimeLeftText = string.Format("{0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);

		// Init slider
		uiManager.InitTimeLeftSlider(
			0F, 
			maxSeconds, 
			secondsRemaining);

		// Start countdown
		InvokeRepeating("TickDown", 1.0F, 1.0F);
	}

	void TickDown()
	{
		// Tick down
		--secondsRemaining;

		// Update number display
		uiManager.TimeLeftText = string.Format("{0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);

		// Update slider
		uiManager.TimeLeftSliderValue = secondsRemaining;

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
		if (gameOver)
			return;

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
		uiManager.TimeLeftText = string.Format("{0:D2}:{1:D2}", secondsRemaining / 60, secondsRemaining % 60);

		// Update slider
		uiManager.TimeLeftSliderValue = secondsRemaining;
	}

	void KillPlayer()
	{
		gameOver = true;
		uiManager.TimeLeftTextColor = Color.red;
		CancelInvoke("TickDown");
		controller.KillPlayer();
	}

	/// <summary>
	/// Stops the timer from ticking.
	/// </summary>
	public void Stop()
	{
		CancelInvoke("TickDown");
	}
}