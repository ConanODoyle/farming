
$count = 0;
if (isObject($PurchaseDialogue0))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($PurchaseDialogue[%i]))
		{
			$PurchaseDialogue[%i].delete();
		}
	}
}

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	functionOnStart = "setupPurchase";

	dialogueTransitionOnTimeout = "PurchaseDialogueCore";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseDialogueCore)
{
	response["CanPurchase"] = "PurchaseConfirmation";
	response["CanPurchaseSingular"] = "PurchaseConfirmationSingular";
	response["InsufficientMoney"] = "PurchaseFail";
	response["InsufficientMoneySingular"] = "PurchaseFailSingular";
	response["InvalidAmount"] = "PurchaseInvalid";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "I'm selling %product%s at $%price% per item! How many would you like to buy?";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "purchaseResponseParser";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseConfirmation)
{
	response["Yes"] = "PurchaseProduct";
	response["No"] = "PurchaseDialogueCore";
	response["Quit"] = "PurchaseDialogueCore";
	response["Error"] = "PurchaseDialogueCore";

	messageCount = 1;
	message[0] = "That'll be $%total% for %amount% %product%s. Are you sure? Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseConfirmationSingular : PurchaseConfirmation)
{
	message[0] = "That'll be $%total% for %amount% %product%. Are you sure? Say yes to confirm.";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseProduct)
{
	messageCount = 1;
	message[0] = "Here's %amount% %product%s for $%total%! Thanks!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_purchaseProduct";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseFail)
{
	messageCount = 2;
	message[0] = "You don't have enough money! %amount% %product%s cost $%total%.";
	messageTimeout[0] = 1;
	message[1] = "You can buy at most %maxAmount% %product%s for $%maxTotal%.";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "PurchaseDialogueCore";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseFailSingular : PurchaseFail)
{
	message[0] = "You don't have enough money! %amount% %product% costs $%total%.";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseInvalid)
{
	messageCount = 1;
	message[0] = "I can't sell you %amount% %product%s...";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};





function dialogue_purchaseProduct(%dataObj)
{
	%pl = %dataObj.player;
	%cl = %pl.client;

	if (%cl.score >= %dataObj.var_total)
	{
		%cl.setScore(%cl.score - %dataObj.var_total);
		%item = %dataObj.sellItem;
		if (!%item.isStackable)
		{
			for (%i = 0; %i < %dataObj.var_amount; %i++)
			{
				%pl.farmingAddItem(%item);
			}
		}
		else
		{
			%pl.farmingAddStackableItem(%item, %dataObj.var_amount);
		}
	}
	return 0;
}

function setupPurchase(%dataObj)
{
	%pl = %dataObj.player;
	%seller = %dataObj.speaker;

	%dataObj.sellItem = %seller.sellItem;
	%dataObj.var_product = %seller.sellItem.uiName;
	%dataObj.var_price = mFloatLength(getBuyPrice(%seller.sellItem.uiName, 1), 2);
}

function purchaseResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;
	%product = %dataObj.sellItem;

	%string = stripChars(%msg, "-1234567890");
	%num = stripChars(%msg, %string);

	if (%num > 0)
	{
		%price = getBuyPrice(%product.uiName, %num);
	}

	%dataObj.var_amount = %num;
	%dataObj.var_total = mFloatLength(%price, 2);
	%dataObj.var_maxAmount = mFloor(%pl.client.score / %dataObj.var_price);
	%dataObj.var_maxTotal = mFloatLength(getBuyPrice(%product.uiName, %dataObj.var_maxAmount), 2);

	if (%num $= "")
	{
		return "Error";
	}
	else if (%num <= 0 || %num > 500)
	{
		%num = %num + 0;
		return "InvalidAmount";
	}
	else if (%pl.client.score < %price)
	{
		if (%num == 1)
		{
			return "InsufficientMoneySingular";
		}
		return "InsufficientMoney";
	}
	else if (%pl.client.score >= %price)
	{
		if (%num == 1)
		{
			return "CanPurchaseSingular";
		}
		return "CanPurchase";
	}
}













$count = 0;
if (isObject($StoreDialogue0))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($StoreDialogue[%i]))
		{
			$StoreDialogue[%i].delete();
		}
	}
}

$StoreDialogue[$count++] = new ScriptObject(StoreDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	functionOnStart = "setupStorePurchase";

	dialogueTransitionOnTimeout = "StoreDialogueCore";
};

$StoreDialogue[$count++] = new ScriptObject(StoreDialogueCore)
{
	response["ValidSelection"] = "PurchaseDialogueCore";
	response["InvalidSelection"] = "SelectionInvalid";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 2;
	message[0] = "I'm selling %productlist%!";
	messageTimeout[0] = 1;
	message[1] = "What would you like to buy?";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "storeSelectionParser";
};

$StoreDialogue[$count++] = new ScriptObject(SelectionInvalid)
{
	messageCount = 1;
	message[0] = "I'm not selling that. Something else, maybe?";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "StoreDialogueCore";
};





function setupStorePurchase(%dataObj)
{
	%pl = %dataObj.player;
	%seller = %dataObj.speaker;

	for (%i = 0; %i < getWordCount(%seller.sellItems); %i++)
	{
		%item = getWord(%seller.sellItems, %i);
		if (%item $= "" || !isObject(%item))
		{
			continue;
		}

		%dataObj.sellItem[%i] = %item;
		if (%list !$= "")
		{
			%list = %list TAB %dataObj.sellItem[%i].uiName @ "s";
		}
		else
		{
			%list = %dataObj.sellItem[%i].uiName @ "s";
		}
		%count++;
	}
	%dataObj.sellItemCount = %count;

	%list = trim(setField(%list, %count - 1, "and " @ getField(%list, %count - 1)));
	%list = strReplace(%list, "\t", ", ");

	%dataObj.var_productList = %list;
}

function storeSelectionParser(%dataObj, %msg)
{
	%pl = %dataObj.player;

	%msg = strLwr(%msg);
	if (strPos(%msg, "nothing") >= 0)
	{
		return "Quit";
	}

	for (%i = 0; %i < %dataObj.sellItemCount; %i++)
	{
		%item = %dataObj.sellItem[%i];
		if (strPos(strLwr(%item.uiName), %msg) >= 0)
		{
			%selectedItem = %item;
			break;
		}
	}

	if (!isObject(%selectedItem))
	{
		return "InvalidSelection";
	}
	else
	{
		%dataObj.sellItem = %item;
		%dataObj.var_product = %item.uiName;
		%dataObj.var_price = mFloatLength(getBuyPrice(%item.uiName, 1), 2);
		return "ValidSelection";
	}
}















function AIPlayer::setSellItems(%bot, %string)
{
	%sellItem = "";
	for (%i = 0; %i < getWordCount(%string); %i++)
	{
		%item = getWord(%string, %i);
		if (isObject(%item))
		{
			%sellItem = %sellItem SPC %item;
			%count++;
		}
	}
	%sellItem = trim(%sellItem);

	if (%count == 1)
	{
		%bot.sellItem = %sellItem;
	}
	else if (%count > 1)
	{
		%bot.sellItems = %sellItem;
	}
}
registerOutputEvent("Bot", "setSellItems", "string 200 200", 1);