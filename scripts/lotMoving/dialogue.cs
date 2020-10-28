$Farming::LotUnloadPrice = 200;

$count = 0;
if (isObject($LotManageDialogue1))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($LotManageDialogue[%i]))
		{
			$LotManageDialogue[%i].delete();
		}
	}
}

$LotManageDialogue[%count++] = new ScriptObject(LotManageDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Welcome to City Hall! I'm the Lot Manager!";
	messageTimeout[0] = 1;
	functionOnStart = "setupLotManagement";

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "LotManagePrompt";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageErrorResponse : ErrorResponse)
{
	message[0] = "I'm sorry, I didn't understand you...";

	dialogueTransitionOnTimeout = "LotManagePrompt";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManagePrompt)
{
	response["Quit"] = "ExitResponse";
	response["Error"] = "LotManageErrorResponse";
	response["LoadOK"] = "LotManageLoad";
	response["LoadBad"] = "LotManageLoadFail";
	response["UnloadOK"] = "LotManageUnload";
	response["UnloadBad"] = "LotManageUnloadFail";

	messageCount = 2;
	message[0] = "I can help you load or unload your lot.";
	messageTimeout[0] = 1;
	message[1] = "Which would you like me to help you with? Load, or unload?";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "lotManageInitialParser";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageLoad)
{
	response["Yes"] = "LotManageLoadConfirmed";
	response["No"] = "LotManagePrompt";
	response["Quit"] = "ExitResponse";
	response["Error"] = "LotManageErrorResponse";

	messageCount = 2;
	message[0] = "If your lot isn't loaded, I can help you find a spot to load it back in.";
	messageTimeout[0] = 1;
	message[1] = "Would you like me to find a space for you?";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageLoadFail : LotManageLoad)
{
	message[1] = "...But it looks like you already have your lot loaded in.";

	waitForResponse = 0;
	responseParser = "";
	dialogueTransitionOnTimeout = "LotManageUnload";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageLoadConfirmed)
{
	messageCount = 1;
	message[0] = "Got it! I'll find a spot where you can load your lot in and send you there now. One moment please...";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "LotManageLoadSent";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageLoadSent)
{
	messageCount = 1;
	message[0] = "Here's the place! If you need more help, come back and let me know!";
	messageTimeout[0] = 1;
	functionOnStart = "sendPlayerToFreeLotLocation";

	dialogueTransitionOnTimeout = "ExitResponse";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageUnload)
{
	response["Yes"] = "LotManageUnloadConfirmed";
	response["InsufficientMoney"] = "LotManageUnloadInsufficientMoney";
	response["No"] = "LotManagePrompt";
	response["Quit"] = "ExitResponse";
	response["Error"] = "LotManageErrorResponse";

	messageCount = 2;
	message[0] = "If your lot is already loaded, I can have it unloaded so you can move for a small fee.";
	messageTimeout[0] = 1;
	message[1] = "Would you like me to have your lot unloaded? This will cost $%price%.";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoPriceResponseParser";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageUnloadFail : LotManageUnload)
{
	message[1] = "...But you don't have your lot loaded in yet.";

	waitForResponse = 0;
	responseParser = "";
	dialogueTransitionOnTimeout = "LotManageLoad";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageUnloadConfirmed)
{
	messageCount = 2;
	message[0] = "Okay, that's $%price% - I'll unload your lot right away!";
	messageTimeout[0] = 1;
	message[1] = "Let me know if you'd like to load your lot again.";
	functionOnStart = "playerUnloadLot";

	dialogueTransitionOnTimeout = "ExitResponse";
};

$LotManageDialogue[%count++] = new ScriptObject(LotManageUnloadInsufficientMoney)
{
	messageCount = 2;
	message[0] = "That'll be $%price%... oh, it looks like you don't have enough money.";
	messageTimeout[0] = 1;
	message[1] = "Come back later when you have enough money to unload your lot!";
	messageTimeout[1] = 1;

	dialogueTransitionOnTimeout = "ExitResponse";
};

function setupLotManagement(%dataObj)
{
	%dataObj.var_price = $Farming::LotUnloadPrice;
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
	return isFile("saves/Autosaver/" @ $Pref::Farming::LastLotAutosave[%bl_id] @ ".bls");
}

function hasLoadedLot(%bl_id)
{
	%bg = "Brickgroup_" @ %bl_id;
	
	%singleLot = 0;
	if (isObject(%bg))
	{
		for (%i = 0; %i < getWordCount(%bg.lotList); %i++)
		{
			%b = getWord(%bg.lotList, %i);
			if (%b.getDatablock().isLot && %b.getDatablock().isSingle)
			{
				%singleLot = 1;
				break;
			}
		}
	}

	if (hasSavedLot(%bl_id)) //has a lot save, return lot value
	{
		return %singleLot;
	}
	else if (!%singleLot) //no file, no lot at all
	{
		return 2;
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
			if (%cl.score < %price)
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

	%cl.score -= %dataObj.var_price;

	unloadLot(%cl.BL_ID);
}