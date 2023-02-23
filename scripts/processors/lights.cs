

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
			%brick.centerprintMenu.menuOption[2] = "Uses " @ %db.energyUse @ " power per tick";
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