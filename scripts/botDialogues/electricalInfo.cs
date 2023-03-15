if (!isObject($ElectricalInfoSet))
{
	$ElectricalInfoSet = new SimSet(ElectricalInfoSet);
}
$ElectricalInfoSet.deleteAll();

$obj = new ScriptObject(ElectricalInfoStart)
{
	response["Quit"] = "ExitResponse";

	messageCount = 2;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	message[1] = "I've got info on every electrical device out there!";
	messageTimeout[1] = 2;

	dialogueTransitionOnTimeout = "ElectricalInfoCore";
};
$ElectricalInfoSet.add($obj);

$obj = new ScriptObject(ElectricalInfoCore)
{
	response["InvalidSelection"] = "ElectricalInfoInvalidSelection";
	response["BasicInfo"] = "ElectricalInfoSelection";
	response["Quit"] = "ExitResponse";
	response["Error"] = "ErrorResponse";

	messageCount = 1;
	message[0] = "Which electrical device would you like to know more about?";
	messageTimeout[0] = 2;

	waitForResponse = 1;
	responseParser = "electricalInfoSelectionParser";
};
$ElectricalInfoSet.add($obj);

$obj = new ScriptObject(ElectricalInfoInvalidSelection)
{
	response["Quit"] = "ExitResponse";

	messageCount = 1;
	message[0] = "I can't tell you anything about that, sorry...";
	messageTimeout[0] = 2;

	dialogueTransitionOnTimeout = "ElectricalInfoCore";
};
$ElectricalInfoSet.add($obj);

$obj = new ScriptObject(ElectricalInfoSelection)
{
	response["Quit"] = "ExitResponse";

	messageCount = 3;
	message[0] = "%data1%";
	messageTimeout[0] = 3;
	message[1] = "%data2%";
	messageTimeout[1] = 3;
	message[2] = "%data3%";
	messageTimeout[2] = 3;

	functionOnStart = "setupElectricalInfo";
	dialogueTransitionOnTimeout = "ElectricalInfoCore";
};
$ElectricalInfoSet.add($obj);







function electricalInfoSelectionParser(%dataObj, %msg)
{
	%msg = trim(strLwr(%msg));

	%list = "light matrix battery batteries canner fertili solar ethanol pump coal control";

	for (%i = 0; %i < getWordCount(%list); %i++)
	{
		%word = getWord(%list, %i);
		if (striPos(%msg, %word) >= 0)
		{
			%dataObj.ElectricalInfoSelection = %word;
			break;
		}
	}

	if (%dataObj.ElectricalInfoSelection $= "")
	{
		return "Error";
	}

	if (%dataObj.ElectricalInfoSelection !$= "")
	{
		return "BasicInfo";
	}
	else
	{
		return "InvalidSelection";
	}
}

function setupElectricalInfo(%dataObj)
{
	%pl = %dataObj.player;
	%seller = %dataObj.speaker;
	%type = %dataObj.ElectricalInfoSelection;

	%basic = "You can get them from basic electrical quests - the two quest pads with the brown rim!";
	%advanced = "You can get them from advanced electrical quests - the two quest pads with the gray rim!";
	%light = "You can get them from simple quests - the two quest pads with the yellow rim!";
	
	switch$ (%type)
	{
		case "light":
			%s1 = "Lights use 1-2 power to provide 75% light to the area they cover! They allow you to grow crops that require light indoors.";
			%s2 = "Portobellos grow significantly faster under powered lights compared to indoors or outdoors!";
			%s3 = %light;
		case "matrix":
			%s1 = "The battery matrix is a larger battery, storing " @ brickBatteryMatrixData.capacity @ " power and discharging at a rate of " @ brickBatteryMatrixData.dischargeRate @ " watts per tick!";
			%s2 = "It's pretty big, but lets you save on battery slots in your power system.";
			%s3 = %advanced;
		case "battery":
			%s1 = "The battery stores energy, up to " @ brickBatteryData.capacity @ " power and discharging at a rate of " @ brickBatteryData.dischargeRate @ " watts per tick!";
			%s2 = "They can be hooked up to power systems and store excess unused energy.";
			%s3 = %basic;
		case "batteries":
			%s1 = "The battery stores energy, up to " @ brickBatteryData.capacity @ " power and discharging at a rate of " @ brickBatteryData.dischargeRate @ " watts per tick!";
			%s2 = "They can be hooked up to power systems and store excess unused energy.";
			%s3 = %basic;
		case "canner":
			%s1 = "The cannery takes " @ brickCanneryData.energyUse @ " energy per tick to create a can from a stack of crops!";
			%s2 = "Cans can be stacked to 10 and stored in basic crates, making storing crops extremely compact! You can later unpack the cans for sale.";
			%s3 = %advanced;
		case "fertili":
			%s1 = "The fertilizer mixer takes " @ brickFertilizerMixerData.energyUse @ " energy per tick to create fertilizer from compost and phosphate!";
			%s2 = "Fertilizer can be used to quickly grow plants, much like bone meal, and has a chance to turn them shiny for bonus yield!";
			%s3 = %advanced;
		case "solar":
			%s1 = "Solar panels generate power from sunlight! The 4x4 panel generates " @ brickSolarPanel4x4Data.generation @ " power per tick and can be found in basic quests.";
			%s2 = "The 8x8 panel generates " @ brickSolarPanel8x8Data.generation @ " power per tick and can be found in advanced quests.";
			%s3 = "Don't forget to turn them on after hooking them up!";
		case "ethanol":
			%s1 = "The ethanol refinery takes " @ brickEthanolRefineryData.energyUse @ " energy per tick to create ethanol from corn!";
			%s2 = "Ethanol generators burn ethanol to generate " @ brickEthanolGeneratorData.generation @ " energy per tick.";
			%s3 = "You can get them from both basic and advanced quests!";
		case "pump":
			%s1 = "Water pumps take energy to fill medium or large tanks, depending on the pump type! Line them up properly when placing them to ensure they fit.";
			%s2 = "Both medium and large tank pumps have configurable pump rates to let you determine how much power it should use.";
			%s3 = "Medium water pumps can be found in basic quests, and large water pumps can be found in advanced quests!";
		case "coal":
			%s1 = "Coal generators generate " @ brickCoalGeneratorData.generation @ " energy per tick when given coal!";
			%s2 = "You can buy a coal generator from the salesman in the building next to me!";
			%s3 = "Coal can be mined from the coal mine, or bought at Koals.";
		case "control":
			%s1 = "Power control boxes make up the core of your power network! All electrical devices need to be hooked up to one to work. Use electrical cable to connect devices!";
			%s2 = "Power boxes can have a maximum of " @ brickPowerControlBoxData.maxGenerators @ " generators, " @ brickPowerControlBoxData.maxProcessors @ " devices, and " @ brickPowerControlBoxData.maxBatteries @ " batteries attached.";
			%s3 = "You can buy one from the salesman in the building next to me!";
		default:
			%s1 = "...";
			%s2 = "...I don't know anything about that, sorry.";
			%s3 = "...";
	}

	%dataObj.var_data1 = %s1;
	%dataObj.var_data2 = %s2;
	%dataObj.var_data3 = %s3;
}