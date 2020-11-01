
$count = 0;
if (isObject($RepairDialogue1))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($RepairDialogue[%i]))
		{
			$RepairDialogue[%i].delete();
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
	response["LicenseRequired"] = "LicenseRequiredDialogue";
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