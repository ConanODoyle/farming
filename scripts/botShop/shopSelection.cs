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
function AIPlayer::startRandomShopLoop(%bot, %selectionCount, %speak, %timerange, %shopObjects)
{
	if (%selectionCount <= 0)
	{
		cancel(%bot.randomShopLoopSched);
		return;
	}

	%bot.randomShopLoop(%selectionCount, %speak, %timerange, %shopObjects);
}

function AIPlayer::randomShopLoop(%bot, %selectionCount, %speak, %timerange, %shopObjects)
{
	cancel(%bot.randomShopLoopSched);

	%count = 0;
	for (%i = 0; %i < %selectionCount; %i++)
	{
		%id = getRandom(getWordCount(%shopObjects) - 1);
		%obj = getWordCount(%shopObjects, %id);
		if (isObject(%obj))
		{
			%selection[%count] = %obj.selectRandom();
			%count++;
		}
	}

	if (%selectionCount > 1)
	{
		%bot.
	}

	%bot.randomShopLoopSched = %bot.schedule(10000, %selectionCount, %speak, %timerange, %shopObjects);
}