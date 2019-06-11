

function weedLoop(%index)
{
	cancel($masterRainLoop);

	if (%index < 0)
	{
		%index = RainFillSimSet.getCount() - 1;
	}
	else if (%index > RainFillSimSet.getCount() - 1)
	{
		%index = RainFillSimSet.getCount() - 1;
	}

	%max = 16;
	for (%i = 0; %i < %max; %i++)
	{
		if (%i > %index)
		{
			break;
		}
		%brick = RainFillSimSet.getObject(%index - %i);

		if (%brick.nextRain $= "")
		{
			%brick.nextRain = $Sim::Time;
		}

		%db = %brick.getDatablock();
		%ray = %brick.getPosition();
		%ray = containerRaycast(%ray, vectorAdd(%ray, "0 0 100"), $TypeMasks::fxBrickAlwaysObjectType, %brick);
		if (%brick.nextRain < $Sim::Time)
		{
			%numTimes = mFloor(($Sim::Time - %brick.nextRain) / 2) + 1;
			if (!%brick.inGreenhouse && %brick.waterLevel < %db.maxWater)
			{
				if (%db.isDirt)
				{
					%brick.setWaterLevel(%brick.waterLevel + 40 * %numTimes);
				}
				else if (%db.isWaterTank && !isObject(%ray))
				{
					%brick.setWaterLevel(%brick.waterLevel + 30 * %numTimes); //%db.maxWater * %numTimes / 500);
				}
			}
			%brick.nextRain = $Sim::Time + 2;
		}
		
		%totalBricksProcessed++;
	}

	$masterRainLoop = schedule(33, 0, rainLoop, %index - %totalBricksProcessed);
}