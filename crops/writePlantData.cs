function totalWater(%type, %startStage)
{
	%ticks = $Farming::PlantData_[%type, %startStage, "numWetTicks"];
	%time = $Farming::PlantData_[%type, %startStage, "tickTime"];
	%water = $Farming::PlantData_[%type, %startStage, "waterPerTick"];
	if (%time <= 0)
	{
		return 0;
	}
	else
	{
		return %water * %ticks + totalWater(%type, %startStage + 1);
	}
}

function totalTime(%type, %startStage)
{
	%ticks = $Farming::PlantData_[%type, %startStage, "numWetTicks"];
	%time = $Farming::PlantData_[%type, %startStage, "tickTime"];
	if (%time <= 0)
	{
		return 0;
	}
	else
	{
		return %time * %ticks + totalTime(%type, %startStage + 1);
	}
}

function writePlantData(%writeType)
{
	exec("Add-ons/Server_Farming/crops/plants/plantData.cs");
	exec("Add-ons/Server_Farming/config.cs");
	exec("Add-ons/Server_Farming/scripts/botDeals/selectionConfig.cs");
	%file = new FileObject();
	%file.openForWrite("Add-ons/Server_Farming/PlantInfo.txt");
	%file.writeLine("Generated " @ getDateTime() @ "");

	for (%i = 0; %i < $ProduceCount; %i++)
	{
		%type = getField($ProduceList_[%i], 0);
		%sellPrice = mFloatLength($Produce::BuyCost_[%type], 2);
		%seedPrice = mFloatLength($Produce::BuyCost_[%type @ "Seed"], 2);
		%pExp = $Farming::PlantData_[%type, "plantingExperience"] + 0;
		%hExp = $Farming::PlantData_[%type, "harvestExperience"] + 0;
		%expCost = $Farming::PlantData_[%type, "experienceCost"] + 0;

		%bestYield = "";
		for (%j = 0; %j < 30; %j++)
		{
			%yield = $Farming::PlantData_[%type, %j, "yield"];
			if (%yield !$= "" && getWord(%yield, 0) + getWord(%yield, 1) > getWord(%bestYield, 0) + getWord(%bestYield, 1))
			{
				%bestYield = %yield;
				%avgHarvest = (getWord(%bestYield, 0) + getWord(%bestYield, 1)) / 2;
			}
		}
		%growTime = totalTime(%type, 0) / 1000;
		%water = totalWater(%type, 0);
		%density = $Farming::PlantData_[%type, "plantSpace"];
		switch (%density)
		{
			case 0: %density = 8 * 8;
			case 1: %density = 4 * 4;
			case 2: %density = 3 * 3;
			case 3: %density = 2 * 2;
			case 4: %density = 2 * 2;
			case 5: %density = 1.5 * 1.5;
			case 6: %density = 1.5 * 1.5;
			case 7: %density = 1;
			default: %density = 8 / (%density + 1);
		}

		%file.writeLine("");
		%file.writeLine("");
		%file.writeLine("");
		%file.writeLine("------");
		%file.writeLine("-" @ %type @ "-");
		if (%writeType $= "" || strPos(strUpr(%writeType), "COST") >= 0)
		{
			%file.writeLine("$" @ %seedPrice @ " per seed");
			%file.writeLine("$" @ %sellPrice @ " per produce");
			%file.writeLine(%pExp @ "p + " @ %hExp @ "h - " @ %expCost @ " EXP");
			%file.writeLine("Avg " @ %avgHarvest @ " per harvest (range: " @ %bestYield @ ")");

			%isRepeatHarvest = 0;
			%loopTime = 0;
			if (%seedPrice > 100 || $Farming::PlantData_[%type, "loopStages"] !$= "")
			{
				%seedPrice = 0;
				%isRepeatHarvest = 1;
				//calculate loop time
				%loopStages = $Farming::PlantData_[%type, "loopStages"];
				for (%j = 0; %j < getWordCount(%loopStages); %j++)
				{
					%stage = getWord(%loopStages, %j);
					if ($Farming::PlantData_[%type, %stage, "tickTime"] > 0)
						%loopTime += $Farming::PlantData_[%type, %stage, "tickTime"] / 1000 * $Farming::PlantData_[%type, %stage, "numWetTicks"];
				}
				%maxHarvestRange = $Farming::PlantData_[%type, getWord(%loopStages, %j - 1), "maxHarvestTimes"];
				%avgHarvestTimes = (getWord(%maxHarvestRange, 0) + getWord(%maxHarvestRange, 1)) / 2;
			}
			if (!%isRepeatHarvest)
				%file.writeLine("Avg Profit Per Seed: " @ (%income = %avgHarvest * %sellPrice - %seedPrice) @ "");
			else
			{
				%file.writeLine("Avg Income Per Harvest: " @ (%income = %avgHarvest * %sellPrice) @ "");
				if (%avgHarvestTimes > 0)
				{
					%file.writeLine("Avg Total Harvests: " @ %avgHarvestTimes @ " (range: " @ %maxHarvestRange @ ")");
					%file.writeLine("Avg Total Income: " @ %avgHarvestTimes * %income);
				}
			}
			%file.writeLine("");
		}

		if (%writeType $= "" || strPos(strUpr(%writeType), "INFO") >= 0)
		{
			%file.writeLine("TTime: " @ getTimeString(%growTime) @ "");
			%file.writeLine("Water: " @ %water @ "");
			%file.writeLine("8x8 Density: " @ %density @ "");
			%file.writeLine("Water/sec: " @ mFloatLength(%water / %growTime, 2) @ "");
			%file.writeLine("Water/sec/8x8: " @ mFloatLength(%water / %growTime, 2) * %density @ "");
			%file.writeLine("8x8 Income: " @ mFloatLength(%density * %income, 2) @ "");

			if (!%isRepeatHarvest)
			{
				%file.writeLine("Income/min: " @ mFloatLength(%density * %income / %growTime * 60, 3) @ "");
				// %file.writeLine("Harvest Income/min: " @ mFloatLength(%density * %income / (%growTime + (200 + 15 * %density) / 16) * 60, 6) @ "");
			}
			else
			{
				%file.writeLine("LoopTime: " @ getTimeString(%loopTime));
				%file.writeLine("Income/min: " @ mFloatLength(%density * %income / %loopTime * 60, 3) @ "");
			}
		}
		%file.writeLine("------");
	}

	%file.close();
	%file.delete();
}