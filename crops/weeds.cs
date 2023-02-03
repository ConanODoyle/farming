//depends on the weather-generated RainFillSimSet (to fill dirt/water tanks with water during rain)
function weedTick(%index)
{
	cancel($masterWeedSchedule);

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

		%db = %brick.getDatablock();
		if (%db.isDirt && !(%db.isPlanter || %db.isPot) && %brick.getGroup().bl_id != 888888 && %brick.nextWeedCheck < $Sim::Time)
		{
			%pos = %brick.getPosition();
			%brick.nextWeedCheck = $Sim::Time + $WeedSpawnTickTime;
			%hit = containerRaycast(%pos, vectorAdd(%pos, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType, %brick);
			if (isObject(%hit) && %hit.getDatablock().isGreenhouse)
			{
				%brick.inGreenhouse = 1;
				continue;
			}
			generateWeed(%brick);
		}
	}

	$masterWeedSchedule = schedule(66, 0, weedTick, %index - %i);
}

//weed mechanics:
//	spawn more often on dirt with fertilizer applied to it (successful attempts reduce the modifier significantly)
//	spawn more often during rain
//	spawn less often during heat waves
//	don't spawn on dry bricks
function generateWeed(%brick)
{
	//check if we create a weed
	if (%brick.weedImmunityExpires > $Sim::Time || %brick.inGreenhouse) {
		return "";
	}

	%rand = getRandom();
	%chance = $WeedBaseChance + ($WeedFertModifier * %brick.fertilizerWeedModifier);

	if (%brick.waterLevel / %brick.dataBlock.maxWater < 0.15)
	{
		return "";
	}

	//weather effects
	if ($rainTicksLeft > 0)
	{
		%chance *= $WeedWeatherFactor;
	}
	else if ($heatWaveTicksLeft > 0)
	{
		%chance /= $WeedWeatherFactor;
	}

	//fertilizer usage modifier on chance
	if (%rand > %chance)
	{
		%brick.fertilizerWeedModifier = getMax(%brick.fertilizerWeedModifier - 0.05, 0);
		return "";
	}

	//create the weed
	%zOffset = brickWeed0CropData.brickSizeZ * 0.1;

	%box = %brick.getWorldBox();
	%small = getWords(%box, 0, 2);
	%large = getWords(%box, 3, 5);

	%xRange = getWord(%large, 0) * 2 - getWord(%small, 0) * 2 - 0.5;
	%yRange = getWord(%large, 1) * 2 - getWord(%small, 1) * 2 - 0.5;
	%z = getWord(%large, 2);

	%xRand = getRandom(%xRange) / 2;
	%yRand = getRandom(%yRange) / 2;

	%pos = getWord(%small, 0) + %xRand SPC getWord(%small, 1) + %yRand SPC %z + %zOffset;

	// %end = %pos;
	// %start = vectorAdd(%pos, "0 0 5");
	// %pos = getWords(containerRaycast(%start, %end, $Typemasks::fxBrickAlwaysObjectType), 1, 3);

	//attempt placement
	%b = new fxDTSBrick()
	{
		seedPlant = 1;
		position = %pos;
		isPlanted = 1;
		dataBlock = brickWeed0CropData;
		rotation = getRandomBrickOrthoRot();
	};
	%error = %b.plant();
	if (%error > 0 || %error $= "")
	{
		%b.delete();
		%brick.fertilizerWeedModifier -= 1;
		%brick.fertilizerWeedModifier = (%brick.fertilizerWeedModifier < 0 ? 0 : %brick.fertilizerWeedModifier);
		return "";
	}

	//weed planted, decrease modifier
	%b.setTrusted(1);
	%b.setColliding(0);

	%brick.fertilizerWeedModifier -= 10;
	%brick.fertilizerWeedModifier = (%brick.fertilizerWeedModifier < 0 ? 0 : %brick.fertilizerWeedModifier);

	%brick.getGroup().add(%b);

	//apply weed directly on plants
	weedVictimSearch(%b);
	return %b;
}

function weedVictimSearch(%brick)
{
	initContainerRadiusSearch(%brick.getPosition(), $WeedSearchRadius, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (%next == %brick)
		{
			continue;
		}
		if (!%next.getDatablock().isPlant || %next.getDatablock().isWeed || %next.greenhouseBonus)
		{
			continue;
		}

		addWeed(%next, %brick);
	}
}

function addWeed(%plant, %weed)
{
	if (!isObject(%plant) || !isObject(%weed))
	{
		return;
	}

	%weedList = " " @ %plant.weedList @ " ";
	%weedSearch = " " @ %weed @ " ";
	// talk("Addweed: [" @ %weedList @ "] [" @ %weedSearch @ "]");

	if (strPos(%weedList, %weedSearch) < 0)
	{
		%weedList = %weedList @ %weed @ " ";
	}

	%plant.weedList = trim(%weedList);
}

function removeWeed(%plant, %weed)
{
	if (!isObject(%plant) || !isObject(%weed))
	{
		return;
	}

	%weedList = " " @ %plant.weedList @ " ";
	%weedSearch = " " @ %weed @ " ";

	%weedList = strReplace(%weedList, %weedSearch, " ");

	%plant.weedList = trim(%weedList);
}

function getWeedTimeModifier(%plant)
{
	%weedList = %plant.weedList;
	// if (!%plant.getDatablock().isWeed)
		// talk("Time modifier: " @ %weedList);
	for (%i = 0; %i < getWordCount(%weedList); %i++)
	{
		%obj = getWord(%weedList, %i);
		if (!isObject(%obj) || !%obj.getDatablock().isWeed)
		{
			continue;
		}
		%final = %final SPC %obj;
		%multiplier += %obj.getDatablock().timeDelay;
	}
	// if (!%plant.getDatablock().isWeed)
		// talk("Final: " @ %final);
	%plant.weedList = getSubStr(%final, 1, strLen(%final));

	return %multiplier;
}

function pickWeed(%brick, %pl)
{
	initContainerRadiusSearch(%brick.getPosition(), $WeedSearchRadius, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (%next == %brick)
		{
			continue;
		}
		if (!%next.getDatablock().isPlant || %next.getDatablock().isWeed)
		{
			continue;
		}

		removeWeed(%next, %brick);
	}
	%brick.delete();
}


package WeedSearch
{
	function fxDTSBrick::runGrowthTick(%brick)
	{
		%db = %brick.dataBlock;
		if (%db.isWeed && %brick.nextWeedVictimSearch < $Sim::Time)
		{
			weedVictimSearch(%brick);
			%brick.nextWeedVictimSearch = $Sim::Time + 10;
		}
		return parent::runGrowthTick(%brick);
	}
};
activatePackage(WeedSearch);