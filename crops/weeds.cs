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
		

		if (%brick.getDatablock().isDirt && %brick.getGroup().bl_id != 888888 && %brick.nextWeedCheck < $Sim::Time)
		{
			%brick.nextWeedCheck = $Sim::Time + $WeedTickLength;
			%hit = containerRaycast(%brick.getPosition(), vectorAdd(%pos, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType, %brick);
			if (isObject(%hit) && %hit.getDatablock().isGreenhouse)
			{
				continue;
			}
			generateWeed(%brick);
		}
	}

	$masterWeedSchedule = schedule(66, 0, weedTick, %index - %i);
}

function generateWeed(%brick)
{
	//check if we create a weed
	%rand = getRandom();
	%chance = $WeedBaseChance + ($WeedFertModifier * %brick.fertilizerWeedModifier);

	if (%rand > %chance)
	{
		%brick.fertilizerWeedModifier -= 0.05;
		%brick.fertilizerWeedModifier = getMax(%brick.fertilizerWeedModifier, 0);
		return;
	}

	//create the weed
	%zOffset = brickWeed0CropData.brickSizeZ * 0.1;

	%box = %brick.getWorldBox();
	%small = getWords(%box, 0, 2);
	%large = getWords(%box, 3, 5);

	%xRange = getWord(%large, 0) * 2 - getWord(%small, 0) * 2;
	%yRange = getWord(%large, 1) * 2 - getWord(%small, 1) * 2;
	%z = getWord(%large, 2);

	%xRand = getRandom(%xRange) / 2;
	%yRand = getRandom(%yRange) / 2;

	%pos = getWord(%small, 0) + %xRand SPC getWord(%small, 1) + %yRand SPC %z + %zOffset;


	//attempt placement
	%b = new fxDTSBrick()
	{
		seedPlant = 1;
		colorID = %brickDB.defaultColor + 0;
		position = %pos;
		isPlanted = 1;
		dataBlock = brickWeed0CropData;
		rotation = getRandomBrickOrthoRot();
	};
	%error = %b.plant();
	if (%error > 0 || %error $= "")
	{
		%b.delete();
		%brick.fertilizerWeedModifier -= 0.1;
		%brick.fertilizerWeedModifier = getMax(%brick.fertilizerWeedModifier, 0);
		return;
	}

	//weed planted, decrease modifier
	%b.setColliding(0);

	%brick.fertilizerWeedModifier -= 10;
	%brick.fertilizerWeedModifier = getMax(%brick.fertilizerWeedModifier, 0);

	%brick.getGroup().add(%b);

	//apply weed directly on plants
	weedVictimSearch(%b);
}

function weedVictimSearch(%brick)
{
	initContainerRadiusSearch(%brick.getPosition(), $WeedSearchRadius, $Typemasks::fxDTSBrick)
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
	for (%i = 0; %i < getWordCount(%weedList); %i++)
	{
		%obj = getWord(%weedList);
		if (!isObject(%obj) || !%obj.getDatablock().isWeed)
		{
			continue;
		}
		%final = %final SPC %obj;
		%multiplier += %obj.getDatablock().timeDelay;
	}
	%plant.weedList = trim(%final);

	return %multiplier;
}