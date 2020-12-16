

//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickMediumPumpData)
{
	uiName = "Medium Pump";

	brickFile = "./resources/waterpump/medTankPump.blb";

	iconName = "Add-Ons/Server_Farming/icons/MediumTankPump";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "MediumPumpInfo";
	placerItem = "MediumPumpItem";
	callOnActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 2;
	pumpPowerMod = 2;
	baseRate = 100;
	pumpRate = 50;
	maxRate = 4;
	powerFunction = "pumpWater";

	isWaterPump = 1;
	targetDatablock = "brickMediumWaterTankData";
	rotationChange = 0; //add to pump >> tank rotation
	tankOffset = "0 0.25 0.4"; //add to pump, should be tank location

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;

	musicRange = 50;
	musicDescription = "AudioMusicLooping3d";
};

datablock fxDTSBrickData(brickLargePumpData)
{
	uiName = "Large Pump";

	brickFile = "./resources/waterpump/largeTankPump.blb";

	iconName = "Add-Ons/Server_Farming/icons/LargeTankPump";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "LargePumpInfo";
	placerItem = "LargePumpItem";
	callOnActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 3;
	pumpPowerMod = 2;
	baseRate = 150;
	pumpRate = 100;
	maxRate = 6;
	powerFunction = "pumpWater";

	isWaterPump = 1;
	targetDatablock = "brickLargeWaterTankData";
	rotationChange = 0; //add to pump >> tank rotation
	tankOffset = "0.75 -1.75 1"; //add to pump, should be tank location

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;

	musicRange = 50;
	musicDescription = "AudioMusicLooping3d";
};



///////////////
//Placer Item//
///////////////

datablock ItemData(MediumPumpItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Medium Tank Pump";
	image = "MediumPumpBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/MediumTankPump";
};

datablock ShapeBaseImageData(MediumPumpBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = MediumPumpItem;
	
	doColorshift = true;
	colorShiftColor = MediumPumpItem.colorShiftColor;

	toolTip = "Places a Medium Tank water pump";
	loopTip = "When powered, pumps water into a Medium Tank";
	placeBrick = "brickMediumPumpData";
};

function MediumPumpBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function MediumPumpBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function MediumPumpBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function MediumPumpBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(LargePumpItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Large Tank Pump";
	image = "LargePumpBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/LargeTankPump";
};

datablock ShapeBaseImageData(LargePumpBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = LargePumpItem;
	
	doColorshift = true;
	colorShiftColor = LargePumpItem.colorShiftColor;

	toolTip = "Places a Large Tank water pump";
	loopTip = "When powered, pumps water into a Large Tank";
	placeBrick = "brickLargePumpData";
};

function LargePumpBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function LargePumpBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function LargePumpBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function LargePumpBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



//////////////
//Light code//
//////////////

package WaterPump
{
	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID); //call parent since poweredProcessor has its own package
		%db = %brick.getDatablock();
		if (%db.isWaterPump)
		{
			if (isObject(%brick.pumpTank) && %brick.isPoweredOn())
			{
				%water = %db.baseRate * %brick.pumpPower 
					+ mCeil(getDataIDArrayTagValue(%dataID, "rate") * %brick.pumpPower * %brick.pumpPower * %db.pumpRate);
			}
			else
			{
				%water = 0;
			}

			if (%brick.isPoweredOn())
			{
				%energyUse = %db.energyUse + getDataIDArrayTagValue(%dataID, "rate") * %db.pumpPowerMod;
			}
			else
			{
				%energyUse = 0;
				%brick.pumpPower = 0;
			}

			%power = mFloor(%brick.pumpPower * 100);
			if (%power < 50) %color = "\c0";
			else if (%power < 100) %color = "\c3";
			else %color = "\c2";
			
			%brick.centerprintMenu.menuOptionCount = 3; //only keep the first three power toggle option accessible
			%brick.centerprintMenu.menuOption[1] = "Inc. rate";
			%brick.centerprintMenu.menuFunction[1] = "increasePumpRate";
			%brick.centerprintMenu.menuOption[2] = "Dec. rate";
			%brick.centerprintMenu.menuFunction[2] = "decreasePumpRate";
			%brick.centerprintMenu.menuOption[3] = "Current rate: " @ %water @ " | " 
											@ " Current Power: " @ %color @ mFloor(%brick.pumpPower * 100) @ "%";
			%brick.centerprintMenu.menuOption[4] = "Uses " @ %energyUse @ " power per tick";
		}
		return %ret;
	}

	function fxDTSBrick::getEnergyUse(%brick)
	{
		if (%brick.getDatablock().isWaterPump)
		{
			%db = %brick.getDatablock();
			%dataID = %brick.eventOutputParameter0_1;
			if (!isObject(%brick.pumpTank))
			{
				%brick.pumpTank = checkForTank(%brick);
				if (!isObject(%brick.pumpTank))
				{
					return 0;
				}
			}

			%power = %db.energyUse + getDataIDArrayTagValue(%dataID, "rate") * %db.pumpPowerMod;
            %brick.updateStorageMenu(%dataID);
			return %power;
		}
		return parent::getEnergyUse(%brick);
	}
};
activatePackage(WaterPump);

function increasePumpRate(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%rate = getMin(getDataIDArrayTagValue(%dataID, "rate") + 1, %brick.getDatablock().maxRate);
	setDataIDArrayTagValue(%dataID, "rate", %rate);
	serverPlay3D(ToggleStartSound, %brick.getPosition());
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return %rate;
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

function pumpWater(%brick, %powerRatio)
{
    %db = %brick.getDatablock();
    %dataID = %brick.eventOutputParameter0_1;
    %water = %db.baseRate * %powerRatio 
        + mCeil(getDataIDArrayTagValue(%dataID, "rate") * %powerRatio * %powerRatio * %db.pumpRate);
    if (isObject(%brick.pumpTank))
    {
        %brick.pumpTank.setWaterLevel(%brick.pumpTank.waterLevel + %water);
    }
    %brick.pumpPower = %powerRatio;

    if (%powerRatio > 0)
    {
        if (!isObject(%brick.audioEmitter))
        {
            %brick.setMusic("WaterPumpSound".getID());
        }
    }
    else if (%powerRatio <= 0)
    {
        if (isObject(%brick.audioEmitter))
        {
            %brick.setMusic("");
        }
    }
}

function checkForTank(%pump)
{
	if (!isObject(%pump) || !%pump.getDatablock().isWaterPump)
	{
		return 0;
	}

	%rotation = (%pump.getDatablock().rotationChange + %pump.angleID) % 4;
	%offset = %pump.getDatablock().tankOffset;
	
	%x = getWord(%offset, 0);
	%y = getWord(%offset, 1);
	%z = getWord(%offset, 2);

	switch (%pump.angleID)
	{
		case 0: %xf = %x;		%yf = %y;
		case 1: %xf = %y;		%yf = -1 * %x;
		case 2: %xf = -1 * %x;	%yf = -1 * %y;
		case 3: %xf = -1 * %y;	%yf = %x;
		default: talk("REEEEEEE rotation issues REEEEEE");
	}
	%offset = %xf SPC %yf SPC %z;

	%box = "1 1 1";
	initContainerBoxSearch(%pump.getPosition(), %box, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (!%next.getDatablock().isWaterTank)
		{
			continue;
		}

		%diff = vectorSub(%next.getPosition(), %pump.getPosition());
		%rot = %next.angleID;

		if (vectorDist(%diff, %offset) < 0.05 && %rot == %rotation 
			&& %next.getDatablock() $= %pump.getDatablock().targetDatablock.getID())
		{
			return %next;
		}
	}
	return 0;
}



datablock AudioProfile(WaterPumpSound : BatteryLoopSound)
{
    description = AudioMusicLoopingLoud3d;
    preload = true;
    uiName = "";
};

datablock AudioDescription(AudioMusicLoopingLoud3d : AudioMusicLooping3d)
{
    volume = 1;
    maxDistance = 20;
};