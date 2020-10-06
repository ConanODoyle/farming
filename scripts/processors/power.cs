//dataID tags:
//	powerType - determines if the brick is a generator, processor, powercontrolbox, or battery
//	isPoweredOn - determines if the brick is running
//	brickName - the name of the brick associated with this processor
//	powerControlBox - the power control box dataID this processor is connected to
//
//Generators check internal inventory for resources to burn when powering a system
//generator fields:
//	isGenerator - true
//	burnRate - # of resources to burn per tick
//	generation - # of watts created per tick
//	fuelType - space-delimited stacktypes of the fuels it accepts
//
//Powered Processors draw power, and if supplied enough, run their internal tasks
//processor fields:
//	isPoweredProcessor - true
//	energyUse - # of watts consumed per tick
//	powerFunction - function to call with power ratio
//
//Batteries pick up excess power from the network, and discharge it when generation is not enough
//battery fields:
//	isBattery - true
//	dischargeRate - maximum discharge/charge rate (watts per tick)
//	capacity - maximum storable watts per tick
//tags:
//	charge - amount of watt-ticks stored
//
//Power Control Boxes display network power information and connect generators to powered processors
//powercontrolbox fields:
//	isPowerControlBox - true
//	maxGenerators - maximum number of attached generators
//	maxProcessors - maximum number of attached processors
//	maxBatteries - maximum number of attached batteries
//	tickTime - seconds between ticks


if (!isObject(PowerControlSimSet)) 
{
	$PowerControlSimSet = new SimSet(PowerControlSimSet) {};
}

package PowerSystems
{
	function fxDTSBrick::onAdd(%obj) 
	{
		%ret = parent::onAdd(%obj);

		%db = %obj.getDatablock();
		if (%obj.isPlanted)
		{
			if (%db.isPowerControlBox)
			{
				PowerControlSimSet.add(%obj);
			}

			if (%db.isPowerControlBox || %db.isPoweredProcessor || %db.isBattery || %db.isGenerator)
			{
				%obj.schedule(100, autoAddPowerControlSystem);
			}
		}

		return %ret;
	}

	function fxDTSBrick::onRemove(%obj)
	{
		%db = %obj.getDatablock();
		if (%db.isPowerControlBox || %db.isPoweredProcessor || %db.isBattery || %db.isGenerator)
		{
			%powerDataID = getSubStr(%obj.getName(), 1, 50);
			if (%powerDataID !$= "")
			{
				disconnectFromControlSystem(%obj, %powerDataID, 1);
			}
		}

		return parent::onRemove(%obj);
	}

	function addStorageEvent(%brick)
	{
		parent::addStorageEvent(%brick);
		%db = %brick.getDatablock();
		if (%db.isGenerator || %db.isPoweredProcessor || %db.isPowerControlBox || %db.isBattery)
		{
			if (%db.isGenerator) %type = "generator";
			else if (%db.isPoweredProcessor) %type = "processor";
			else if (%db.isBattery) %type = "battery";
			else if (%db.isPowerControlBox) %type = "powerControlBox";

			if (%type !$= "")
			{
				%dataID = %brick.eventOutputParameter0_1;
				setDataIDArrayTagValue(%dataID, "powerType", %type);
			}
		}
	}

	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID)
	{
		if (%storageObj.getDatablock().isGenerator && !%storageObj.isAcceptingFuel)
		{
			return 3;
		}
		else if (%storageObj.getDatablock().isPowerControlBox)
		{
			return 2;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID);
		%db = %brick.getDatablock();
		if (%db.isGenerator)
		{
			%brick.centerprintMenu.menuOptionCount = 2;
			%brick.centerprintMenu.menuOption[1] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[1] = "togglePower";
		}
		else if (%db.isPowerControlBox)
		{
			%brick.centerprintMenu.menuOptionCount = 8;
			%brick.centerprintMenu.menuOption[0] = "\c5v Diagnostics v";
			if (%brick.isInputOn())
				%brick.centerprintMenu.menuOption[1] = "Producing " @ %brick.totalGeneratedPower + 0 @ " watts";
			else
				%brick.centerprintMenu.menuOption[1] = "[Production off]";

			if (%brick.isOutputOn())
				%brick.centerprintMenu.menuOption[2] = "Using " @ %brick.totalPowerUsage + 0 @ " watts";
			else
				%brick.centerprintMenu.menuOption[2] = "[Energy usage off]";

			if (%brick.getBatteryMode() $= "Charging")
				%color = "\c2";
			%brick.centerprintMenu.menuOption[3] = %color @ "Battery: " @ %brick.totalBatteryPower + 0 @ " watt-ticks"; 
			%brick.centerprintMenu.menuOption[4] = "\c5v Controls v";
			%brick.centerprintMenu.menuOption[5] = "Input: " @ (%brick.isInputOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuOption[6] = "Output: " @ (%brick.isOutputOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuOption[7] = "Battery Mode: " @ (%brick.getBatteryMode());

			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[1] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[2] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[3] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[4] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[5] = "toggleInputOn";
			%brick.centerprintMenu.menuFunction[6] = "toggleOutputOn";
			%brick.centerprintMenu.menuFunction[7] = "toggleBatteryMode";
		}
		else if (%db.isBattery)
		{
			%brick.centerprintMenu.menuOptionCount = 2;
			%brick.centerprintMenu.menuOption[0] = "Stored: " @ getDataIDArrayTagValue(%dataID, "charge") + 0
					 @ "/" @ %db.capacity;
			%brick.centerprintMenu.menuOption[1] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");

			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[1] = "togglePower";
		}
		else if (%db.isPoweredProcessor)
		{
			%brick.centerprintMenu.menuOption[0] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuFunction[0] = "togglePower";
		}
		return %ret;
	}

	function removeStack(%cl, %menu, %option)
	{
		%brick = %menu.brick;
		if (isObject(%brick) && %brick.getDatablock().isGenerator)
		{
			return;
		}
		return parent::removeStack(%cl, %menu, %option);
	}
};
activatePackage(PowerSystems);

function powerTick(%index)
{
	cancel($masterPowerTickSchedule);
	
	if ($restartCheck $= "")
	{
		$restartCheck = getSubStr(getRandomHash(), 0, 5);
	}

	if (!isObject(MissionCleanup)) 
	{
		return;
	}

	//if no Power bins just skip everything
	%count = PowerControlSimSet.getCount();
	if (%count <= 0)
	{
		$masterPowerTickSchedule = schedule(100, 0, PowerTick, %index);
		return;
	}

	for (%i = 0; %i < %count; %i++)
	{
		if (%index >= %count)
		{
			break;
		}
		%brick = PowerControlSimSet.getObject(%index);
		%powerDataID = getSubStr(%brick.getName(), 1, 50);

		if (%powerDataID $= "")
		{
			continue;
		}

		if (getDataIDArrayTagValue(%powerDataID, "lastValidated") + 5 < $Sim::Time 
			|| getDataIDArrayTagValue(%powerDataID, "restartCheck") !$= $restartCheck)
		{
			validateControlSystem(%powerDataID);
			setDataIDArrayTagValue(%powerDataID, "lastValidated", $Sim::Time);
			setDataIDArrayTagValue(%powerDataID, "restartCheck", $restartCheck);
		}

		if (%brick.nextPowerCheck < getSimTime())
		{
			powerCheck(%brick);
		}
		%index++;
	}

	if (%index >= %count)
	{
		%index = 0;
	}

	$masterPowerTickSchedule = schedule(100, 0, PowerTick, %index);
}

function powerCheck(%brick)
{
	%db = %brick.getDatablock();
	%dataID = %brick.eventOutputParameter0_1;
	%brick.nextPowerCheck = getSimTime() + (%db.tickTime * 1000 | 0) | 0;
	if (!%db.isPowerControlBox)
	{
		return;
	}
	if (%brick.debugPower)
	{
		talk("Calling powercheck on " @ %brick);
	}

	%inputOn = !getDataIDArrayTagValue(%dataID, "isInputOff");
	%outputOn = !getDataIDArrayTagValue(%dataID, "isOutputOff");
	%batteryChargeOnly = getDataIDArrayTagValue(%dataID, "batteryMode");

	%powerDataID = getSubStr(%brick.getName(), 1, 50);

	%totalGeneratedPower = 0;
	%genList = getDataIDArrayTagValue(%powerDataID, "generators");
	%genCount = getWordCount(%genList);
	for (%i = 0; %i < %genCount; %i++)
	{
		%gen = getWord(%genList, %i);
		if (!isObject(%gen))
		{
			continue;
		}

		%genDB = %gen.getDatablock();
		%genDataID = %gen.eventOutputParameter0_1;
		%on = getDataIDArrayTagValue(%genDataID, "isPoweredOn");

		if (%genDB.isGenerator && %on && %inputOn)
		{
			%genDB = %genDB;
			%powerGen = %genDB.generation;
			%burn = %genDB.burnRate;

			%fuelStorage = getDataIDArrayValue(%genDataID, 1);
			%fuelType = getField(%fuelStorage, 0);
			%count = getField(%fuelStorage, 1);

			if (%count > 0 && %gen.canAcceptFuel(%fuelType)) //has fuel, burn some and generate power
			{
				%totalGeneratedPower += %powerGen;
				if (%count - %burn > 0)
					%newCount = mFloatLength(%count - %burn, 2);
				else
					%newCount = 0;
				setDataIDArrayValue(%genDataID, 1, %fuelType TAB %newCount TAB getField(%fuelStorage, 2));

				if (!isObject(%gen.audioEmitter))
				{
					%gen.setMusic("BatteryLoopSound".getID());
				}
				%gen.updateStorageMenu(%genDataID);
			}
			else if (isObject(%gen.audioEmitter))
			{
				%gen.setMusic("");
			}
			%genOnCount++;
		}
		else if (isObject(%gen.audioEmitter))
		{
			%gen.setMusic("");
		}
	}

	%totalPowerUsage = 0;
	%devList = getDataIDArrayTagValue(%powerDataID, "devices");
	%devCount = getWordCount(%devList);
	for (%i = 0; %i < %devCount; %i++)
	{
		%dev = getWord(%devList, %i);
		if (!isObject(%dev))
		{
			continue;
		}

		%devDB = %dev.getDatablock();
		%devDataID = %dev.eventOutputParameter0_1;
		%on = getDataIDArrayTagValue(%devDataID, "isPoweredOn");

		if (%devDB.isPoweredProcessor && %on && %outputOn)
		{
			%dev_on[%dev_onCount++ - 1] = %dev;
			%proDB = %devDB;
			%powerDraw = %pro.getEnergyUse();

			%totalPowerUsage += %powerDraw;
			%proOnCount++;
		}
		else if (isFunction(%devDB.powerFunction)) //its off, so make sure it has 0 power
		{
			call(%devDB.powerFunction, %dev, 0);
		}
	}

	%powerDiff = %totalGeneratedPower - %totalPowerUsage;
	%totalBatteryPower = 0;
	%batteryDischarge = 0;
	%batList = getDataIDArrayTagValue(%powerDataID, "batteries");
	%batCount = getWordCount(%batList);
	for (%i = 0; %i < %batCount; %i++)
	{
		%bat = getWord(%batList, %i);
		if (!isObject(%bat))
		{
			continue;
		}

		%batDB = %bat.getDatablock();
		%batDataID = %bat.eventOutputParameter0_1;
		%on = getDataIDArrayTagValue(%batDataID, "isPoweredOn");
		
		%chargeAvailable = getDataIDArrayTagValue(%batDataID, "charge");
		%max = %batDB.capacity;

		if (%batDB.isBattery && %on)
		{
			%bat_on[%bat_onCount++ - 1] = %bat.getID();
			%batDB = %batDB;
			%discharge = getMin(%batDB.dischargeRate, %chargeAvailable);

			if (%powerDiff < 0 && %discharge > 0 && !%batteryChargeOnly) //need extra power
			{
				%dischargeAmt = getMin(%discharge, mAbs(%powerDiff));
				%powerDiff += %dischargeAmt;
				setDataIDArrayTagValue(%batDataID, "charge", %chargeAvailable - %dischargeAmt);

				%totalBatteryPower += %chargeAvailable - %dischargeAmt;
				%batteryDischarge += %dischargeAmt;
				
				%bat.updateStorageMenu(%batDataID);
			}
			else if (%powerDiff > 0 && %chargeAvailable < %max) //extra power available to charge battery with
			{
				%addAmt = getMin(%powerDiff, %max - %chargeAvailable);
				%powerDiff -= %addAmt;
				setDataIDArrayTagValue(%batDataID, "charge", %chargeAvailable + %addAmt);
				
				%totalBatteryPower += %chargeAvailable + %addAmt;
				%batteryDischarge -= %addAmt;
				
				%bat.updateStorageMenu(%batDataID);
			}
			else
			{
				%totalBatteryPower += %chargeAvailable;
			}
			%batOnCount++;
		}
		else if (%batDB.isBattery && %powerDiff > 0 && %chargeAvailable < %max) //extra power available to charge battery with
		{
			%addAmt = getMin(%powerDiff, %max - %chargeAvailable);
			%powerDiff -= %addAmt;
			setDataIDArrayTagValue(%batDataID, "charge", %chargeAvailable + %addAmt);
			
			%totalBatteryPower += %chargeAvailable + %addAmt;
			%batteryDischarge -= %addAmt;
			
			%bat.updateStorageMenu(%batDataID);
		}
	}

	%powerRatio = (%totalGeneratedPower + %batteryDischarge) / %totalPowerUsage;
	if (%totalPowerUsage <= 0)
	{
		%powerRatio = 0;
	}

	for (%i = 0; %i < %dev_onCount; %i++)
	{
		%proDB = %dev_on[%i].getDatablock();
		if (isFunction(%proDB.powerFunction))
		{
			call(%proDB.powerFunction, %dev_on[%i], %powerRatio);
		}
	}

	%brick.totalBatteryPower = %totalBatteryPower;
	%brick.totalPowerUsage = %totalPowerUsage;
	%brick.totalGeneratedPower = %totalGeneratedPower;

	%brick.updateStorageMenu();
}

function fxDTSBrick::autoAddPowerControlSystem(%brick)
{
	%db = %brick.getDatablock();
	if (%brick.getName() !$= "")
	{
		%powerDataID = getSubStr(%brick.getName(), 1, 50);
		if (%db.isPowerControlBox || %db.isPoweredProcessor || %db.isBattery || %db.isGenerator)
		{
			connectToControlSystem(%brick, %powerDataID);
		}
	}
}

function fxDTSBrick::getEnergyUse(%brick)
{
	return %brick.getDatablock().energyUse;
}

function togglePower(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%toggleOn = !getDataIDArrayTagValue(%dataID, "isPoweredOn");
	setDataIDArrayTagValue(%dataID, "isPoweredOn", %toggleOn);
	if (%toggleOn)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return %toggleOn;
}

function toggleInputOn(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%toggleOff = !getDataIDArrayTagValue(%dataID, "isInputOff"); //default is on
	setDataIDArrayTagValue(%dataID, "isInputOff", %toggleOff);
	if (%toggleOff)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return !%toggleOff;
}

function toggleOutputOn(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%toggleOff = !getDataIDArrayTagValue(%dataID, "isOutputOff"); //default is on
	setDataIDArrayTagValue(%dataID, "isOutputOff", %toggleOff);
	if (%toggleOff)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return !%toggleOff;
}

function toggleBatteryMode(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%batteryMode = !getDataIDArrayTagValue(%dataID, "batteryMode"); //default: full, alt: charge only
	setDataIDArrayTagValue(%dataID, "batteryMode", %batteryMode);
	if (%batteryMode)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return %batteryMode;
}

function addFuel(%brick, %cl, %slot)
{
	if (getTrustLevel(%brick, %cl) < 1)
	{
		serverCmdUnuseTool(%cl);
		%cl.centerprint(getBrickgroupFromObject(%brick).name @ "<color:ff0000> does not trust you enough to do that!", 1);
		return;
	}

	%pl = %cl.player;
	%item = %pl.tool[%slot];
	if (%item.isStackable && %item.stackType !$= "")
	{
		if (!%brick.canAcceptFuel(%item.stackType))
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("This generator only accepts " @ strReplace(%brick.getDatablock().fuelType, " ", ", ") @ "!", 1);
			return;
		}
		
		%brick.isAcceptingFuel = 1;
		%success = %brick.insertIntoStorage(%brick.eventOutputParameter[0, 1], 
										%item, 
										!%pl.tool[%slot].isStackable ? 1 : %pl.toolStackCount[%slot], 
										%pl.toolDataID[%slot]);
		%brick.isAcceptingFuel = 0;
		if (%success == 0) //complete insertion
		{
			%pl.toolStackCount[%slot] = 0;
			%pl.tool[%slot] = 0;
			messageClient(%cl, 'MsgItemPickup', "", %slot, 0);
			if (%pl.currTool == %slot)
			{
				%pl.unmountImage(0);
			}
			return;
		}
		else if (%success == 1) //partial insertion
		{
			%pl.toolStackCount[%slot] = getWord(%success, 1);
			%db = getStackTypeDatablock(%pl.tool[%slot].stackType, getWord(%success, 1)).getID();
			messageClient(%cl, 'MsgItemPickup', "", %slot, %db);
			%pl.tool[%slot] = %db;
			if (%pl.currTool == %slot)
			{
				%pl.mountImage(%db.image, 0);
			}
			return;
		}
	}
	else
	{
		%cl.centerprint("You cannot use this as fuel!", 1);
		return;
	}
}

function fxDTSBrick::isPoweredOn(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	return getDataIDArrayTagValue(%dataID, "isPoweredOn");
}

function fxDTSBrick::isInputOn(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	return !getDataIDArrayTagValue(%dataID, "isInputOff");
}

function fxDTSBrick::isOutputOn(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	return !getDataIDArrayTagValue(%dataID, "isOutputOff");
}

function fxDTSBrick::getBatteryMode(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	if (getDataIDArrayTagValue(%dataID, "batteryMode"))
	{
		return "Charging only";
	}
	else
	{
		return "Discharging on";
	}
}

function fxDTSBrick::canAcceptFuel(%brick, %stackType)
{
	return strPos(strLwr(%brick.getDatablock().fuelType), strLwr(%stackType)) >= 0;
}

function connectToControlSystem(%brick, %dataID)
{
	if (%dataID $= "" || !isObject(%brick))
	{
		return -1;
	}

	%box = getDataIDArrayTagValue(%dataID, "powerControlBox");
	if (isObject(%box))
	{
		%boxAvailable = 1;
		%boxDB = %box.getDatablock();
		%genMax = %boxDB.maxGenerators;
		%batMax = %boxDB.maxBatteries;
		%devMax = %boxDB.maxProcessors;
	}
	%brickDB = %brick.getDatablock();
	if (%brickDB.isPoweredProcessor)
	{
		%deviceList = getDataIDArrayTagValue(%dataID, "devices");
		if (strPos(" " @ %deviceList @ " ", " " @ %brick @ " ") >= 0)
		{
			return 2; //already in list
		}
		else if (%boxAvailable && getWordCount(%deviceList) >= %devMax)
		{
			return 3; //maxed out connection type
		}
		setDataIDArrayTagValue(%dataID, "devices", trim(%deviceList SPC %brick));
	}
	else if (%brickDB.isBattery)
	{
		%batteryList = getDataIDArrayTagValue(%dataID, "batteries");
		if (strPos(" " @ %batteryList @ " ", " " @ %brick @ " ") >= 0)
		{
			return 2;
		}
		else if (%boxAvailable && getWordCount(%batteryList) >= %batMax)
		{
			return 3;
		}
		setDataIDArrayTagValue(%dataID, "batteries", trim(%batteryList SPC %brick));	
	}
	else if (%brickDB.isGenerator)
	{
		%generatorList = getDataIDArrayTagValue(%dataID, "generators");
		if (strPos(" " @ %generatorList @ " ", " " @ %brick @ " ") >= 0)
		{
			return 2;
		}
		else if (%boxAvailable && getWordCount(%generatorList) >= %genMax)
		{
			return 3;
		}
		setDataIDArrayTagValue(%dataID, "generators", trim(%generatorList SPC %brick));	
	}
	else if (%brickDB.isPowerControlBox)
	{
		if (isObject(%box))
		{
			echo("Power system already has control box! current:" @ %box @ " new: " @ %brick);
			return 1;
		}
		setDataIDArrayTagValue(%dataID, "powerControlBox", %brick);
	}
	else
	{
		return -1; //not a power system brick
	}

	%brick.setName("_" @ %dataID);
	return 0;
}

function disconnectFromControlSystem(%brick, %dataID, %skipUnsetName)
{
	if (%dataID $= "" || !isObject(%brick))
	{
		return -1;
	}

	%brickDB = %brick.getDatablock();
	if (%brickDB.isPoweredProcessor)
	{
		%deviceList = getDataIDArrayTagValue(%dataID, "devices");
		if (strPos(" " @ %deviceList @ " ", " " @ %brick @ " ") < 0)
		{
			return 2; //not in list
		}
		%deviceList = strReplace(" " @ %deviceList @ " ", " " @ %brick @ " ", " ");
		setDataIDArrayTagValue(%dataID, "devices", trim(%deviceList));
	}
	else if (%brickDB.isBattery)
	{
		%batteryList = getDataIDArrayTagValue(%dataID, "batteries");
		if (strPos(" " @ %batteryList @ " ", " " @ %brick @ " ") < 0)
		{
			return 2;
		}
		%batteryList = strReplace(" " @ %batteryList @ " ", " " @ %brick @ " ", " ");
		setDataIDArrayTagValue(%dataID, "batteries", trim(%batteryList));	
	}
	else if (%brickDB.isGenerator)
	{
		%generatorList = getDataIDArrayTagValue(%dataID, "generators");
		if (strPos(" " @ %generatorList @ " ", " " @ %brick @ " ") < 0)
		{
			return 2;
		}
		%generatorList = strReplace(" " @ %generatorList @ " ", " " @ %brick @ " ", " ");
		setDataIDArrayTagValue(%dataID, "generators", trim(%generatorList));	
	}
	else if (%brickDB.isPowerControlBox)
	{
		%box = getDataIDArrayTagValue(%dataID, "powerControlBox");
		if (!isObject(%box) || %box != %brick)
		{
			echo("Wrong box passed in/system has no box! current:" @ %box @ " new: " @ %brick);
			return 1;
		}
		setDataIDArrayTagValue(%dataID, "powerControlBox", "");
	}
	else
	{
		return -1; //not a power system brick
	}

	if (!%skipUnsetName)
	{
		%brick.setName("");
	}
	return 0;
}

function validateControlSystem(%powerDataID)
{
	//dont validate anything if no control box
	%box = getDataIDArrayTagValue(%powerDataID, "powerControlBox");
	if (!isObject(%box) || !%box.getDatablock().isPowerControlBox || %box.getName() !$= ("_" @ %powerDataID)) 
	{
		if (isObject(%box))
		{
			setDataIDArrayTagValue(%powerDataID, "powerControlBox", 0);
			%box.setName("");
		}
		return 1;
	}

	//validate all lists
	%objMax0 = %box.getDatablock().maxGenerators;
	%objMax1 = %box.getDatablock().maxBatteries;
	%objMax2 = %box.getDatablock().maxProcessors;

	%objType0 = "Generator";
	%objType1 = "Battery";
	%objType2 = "PoweredProcessor";

	%objList0 = getDataIDArrayTagValue(%powerDataID, "generators");
	%objList1 = getDataIDArrayTagValue(%powerDataID, "batteries");
	%objList2 = getDataIDArrayTagValue(%powerDataID, "devices");
	%brickName = "_" @ %powerDataID;

	for (%idx = 0; %idx < 3; %idx++)
	{
		%list = %objList[%idx];
		%type = %objType[%idx];
		%max = %objMax[%idx];
		%count = getWordCount(%list);

		%objCount = 0;
		for (%i = 0; %i < %count; %i++)
		{
			%obj = getWord(%list, %i);
			if (!isObject(%obj)) 
			{
				%change = 1;
				continue;
			}

			if (!%obj.getDatablock().is[%type] || %obj.getName() !$= %brickName || %objCount >= %max)
			{
				%change = 1;
				%remove = %remove SPC %obj;
				continue;
			}

			%finalObj[%idx] = %finalObj[%idx] SPC %obj;
			%objCount++;
		}
	}

	if (!%change)
	{
		return 0; //no changes made, dont need to set values
	}

	for (%i = 0; %i < getWordCount(%remove); %i++)
	{
		%obj = getWord(%remove, %i);
		if (isObject(%obj))
		{
			%obj.setName("");
		}
	}

	setDataIDArrayTagValue(%powerDataID, "generators", trim(%finalObj0));
	setDataIDArrayTagValue(%powerDataID, "batteries", trim(%finalObj1));
	setDataIDArrayTagValue(%powerDataID, "devices", trim(%finalObj2));
	return 0;
}

function connectToControlBox(%brick, %controlBox)
{
	if (!isObject(%brick) || !isObject(%controlBox))
	{
		return -1;
	}

	%brickDB = %brick.getDatablock();
	%controlDB = %controlBox.getDatablock();
	%brickName = %brick.getName();
	
	//ensure devices have associated dataID for setting storage
	%brickDataID = %brick.eventOutputParameter0_1;
	%controlDataID = %controlBox.eventOutputParameter0_1;
	if (%brickDataID $= "" || %controlDataID $= "")
	{
		error("ERROR: connectToControlBox - data ID is empty! (" @ %brickDataID @ ", " @ %controlDataID @ ")");
		talk("ERROR: connectToControlBox - data ID is empty! (" @ %brickDataID @ ", " @ %controlDataID @ ")");
		return -1;
	}
	else if (%brickDB.isPowerControlBox || !%controlDB.isPowerControlBox)
	{
		error("ERROR: connectToControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		talk("ERROR: connectToControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		return -1;	
	}

	//retrieve/set control box power system dataID
	if (%controlBox.getName() $= "")
	{
		%powerDataID = getSubStr(getRandomHash(), 0, 20) @ "Power";
		connectToControlSystem(%controlBox, %powerDataID);
		%boxName = "_" @ %powerDataID;
	}
	else
	{
		%powerDataID = getSubStr(%controlBox.getName(), 1, 50);
		%boxName = "_" @ %powerDataID;
	}

	//do not connect if device is already in a power system
	if (%brickName !$= "" && isObject(getDataIDArrayTagValue(getSubStr(%brickName, 1, 50), "powerControlBox")))
	{
		if (%brickName $= %boxName)
		{
			error("ERROR: connectToControlBox - " @ %brick @ " already connected to this system!");
			return 2;
		}
		else
		{
			error("ERROR: connectToControlBox - " @ %brick @ " already is connected to a control box!");
			return 1;
		}
	}

	//distance check
	if (vectorDist(%brick.getPosition(), %controlBox.getPosition()) > 32)
	{
		error("ERROR: connectToControlBox - Cannot connect \"" @ %brick @ "\" to control box, too far");
		return 4;
	}

	//all basic checks passed, ATTEMPT TO connect them (returns proper error code if invalid)
	return connectToControlSystem(%brick, %powerDataID);
}

function disconnectFromControlBox(%brick, %controlBox)
{
	if (!isObject(%brick) || !isObject(%controlBox))
	{
		return -1;
	}

	%brickDB = %brick.getDatablock();
	%controlDB = %controlBox.getDatablock();
	if (%brickDB.isPowerControlBox || !%controlDB.isPowerControlBox)
	{
		error("ERROR: disconnectFromControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		talk("ERROR: disconnectFromControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		return -1;	
	}

	if (%brick.getName() $= "" || %controlBox.getName() $= "")
	{
		error("ERROR: disconnectFromControlBox - one or both bricks not connected to a power system! " @ %brick SPC %controlBox);
		return 1;
	}
	else if (%brick.getName() !$= %controlBox.getName())
	{
		error("ERROR: disconnectFromControlBox - devices not on same network!");
		return 1;
	}

	return disconnectFromControlSystem(%brick, getSubStr(%controlBox.getName(), 1, 50));
}

function drawControlNetwork(%PowerDataID, %simSet, %focusObj)
{
	if (!isObject(%simSet))
	{
		%simSet = new SimSet(PowerNetworkShapes);
	}

	for (%i = 0; %i < %simSet.getCount(); %i++)
	{
		%line[%i] = %simSet.getObject(%i);
	}
	%totalLines = %simSet.getCount();
	%currLine = 0;

	//determine focus object
	if (isObject(%focusObj))
	{
		%focusObjDB = %focusObj.getDatablock();
		%offset = "0 0 " @ %focusObjDB.brickSizeZ * 0.1;
		if (%focusObjDB.isPowerControlBox)
		{
			%focusBox = %focusObj;
			%focusBoxPos = vectorAdd(%focusObj.getPosition(), %offset);
		}
		else if (%focusObjDB.isPoweredProcessor || %focusObjDB.isBattery || %focusObjDB.isGenerator)
		{
			%focusPower = %focusObj;
			%focusPowerPos = vectorAdd(%focusObj.getPosition(), %offset);
		}
	}

	%controlBox = getDataIDArrayTagValue(%PowerDataID, "powerControlBox");
	%controlBoxPos = vectorAdd("0 0 " @ (%controlBox.getDatablock().brickSizeZ * 0.1), %controlBox.getPosition());

	if (isObject(%focusPower) && isObject(%controlBox))
	{
		if (!isObject(%line[%currLine]))
		{
			%line[%currLine] = drawLine(%controlBoxPos, %focusPowerPos, "1 1 1 1", 0.05);
		}
		else
		{
			%line[%currLine].drawLine(%controlBoxPos, %focusPowerPos, "1 1 1 1", 0.05);
		}
		%currLine++;
	}
	else if (isObject(%focusBox) && %focusBox == %controlBox)
	{
		%objType0 = "Generator";
		%objType1 = "Battery";
		%objType2 = "PoweredProcessor";

		%objColor0 = "0 1 0";
		%objColor1 = "1 1 0";
		%objColor2 = "1 0 0";

		%objList0 = getDataIDArrayTagValue(%powerDataID, "generators");
		%objList1 = getDataIDArrayTagValue(%powerDataID, "batteries");
		%objList2 = getDataIDArrayTagValue(%powerDataID, "devices");
		%brickName = "_" @ %powerDataID;

		for (%idx = 0; %idx < 3; %idx++)
		{
			%list = %objList[%idx];
			%type = %objType[%idx];
			%color = %objColor[%idx];
			%count = getWordCount(%list);
			for (%i = 0; %i < %count; %i++)
			{
				%obj = getWord(%list, %i);
				if (!isObject(%obj)) 
				{
					continue;
				}

				if (!%obj.getDatablock().is[%type] || %obj.getName() !$= %brickName)
				{
					continue;
				}
				%pos = vectorAdd("0 0 " @ (%obj.getDatablock().brickSizeZ * 0.1), %obj.getPosition());

				%factor = ((%count - %i) / 2 + %count / 2) / %count;

				if (!isObject(%line[%currLine]))
				{
					%line[%currLine] = drawLine(%focusBoxPos, %pos, vectorScale(%objColor[%idx], %factor) SPC 1, 0.05);
				}
				else
				{
					%line[%currLine].drawLine(%focusBoxPos, %pos, vectorScale(%objColor[%idx], %factor) SPC 1, 0.05);
				}
				%currLine++;
			}
		}
	}

	//add lines to simset
	if ($powerNetworkDebug)
	{
		talk("totalLines: " @ %totalLines);
		talk("currLine: " @ %currLine);
		%q = 0;
		while (%line[%q] !$= "")
		{
			talk(%q @ ": " @ %line[%q]);
			%q++;
		}
	}
	%simSet.clear();
	for (%i = 0; %i < %currLine; %i++)
	{
		if ($powerNetworkDebug) talk("Adding " @ %line[%i]);
		%simSet.add(%line[%i]);
	}
	for (%i = %currLine; %i < %totalLines; %i++)
	{
		if ($powerNetworkDebug) talk("Deleting " @ %line[%i]);
		%line[%i].delete();
	}

	return %simSet;
}





///////////////
//Linker Item//
///////////////

datablock ItemData(ElectricalCableItem : HammerItem)
{
	shapeFile = "./resources/power/electricalcable.dts";
	uiName = "Electrical Cable";

	image = "ElectricalCableImage";

	doColorshift = false;
	colorShiftColor = "1 1 1 1";
};

datablock ShapeBaseImageData(ElectricalCableImage)
{
	shapeFile = "./resources/power/electricalcable.dts";
	emap = true;

	offset = "-0.05 0.3 -0.06";
	rotation = eulerToMatrix("0 90 0");
	// eyeOffset = "0 0 0";

	item = "ElectricalCableItem";
	armReady = 1;

	doColorshift = true;
	colorShiftColor = "1 1 1 1";

	toolTip = "Connects electrical devices";
	mountPoint = 0;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "ReadyLoop";
	stateTimeoutValue[1] = 0.1;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "PostFire";

	stateName[3] = "PostFire";
	stateTransitionOnTriggerUp[3] = "Ready";

	stateName[4] = "ReadyLoop";
	stateTransitionOnTimeout[4] = "Ready";
	stateTimeoutValue[4] = 0.1;
	stateScript[4] = "onReady";
	stateTransitionOnTriggerDown[4] = "Fire";
};

function ElectricalCableImage::onMount(%this, %obj, %slot)
{
	%obj.powerControlBrick = "";
	%obj.poweredBrick = "";
}

function ElectricalCableImage::onUnmount(%this, %obj, %slot)
{
	%obj.powerControlBrick = "";
	%obj.poweredBrick = "";
	if (isObject(%obj.displaySet))
	{
		%obj.displaySet.deleteAll();
		%obj.displaySet.delete();
	}
}

function ElectricalCableImage::onReady(%this, %obj, %slot)
{
	//detect looked-at brick and show connection information
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%masks = $Typemasks::fxBrickObjectType;
	%ray = containerRaycast(%start, %end, %masks);
	if (isObject(%hit = getWord(%ray, 0)))
	{
		%db = %hit.getDatablock();
		if (%db.isGenerator || %db.isPoweredProcessor || %db.isBattery || %db.isPowerControlBox)
		{
			if (%db.isGenerator) %type = "Generator";
			else if (%db.isPoweredProcessor) %type = "Device";
			else if (%db.isBattery) %type = "Battery";
			else %type = "Power Control Box";

			%obj.brickInfo = %type;
			%dataID = getSubStr(%hit.getName(), 1, 50);
		
			%obj.displaySet = drawControlNetwork(%dataID, %obj.displaySet, %hit);
		}
		else //invalid brick being looked at
		{
			if (isObject(%obj.displaySet))
			{
				%obj.displaySet.deleteAll();
			}
			%obj.brickInfo = "";
		}
	}
	else //no brick being looked at
	{
		if (isObject(%obj.displaySet))
		{
			%obj.displaySet.deleteAll();
		}
		%obj.brickInfo = "";
	}

	//display connection lines
	if (isObject(%obj.poweredBrick))
	{
		%pos1 = %obj.poweredBrick.getPosition();
		%pos2 = %obj.getMuzzlePoint(%slot);
		if (!isObject(%obj.poweredBrickLine))
		{
			%obj.poweredBrickLine = drawLine(%pos1, %pos2, "0 0 1 1", 0.05);
		}
		else
		{
			%obj.poweredBrickLine.drawLine(%pos1, %pos2, "0 0 1 1", 0.05);
		}
		cancel(%obj.poweredBrickLine.deleteSched);
		%obj.poweredBrickLine.deleteSched = %obj.poweredBrickLine.schedule(200, delete);
	}

	if (isObject(%obj.powerControlBrick))
	{
		%pos1 = %obj.powerControlBrick.getPosition();
		%pos2 = %obj.getMuzzlePoint(%slot);
		if (!isObject(%obj.powerControlBrickLine))
		{
			%obj.powerControlBrickLine = drawLine(%pos1, %pos2, "1 0 1 1", 0.05);
		}
		else
		{
			%obj.powerControlBrickLine.drawLine(%pos1, %pos2, "1 0 1 1", 0.05);
		}
		cancel(%obj.powerControlBrickLine.deleteSched);
		%obj.powerControlBrickLine.deleteSched = %obj.powerControlBrickLine.schedule(200, delete);
	}

	//display centerprint
	if (isObject(%cl = %obj.client))
	{
		if (%obj.errorTime < $Sim::Time)
		{
			%obj.errorString = "";
		}
		if (%obj.responseTime < $Sim::Time)
		{
			%obj.responseString = "";
		}

		%cpstr = "<just:right>\c3-Electrical Cable- <br>";
		%currDevice = (isObject(%obj.poweredBrick) ? %obj.poweredBrick.getDatablock().uiName : "\c0None");
		%currPowerBrick = (isObject(%obj.powerControlBrick) ? %obj.powerControlBrick.getPosition() : "\c0None");
		%cpstr = %cpstr @ "\c6Current Device: \c3" @ %currDevice @ " <br>";
		%cpstr = %cpstr @ "\c6Current Box: \c3" @ %currPowerBrick @ " <br>";
		%cpstr = %cpstr @ "\c4" @ %obj.brickInfo @ " <br>";
		%cpstr = %cpstr @ %obj.responseString @ " <br>";
		%cpstr = %cpstr @ %obj.errorString;

		%cl.centerprint(%cpstr, 2);
	}
}

function ElectricalCableImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%masks = $Typemasks::fxBrickObjectType;
	%ray = containerRaycast(%start, %end, %masks);
	if (isObject(%hit = getWord(%ray, 0)))
	{
		if (%hit == %obj.poweredBrick || %hit == %obj.powerControlBrick)
		{
			%obj.poweredBrick = (%hit == %obj.poweredBrick ? "" : %obj.poweredBrick);
			%obj.powerControlBrick = (%hit == %obj.powerControlBrick ? "" : %obj.powerControlBrick);

			%obj.responseString = "\c2Deselected!";
			%obj.responseTime = $Sim::Time + 3;
			ElectricalCableImage::onReady(%this, %obj, %slot);
			return;
		}

		%db = %hit.getDatablock();
		if (!%db.isGenerator && !%db.isPoweredProcessor && !%db.isBattery && !%db.isPowerControlBox)
		{
			%obj.errorString = "\c0Invalid object!";
			%obj.errorTime = $Sim::Time + 3;

			ElectricalCableImage::onReady(%this, %obj, %slot);
			return;
		}
		else if (getTrustLevel(%hit, %obj) < 2)
		{
			%obj.errorString = "\c0You need full trust to select this brick!";
			%obj.errorTime = $Sim::Time + 3;

			ElectricalCableImage::onReady(%this, %obj, %slot);
			return;
		}

		if (%db.isGenerator || %db.isPoweredProcessor || %db.isBattery)
		{
			%obj.poweredBrick = %hit;
			%lastAdded = "poweredBrick";
			%obj.responseString = "\c2Selected " @ %hit.getDatablock().uiname @ "!";
			%obj.responseTime = $Sim::Time + 3;
		}
		else
		{
			%obj.powerControlBrick = %hit;
			%lastAdded = "powerControlBrick";
			%obj.responseString = "\c2Selected " @ %hit.getDatablock().uiname @ "!";
			%obj.responseTime = $Sim::Time + 3;
		}


		if (%obj.poweredBrick.getDatablock().isGenerator) %type = "generator";
		else if (%obj.poweredBrick.getDatablock().isPoweredProcessor) %type = "device";
		else if (%obj.poweredBrick.getDatablock().isBattery) %type = "battery";

		if (isObject(%obj.powerControlBrick) && isObject(%obj.poweredBrick))
		{
			%error = connectToControlBox(%obj.poweredBrick, %obj.powerControlBrick);

			if (%error != 0)
			{
				switch (%error)
				{
					case 2: %errorString = "\c0Object is already connected to this control brick!"; //replace with disconnect
							%error = disconnectFromControlBox(%obj.poweredBrick, %obj.powerControlBrick);
							%obj.poweredBrick = "";
							if (%error)
							{
								talk("Disconnect critical error! " @ %error);
								return;
							}
							%obj.responseString = "Disconnected!";
							%obj.responseTime = $Sim::Time + 3;
							return;
					case 1: %errorString = "\c0Object is connected to a different control brick!";
					case 3: %errorString = "\c0Power control box has no more free " @ %type @ " connections!";
					default: %errorString = "\c0Critical error! Please report to an admin!";
				}
				%obj.errorString = %errorString;
				%obj.errorTime = $Sim::Time + 2;
				
				%obj.poweredBrick = "";
				if (%lastAdded $= "poweredBrick")
				{
					%obj.responseString = "";
				}
			}
			else
			{
				%obj.responseString = "\c2Linked " @ %obj.poweredBrick.getDatablock().uiName @ " to Power Control Box!";
				%obj.responseTime = $Sim::Time + 5;
				%obj.poweredBrick = "";
			}
			ElectricalCableImage::onReady(%this, %obj, %slot);
		}
	}
}




//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickCoalGeneratorData)
{
	uiName = "Coal Generator";

	brickFile = "./resources/power/CoalGenerator.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	processorFunction = "addFuel";
	// activateFunction = "CoalGeneratorInfo";
	placerItem = "CoalGeneratorItem";
	keepActivate = 1;
	isGenerator = 1;
	burnRate = 0.01;
	generation = 10;
	fuelType = "Coal";

	isStorageBrick = 1;
	storageSlotCount = 1;
	itemStackCount = 0;
	storageMultiplier = 12;
};

datablock fxDTSBrickData(brickPowerControlBoxData)
{
	uiName = "Power Control Box";

	brickFile = "./resources/power/controlBoxClosed.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "CoalGeneratorInfo";
	placerItem = "PowerControlBoxItem";
	keepActivate = 1;
	isPowerControlBox = 1;
	maxGenerators = 4;
	maxProcessors = 16;
	maxBatteries = 4;
	tickTime = 0.8;

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
	storageOpenDatablock = "brickPowerControlBoxOpenData";
	storageClosedDatablock = "brickPowerControlBoxData";
};

datablock fxDTSBrickData(brickPowerControlBoxOpenData : brickPowerControlBoxData)
{
	brickFile = "./resources/power/controlBoxOpen.blb";
};

datablock fxDTSBrickData(brickBatteryData)
{
	uiName = "Battery";

	brickFile = "./resources/power/battery.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "CoalGeneratorInfo";
	placerItem = "BatteryItem";
	keepActivate = 1;
	isBattery = 1;
	dischargeRate = 40;
	capacity = 10000;

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
};



///////////////
//Placer Item//
///////////////

datablock ItemData(CoalGeneratorItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Coal Generator";
	image = "CoalGeneratorBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
};

datablock ShapeBaseImageData(CoalGeneratorBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = CoalGeneratorItem;
	
	doColorshift = true;
	colorShiftColor = CoalGeneratorItem.colorShiftColor;

	toolTip = "Places a Coal Generator";
	loopTip = "Converts fuel into power";
	placeBrick = "brickCoalGeneratorData";
};

function CoalGeneratorBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function CoalGeneratorBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function CoalGeneratorBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function CoalGeneratorBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(PowerControlBoxItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Power Control Box";
	image = "PowerControlBoxBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
};

datablock ShapeBaseImageData(PowerControlBoxBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = PowerControlBoxItem;
	
	doColorshift = true;
	colorShiftColor = PowerControlBoxItem.colorShiftColor;

	toolTip = "Places a Power Control Box";
	loopTip = "Connects electrical machines";
	placeBrick = "brickPowerControlBoxData";
};

function PowerControlBoxBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function PowerControlBoxBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function PowerControlBoxBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function PowerControlBoxBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(BatteryItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Battery";
	image = "BatteryBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
};

datablock ShapeBaseImageData(BatteryBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = BatteryItem;
	
	doColorshift = true;
	colorShiftColor = BatteryItem.colorShiftColor;

	toolTip = "Places a Battery";
	loopTip = "Stores excess electrical power";
	placeBrick = "brickBatteryData";
};

function BatteryBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function BatteryBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function BatteryBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function BatteryBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}