using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WordManager_JnS : MonoBehaviour
{
	/*** TEST IMPLEMENTATION ***/
	private string[] wordlist = { "first", "crackers", "practice", "light", "out", "balloon", "bought", "tonight", "sugar", "fierce", "halfway", "cupboard", "even" };
	private List<string>[] wordLists;
	/*** TEST IMPLEMENTATION ***/

	private List<char> letters;
	private string goal;

	private GameController_JnS controller;
	private UIManager_JnS uiManager;
	private LetterManager_JnS lManager;
	public int respawnDuration = 3;
	public int maxWordLength;

	void Awake()
	{
		letters = new List<char>();
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_JnS>();
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();
		lManager = GameObject.FindGameObjectWithTag("LetterManager").GetComponent<LetterManager_JnS>();
	}

	void Start()
	{
		// Find longest word
		int longest = 0;
		foreach (string word in wordlist)
		{
			if (word.Length > longest)
				longest = word.Length;
		}

		// Create word lists indexed by word length
		wordLists = new List<string>[longest];
		for (int i = 0; i < wordLists.Length; ++i)
		{
			wordLists[i] = new List<string>();
		}
		// Populate word lists
		foreach (string word in wordlist)
		{
			wordLists[word.Length - 1].Add(word);
		}

		int spawnCount = lManager.SpawnPointCount;

		// Validate max word length
		if (maxWordLength > spawnCount ||
			maxWordLength > longest)
		{
			maxWordLength = Mathf.Min(spawnCount, longest);
			if (maxWordLength <= 0)
				throw new Exception("For some reason, maxWordLength has ended up as zero or lower.");
			Debug.Log(string.Format("The input max word length was too long. Value has defaulted to {0}", maxWordLength));
		}

		GetNextWordGoal();
	}

	public void AddLetter(char c)
	{
		letters.Add(c);
		string word = "";
		foreach (char ch in letters)
			word += ch;
		word = word.ToUpper();

		bool isRightLetter = CheckWord(word);
		/*
		Debug.Log(string.Format("correct spelling: {0}", isRightLetter));
		 */
		if (isRightLetter && word.Equals(goal))
		{
			Debug.Log(string.Format("Word complete: {0}", goal));

			uiManager.StatusTextColor = Color.yellow;
			uiManager.StatusText = "Word Complete!";

			controller.UpdateScore(GameController_JnS.ScoreEvent.CompletedWord, goal.Length);
			controller.AddTime(GameController_JnS.ScoreEvent.CompletedWord);

			StartCoroutine(CycleWord());
		}
		else if (!isRightLetter)
		{
			Debug.Log(string.Format("Incorrect letter picked up: {0}", word[word.Length - 1]));

			uiManager.StatusTextColor = Color.red;
			uiManager.StatusText = string.Format("Incorrect letter: {0}", word[word.Length - 1]);

			controller.UpdateScore(GameController_JnS.ScoreEvent.WrongLetter);
			controller.AddTime(GameController_JnS.ScoreEvent.WrongLetter);

			StartCoroutine(CycleWord());
		}
		else
		{
			Debug.Log(string.Format("Correct letter picked up: {0}", word[word.Length - 1]));

			uiManager.ProgressText = string.Format("Word: <color=\"#00CECEFF\">{0}</color>{1}", word, goal.Substring(word.Length));

			controller.UpdateScore(GameController_JnS.ScoreEvent.CorrectLetter);
			controller.AddTime(GameController_JnS.ScoreEvent.CorrectLetter);
		}

		uiManager.ScoreText = string.Format("Score: {0}", GameData.dataHolder.score);
	}

	/// <summary>
	/// Checks if the word has been spelled correctly so far. Returns true if so, and false otherwise.
	/// </summary>
	/// <param name="word">The letters collected so far</param>
	/// <returns></returns>
	private bool CheckWord(string word)
	{
		if(goal == null || goal.Equals(string.Empty))
		{
			Debug.Log("Error: cannot check word because goal is empty.");
			throw new NullReferenceException();
		}

		int len = word.Length;

		for(int i = 0; i < len; ++i)
		{
			if (!word[i].Equals(goal[i]))
				return false;
		}

		return true;
	}

	private IEnumerator CycleWord()
	{
		// Delete all letters in the list
		letters.Clear();
		lManager.ClearAllLetters();

		// Wait a certain amount of time before creating the new word
		for (int i = respawnDuration; i > 0; --i)
		{
			uiManager.ProgressText = "New word in " + i;
			yield return new WaitForSeconds(1);
		}

		uiManager.StatusText = string.Empty;
		GetNextWordGoal();
	}

	private void GetNextWordGoal()
	{
		// Randomly select a new word
		int wordLength;
		do
		{
			wordLength = UnityEngine.Random.Range(0, maxWordLength);
		}
		while (wordLists[wordLength].Count == 0);
			
		int wordIndex = UnityEngine.Random.Range(0, wordLists[wordLength].Count);
		string chosen = wordLists[wordLength][wordIndex];
		Debug.Log(string.Format("{0} has been chosen. NOTE: REFERENCE AN EXTERNAL DATABASE OR TEXT FILE IN FUTURE.\nIT IS CURRENTLY USING A BUILT-IN STRING ARRAY.", chosen));

		// Set that word as the goal
		goal = chosen.ToUpper();
		uiManager.ProgressText = string.Format("Word: {0}", goal);
		Debug.Log(string.Format("Goal is {0}", goal));

		// Spawn letters
		lManager.SpawnNewLetters(goal);
	}
}
