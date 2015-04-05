using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class WordManager : MonoBehaviour
{
	/*** TEST IMPLEMENTATION ***/
	private string[] wordlist = { "first", "crackers", "practice", "light", "out", "balloon", "bought", "tonight" };
	/*** TEST IMPLEMENTATION ***/

	private List<char> letters;
	private string goal;

	private GameController controller;
	public int respawnDuration = 3;

	void Awake()
	{
		letters = new List<char>();
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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

		bool isRightLetter = CheckWord(word);
		/*
		Debug.Log(string.Format("correct spelling: {0}", isRightLetter));
		 */
		if (isRightLetter && word.Equals(goal))
		{
			Debug.Log(string.Format("Word complete: {0}", goal));
			controller.SetStatusTextColor(Color.yellow);
			controller.SetStatusText("Word Complete!");
			controller.UpdateScore(GameController.ScoreEvent.CompletedWord, goal.Length);
			controller.AddTime(GameController.ScoreEvent.CompletedWord);
			StartCoroutine("CycleWord");
		}
		else if (!isRightLetter)
		{
			Debug.Log(string.Format("Incorrect letter picked up: {0}", word[word.Length - 1]));
			controller.SetStatusTextColor(Color.red);
			controller.SetStatusText(string.Format("Incorrect letter: {0}", word[word.Length - 1]));
			controller.UpdateScore(GameController.ScoreEvent.WrongLetter);
			controller.AddTime(GameController.ScoreEvent.WrongLetter);
			StartCoroutine("CycleWord");
		}
		else
		{
			Debug.Log(string.Format("Correct letter picked up: {0}", word[word.Length - 1]));
			controller.SetProgressText(string.Format("Word: <color=\"#00CECEFF\">{0}</color>{1}", word, goal.Substring(word.Length)));
			controller.UpdateScore(GameController.ScoreEvent.CorrectLetter);
			controller.AddTime(GameController.ScoreEvent.CorrectLetter);
		}

		controller.SetScoreText(string.Format("Score: {0}", GameData.dataHolder.score));
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
		controller.ClearLetterSpawns();

		// Wait a certain amount of time before creating the new word
		for (int i = respawnDuration; i > 0; --i)
		{
			controller.SetProgressText("New word in " + i);
			yield return new WaitForSeconds(1);
		}

		controller.SetStatusText(string.Empty);
		GetNextWordGoal();
	}

	void GetNextWordGoal()
	{
		// Randomly select a new word
		string chosen = wordlist[UnityEngine.Random.Range(0, wordlist.Length)];
		Debug.Log(string.Format("{0} has been chosen. NOTE: REFERENCE AN EXTERNAL DATABASE OR TEXT FILE IN FUTURE.\nIT IS CURRENTLY USING A BUILT-IN STRING ARRAY.", chosen));

		// Set that word as the goal
		goal = chosen.ToUpper();
		controller.SetProgressText(string.Format("Word: {0}", goal));
		Debug.Log(string.Format("Goal is {0}", goal));

		// Spawn letters
		controller.SpawnNewLetters(goal);
	}


}
