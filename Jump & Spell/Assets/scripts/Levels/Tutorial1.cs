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

	private string[] introductionMessages =
	{
		"SYSTEM: Incoming message. Press F to continue...",
		
		"shad0whawk: Ok nerd heres ur dumb wurm. If any1 asks, you didnt get it from me, you got it from a hobo selling warez under his trenchcoat.",
		"shad0whawk: This hobo who passed it to me (who is totally not my cousin) wrote \"a n00b friendly GUI\" just for u so u can see whats going on when u get ur mad tite hax0r hat on.",
		"shad0whawk: Thats the testing environment u shuld be seeing on ur screen now - that square is teh wurm, and the letterz r bits of code it finds lying around in the network of ur hax victim.",
		"shad0whawk: Now in the process of hax0ring, the wurm needs to keep changing and redisguising its signature with those bits of code like that creepy thing that snakes do with ther skin.",
		"shad0whawk: Or else it gets owned by antivirus software which shuts down ur wurm if it manages to trace teh old signatures.",
		"shad0whawk: So I'm told, its all nerd speak 2 me.",
		"shad0whawk: Anyways, the wurm *shuld* be smart enuff to string together the stray bits of code (letters) into convincing new signatures (which u see as words) to buy itself tiem 2 complete its hax.",
		"shad0whawk: Thats all I know, and try not to destroy ur own comp when using it lol.",
		"shad0whawk: AND DONT FORGET TO CHANGE MY GRADES ON THE SCHOOL MAINFRAME OR WHATEVER U CALL IT AS WELL NERD ONcE UR DONE TESTING IT HERE. KTHXBYE",

		"SYSTEM: Message ended.",
		"SYSTEM: Loaded GenTest VM v1.0.201.",

		"TEST_ENV: Default commands to move are LEFT_ARROW, RIGHT_ARROW and SPACE.",
		"TEST_ENV: Default command to view directory is TAB.",
		"TEST_ENV: Default command to exit is E.",
		"TEST_ENV: Loading HUD..."
	};

	void Awake()
	{
		msgController = GameObject.FindGameObjectWithTag("MessageController").GetComponent<MessageController_JnS>();
		ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager_JnS>();
		run = true;
	}

	void Start()
	{
		ui.EnableHUD(false, true);
	}

	void Update()
	{
		if (run)
		{
			msgController.StartMessageSequence(introductionMessages);
			run = false;
		}
	}
}
