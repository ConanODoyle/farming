

//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickIndoorLightData)
{
	uiName = "Indoor Light 2x6";

	brickFile = "./resources/light/2x6light.blb";

	iconName = "Add-ons/Server_Farming/icons/IndoorLight2x6";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "IndoorLightInfo";
	placerItem = "IndoorLightItem";
	callOnActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 1;
	powerFunction = "powerLight";

	isIndoorLight = 1;
	baseLightLevel = 0.75;

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
};

datablock fxDTSBrickData(brickIndoorLight4x6Data)
{
	uiName = "Indoor Light 4x6";

	brickFile = "./resources/light/4x6light.blb";

	iconName = "Add-ons/Server_Farming/icons/IndoorLight4x6";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "IndoorLightInfo";
	placerItem = "IndoorLight4x6Item";
	callOnActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 2;
	powerFunction = "powerLight";

	isIndoorLight = 1;
	baseLightLevel = 0.75;

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
};

datablock fxDTSBrickData(brickIndoorLight4x8Data)
{
	uiName = "Indoor Light 4x8";

	brickFile = "./resources/light/4x8light.blb";

	iconName = "Add-ons/Server_Farming/icons/IndoorLight4x8";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "IndoorLightInfo";
	placerItem = "IndoorLight4x8Item";
	callOnActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 2;
	powerFunction = "powerLight";

	isIndoorLight = 1;
	baseLightLevel = 0.75;

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
};



///////////////
//Placer Item//
///////////////

datablock ItemData(IndoorLightItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Indoor Light - 2x6";
	image = "IndoorLightBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/IndoorLight2x6";
};

datablock ShapeBaseImageData(IndoorLightBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = IndoorLightItem;
	
	doColorshift = true;
	colorShiftColor = IndoorLightItem.colorShiftColor;

	toolTip = "Places an indoor light";
	loopTip = "When powered, lets plants grow";
	placeBrick = "brickIndoorLightData";
};

datablock ItemData(IndoorLight4x6Item : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Indoor Light - 4x6";
	image = "IndoorLight4x6BrickImage";
	colorShiftColor = "0.7 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/IndoorLight4x6";
};

datablock ShapeBaseImageData(IndoorLight4x6BrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = IndoorLight4x6Item;
	
	doColorshift = true;
	colorShiftColor = IndoorLight4x6Item.colorShiftColor;

	toolTip = "Places an indoor light";
	loopTip = "When powered, lets plants grow";
	placeBrick = "brickIndoorLight4x6Data";
};

datablock ItemData(IndoorLight4x8Item : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Indoor Light - 4x8";
	image = "IndoorLight4x8BrickImage";
	colorShiftColor = "0.5 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/IndoorLight4x8";
};

datablock ShapeBaseImageData(IndoorLight4x8BrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = IndoorLight4x8Item;
	
	doColorshift = true;
	colorShiftColor = IndoorLight4x8Item.colorShiftColor;

	toolTip = "Places an indoor light";
	loopTip = "When powered, lets plants grow";
	placeBrick = "brickIndoorLight4x8Data";
};



//////////////
//Light code//
//////////////

package PoweredLight
{
	function fxDTSBrick::getLightLevel(%brick, %lightLevel)
	{
		%db = %brick.getDatablock();
		if (%db.isIndoorLight)
		{
			return %lightLevel * %brick.lightPower * %db.baseLightLevel;
		}
		return parent::getLightLevel(%brick, %lightLevel);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID); //call parent since poweredProcessor has its own package
		%db = %brick.getDatablock();
		if (%db.isIndoorLight)
		{
			%power = mFloor(%brick.lightPower * 100);
			if (%power < 50) %color = "\c0";
			else if (%power < 100) %color = "\c3";
			else %color = "\c2";
			%brick.centerprintMenu.menuOptionCount = 1; //only keep the first power toggle option accessible
			%brick.centerprintMenu.menuOption[1] = "Current Power: " @ %color @ mFloor(%brick.lightPower * 100) @ "%";
			%brick.centerprintMenu.menuFunction[1] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuOption[2] = "Light level: " @ mFloor(%brick.lightPower * %db.baseLightLevel * 100) @ "%";
			%brick.centerprintMenu.menuOption[3] = "Uses " @ %db.energyUse @ " power per tick";
		}
		return %ret;
	}
};
activatePackage(PoweredLight);

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