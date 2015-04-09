using UnityEngine;
using System.Collections;

public class Tutorial1 : MonoBehaviour 
{
	private MessageController_JnS msgController;

	bool run;

	private string[] introductionMessages =
	{
		"Ok nerd heres ur dumb wurm. If any1 asks, you didnt get it from me, you got it from a hobo selling warez under his trenchcoat.",
		"This hobo who passed it to me (who is totally not my cousin) wrote \"a n00b friendly GUI\" just for u so u can see whats going on when u get ur mad tite hax0r hat on.",
		"Thats the testing environment u shuld be seeing on ur screen now - that square is teh wurm, and the letterz r bits of code it finds lying around in the network of ur hax victim.",
		"Now in the process of hax0ring, the wurm needs to keep changing and redisguising its signature with those bits of code like a snake doing whatever the hell it does with its skin.",
		"Or else it gets owned by antivirus software which shuts down ur wurm if it manages to trace teh old signatures.",
		"So I'm told, its all nerd speak 2 me.",
		"Anyways, the wurm *shuld* be smart enuff to string together the stray bits of code (letters) into convincing new signatures (which u see as words) to buy itself tiem 2 complete its hax.",
		"Thats all I know, and try not to destroy ur own comp when using it lol.",
		"AND DONT FORGET TO CHANGE MY GRADES ON THE SCHOOL MAINFRAME OR WHATEVER U CALL IT AS WELL NERD ONcE UR DONE TESTING IT HERE. KTHXBYE"
	};

	void Awake()
	{
		msgController = GameObject.FindGameObjectWithTag("MessageController").GetComponent<MessageController_JnS>();
		run = true;
	}

	void Update()
	{
		if(run)
		{
			msgController.StartMessageSequence(introductionMessages);
			run = false;
		}
	}
}
