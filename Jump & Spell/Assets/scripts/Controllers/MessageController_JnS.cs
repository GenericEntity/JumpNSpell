using UnityEngine;
using System;
using System.Collections;
public class MessageController_JnS : MonoBehaviour
{
	private UIManager_JnS uiManager;
	private GameController_JnS controller;

	private string[] messageSequence;
	private int index;
	private bool isDisplayingMessages;
	private bool isTyping;
	private string currMessage;

	public bool DisplayingMessages
	{
		get { return isDisplayingMessages; }
	}

	[SerializeField]
	private float letterPause = 0.04f;
	//public AudioClip sound;

	void Awake()
	{
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_JnS>();
		index = 0;
		isDisplayingMessages = false;
		isTyping = false;
		currMessage = string.Empty;
	}

	void Update()
	{
		if(isDisplayingMessages && Input.GetKeyDown("f"))
		{
			Debug.Log("NextDialog registered");
			if(isTyping)
			{
				SkipText();
				uiManager.MessageDisplayText = currMessage;
			}
			else if(index >= messageSequence.Length)
			{
				EndMessageSequence();
			}
			else
			{
				StartNextMessage(messageSequence[index]);
				++index;
			}
		}
		else if(isDisplayingMessages && Input.GetKeyDown("space"))
		{
			SkipText();
			EndMessageSequence();
		}
	}

	private void StartNextMessage(string msg)
	{
		// Reset GUI text
		uiManager.MessageDisplayText = string.Empty;
		// Set next message
		currMessage = msg;
		// Start typing
		StartCoroutine("TypeText");
	}

	public void StartMessageSequence(string[] messages)
	{
		if (messages == null ||
			messages.Length == 0)
			return;

		controller.State = GameController_JnS.GameState.DisplayingMessages;
		controller.PauseGame();
		// Allow message advancement
		isDisplayingMessages = true;
		// Set the sequence
		messageSequence = messages;
		// Reset sequence sentinel
		index = 1;
		// Make panel visible
		uiManager.DisplayMessagePanel = true;
		// Start first message
		StartNextMessage(messageSequence[0]);
	}

	public void StartMessageSequence(string[] messages, Color textColor)
	{
		uiManager.MessageDisplayTextColor = textColor;
		StartMessageSequence(messages);
	}

	private void EndMessageSequence()
	{
		// Make panel invisible
		uiManager.DisplayMessagePanel = false;
		// Reset panel text
		uiManager.MessageDisplayText = string.Empty;
		// Disallow message advancement
		isDisplayingMessages = false;

		controller.UnpauseGame();
		controller.State = GameController_JnS.GameState.Playing;
	}

	private void SkipText()
	{
		StopCoroutine("TypeText");
		isTyping = false;
	}

	private IEnumerator TypeText()
	{
		isTyping = true;

		foreach (char letter in currMessage.ToCharArray())
		{
			uiManager.MessageDisplayText += letter;
			//if (sound)
			//	audio.PlayOneShot(sound);

			yield return new WaitForSeconds(letterPause);
		}

		isTyping = false;
	}

	
}

