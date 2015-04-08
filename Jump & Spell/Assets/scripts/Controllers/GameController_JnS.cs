using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 
/// </summary>
public class GameController_JnS : MonoBehaviour
{
	public enum ScoreEvent
	{
		WrongLetter,
		CorrectLetter,
		CompletedWord
	}

	[SerializeField]
	private GameTimer deathTimer;
	[SerializeField]
	private GameStopwatch rescueTimer;
	[SerializeField]
	private GameObject levelExit;
	private PlayerController player;
	private UIManager_JnS uiManager;
	private LetterManager_JnS letterManager;

	[SerializeField]
	private int letterScore = 10;
	[SerializeField]
	private int wordScorePerLetter = 20;
	[SerializeField]
	private int wrongLetterScore = -50;
	[SerializeField]
	private int correctLetterTime = 5;
	[SerializeField]
	private int wordTime = 30;

	private bool gameIsOver;
	private int scoreAtStart;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();
		letterManager = GameObject.FindGameObjectWithTag("LetterManager").GetComponent<LetterManager_JnS>();

		scoreAtStart = GameData.dataHolder.score;
	}

	void Start()
	{
		gameIsOver = false;
	}

// Time-affecting
	/// <summary>
	/// Adds theinput number of seconds to the time remaining on the GameTimer.
	/// </summary>
	/// <param name="seconds">The number of seconds to add</param>
	public void AddTime(ScoreEvent scoreEv)
	{
		switch (scoreEv)
		{
			case ScoreEvent.CorrectLetter:
				deathTimer.IncreaseTimeRemaining(correctLetterTime);
				break;

			case ScoreEvent.CompletedWord:
				deathTimer.IncreaseTimeRemaining(wordTime);
				break;

			case ScoreEvent.WrongLetter:
				break;

			default: throw new NotImplementedException("A score event case that has not been implemented is being used.");
		}
	}

	public void ChargeTimeTank(long seconds)
	{
		Debug.Log(string.Format("Charge tank by {0} seconds", seconds));
	}

// Player Control
	public void DisablePlayerControl()
	{
		player.enabled = false;
	}

	/// <summary>
	/// Enables or disables the player's controller script, if the player has not lost. Otherwise, does nothing.
	/// </summary>
	/// <param name="isEnabled">true to enable, false to disable</param>
	public void TogglePlayerControl(bool isEnabled)
	{
		if(!gameIsOver && 
			player.enabled != isEnabled)
		{
			player.enabled = isEnabled;
		}
	}

// Win/Loss
	public void KillPlayer()
	{
		Debug.Log("Player killed");
		ExecuteGameOverProcedure();
		uiManager.DisplayLevelLostPanel = true;
	}

	public void OpenExit()
	{
		Debug.Log("Exit opened");
		levelExit.SetActive(true);
	}

	public void CongratulatePlayer()
	{
		Debug.Log("Player has won");
		ExecuteGameOverProcedure();
		uiManager.DisplayLevelWonPanel = true;
	}

	private void ExecuteGameOverProcedure()
	{
		gameIsOver = true;
		DisablePlayerControl();
		letterManager.DisableLetterPickup();
		rescueTimer.Stop();
		deathTimer.Stop();
	}

// Level transition
	public void RestartLevel()
	{
		GameData.dataHolder.score = scoreAtStart;
		Application.LoadLevel(Application.loadedLevel);
	}

	public void LoadNextLevel()
	{
		Application.LoadLevel(Application.loadedLevel + 1);
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
}