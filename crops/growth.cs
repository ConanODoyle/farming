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
			%obj.schedule(33, plantDeleteCheck);
			return;
		}

		return parent::onAdd(%obj);
	}
	
	function fxDTSBrick::plant(%obj)
	{
		%ret = parent::plant(%obj);

		%db = %obj.getDatablock();
		if (%db.isPlant && !%obj.seedPlant && isEventPending($masterGrowSchedule)) {
			%obj.schedule(0, plantDeleteCheck);
		}

		return %ret;
	}
};
schedule(1000, 0, activatePackage, NoPlantBuild);

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

function fxDTSBrick::plantDeleteCheck(%obj)
{
	if (%obj.getGroup().isLoadingLot)
	{
		return;
	}
	%obj.delete();
}

function fxDTSBrick::allowPlantGrowth(%brick, %cropType)
{
	if (%brick.getGroup().bl_id == 888888)
	{
		return true;
	}
	return false;
}

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

	%max = 16;
	for (%i = 0; %i < %max; %i++)
	{
		if (%i > %index) //we reached end of plant simset
		{
			break;
		}
		%brick = PlantSimSet.getObject(%index - %i);
		if (!%brick.isDead)
		{
			%ticks = doGrowCalculations(%brick, %brick.getDatablock());
		}
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
		echo("PlantSimSet emptied out...");
	}

	$masterGrowSchedule = schedule(33, 0, growTick, %index - %totalBricksProcessed);
}

//Growth
//0) Get brick state (current resources, available dirt to draw water from)
//1) Get light level and presence of greenhouse
//2) Get weather state
//3) Extract resources from soil if space available (minerals, compost, fertilizer)
//4) Attempt growth - subtract resources and level up as necessary
//		Growth can fail if conditions not met
//5) Get tick time, given level, light, weather, greenhouse and resources

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
	%tickTime = $Farming::PlantData_[%type, %stage, "timePerTick"];
	//weeds slow plant growth
	if (%db.isWeed && %brick.nextWeedVictimSearch < $Sim::Time)
	{
		weedVictimSearch(%brick);
		%brick.nextWeedVictimSearch = $Sim::Time + $WeedTickLength * 2;
	}
	else
	{
		%tickTime = %tickTime + %tickTime * getWeedTimeModifier(%brick);
	}
	
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

		if (%db.isWeed) //kill weeds in greenhouse
		{
			%brick.delete();
			return;
		}
	}
	else if (!isObject(%hit) || !%hit.getDatablock().isGreenhouse)
	{
		%brick.greenhouseBonus = 0;
	}

	if (isObject(%hit) && !%hit.allowPlantGrowth(%type)) //has a brick above it/its greenhouse
	{
		%brick.nextGrow = $Sim::Time + %tickTime / 1000;
		return 0;
	}

	//grow calculations
	%waterReq = $Farming::PlantData_[%type, %stage, "waterPerTick"];
	%maxGrowTicks = $Farming::PlantData_[%type, %stage, "numGrowTicks"];
	%maxDryTicks = $Farming::PlantData_[%type, %stage, "numDryTicks"];
	%dryGrow = $Farming::PlantData_[%type, %stage, "dryStage"];
	%wetGrow = $Farming::PlantData_[%type, %stage, "nextStage"];

	if (%greenhouseFound) //halve the water usage due to double growth time
	{
		%waterReq = mCeil(%waterReq / 2);
	}
	
	if ($isRaining)
	{
		if (!%greenhouseFound)
		{
			%waterReq = mCeil(%waterReq * $Farming::PlantData_[%type, "rainWaterModifier"]);
		}
		%tickTime = mCeil(%tickTime * $Farming::PlantData_[%type, "rainTimeModifier"]);
	}
	else if ($isHeatWave)
	{
		if (!%greenhouseFound)
		{
			%waterReq = mCeil(%waterReq * $Farming::PlantData_[%type, "heatWaveWaterModifier"]);
		}
		%tickTime = mCeil(%tickTime * $Farming::PlantData_[%type, "heatWaveTimeModifier"]);
	}

	// if (%db.brickSizeX % 2 == 1)
	// {
	// 	%dirt = getWord(containerRaycast(%pos, vectorSub(%pos, "0 0" SPC %db.brickSizeZ * 0.1 + 0.1), $TypeMasks::fxBrickObjectType, %brick), 0);
	// }
	// else
	// {
	// 	%offset = vectorSub(%pos, "0.25 0.25 0");
	// 	%dirt = getWord(containerRaycast(%offset, vectorSub(%offset, "0 0" SPC %db.brickSizeZ * 0.1 + 0.1), $TypeMasks::fxBrickObjectType, %brick), 0);	
	// }

	for (%i = 0; %i < %brick.getNumDownBricks(); %i++)
	{
		%dirt = %brick.getDownBrick(%i);
		if (%dirt.getDatablock().isDirt && %dirt.waterLevel >= %waterReq)
		{
			break;
		}
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
				if (%brick.dryTicks >= %maxDryTicks) //its dead, jimbo
				{
					if (isObject(%dryGrow))
					{
						// talk("Death due to dryness	");
						%brick.setDatablock(%dryGrow);
						// RemovePlantSimSet.add(%brick);
					}
					else
					{
						%brick.delete();
					}
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
				%tickTime = $Farming::PlantData_[%wetGrow.cropType, %wetGrow.stage, "timePerTick"];
				%brick.nextGrow = $Sim::Time + %tickTime / 1000;
				
				// Growth particles
				%p = new Projectile()
				{
					dataBlock = "FarmingPlantGrowthProjectile";
					initialVelocity = "0 0 1";
					initialPosition = %brick.position;
				};

				if (isObject(%p))
				{
					MissionCleanup.add(%p);
					%p.explode();
				}
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



///////////
//Utility//
///////////


// Resource functions
//returns dirt bricks under plant, in order of highest water
function getDirtWater(%brick)
{
	for (%i = 0; %i < %brick.getNumDownBricks(); %i++)
	{
		%dirt = %brick.getDownBrick(%i);
		if (%dirt.getDatablock().isDirt)
		{
			%dirtCount++;
			%water = %dirt.waterLevel;
			//insert to list
			for (%j = 0; %j < %dirtCount; %j++)
			{
				if (%water > %dirt_[%j, "water"] || %dirt_[%j, "water"] $= "")
				{
					%tempDirt = %dirt_[%j];
					%tempWater = %dirt_[%j, "water"];

					%dirt_[%j] = %dirt;
					%dirt_[%j, "water"] = %water;

					%dirt = %tempDirt;
					%water = %tempWater;
				}
			}
		}
	}

	for (%i = 0; %i < %dirtCount; %i++)
	{
		%ret = %ret SPC %dirt_[%i];
	}
	return trim(%ret);
}

//returns dirt bricks under plant, in order of highest water
function getDirtWater(%brick)
{
	for (%i = 0; %i < %brick.getNumDownBricks(); %i++)
	{
		%dirt = %brick.getDownBrick(%i);
		if (%dirt.getDatablock().isDirt)
		{
			%dirtCount++;
			%water = %dirt.waterLevel;
			//insert to list
			for (%j = 0; %j < %dirtCount; %j++)
			{
				if (%water > %dirt_[%j, "water"] || %dirt_[%j, "water"] $= "")
				{
					%tempDirt = %dirt_[%j];
					%tempWater = %dirt_[%j, "water"];

					%dirt_[%j] = %dirt;
					%dirt_[%j, "water"] = %water;

					%dirt = %tempDirt;
					%water = %tempWater;
				}
			}
		}
	}

	for (%i = 0; %i < %dirtCount; %i++)
	{
		%ret = %ret SPC %dirt_[%i];
	}
	return trim(%ret);
}


// Light functions
//returns lightlevel SPC greenhouseFound
//lightlevel should be a float between 0 and 1
function getPlantLightLevel(%brick)
{
	//start is at half a plate above the bottom of the brick
	%start = vectorAdd(%brick.getPosition(), "0 0 -" @ ((%brick.getDatablock().brickSizeZ * 0.1) - 0.1));
	%end = vectorAdd(%start, "0 0 100");
	%masks = $Typemasks::fxBrickAlwaysObjectType;

	%ray = containerRaycast(%start, %end, %masks, %brick);
	%light = 1;
	while (%safety++ < 20)
	{
		%hit = getWord(%ray, 0);
		%hitDB = %hit.getDatablock();
		if (%hitDB.isGreenhouse) //ignore greenhouses
		{
			%greenhouseFound = 1;
			%start = getWords(%ray, 1, 3);
			%ray = containerRaycast(%start, %end, %masks, %hit);
			continue;
		}
		else if (%hit.getGroup().bl_id == 888888 || !isObject(%hit)) //has LOS to sky, exit
		{
			break;
		}
		else //hit a player brick
		{
			%light = %hit.getLightLevel(%light);
			if (%light == 0)
			{
				break;
			}
			else if (%hit.canLightPassThrough())
			{
				%start = getWords(%ray, 1, 3);
				%ray = containerRaycast(%start, %end, %masks, %hit);
				continue;
			}
		}
	}

	if (%safety >= 20)
	{
		talk("Light level safety hit!");
	}

	return %light SPC (%greenhouseFound + 0);
}

function fxDTSBrick::getLightLevel(%brick, %lightLevel)
{
	if (%brick.getDatablock().isTree)
	{
		return %lightLevel * $Farming::PlantData_[%brick.getDatablock().cropType, "lightLevelFactor"];
	}
	return 0; //normal bricks block all light
}

function fxDTSBrick::canLightPassThrough(%brick)
{
	if (%brick.getDatablock().isTree)
	{
		return 1;
	}
	return 0; //normal bricks don't let light through
}


// Growth functions
//attempts growth given resources and light
//returns 0 for growth success, 1 for failure