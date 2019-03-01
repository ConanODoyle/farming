function totalWater(%type, %startStage)
{
	%ticks = $Farming::Crops::plantData_[%type, %startStage, "numGrowTicks"];
	%time = $Farming::Crops::plantData_[%type, %startStage, "timePerTick"];
	%water = $Farming::Crops::plantData_[%type, %startStage, "waterPerTick"];
	if (%time <= 0)
	{
		return 0;
	}
	else
	{
		return %water * %ticks + totalWater(%type, %startStage + 1);
	}
}

function totalTime(%type, %startStage)
{
	%ticks = $Farming::Crops::plantData_[%type, %startStage, "numGrowTicks"];
	%time = $Farming::Crops::plantData_[%type, %startStage, "timePerTick"];
	if (%time <= 0)
	{
		return 0;
	}
	else
	{
		return %time * %ticks + totalTime(%type, %startStage + 1);
	}
}

function setAllWaterLevelsFull()
{
	for (%i = 0; %i < MainBrickgroup.getCount(); %i++)
	{
		%group = MainBrickgroup.getObject(%i);

		for (%j = 0; %j < %group.getCount(); %j++)
		{
			%brick = %group.getObject(%j);
			if (%brick.getDatablock().isWaterTank || %brick.getDatablock().isDirt)
			{
				%brick.setWaterLevel(100000);
			}
		}
	}
}