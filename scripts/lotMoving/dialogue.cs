if (!isObject($LotManageDialogueSet))
{
	$LotManageDialogueSet = new SimSet(LotManageDialogueSet);
}
$LotManageDialogueSet.deleteAll();

$obj = new ScriptObject(LotManageDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 0;
	functionOnStart = "setupLotManagement";
};
$LotManageDialogueSet.add($obj);


//no lot, no lot unloaded -> newbie
$obj = new ScriptObject(IntroLotDialogue)
{
	response["Quit"] = "ExitResponse";
	response["Yes"] = "LotTeleportConfirmed";
	response["No"] = "ExitResponse";//"LotManagerExit";
	response["Exit"] = "ExitResponse";//"LotManagerExit";

	messageCount = 4;
	message[0] = "Hello, I'm the Lot Manager! Welcome to Town Hall!";
	messageTimeout[0] = 2;
	message[1] = "I notice you have no lot - you can buy your first lot free. Talk to bots around to get info on game mechanics!";
	messageTimeout[1] = 2;
	message[2] = "To buy a lot, use /buylot while standing over an unowned red Center Lot! You can see who owns it in the bottomprint.";
	messageTimeout[2] = 3;
	message[3] = "Would you like help finding a lot to purchase? I can teleport you to an available free lot.";
	messageTimeout[3] = 3;
	functionOnStart = "setupLotPrice";

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};
$LotManageDialogueSet.add($obj);

$obj = new ScriptObject(LotTeleportConfirmed)
{
	messageCount = 1;
	message[0] = "Got it! I'll find a lot with a free spot and send you there now. One moment please...";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "LotManageLoadSent";
};
$LotManageDialogueSet.add($obj);

$obj = new ScriptObject(LotManageLoadSent)
{
	messageCount = 1;
	message[0] = "Here's the place! If you need more help, come back and let me know!";
	messageTimeout[0] = 1;
	functionOnStart = "sendPlayerToFreeLotLocation";

	dialogueTransitionOnTimeout = "ExitResponse";
};
$LotManageDialogueSet.add($obj);

//has lot saved
$obj = new ScriptObject(LotUnloadedDialogue)
{
	response["Quit"] = "ExitResponse";
	response["Yes"] = "LotTeleportConfirmed";
	response["No"] = "ExitResponse";//"LotManagerExit";
	response["Exit"] = "ExitResponse";//"LotManagerExit";

	messageCount = 4;
	message[0] = "Hello, I'm the Lot Manager! Welcome to Town Hall!";
	messageTimeout[0] = 2;
	message[1] = "I notice that you've got an unloaded lot! To load it, find an unclaimed center lot, then do /loadLot to load it!";
	messageTimeout[1] = 2;
	message[2] = "You can also rotate a loaded lot by doing /rotatelot # while having a loaded lot.";
	messageTimeout[2] = 2;
	message[3] = "Would you like help finding a lot to load onto? I can teleport you to an available free lot.";
	messageTimeout[3] = 3;
	functionOnStart = "setupLotPrice";

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};
$LotManageDialogueSet.add($obj);


//has a lot
$obj = new ScriptObject(LotPresentDialogue)
{
	response["Quit"] = "ExitResponse";
	response["InsufficientMoney"] = "LotManageUnloadInsufficientMoney";
	response["Yes"] = "LotManageUnloadConfirmed";
	response["No"] = "ExitResponse";//"LotManagerExit";
	response["Exit"] = "ExitResponse";//"LotManagerExit";

	messageCount = 4;
	message[0] = "Hello, I'm the Lot Manager! Welcome to Town Hall!";
	messageTimeout[0] = 2;
	message[1] = "I notice that you've already got a lot! If you'd like to rotate it, do /rotatelot # - theres a small fee.";
	messageTimeout[1] = 2;
	message[2] = "You can move if you would like, by unloading your lot for a fee and loading it somewhere else. Loading a lot is always free!.";
	messageTimeout[2] = 2;
	message[3] = "Would you like to unload your lot? The fee is $%price%.";
	messageTimeout[3] = 3;
	functionOnStart = "setupLotPrice";

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoPriceResponseParser";
};
$LotManageDialogueSet.add($obj);

$obj = new ScriptObject(LotManageUnloadConfirmed)
{
	messageCount = 2;
	message[0] = "Okay, that's $%price% - I'll unload your lot right away!";
	messageTimeout[0] = 1;
	message[1] = "Talk to me if you'd like help with loading your lot again.";
	functionOnStart = "playerUnloadLot";

	dialogueTransitionOnTimeout = "ExitResponse";
};
$LotManageDialogueSet.add($obj);

$obj = new ScriptObject(LotManageUnloadInsufficientMoney)
{
	messageCount = 2;
	message[0] = "That'll be $%price%... oh, it looks like you don't have enough money.";
	messageTimeout[0] = 1;
	message[1] = "Come back later when you have enough money to unload your lot!";
	messageTimeout[1] = 1;

	dialogueTransitionOnTimeout = "ExitResponse";
};
$LotManageDialogueSet.add($obj);


function setupLotPrice(%dataObj)
{
	%dataObj.var_price = $Farming::LotUnloadPrice;
}

function setupLotManagement(%dataObj)
{
	%dataObj.var_price = $Farming::LotUnloadPrice;

	%player = %dataObj.player;
	%manager = %dataObj.speaker;

	%type = hasLoadedLot(%player.client.BL_ID);
	if (%type == 1) %type = "hasLoadedLot";
	else if (%type $= "0") %type = "noLot";

	switch$ (%type)
	{
		case "noSavedLot": %manager.startDialogue("IntroLotDialogue", %player.client);
		case "noLot": %manager.startDialogue("LotUnloadedDialogue", %player.client);
		case "hasLoadedLot": %manager.startDialogue("LotPresentDialogue", %player.client);
	}

	return 1; //redirected into different dialogue object BUT still want the text
}


function lotManageInitialParser(%dataObj, %msg)
{
	%lwr = " " @ strLwr(%msg) @ " ";
	%lwr = stripChars(%lwr, "!@#$%^&*()[];,.<>/?[]{}\\|-_=+");
	%unload = "unload";
	%load = "load";

	%pl = %dataObj.player;
	%cl = %pl.client;

	for (%i = 0; %i < getFieldCount(%unload); %i++)
	{
		%word = " " @ getField(%unload, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			if (hasLoadedLot(%cl.BL_ID))
			{
				return "UnloadOK";
			}
			else
			{
				return "UnloadBad";
			}
		}
	}

	for (%i = 0; %i < getFieldCount(%load); %i++)
	{
		%word = " " @ getField(%load, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			if (!hasLoadedLot(%cl.BL_ID))
			{
				return "LoadOK";
			}
			else
			{
				return "LoadBad";
			}
		}
	}

	return "";
}

function hasSavedLot(%bl_id)
{
	if (!isFile($Pref::Farming::LastLotAutosave[%bl_id]))
	{
		return isFile($Pref::Server::AS_["Directory"] @ $Pref::Farming::LastLotAutosave[%bl_id] @ ".bls");
	}
	return true;
}

function getLoadedLot(%bl_id)
{
	%bg = "Brickgroup_" @ %bl_id;

	if (isObject(%bg))
	{
		for (%i = 0; %i < getWordCount(%bg.lotList); %i++)
		{
			%b = getWord(%bg.lotList, %i);
			if (%b.getDatablock().isLot && %b.getDatablock().isSingle)
			{
				return %b;
			}
		}
	}
	return 0;
}

function hasLoadedLot(%bl_id)
{
	%lotExists = isObject(getLoadedLot(%bl_id));

	if (hasSavedLot(%bl_id)) //has a lot save, return lot value
	{
		return %lotExists;
	}
	else if (!%lotExists) //no file, no lot at all
	{
		return "noSavedLot";
	}
	else
	{
		return 1;
	}
}

function sendPlayerToFreeLotLocation(%dataObj)
{
	%pl = %dataObj.player;

	%count = Brickgroup_888888.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%b = Brickgroup_888888.getObject(%i);
		if (%b.getDatablock().isLot && %b.getDatablock().isSingle)
		{
			%found = %b;
			break;
		}
	}

	if (isObject(%found))
	{
		%pl.setTransform(%found.getSpawnPoint());
		%pl.spawnExplosion(spawnProjectile, 1);
		%pl.setWhiteout(1);
	}
}

function yesNoPriceResponseParser(%dataObj, %msg)
{
	%lwr = " " @ strLwr(%msg) @ " ";
	%lwr = stripChars(%lwr, "!@#$%^&*()[];,.<>/?[]{}\\|-_=+");
	%yes = "yes\tyeah\tye\tyea\ty\tok\talright\ti guess\tig\tsure";
	%no = "no\tn\tnope\tcancel\tquit\tfuck off";

	%pl = %dataObj.player;
	%cl = %pl.client;

	%price = %dataObj.var_price;

	for (%i = 0; %i < getFieldCount(%yes); %i++)
	{
		%word = " " @ getField(%yes, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			if (!%cl.checkMoney(%price))
			{
				return "InsufficientMoney";
			}

			return "Yes";
		}
	}

	for (%i = 0; %i < getFieldCount(%no); %i++)
	{
		%word = " " @ getField(%no, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			return "No";
		}
	}

	return "";
}

function playerUnloadLot(%dataObj)
{
	%pl = %dataObj.player;
	%cl = %pl.client;

	%cl.subMoney(%dataObj.var_price);

	unloadLot(%cl.BL_ID);
}