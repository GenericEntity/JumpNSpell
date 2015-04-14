using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour 
{
	private MessageController_JnS messageController;
	private GameController_JnS controller;
	private UIManager_JnS ui;
	private bool run;
	private bool run1;

	private string[] startingMessages = 
	{
		"Connecting to archive.orache.edu...",
		"Connected",
		"Deploying Kosha-type3..."
	};

	private string[] winMessages = 
	{
		"Purging logs... Complete",
		"Disconnecting from host..."
	};

	void Awake()
	{
		messageController = GameObject.FindGameObjectWithTag("MessageController").GetComponent<MessageController_JnS>();
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_JnS>();
		ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();
		run = true;
		run1 = true;
	
	}

	void Update()
	{
		if(run)
		{
			ui.CoverLevel = true;
			messageController.StartMessageSequence(startingMessages,
				MessageController_JnS.MessageType.Beginning,
				MessageController_JnS.MessageOptions.DoOpposite,
				MessageController_JnS.MessageOptions.Do);

			run = false;
		}

		if(run1 &&
			controller.State == GameController_JnS.GameState.Over)
		{
			ui.CoverLevel = true;
			messageController.StartMessageSequence(winMessages,
				MessageController_JnS.MessageType.Ending,
				MessageController_JnS.MessageOptions.DoOpposite);
			run1 = false;
		}
	}
}
