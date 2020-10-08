
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
	messageCount = 1;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	functionOnStart = "setupPurchase";

	dialogueTransitionOnTimeout = "PurchaseDialogueCore";
};

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseDialogueCore)
{
	response["CanPurchase"] = "PurchaseConfirmation";
	response["InsufficientMoney"] = "PurchaseFail";
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

$PurchaseDialogue[$count++] = new ScriptObject(PurchaseInvalid)
{
	messageCount = 1;
	message[0] = "I can't sell you %amount% %product%s...";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "PurchaseDialogueCore";
};





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
		return "InsufficientMoney";
	}
	else if (%pl.client.score >= %price)
	{
		return "CanPurchase";
	}
}