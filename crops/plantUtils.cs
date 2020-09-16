
///////////
//Utility//
///////////


// Resource functions
//returns dirt bricks under plant, in order of highest water
//bricks with same water level are slightly randomized
function fxDTSBrick::getDirtWater(%brick)
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
				if (%water > %dirt_[%j, "water"] || %dirt_[%j, "water"] $= ""
					|| (%water == %dirt_[%j, "water"] && getRandom() > 0.75))
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

//returns dirt nutrients - nitrogen, phosphate,
function fxDTSBrick::getNutrients(%dirt)
{
	return decodeNutrientName(%dirt.getName());
}

//parses a brick name for nutrients - returns nitrogen SPC phospahte SPC weedkilelr
function decodeNutrientName(%string)
{
	%split = trim(strReplace(%string, "_", "\t")); //prefixed _ is removed in the trim
	for (%i = 0; %i < getFieldCount(%split); %i++)
	{
		%field = getField(%split, %i);
		%id = getSubStr(%field, 0, 1);
		%num = hexToInt(getSubStr(%field, 1, 100));
		switch$ (%id)
		{
			case "n": %nit = %num; //nitrogen
			case "p": %pho = %num; //phosphate
			case "w": %weedKiller = %num; //weedkiller
			default: talk("decodeNutrientName: unknown identifier " @ %id @ "! " @ ("_" @ %string).getID());
		}
	}

	return %nit SPC %pho SPC %weedKiller;
}

//encodes dirt nutrients into a brickname
function encodeNutrientName(%nit, %pho, %weedKiller)
{
	%ret = "_";
	%ret = %ret @ "n" @ intToHex(%nit) @ "_";
	%ret = %ret @ "p" @ intToHex(%pho) @ "_";
	%ret = %ret @ "w" @ intToHex(%weedKiller) @ "_";

	return %ret;
}

//handles applying given values to the dirt brick provided
function fxDTSBrick::setNutrients(%brick, %nit, %pho, %weedKiller)
{
	if (!%brick.getDatablock().isDirt)
	{
		return;
	}

	if (%nit $= "" || %pho $= "" || %weedKiller $= "")
	{
		%nutrients = decodeNutrientName(%brick);
		%currNit = getWord(%nutrients, 0);
		%currPho = getWord(%nutrients, 1);
		%currWeedKiller = getWord(%nutrients, 2);
	}
	%brick.setName(
		encodeNutrientName(
			(%nit $= "" ? %currNit : %nit),
			(%pho $= "" ? %currPho : %pho),
			(%weedKiller $= "" ? %currWeedKiller: %weedKiller)
			)
		);
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
		talk("Light level safety hit for " @ %brick @ "!");
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
function fxDTSBrick::attemptGrowth(%brick, %dirt, %nutrients, %light, %weather)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;
	if (!%db.isPlant)
	{
		return;
	}

	%waterReq = $Farming::PlantData_[%type, %stage, "waterPerTick"];
	%maxGrowTicks = $Farming::PlantData_[%type, %stage, "numGrowTicks"];
	%maxDryTicks = $Farming::PlantData_[%type, %stage, "numDryTicks"];
	%dryGrow = $Farming::PlantData_[%type, %stage, "dryStage"];
	%wetGrow = $Farming::PlantData_[%type, %stage, "nextStage"];

	%rainWaterMod = $Farming::PlantData_[%type, "rainWaterModifier"];
	%rainTimeMod = $Farming::PlantData_[%type, "rainTimeModifier"];
	%heatWaterMod = $Farming::PlantData_[%type, "heatWaveWaterModifier"];
	%heatTimeMod = $Farming::PlantData_[%type, "heatWaveTimeModifier"];

	
}

//if true, brick will be removed from the plant growth simset
function fxDTSBrick::canGrow(%brick)
{

}

//returns next tick time of the brick
function fxDTSBrick::getNextTickTime(%brick, %nutrients, %light)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;

	%tickTime = $Farming::PlantData_[%type, %stage, "tickTime"];

	//bigger difference between provided light and required -> bigger time
	//requiredLight = light level expected
	//lightAdjustTime = base time value, multiplied against difference between light and required light
	%requiredLight = $Farming::PlantData_[%type, "requiredLightLevel"] $= "" ? 1 : $Farming::PlantData_[%type, "requiredLightLevel"];
	%lightAdjustTime = $Farming::PlantData_[%type, "lightModifierTime"] $= "" ? %tickTime : $Farming::PlantData_[%type, "lightModifierTime"];
	%modifier = mAbs(%light - %requiredLight) * %lightAdjustTime;

	return getMax(1, %tickTime + %modifier); //lowest value is 1 second to prevent infinite recursion/fast plant checks
}