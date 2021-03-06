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
	message[0] = "Your %toolName% doesn't need repairs...?";
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

	if (%cl.score >= %dataObj.var_total)
	{
		%cl.setScore(%cl.score - %dataObj.var_repairPrice);
		%toolDataID = %dataObj.var_toolDataID;
		setDataIDArrayTagValue(%toolDataID, "durability", %dataObj.var_maxDurability | 0);
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
	%product = %dataObj.sellItem;

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

	if (!isObject(%tool) || getDataIDArrayTagValue(%toolDataID, "maxDurability") <= 0
		|| !%tool.hasDataID || trim(%toolDataID) $= "")
	{
		return "CannotRepair";
	}

	%durabilty = getDataIDArrayTagValue(%toolDataID, "durability");
	%maxDurability = getDataIDArrayTagValue(%toolDataID, "maxDurability");
	%price = getRepairPrice(%tool, %durability, %maxDurability);

	%dataObj.var_tool = %tool;
	%dataObj.var_toolDataID = %toolDataID;
	%dataObj.var_toolName = %tool.uiName;
	%dataObj.var_repairPrice = %price;
	%dataObj.var_maxDurability = %maxDurability;

	if (%tool $= "")
	{
		return "Error";
	}
	else if (%maxDurability == %durability)
	{
		return "FullDurability";
	}
	else if (%pl.client.score < %price)
	{
		return "InsufficientMoney";
	}
	else if (%pl.client.score >= %price)
	{
		return "CanRepair";
	}
	return "Error";
}