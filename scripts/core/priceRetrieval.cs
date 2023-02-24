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

	if (getSubStr(%name, getMax(0, strLen(%name) - 4), 4) $= "Seed") //is a crop seed, get the crop buy price
	{
		%name = getSubStr(%name, 0, getMax(0, strLen(%name) - 4));
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

function fxDTSBrick::listProducePrices(%brick, %cl)
{
	if ($generatedPriceList $= "")
	{
		%str = "<bitmap:Add-Ons/Server_Farming/icons/shopstall>\n\n\n\n\n\n\n<font:Palatino Linotype:28>Sell prices:\n";
		%str = %str @ "<just:left> Produce: <just:right>Fish:<font:Arial:15>\n\n";

		%count = getMax(getFieldCount($SellProduceList), getFieldCount($SellFishList));
		for (%i = 0; %i < %count; %i++)
		{
			%str = %str @ "<just:left> ";
			%produce = getField($SellProduceList, %i);
			if (%produce !$= "")
			{
				%cost = getSellPrice(%produce);
				%str = %str @ %space @ %produce @ " - $" @ mFloatLength(%cost, 2);
			}

			%produce = getField($SellFishList, %i);
			if (%produce !$= "")
			{
				%str = %str @ "<just:right>";
				%cost = getSellPrice(%produce);
				%str = %str @ %space @ strReplace(%produce, "_", " ") @ " - $" @ mFloatLength(%cost, 2);
			}
			%str = %str @ " \n";
		}
		// for (%i = 0; %i < getFieldCount($SellProduceList); %i++)
		// {
		// 	%space = %alt ? "<just:right>" : "\n<just:left>";
		// 	%alt = !%alt;
			
		// 	%produce = getField($SellProduceList, %i);
		// 	%cost = getSellPrice(%produce);
		// 	%str = %str @ %space @ %produce @ " - $" @ mFloatLength(%cost, 2);
		// }

		// %str = %str @ "\n\n";

		// for (%i = 0; %i < getFieldCount($SellFishList); %i++)
		// {
		// 	%space = %alt ? "<just:right>" : "\n<just:left>";
		// 	%alt = !%alt;

		// 	%produce = getField($SellFishList, %i);
		// 	%cost = getSellPrice(%produce);
		// 	%str = %str @ %space @ %produce @ " - $" @ mFloatLength(%cost, 2);
		// }
		$generatedPriceList = %str;
	}

	for (%i = 0; %i < 4; %i++)
	{
		%str[%i] = getSubStr($generatedPriceList, %i * 250, 250);
	}
	commandToClient(%cl, 'MessageBoxOK', "Sell Prices", '%1%2%3%4', %str0, %str1, %str2, %str3);
}

registerOutputEvent("fxDTSBrick", "listProducePrices", "", 1);