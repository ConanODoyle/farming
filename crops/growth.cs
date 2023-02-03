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
	function ndTrustCheckSelect(%brick, %group, %bl_id, %admin)
	{
		if (%brick.getDatablock().isPlant && !findClientByBL_ID(%bl_id).isBuilder)
		{
			return 0;
		}

		return parent::ndTrustCheckSelect(%brick, %group, %bl_id, %admin);
	}

	function serverCmdPlantBrick(%cl)
	{
		if (isObject(%cl.player.tempBrick) && %cl.player.tempBrick.dataBlock.isPlant)
		{
			%cl.player.tempBrick.delete();
			messageClient(%cl, '', "You are not allowed to place plant bricks directly!");
		}
		parent::serverCmdPlantBrick(%cl);
	}

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

		if ((%brick.isPlanted && %brick.getDatablock().isPlant)
			|| %brick.getGroup().isLoadingLot)
		{
			PlantSimSet.add(%brick);
			%brick.plantSimSetAddCount++;
			if (%brick.plantSimSetAddCount > 10)
			{
				echo(%brick @ " addCount: " @ %brick.plantSimSetAddCount);
			}
		}
		return %ret;
	}

	function fxDTSBrick::onLoadPlant(%brick)
	{
		%ret = parent::onLoadPlant(%brick);

		if (%brick.getDatablock().isPlant)
		{
			PlantSimSet.add(%brick);
			%brick.plantSimSetAddCount++;
			if (%brick.plantSimSetAddCount > 10)
			{
				echo(%brick @ " addCount: " @ %brick.plantSimSetAddCount);
			}
		}
		return %ret;
	}

	function fxDTSBrick::plant(%brick)
	{
		%ret = parent::plant(%brick);

		if (%brick.getDatablock().isPlant)
		{
			PlantSimSet.add(%brick);
			%brick.plantSimSetAddCount++;
			if (%brick.plantSimSetAddCount > 10)
			{
				echo(%brick @ " addCount: " @ %brick.plantSimSetAddCount);
			}
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
			%ticks = %brick.runGrowthTick();
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
function fxDTSBrick::runGrowthTick(%plant)
{
	%db = %plant.dataBlock;

	//remove if not a crop
	if (!%db.isPlant || !isObject(%plant) || %plant.getGroup().bl_id == 888888)
	{
		if (!RemovePlantSimSet.isMember(%plant))
		{
			RemovePlantSimSet.add(%plant);
		}
		return 0;
	}

	if (%plant.nextGrowTime > $Sim::Time)
	{
		return 0;
	}

	//list of dirt under brick
	%dirtList = %plant.getDirtList();
	if (%dirtList $= "")
	{
		%plant.nextGrowTime = $Sim::Time + 10;
		return 0;
	}

	%dirt = getWord(%dirtList, 0);
	if (!isObject(%dirt))
	{
		%plant.nextGrowTime = $Sim::Time + 10;
		return 0;
	}

	%dirtNutrients = getTotalDirtNutrients(%dirtList);
	%lightInfo = getPlantLightLevel(%plant);
	%light = getWord(%lightInfo, 0);
	%greenhouse = getWord(%lightInfo, 1);
	
	//do not grow if it is a tree and in a greenhouse
	//revert into seedling state
	if (%db.isTree && %greenhouse)
	{
		if (%db.stage > 0)
		{
			if (%db.cropType !$= "Cactus")
			{
				%plant.setDatablock("brick" @ %db.croptype @ "Tree0CropData");
			}
			else
			{
				%plant.setDatablock("brick" @ %db.cropType @ "0CropData");
			}
		}
		return 0;
	}
	%weather = ($isRaining + 0) SPC ($isHeatWave + 0);

	%leftover = %plant.extractNutrients(%dirtNutrients);
	removeTotalDirtNutrients(%dirtList, vectorSub(%dirtNutrients, %leftover));

	%plantNutrients = %plant.getNutrients();
	%plant.greenhouseBonus = %greenhouse;

	//check if can potentially grow at all
	//placed here so that fully grown plant bricks on load will get their greenhouse bonus once the growth tick starts
	if (!%plant.canGrow())
	{
		if (!RemovePlantSimSet.isMember(%plant))
		{
			RemovePlantSimSet.add(%plant);
		}
		return 0;
	}

	%plant.attemptGrowth(%dirt, %plantNutrients, %light, %weather);

	%plantNutrients = %plant.getNutrients();
	%nextTickTime = %plant.getNextTickTime(%plantNutrients, %light, %weather);

	if (%plant.nextGrowTime $= "" || %plant.nextGrowTime < $Sim::Time - 100)
	{
		%plant.nextGrowTime = $Sim::Time + %nextTickTime;
	}
	else
	{
		%plant.nextGrowTime = %plant.nextGrowTime + %nextTickTime;
	}

	//if things have been significantly delayed, keep going till it is ready
	if (%plant.nextGrowTime < $Sim::Time)
	{
		return 1 + %plant.runGrowthTick();
	}
	else
	{
		return 1;
	}
}