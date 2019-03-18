function fxDTSBrick::generateRandomSale(%this, %time, %options)
{
	cancel(%this.generateRandomSaleSched);
	if (!%this.getDatablock().isShopBrick)
	{
		return;
	}

	for (%i = 1; %i < 5; %i++)
	{
		%str[%i] = validateStorageContents(%this.eventOutputParameter0_[%i], %this);
		if (trim(%str[%i]) $= "")
		{
			%foundEmpty = %i;
		}
	}

	if (%foundEmpty < 1)
	{
		%this.generateRandomSaleSched = %this.schedule(%time / 2, %options);
		return;
	}

	%option = getWord(%options, getRandom(0, getWordCount(%options) - 1));
	%event = pickRandomEvent(%this, %option);
	%stackType = getField(%event, 1);
	%price = getField(%event, 2);
	%msg = getField(%event, 3);
	if (isObject(%stackType)) //is an item db
	{
		%count = getRandom(1, 3);
	}
	else
	{
		%adjustedMax = mCeil(getMax(0, (1 - %price / 100)) * 30 + 5);

		%count = getRandom(1, %adjustedMax);
	}

	%newStr = %stackType @ "\"" @ %count;
	%this.eventOutputParameter0_[%foundEmpty] = %newStr;

	for (%i = 1; %i < 5; %i++)
	{
		%rawstr[%i] = %this.eventOutputParameter0_[%i];
	}
	%this.updateShopMenus(%rawstr1, %rawstr2, %rawstr3, %rawstr4);
	%this.generateRandomSaleSched = %this.schedule(%time, %options);
}

registerOutputEvent("fxDTSBrick", "generateRandomSale", "int 0 100000 1200" TAB "string 200 100", 1);