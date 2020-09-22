
///////////
//Utility//
///////////


//Math utility functions
//returns average gained seeds per seed given total harvestcount and drop probability
function getAverageExtraSeedsPerSeed(%harvestCount, %dropProb)
{
	%noDropProb = 1 - %dropProb;
	for (%i = 0; %i < %harvestCount; %i++)
	{
		%prob = 1;
		for (%j = 0; %j < %harvestCount; %j++)
		{
			%curr = %j <= %i ? %dropProb : %noDropProb;
			echo("   Curr: " @ %curr);
			%prob *= %j <= %i ? %dropProb : %noDropProb;
		}
		echo("Adding " @ %prob * %harvestCount * (%i + 1));
		%total += %prob * %harvestCount * (%i + 1);
	}
	return %total;
}

function getSeedsPerSeed(%harvestCount, %dropProb)
{
	%amt = getAverageExtraSeedsPerSeed(%harvestCount, %dropProb);
	return (1 / %amt) / (1 / %amt - 1);
}


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

//returns brick nutrients - nitrogen, phosphate, weedkiller/harvestCount
function fxDTSBrick::getNutrients(%brick)
{
	return decodeNutrientName(%brick.getName());
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
		return %lightLevel * getPlantData(%brick.getDatablock().cropType, "lightLevelFactor");
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
//returns 0 for growth success, nonzero for failure
function fxDTSBrick::attemptGrowth(%brick, %dirt, %plantNutrients, %light, %weather)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;
	if (!%db.isPlant)
	{
		return -1;
	}

	%waterReq = getPlantData(%type, %stage, "waterPerTick");
	%maxWetTicks = getPlantData(%type, %stage, "numWetTicks");
	%maxDryTicks = getPlantData(%type, %stage, "numDryTicks");
	%dryGrow = getPlantData(%type, %stage, "dryNextStage");
	%wetGrow = getPlantData(%type, %stage, "wetNextStage");
	%killOnDryGrow = getPlantData(%type, %stage, "killOnDryGrow");
	%killOnWetGrow = getPlantData(%type, %stage, "killOnWetGrow");
	%requiredLight = getPlantData(%type, "requiredLightLevel") $= "" ? 1 : getPlantData(%type, "requiredLightLevel");

	%isRaining = getWord(%weather, 0);
	%isHeatWave = getWord(%weather, 1);
	%rainWaterMod = getPlantData(%type, "rainWaterModifier");
	%heatWaterMod = getPlantData(%type, "heatWaveWaterModifier");

	%nutrientUse = getPlantData(%type, %stage, "nutrientUsePerTick");
	%wetNutriUse = getPlantData(%type, %stage, "nutrientUseIfWet");
	%dryNutriUse = getPlantData(%type, %stage, "nutrientUseIfDry");
	%levelUpRequirement = getPlantData(%type, %stage, "nutrientStageRequirement");

	if (%requiredLight == 1 && %light == 0) //plant requires full light to grow, no light available
	{
		return 1;
	}

	//adjust water level based on weather and greenhouse presence
	if (%brick.greenhouseBonus)
	{
		%waterReq = mCeil(%waterReq / 2);
	}
	else
	{
		if (%isRaining) //raining
		{
			%waterReq = mCeil(%waterReq, %rainWaterMod);
		}
		else if (%isHeatWave)
		{
			%waterReq = mCeil(%waterReq, %heatWaterMod);
		}
	}

	//attempt growth based on water and nutrient level
	if (%dirt.waterLevel < %waterReq)
	{
		%brick.dryTicks++;
		if (%dryNutriUse)
		{
			%plantNutrients = vectorSub(%plantNutrients, %nutrientUse);
			%brick.setNutrients(getWord(%plantNutrients, 0), getWord(%plantNutrients, 1));
		}

		%nutrientDiff = vectorSub(%plantNutrients, %levelUpRequirement);
		if (%brick.dryTicks > %maxDryTicks && %maxDryTicks != -1)
		{
			if (isObject(%dryGrow) && strPos(%nutrientDiff, "-") < 0) //nutrients available >= %nutrientUse
			{
				%brick.setDatablock(%dryGrow);
				%brick.dryTicks = 0;
				%brick.wetTicks = 0;
				%growth = 1;
			}
			else if (%killOnDryGrow)
			{
				%death = 1;
			}
		}
	}
	else
	{
		%brick.wetTicks++;
		%dirt.setWaterLevel(%dirt.waterLevel - %waterReq);
		if (%wetNutriUse)
		{
			%plantNutrients = vectorSub(%plantNutrients, %nutrientUse);
			%brick.setNutrients(getWord(%plantNutrients, 0), getWord(%plantNutrients, 1));
		}
		
		%nutrientDiff = vectorSub(%plantNutrients, %levelUpRequirement);
		if (%brick.wetTicks > %maxWetTicks && %maxWetTicks != -1)
		{
			if (isObject(%wetGrow) && strPos(%nutrientDiff, "-") < 0)
			{
				%brick.setDatablock(%wetGrow);
				%brick.wetTicks = 0;
				%brick.wetTicks = 0;
				%growth = 1;
			}
			else if (%killOnWetGrow)
			{
				%death = 1;
			}
		}
	}

	//effects
	if (%growth)
	{
		// Growth particles
		%p = new Projectile()
		{
			dataBlock = "FarmingPlantGrowthProjectile";
			initialVelocity = "0 0 1";
			initialPosition = %brick.position;
		};

		if (isObject(%p))
		{
			%p.explode();
		}
	}

	if (%death)
	{
		// Growth particles
		%p = new Projectile()
		{
			dataBlock = "deathProjectile";
    		scale = "0.2 0.2 0.2";
			initialVelocity = "0 0 -10";
			initialPosition = %brick.position;
		};

		if (isObject(%p))
		{
			%p.explode();
		}

		%brick.delete();
	}
	return 0;
}

//indicates if this brick can potentially grow. if false, brick will be removed from the plant growth simset
function fxDTSBrick::canGrow(%brick)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;
	if (!%db.isPlant)
	{
		return 0;
	}

	%dryGrow = getPlantData(%type, %stage, "dryNextStage");
	%wetGrow = getPlantData(%type, %stage, "wetNextStage");
	%killOnDryGrow = getPlantData(%type, %stage, "killOnDryGrow");
	%killOnWetGrow = getPlantData(%type, %stage, "killOnWetGrow");
	if (isObject(%dryGrow) || isObject(%wetGrow) || %killOnDryGrow || %killOnWetGrow)
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

function fxDTSBrick::extractNutrients(%brick, %nutrients)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;
	if (!%db.isPlant || %nutrients $= "0 0 0")
	{
		return %nutrients;
	}

	%maxNutrients = getPlantData(%type, %stage, "maxNutrients") $= "" ?
		getPlantData(%type, "maxNutrients") : getPlantData(%type, %stage, "maxNutrients");
	%brickNutrients = %brick.getNutrients();
	%space = vectorSub(%maxNutrients, %brickNutrients);
	if (%space !$= "0 0 0" && strPos(%space, "-") < 0)
	{
		%nit = getMin(getWord(%nutrients, 0), getWord(%space, 0));
		%pho = getMin(getWord(%nutrients, 1), getWord(%space, 1));
		%weedKiller = getWord(%nutrients, 2);

		%brick.setNutrients(getWord(%brickNutrients, 0) + %nit, getWord(%brickNutrients, 1) + %pho);
		%nutrients = vectorSub(%nutrients, %nit SPC %pho SPC %weedKiller);
	}
	return %nutrients;
}

//returns next tick time of the brick
function fxDTSBrick::getNextTickTime(%brick, %nutrients, %light, %weather)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;

	%tickTime = getPlantData(%type, %stage, "tickTime");
	%nutrientTimeModifier = getPlantData(%type, %stage, "nutrientTimeModifier");
	%rainTimeMod = getPlantData(%type, "rainTimeModifier");
	%heatTimeMod = getPlantData(%type, "heatWaveTimeModifier");
	
	%isRaining = getWord(%weather, 0);
	%isHeatWave = getWord(%weather, 1);

	//bigger difference between provided light and required -> bigger time
	//requiredLight = light level expected
	//lightAdjustTime = base time value, multiplied against difference between light and required light
	%requiredLight = getPlantData(%type, "requiredLightLevel") $= "" ? 1 : getPlantData(%type, "requiredLightLevel");
	%lightAdjustTime = getPlantData(%type, "lightTimeModifier") $= "" ? %tickTime : getPlantData(%type, "lightTimeModifier");
	%lightModifier = mAbs(%light - %requiredLight) * %lightAdjustTime;

	//nitrogen/phosphate growth time factor
	%nit = getWord(%nutrients, 0);
	%pho = getWord(%nutrients, 1);
	%nutrientModifier = eval("%nit=" @ %nit @ ";%pho=" @ %pho @ ";return " @ %nutrientTimeModifier @ ";");

	%multi = 1;
	if (%isRaining) //raining
	{
		%multi = %multi * %rainTimeMod;
	}
	
	if (%isHeatWave)
	{
		%multi = %multi * %heatTimeMod;
	}

	//lowest value is 1 second to prevent infinite recursion/fast plant checks
	return getMax(1, (%tickTime + %lightModifier + %nutrientModifier) * %multi);
}