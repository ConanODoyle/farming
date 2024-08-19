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

$obj = new ScriptObject(DailiesDialogueCore)
{
	messageCount = 1;
	message[0] = "I give out rewards for completing daily quests! Here's todays daily quest!";
	messageTimeout[0] = 1;

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


	%dataObj.var_price = $Farming::LotUnloadPrice;

	%type = hasLoadedLot(%player.client.BL_ID);
	if (%type == 1) %type = "hasLoadedLot";
	else if (%type $= "0") %type = "noLot";

	switch$ (%type)
	{
		case "noSavedLot": %manager.startDialogue("IntroLotDialogue", %player.client);t
		case "noLot": %manager.startDialogue("LotUnloadedDialogue", %player.client);
		case "hasLoadedLot": %manager.startDialogue("LotPresentDialogue", %player.client);
	}

	return 1; //redirected into different dialogue object BUT still want the text
}
