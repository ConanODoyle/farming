function getBuyPrice(%name, %count) //price to purchase items from bots
{
	%count = %count $= "" ? 1 : %count;

	if (isObject(%name)) //item datablock
	{
		%name = %name.stackType !$= "" ? %name.stackType : %name.uiName;
	}
	%name = strReplace(%name, "-", "DASH");
	%name = strReplace(%name, " ", "_");
	%name = stripChars(%name, "!@#$%^&*()[]{}:;<>,.?/|=+");

	if (getSubStr(%name, strLen(%name) - 4, 4) $= "Seed") //is a crop seed, get the crop buy price
	{
		%name = getSubStr(%name, 0, strLen(%name) - 4);
	}

	%basePrice = getPlantData(%name, "buyPrice");
	if (%basePrice $= "")
	{
		%basePrice = $BuyCost_[%name];
	}

	return %basePrice * %count;
}

function getSellPrice(%name, %count) //price to sell items to bots
{
	%count = %count $= "" ? 1 : %count;

	if (isObject(%name)) //item datablock
	{
		%name = %name.stackType !$= "" ? %name.stackType : %name.uiName;
	}
	%name = strReplace(%name, "-", "DASH");
	%name = strReplace(%name, " ", "_");
	%name = stripChars(%name, "!@#$%^&*()[]{}:;<>,.?/|=+");

	%basePrice = getPlantData(%name, "sellPrice");
	if (%basePrice $= "")
	{
		%basePrice = $SellCost_[%name];
	}

	if (%basePrice <= 0)
	{
		//fallback to 50% of buy price
		%basePrice = mFloatLength(getBuyPrice(%name, 1) * 0.5, 2);
	}

	return %basePrice * %count;
}