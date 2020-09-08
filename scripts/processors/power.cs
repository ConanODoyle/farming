//dataID tags:
//	powerType - determines if the brick is a generator, processor, or battery
//	isPoweredOn - determines if the brick is running
//	brickName - the name of the brick associated with this processor
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
//	maximumGenerators - maximum number of attached generators
//	maximumProcessors - maximum number of attached processors

if (!isObject(PowerControlSimSet)) 
{
	$PowerControlSimSet = new SimSet(PowerControlSimSet) {};
}

package PowerControlSimSetCollector
{
	function fxDTSBrick::onAdd(%brick) 
	{
		%ret = parent::onAdd(%brick);

		if (%brick.isPlanted && %brick.getDatablock().isPowerControlBox)
		{
			PowerControlSimSet.add(%brick);
		}
		return %ret;
	}
};
activatePackage(PowerControlSimSetCollector);

package GeneratorPower
{
	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID)
	{
		if (%storageObj.getDatablock().isGenerator && !%storageObj.isAcceptingFuel)
		{
			return 2;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);
	}

	function fxDTSBrick::accessStorage(%brick, %dataID, %cl)
	{
		return parent::accessStorage(%brick, %dataID, %cl);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID);
		if (%brick.getDatablock().isGenerator)
		{
			%brick.centerprintMenu.menuOptionCount = 2;
			%brick.centerprintMenu.menuOption[1] = "Power: " @ %brick.isPoweredOn() ? "\c6On" : "\c0Off";
			%brick.centerprintMenu.menuFunction[1] = "togglePower";
		}
		else if (%brick.getDatablock().isPowerControlBox)
		{
			%brick.centerprintMenu.menuOptionCount = 3;
			%brick.centerprintMenu.menuOption[0] = "Generating: " @ %brick.totalGeneratedPower + 0;
			%brick.centerprintMenu.menuOption[1] = "Using: " @ %brick.totalPowerUsage + 0;
			%brick.centerprintMenu.menuOption[2] = "Total Battery: " @ %brick.totalBatteryPower + 0; 

			%brick.centerprintMenu.menuFunction[0] = "";
			%brick.centerprintMenu.menuFunction[1] = "";
			%brick.centerprintMenu.menuFunction[2] = "";
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
activatePackage(GeneratorPower);

function powerTick(%index)
{
	cancel($masterPowerTickSchedule);
	
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

		if (%brick.nextPowerCheck < $Sim::Time)
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
	%db = %brick;
	%dataID = %brick.eventOutputParameter0_1;
	if (!%db.isPowerControlBox)
	{
		return;
	}

	for (%i = 0; %i < getDataIDArrayCount(%dataID))
	{
		%subDataID = getDataIDArrayValue(%dataID, %i);
		%type = strLwr(getDataIDArrayTagValue(%subDataID, "powerType"));
		switch$ (%type)
		{
			case "generator":
				%gen[%genCount++ - 1] = %subDataID;
			case "processor":
				%pro[%proCount++ - 1] = %subDataID;
			case "battery":
				%bat[%batCount++ - 1] = %subDataID;
		}
	}

	//TODO: Iterate over processors, batteries to tick power and get diagnostic info to display
	%totalGeneratedPower = 0;
	for (%i = 0; %i < %genCount; %i++)
	{
		%on = getDataIDArrayTagValue(%gen[%i], "isPoweredOn");
		%brickName = getDataIDArrayTagValue(%gen[%i], "brickName");

		if (isObject(%brickName) && %brickName.getDatablock().isGenerator && %on)
		{
			%genDB = %brickName.getDatablock();
			%powerGen = %genDB.generation;
			%burn = %genDB.burnRate;

			%fuelStorage = getDataIDArrayValue(%gen[%i], 1);
			%fuelType = getField(%fuelStorage, 0);
			%count = getField(%fuelStorage, 1);

			if (%count > 0 && %fuelType !$= %genDB.fuelType) //has fuel, burn some and generate power
			{
				%totalGeneratedPower += %powerGen;
				%count = %count - %burn < 0 ? 0 : mFloatLength(%count - %burn, 2);
				setDataIDArrayValue(%gen[%i], 1, %fuelType TAB %count TAB getField(%fuelStorage, 2));

				if (!isObject(%brickName.audioEmitter))
				{
					%brickName.setMusic("BatteryLoopSound");
				}
			}
			else if (isObject(%brickName.audioEmitter))
			{
				%brickName.setMusic("None");
			}
		}
		else if (isObject(%brickName.audioEmitter))
		{
			%brickName.setMusic("None");
		}
	}

	%totalPowerUsage = 0;
	for (%i = 0; %i < %proCount; %i++)
	{
		%on = getDataIDArrayTagValue(%pro[%i], "isPoweredOn");
		%brickName = getDataIDArrayTagValue(%pro[%i], "brickName");

		if (isObject(%brickName) && %brickName.getDatablock().isPoweredProcessor && %on)
		{
			%pro_on[%pro_onCount++ - 1] = %brickName.getID();
			%proDB = %brickName.getDatablock();
			%powerDraw = %proDB.energyUse;

			%totalPowerUsage += %powerDraw;
		}
	}

	%powerDiff = %totalGeneratedPower - %totalPowerUsage;
	%totalBatteryPower = 0;
	for (%i = 0; %i < %batCount; %i++)
	{
		%on = getDataIDArrayTagValue(%bat[%i], "isPoweredOn");
		%brickName = getDataIDArrayTagValue(%bat[%i], "brickName");

		if (isObject(%brickName) && %brickName.getDatablock().isBattery && %on)
		{
			%bat_on[%bat_onCount++ - 1] = %brickName.getID();
			%chargeAvailable = getDataIDArrayTagValue(%bat[%i], "charge");
			%batDB = %brickName.getDatablock();
			%discharge = getMin(%batDB.dischargeRate, %chargeAvailable[%bat_onCount - 1]);
			%max = %batDB.capacity;

			if (%powerDiff < 0 && %discharge > 0) //need extra power
			{
				%dischargeAmt = getMin(%discharge, mAbs(%powerDiff));
				%powerDiff += %dischargeAmt;
				setDataIDArrayTagValue(%bat[%i], "charge", %chargeAvailable - %dischargeAmt);
				%totalBatteryPower += %chargeAvailable - %dischargeAmt;
			}
			else if (%powerDiff > 0 && %chargeAvailable < %max) //extra power available to charge battery with
			{
				%addAmt = getMin(%powerDiff, %max - %chargeAvailable);
				%powerDiff -= %addAmt;
				setDataIDArrayValue(%bat[%i], "charge", %chargeAvailable + %addAmt);
				%totalBatteryPower += %chargeAvailable + %addAmt;
			}
			else
			{
				%totalBatteryPower += %chargeAvailable;
			}
		}
	}

	%powerRatio = (%totalGeneratedPower + %powerDiff) / %totalGeneratedPower;
	for (%i = 0; %i < %pro_onCount; %i++)
	{
		%proDB = %pro_on[%i].getDatablock();
		if (isFunction(%proDB.powerFunction))
		{
			call(%proDB.powerFunction, %pro_on[%i], %powerRatio);
		}
	}

	%brick.totalBatteryPower = %totalBatteryPower;
	%brick.totalPowerUsage = %totalPowerUsage;
	%brick.totalGeneratedPower = %totalGeneratedPower;

	%brick.updateStorageMenu();
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
	return (%toggleOn);
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
		if (%brick.canAcceptFuel(%item.stackType))
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

function fxDTSBrick::canAcceptFuel(%brick, %stackType)
{
	return strPos(strLwr(%brick.getDatablock().fuelType), strLwr(%item.stackType)) < 0;
}



//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickCoalGeneratorData)
{
	uiName = "CoalGenerator";

	brickFile = "./resources/CoalGenerator/CoalGenerator.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "CoalGeneratorInfo";
	placerItem = "CoalGeneratorItem";
	keepActivate = 1;
	isGenerator = 1;
	fuelType = "Coal";

	isStorageBrick = 1;
	storageSlotCount = 1;
	itemStackCount = 0;
	storageMultiplier = 12;

	tickTime = 10;
	tickAmt = 1;
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
	
	cost = 800;
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