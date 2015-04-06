using UnityEngine;
using UnityEngine.UI;

	/// <summary>
	/// Controls and updates the UI displays.
	/// </summary>
public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Text statusDisplay;
	[SerializeField]
	private Text progressDisplay;
	[SerializeField]
	private Text scoreDisplay;
	[SerializeField]
	private GameObject gameOverPanel;
	[SerializeField]
	private Text timeGoalNumberDisplay;
	[SerializeField]
	private Slider timeGoalSlider;
	[SerializeField]
	private Text timeLeftNumberDisplay;
	[SerializeField]
	private Slider timeLeftSlider;

	void Start()
	{
		ToggleGameOverPanel(false);
		scoreDisplay.text = string.Format("Score: {0}", GameData.dataHolder.score);
	}

	// UI Text Methods
	/// <summary>
	/// Sets the text of the statusDisplay
	/// </summary>
	/// <param name="text">The replacement text</param>
	public void SetStatusText(string text)
	{
		statusDisplay.text = text;
	}
	/// <summary>
	/// Appends the input string to the statusDisplay.
	/// </summary>
	/// <param name="toAppend">The text to append</param>
	public void AppendStatusText(string toAppend)
	{
		statusDisplay.text += toAppend;
	}
	/// <summary>
	/// Sets the color of the text of the statusDisplay.
	/// </summary>
	/// <param name="color">The replacement color</param>
	public void SetStatusTextColor(Color color)
	{
		statusDisplay.color = color;
	}
	/// <summary>
	/// Sets the text of the progressDisplay.
	/// </summary>
	/// <param name="text">The replacement text</param>
	public void SetProgressText(string text)
	{
		progressDisplay.text = text;
	}
	/// <summary>
	/// Appends the input text to the progressDisplay.
	/// </summary>
	/// <param name="toAppend">The text to eppend</param>
	public void AppendProgressText(string toAppend)
	{
		progressDisplay.text += toAppend;
	}
	/// <summary>
	/// Sets the color of the text of the progressDisplay.
	/// </summary>
	/// <param name="color">The replacement color</param>
	public void SetProgressTextColor(Color color)
	{
		progressDisplay.color = color;
	}
	/// <summary>
	/// Sets the text of the scoreDisplay.
	/// </summary>
	/// <param name="text">The replacement text</param>
	public void SetScoreText(string text)
	{
		scoreDisplay.text = text;
	}
	/// <summary>
	/// Appends the input text to the scoreDisplay.
	/// </summary>
	/// <param name="toAppend">The text to append</param>
	public void AppendScoreText(string toAppend)
	{
		scoreDisplay.text += toAppend;
	}
	/// <summary>
	/// Sets the color of the text of the scoreDisplay.
	/// </summary>
	/// <param name="color">The replacement color</param>
	public void SetScoreTextColor(Color color)
	{
		scoreDisplay.color = color;
	}
	/// <summary>
	/// Enables or disables Game Over Overlay Panel.
	/// </summary>
	/// <param name="willEnable">True to enable, false to disable</param>
	public void ToggleGameOverPanel(bool willEnable)
	{
		gameOverPanel.SetActive(willEnable);
	}
	/// <summary>
	/// Sets the text within the Time Goal slider.
	/// </summary>
	/// <param name="text">The replacement text</param>
	public void SetTimeGoalText(string text)
	{
		timeGoalNumberDisplay.text = text;
	}
	/// <summary>
	/// Sets the color of the text within the Time Goal slider.
	/// </summary>
	/// <param name="color">The replacement color</param>
	public void SetTimeGoalTextColor(Color color)
	{
		timeGoalNumberDisplay.color = color;
	}
	/// <summary>
	/// Sets the value of the Time Goal slider to the input value.
	/// </summary>
	/// <param name="newValue">The replacement value</param>
	public void SetTimeGoalSliderValue(float newValue)
	{
		timeGoalSlider.value = newValue;
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
	/// Sets the text within the Time Left slider.
	/// </summary>
	/// <param name="text">The replacement text</param>
	public void SetTimeLeftText(string text)
	{
		timeLeftNumberDisplay.text = text;
	}
	/// <summary>
	/// Sets the color of the text within the Time Left slider.
	/// </summary>
	/// <param name="color">The replacement color</param>
	public void SetTimeLeftTextColor(Color color)
	{
		timeLeftNumberDisplay.color = color;
	}
	/// <summary>
	/// Sets the value of the Time Left slider to the input value.
	/// </summary>
	/// <param name="newValue">The replacement value</param>
	public void SetTimeLeftSliderValue(float newValue)
	{
		timeLeftSlider.value = newValue;
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
