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
function fxDTSBrick::runGrowthTick(%brick)
{
	%db = %brick.getDatablock();

	//check if a crop
	if (%db.cropType $= "" || !isObject(%brick) || %brick.getGroup().bl_id == 888888)
	{
		RemovePlantSimSet.add(%brick);
		return 0;
	}

	//wait for timeout
	if (%brick.nextGrowTime > $Sim::Time)
	{
		return 0;
	}
	else if (%db.isWeed && %brick.nextWeedVictimSearch < $Sim::Time)
	{
		weedVictimSearch(%brick);
		%brick.nextWeedVictimSearch = $Sim::Time + 10;
	}

	%dirtList = %brick.getDirtWater();
	%dirt = getWord(%dirtList, 0);
	if (!isObject(%dirt))
	{
		talk("Plant has no dirt under it!");
	}
	%dirtNutrients = %dirt.getNutrients();
	%lightInfo = getPlantLightLevel(%brick);
	%light = getWord(%lightInfo, 0);
	%greenhouse = getWord(%lightInfo, 1);
	%weather = ($isHeatWave + 0) SPC ($isRaining + 0);

	%leftover = %brick.extractNutrients(%dirtNutrients);
	%dirt.setNutrients(getWord(%leftover, 0), getWord(%leftover, 1), getWord(%leftover, 2));
	%brickNutrients = %brick.getNutrients();

	%brick.greenhouseBonus = %greenhouse;
	//check if can potentially grow at all
	//placed here so that fully grown plant bricks on load will get their greenhouse bonus
	if (!%brick.canGrow())
	{
		RemovePlantSimSet.add(%brick);
		return 0;
	}

	%brick.attemptGrowth(%dirt, %brickNutrients, %light, %weather);

	%brickNutrients = %brick.getNutrients();
	%nextTickTime = %brick.getNextTickTime(%brickNutrients, %light, %weather);

	%brick.nextGrowTime = $Sim::Time + %nextTickTime;

	//if things have been significantly delayed, keep going till it is ready
	if (%brick.nextGrowTime < $Sim::Time)
	{
		return 1 + %brick.runGrowthTick();
	}
	else
	{
		return 1;
	}
}