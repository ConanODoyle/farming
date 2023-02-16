package VoidGrowth
{
	function fxDTSBrick::grow(%brick, %growDB)
	{
		if (isObject(%brick.voidKeyPlant))
		{
			%brick.dryTicks = 0;
			%brick.wetTicks = 0;
			%brick.setNutrients(0, 0);
			%brick.voidKeyPlant.queueGrowTicks++;

			if (%brick.dataBlock.stage > 0)
			{
				//apple trees have a split at 10/20 so can't decrement backwards directly
				%brickDB = %brick.dataBlock.getName();
				%lastStage = getMin(9, %brickDB.stage - 1);
				while (!isObject(%lastDB) && %safety++ < 15)
				{
					%lastDB = strReplace(%brickDB, %brickDB.stage, %lastStage);
					%lastStage--;
				}
				if (isObject(%lastStage))
				{
					%brick.setDatablock(%lastStage);
				}
			}
			return;
		}
		return parent::grow(%brick, %growDB);
	}

	function fxDTSBrick::attemptGrowth(%brick, %dirt, %plantNutrients, %light, %weather)
	{
		if (%brick.dataBlock.cropType $= "Void")
		{
			return voidAttemptGrowth(%brick);
		}
		return parent::attemptGrowth(%brick, %dirt, %plantNutrients, %light, %weather);
	}
};
schedule(1000, 0, activatePackage, VoidGrowth);

function voidAttemptGrowth(%brick)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;

	%wetGrow = getPlantData(%type, %stage, "wetNextStage");
	%maxWetTicks = getPlantData(%type, %stage, "numWetTicks");

	if (%brick.queueGrowTicks >= 30)
	{
		%brick.wetTicks++;
		%brick.queueGrowTicks -= 30;
	}

	if (%brick.wetTicks > %maxWetTicks && %maxWetTicks != -1)
	{
		%brick.grow(%wetGrow);
		if (%wetGrow.stage == 5)
		{
			applyVoidKeyPlant(%brick.position, 0);
			return 0;
		}
	}

	applyVoidKeyPlant(%brick.position, %brick);
	return 0;
}

function applyVoidKeyPlant(%pos, %brick)
{
	%radius = 4;
	initContainerRadiusSearch(%pos, %radius, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (!%next.dataBlock.isPlant || %next.dataBlock.cropType $= "Void"
			|| vectorDist(%next.position, %pos) > %radius)
		{
			continue;
		}
		%next.voidKeyPlant = %brick;
	}
}