if (!isObject($RepairDialogueSet))
{
	$RepairDialogueSet = new SimSet(RepairDialogueSet);
}
$RepairDialogueSet.deleteAll();

$obj = new ScriptObject(RepairDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello! I can repair your tools!";
	messageTimeout[0] = 1;

	dialogueTransitionOnTimeout = "RepairDialogueCore";
};
$RepairDialogueSet.add($obj);

$obj = new ScriptObject(RepairDialogueCore)
{
	response["CanRepair"] = "RepairConfirmation";
	response["CanRepairMultiple"] = "RepairConfirmationMultiple";
	response["InsufficientMoney"] = "RepairFail";
	response["FullDurability"] = "RepairUnneeded";
	response["CannotRepair"] = "RepairInvalid";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 2;
	message[0] = "Which tool would you like me to repair?";
	messageTimeout[0] = 1;
	message[1] = "Say the name, slot number (first slot is 1), or 'current tool' if you're holding it.";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "RepairResponseParser";
};
$RepairDialogueSet.add($obj);

$obj = new ScriptObject(RepairFail)
{
	messageCount = 1;
	message[0] = "You don't have enough money! Repairing %toolName% to full costs $%repairPrice%.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "RepairDialogueCore";
};
$RepairDialogueSet.add($obj);

$obj = new ScriptObject(RepairConfirmation)
{
	response["Yes"] = "RepairProduct";
	response["No"] = "RepairDialogueCore";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "It will cost $%repairPrice% to repair your %toolName% to %maxDurability%. Are you sure? Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};
$RepairDialogueSet.add($obj);

$obj = new ScriptObject(RepairConfirmationMultiple)
{
	response["Yes"] = "RepairProduct";
	response["No"] = "RepairDialogueCore";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "It will cost $%repairPrice% to repair all of your tools. Are you sure? Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};
$RepairDialogueSet.add($obj);


$obj = new ScriptObject(RepairInvalid)
{
	messageCount = 1;
	message[0] = "I can't repair that...";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$RepairDialogueSet.add($obj);

$obj = new ScriptObject(RepairUnneeded)
{
	messageCount = 1;
	message[0] = "Your %toolName% %toolPlural% need repairs...?";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "RepairDialogueCore";
};
$RepairDialogueSet.add($obj);

$obj = new ScriptObject(RepairProduct)
{
	messageCount = 1;
	message[0] = "I've repaired your %toolName%! Come again soon!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_RepairProduct";
};
$RepairDialogueSet.add($obj);













function dialogue_RepairProduct(%dataObj)
{
	%pl = %dataObj.player;
	%cl = %pl.client;

	if (%cl.checkMoney(%dataObj.var_repairPrice))
	{
		%cl.subMoney(%dataObj.var_repairPrice);
		for(%i = 0; %i < %dataObj.var_toolCount; %i++)
		{
			%toolDataID = %dataObj.var_toolDataID[%i];
			%maxDurability = getDataIDArrayTagValue(%toolDataID, "maxDurability");
			setDataIDArrayTagValue(%toolDataID, "durability", %maxDurability | 0);
		}
	}
	return 0;
}

function getRepairPrice(%itemDB, %durabilityLevel, %durabilityMax)
{
	%basePrice = getBuyPrice(%itemDB);
	if (%basePrice >= 1000)
	{
		%variableFactor = 50;
	}
	else
	{
		%variableFactor = %basePrice / 20;
	}
	%flatFee = %basePrice / 100; //$10 for $1000 item
	%variableFee = mFloor(%variableFactor * ((%durabilityMax - %durabilityLevel) / %durabilityMax));
	%price = mFloor(%flatFee + %variableFee);
	return %price;
}

function RepairResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;

	%toolCount = 0;
	if (%msg > 0)
	{
		%tool[0] = %pl.tool[%msg - 1];
		%toolDataID[0] = %pl.toolDataID[%msg - 1];
		%toolCount = 1;
	}
	else if (%msg $= "current tool")
	{
		%tool[0] = %pl.tool[%pl.currTool];
		%toolDataID[0] = %pl.toolDataID[%pl.currTool];
		%toolCount = 1;
	}
	else if (%msg $= "all tools" || %msg $= "all")
	{
		%toolCount = %pl.getDatablock().maxTools;
		for (%i = 0; %i < %toolCount; %i++)
		{
			%currTool = %pl.tool[%i];
			%tool[%i] = %currTool;
			%toolDataID[%i] = %pl.toolDataID[%i];
		}
	}
	else
	{
		%msg = strLwr(%msg);
		for (%i = 0; %i < %pl.getDatablock().maxTools; %i++)
		{
			%currTool = %pl.tool[%i];
			if (strPos(strLwr(%currTool.uiName), %msg) >= 0)
			{
				%tool[0] = %currTool;
				%toolDataID[0] = %pl.toolDataID[%i];
				%toolCount = 1;
				break;
			}
		}
	}

	%repairableToolCount = 0;
	%totalRepairPrice = 0;
	%lastToolError = "Error";
	for(%i = 0; %i < %toolCount; %i++)
	{
		%tool = %tool[%i];
		%toolDataID = %toolDataID[%i];

		if (!isObject(%tool) || getDataIDArrayTagValue(%toolDataID, "maxDurability") <= 0
			|| !%tool.hasDataID || trim(%toolDataID) $= "")
		{
			%lastToolError = "CannotRepair";
			continue;
		}

		%durability = getDataIDArrayTagValue(%toolDataID, "durability");
		%maxDurability = getDataIDArrayTagValue(%toolDataID, "maxDurability");
		if (%durability == %maxDurability)
		{
			%lastToolError = "FullDurability";
			continue;
		}

		%price = getRepairPrice(%tool, %durability, %maxDurability);
		if (%tool.isRepairTool)
		{
			%price = %price * 10;
		}
		
		%totalRepairPrice += %price;

		%repairableTool[%repairableToolCount] = %tool;
		%repairableToolDataID[%repairableToolCount] = %toolDataID;
		%repairableToolCount++;
	}

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

	if (%repairableToolCount == 0)
	{
		return %lastToolError;
	}

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
