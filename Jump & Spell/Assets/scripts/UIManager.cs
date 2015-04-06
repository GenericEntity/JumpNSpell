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

		void Start()
		{
			ToggleGameOverPanel(false);
			scoreDisplay.text = string.Format("Score: {0}", GameData.dataHolder.score);
		}

// UI Text Methods
		/// <summary>
		/// Sets the text of the statusDisplay
		/// </summary>
		/// <param name="text">The text to set</param>
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
		/// <param name="color">The color to set</param>
		public void SetStatusTextColor(Color color)
		{
			statusDisplay.color = color;
		}
		/// <summary>
		/// Sets the text of the progressDisplay.
		/// </summary>
		/// <param name="text">The text to set</param>
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
		/// <param name="color">The color to set</param>
		public void SetProgressTextColor(Color color)
		{
			progressDisplay.color = color;
		}
		/// <summary>
		/// Sets the text of the scoreDisplay.
		/// </summary>
		/// <param name="text">The text to set</param>
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
		/// <param name="color">The color to set</param>
		public void SetScoreTextColor(Color color)
		{
			scoreDisplay.color = color;
		}

		public void ToggleGameOverPanel(bool willEnable)
		{
			gameOverPanel.SetActive(willEnable);
		}
	}
