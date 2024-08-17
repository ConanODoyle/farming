datablock ItemData(OrganicAnalyzerItem : HammerItem)
{
	shapeFile = "./organicAnalyzer/organicanalyzer.dts";
	uiName = "Organic Analyzer";
	image = "OrganicAnalyzerImage";
	iconName = "Add-ons/Server_Farming/icons/OrganicAnalyzer";
	doColorShift = false;
	colorShiftColor = "0.2 0.6 0.2 1";
};

datablock ShapeBaseImageData(OrganicAnalyzerImage)
{
	shapeFile = "./organicAnalyzer/organicanalyzer.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = OrganicAnalyzerItem.colorShiftColor;

	item = "OrganicAnalyzerItem";

	armReady = 1;

	toolTip = "Displays dirt and crop information";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";
};

function OrganicAnalyzerImage::onLoop(%this, %obj, %slot)
{
	if (!isObject(%cl = %obj.client))
	{
		return;
	}

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::PlayerObjectType, %obj), 0);
	%db = %hit.dataBlock;

	if (!isObject(%hit))
	{
		%cl.centerprint("", 1);
		return;
	}
	
	if (%db.isDirt)
	{
		displayDirtStatus(%hit, %cl);
	}
	else if (%db.isPlant)
	{
		//periodically update info rather than always update, to reduce function calls/raycast spam
		if (%hit.nextUpdateInfo < $Sim::Time)
		{
			updatePlantStatus(%hit);
		}

		displayPlantStatus(%hit, %cl);
	}
	else if (%hit.client.player == %hit)
	{
		%string = "\c3" @ %hit.client.getPlayerName() @ " \n";
		if (isObject(%hit.getMountedImage(0)))
		{
			%string = %string @ "\c6" @ %hit.getMountedImage(0).item.uiName @ " \n";
		}

		%statTrak = %hit.getToolStatTrak();
		if (%statTrak !$= "")
		{
			%string = %string @ "\c4" @ %statTrak @ " ";
		}

		%cl.centerprint("<just:right>" @ %string, 1);
	}
}

function displayDirtStatus(%brick, %cl)
{
	%db = %brick.dataBlock;
	%waterLevel = %brick.waterLevel + 0 @ "/" @ %db.maxWater;
	%string = "\c5" @ %db.uiName @ " \n";
	%string = %string @ "\c6Water Level: " @ %waterLevel @ " \n";
	%nutrients = %brick.getNutrients();
	%string = %string @ "\c6Nutrients: " @ getWord(%nutrients, 0) @ " nitrogen, " @ getWord(%nutrients, 1) @ " phosphate" @ " \n";
	%string = %string @ "\c6Max Nutrients: " @ (%db.maxNutrients + 0) @ " \n";
	%string = %string @ "\c6Weedkiller: " @ getWord(%nutrients, 2) @ "/" @ %db.maxWeedkiller @ " ";
	
	%cl.centerprint("<just:right>\c6" @ %string, 1);
}

function updatePlantStatus(%brick)
{
	%db = %brick.dataBlock;
	%weather = ($isHeatWave + 0) SPC ($isRaining + 0);
	%light = getPlantLightLevel(%brick);

	%brick.lightLevel = getWord(%light, 0);
	%brick.inGreenhouse = getWord(%light, 1);
	%brick.nextTickTime = %brick.getNextTickTime(%brick.getNutrients(), %brick.lightLevel, %weather);
	
	%cropType = %db.cropType;
	%nutrients = %brick.getNutrients();
	%requiredNutrients = getPlantData(%cropType, %db.stage, "nutrientStageRequirement");
	if (vectorLen(%requiredNutrients) > 0)
	{
		%brick.requiresNutrients = %requiredNutrients;
	}
	else
	{
		%brick.requiresNutrients = "";
	}
	%brick.canGrow = %brick.canGrow();
	%brick.waterNeeded = getPlantData(%cropType, %db.stage, "waterPerTick");
	%brick.nutrients = %nutrients;
	%brick.harvestMax = getPlantData(%cropType, %db.stage, "harvestMax");
	%brick.willKillOnWetGrow = getPlantData(%cropType, %db.stage, "killOnWetGrow");
	%brick.willKillOnDryGrow = getPlantData(%cropType, %db.stage, "killOnDryGrow");
	%brick.maxWetTicks = getPlantData(%cropType, %db.stage, "numWetTicks");
	%brick.maxDryTicks = getPlantData(%cropType, %db.stage, "numDryTicks");
	%brick.nextUpdateInfo = $Sim::Time + 1;
}

function displayPlantStatus(%brick, %cl)
{
	%db = %brick.dataBlock;
	%string = "\c2" @ %db.cropType @ " \n";
	%string = %string @ "\c6Light Level: " @ mFloor(%brick.lightLevel * 100) @ "% \n";
	
	if (%brick.canGrow)
	{
		%string = %string @ "\c6Water per tick: " @ %brick.waterNeeded @ " \n";
		%string = %string @ "\c6Tick time: " @ %brick.nextTickTime @ "s \n";
	}
	else
	{
		%string = %string @ "\c6Crop is done growing! \n";
	}

	if (%brick.harvestMax > 1)
	{
		%string = %string @ "\c6Harvests: " @ getWord(%brick.nutrients, 2) @ "/" @ %brick.harvestMax @ " \n";
	}

	if (%brick.requiresNutrients !$= "")
	{
		%nitReq = getWord(%brick.requiresNutrients, 0);
		%phoReq = getWord(%brick.requiresNutrients, 1);
		%nitHas = getWord(%brick.nutrients, 0);
		%phoHas = getWord(%brick.nutrients, 1);
		if (%nitReq > 0)
		{
			%string = %string @ "\c6Has " @ %nitHas @ "/" @ %nitReq @  " needed nitrogen \n";
		}
		if (%phoReq > 0)
		{
			%string = %string @ "\c6Has " @ %phoHas @ "/" @  %phoReq @  " needed phosphate \n";
		}
	}

	if (%brick.canGrow && (%brick.willKillOnWetGrow || %brick.willKillOnDryGrow))
	{
		if (%brick.willKillOnWetGrow)
		{
			%maxWiltTicks = %brick.maxWetTicks;
			%curGrowTick  = %brick.wetTicks;
		}
		else
		{
			%maxWiltTicks = %brick.maxDryTicks;
			%curGrowTick  = %brick.dryTicks;
		}

		%timeTillWilt = (%maxWiltTicks - %curGrowTick) * %brick.nextTickTime;
		if (%timeTillWilt > 60)
		{
			%timeTillWilt = mFloatLength(%timeTillWilt / 60, 0) @ "m";
		}
		else
		{
			%timeTillWilt = %timeTillWilt @ "s";
		}
		%string = %string @ "\c6Wilts in: " @ %timeTillWilt;
	}
	
	%cl.centerprint("<just:right>\c6" @ %string, 1);
}