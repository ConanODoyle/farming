function generateLootResults(%percent, %maxTier)
{
	for (%i = 0; %i < 1000; %i++)
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