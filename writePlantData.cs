function writePlantData()
{
	exec("Add-ons/Server_Farming/prices.cs");
	exec("Add-ons/Server_Farming/crops/plantData.cs");

	%file = new FileObject();
	%file.openForWrite("Add-ons/Server_Farming/PlantInfo.txt");
	%file.writeLine("Generated " @ getDateTime() @ "");

	for (%i = 0; %i < $ProduceCount; %i++)
	{
		%type = getField($ProduceList_[%i], 0);
		%sellPrice = mFloatLength($Produce::BuyCost_[%type], 2);
		%seedPrice = mFloatLength($Produce::BuyCost_[%type @ "Seed"], 2);
		%bestYield = "";
		for (%j = 0; %j < 20; %j++)
		{
			%yield = $Farming::Crops::PlantData_[%type, %j, "yield"];
			if (%yield !$= "" && getWord(%yield, 0) + getWord(%yield, 1) > getWord(%bestYield, 0) + getWord(%bestYield, 1))
			{
				%bestYield = %yield;
				%avgHarvest = (getWord(%bestYield, 0) + getWord(%bestYield, 1)) / 2;
			}
		}
		%growTime = totalTime(%type, 0) / 1000;
		%tomatoGrowTime = totalTime(%type, 2) / 1000;
		%water = totalWater(%type, 0);
		%density = $Farming::Crops::PlantData_[%type, "plantSpace"];
		switch (%density)
		{
			case 0: %density = 8 * 8;
			case 1: %density = 4 * 4;
			case 2: %density = 3 * 3;
			case 3: %density = 2 * 2;
		}

		%file.writeLine("");
		%file.writeLine("");
		%file.writeLine("");
		%file.writeLine("------");
		%file.writeLine("-" @ %type @ "-");
		%file.writeLine("$" @ %seedPrice @ " per seed");
		%file.writeLine("$" @ %sellPrice @ " per produce");
		%file.writeLine("Avg " @ %avgHarvest @ " per harvest (range: " @ %bestYield @ ")");

		if (%type !$= "Tomato")
			%file.writeLine("Avg Income Per Seed: " @ (%income = %avgHarvest * %sellPrice - %seedPrice) @ "");
		else
			%file.writeLine("Avg Income Per Harvest: " @ (%income = %avgHarvest * %sellPrice) @ "");

		%file.writeLine("");
		%file.writeLine("TTime: " @ %growTime @ "");
		%file.writeLine("Water: " @ %water @ "");
		%file.writeLine("8x8 Density: " @ %density @ "");
		%file.writeLine("Water/sec: " @ mFloatLength(%water / %growTime, 2) @ "");
		%file.writeLine("Water/sec/8x8: " @ mFloatLength(%water / %growTime, 2) * %density @ "");
		%file.writeLine("8x8 Income: " @ mFloatLength(%density * %income, 2) @ "");

		if (%type !$= "Tomato")
		{
			%file.writeLine("Income/min: " @ mFloatLength(%density * %income / %growTime * 60, 6) @ "");
			%file.writeLine("Harvest Income/min: " @ mFloatLength(%density * %income / (%growTime + (200 + 15 * %density) / 16) * 60, 6) @ "");
		}
		else
		{
			%file.writeLine("Income/min: " @ mFloatLength(%density * %income / %tomatoGrowTime * 60, 6) @ "");
			%file.writeLine("Harvest Income/min: " @ mFloatLength(%density * %income / (%tomatoGrowTime + 200 / 16) * 60, 6) @ "");
		}
		%file.writeLine("------");
	}

	%file.close();
	%file.delete();
}