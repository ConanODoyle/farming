

//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickMediumPumpData)
{
	uiName = "Medium Pump";

	brickFile = "./resources/waterpump/mediumPump.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "MediumPumpInfo";
	placerItem = "MediumPumpItem";
	keepActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 2;
	powerFunction = "pumpWater";

	isWaterPump = 1;
	targetDatablock = "brickMediumWaterTankData";
	rotationChange = 1; //add to pump >> tank rotation
	tankOffset = "0 -2.25 0.6"; //add to pump, should be tank location

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
};

datablock fxDTSBrickData(brickLargePumpData)
{
	uiName = "Large Pump";

	brickFile = "./resources/waterpump/LargePump.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "LargePumpInfo";
	placerItem = "LargePumpItem";
	keepActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 3;
	powerFunction = "pumpWater";

	isWaterPump = 1;
	targetDatablock = "brickLargeWaterTankData";
	rotationChange = 1; //add to pump >> tank rotation
	tankOffset = "0 -2.25 0.6"; //add to pump, should be tank location

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
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

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
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

	toolTip = "Places an indoor light";
	loopTip = "When powered, lets plants grow";
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



//////////////
//Light code//
//////////////

package WaterPump
{
	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID); //call parent since poweredProcessor has its own package
		%db = %brick.getDatablock();
		if (%db.isMediumPump)
		{
			%power = mFloor(%brick.lightPower * 100);
			if (%power < 50) %color = "\c0";
			else if (%power < 100) %color = "\c3";
			else %color = "\c2";
			%brick.centerprintMenu.menuOptionCount = 1; //only keep the first power toggle option accessible
			%brick.centerprintMenu.menuOption[1] = "Current Power: " @ %color @ mFloor(%brick.lightPower * 100) @ "%";
			%brick.centerprintMenu.menuFunction[1] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuOption[2] = "Uses " @ %db.energyUse @ " power per tick";
		}
		return %ret;
	}

	function fxDTSBrick::getEnergyUse(%brick)
	{
		if (%brick.getDatablock().isWaterPump)
		{

		}
		return fxDTSBrick::getEnergyUse(%brick);
	}
};
activatePackage(WaterPump);

function powerLight(%brick, %powerRatio)
{
	%newLightPower = getMin(1, %powerRatio);
	%oldLightPower = %brick.lightPower;
	%brick.lightPower = %newLightPower;
	if (%newlightPower != %oldLightPower)
	{
		%brick.updateStorageMenu(%brick.eventOutputParameter0_1);
	}

	if (%newLightPower > 0 && !isObject(%brick.emitter))
	{
		%brick.setEmitter("silverEmitter");
	}
	else if (%newLightPower <= 0 && isObject(%brick.emitter))
	{
		%brick.setEmitter("None");
	}
}

function checkForTank(%pump)
{
	if (!isObject(%pump) || !%pump.getDatablock().isWaterPump)
	{
		return;
	}

	%rotation = (%pump.getDatablock().rotationChange + %pump.angleID) % 4;
	%offset = %pump.getDatablock().tankOffset;
	
	%x = getWord(%offset, 0);
	%y = getWord(%offset, 1);
	%z = getWord(%offset, 2);

	switch (%pump.angleID)
	{
		case 0: %xf = %x;		%yf = %y;
		case 1: %xf = %y;		%yf = %x;
		case 2: %xf = -1 * %x;	%yf = -1 * %y;
		case 3: %xf = -1 * %y;	%yf = -1 * %x;
		default: talk("REEEEEEE rotation issues REEEEEE");
	}
	%offset = %xf SPC %yf SPC %z;

	%top = vectorAdd(%pump.getPosition(), "0 0 " @ (%pump.getDatablock().brickSizeZ * 0.1 - 0.1));
	%end = vectorAdd(%top, %xf SPC %yf SPC 0);
	%ray = containerRaycast(%top, %end, $Typemasks::fxBrickObjectType, %pump);

	if (isObject(%hit = getWord(%ray, 0)))
	{
		%diff = vectorSub(%hit.getPosition(), %pump.getPosition());
		%rot = %hit.angleID;

		talk(%diff);
		if (vectorDist(%diff, %offset) < 0.05 && %rot == %rotation)
		{
			return %hit;
		}
	}
	return 0;
}