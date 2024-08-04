$DealRerollTimeout = 60 * 5;

if (!isObject($DealDialogueSet))
{
	$DealDialogueSet = new SimSet(DealDialogueSet);
}
$DealDialogueSet.deleteAll();

$obj = new ScriptObject(ShopNotDeal)
{
	messageCount = 1;
	message[0] = "Sorry, I don't do special deals. Try asking someone whose offer changes regularly!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};
$DealDialogueSet.add($obj);

$obj = new ScriptObject(ShopNotDealHonse)
{
	messageCount = 1;
	message[0] = "Sorry, I don't do special deals. I'm a horse!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};
$DealDialogueSet.add($obj);

$obj = new ScriptObject(ShopNotDealCargo)
{
	messageCount = 1;
	message[0] = "Sorry, I don't do special- uhh, what I meant to say was- *cargo noises*";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};
$DealDialogueSet.add($obj);

$obj = new ScriptObject(ShopNotDealHarvester)
{
	messageCount = 1;
	message[0] = "SILENCE, FARMER!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};
$DealDialogueSet.add($obj);

$obj = new ScriptObject(DealTimeout)
{
	messageCount = 1;
	message[0] = "Sorry, you'll have to wait %time% before I can change my deal!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
	functionOnStart = "checkDealTimeout";
};
$DealDialogueSet.add($obj);

$obj = new ScriptObject(DealChange)
{
	messageCount = 1;
	message[0] = "Okay, I can change my deal! Thanks for being a donator!";
	messageTimeout[0] = 1;

	botTalkAnim = 1;
};
$DealDialogueSet.add($obj);



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

			if (%picked[%curr])
			{
				continue;
			}

			%selection[%count] = %curr;
			%picked[%curr] = 1;
			%sellList = %sellList TAB getPluralWord(%curr.uiName);
			%count++;
		}
	}

	if (%count > 1)
	{
		for (%i = 0; %i < %count; %i++)
		{
			%list = %list SPC %selection[%i];
		}
		%bot.sellItems = trim(%list);
		%bot.sellItem = "";
	}
	else if (%count == 1)
	{
		%bot.sellItem = %selection[0];
		%bot.sellItems = "";
	}
	else
	{
		%bot.sellItem = "";
		%bot.sellItems = "";
	}

	if (%speak > 0 && %count > 0)
	{
		%prefix = "<bitmap:base/data/particles/exclamation><bitmap:base/client/ui/ci/star>";
		%name = %bot.name $= "" ? "Seller" : %bot.name;
		switch (%speak)
		{
			case 1:
				%str = %prefix @ "\c3" SPC %name @ "\c6: I got some new deals, come by and check!";
			case 2:
				%str = %prefix @ "\c3" SPC %name @ "\c6: I'm selling ";
				%sellList = trim(%sellList);
				%sellListCount = getFieldCount(%sellList);
				if (%sellListCount > 1)
				{
					%pre = getFields(%sellList, 0, %sellListCount - 2);
					%post = getField(%sellList, %sellListCount - 1);
					%sellList = trim(strReplace(%pre, "\t", ", ")) @ (%sellListCount > 2 ? "," : "") @ " and " @ %post;
				}

				if (%bot.sellPriceMod > 0)
				{
					%mod = " for " @ mFloor(%bot.sellPriceMod * 100) @ "% of the normal price";
				}
				%str = %str @ %sellList @ %mod @ "!";
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

	if (%bot.nextDealTime > $Sim::Time)
	{
		//slight drift to prevent permanent sync
		%bot.randomDealLoopSched = %bot.schedule(getRandom(8, 12) * 1000, randomBuyerLoop, %selectionCount, %speak, %timeRange, %shopObjects);
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

			if (%picked[%curr])
			{
				continue;
			}

			if (!isObject(%curr))
			{
				%old = %curr;
				%curr = %curr @ "Item";
				if (!isObject(%curr))
				{
					%curr = getStackTypeDatablock(%old, 1);
				}
			}

			%selection[%count] = %curr;
			%picked[%curr] = 1;
			%buyList = %buyList TAB getPluralWord(%curr.uiName);
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
		if (%bot.buyItem !$= "" || %bot.buyItems !$= "")
		{
			%lastHadItem = 1;
			%lastItem = %bot.buyItem;
			%lastItems = %bot.buyItems;
		}
		%bot.buyItem = "";
		%bot.buyItems = "";
	}

	if (%speak > 0)
	{
		%prefix = "<bitmap:base/data/particles/exclamation><bitmap:base/client/ui/ci/star>";
		%name = %bot.name $= "" ? "Buyer" : %bot.name;
		if (%bot.buyItem !$= "" || %bot.buyItems !$= "")
		{
			switch (%speak)
			{
				case 1: %str = %prefix @ "\c3" SPC %name @ "\c6: I got some new deals, come by and check!";
				case 2: %str = %prefix @ "\c3" SPC %name @ "\c6: I'm buying ";
						%buyList = trim(%buyList);
						%buyListCount = getFieldCount(%buyList);
						if (%buyListCount > 1)
						{
							%pre = getFields(%buyList, 0, %buyListCount - 2);
							%post = getField(%buyList, %buyListCount - 1);
							%buyList = trim(strReplace(%pre, "\t", ", ")) @ " and " @ %post;
						}

						if (%bot.buyPriceMod > 0)
						{
							%mod = " for " @ mFloor(%bot.buyPriceMod * 100) @ "% of the normal price";
						}
						%str = %str @ %buyList @ %mod @ "!";
			}
		}
		else if (%lastHadItem)
		{
			%str = %prefix @ "\c3" SPC %name @ "\c6: My offer has ended!";
		}

		if (%str !$= "")
		{
			messageAll('', %str);
		}
	}

	%bot.lastChangedSale = $Sim::Time;
	//slight drift to prevent permanent sync
	%bot.randomDealLoopSched = %bot.schedule(getRandom(8, 12) * 1000, randomBuyerLoop, %selectionCount, %speak, %timeRange, %shopObjects);
}

function AIPlayer::rerollDeal(%bot, %cl)
{
	if (!isObject(%pl = %cl.player))
	{
		return;
	}

	if (!isEventPending(%bot.randomDealLoopSched))
	{
		if (stripos(%bot.dataBlock.uiName, "horse") >= 0)
		{
			%pl.startDialogue(%bot, "ShopNotDealHonse");
		}
		else if (stripos(%bot.dataBlock.uiName, "harvester") >= 0)
		{
			%pl.startDialogue(%bot, "ShopNotDealHarvester");
		}
		else if (stripos(%bot.dataBlock.getName(), "cargo") >= 0 || stripos(%bot.dataBlock.uiname, "cart") >= 0)
		{
			%pl.startDialogue(%bot, "ShopNotDealCargo");
		}
		else 
		{
			%pl.startDialogue(%bot, "ShopNotDeal");
		}
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
