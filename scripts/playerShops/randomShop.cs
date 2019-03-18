function fxDTSBrick::generateRandomSale(%this, %options)
{
	if (!%this.getDatablock().isShopBrick)
	{
		return;
	}

	for (%i = 1; %i < 5; %i++)
	{
		%str[%i] = validateStorageContents(%this.eventOutputParameter0_[%i], %this);
		if (trim(%str[%i]) $= "")
		{
			%foundEmpty = 1;
		}
	}

	if (!%foundEmpty)
	{
		return;
	}

	%option = getWord(%options, getRandom(0, getWordCount(%options) - 1));
	%event = pickRandomEvent(%this, %option);
	%stackType = getField(%event, 1);
	%price = getField(%event, 2);
	if (isObject(%stackType)) //is an item db
	{
		%count = getRandom(1, 3);
	}
	else
	{
		%adjustedMax = mCeil(getMax(0, (1 - %price / 100)) * 30 + 5);

		%count = getRandom(1, %adjustedMax);
	}
}