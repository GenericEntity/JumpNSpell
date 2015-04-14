using UnityEngine;
using System;
using System.Collections;
public class MessageController_JnS : MonoBehaviour
{
	public enum MessageOptions
	{
		LeaveAlone,
		Do,
		DoOpposite
	}

	public enum MessageType
	{
		Beginning,
		During,
		Ending
	}

	private UIManager_JnS uiManager;
	private GameController_JnS controller;

	private string[] messageSequence;
	private int index;
	private bool isDisplayingMessages;
	private bool isTyping;
	private string currMessage;

	private MessageType msgType;
	private MessageOptions coverAfter;
	private MessageOptions hudAfter;
	private GameController_JnS.GameState prevState;

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
		if(isDisplayingMessages && Input.GetKeyDown(KeyCode.F))
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
		else if(isDisplayingMessages && Input.GetKeyDown(KeyCode.Space))
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

	public void StartMessageSequence(string[] messages, 
		MessageType type,
		MessageOptions coverAfter = MessageOptions.LeaveAlone,
		MessageOptions enableHudAfter = MessageOptions.LeaveAlone)
	{
		if (messages == null ||
			messages.Length == 0)
			return;

		this.msgType = type;
		this.coverAfter = coverAfter;
		this.hudAfter = enableHudAfter;
		this.prevState = controller.State;
		controller.State = GameController_JnS.GameState.DisplayingMessages;

		switch(msgType)
		{
			case MessageType.Beginning:
				controller.ForceEnableAllTimers(false);
				controller.ForceEnableAllControls(false);
				break;
			case MessageType.During:
				controller.ForceEnableAllTimers(false);
				controller.ForceEnableAllControls(false);				
				break;
			case MessageType.Ending:
				uiManager.DisplayLevelLostPanel = false;
				uiManager.DisplayLevelWonPanel = false;
				break;
		}

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
	
	private void EndMessageSequence()
	{
		switch (this.coverAfter)
		{
			case MessageOptions.Do:
				uiManager.CoverLevel = true;
				break;
			case MessageOptions.DoOpposite:
				uiManager.CoverLevel = false;
				break;
		}

		switch (this.hudAfter)
		{
			case MessageOptions.Do:
				uiManager.EnableHUD(true);
				break;
			case MessageOptions.DoOpposite:
				uiManager.EnableHUD(false);
				break;
		}

		// Make panel invisible
		uiManager.DisplayMessagePanel = false;
		// Reset panel text
		uiManager.MessageDisplayText = string.Empty;
		// Disallow message advancement
		isDisplayingMessages = false;

		switch(msgType)
		{
			case MessageType.Beginning:
				controller.ForceEnableAllTimers(true);
				controller.ForceEnableAllControls(true);
				break;

			case MessageType.During:
				switch(this.prevState)
				{
					case GameController_JnS.GameState.Playing:
						controller.ForceEnableAllTimers(true);
						controller.ForceEnableAllControls(true);
						break;

					case GameController_JnS.GameState.Over:
						break;

					default: throw new Exception();
				}
				break;

			case MessageType.Ending:
				if (controller.HasWon)
					uiManager.DisplayLevelWonPanel = true;
				else
					uiManager.DisplayLevelLostPanel = true;
				break;
		}

		controller.State = prevState;
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

