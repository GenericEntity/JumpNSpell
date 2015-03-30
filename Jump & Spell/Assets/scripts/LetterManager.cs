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

	public Text statusDisplay;
	public Text progressDisplay;
	public LetterSpawner spawner;
	public int respawnDuration = 3;

	void Start()
	{
		letters = new List<char>();
		GetNextWordGoal();
	}

	public void AddLetter(char c)
	{
		letters.Add(c);
		string word = "";
		foreach (char ch in letters)
			word += ch;
		word = word.ToUpper();
		progressDisplay.text = word;

		bool isRightLetter = CheckWord(word);
		/*
		Debug.Log(string.Format("correct spelling: {0}", isRightLetter));
		 */
		if (isRightLetter && word.Equals(goal))
		{
			Debug.Log(string.Format("Word complete: {0}", goal));
			progressDisplay.color = Color.green;
			progressDisplay.text = "Word Complete!";
			StartCoroutine("CycleWord");
		}
		else if (!isRightLetter)
		{
			Debug.Log(string.Format("Incorrect letter picked up: {0}", word[word.Length - 1]));
			progressDisplay.color = Color.red;
			progressDisplay.text = string.Format("Incorrect letter: {0}", word[word.Length - 1]);
			StartCoroutine("CycleWord");
		}
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
			statusDisplay.text = "New word in " + i;
			yield return new WaitForSeconds(1);
		}

		progressDisplay.text = string.Empty;
		progressDisplay.color = Color.black;
		GetNextWordGoal();
	}

	void GetNextWordGoal()
	{
		// Randomly select a new word
		string chosen = wordlist[UnityEngine.Random.Range(0, wordlist.Length)];
		Debug.Log(string.Format("{0} has been chosen. NOTE: REFERENCE AN EXTERNAL DATABASE OR TEXT FILE IN FUTURE.\nIT IS CURRENTLY USING A BUILT-IN STRING ARRAY.", chosen));

		// Set that word as the goal
		goal = chosen.ToUpper();
		statusDisplay.text = string.Format("Goal: {0}", goal);
		Debug.Log(string.Format("Goal is {0}", goal));

		// Spawn letters
		spawner.SpawnNewLetters(goal);
	}
}
