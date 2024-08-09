
///////////
//Utility//
///////////





// Resource functions
//returns dirt bricks under plant, in order of highest water
//bricks with same water level are slightly randomized
function fxDTSBrick::getDirtList(%brick)
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

function fxDTSBrick::getHarvestCount(%brick)
{
	return getWord(decodeNutrientName(%brick.getName()), 2);
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

	return (%nit + 0) SPC (%pho + 0) SPC (%weedKiller + 0);
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
	if (!%brick.getDatablock().isDirt && !%brick.getDatablock().isPlant)
	{
		return;
	}

	if (%nit $= "" || %pho $= "" || %weedKiller $= "")
	{
		%nutrients = decodeNutrientName(%brick.getName());
		%currNit = getWord(%nutrients, 0);
		%currPho = getWord(%nutrients, 1);
		%currWeedKiller = getWord(%nutrients, 2);
	}
	%brick.setName(
		encodeNutrientName(
			(%nit $= "" ? %currNit : %nit),
			(%pho $= "" ? %currPho : %pho),
			(%weedKiller $= "" ? %currWeedKiller : %weedKiller)
			)
		);
}

function fxDTSBrick::addNutrients(%brick, %nit, %pho, %weedKiller)
{
	%db = %brick.dataBlock;
	if (!%db.isDirt && !%db.isPlant)
	{
		return;
	}

	%nutrients = decodeNutrientName(%brick.getName());
	%currNit = getWord(%nutrients, 0);
	%currPho = getWord(%nutrients, 1);
	%currWeedKiller = getWord(%nutrients, 2);

	%newNit = %currNit + %nit;
	%newPho = %currPho + %pho;
	%newWeedkiller = %currWeedKiller + %weedKiller;

	if (%newNit > %db.maxNutrients || %newNit < 0)
	{
		%excessNit = %newNit > %db.maxNutrients ? %newNit - %db.maxNutrients : %newNit;
		%newNit = getMin(getMax(%newNit, 0), %db.maxNutrients);
	}

	if (%newPho > %db.maxNutrients || %newPho < 0)
	{
		%excessPho = %newPho > %db.maxNutrients ? %newPho - %db.maxNutrients : %newPho;
		%newPho = getMin(getMax(%newPho, 0), %db.maxNutrients);
	}

	if (%newWeedkiller > %db.maxWeedkiller || %newWeedkiller < 0)
	{
		%excessWeedkiller = %newWeedkiller > %db.maxWeedkiller ? %newWeedkiller - %db.maxWeedkiller : %newWeedkiller;
		%newWeedkiller = getMin(getMax(%newWeedkiller, 0), %db.maxWeedkiller);
	}

	%brick.setName(encodeNutrientName(%newNit, %newPho, %newWeedkiller));
	return (%excessNit + 0) SPC (%excessPho + 0);
}




// Light functions
//returns lightlevel SPC greenhouseFound
//lightlevel should be a float between 0 and 1
function getPlantLightLevel(%brick)
{
	//start is at half a plate above the bottom of the brick
	%start = vectorAdd(%brick.getPosition(), "0 0 -" @ ((%brick.getDatablock().brickSizeZ * 0.1) - 0.1));

	//inner center of the brick
	//1x1 or 2x2 above the brick, based on the brick dimensions
	%db = %brick.dataBlock;
	%side = %db.brickSizeX;
	%pos = vectorAdd(%brick.getPosition(), "0 0 -" @ (%db.brickSizeZ * 0.1 - 0.1));
	if (%side % 2 == 1)
	{
		%pos0 = %pos;
		%count = 1;
	}
	else
	{
		%pos0 = vectorAdd(%pos, "0.25 0.25 0");
		%pos1 = vectorAdd(%pos, "0.25 -0.25 0");
		%pos2 = vectorAdd(%pos, "-0.25 0.25 0");
		%pos3 = vectorAdd(%pos, "-0.25 -0.25 0");
		%count = 4;
	}

	for (%i = 0; %i < %count; %i++)
	{
		%pointLevel = lightRaycastCheck(%pos[%i], %brick);
		%light += getWord(%pointLevel, 0);
		%greenhouseFound += getWord(%pointLevel, 1);
	}
	%light = %light / %count;

	return %light SPC %greenhouseFound;
}

function lightRaycastCheck(%pos, %brick)
{
	%start = %pos;
	%end = vectorAdd(%start, "0 0 100");
	%masks = $Typemasks::fxBrickAlwaysObjectType;

	%ray = containerRaycast(%start, %end, %masks, %brick);
	%light = 1;
	%greenhouseFound = 0;
	while (%safety++ < 4)
	{
		%hit = getWord(%ray, 0);
		if (!isObject(%hit) || %hit.getGroup().bl_id == 888888)
		{
			%lightSourceFound = 1;
			break;
		}

		if (!%hit.canLightPassThrough())
		{
			if (!%lightLevelFixed)
			{
				%light = %hit.getLightLevel(%light);
			}
			break;
		}

		%hitDB = %hit.dataBlock;
		if (%hitDB.isGreenhouse)
		{
			//are we (the plant) in the greenhouse? or is it way above us?
			%z = getWord(%hit.getPosition(), 2) - %hitDB.brickSizeZ * 0.1;
			if (getWord(%pos, 2) >= %z)
			{
				%greenhouseFound = 1;
				if (%lightSourceFound)
				{
					break;
				}
			}
		}

		if (!%lightLevelFixed)
		{
			%light = %hit.getLightLevel(%light);
			if (%light == 0)
			{
				break;
			}
		}

		if (%hitDB.isIndoorLight)
		{
			%lightSourceFound = 1;
			%lightLevelFixed = 1;
			if (%greenhouseFound)
			{
				break;
			}
		}
	
		%start = getWords(%ray, 1, 3);
		%ray = containerRaycast(%start, %end, %masks, %brick, %hit);
		continue;
	}
	if (%light == 1 && !%lightSourceFound)
	{
		%light = 0;
	}

	return %light SPC %greenhouseFound;
}

function fxDTSBrick::getLightLevel(%brick, %lightLevel)
{
	%db = %brick.dataBlock;
	if (%db.customLightLevel)
	{
		return %db.customLightLevel;
	}
	else if (%db.isTree)
	{
		return %lightLevel * getPlantData(%db.cropType, "lightLevelFactor");
	}
	else if (%db.isSprinkler || %db.isGreenhouse || %db.allowLightThrough)
	{
		return %lightLevel;
	}

	return 0; //normal bricks block all light
}

function fxDTSBrick::canLightPassThrough(%brick)
{
	%db = %brick.dataBlock;
	if (%db.isTree || %db.isSprinkler || %db.isGreenhouse || %db.isIndoorLight || %db.allowLightThrough)
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
	%db = %brick.dataBlock;
	%type = %db.cropType;
	%stage = %db.stage;
	if (!%db.isPlant || !isObject(%dirt))
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

	%nutrientAdd = getPlantData(%type, %stage, "nutrientAddPerTick");
	%wetNutriAdd = getPlantData(%type, %stage, "nutrientAddIfWet");
	%dryNutriAdd = getPlantData(%type, %stage, "nutrientAddIfDry");
	%levelUpRequirement = getPlantData(%type, %stage, "nutrientStageRequirement");
	if (isObject(%dirt))
	{
		%dirtNutrients = %dirt.getNutrients();
	}

	if (%requiredLight == 1 && %light == 0) //plant requires full light to grow, no light available
	{
		return 1;
	}

	//adjust water level based on weather and greenhouse presence
	if (%brick.inGreenhouse)
	{
		%waterReq = mCeil(%waterReq / 2);
	}
	else
	{
		if (%isRaining) //raining
		{
			%waterReq = mCeil(%waterReq * %rainWaterMod);
		}
		else if (%isHeatWave)
		{
			%waterReq = mCeil(%waterReq * %heatWaterMod);
		}
	}

	//attempt growth based on water and nutrient level
	if (%dirt.waterLevel < %waterReq)
	{
		%brick.dryTicks++;
		%brick.wetTicks = getMax(%brick.wetTicks - 1, 0);
		if (%dryNutriAdd)
		{
			%dirt.addNutrients(getWord(%nutrientAdd, 0), getWord(%nutrientAdd, 1), getWord(%nutrientAdd, 2));
		}

		%nutrientDiff = vectorSub(%plantNutrients, %levelUpRequirement);
		if (%brick.dryTicks > %maxDryTicks && %maxDryTicks != -1)
		{
			if (isObject(%dryGrow) && strPos(%nutrientDiff, "-") < 0) //nutrients available >= %nutrientUse
			{
				%brick.grow(%dryGrow);
			}
			else if (%killOnDryGrow)
			{
				%brick.killPlant();
			}
		}
	}
	else
	{
		%brick.wetTicks++;
		%brick.dryTicks = getMax(%brick.dryTicks - 1, 0);
		%dirt.setWaterLevel(%dirt.waterLevel - %waterReq);
		if (%wetNutriAdd)
		{
			%dirt.addNutrients(getWord(%nutrientAdd, 0), getWord(%nutrientAdd, 1), getWord(%nutrientAdd, 2));
		}
		
		%nutrientDiff = vectorSub(%plantNutrients, %levelUpRequirement);
		if (%brick.wetTicks > %maxWetTicks && %maxWetTicks != -1)
		{
			if (isObject(%wetGrow) && strPos(%nutrientDiff, "-") < 0)
			{
				if (vectorLen(%levelUpRequirement) > 0)
				{
					%brick.setNutrients(getWord(%nutrientDiff, 0), getWord(%nutrientDiff, 1));
				}
				%brick.grow(%wetGrow);
			}
			else if (%killOnWetGrow)
			{
				%brick.killPlant();
			}
		}
	}

	return 0;
}

//indicates if this brick can potentially change when growing
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
	%numWetTicks = getPlantData(%type, %stage, "numWetTicks");
	%numDryTicks = getPlantData(%type, %stage, "numDryTicks");
	if (isObject(%dryGrow) || isObject(%wetGrow) || %killOnDryGrow || %killOnWetGrow
		|| %numWetTicks > %brick.wetTicks || %numDryTicks > %brick.dryTicks)
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

function getTotalDirtNutrients(%dirtList)
{
	for (%i = 0; %i < getWordCount(%dirtList); %i++)
	{
		%dirt = getWord(%dirtList, %i);
		%nutrients = vectorAdd(%nutrients, %dirt.getNutrients());
	}
	return %nutrients;
}

//returns 0 for fail, 1 for success
//removes %removeTotal amount of nutrients approximately evenly across all the dirt in %dirtList
function removeTotalDirtNutrients(%dirtList, %removeTotal)
{
	if (vectorLen(%removeTotal) <= 0.1)
	{
		return 1;
	}

	for (%i = 0; %i < getWordCount(%dirtList); %i++)
	{
		%dirt[%i] = getWord(%dirtList, %i);
		%nutrients = vectorAdd(%nutrients, %dirt[%i].getNutrients());
	}
	if (getWord(%nutrients, 0) < getWord(%removeTotal, 0)
		|| getWord(%nutrients, 1) < getWord(%removeTotal, 1))
	{
		error("ERROR: cannot remove " @ %removeTotal @ " from dirt; total nutrients under required amount!");
		error("    dirtList: " @ %dirtList @ " nutrients: " @ %nutrients);
		return 0;
	}

	%removeTotal = getWords(%removeTotal, 0, 1);

	//two loops
	//first loop - remove proportional amount of nutrients from each brick
	//second loop - remove as much possible to clear the remainder
	//not perfectly balanced, but easier to debug/less processing required.
	%removedNit = 0;
	%removedPho = 0;
	%avg = vectorScale(%removeTotal, 1 / getWordCount(%dirtList));
	%avgNit = mFloor(getWord(%avg, 0));
	%avgPho = mFloor(getWord(%avg, 1));
	for (%i = 0; %i < getWordCount(%dirtList); %i++)
	{
		//announce("Dirt: " @ %dirt[%i].getNutrients() @ " Removing: " @ %avgNit SPC %avgPho);
		%remainder = %dirt[%i].addNutrients(-1 * %avgNit, -1 * %avgPho);
		%removeTotal = vectorSub(%removeTotal, %remainder);
		%removeTotal = vectorSub(%removeTotal, %avgNit SPC %avgPho);
		//announce("   Remainder: " @ %remainder @ " Dirt: " @ %dirt[%i].getNutrients() @ " removeTotal: " @ %removeTotal);
	}

	for (%i = 0; %i < getWordCount(%dirtList); %i++)
	{
		//announce("Dirt: " @ %dirt[%i].getNutrients() @ " Removing: " @ %removeTotal);
		%remainder = %dirt[%i].addNutrients(-1 * getWord(%removeTotal, 0),  -1 * getWord(%removeTotal, 1));
		%removeTotal = vectorSub(vectorSub(%removeTotal, %remainder), %removeTotal);
		//announce("   Remainder: " @ %remainder @ " Dirt: " @ %dirt[%i].getNutrients() @ " removeTotal: " @ %removeTotal);
		if (vectorLen(%removeTotal) <= 0.01)
		{
			return 1;
		}
	}

	if (vectorLen(%removeTotal) > 0)
	{
		talk("ERROR: Remaining nutrients after removing: " @ %removeTotal);
		return 0;
	}
	return 1;
}

//adds nutrients to the specified brick, taking from %nutrients. intended for plant bricks, not dirt.
//returns leftover nutrients - only withdraws up to %nutrients provided amount
function fxDTSBrick::extractNutrients(%brick, %nutrients)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;
	if (!%db.isPlant || %nutrients $= "0 0 0")
	{
		return %nutrients;
	}

	%availableNitrogen = getWord(%nutrients, 0);
	%availablePhosphate = getWord(%nutrients, 1);

	%maxNutrients = getPlantData(%type, %stage, "maxNutrients");

	%maxNitrogen = getWord(%maxNutrients, 0) | 0;
	%maxPhosphate = getWord(%maxNutrients, 1) | 0;

	%brickNutrients = %brick.getNutrients();

	%brickNitrogen = getWord(%brickNutrients, 0) | 0;
	%brickPhosphate = getWord(%brickNutrients, 1) | 0;

	%desiredNitrogen = %maxNitrogen - %brickNitrogen;
	%desiredPhosphate = %maxPhosphate - %brickPhosphate;

	%extractedNitrogen = 0;
	%extractedPhosphate = 0;

	if (%desiredNitrogen > 0 && %availableNitrogen > 0)
	{
		%remainingNitrogen = getMax(%availableNitrogen - %desiredNitrogen, 0);
		%extractedNitrogen = %availableNitrogen - %remainingNitrogen;
		%nutrients = setWord(%nutrients, 0, %remainingNitrogen);
	}

	if (%desiredPhosphate > 0 && %availablePhosphate > 0)
	{
		%remainingPhosphate = getMax(%availablePhosphate - %desiredPhosphate, 0);
		%extractedPhosphate = %availablePhosphate - %remainingPhosphate;
		%nutrients = setWord(%nutrients, 1, %remainingPhosphate);
	}

	%brick.setNutrients(%brickNitrogen + %extractedNitrogen, %brickPhosphate + %extractedPhosphate);

	return %nutrients;
}

//returns next tick time of the brick
function fxDTSBrick::getNextTickTime(%brick, %nutrients, %light, %weather)
{
	%db = %brick.getDatablock();
	%type = %db.cropType;
	%stage = %db.stage;

	if (%nutrients $= "") %nutrients = %brick.getNutrients();
	if (%light $= "") %light = getPlantLightLevel(%brick);
	if (%weather $= "") %weather = ($isRaining + 0) SPC ($isHeatWave + 0);

	%tickTime = getPlantData(%type, %stage, "tickTime");
	%nutrientTimeModifier = getPlantData(%type, %stage, "nutrientTimeModifier");
	%rainTimeMod = getPlantData(%type, "rainTimeModifier");
	%heatTimeMod = getPlantData(%type, "heatWaveTimeModifier");
	
	%isRaining = getWord(%weather, 0);
	%isHeatWave = getWord(%weather, 1);

	//bigger difference between provided light and required -> bigger time
	//requiredLight = light level expected
	//lightAdjustTime = base time value, multiplied against difference between light and required light
	if (getPlantData(%type, "requiredLightLevel") $= "")
	{
		%requiredLight = 1;
	}
	else
	{
		%requiredLight = getPlantData(%type, "requiredLightLevel");
	}
	%lightAdjustTime = getPlantData(%type, "lightTimeModifier") $= "" ? %tickTime : getPlantData(%type, "lightTimeModifier");
	%lightModifier = mAbs(%light - %requiredLight) * %lightAdjustTime;

	//nitrogen/phosphate growth time factor
	%nit = getWord(%nutrients, 0) + 0;
	%pho = getWord(%nutrients, 1) + 0;
	if (%nutrientTimeModifier !$= "")
	{
		%nutrientModifier = eval("%nit=" @ %nit @ ";%pho=" @ %pho @ ";return " @ %nutrientTimeModifier @ ";");
	}

	%multi = 1;
	if (%isRaining) //raining
	{
		%multi = %multi * %rainTimeMod;
	}
	
	if (%isHeatWave)
	{
		%multi = %multi * %heatTimeMod;
	}

	if (%brick.inGreenhouse)
	{
		%multi *= 0.5;
	}

	//lowest value is 1 second to prevent infinite recursion/fast plant checks
	return getMax(1, (%tickTime + %lightModifier + %nutrientModifier + %weedTimeModifier) * %multi);
}

function fxDTSBrick::grow(%brick, %growDB)
{
	%brick.setDatablock(%growDB);
	%brick.dryTicks = 0;
	%brick.wetTicks = 0;

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

function fxDTSBrick::killPlant(%brick)
{
	// Growth particles
	%p = new Projectile()
	{
		dataBlock = "deathProjectile";
		scale = "0.2 0.2 0.2";
		initialVelocity = "0 0 -10";
		initialPosition = %brick.position;
	};

	%brick.schedule(33, delete);
}



// Dirt and plant status analyzing
package DirtStatus
{
	function Player::activateStuff(%obj)
	{
		if (isObject(%cl = %obj.client))
		{
			%start = %obj.getEyeTransform();
			%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			%db = %hit.dataBlock;
			if (isObject(%hit) && (%db.isDirt || %db.isWaterTank))
			{
				%waterLevel = %hit.waterLevel + 0 @ "/" @ %db.maxWater;
				%string = "Water Level: " @ %waterLevel @ " ";
				if (%db.isDirt)
				{
					%nutrients = %hit.getNutrients();
					if (getWord(%nutrients, 0) > 0 || getWord(%nutrients, 1) > 0)
					{
						%string = %string @ "\nHas nutrients ";
					}
					if (getWord(%nutrients, 2) > 0)
					{
						%string = %string @ "\nHas weedkiller ";
					}
				}

				%cl.centerprint("<just:right><color:ffffff>" @ %string, 1);
				%cl.schedule(50, centerprint, "<just:right><color:cccccc>" @ %string, 2);
			}
		}

		return parent::activateStuff(%obj);
	}
};
activatePackage(DirtStatus);