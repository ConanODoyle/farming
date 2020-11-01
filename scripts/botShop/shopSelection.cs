$DealRerollTimeout = 60 * 5;

$count = 0;
if (isObject($DealDialogue1))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($DealDialogue[%i]))
		{
			$DealDialogue[%i].delete();
		}
	}
}

$DealDialogue[$count++] = new ScriptObject(ShopNotDeal)
{
	messageCount = 1;
	message[0] = "Sorry, I don't do special deals. Try asking someone whose offer changes regularly!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};

$DealDialogue[$count++] = new ScriptObject(DealTimeout)
{
	messageCount = 1;
	message[0] = "Sorry, you'll have to wait %time% before I can change my deal!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "checkDealTimeout";
};

$DealDialogue[$count++] = new ScriptObject(DealChange)
{
	messageCount = 1;
	message[0] = "Okay, I can change my deal! Thanks for being a donator!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};



function ShopObject::selectRandom(%obj)
{
	if (%obj.totalWeight <= 0)
	{
		for (%i = 0; %i < %obj.count; %i++)
		{
			%weight += getField(%obj.option[%i], 1);
		}
		%obj.totalWeight = %weight;
	}

	%currWeight = 0;
	%rand = getRandom();
	for (%i = 0; %i < %obj.count; %i++)
	{
		%currWeight += getField(%obj.option[%i], 1);
		if (%rand < %currWeight / %obj.totalWeight)
		{
			return getField(%obj.option[%i], 0);
		}
	}
	return "";
}


registerOutputEvent("Bot", "startRandomShopLoop", "int 0 10 1" 
	TAB "list None 0 Announce 1 AnnounceDetails 2" 
	TAB "string 200 50" 
	TAB "string 200 100");
function AIPlayer::startRandomShopLoop(%bot, %selectionCount, %speak, %timeRange, %shopObjects)
{
	if (%selectionCount <= 0)
	{
		cancel(%bot.randomDealLoopSched);
		return;
	}

	%bot.randomShopLoop(%selectionCount, %speak, %timeRange, %shopObjects);
}

registerOutputEvent("Bot", "startRandomBuyerLoop", "int 0 10 1" 
	TAB "list None 0 Announce 1 AnnounceDetails 2" 
	TAB "string 200 50" 
	TAB "string 200 100");
function AIPlayer::startRandomBuyerLoop(%bot, %selectionCount, %speak, %timeRange, %shopObjects)
{
	if (%selectionCount <= 0)
	{
		cancel(%bot.randomDealLoopSched);
		return;
	}

	%bot.randomBuyerLoop(%selectionCount, %speak, %timeRange, %shopObjects);
}

function checkDealTimeout(%dataObj)
{
	%seller = %dataObj.speaker;
	%diff = $Sim::Time - %seller.lastChangedSale;
	%diff = $DealRerollTimeout - mFloor(%diff);
	%dataObj.var_time = getTimeString(%diff);
}

function AIPlayer::randomShopLoop(%bot, %selectionCount, %speak, %timeRange, %shopObjects)
{
	cancel(%bot.randomDealLoopSched);

	%bot.lastDealFunction = "randomShopLoop";
	%bot.lastSelectionCount = %selectionCount;
	%bot.lastSpeak = %speak;
	%bot.lastTimeRange = %timeRange;
	%bot.lastShopObjects = %shopObjects;

	if (%bot.nextDealTime $= "")
	{
		%bot.nextDealTime = $Sim::Time + getRandom(getWord(%timeRange, 0), getWord(%timeRange, 1));
	}

	if (%bot.nextDealTime > $Sim::Time)
	{
		//slight drift to prevent permanent sync
		%bot.randomDealLoopSched = %bot.schedule(getRandom(8, 12) * 1000, randomShopLoop, %selectionCount, %speak, %timeRange, %shopObjects);
		return;
	}
	else
	{
		%bot.nextDealTime = $Sim::Time + getRandom(getWord(%timeRange, 0), getWord(%timeRange, 1));
	}

	%count = 0;
	for (%i = 0; %i < %selectionCount; %i++)
	{
		%id = getRandom(getWordCount(%shopObjects) - 1);
		%obj = getWord(%shopObjects, %id);
		if (isObject(%obj))
		{
			%safety = 0;
			%curr = %obj.selectRandom();
			while (%picked[%curr] && %safety++ < 10)
			{
				%curr = %obj.selectRandom();
			}

			%selection[%count] = %curr;
			%picked[%curr] = 1;
			%sellList = %sellList TAB %curr.uiName @ "s";
			%count++;
		}
	}

	if (%selectionCount > 1)
	{
		for (%i = 0; %i < %count; %i++)
		{
			%list = %list SPC %selection[%i];
		}
		%bot.sellItems = trim(%list);
		%bot.sellItem = "";
	}
	else if (%selectionCount == 1)
	{
		%bot.sellItem = %selection[0];
		%bot.sellItems = "";
	}
	else
	{
		%bot.sellItem = "";
		%bot.sellItems = "";
	}

	if (%speak > 0)
	{
		%prefix = "<bitmap:base/data/particles/exclamation><bitmap:base/client/ui/ci/star>";
		%name = %bot.name $= "" ? "Seller" : %bot.name;
		switch (%speak)
		{
			case 1: %str = %prefix @ "\c3" SPC %name @ "\c6: I got some new deals, come by and check!";
			case 2: %str = %prefix @ "\c3" SPC %name @ "\c6: I'm selling ";
					%sellList = trim(%sellList);
					%sellListCount = getFieldCount(%sellList);
					if (%sellListCount > 1)
					{
						%pre = getFields(%sellList, 0, %sellListCount - 2);
						%post = getField(%sellList, %sellListCount - 1);
						%sellList = trim(strReplace(%pre, "\t", ", ")) @ " and " @ %post;
					}
					%str = %str @ %sellList @ "!";
		}
		messageAll('', %str);
	}

	%bot.lastChangedSale = $Sim::Time;
	//slight drift to prevent permanent sync
	%bot.randomDealLoopSched = %bot.schedule(getRandom(8, 12) * 1000, randomShopLoop, %selectionCount, %speak, %timeRange, %shopObjects);
}

function AIPlayer::randomBuyerLoop(%bot, %selectionCount, %speak, %timeRange, %shopObjects)
{
	cancel(%bot.randomDealLoopSched);

	%bot.lastDealFunction = "randomBuyerLoop";
	%bot.lastSelectionCount = %selectionCount;
	%bot.lastSpeak = %speak;
	%bot.lastTimeRange = %timeRange;
	%bot.lastShopObjects = %shopObjects;

	if (%bot.nextDealTime $= "")
	{
		%bot.nextDealTime = $Sim::Time + getRandom(getWord(%timeRange, 0), getWord(%timeRange, 1));
	}

	if (%bot.nextDealTime > $Sim::Time)
	{
		//slight drift to prevent permanent sync
		%bot.randomDealLoopSched = %bot.schedule(getRandom(8, 12) * 1000, %selectionCount, %speak, %timeRange, %shopObjects);
		return;
	}
	else
	{
		%bot.nextDealTime = $Sim::Time + getRandom(getWord(%timeRange, 0), getWord(%timeRange, 1));
	}

	%count = 0;
	for (%i = 0; %i < %selectionCount; %i++)
	{
		%id = getRandom(getWordCount(%shopObjects) - 1);
		%obj = getWord(%shopObjects, %id);
		if (isObject(%obj))
		{
			%safety = 0;
			%curr = %obj.selectRandom();
			while (%picked[%curr] && %safety++ < 10)
			{
				%curr = %obj.selectRandom();
			}

			%selection[%count] = %curr;
			%picked[%curr] = 1;
			%sellList = %sellList TAB %curr.uiName @ "s";
			%count++;
		}
	}

	if (%selectionCount > 1)
	{
		for (%i = 0; %i < %count; %i++)
		{
			%list = %list SPC %selection[%i];
		}
		%bot.buyItems = trim(%list);
		%bot.buyItem = "";
	}
	else if (%selectionCount == 1)
	{
		%bot.buyItem = %selection[0];
		%bot.buyItems = "";
	}
	else
	{
		%bot.buyItem = "";
		%bot.buyItems = "";
	}

	if (%speak > 0)
	{
		%prefix = "<bitmap:base/data/particles/exclamation><bitmap:base/client/ui/ci/star>";
		%name = %bot.name $= "" ? "Buyer" : %bot.name;
		switch (%speak)
		{
			case 1: %str = %prefix @ "\c3" SPC %name @ "\c6: I got some new deals, come by and check!";
			case 2: %str = %prefix @ "\c3" SPC %name @ "\c6: I'm buying ";
					%sellList = trim(%sellList);
					%sellListCount = getFieldCount(%sellList);
					if (%sellListCount > 1)
					{
						%pre = getFields(%sellList, 0, %sellListCount - 2);
						%post = getField(%sellList, %sellListCount - 1);
						%sellList = trim(strReplace(%pre, "\t", ", ")) @ " and " @ %post;
					}
					%str = %str @ %sellList @ "!";
		}
		messageAll('', %str);
	}

	%bot.lastChangedSale = $Sim::Time;
	//slight drift to prevent permanent sync
	%bot.randomDealLoopSched = %bot.schedule(getRandom(8, 12) * 1000, %selectionCount, %speak, %timeRange, %shopObjects);
}

function AIPlayer::rerollDeal(%bot, %cl)
{
	if (!isObject(%pl = %cl.player))
	{
		return;
	}

	if (!isEventPending(%bot.randomDealLoopSched))
	{
		%pl.startDialogue(%bot, "ShopNotDeal");
	}
	else if (%bot.lastChangedSale + $DealRerollTimeout > $Sim::Time)
	{
		%pl.startDialogue(%bot, "DealTimeout");
	}
	else
	{
		%pl.startDialogue(%bot, "DealChange");
		%bot.lastChangedSale = $Sim::Time;
		%bot.nextDealTime = 0;
		%bot.schedule(1000, %bot.lastDealFunction, %bot.lastSelectionCount, %bot.lastSpeak, 
			%bot.lastTimeRange, %bot.lastShopObjects);
	}
}

function serverCmdRefreshDeal(%cl)
{
	if (!%cl.canRefreshDeal)
	{
		messageClient(%cl, '', "You must be a donator to do this! <a:https://forum.blockland.us/index.php?topic=322462.0>Check the topic for the donation link!</a>");
		return;
	}
	
	if (isObject(%pl = %cl.player))
	{
		%start = %pl.getEyeTransform();
		%end = vectorAdd(%start, vectorScale(%pl.getEyeVector(), 5));
		%ray = containerRaycast(%start, %end, $TypeMasks::PlayerObjectType, %pl);

		if (isObject(%hit = getWord(%ray, 0)) && %hit.getClassName() $= "AIPlayer")
		{
			%hit.rerollDeal(%cl);
		}
	}
}
