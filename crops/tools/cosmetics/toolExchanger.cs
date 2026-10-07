$Farming::ReskinPrice = 40; // in bux
$Farming::ReskinRemovePrice = 5;

if (!isObject($ToolExchangerDialogueSet))
{
	$ToolExchangerDialogueSet = new SimSet(ToolExchangerDialogueSet);
}
$ToolExchangerDialogueSet.deleteAll();

$obj = new ScriptObject(ToolExchangerDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello! I can reskin your tools!";
	messageTimeout[0] = 1;

	dialogueTransitionOnTimeout = "ToolExchangerDialogueHandler";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ToolExchangerDialogueHandler)
{
	response["Quit"] = "ExitResponse";
	messageCount = 0;
	functionOnStart = "setupToolExchanger";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ToolExchangerNoBux)
{
	messageCount = 2;
	message[0] = "Reskins cost" SPC $Farming::ReskinPrice SPC "Bux! Come back when you have Bux to show me!";
	messageTimeout[0] = 1;
	message[1] = "You can get Bux by delivering daily quests to the Dailies Manager!";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};

$obj = new ScriptObject(ToolExchangerDialogueCore)
{
	response["CanReskin"] = "ReskinConfirmation";
	response["CanReskinWithOptions"] = "ReskinOptions";
	response["InsufficientMoney"] = "ReskinFail";
	response["CannotReskin"] = "ReskinInvalid";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 2;
	message[0] = "Which tool would you like me to reskin?";
	messageTimeout[0] = 1;
	message[1] = "Say the name, slot number (first slot is 1), or 'current tool' if you're holding it.";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "ReskinResponseParser";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ReskinFail)
{
	messageCount = 1;
	message[0] = "You don't have enough Bux! Reskinning costs" SPC $Farming::ReskinPrice SPC "Bux.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ReskinOptions) // todo
{
	response["CanReskin"] = "ReskinConfirmation";
	response["CannotReskin"] = "ReskinInvalid";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 3;
	message[0] = "%toolName% has multiple reskin options:";
	messageTimeout[0] = 1;
	message[1] = "%toolReskinList%";
	messageTimeout[1] = 1;
	message[2] = "Which one would you like to reskin to? Say the name or number of the reskin.";
	messageTimeout[2] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "ReskinOptionsResponseParser";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ReskinConfirmation)
{
	response["Yes"] = "ReskinProduct";
	response["No"] = "ToolExchangerDialogueCore";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "It will cost" SPC $Farming::ReskinPrice SPC "Bux to reskin your %toolName% into %article% %toolReskinName%. Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ReskinProduct) // todo need a handler for tix subtraction at moment of reskin
{
	messageCount = 1;
	message[0] = "I've reskinned your %toolName%! Come again soon!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_ReskinProduct";
};
$ToolExchangerDialogueSet.add($obj);

// $obj = new ScriptObject(RepairConfirmationMultiple)
// {
// 	response["Yes"] = "RepairProduct";
// 	response["No"] = "ToolExchangerDialogueCore";
// 	response["Quit"] = "ExitResponse";
// 	response["Error"] = "ErrorResponse";

// 	messageCount = 1;
// 	message[0] = "It will cost $%repairPrice% to repair all of your tools. Are you sure? Say yes to confirm.";
// 	messageTimeout[0] = 1;

// 	botTalkAnim = 1;
// 	waitForResponse = 1;
// 	responseParser = "yesNoResponseParser";
// };
// $ToolExchangerDialogueSet.add($obj);


$obj = new ScriptObject(ReskinInvalid)
{
	messageCount = 1;
	message[0] = "I can't reskin that...";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ToolExchangerDialogueSet.add($obj);











function dialogue_ReskinProduct(%dataObj)
{
	%pl = %dataObj.player;
	%cl = %pl.client;

	%cl.messageBoxOKLong("SAMPLE TEXT", "reskin goes hereS" NL "" NL "AMPLE TEXT");
	
	return 0;
}

function setupToolExchanger(%dataObj)
{
	%pl = %dataObj.player;
	// %dataObj.var_reskinPrice = $Farming::ReskinPrice;
	// %dataObj.var_reskinRemovePrice = $Farming::ReskinRemovePrice;
	%exchanger = %dataObj.speaker;

	%hasBux = %pl.hasAmountCurrency("Bux 1");

	if (%hasBux)
	{
		%exchanger.startDialogue("ToolExchangerDialogueCore", %pl.client);
	}
	else
	{
		%exchanger.startDialogue("ToolExchangerNoBux", %pl.client);
	}

	return 1;
}

function ReskinResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;

	if (%msg > 0)
	{
		%tool = %pl.tool[%msg - 1];
		%toolDataID = %pl.toolDataID[%msg - 1];
	}
	else if (%msg $= "current tool")
	{
		%tool = %pl.tool[%pl.currTool];	
		%toolDataID = %pl.toolDataID[%pl.currTool];
	}
	else
	{
		%msg = strLwr(%msg);
		for (%i = 0; %i < %pl.getDatablock().maxTools; %i++)
		{
			%currTool = %pl.tool[%i];
			if (strPos(strLwr(%currTool.uiName), %msg) >= 0)
			{
				%tool = %currTool;
				%toolDataID = %pl.toolDataID[%i];
				break;
			}
		}
	}

	

	if (!isObject(%tool) || %tool.getNumReskins() < 1
		|| !%tool.hasDataID || trim(%toolDataID) $= "")
	{
		return "CannotReskin";
	}

	%dataObj.var_toolName = %tool.uiName;

	if (%tool.hasSkin) // todo proper skin check
	{
		if (%pl.hasAmountCurrency("Bux" SPC $Farming::ReskinRemovePrice))
		{
			return "CanRemoveSkin";
		}
		else
		{
			return "InsufficientMoneySkinRemove";  //  todo add this
		}
	}
	else if (!%pl.hasAmountCurrency("Bux" SPC $Farming::ReskinPrice))
	{
		return "InsufficientMoney";
	}

	if (%tool.getNumReskins() > 1)
	{
		%str = "";
		for (%i = 0; %i < getFieldCount(%tool.getReskinOptions()); %i++)
		{
			%cosmetic = getField(%tool.getReskinOptions(), %i);
			%str = %str @ ", " @ %i+1 @ ")" SPC %cosmetic.uiName;
		}
		%dataObj.var_toolReskinList = ltrim(strchr(%str, 1));
		return "CanReskinWithOptions";
	}
	else if (%tool.getNumReskins() == 1)
	{	
		%reskin = getField(%tool.getReskinOptions(), 0);
		if (isVowel(getSubStr(%reskin.uiName, 0, 1)))
		{
			%dataObj.var_article = "an";
		}
		else
		{
			%dataObj.var_article = "a";
		}
		%dataObj.var_toolReskin = %reskin;
		%dataObj.var_toolReskinName = %reskin.uiName;
		return "CanReskin";
	}

	return "Error";
}

function isVowel(%letter)
{
	%letter = strLwr(%letter);
	if (%letter $= "a" || %letter $= "e" || %letter $= "i" || %letter $= "o" || %letter $= "u")
	{
		return 1;
	}
	return 0;
}
