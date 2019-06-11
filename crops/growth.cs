//Handles starting growing plants

////////
//CORE//
////////

if (!isObject(PlantSimSet))
{
	$PlantSimSet = new SimSet(PlantSimSet) {};
	$RemoveSet = new SimSet(RemovePlantSimSet) {};
}

package NoPlantBuild
{
	function fxDTSBrick::onAdd(%obj)
	{
		%db = %obj.getDatablock();
		if (%db.isPlant && !%obj.seedPlant && isEventPending($masterGrowSchedule))
		{
			%obj.isPlanted = 0;
			%obj.schedule(33, delete);
			return;
		}

		return parent::onAdd(%obj);
	}
	
	function fxDTSBrick::plant(%obj)
	{
		%ret = parent::plant(%obj);

		%db = %obj.getDatablock();
		if (%db.isPlant && !%obj.seedPlant && isEventPending($masterGrowSchedule)) {
			%obj.isPlanted = 0;
			%obj.schedule(0, delete);
		}

		return %ret;
	}
};
schedule(1000, 0, activatePackage, NoPlantBuild);

function growTick(%index)
{
	cancel($masterGrowSchedule);

	if (!isObject(MissionCleanup))
	{
		return;
	}
	
	//if no plants just skip everything
	if (PlantSimSet.getCount() <= 0)
	{
		$masterGrowSchedule = schedule(33, 0, growTick, 0);
		return;
	}

	//loop runs backwards to reduce chance of skipping plants (due to periodic plant removal from simset)
	if (%index < 0)
	{
		%index = PlantSimSet.getCount() - 1;
	}
	else if (%index > PlantSimSet.getCount() - 1)
	{
		%index = PlantSimSet.getCount() - 1;
	}
	// echo("CurrIndex: " @ %index);

	%max = 16;
	for (%i = 0; %i < %max; %i++)
	{
		if (%i > %index) //we reached end of plant simset
		{
			break;
		}
		%brick = PlantSimSet.getObject(%index - %i);
		// echo("Calculating growth for " @ %brick);
		if (!%brick.isDead)
		{
			%ticks = doGrowCalculations(%brick, %brick.getDatablock());
		}
		// if (%ticks > 0)
		// {
		// 	echo("Growing " @ %brick @ "...");
		// 	echo("    " @ %ticks @ " used");
		// }
		// talk("   Ticks used: " @ %ticks);
		// %max -= %ticks - 1; //reduce number of bricks to be processed if plant ran more than one tick
		%totalBricksProcessed++;
	}

	//only remove after processing to prevent conflict with grow calc loop
	for (%i = 0; %i < RemovePlantSimSet.getCount(); %i++)
	{
		%obj = RemovePlantSimSet.getObject(%i);
		PlantSimSet.remove(%obj);
	}
	RemovePlantSimSet.clear();

	if (PlantSimSet.getCount() <= 0)
	{
		echo("PlantSimSet empty...");
	}

	$masterGrowSchedule = schedule(33, 0, growTick, %index - %totalBricksProcessed);
}

function doGrowCalculations(%brick, %db)
{
	if (%db.cropType $= "" || !isObject(%brick) || %brick.getGroup().bl_id == 888888) //this isnt a crop brick wtf
	{
		// talk("Not plant");
		RemovePlantSimSet.add(%brick);
		return 0;
	}

	%nextGrow = %brick.nextGrow;
	%stage = %db.stage;
	%type = %db.cropType;
	%growTicks = %brick.growTicks;
	%tickTime = $Farming::Crops::PlantData_[%type, %stage, "timePerTick"];
	//weeds slow plant growth
	%tickTime = %tickTime + %tickTime * getWeedTimeModifier(%brick);
	
	if (%tickTime <= 1) //check if its there is any grow time at all, remove from set if so
	{
		// talk("No growth time");

		if (!%brick.greenhouseBonus)
		{
			%pos = %brick.getPosition();
			%hit = containerRaycast(%pos, vectorAdd(%pos, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType);
			if (isObject(%hit) && %hit.getDatablock().isGreenhouse)
			{
				%brick.greenhouseBonus = 1;
			}
		}

		RemovePlantSimSet.add(%brick);
		return 0;
	}
	if (%nextGrow > $Sim::Time) //check if its time to grow yet
	{
		return 0;
	}
	if (%nextGrow $= "")
	{
		%brick.nextGrow = $Sim::Time + %tickTime / 1000;
		return 0;
	}

	//check if its covered by another brick
	%pos = %brick.getPosition();
	%hit = containerRaycast(%pos, vectorAdd(%pos, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType, %brick);
	if (isObject(%hit) && %hit.getDatablock().isGreenhouse)
	{
		%hit = containerRaycast(%pos, vectorAdd(%pos, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType, %hit, %brick);
		%greenhouseFound = 1;
		%brick.greenhouseBonus = 1;
		%tickTime = mFloor(%tickTime / 2);
	}
	else if (!isObject(%hit) || !%hit.getDatablock().isGreenhouse)
	{
		%brick.greenhouseBonus = 0;
	}

	if (isObject(%hit) && !%db.growsWithoutSunlight) //has a brick above it/its greenhouse
	{
		%brick.nextGrow = $Sim::Time + %tickTime / 1000;
		return 0;
	}

	//grow calculations
	%waterReq = $Farming::Crops::PlantData_[%type, %stage, "waterPerTick"];
	%maxGrowTicks = $Farming::Crops::PlantData_[%type, %stage, "numGrowTicks"];
	%maxDryTicks = $Farming::Crops::PlantData_[%type, %stage, "numDryTicks"];
	%dryGrow = $Farming::Crops::PlantData_[%type, %stage, "dryStage"];
	%wetGrow = $Farming::Crops::PlantData_[%type, %stage, "nextStage"];

	if (%greenhouseFound) //halve the water usage due to double growth time
	{
		%waterReq = mCeil(%waterReq / 2);
	}
	
	if ($isRaining)
	{
		if (!%greenhouseFound)
		{
			%waterReq = mCeil(%waterReq * $Farming::Crops::PlantData_[%type, "rainWaterModifier"]);
		}
		%tickTime = mCeil(%tickTime * $Farming::Crops::PlantData_[%type, "rainTimeModifier"]);
	}
	else if ($isHeatWave)
	{
		if (!%greenhouseFound)
		{
			%waterReq = mCeil(%waterReq * $Farming::Crops::PlantData_[%type, "heatWaveWaterModifier"]);
		}
		%tickTime = mCeil(%tickTime * $Farming::Crops::PlantData_[%type, "heatWaveTimeModifier"]);
	}

	if (%db.brickSizeX % 2 == 1)
	{
		%dirt = getWord(containerRaycast(%pos, vectorSub(%pos, "0 0" SPC %db.brickSizeZ * 0.1 + 0.1), $TypeMasks::fxBrickObjectType, %brick), 0);
	}
	else
	{
		%offset = vectorSub(%pos, "0.25 0.25 0");
		%dirt = getWord(containerRaycast(%offset, vectorSub(%offset, "0 0" SPC %db.brickSizeZ * 0.1 + 0.1), $TypeMasks::fxBrickObjectType, %brick), 0);	
	}
	if (!isObject(%dirt) || !%dirt.getDatablock().isDirt)
	{
		// talk(%brick @ " plant brick has no dirt under it! Deleting...");
		// talk("hit: " @ %dirt);
		// talk("brick: " @ %brick);
		// %brick.delete();
		return 0;
	}
	else
	{
		%dirt.inGreenhouse = %greenhouseFound + 0;
		// talk("Dirt: " @ %dirt);
		%water = %dirt.waterLevel;
		if (%dirt.waterLevel < %waterReq) //not enough water to grow
		{
			if (%maxDryTicks > 0) //maximum dry ticks set, crop has death penalty for being dry
			{
				%brick.dryTicks++;
				if (%brick.dryTicks >= %maxDryTicks && isObject(%dryGrow)) //its dead, jimbo
				{
					// talk("Death due to dryness	");
					%brick.setDatablock(%dryGrow);
					// RemovePlantSimSet.add(%brick);
				}
			}
			else //do nothing, crop has no death penalty for being dry, just doesn't grow
			{
				%extra = 0;
			}
			%brick.nextGrow = $Sim::Time + %tickTime / 1000;
		}
		else //has enough water
		{
			%dirt.setWaterLevel(%dirt.waterLevel - %waterReq);
			%brick.growTicks++;

			%brick.nextGrow = %nextGrow + %tickTime / 1000;

			if (%brick.growTicks >= %maxGrowTicks) //ready to grow
			{
				//reset brick ticks
				%brick.growTicks = 0;
				%brick.dryTicks = 0;

				//change db
				%brick.setDatablock(%wetGrow);

				//update with correct growTime
				%tickTime = $Farming::Crops::PlantData_[%wetGrow.cropType, %wetGrow.stage, "timePerTick"];
				%brick.nextGrow = $Sim::Time + %tickTime / 1000;
			}

			//extra tick if next grow is still before the last growtime; recursive
			if (%brick.nextGrow < $Sim::Time)
			{
				%extra = doGrowCalculations(%brick, %brick.getDatablock());
			}
		}
	}
	return 1 + %extra;
}

package PlantSimSetCollector
{
	function fxDTSBrick::onAdd(%brick)
	{
		%ret = parent::onAdd(%brick);

		if (%brick.isPlanted && %brick.getDatablock().isPlant)
		{
			PlantSimSet.add(%brick);
		}
		return %ret;
	}
};
activatePackage(PlantSimSetCollector);