///////////
//Cannery//
///////////

datablock fxDTSBrickData(brickCanneryData : brick1x1Data) 
{
	category = "";
	subCategory = "";
	uiName = "Cannery";

	brickFile = "./resources/cannery/cannery.blb";

	// iconName = "Add-Ons/Server_Farming/icons/4x_planter";

	cost = 0;
	isProcessor = 1;
	processorFunction = "addCanneryIngredients";
	placerItem = "CanneryItem";
	callOnActivate = 1;
	isPoweredProcessor = 1;
	hasCustomMenu = 1;
	energyUse = 80;
	canRate = 4;
	powerFunction = "canCrops";

	isCannery = 1;
	isRecipeProcessor = 1;

	isStorageBrick = 1;
	storageSlotCount = 2;
	itemStackCount = 0;
	storageMultiplier = 6;

	musicRange = 50;
	musicDescription = "AudioMusicLooping3d";
};


////////
//Item//
////////

datablock ItemData(CanneryItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Cannery";
	image = "CanneryBrickImage";
	colorShiftColor = "0.90 0.48 0 1";

	// iconName = "Add-ons/Server_Farming/icons/4x_planter";
};

datablock ShapeBaseImageData(CanneryBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = CanneryItem;
	
	doColorshift = true;
	colorShiftColor = CanneryItem.colorShiftColor;

	toolTip = "Places a Cannery";
	loopTip = "When powered, compresses stacks of crops into cans";
	placeBrick = "brickCanneryData";
};



package Cannery
{
	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot)
	{
		//reject inserting if we've already determined we can't accept more
		if (%storageObj.dataBlock.isRecipeProcessor && !%storageObj.isAcceptingIngredients)
		{
			return 3;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID); //call parent since poweredProcessor has its own package
		%db = %brick.dataBlock;
		if (%db.isCannery)
		{
			if (%brick.isPoweredOn())
			{
				%energyUse = %db.energyUse;
			}
			else
			{
				%energyUse = 0;
				%db.devicePower = 0;
			}

			%power = mFloor(%brick.devicePower * 100);
			if (%power < 50) %color = "\c0";
			else if (%power < 100) %color = "\c3";
			else %color = "\c2";

			%brick.centerprintMenu.menuOptionCount = 5; //add on/off toggle
			%brick.centerprintMenu.menuOption[0] = %brick.centerprintMenu.menuOption[0] SPC "(Input)";
			%brick.centerprintMenu.menuOption[1] = %brick.centerprintMenu.menuOption[1] SPC "(Output)";
			%brick.centerprintMenu.menuOption[2] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuFunction[2] = "togglePower";
			%brick.centerprintMenu.menuOption[3] = "Progress: " @ %brick.deviceProgress @ "% | Current Power: " @ %color @ mFloor(%brick.devicePower * 100) @ "%";
			%brick.centerprintMenu.menuOption[4] = "Uses " @ %energyUse @ " power per tick";
		}
		return %ret;
	}

	function fxDTSBrick::getEnergyUse(%brick)
	{
		if (%brick.getDatablock().isCannery)
		{
			%db = %brick.getDatablock();
			%dataID = %brick.eventOutputParameter0_1;

			// check if it can process recipe - if not, draw no power
			if (!canCan(%brick))
			{
				return 0;
			}

			%brick.updateStorageMenu(%dataID);
		}
		return parent::getEnergyUse(%brick);
	}
};
activatePackage(Cannery);


function addCanneryIngredients(%brick, %cl, %slot)
{
	if (getTrustLevel(%brick, %cl) < 1)
	{
		serverCmdUnuseTool(%cl);
		%cl.centerprint(getBrickgroupFromObject(%brick).name @ "\c0 does not trust you enough to do that!", 1);
		return;
	}

	%pl = %cl.player;
	%item = %pl.tool[%slot];
	if (%item.isStackable && %item.stackType !$= "")
	{
		if (getCropClass(%item.stackType) $= "")
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("You cannot can non-crop items!", 1);
			return;
		}
		
		//check for space for ingredients - only insert that much
		%dataID = %brick.eventOutputParameter[0, 1];
		%max = %brick.getStorageMax(%item);
		%slot0 = validateStorageValue(getDataIDArrayValue(%dataID, 1));
		%itemCount = getField(%slot0, 2);
		%space = getMin(%pl.toolStackCount[%slot], %max - %itemCount);

		if (%space <= 0)
		{
			return;
		}
		
		%brick.isAcceptingIngredients = 1;
		%success = %brick.insertIntoStorage(%dataID, 
										%item, 
										%space, 
										"");
		%brick.isAcceptingIngredients = 0;
		if (%success == 0) //complete insertion
		{
			%pl.toolStackCount[%slot] -= %space;
			if (%pl.toolStackCount[%slot] <= 0)
			{
				%pl.tool[%slot] = 0;
				%pl.toolStackCount[%slot] = 0;
				messageClient(%cl, 'MsgItemPickup', "", %slot, 0);
				if (%pl.currTool == %slot)
				{
					%pl.unmountImage(0);
				}
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
		%cl.centerprint("You cannot can non-crop items!", 1);
		return;
	}
}

function canCan(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%input = validateStorageValue(getDataIDArrayValue(%dataID, 1));
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 2));
	%cropType = getField(%input, 1);
	%outputType = getField(%output, 1);
	%canType = "Canned" @ %cropType;
	%maxOutput = %brick.getStorageMax(getStackTypeDatablock("Canned" @ %cropType, 1));
	%inputCount = getField(%input, 2);
	%outputCount = getField(%output, 2);
	%outputSpace = %maxOutput - %outputCount;

	if (%inputCount < getMaxStack(%cropType) || %outputSpace < 1
		|| %outputType !$= %canType)
	{
		return false;
	}
	return true;
}

function createCannedCrops(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%input = validateStorageValue(getDataIDArrayValue(%dataID, 1));
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 2));
	%inputCount = getField(%input, 2);
	%outputCount = getField(%output, 2);
	%cropType = getField(%input, 1);

	%newInput = getStorageValue(%cropType, %inputCount - getMaxStack(%cropType));
	if (%inputCount - getMaxStack(%cropType) <= 0)
	{
		%newInput = "";
	}
	%newOutput = getStorageValue("Canned" @ %cropType, %outputCount + 1);
	setDataIDArrayValue(%dataID, 1, %newInput);
	setDataIDArrayValue(%dataID, 2, %newOutput);
	%brick.updateStorageMenu(%dataID);
}

function canCrops(%brick, %powerRatio)
{
	%db = %brick.getDatablock();
	%dataID = %brick.eventOutputParameter0_1;
	%rate = mFloatLength(%powerRatio * %db.canRate, 1);
	%brick.devicePower = %powerRatio;

	if (canCan(%brick) && %brick.deviceProgress >= 100)
	{
		createCannedCrops(%brick);
		%brick.deviceProgress = 0;
	}
	else if (canCan(%brick))
	{
		%brick.deviceProgress = getMin(100, %brick.deviceProgress + %rate);
	}
	else
	{
		%brick.deviceProgress = getMax(%brick.deviceProgress - 10, 0);
	}
}