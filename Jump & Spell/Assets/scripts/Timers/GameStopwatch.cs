using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStopwatch : MonoBehaviour
{
	private GameController_JnS controller;
	private UIManager_JnS uiManager;
	
	private long secondsLived = 0;
	public long goalInSeconds = 240;

	private byte tenthOfSecCount;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_JnS>();
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();

		// Check for invalid values
		if (goalInSeconds <= 0)
			throw new Exception("goalInSeconds must be greater than 0.");
	}

	void Start()
	{
		// Init number display
		uiManager.TimeGoalText = "00:00";

		// Init slider
		uiManager.InitTimeGoalSlider(
			0F,
			goalInSeconds,
			0F);

		tenthOfSecCount = 0;

		// Start stopwatch
		InvokeRepeating("CountUp", 0.1F, 0.1F);
	}

	void CountUp()
	{
		++tenthOfSecCount;

		if (tenthOfSecCount % 10 == 0)
		{
			// Tick
			++secondsLived;

			// Update number display
			uiManager.TimeGoalText = string.Format("{0:D2}:{1:D2}", secondsLived / 60, secondsLived % 60);

			// Adjust slider
			uiManager.TimeGoalSliderValue = secondsLived;

			// Check for goal
			if (secondsLived == goalInSeconds)
			{
				CancelInvoke("CountUp");
				controller.OpenExit();
			}

			tenthOfSecCount = 0;
		}
	}

	/// <summary>
	/// Stops the stopwatch from ticking.
	/// </summary>
	public void Stop()
	{
		CancelInvoke("CountUp");
	}

	public void Resume()
	{
		InvokeRepeating("CountUp", 0.1F, 0.1F);
	}
}