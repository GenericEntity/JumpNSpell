using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LetterManager : MonoBehaviour
{
	/*** TEST IMPLEMENTATION ***/
	string[] wordlist = { "first", "crackers", "practice", "light", "out", "balloon", "bought", "tonight" };
	/*** TEST IMPLEMENTATION ***/

	List<char> letters;
	string goal;

	public LetterSpawner spawner;
	public int respawnDuration = 3;

	void Awake()
	{
		letters = new List<char>();
	}

	void Start()
	{
		GetNextWordGoal();
	}

	public void AddLetter(char c)
	{
		letters.Add(c);
		string word = "";
		foreach (char ch in letters)
			word += ch;
		word = word.ToUpper();
		GameController.controller.SetProgressText(word);

		bool isRightLetter = CheckWord(word);
		/*
		Debug.Log(string.Format("correct spelling: {0}", isRightLetter));
		 */
		if (isRightLetter && word.Equals(goal))
		{
			Debug.Log(string.Format("Word complete: {0}", goal));
			GameController.controller.SetProgressColor(Color.green);
			GameController.controller.SetProgressText("Word Complete!");
			GameController.controller.UpdateScore(GameController.ScoreEvent.CompletedWord, goal.Length);
			StartCoroutine("CycleWord");
		}
		else if (!isRightLetter)
		{
			Debug.Log(string.Format("Incorrect letter picked up: {0}", word[word.Length - 1]));
			GameController.controller.SetProgressColor(Color.red);
			GameController.controller.SetProgressText(string.Format("Incorrect letter: {0}", word[word.Length - 1]));
			GameController.controller.UpdateScore(GameController.ScoreEvent.WrongLetter);
			StartCoroutine("CycleWord");
		}
		else
		{
			Debug.Log(string.Format("Correct letter picked up: {0}", word[word.Length - 1]));
			GameController.controller.UpdateScore(GameController.ScoreEvent.CorrectLetter);
		}

		GameController.controller.SetScoreText(string.Format("Score: {0}", GameData.dataHolder.score));
	}

	/// <summary>
	/// Checks if the word has been spelled correctly so far. Returns true if so, and false otherwise.
	/// </summary>
	/// <param name="word">The letters collected so far</param>
	/// <returns></returns>
	bool CheckWord(string word)
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

	IEnumerator CycleWord()
	{
		// Delete all letters in the list
		letters.Clear();
		spawner.ClearAllLetters();

		// Wait a certain amount of time before creating the new word
		for (int i = respawnDuration; i > 0; --i)
		{
			GameController.controller.SetStatusText("New word in " + i);
			yield return new WaitForSeconds(1);
		}

		GameController.controller.SetProgressText(string.Empty);
		GameController.controller.SetProgressColor(Color.black);
		GetNextWordGoal();
	}

	void GetNextWordGoal()
	{
		// Randomly select a new word
		string chosen = wordlist[UnityEngine.Random.Range(0, wordlist.Length)];
		Debug.Log(string.Format("{0} has been chosen. NOTE: REFERENCE AN EXTERNAL DATABASE OR TEXT FILE IN FUTURE.\nIT IS CURRENTLY USING A BUILT-IN STRING ARRAY.", chosen));

		// Set that word as the goal
		goal = chosen.ToUpper();
		GameController.controller.SetStatusText(string.Format("Goal: {0}", goal));
		Debug.Log(string.Format("Goal is {0}", goal));

		// Spawn letters
		spawner.SpawnNewLetters(goal);
	}


}
