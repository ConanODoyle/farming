$PurchaseDialogueStart = new ScriptObject(PurchaseDialogue)
{
	response["CanPurchase"] = "PurchaseProduct";
	response["InsufficientMoney"] = "PurchaseFail";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 2;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	message[1] = "I'm selling %product% at %price%! How many would you like to buy?";
	messageTimeout[1] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "purchaseResponseParser";
	functionOnStart = "setupPurchase";
};

$PurchaseDialoguePurchase = new ScriptObject(PurchaseProduct)
{
	messageCount = 1;
	message[0] = "Here's %amount% %product% for %total%! Thanks!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_purchaseProduct";
};

$PurchaseDialogueFail = new ScriptObject(PurchaseFail)
{
	messageCount = 1;
	message[0] = "You don't have enough money! %amount% %product% costs %total%!";
	messageTimeout[0] = 1;

	dialogueTransitionOnTimeout = "PurchaseDialogue";
};





function setupPurchase(%dataObj)
{
	%pl = %dataObj.player;
	%seller = %dataObj.speaker;

	%dataObj.var_product = %seller.sellItem.uiName;
	%dataObj.var_price = "$" @ MFloatLength(getBuyPrice(%seller.sellItem.uiName, 1), 2);
}

function purchaseResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;
	return %dataObj.dialogueObject.response["Quit"];
}