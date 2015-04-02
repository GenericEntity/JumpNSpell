using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
	public enum ScoreEvent
	{
		WrongLetter,
		CorrectLetter,
		CompletedWord
	}

	public Text statusDisplay;
	public Text progressDisplay;
	public Text scoreDisplay;
	public GameTimer timer;

	public int letterScore = 10;
	public int wordScorePerLetter = 20;
	public int wrongLetterScore = -50;

	public int correctLetterTime = 5;
	public int wordTime = 30;
	public int wrongLetterTime = -10;

	void Awake()
	{

	}

	void Start()
	{
		scoreDisplay.text = string.Format("Score: {0}", GameData.dataHolder.score);
	}

	/// <summary>
	/// Adds theinput number of seconds to the time remaining on the GameTimer.
	/// </summary>
	/// <param name="seconds">The number of seconds to add</param>
	public void AddTime(ScoreEvent scoreEv)
	{
		switch(scoreEv)
		{
			case ScoreEvent.CorrectLetter:
				timer.IncreaseTimeRemaining(correctLetterTime);
				break;
				
			case ScoreEvent.CompletedWord:
				timer.IncreaseTimeRemaining(wordTime);
				break;

			case ScoreEvent.WrongLetter:
				timer.IncreaseTimeRemaining(wrongLetterTime);
				break;

			default: throw new NotImplementedException("A score event case that has not been implemented is being used.");
		}
	}

	public void KillPlayer()
	{
		
	}

	public void OpenExit()
	{

	}

// Score methods
	/// <summary>
	/// Sets the score of the player
	/// </summary>
	/// <param name="score">The score amount</param>
	public void SetScore(int score)
	{
		GameData.dataHolder.score = score;
	}
	/// <summary>
	/// Adds the input amount to the score of the player
	/// </summary>
	/// <param name="toAdd">The amount of score to add</param>
	public void AddScore(int toAdd)
	{
		GameData.dataHolder.score += toAdd;
	}
	/// <summary>
	/// Updates the score according to a score-affecting event which has taken place (cases defined in enum ScoreEvent).
	/// </summary>
	/// <param name="scoreEvent">The score-affecting event which took place (e.g. picking up a correct letter)</param>
	/// <param name="goalLength">Optional length of word which must be included if action has the value ScoreAction.CompletedWord</param>
	public void UpdateScore(ScoreEvent scoreEvent, int goalLength = -1)
	{
		switch (scoreEvent)
		{
			case ScoreEvent.CorrectLetter:
				GameData.dataHolder.score += letterScore;
				break;

			case ScoreEvent.CompletedWord:
				if (goalLength < 1)
					throw new Exception("If a word has been completed, a valid word length must be provided.");
				GameData.dataHolder.score += letterScore + (goalLength * wordScorePerLetter);
				break;

			case ScoreEvent.WrongLetter:
				GameData.dataHolder.score += wrongLetterScore;
				break;

			default: throw new NotImplementedException("A score action case is being called but has not been implemented.");
		}
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
}