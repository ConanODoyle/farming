
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
	keepActivate = 1;
	isGenerator = 1;
	burnRate = 0.01;
	generation = 30;
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

	brickFile = "./resources/waterRefinery/medTankRefinery.blb";

	iconName = "Add-Ons/Server_Farming/icons/EthanolTankRefinery";

	cost = 0;
	isProcessor = 1;
	processorFunction = "addEthanolIngredients";
	// activateFunction = "EthanolRefineryInfo";
	placerItem = "EthanolRefineryItem";
	keepActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 5;
	refineRate = 5;
	powerFunction = "refineEthanol";

	isEthanolRefinery = 1;
	isRecipeProcessor = 1;

	isStorageBrick = 1; //purely for the gui, don't enable storage
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
	loopTip = "Converts fuel into power";
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

	toolTip = "Places a Ethanol Tank water Refinery";
	loopTip = "When powered, Refinerys water into a Ethanol Tank";
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
		if (%storageObj.getDatablock().isRecipeProcessor && !%storageObj.isAcceptingIngredients)
		{
			return 3;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID); //call parent since poweredProcessor has its own package
		%db = %brick.getDatablock();
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
			
			%brick.centerprintMenu.menuOptionCount = 3; //add on/off toggle
			%brick.centerprintMenu.menuOption[0] = %brick.centerprintMenu.menuOption[0] SPC "(Input)";
			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuOption[1] SPC "(Output)";
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

			%brick.recipeProcessor

			%power = %db.energyUse + getDataIDArrayTagValue(%dataID, "rate") * %db.pumpPowerMod;
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
		%cl.centerprint(getBrickgroupFromObject(%brick).name @ "<color:ff0000> does not trust you enough to do that!", 1);
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
		
		%dataID = %brick.eventOutputParameter[0, 1];
		%max = %brick.getStorageMax(%item);
		%slot0 = validateStorageValue(getDataIDArrayValue(%dataID, 0));
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
										%pl.toolDataID[%slot]);
		%brick.isAcceptingIngredients = 0;
		if (%success == 0) //complete insertion
		{
			%pl.toolStackCount[%slot] -= %space;
			if (%pl.toolStackCount[%slot] <= 0)
			{
				%pl.tool[%slot] = 0;
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

function decreasePumpRate(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%rate = getMax(getDataIDArrayTagValue(%dataID, "rate") - 1, 0);
	setDataIDArrayTagValue(%dataID, "rate", %rate);
	serverPlay3D(ToggleStopSound, %brick.getPosition());
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return %rate;
}

function canCreateEthanol(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%input = validateStorageValue(getDataIDArrayValue(%dataID, 0));
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 1));
	%maxOutput = %brick.getStorageMax(getStackTypeDatablock("ethanol", 1));
	%inputCount = getField(%input, 2);
	%outputCount = getField(%output, 2);
	%outputSpace = getMin(%pl.toolStackCount[%slot], %maxOutput - %outputCount);

	if (%inputCount < 1 || %outputSpace < 2)
	{
		return false;
	}
	return true;
}

function createEthanol(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%input = validateStorageValue(getDataIDArrayValue(%dataID, 0));
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 1));
	%inputCount = getField(%input, 2);
	%outputCount = getField(%output, 2);

	%newInput = getStorageValue("Corn", %inputCount - 1);
	%newOutput = getStorageValue("Ethanol", %outputCount + 2);
	setDataIDArrayValue(%dataID, 0, %newInput);
	setDataIDArrayValue(%dataID, 1, %newOutput);
}

function refineEthanol(%brick, %powerRatio)
{
    %db = %brick.getDatablock();
    %dataID = %brick.eventOutputParameter0_1;
    %rate = mFloor(%powerRatio * %db.refineRate);
    %brick.devicePower = %powerRatio;

    if (%brick.deviceProgress == 100)
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
    	%brick.deviceProgress = 0;
    }
}