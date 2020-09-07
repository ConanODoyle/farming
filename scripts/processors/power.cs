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

		if (%brick.isPoweredOn() && %brick.nextPowerCheck < $Sim::Time)
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

	%genTotal = 0;
	//TODO: Iterate over generators, processors, batteries to get diagnostic info to display
}

function togglePower(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	setDataIDArrayTagValue(%dataID, "isPoweredOn", !getDataIDArrayTagValue(%dataID, "isPoweredOn"));
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);
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