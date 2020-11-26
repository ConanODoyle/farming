if (!isObject($LicenseDialogueSet))
{
	$LicenseDialogueSet = new SimSet(LicenseDialogueSet);
}
$LicenseDialogueSet.deleteAll();

$obj = new ScriptObject(LicenseDialogueStart)
{
	response["Quit"] = "ExitResponse";
	messageCount = 1;
	message[0] = "Hello!";
	messageTimeout[0] = 1;

	dialogueTransitionOnTimeout = "LicenseDialogueCore";
};
$LicenseDialogueSet.add($obj);

$obj = new ScriptObject(LicenseDialogueCore)
{
	response["CanPurchase"] = "LicenseConfirmation";
	response["InsufficentExp"] = "LicenseFail";
	response["LicenseOwned"] = "LicenseOwned";
	response["InvalidChoice"] = "LicenseInvalid";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "I sell crop licenses that let you buy crops! What license would you like to buy?";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "licenseResponseParser";
};
$LicenseDialogueSet.add($obj);

$obj = new ScriptObject(LicenseConfirmation)
{
	response["Yes"] = "LicenseProduct";
	response["No"] = "LicenseDialogueCore";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "That'll be %total% experience for %licenseType%. Are you sure? Say yes to confirm.";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	waitForResponse = 1;
	responseParser = "yesNoResponseParser";
};
$LicenseDialogueSet.add($obj);

$obj = new ScriptObject(LicenseProduct)
{
	messageCount = 1;
	message[0] = "Here's your %licenseType% licence! Make sure you talk to the Crop Expert to find out how to grow them!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "dialogue_purchaseLicense";
};
$LicenseDialogueSet.add($obj);

$obj = new ScriptObject(LicenseFail)
{
	messageCount = 1;
	message[0] = "You don't have enough farming experience! The %licenseType% license costs %total% experience.";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "LicenseDialogueCore";
};
$LicenseDialogueSet.add($obj);

$obj = new ScriptObject(LicenseOwned)
{
	messageCount = 1;
	message[0] = "You already own a %licenseType% license!";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "LicenseDialogueCore";
};
$LicenseDialogueSet.add($obj);

$obj = new ScriptObject(LicenseInvalid)
{
	messageCount = 1;
	message[0] = "You don't need a license for \"%licenseType%\"...";
	messageTimeout[0] = 2;

	botTalkAnim = 1;
	dialogueTransitionOnTimeout = "ExitResponse";
};
$LicenseDialogueSet.add($obj);





function dialogue_purchaseLicense(%dataObj)
{
	%pl = %dataObj.player;
	%cl = %pl.client;

	return %cl.buyLicense(%dataObj.var_licenseType);
}

function licenseResponseParser(%dataObj, %msg)
{
	%pl = %dataObj.player;
	%cl = %pl.client;
	%product = %dataObj.sellItem;

	%license = trim(strLwr(%msg));

	%dataObj.var_licenseType = %license;

	%licenseCost = getLicenseCost(%license);

	if (%licenseCost <= 0)
	{
		return "InvalidChoice";
	}
	else if (%cl.hasLicense(%license))
	{
		return "LicenseOwned";
	}

	%dataObj.var_total = %licenseCost;

	if (%license $= "")
	{
		return "Error";
	}
	else if (%licenseCost > %cl.farmingExperience)
	{
		return "InsufficentExp";
	}
	else if (%licenseCost <= %cl.farmingExperience)
	{
		return "CanPurchase";
	}
}





function GameConnection::hasLicense(%cl, %type)
{
	%type = strLwr(trim(%type));
	if (%type $= "")
	{
		return 0;
	}
	if (getSubStr(%type, strLen(%type) - 4, 4) $= "seed")
	{
		%type = getSubStr(%type, 0, strLen(%type) - 4);
	}

	%licenseList = " " @ strLwr($Pref::Farming::License[%cl.bl_id]) @ " ";

	return strPos(%licenseList, " " @ %type @ " ") >= 0;
}

function GameConnection::buyLicense(%cl, %type)
{
	%type = strLwr(trim(%type));
	if (%cl.hasLicense(%type) || %type $= "")
	{
		return 1;
	}
	if (getSubStr(%type, strLen(%type) - 4, 4) $= "seed")
	{
		%type = getSubStr(%type, strLen(%type) - 4);
	}

	%price = getPlantData(%type, 0, "licenseCost");
	if (%price <= 0)
	{
		return 2;
	}

	if (%cl.farmingExperience < %price)
	{
		return 3;
	}
	else
	{
		%cl.farmingExperience = %cl.farmingExperience - %price | 0;
		$Pref::Farming::License[%cl.bl_id] = trim($Pref::Farming::License[%cl.bl_id] SPC %type);
		return 0;
	}
}

function getLicenseCost(%type)
{
	%type = strLwr(trim(%type));
	if (getSubStr(%type, strLen(%type) - 4, 4) $= "seed")
	{
		%type = getSubStr(%type, 0, strLen(%type) - 4);
	}
	
	%price = getPlantData(%type, "licenseCost");
	return %price + 0;
}