if (!isObject($DailiesDialogueSet))
{
	$DailiesDialogueSet = new SimSet(DailiesDialogueSet);
}
$DailiesDialogueSet.deleteAll();

$obj = new ScriptObject(DailiesDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	functionOnStart = "setupDailiesDialogue";

	dialogueTransitionOnTimeout = "DailiesDialogueCore";

	botTalkAnim = 1;
};
$DailiesDialogueSet.add($obj);

$obj = new ScriptObject(DailiesDialogueIncomplete)
{
	messageCount = 1;
	message[0] = "I give out rewards for completing daily requests! Here's todays daily request!";
	messageTimeout[0] = 1.5;
	functionOnStart = "displayDailyProgress";

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(DailiesDialogueCompleteReward)
{
	messageCount = 1;
	message[0] = "Thanks for completing today's daily request! Here's your reward!";
	messageTimeout[0] = 1.5;

	botTalkAnim = 1;
	functionOnStart = "grantDailyReward";
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(DailiesDialogueCompleteRewardClaimed)
{
	messageCount = 1;
	message[0] = "Thanks for completing today's daily request! Come back tomorrow for another one!";
	messageTimeout[0] = 1.5;
	message[1] = "Daily requests reset at midnight EDT. The current time is %currTime%.";
	messageTimeout[1] = 1.5;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ShopDialogueSet.add($obj);



function setupDailiesDialogue(%dataObj)
{
	%player = %dataObj.player;
	%manager = %dataObj.speaker;

	if (!isObject(%client = %player.client))
	{
		return 0;
	}

	%dataObj.var_currTime = getWord(getDateTime(), 1);

	%hasCompletedDaily = checkDailyGoalProgress(%client);
	%hasClaimedDailyReward = hasClaimedDailyReward(%client);

	if (!%hasCompletedDaily)
	{
		%manager.startDialogue("DailiesDialogueIncomplete", %client);
	}
	else if (%hasCompletedDaily && !%hasClaimedDailyReward)
	{
		%manager.startDialogue("DailiesDialogueCompleteReward", %client);
	}
	else if (%hasClaimedDailyReward)
	{
		%manager.startDialogue("DailiesDialogueCompleteRewardClaimed", %client);
	}

	return 1; //redirected into different dialogue object, skip this dialogue
}
