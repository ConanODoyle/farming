function generateLootResults(%percent, %quality, %num)
{
	if (%num $= "")
	{
		%num = 1000;
	}
	for (%i = 0; %i < %num; %i++)
	{
		%result = pickFromTable(FishingLootTable, %percent, %quality);
		if (%count_[%result] <= 0)
		{
			%list_[%listCount++ - 1] = %result;
		}
		%count_[%result]++;
	}

	%file = new FileObject();
	%file.openForWrite("config/Tests/FishingLootResults.cs");

	echo("Out of " @ %num @ " reels, with p=" @ %percent @ " and q=" @ %quality @ ":");
	for (%i = 0; %i < %listCount; %i++)
	{
		%income = getSellPrice($uiNameTable_Items[%list_[%i]], %count_[%list_[%i]]);
		%file.writeLine(%list_[%i] @ ": " @ %count_[%list_[%i]] @ " $" @ %income);
		echo(%list_[%i] @ ": " @ %count_[%list_[%i]] @ " $" @ %income);
		%total += %income;
	}
	echo("Total income: " @ %total);

	%file.close();
	%file.delete();
}

function generateReelModifiers(%base, %pSub, %pDiv, %qSub, %qDiv, %time)
{
	if (%time > 0)
	{
		%percent = calculatePercent(%time, %pSub, %pDiv);
		%quality = calculateQuality(%time, %base, %qSub, %qDiv);
		return %percent SPC %quality;
	}

	for (%i = 100; %i < 2000; %i+= 100)
	{
		%percent = calculatePercent(%i, %pSub, %pDiv);
		%quality = calculateQuality(%i, %base, %qSub, %qDiv);
		echo(%i @ "ms - p: " @ mFloatLength(%percent, 2) @ " q: " @ mFloatLength(%quality, 2));
	}
}

function getPoleReelModifier(%pole, %delay)
{
	%percent = calculatePercent(%i, %pole.fishingPSub, %pole.fishingPDiv);
	%quality = calculateQuality(%i, %pole.fishingBaseQuality, %pole.fishingQSub, %pole.fishingQDiv);
	return %percent SPC %quality;
}

function generatePoleReelModifiers(%pole)
{
	generateReelModifiers(%pole.fishingBaseQuality, %pole.fishingPSub, %pole.fishingPDiv, %pole.fishingQSub, %pole.fishingQDiv);
}

function generatePoleLootResults(%pole, %time)
{
	%mods = generateReelModifiers(%pole.fishingBaseQuality, %pole.fishingPSub, %pole.fishingPDiv, %pole.fishingQSub, %pole.fishingQDiv, %time);
	%percent = getWord(%mods, 0);
	%quality = getWord(%mods, 1);
	generateLootResults(%percent, %quality, 5000);
}