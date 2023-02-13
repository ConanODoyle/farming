if(!isObject($WandererDialogueSet))
{
	$WandererDialogueSet = new SimSet(WandererDialogueSet);
}
$WandererDialogueSet.deleteAll();

//------------------//
// No Key Dialogue: //
//------------------//

$obj = new ScriptObject(WandererDialogue)
{
	functionOnStart = "dialogue_WandererKeyCheckRedirect";
	
	messageCount = 1;
	
	message[0] = "Hello! I'm a wandering archaeologist. I study the lay of the land.";
	messageTimeout[0] = 1;
};
$WandererDialogueSet.add($obj);

//--------------------------//
// Initial Repair Dialogue: //
//--------------------------//

$obj = new ScriptObject(WandererRedirectDialogue)
{
	maxRange = 18;
	
	messageCount = 1;
	
	message[0] = "Hello! I'm a wandering archaeologist... say, that key you have...";
	messageTimeout[0] = 3;
	
	dialogueTransitionOnTimeout = "WandererRepairInitialDialogue";
};
$WandererDialogueSet.add($obj);

$obj = new ScriptObject(WandererRepairInitialDialogue)
{
	response["Yes"] = "WandererRepairPriceDialogue";
	response["No"] = "ExitResponse";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";
	
	responseParser = "wandererInitialResponseParser";
	waitForResponse = true;
	maxRange = 18;

	messageCount = 3;
	
	message[0] = "That's an ancient relic you've found! However, it doesn't look usable in it's current state...";
	messageTimeout[0] = 3;
	message[1] = "Fortunately, I know how to repair your key... for a small price, of course.";
	messageTimeout[1] = 3;
	message[2] = "Well, what do you say? Say yes to confirm.";
	messageTimeout[2] = 1;
	
	botTalkAnim = true;
};
$WandererDialogueSet.add($obj);

//-------------------------------//
// Repair Confirmation Dialogue: //
//-------------------------------//

$obj = new ScriptObject(WandererRepairPriceDialogue)
{
	response["Repair"] = "WandererRepairKeyDialogue";
	response["RequirementsNotMet"] = "WandererRepairRequirementsNotMet";
	response["No"] = "ExitResponse";
	response["Error"] = "ErrorResponse";
	
	responseParser = "wandererRepairResponseParser";
	waitForResponse = true;
	maxRange = 18;
	
	messageCount = 1;
	
	message[0] = "I think it will take, hnmmm, $%keyRepairPrice% and %keyRepairEXPPrice% XP to repair your key. Say yes to confirm.";
	messageTimeout[0] = 1;
	
	botTalkAnim = true;
};
$WandererDialogueSet.add($obj);

//------------------//
// Repair Dialogue: //
//------------------//

$obj = new ScriptObject(WandererRepairKeyDialogue)
{
	functionOnStart = "dialogue_WandererRepairKey";
	
	maxRange = 18;
	
	messageCount = 2;
	
	message[0] = "There we go! Your key is as good as new.";
	messageTimeout[0] = 2;
	message[1] = "Although, I must ask... what do you plan to do with such an ancient artifact?";
	messageTimeout[1] = 1;
	
	botTalkAnim = true;
};
$WandererDialogueSet.add($obj);

//--------------------------------//
// Requirements Not Met Dialogue: //
//--------------------------------//

$obj = new ScriptObject(WandererRepairRequirementsNotMet)
{
	messageCount = 1;
	
	message[0] = "Hmm... it seems that you aren't quite ready.";
	messageTimeout[0] = 1;
	
	botTalkAnim = true;
};
$WandererDialogueSet.add($obj);

//--------------------------------//
// Key Missing Dialogue: //
//--------------------------------//

$obj = new ScriptObject(WandererKeyMissingDialogue)
{
	messageCount = 1;
	
	message[0] = "Hmm... where did your key go?";
	messageTimeout[0] = 1;
	
	botTalkAnim = true;
};
$WandererDialogueSet.add($obj);

//--------------------------//
// Initial Response Parser: //
//--------------------------//

function wandererInitialResponseParser(%dataObj, %message)
{
	%player = %dataObj.player;
	
	// Yes/no handling taken from yesNoResponseParser.
	%lwr = " " @ strLwr(%message) @ " ";
	%lwr = stripChars(%lwr, "!@#$%^&*()[];,.<>/?[]{}\\|-_=+");
	%yes = "yes\tyeah\tye\tyea\ty\tok\talright\ti guess\tig";
	%no = "no\tn\tnope\tcancel\tquit\tfuck off";

	for (%i = 0; %i < getFieldCount(%yes); %i++)
	{
		%word = " " @ getField(%yes, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			%dataObj.var_keyRepairPrice = 500;
			%dataObj.var_keyRepairEXPPrice = 1000;
			
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

	return "Error";
}

//-------------------------//
// Repair Response Parser: //
//-------------------------//

function wandererRepairResponseParser(%dataObj, %message)
{
	%player = %dataObj.player;

	// Yes/no handling taken from yesNoResponseParser.
	%lwr = " " @ strLwr(%message) @ " ";
	%lwr = stripChars(%lwr, "!@#$%^&*()[];,.<>/?[]{}\\|-_=+");
	%yes = "yes\tyeah\tye\tyea\ty\tok\talright\ti guess\tig";
	%no = "no\tn\tnope\tcancel\tquit\tfuck off";

	for (%i = 0; %i < getFieldCount(%yes); %i++)
	{
		%word = " " @ getField(%yes, %i) @ " ";
		if (strPos(%lwr, %word) >= 0)
		{
			%client = %player.client;
			if(!%client.checkMoney(%dataObj.var_keyRepairPrice) || %client.farmingExperience < %dataObj.var_keyRepairEXPPrice)
			{
				return "RequirementsNotMet";
			}
			
			return "Repair";
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

	return "Error";
}

//-------------------------//
// Repair Redirect Script: //
//-------------------------//

function dialogue_WandererKeyCheckRedirect(%dataObj)
{
	if (%dataObj.player.hasItem(VoidEncasedKeyItem))
	{
		%dataObj.enterDialogue("WandererRedirectDialogue");
	}
}

//----------------//
// Repair Script: //
//----------------//

function dialogue_WandererRepairKey(%dataObj)
{
	if (%dataObj.player.hasItem(VoidEncasedKeyItem))
	{
		%pl = %dataObj.player;

		for (%i = 0; %i < %pl.dataBlock.maxTools; %i++)
		{
			if (%pl.tool[%i].getID() == VoidEncasedKeyItem.getID())
			{
				%slot = %i;
				break;
			}
		}

		%pl.tool[%slot] = VoidKeyItem.getID();
		messageClient(%pl.client, 'MsgItemPickup', "", %slot, VoidKeyItem.getID());
		serverCmdUseTool(%cl, %slot);

		%cl.subMoney(%dataObj.var_keyRepairPrice);
		%cl.addExperience(%dataObj.var_keyRepairEXPPrice * -1);
	}
	else
	{
		%dataObj.enterDialogue("WandererKeyMissingDialogue");
	}
}