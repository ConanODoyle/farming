if (!isObject($ShopDialogueSet))
{
	$ShopDialogueSet = new SimSet(ShopDialogueSet);
}
$ShopDialogueSet.deleteAll();

$obj = new ScriptObject(PurchaseDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	functionOnStart = "setupPurchase";

	dialogueTransitionOnTimeout = "PurchaseDialogueCore";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(PurchaseDialogueCore)
{
	response["CanPurchase"] = "PurchaseConfirmation";
	response["CanPurchaseSingular"] = "PurchaseConfirmationSingular";
	response["InsufficientMoney"] = "PurchaseFail";
	response["LicenseRequired"] = "LicenseRequiredDialogue";
	response["InsufficientMoneySingular"] = "PurchaseFailSingular";
	response["InvalidAmount"] = "PurchaseInvalid";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "I'm selling %productPlural% at $%price% per item! How many would you like to buy?";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "purchaseResponseParser";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(PurchaseConfirmation)
{
	response["Yes"] = "PurchaseProduct";
	response["No"] = "PurchaseDialogueCore";
	response["Quit"] = "PurchaseDialogueCore";
	response["Error"] = "PurchaseDialogueCore";

	messageCount = 1;
	message[0] = "That'll be $%total% for %amount% %productPlural%. Are you sure? Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(PurchaseConfirmationSingular : PurchaseConfirmation)
{
	message[0] = "That'll be $%total% for %amount% %product%. Are you sure? Say yes to confirm.";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(PurchaseProduct)
{
	messageCount = 1;
	message[0] = "Here's %amount% %productPlural% for $%total%! Thanks!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_purchaseProduct";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(PurchaseFail)
{
	messageCount = 2;
	message[0] = "You don't have enough money! %amount% %productPlural% cost $%total%.";
	messageTimeout[0] = 1;
	message[1] = "You can buy at most %maxAmount% %productPlural% for $%maxTotal%.";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "PurchaseDialogueCore";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(PurchaseFailSingular : PurchaseFail)
{
	message[0] = "You don't have enough money! %amount% %product% costs $%total%.";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(PurchaseInvalid)
{
	messageCount = 1;
	message[0] = "I can't sell you %amount% %productPlural%...";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(LicenseRequiredDialogue)
{
	messageCount = 1;
	message[0] = "You need a license to buy those! Get one from the Farming Overseer in City Hall!";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(NotSellingAnythingDialogue)
{
	messageCount = 1;
	message[0] = "Sorry, I'm not selling anything right now. Check back later!";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$ShopDialogueSet.add($obj);





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

	%mod = %seller.sellPriceMod > 0 ? %seller.sellPriceMod : 1;
	%uiName = %seller.sellItem.uiName;
	%productPlural = getPluralWord(%uiName);

	//reroute dialogue if not selling anything
	if (!isObject(%seller.sellItem))
	{
		%pl.startDialogue(%seller, NotSellingAnythingDialogue);
		return 1;
	}

	%dataObj.sellItem = %seller.sellItem;
	%dataObj.var_product = %seller.sellItem.uiName;
	%dataObj.var_productPlural = %productPlural;
	%dataObj.var_price = mFloatLength(getBuyPrice(%seller.sellItem, 1) * %mod, 2);
}

function purchaseResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;

	%string = stripChars(%msg, "-1234567890");
	%num = stripChars(%msg, %string);

	if (%num > 0)
	{
		%price = %dataObj.var_price * %num;
	}

	%type = %dataObj.sellItem.stackType;
	if (getLicenseCost(%type) > 0 && !%pl.client.hasLicense(%type))
	{
		return "LicenseRequired";
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














$obj = new ScriptObject(StoreDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	functionOnStart = "setupStorePurchase";

	dialogueTransitionOnTimeout = "StoreDialogueCore";
};
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(StoreDialogueCore)
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
$ShopDialogueSet.add($obj);

$obj = new ScriptObject(SelectionInvalid)
{
	messageCount = 1;
	message[0] = "I'm not selling that. Something else, maybe?";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "StoreDialogueCore";
};
$ShopDialogueSet.add($obj);





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
		%list = %list TAB getPluralWord(%dataObj.sellItem[%i].uiName);
		%count++;
	}
	%dataObj.sellItemCount = %count;
	%list = trim(%list);

	%list = trim(setField(%list, %count - 1, "and " @ getField(%list, %count - 1)));
	if (getFieldCount(%list) > 2)
	{
		%list = strReplace(%list, "\t", ", ");
	}
	else
	{
		%list = strReplace(%list, "\t", " ");
	}

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
		%dataObj.sellItem = %selectedItem;
		%dataObj.var_product = %selectedItem.uiName;
		%dataObj.var_productPlural = getPluralWord(%selectedItem.uiName);
		%dataObj.var_price = mFloatLength(getBuyPrice(%selectedItem.uiName, 1), 2);
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

function AIPlayer::setSellPriceMod(%bot, %num)
{
	%bot.sellPriceMod = %num;
}
registerOutputEvent("Bot", "setSellPriceMod", "string 200 50", 1);