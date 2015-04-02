using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameStopwatch : MonoBehaviour
{
	public long secondsLived = 0;
	public Slider progressBar;
	public long goalInSeconds = 240;
	GameController controller;

	void Awake()
	{
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void Start()
	{
		progressBar.minValue = 0F;
		progressBar.maxValue = goalInSeconds;
		progressBar.value = 0F;
		InvokeRepeating("CountUp", 1.0F, 1.0F);
	}

	void CountUp()
	{
		++secondsLived;
		progressBar.value = secondsLived;

		if(secondsLived == goalInSeconds)
		{
			CancelInvoke("CountUp");
			controller.OpenExit();
		}
	}
}