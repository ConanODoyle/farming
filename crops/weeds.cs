//depends on the weather-generated RainFillSimSet (to fill dirt/water tanks with water during rain)
function weedLoop(%index)
{
	cancel($masterWeedLoop);

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
			generateWeed(%brick);
			%brick.nextWeedCheck = $Sim::Time + $WeedTickLength;
		}
	}

	$masterWeedLoop = schedule(66, 0, weedLoop, %index - %i);
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

}