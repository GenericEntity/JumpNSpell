using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStopwatch : MonoBehaviour
{
	private GameController controller;
	
	private long secondsLived = 0;
	public long goalInSeconds = 240;
	public Slider timeGoalSlider;
	public Text timeGoalNumberDisplay;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		// Check for invalid values
		if (goalInSeconds <= 0)
			throw new Exception("goalInSeconds must be greater than 0.");
	}

	void Start()
	{
		// Init number display
		timeGoalNumberDisplay.text = "00:00";

		// Init slider
		timeGoalSlider.minValue = 0F;
		timeGoalSlider.maxValue = goalInSeconds;
		timeGoalSlider.value = 0F;

		// Start stopwatch
		InvokeRepeating("CountUp", 1.0F, 1.0F);
	}

	void CountUp()
	{
		// Tick
		++secondsLived;

		// Update number display
		timeGoalNumberDisplay.text = string.Format("{0:D2}:{1:D2}", secondsLived / 60, secondsLived % 60);

		// Adjust slider
		timeGoalSlider.value = secondsLived;

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