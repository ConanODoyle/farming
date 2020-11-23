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

	if (isObject(%hit))
	{
		%db = %hit.getDatablock();
		if (%db.isDirt)
		{
			%waterLevel = %hit.waterLevel + 0 @ "/" @ %db.maxWater;
			%string = "\c5" @ %db.uiName @ " \n";
			%string = %string @ "Water Level: " @ %waterLevel;
			%nutrients = %hit.getNutrients();
			%string = %string @ " \nNutrients: " @ getWord(%nutrients, 0) @ "nitrogen, " @ getWord(%nutrients, 1) @ "phosphate";
			%string = %string @ " \nMax Nutrients: " @ (%db.maxNutrients + 0);
			%string = %string @ " \nWeedkiller: " @ getWord(%nutrients, 2) @ "/"
				@ %db.maxWeedkiller @ " ";
			
			%cl.centerprint("<just:right><color:ffffff>" @ %string, 1);
		}
		else if (%db.isPlant)
		{
			if (%hit.lightLevel $= "" || %hit.nextUpdateInfo < $Sim::Time)
			{
				%weather = ($isHeatWave + 0) SPC ($isRaining + 0);
				%hit.lightLevel = getWord(getPlantLightLevel(%hit), 0);
				%hit.nextTickTime = %hit.getNextTickTime(%hit.getNutrients(), %hit.lightLevel, %weather);
				
				%cropType = %db.cropType;
				%nutrients = %hit.getNutrients();
				%requiredNutrients = getPlantData(%cropType, %db.stage, "nutrientStageRequirement");
				if (%requiredNutrients > 0)
				{
					%hit.requiresNutrients = getWords(vectorSub(%requiredNutrients, %nutrients), 0, 1);
				}
				else
				{
					%hit.requiresNutrients = "";
				}
				%hit.nextUpdateInfo = $Sim::Time + 1;
			}
			%string = "\c2" @ %db.cropType @ " \n";
			%string = %string @ "Light Level: " @ %hit.lightLevel * 100 @ "% \n";
			if (isObject(getPlantData(%db.cropType, %db.stage, "wetNextStage")) 
				|| isObject(getPlantData(%db.cropType, %db.stage, "dryNextStage")))
			{
				%string = %string @ "Time per growth tick: " @ %hit.nextTickTime @ "s \n";
			}
			else
			{
				%string = %string @ "Crop is fully grown! \n";
			}

			if (%hit.requiresNutrients !$= "")
			{
				%nitReq = getWord(%hit.requiresNutrients, 0);
				%phoReq = getWord(%hit.requiresNutrients, 1);
				if (%nitReq > 0)
				{
					%string = %string @ "Needs " @ %nitReq @  " nitrogen \n";
				}
				if (%phoReq > 0)
				{
					%string = %string @ "Needs " @ %phoReq @  " phosphate \n";
				}
			}
			
			%cl.centerprint("<just:right><color:ffffff>" @ %string, 1);
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
	else
	{
		%cl.centerprint("", 1);
	}
}
