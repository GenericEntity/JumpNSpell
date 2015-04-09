using UnityEngine;
using UnityEngine.UI;
using System;

	/// <summary>
	/// Controls and updates the UI displays.
	/// </summary>
public class UIManager_JnS : MonoBehaviour
{
	[SerializeField]
	private Text statusDisplay;
	[SerializeField]
	private Text progressDisplay;
	[SerializeField]
	private Text scoreDisplay;
	[SerializeField]
	private GameObject levelLostPanel;
	[SerializeField]
	private GameObject levelWonPanel;
	[SerializeField]
	private Text timeGoalNumberDisplay;
	[SerializeField]
	private Slider timeGoalSlider;
	[SerializeField]
	private Text timeLeftNumberDisplay;
	[SerializeField]
	private Slider timeLeftSlider;
	[SerializeField]
	private GameObject messagePanel;
	[SerializeField]
	private Text messageDisplay;

	public string StatusText
	{
		set { statusDisplay.text = value; }
	}

	public Color StatusTextColor
	{
		get { return statusDisplay.color; }
		set { statusDisplay.color = value; }
	}

	public string ProgressText
	{
		set { progressDisplay.text = value; }
	}

	public Color ProgressTextColor
	{
		get { return progressDisplay.color; }
		set { progressDisplay.color = value; }
	}

	public string ScoreText
	{
		set { scoreDisplay.text = value; }
	}

	public Color ScoreTextColor
	{
		get { return scoreDisplay.color; }
		set { scoreDisplay.color = value; }
	}

	public bool DisplayLevelLostPanel
	{
		get { return levelLostPanel.activeInHierarchy; }
		set { levelLostPanel.SetActive(value); }
	}

	public bool DisplayLevelWonPanel
	{
		get { return levelWonPanel.activeInHierarchy; }
		set { levelWonPanel.SetActive(value); }
	}

	public string TimeGoalText
	{
		set { timeGoalNumberDisplay.text = value; }
	}

	public Color TimeGoalTextColor
	{
		get { return timeGoalNumberDisplay.color; }
		set { timeGoalNumberDisplay.color = value; }
	}

	public float TimeGoalSliderValue
	{
		get { return timeGoalSlider.value; }
		set 
		{
			if (value >= timeGoalSlider.minValue &&
				value <= timeGoalSlider.maxValue)
				timeGoalSlider.value = value;
			else
				throw new Exception(string.Format(
					"The input value is outside the range of the slider. Min value is {0}; max value is {1}",
					timeGoalSlider.minValue,
					timeGoalSlider.maxValue));
		}
	}

	public string TimeLeftText
	{
		set { timeLeftNumberDisplay.text = value; }
	}

	public Color TimeLeftTextColor
	{
		get { return timeLeftNumberDisplay.color; }
		set { timeLeftNumberDisplay.color = value; }
	}

	public float TimeLeftSliderValue
	{
		get { return timeLeftSlider.value; }
		set
		{
			if (value >= timeLeftSlider.minValue &&
				value <= timeLeftSlider.maxValue)
				timeLeftSlider.value = value;
			else
				throw new Exception(string.Format(
					"The input value is outside the range of the slider. Min value is {0}; max value is {1}",
					timeLeftSlider.minValue,
					timeLeftSlider.maxValue));
		}
	}

	public bool DisplayMessagePanel
	{
		get { return messagePanel.activeInHierarchy; }
		set { messagePanel.SetActive(value); }
	}

	public string MessageDisplayText
	{
		get { return messageDisplay.text; }
		set { messageDisplay.text = value; }
	}

	public Color MessageDisplayTextColor
	{
		get { return messageDisplay.color; }
		set { messageDisplay.color = value; }
	}


	void Start()
	{
		this.DisplayLevelLostPanel = false;
		this.DisplayLevelWonPanel = false;
		this.ScoreText = string.Format("Score: {0}", GameData.dataHolder.score);
	}

	/// <summary>
	/// Initialises the min, max and current values of the Time Goal slider.
	/// </summary>
	/// <param name="minValue">The minimum value the slider can represent</param>
	/// <param name="maxValue">The maximum value the slider can represent</param>
	/// <param name="initialValue">The starting value of the slider</param>
	public void InitTimeGoalSlider(float minValue, float maxValue, float initialValue)
	{
		timeGoalSlider.minValue = minValue;
		timeGoalSlider.maxValue = maxValue;
		timeGoalSlider.value = initialValue;
	}
	/// <summary>
	/// Initialises the min, max and current values of the Time Left slider.
	/// </summary>
	/// <param name="minValue">The minimum value the slider can represent</param>
	/// <param name="maxValue">The maximum value the slider can represent</param>
	/// <param name="initialValue">The starting value of the slider</param>
	public void InitTimeLeftSlider(float minValue, float maxValue, float initialValue)
	{
		timeLeftSlider.minValue = minValue;
		timeLeftSlider.maxValue = maxValue;
		timeLeftSlider.value = initialValue;
	}

}
