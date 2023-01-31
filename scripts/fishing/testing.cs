function generateLootResults(%percent, %maxTier, %num)
{
	if (%num $= "")
	{
		%num = 1000;
	}
	for (%i = 0; %i < %num; %i++)
	{
		%result = pickFromTable(FishingLootTable, %percent, %maxTier);
		if (%count_[%result] <= 0)
		{
			%list_[%listCount++ - 1] = %result;
		}
		%count_[%result]++;
	}

	%file = new FileObject();
	%file.openForWrite("config/Tests/FishingLootResults.cs");

	echo("Out of " @ %num @ " reels:");
	for (%i = 0; %i < %listCount; %i++)
	{
		%file.writeLine(%list_[%i] @ ": " @ %count_[%list_[%i]]);
		echo(%list_[%i] @ ": " @ %count_[%list_[%i]]);
	}

	%file.close();
	%file.delete();
}

function generateReelModifiers(%base, %pSub, %pDiv, %qSub, %qDiv)
{
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