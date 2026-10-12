// I CANT GET MAX RANGE TO WORK SO IM MAKING THIS THE SHODDIEST POSSIBLE BOT EVER MADE

if (!isObject($buxGiverDialogueSet))
{
	$buxGiverDialogueSet = new SimSet(BuxGiverDialogueSet);
}
$buxGiverDialogueSet.deleteAll();

$obj = new ScriptObject(BuxGiverDialogueStart)
{
	messageCount = 0;
	maxRange = 100;

    functionOnStart = "setupBuxGiver";
};
$buxGiverDialogueSet.add($obj);

$obj = new ScriptObject(BuxGiverOfferDeal)
{
	messageCount = 3;
	message[0] = "Hey kid. You look like you could use some Bux!";
	messageTimeout[0] = 2;
	message[1] = "How'd you like to get a stack of your own to hold on to?";
	messageTimeout[1] = 2;
    message[2] = "On the house! A welcome gift for visiting my reskin business.";
	messageTimeout[2] = 2;

	botTalkAnim = 1;
	maxRange = 100; // THIS CRAP AINT WORKIN
	// dialogueTransitionOnTimeout = "BuxGiverDealHandler";
};

$obj = new ScriptObject(BuxGiverDealed)
{
	messageCount = 1;
	message[0] = "Don't spend it all in one place! Unless I own the place!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};

$obj = new ScriptObject(BuxGiverNoDeal)
{
	messageCount = 2;
	message[0] = "Buzz off, kid! I already cut you a stack of your own!";
	messageTimeout[0] = 2;
	message[1] = "You gotta work for the rest of your Bux! Do some labor for a change!";
	messageTimeout[1] = 2;

	botTalkAnim = 1;
};

$obj = new ScriptObject(BuxGiverDealHandler)
{
	messageCount = 0;
	maxRange = 100;

    functionOnStart = "buxGiverDeal";
};

function setupBuxGiver(%dataObj)
{
	%pl = %dataObj.player;
    %cl = %pl.client;
	%bot = %dataObj.speaker;
    %dataObj.speaker.e = "Blooms McBux";

	if (!isObject(%cl))
	{
		return;
	}

	%tookBuxPackage = %cl.tookGiveaway;

	if (%tookBuxPackage != 1)
	{
		%cl.schedule(6000, buxGiverDeal);
		%bot.startDialogue("BuxGiverOfferDeal", %cl);
	}
	else
	{
		%bot.startDialogue("BuxGiverNoDeal", %cl);
	}

	return 1;
}

function GameConnection::buxGiverDeal(%cl)
{	
	if (!isObject(%cl))
	{
		return;
	}
	
	%tookBuxPackage = %cl.tookGiveaway;

    %head = "TRADE OFFER:" SPC "Blooms McBux";
    %text = "<font:Arial:16>Redeem 35 Bux giveaway?";
    // %cl.messageBoxYesNo(%head, %text, 'attemptBuxGiveaway', '');
	%cl.messageBoxYesNoCallBack = "serverCmdDeclinedBuxGiveaway";
	commandToClient(%cl, 'messageboxYesNo', %head, %text, 'AttemptBuxGiveaway');

	return 1;
}

function serverCmdAttemptBuxGiveaway(%cl)
{
	%pl = %cl.player;
	%tookBuxPackage = %cl.tookGiveaway;
	%starBitmap = "<bitmap:base/client/ui/ci/star>";

	if (!isObject(%cl))
	{
		return;
	}

	if (!isObject(%pl))
	{
		messageClient(%cl, '', "You must be alive!");
		return;
	}

	if (%tookBuxPackage == 0)
	{
		%cl.tookGiveaway = 1;
        %pl.farmingaddstackableitem(bux0item,35);

		%msg = "Don't spend it all in one place! Unless I own the place!";
		%string = %starBitmap @ " \c3" @ "Blooms McBux" @ "\c6: " @ %msg;
		messageClient(%cl, '', %string);
        // %bot.startDialogue("BuxGiverDealed", %cl);
	}
	else
	{
		%msg = "This ain't a charity! I already gave you a cut.";
		%string = %starBitmap @ " \c3" @ "Blooms McBux" @ "\c6: " @ %msg;
		messageClient(%cl, '', %string);
	}

	return 1;
}

function serverCmdDeclinedBuxGiveaway(%cl)
{
	%starBitmap = "<bitmap:base/client/ui/ci/star>";

	if (!isObject(%cl))
	{
		return;
	}

	if (%cl.tookGiveaway == 1)
	{
		return;
	}
	
	%msg = "Your loss, kid. I got plenty to spare!";
	%string = %starBitmap @ " \c3" @ "Blooms McBux" @ "\c6: " @ %msg;
	messageClient(%cl, '', %string);

	return 1;
}



RegisterPersistenceVar("tookGiveaway", false, "");