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
		case "Potato": 
			%s1 = "Potatoes are very quick to grow, are harvested once, and drop between 2-4 potatoes and usually a seed.";
			%s2 = "Be careful not to let them grow too long or else they'll drop less!";
		case "Carrot":
			%s1 = "Carrots are quick to grow, are harvested once, and drop between 2-3 carrots and usually a seed.";
			%s2 = "If they grow too long, they'll drop less, so keep an eye on them!";
		case "Onion":
			%s1 = "Onions take a while to grow, but give a lot of experience and can be carried in big stacks. They drop 3-5 onions.";
			%s2 = "You can harvest them early, but you'll get fewer onions and a lower chance for a seed drop.";
		case "Turnip":
			%s1 = "Turnips are profitable cash crops, but take a decent bit to grow, and don't stack well.";
			%s2 = "I hear sometimes they can look like they're staring at you...";
		case "Portobello":
			%s1 = "Portobellos are mushrooms that are sensitive to light. They even grow with no light!";
			%s2 = "That said, they still want some light - imperfect lighting will slow their growth significantly.";

		case "Tomato":
			%s1 = "Tomatoes are cheap crops that can be harvested up to 12 times. They drop 2-5 per harvest.";
			%s2 = "They cost 10 experience to plant, and give 0-1 experience per harvest.";
		case "Corn":
			%s1 = "Corn can be harvested up to 3 times and drop 3-4 per harvest, but need nutrients to reach their final stage.";
			%s2 = "They cost 9 experience to plant, and give 2-3 experience per harvest.";
		case "Wheat":
			%s1 = "Wheat is a more compact alternative to corn, dropping 3-5 per harvest. They take slightly longer to grow as well.";
			%s2 = "They cost 12 experience to plant, and give 2-3 experience per harvest.";
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