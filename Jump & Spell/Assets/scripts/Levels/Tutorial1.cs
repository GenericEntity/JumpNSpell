using UnityEngine;
using System.Collections;

public class Tutorial1 : MonoBehaviour
{
	private MessageController_JnS msgController;
	private GameController_JnS controller;
	private UIManager_JnS ui;
	[SerializeField]
	private GameObject levelCover;

	bool run;
	int counter;

	private string[] introMessage =
	{
		"SYSTEM: Incoming message. Press F to read, or SPACE to skip.",
		
		"shad0whawk: Ok nerd heres ur dumb wurm. If any1 asks, you didnt get it from me, you got it from a hobo selling warez under his trenchcoat.",
		"shad0whawk: This hobo who passed it to me (who is totally not my cousin) wrote \"a n00b friendly GUI\" just for u so u can see whats going on when u get ur mad tite hax0r hat on.",
		"shad0whawk: Thats the testing environment u shuld be seeing on ur screen soon - that square is teh wurm, and the letterz r bits of code it finds lying around in the network of ur hax victim.",
		"shad0whawk: Now in the process of hax0ring, the wurm needs to keep changing and redisgiusing its signature with those bits of code like a snake doing that creepy thing with its skin.",
		"shad0whawk: Or else it gets owned by antivirus software which shuts down ur wurm if it manages to trace teh old signatures.",
		"shad0whawk: So im told, its all nerd speak 2 me.",
		"shad0whawk: Anyways, u have to control the wuem and make it copy code (u see them as letters 2 pickup) to form new signatures (words).",
		"shad0whawk: Thats all I know, and try not to destroy ur own comp when using it lol.",
		"shad0whawk: AND DONT FORGET TO CHANGE MY GRADES ON THE SCHOOL MAINFRAME OR WHATEVER U CALL IT AS WELL NERD ONcE UR DONE TESTING IT HERE. KTHXBYE",

		"SYSTEM: Message ended.",
		"SYSTEM: Loaded GenTest VM v1.0.201.",
		"TEST_ENV: Loading HUD..."
	};

	private string[] tutMsg1 = 
	{
		"TEST_ENV: Welcome to GenTest. Press F to view instructions, or SPACE to skip.",
		"TEST_ENV: Hello, I'm the guy who made this GUI. Instead of confusing you with technical jargon about what is really going on, I'll explain this as if it were all a game.",
		"TEST_ENV: The following are the default controls. If you changed them earlier, then I hope you knew what you were doing.",
		"TEST_ENV: The ARROW KEYS (left and right) move the worm, and the SPACEBAR makes it 'jump'.",
		"TEST_ENV: To see the layout of the entire system you're infecting, hold down TAB. You won't be able to move while doing so.",
	};

	private string[] tutMsg2 = 
	{
		"TEST_ENV: Your score display should have just loaded. Your score increases with every correct letter collected, with a bonus per word depending on its length.",
		"TEST_ENV: A wrong letter costs you some points.",
		"TEST_ENV: The score indicates how pro you are at this 'game'. Apart from that, it serves no other purpose."
	};

	private string[] tutMsg3 = 
	{
		"TEST_ENV: The status display should have loaded. This display shows your goal and progress in collecting a word.",
		"TEST_ENV: Whenever you collect a letter (correct or wrong), or complete a word, you should see some indication here.",
	};

	private string[] tutMsg4 = 
	{
		"TEST_ENV: For lack of a better name, I call this one the 'die-in timer'. Basically, it tells you how long you have before the anti-virus(AV) gets you.",
		"TEST_ENV: Collecting letters, and thereby changing how you appear to the AV, buys you some time."
	};

	private string[] tutMsg5 = 
	{
		"TEST_ENV: Think of this display as your 'progress bar'. When it fills up, the worm has completed its job on the system (whatever it may be) and can leave.",
		"TEST_ENV: An 'exit' of sorts should appear. When it does, get to it as soon as you can and press E (default) to leave the system and erase your tracks.",

		"TEST_ENV: And that's all. This is currently a testing environment simulating a crappy AV and a job which the worm can complete quickly.",
		"TEST_ENV: You can, and should, use this place to try out anything that you didn't understand in my hail of instructions so far.",
		"TEST_ENV: If you forgot anything, don't worry. Just remember: ARROW KEYS to move. SPACEBAR to 'jump'. TAB to view the system. E to exit.",
		"TEST_ENV: The exit should appear in a few seconds and then you can leave whenever you want.",
		"TEST_ENV: Have fun."
	};

	void Awake()
	{
		msgController = GameObject.FindGameObjectWithTag("MessageController").GetComponent<MessageController_JnS>();
		ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();
		run = true;
		counter = 0;
	}

	void Start()
	{
		ui.EnableHUD(false, true);
	}

	void Update()
	{
		if (run)
		{
			if (!ui.CoverLevel &&
				Input.GetKeyDown("space"))
			{
				ui.EnableHUD(true);
				run = false;
				return;
			}

			if(!msgController.DisplayingMessages)
			{
				switch(counter)
				{
					case 0:
						msgController.StartMessageSequence(introMessage);
						++counter;
						break;
					case 1:
						msgController.StartMessageSequence(tutMsg1);
						ui.CoverLevel = false;
						++counter;
						break;
					case 2:
						ui.DisplayScorePanel = true;
						msgController.StartMessageSequence(tutMsg2);
						++counter;
						break;
					case 3:
						ui.DisplayStatusPanel = true;
						msgController.StartMessageSequence(tutMsg3);
						++counter;
						break;
					case 4:
						ui.DisplayTimeLeftPanel = true;
						msgController.StartMessageSequence(tutMsg4);
						++counter;
						break;
					case 5:
						ui.DisplayTimeGoalPanel = true;
						msgController.StartMessageSequence(tutMsg5);
						++counter;
						run = false;
						break;
				}
				
			}
			
		}
	}
}
