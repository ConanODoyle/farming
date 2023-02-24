//config/fishing.cs for the actual tables
//these are functions to interact with them

function lerp(%a, %b, %percent)
{
	return %a * (1 - %percent) + %b * %percent;
}

function getTotalWeight(%table, %percent, %maxTier)
{
	for (%i = 0; %i < %table.count; %i++)
	{
		if (%maxTier !$= "" && %table.tier[%i] > %maxTier)
		{
			continue;
		}

		%a = %table.aWeight[%i];
		%b = %table.bWeight[%i];
		%total += getMax(lerp(%a, %b, %percent), 0);
	}
	return %total;
}

function pickFromTable(%table, %percent, %maxTier)
{
	%weight = getTotalWeight(%table, %percent, %maxTier);
	%pickWeight = getRandom() * %weight;
	%currWeight = 0;
	for (%i = 0; %i < %table.count; %i++)
	{
		%extraWeight = lerp(%table.aWeight[%i], %table.bWeight[%i], %percent);
		if (%extraWeight < 0 || %table.tier[%i] > %maxTier)
		{
			continue;
		}
		%currWeight = %currWeight + %extraWeight;
		if (%pickWeight < %currWeight)
		{
			%pick = %i;
			break;
		}
	}

	if (%pick $= "")
	{
		talk("ERROR: pickFromTable failed to find a pick! Params: " @ %table SPC %percent SPC %maxTier);
		talk("Weight: " @ %weight @ " pick: " @ %pickWeight @ " currWeight: " @ %currWeight);
		return 0;
	}
	else
	{
		return %table.option[%pick];
	}
}

function getFishingReward(%table, %percent, %maxTier)
{
	if (!isObject(%table))
	{
		%table = FishingLootTable;
	}

	%uiName = pickFromTable(%table, %percent, %maxTier);
	if (!isObject($UINameTable_Items[%uiName]))
	{
		talk("No item found for " @ %uiName);
	}
	return $UINameTable_Items[%uiName];
}