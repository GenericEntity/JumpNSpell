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
	public enum GameState
	{
		Playing,
		Paused,
		Over,
		DisplayingMessages
	}

	[SerializeField]
	private GameTimer deathTimer;
	[SerializeField]
	private GameStopwatch rescueTimer;
	[SerializeField]
	private GameObject levelExit;
	private GameObject sceneCam;
	private Player player;
	private UIManager_JnS uiManager;
	private WordManager_JnS wordManager;

	[SerializeField]
	private int letterScore = 10;
	[SerializeField]
	private int wordScorePerLetter = 20;
	[SerializeField]
	private int wrongLetterScore = -50;
	[SerializeField]
	private int correctLetterTime = 5;
	[SerializeField]
	private int wordTimePerLetter = 5;

	public GameState State { get; set; }

	public bool HasWon { get; set; }

	private int scoreAtStart;

	public bool canPause { get; set; }
	private bool isPaused;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();
		sceneCam = GameObject.FindGameObjectWithTag("MainCamera");
		wordManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WordManager_JnS>();

		canPause = true;
		HasWon = false;
		State = GameState.Playing;
	}
	
	void Start()
	{
		scoreAtStart = GameData.dataHolder.score;
		ForceEnableAllTimers(true);
		ForceEnableAllControls(true);
		levelExit.SetActive(false);
		uiManager.DisplayLevelLostPanel = false;
		uiManager.DisplayLevelWonPanel = false;
		uiManager.EnableHUD(true);
		uiManager.CoverLevel = false;
		uiManager.DisplayGameMenu = false;
	}

// Time-affecting
	/// <summary>
	/// Adds theinput number of seconds to the time remaining on the GameTimer.
	/// </summary>
	/// <param name="seconds">The number of seconds to add</param>
	public void AddTime(ScoreEvent scoreEv, int goalLength = -1)
	{
		switch (scoreEv)
		{
			case ScoreEvent.CorrectLetter:
				deathTimer.IncreaseTimeRemaining(correctLetterTime);
				break;

			case ScoreEvent.CompletedWord:
				deathTimer.IncreaseTimeRemaining(wordTimePerLetter * goalLength);
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
	public void EnableMovementIfValid(bool enable)
	{
		if (State == GameState.Playing)
			player.enabled = enable;
	}

	public void ForceEnableAllTimers(bool enable)
	{
		if(enable)
		{
			deathTimer.Resume();
			rescueTimer.Resume();
			wordManager.PauseSpawning();
		}
		else
		{
			// Death timer
			deathTimer.Stop();
			// Rescue timer
			rescueTimer.Stop();
			// Letter respawn timer
			wordManager.ResumeSpawning();
		}

	}

	public void ForceEnableAllControls(bool enable)
	{
		// Movement
		player.enabled = enable;
		// Cam Zoom
		sceneCam.GetComponent<CameraZoom_JnS>().enabled = enable;
		// Exit level
		levelExit.GetComponent<LevelExit_JnS>().enabled = enable;
		// Pause
		canPause = enable;
	}

// Win/Loss
	public void KillPlayer()
	{
		GameOverProcedure();
		HasWon = false;
		uiManager.DisplayLevelLostPanel = true;
	}

	public void OpenExit()
	{
		levelExit.SetActive(true);
	}

	public void CongratulatePlayer()
	{
		GameOverProcedure();
		HasWon = true;
		uiManager.DisplayLevelWonPanel = true;
	}

	private void GameOverProcedure()
	{
		State = GameState.Over;
		ForceEnableAllControls(false);
		ForceEnableAllTimers(false);
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

	public void LoadMainMenu()
	{
		UnpauseGame();
		Application.LoadLevel(0);
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

// Pause
	public void PauseGame()
	{
		if(canPause)
		{
			Time.timeScale = 0F;
			isPaused = true;
			player.enabled = false;
			sceneCam.GetComponent<CameraZoom_JnS>().enabled = false;
			levelExit.GetComponent<LevelExit_JnS>().enabled = false;
			uiManager.DisplayGameMenu = true;
		}
			
	}

	public void UnpauseGame()
	{
		if (canPause)
		{
			Time.timeScale = 1F;
			isPaused = false;
			player.enabled = true;
			sceneCam.GetComponent<CameraZoom_JnS>().enabled = true;
			levelExit.GetComponent<LevelExit_JnS>().enabled = true;
			uiManager.DisplayGameMenu = false;
		}
			
	}

	void Update()
	{
		if(Input.GetButtonDown("OpenMenu"))
		{
			if (isPaused)
				UnpauseGame();
			else
				PauseGame();
		}
	}
}