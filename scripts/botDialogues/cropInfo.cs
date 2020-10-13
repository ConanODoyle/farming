$count = 0;
if (isObject($CropInfo1))
{
	for (%i = 0; %i < 30; %i++)
	{
		if (isObject($CropInfo[%i]))
		{
			$CropInfo[%i].delete();
		}
	}
}

$CropInfo[$count++] = new ScriptObject(CropInfoStart)
{
	response["Quit"] = "ExitResponse";

	messageCount = 2
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	message[1] = "I can tell you information about any crop!";
	messageTimeout[1] = 2;

	dialogueTransitionOnTimeout = "CropInfoCore";
};

$CropInfo[$count++] = new ScriptObject(CropInfoCore)
{
	response["InvalidSelection"] = "CropInfoInvalidSelection";
	response["BasicInfo"] = "CropInfoSelection";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1
	message[0] = "What crop would you like to know more about?";
	messageTimeout[0] = 2;

	waitForResponse = 1;
	responseParser = "cropInfoSelectionParser";
};

$CropInfo[$count++] = new ScriptObject(CropInfoInvalidSelection)
{
	response["Quit"] = "ExitResponse";

	messageCount = 1
	message[1] = "I can't tell you anything about that, sorry...";
	messageTimeout[1] = 2;

	dialogueTransitionOnTimeout = "CropInfoCore";
};

$CropInfo[$count++] = new ScriptObject(CropInfoSelection)
{
	response["Quit"] = "ExitResponse";

	messageCount = 2
	message[0] = "%data1%";
	messageTimeout[1] = 3;
	message[1] = "%data2%";
	messageTimeout[1] = 3;

	functionOnStart = "setupCropInfo";
	dialogueTransitionOnTimeout = "CropInfoCore";
};







function cropInfoSelectionParser(%dataObj, %msg)
{
	%msg = trim(strLwr(%msg));
	if (strLen(%msg) < 3)
	{
		return "Error";
	}

	%list = strLwr($ProduceList);
	%first = "";
	%firstSlot = 1000;
	for (%i = 0; %i < getFieldCount(%list); %i++)
	{
		%produce = getField(%list, %i);
		%pos = strPos(%msg, %produce);
		if (%pos >= 0 && %pos < %firstSlot)
		{
			%first = %produce;
			%firstSlot = %pos;
		}
	}

	%dataObj.cropInfoSelection = %first;
	if (%first !$= "")
	{
		return "BasicInfo";
	}
	else
	{
		return "InvalidSelection";
	}
}

function setupPurchase(%dataObj)
{
	%pl = %dataObj.player;
	%seller = %dataObj.speaker;
	%crop = %dataObj.cropInfoSelection;
	
	switch$ (%crop)
	{
		case "Potato": %v1 = "var";
		case "Carrot":
		case "Onion":
		case "Turnip":
		case "Portobello":

		case "Tomato":
		case "Corn":
		case "Wheat":
		case "Cabbage":
		case "Blueberry":
		case "Chili":
		case "Watermelon":

		case "Cactus":
		case "Apple":
		case "Mango":
		case "Peach":
		case "Date":

		case "Lily":
		case "Daisy":
		case "Rose":
	}

	%dataObj.var_product = %seller.sellItem.uiName;
	%dataObj.var_price = mFloatLength(getBuyPrice(%seller.sellItem.uiName, 1), 2);
}