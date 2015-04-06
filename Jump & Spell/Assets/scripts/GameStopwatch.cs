using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStopwatch : MonoBehaviour
{
	private GameController controller;
	private UIManager uiManager;
	
	private long secondsLived = 0;
	public long goalInSeconds = 240;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

		// Check for invalid values
		if (goalInSeconds <= 0)
			throw new Exception("goalInSeconds must be greater than 0.");
	}

	void Start()
	{
		// Init number display
		uiManager.SetTimeGoalText("00:00");

		// Init slider
		uiManager.InitTimeGoalSlider(
			0F,
			goalInSeconds,
			0F);

		// Start stopwatch
		InvokeRepeating("CountUp", 1.0F, 1.0F);
	}

	void CountUp()
	{
		// Tick
		++secondsLived;

		// Update number display
		uiManager.SetTimeGoalText(string.Format("{0:D2}:{1:D2}", secondsLived / 60, secondsLived % 60));

		// Adjust slider
		uiManager.SetTimeGoalSliderValue(secondsLived);

		// Check for goal
		if(secondsLived == goalInSeconds)
		{
			CancelInvoke("CountUp");
			controller.OpenExit();
		}
	}

	/// <summary>
	/// Stops the stopwatch from ticking.
	/// </summary>
	public void Stop()
	{
		CancelInvoke("CountUp");
	}
}