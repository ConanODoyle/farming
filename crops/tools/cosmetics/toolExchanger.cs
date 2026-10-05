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

	functionOnStart = "setupToolExchanger";

	// dialogueTransitionOnTimeout = "ToolExchangerDialogueCore";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ToolExchangerDialogueCore)
{
	response["CanReskin"] = "ReskinConfirmation";
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
	message[0] = "You don't have enough Bux! Reskinning costs %reskinPrice% Bux.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ToolExchangerDialogueSet.add($obj);

$obj = new ScriptObject(ReskinConfirmation)
{
	response["Yes"] = "ReskinProduct";
	response["No"] = "ToolExchangerDialogueCore";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "It will cost %reskinPrice% Bux to reskin your %toolName%. Are you sure? Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
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


// $obj = new ScriptObject(RepairProduct)
// {
// 	messageCount = 1;
// 	message[0] = "I've repaired your %toolName%! Come again soon!";
// 	messageTimeout[0] = 1;

// 	botTalkAnim = 1;
// 	functionOnStart = "dialogue_RepairProduct";
// };
// $ToolExchangerDialogueSet.add($obj);













// function dialogue_RepairProduct(%dataObj)
// {
// 	%pl = %dataObj.player;
// 	%cl = %pl.client;

// 	if (%cl.checkMoney(%dataObj.var_repairPrice))
// 	{
// 		%cl.subMoney(%dataObj.var_repairPrice);
// 		for(%i = 0; %i < %dataObj.var_toolCount; %i++)
// 		{
// 			%toolDataID = %dataObj.var_toolDataID[%i];
// 			%maxDurability = getDataIDArrayTagValue(%toolDataID, "maxDurability");
// 			setDataIDArrayTagValue(%toolDataID, "durability", %maxDurability | 0);
// 		}
// 	}
// 	return 0;
// }

// function getRepairPrice(%itemDB, %durabilityLevel, %durabilityMax)
// {
// 	%basePrice = getBuyPrice(%itemDB);
// 	if (%basePrice >= 1000)
// 	{
// 		%variableFactor = 50;
// 	}
// 	else
// 	{
// 		%variableFactor = %basePrice / 20;
// 	}
// 	%flatFee = %basePrice / 100; //$10 for $1000 item
// 	%variableFee = mFloor(%variableFactor * ((%durabilityMax - %durabilityLevel) / %durabilityMax));
// 	%price = mFloor(%flatFee + %variableFee);
// 	return %price;
// }

function setupToolExchanger(%dataObj)
{
	%player = %dataObj.player;
	%dataObj.var_reskinPrice = $Farming::ReskinPrice;
	%dataObj.var_reskinRemovePrice = $Farming::ReskinRemovePrice;
	%exchanger = %dataObj.speaker;

	for (%i = 0; %i < %player.getDatablock().maxTools; %i++)
	{
		%tool = %player.tool[%i];
		if (isObject(%tool) && strstr(%tool.getName(), "Bux") > 0))
		{
			%hasBux = true;
			break;
		}
	}

	if (%hasBux)
	{
		%exchanger.startDialogue("ToolExchangerDialogueCore", %player.client);
	}
	else
	{
		%exchanger.startDialogue("ToolExchangerNoBux", %player.client);
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
			%currTool = %pl.tool;
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



	%price = getRepairPrice(%tool, %durability, %maxDurability);

	%dataObj.var_toolCount = %repairableToolCount;
	for (%i = 0; %i < %repairableToolCount; %i++)
	{
		%dataObj.var_tool[%i] = %repairableTool[%i];
		%dataObj.var_toolDataID[%i] = %repairableToolDataID[%i];
	}
	
	%repairMultipleTools = %toolCount > 1;
	if (%repairMultipleTools)
	{
		%dataObj.var_toolName = "tools";
		%dataObj.var_toolPlural = "don't";
	}
	else
	{
		%tool = %tool[0];
		%toolDataID = %toolDataID[0];
		%maxDurability = getDataIDArrayTagValue(%toolDataID, "maxDurability");
		%dataObj.var_toolName = %tool.uiName;
		%dataObj.var_maxDurability = %maxDurability;
		%dataObj.var_toolPlural = "doesn't";
	}

	%dataObj.var_repairPrice = %totalRepairPrice;


	if (!%pl.client.checkMoney(%totalRepairPrice))
	{
		return "InsufficientMoney";
	}
	else if (%pl.client.checkMoney(%totalRepairPrice))
	{
		if (%repairMultipleTools)
		{
			return "CanRepairMultiple";
		}
		else
		{
			return "CanRepair";
		}
	}
	return "Error";
}
