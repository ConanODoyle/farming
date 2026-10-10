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
	response["CanRemoveReskin"] = "RemoveReskinConfirmation";
	response["InsufficientMoney"] = "ReskinFail";
	response["RemoveInsufficientMoney"] = "RemoveReskinFail";
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

$obj = new ScriptObject(ReskinOptions)
{
	response["CanReskin"] = "ReskinConfirmation";
	response["BadOption"] = "ReskinOptionInvalid";
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

$obj = new ScriptObject(ReskinOptionInvalid)
{
	messageCount = 1;
	message[0] = "That isn't an option...";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ToolExchangerDialogueCore";
};

$obj = new ScriptObject(ReskinConfirmation)
{
	response["Yes"] = "ReskinProduct";
	response["No"] = "ToolExchangerDialogueCore";
	response["InsufficientMoney"] = "ReskinFail";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "It will cost" SPC $Farming::ReskinPrice SPC "Bux to reskin your %toolName% into %article% %toolReskinName%. Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoReskinPriceResponseParser";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ReskinProduct)
{
	messageCount = 1;
	message[0] = "I've reskinned your %toolName%! Come again soon!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_ReskinProduct";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(RemoveReskinConfirmation)
{
	response["Yes"] = "RemoveReskinProduct";
	response["No"] = "ToolExchangerDialogueCore";
	response["InsufficientMoney"] = "RemoveReskinFail";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "It will cost" SPC $Farming::ReskinRemovePrice SPC "Bux to return your %toolReskinName% into %article% %toolName%. Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoRemoveReskinPriceResponseParser";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(RemoveReskinFail)
{
	messageCount = 1;
	message[0] = "You don't have enough Bux! Reskin removals cost" SPC $Farming::ReskinRemovePrice SPC "Bux.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};

$obj = new ScriptObject(RemoveReskinProduct)
{
	messageCount = 1;
	message[0] = "I've returned your %toolName%! Come again soon!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_RemoveReskinProduct";
};

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
	%exchanger = %dataObj.speaker;

	%result = %pl.reskinItem(%dataObj.var_tool, %dataObj.var_toolDataID, %dataObj.var_toolReskin);
	if (%result == 1)
	{
		%pl.removeStackableItemTotal("Bux", $Farming::ReskinPrice);
		%exchanger.mountReskinPreview("", 0); // reattach base tool after some time
		%exchanger.setWeapon(-1);
	}
	else
	{
		error("	WARNING: dialogue_ReskinProduct input failure, BLID" SPC %cl.BL_ID);
		commandToClient(%cl, 'MessageBoxOK', "Item not found!", "Your item could not be found! Please ensure it remains in your inventory. ");
		messageClient(%cl, '', "The transaction was cancelled due to invalid data.");
	}
	
	return 0;
}

function dialogue_RemoveReskinProduct(%dataObj)
{
	%pl = %dataObj.player;
	%cl = %pl.client;

	%result = %pl.removeitemreskin(%dataObj.var_tool, %dataObj.var_toolDataID);
	if (%result == 1)
	{
		%pl.removeStackableItemTotal("Bux", $Farming::ReskinRemovePrice);	
	}
	else
	{
		error("	WARNING: dialogue_RemoveReskinProduct input failure, BLID" SPC %cl.BL_ID);
		commandToClient(%cl, 'MessageBoxOK', "Item not found!", "Your item could not be found! Please ensure it remains in your inventory. You have not been charged.");
		messageClient(%cl, '', "The transaction was cancelled due to invalid data.");
	}
	
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
	%exchanger = %dataObj.speaker;

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

	if (!isObject(%tool)
		|| !%tool.hasDataID || trim(%toolDataID) $= ""
		|| (%tool.skinBase $= "" && %tool.getReskinCount() < 1))
	{
		return "CannotReskin";
	}

	%dataObj.var_tool = %tool;
	%dataObj.var_toolDataID = %toolDataID;
	%dataObj.var_toolName = %tool.uiName;

	%isReskin =  %tool.skinBase !$= "";

	if (%isReskin)
	{
		if (%pl.hasAmountCurrency("Bux" SPC $Farming::ReskinRemovePrice))
		{
			%reskin = %tool;
			%tool = %reskin.skinBase;
			%dataObj.var_toolName = %tool.uiName;

			%dataObj.var_article = getProperArticle(%tool);
			%dataObj.var_toolReskin = %reskin;
			%dataObj.var_toolReskinName = %reskin.uiName;
			return "CanRemoveReskin";
		}
		else
		{
			return "RemoveInsufficientMoney";
		}
	}
	else if (!%pl.hasAmountCurrency("Bux" SPC $Farming::ReskinPrice))
	{
		return "InsufficientMoney";
	}

	if (%tool.getReskinCount() > 1)
	{
		%str = "";
		for (%i = 0; %i < getFieldCount(%tool.getReskinOptions()); %i++)
		{
			%reskin = getField(%tool.getReskinOptions(), %i);
			%str = %str @ ", " @ %i+1 @ ")" SPC %reskin.uiName;
		}
		%dataObj.var_toolReskinList = ltrim(strchr(%str, 1));
		return "CanReskinWithOptions";
	}
	else if (%tool.getReskinCount() == 1)
	{	
		%reskin = getField(%tool.getReskinOptions(), 0);
		%dataObj.var_article = getProperArticle(%reskin.uiName);
		%dataObj.var_toolReskin = %reskin;
		%dataObj.var_toolReskinName = %reskin.uiName;
		
		%exchanger.mountReskinPreview(%reskin);
		return "CanReskin";
	}

	return "Error";
}

function ReskinOptionsResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;
	// %dataObj.var_reskinPrice = $Farming::ReskinPrice;
	// %dataObj.var_reskinRemovePrice = $Farming::ReskinRemovePrice;
	%exchanger = %dataObj.speaker;

	%tool = %dataObj.var_tool;
	%optionCount = %tool.getReskinCount();
	%reskinOptions = %tool.getReskinOptions();

	%choiceIdx = -1;

	if (!isNumber(%msg))
	{
		%choice = strLwr(%msg);
		for (%i = 0; %i < getFieldCount(%reskinOptions); %i++)
		{
			%reskin = getField(%reskinOptions, %i);
			if (strPos(strLwr(%reskin.uiName), %choice) >= 0)
			{
				%choiceIdx = %i;
				break;
			}
		}

		// no matching string
		if (%choiceIdx == -1)
		{
			return "BadOption";
		}
	}
	else if (%msg > %optionCount || %msg < 1)
	{
		return "BadOption";
	}
	else
	{
		%choiceIdx = %msg - 1; // Displayed index starts at 1
	}
	%choiceReskin = getField(%reskinOptions, %choiceIdx);
	%dataObj.var_toolReskin = %choiceReskin;
	%dataObj.var_toolReskinName = %choiceReskin.uiName;

	%dataObj.var_article = getProperArticle(%choiceReskin.uiName);

	%exchanger.mountReskinPreview(%choiceReskin);
	return "CanReskin";
}

function yesNoReskinPriceResponseParser(%dataObj, %msg)
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
			if (!%pl.hasAmountCurrency("Bux" SPC $Farming::ReskinPrice))
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

function yesNoRemoveReskinPriceResponseParser(%dataObj, %msg)
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
			if (!%pl.hasAmountCurrency("Bux" SPC $Farming::ReskinRemovePrice))
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

// Make bot hold the item for buyer preview
function AIPlayer::mountReskinPreview(%bot, %item)
{
	if (isEventPending(%bot.mountSchedule))
	{
		cancel(%bot.mountSchedule);
	}

	%bot.setWeapon(%item);
	%bot.mountSchedule = %bot.schedule(10000, setWeapon, UpgradeToolItem);
}