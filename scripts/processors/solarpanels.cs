
//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickSolarPanel4x4Data)
{
	uiName = "Solar Panel 4x4";

	brickFile = "./resources/solarpanels/solarpanel4x.blb";

	iconName = "Add-Ons/Server_Farming/icons/SolarPanel4x";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "addFuel";
	// activateFunction = "EthanolGeneratorInfo";
	placerItem = "SolarPanel4x4Item";
	callOnActivate = 1;
	isGenerator = 1;
	// burnRate = 0.00;
	// fuelType = "";
	generation = 2;
	generatorFunction = "solarPanelGeneratorTick";

	isStorageBrick = 1;
	storageSlotCount = 0;
	itemStackCount = 0;
	storageMultiplier = 0;

	musicRange = 0;
	musicDescription = "AudioMusicLooping3d";
};

datablock fxDTSBrickData(brickSolarPanel8x8Data)
{
	uiName = "Solar Panel 8x8";

	brickFile = "./resources/solarpanels/solarpanel8x.blb";

	iconName = "Add-Ons/Server_Farming/icons/SolarPanel8x";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "addFuel";
	// activateFunction = "EthanolGeneratorInfo";
	placerItem = "SolarPanel8x8Item";
	callOnActivate = 1;
	isGenerator = 1;
	// burnRate = 0.00;
	// fuelType = "";
	generation = 12;
	generatorFunction = "solarPanelGeneratorTick";

	isStorageBrick = 1;
	storageSlotCount = 0;
	itemStackCount = 0;
	storageMultiplier = 0;

	musicRange = 0;
	musicDescription = "AudioMusicLooping3d";
};



///////////////
//Placer Item//
///////////////

datablock ItemData(SolarPanel4x4Item : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Solar Panel 4x4";
	image = "SolarPanel4x4BrickImage";
	colorShiftColor = "0 0 0.5 1";

	iconName = "Add-ons/Server_Farming/icons/SolarPanel4x42x6";
};

datablock ShapeBaseImageData(SolarPanel4x4BrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = SolarPanel4x4Item;
	
	doColorshift = true;
	colorShiftColor = SolarPanel4x4Item.colorShiftColor;

	toolTip = "Places a solar panel";
	loopTip = "Provides 2 power when outside";
	placeBrick = "brickSolarPanel4x4Data";
};

datablock ItemData(SolarPanel8x8Item : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Solar Panel 8x8";
	image = "SolarPanel8x8BrickImage";
	colorShiftColor = "0 0 0.5 1";

	iconName = "Add-ons/Server_Farming/icons/SolarPanel8x82x6";
};

datablock ShapeBaseImageData(SolarPanel8x8BrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = SolarPanel8x8Item;
	
	doColorshift = true;
	colorShiftColor = SolarPanel8x8Item.colorShiftColor;

	toolTip = "Places a solar panel";
	loopTip = "Provides 12 power when outside";
	placeBrick = "brickSolarPanel8x8Data";
};



/////////////
//Sun Check//
/////////////

function solarPanelGeneratorTick(%gen, %genDataID)
{
	%genDB = %gen.dataBlock;

	%box = %gen.getWorldBox();
	%x1 = getWord(%box, 0);
	%y1 = getWord(%box, 1);
	%x2 = getWord(%box, 3);
	%y2 = getWord(%box, 4);

	%xf = %x1 + (%x2 - %x1) * getRandom();
	%yf = %y1 + (%y2 - %y1) * getRandom();
	%pos = %xf SPC %yf SPC getWord(%gen.position, 2);

	%light = lightRaycastCheck(%pos, %gen);
	return mFloor(%light * %genDB.generation);
}


package SolarPanels
{
	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID);
		%db = %brick.getDatablock();
		if (stripos(%db.uiName, "Solar Panel") == 0)
		{
			%brick.centerprintMenu.menuOption[0] = "Generates " @ %brick.dataBlock.generation @ " watts per tick";
		}

		return %ret;
	}
};
activatePackage(SolarPanels);