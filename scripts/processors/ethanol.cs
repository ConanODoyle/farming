
//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickEthanolGeneratorData)
{
	uiName = "Ethanol Generator";

	brickFile = "./resources/power/EthanolGenerator.blb";

	iconName = "Add-Ons/Server_Farming/icons/EthanolGenerator";

	cost = 0;
	isProcessor = 1;
	processorFunction = "addFuel";
	// activateFunction = "EthanolGeneratorInfo";
	placerItem = "EthanolGeneratorItem";
	callOnActivate = 1;
	isGenerator = 1;
	burnRate = 0.01;
	generation = 50;
	fuelType = "Ethanol";

	isStorageBrick = 1;
	storageSlotCount = 1;
	itemStackCount = 0;
	storageMultiplier = 25;

	musicRange = 30;
	musicDescription = "AudioMusicLooping3d";
};

datablock fxDTSBrickData(brickEthanolRefineryData)
{
	uiName = "Ethanol Refinery";

	brickFile = "./resources/refinery/refinery.blb";

	iconName = "Add-Ons/Server_Farming/icons/EthanolTankRefinery";

	cost = 0;
	isProcessor = 1;
	processorFunction = "addEthanolIngredients";
	// activateFunction = "EthanolRefineryInfo";
	placerItem = "EthanolRefineryItem";
	callOnActivate = 1;
	isPoweredProcessor = 1;
	hasCustomMenu = 1;
	energyUse = 5;
	refineRate = 5;
	powerFunction = "refineEthanol";

	isEthanolRefinery = 1;
	isRecipeProcessor = 1;

	isStorageBrick = 1;
	storageSlotCount = 2;
	itemStackCount = 0;
	storageMultiplier = 10;

	musicRange = 50;
	musicDescription = "AudioMusicLooping3d";
};






///////////////
//Placer Item//
///////////////

datablock ItemData(EthanolGeneratorItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Ethanol Generator";
	image = "EthanolGeneratorBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/EthanolGenerator";
};

datablock ShapeBaseImageData(EthanolGeneratorBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = EthanolGeneratorItem;
	
	doColorshift = true;
	colorShiftColor = EthanolGeneratorItem.colorShiftColor;

	toolTip = "Places a Ethanol Generator";
	loopTip = "Converts ethanol into " @ brickEthanolGeneratorData.generation @ " power (" @ brickEthanolGeneratorData.burnRate @ "/tick)";
	placeBrick = "brickEthanolGeneratorData";
};

function EthanolGeneratorBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function EthanolGeneratorBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function EthanolGeneratorBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function EthanolGeneratorBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}





////////////////
//Ethanol Item//
////////////////

$Stackable_Ethanol_StackedItem0 = "EthanolItem 50";
$Stackable_Ethanol_StackedItemTotal = 1;

datablock ItemData(EthanolItem : HammerItem)
{
	shapeFile = "./resources/tank.dts";
	uiName = "Ethanol";
	image = EthanolImage;
	doColorShift = true;
	colorShiftColor = "0.9 0.8 0 1";
	iconName = "";

	isStackable = 1;
	stackType = "Ethanol";
};

datablock ShapeBaseImageData(EthanolImage)
{
	shapeFile = "./resources/tank.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = EthanolItem.colorShiftColor;

	item = EthanolItem;
	
	armReady = 1;

	offset = "-0.56 0 -0.3";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "LoopA";
	stateTimeoutValue[3] = 0.1;
	stateWaitForTimeout[3] = true;
};

function EthanolImage::onFire(%this, %obj, %slot)
{

}

function EthanolImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function EthanolImage::onUnmount(%this, %obj, %slot)
{

}

function EthanolImage::onLoop(%this, %obj, %slot)
{
	ethanolLoop(%this, %obj);
}

function ethanolLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right>\c3-Ethanol " @ %obj.currTool + 1 @ "- <br>" @ %type @ "\c6: " @ %count @ " ", 1);
	}
}



/////////////////
//Refinery Code//
/////////////////

datablock ItemData(EthanolRefineryItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Ethanol Tank Refinery";
	image = "EthanolRefineryBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/EthanolTankRefinery";
};

datablock ShapeBaseImageData(EthanolRefineryBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = EthanolRefineryItem;
	
	doColorshift = true;
	colorShiftColor = EthanolRefineryItem.colorShiftColor;

	toolTip = "Places a Ethanol Refinery";
	loopTip = "When powered, refines corn into Ethanol";
	placeBrick = "brickEthanolRefineryData";
};

function EthanolRefineryBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function EthanolRefineryBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function EthanolRefineryBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function EthanolRefineryBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}




















package EthanolRefinery
{
	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID)
	{
		//reject inserting if we've already determined we can't accept more
		if (%storageObj.dataBlock.isRecipeProcessor && !%storageObj.isAcceptingIngredients)
		{
			return 3;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID); //call parent since poweredProcessor has its own package
		%db = %brick.dataBlock;
		if (%db.isEthanolRefinery)
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
			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
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
		if (%brick.getDatablock().isRecipeProcessor)
		{
			%db = %brick.getDatablock();
			%dataID = %brick.eventOutputParameter0_1;

			// check if it can process recipe - if not, draw no power
			if (!canCreateEthanol(%brick))
			{
				return 0;
			}

			%power = %db.energyUse;
			%brick.updateStorageMenu(%dataID);
			return %power;
		}
		return parent::getEnergyUse(%brick);
	}
};
activatePackage(EthanolRefinery);


function addEthanolIngredients(%brick, %cl, %slot)
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
		if (%item.stackType !$= "Corn")
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("This refinery only accepts corn!", 1);
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
		%cl.centerprint("You cannot process this into ethanol!", 1);
		return;
	}
}

$Ethanol::InputAmount = 5;
$Ethanol::OutputAmount = 2;

function canCreateEthanol(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%input = validateStorageValue(getDataIDArrayValue(%dataID, 1));
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 2));
	%maxOutput = %brick.getStorageMax(getStackTypeDatablock("ethanol", 1));
	%inputCount = getField(%input, 2);
	%outputCount = getField(%output, 2);
	%outputSpace = %maxOutput - %outputCount;

	if (%inputCount < $Ethanol::InputAmount || %outputSpace < $Ethanol::OutputAmount)
	{
		return false;
	}
	return true;
}

function createEthanol(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%input = validateStorageValue(getDataIDArrayValue(%dataID, 1));
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 2));
	%inputCount = getField(%input, 2);
	%outputCount = getField(%output, 2);

	%newInput = getStorageValue("Corn", %inputCount - $Ethanol::InputAmount);
	%newOutput = getStorageValue("Ethanol", %outputCount + $Ethanol::OutputAmount);
	setDataIDArrayValue(%dataID, 1, %newInput);
	setDataIDArrayValue(%dataID, 2, %newOutput);
	%brick.updateStorageMenu(%dataID);
}

function refineEthanol(%brick, %powerRatio)
{
	%db = %brick.getDatablock();
	%dataID = %brick.eventOutputParameter0_1;
	%rate = mFloatLength(%powerRatio * %db.refineRate, 1);
	%brick.devicePower = %powerRatio;

	if (canCreateEthanol(%brick) && %brick.deviceProgress >= 100)
	{
		createEthanol(%brick);
		%brick.deviceProgress = 0;
	}
	else if (canCreateEthanol(%brick))
	{
		%brick.deviceProgress = getMin(100, %brick.deviceProgress + %rate);
	}
	else
	{
		%brick.deviceProgress = getMax(%brick.deviceProgress - 10, 0);
	}
}