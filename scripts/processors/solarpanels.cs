
//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickSolarPanel4x4Data)
{
	uiName = "Solar Panel 4x4";

	brickFile = "./resources/power/solarpanel4x.blb";

	iconName = "Add-Ons/Server_Farming/icons/SolarPanel4x";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "addFuel";
	// activateFunction = "EthanolGeneratorInfo";
	placerItem = "SolarPanel4x4Item";
	callOnActivate = 1;
	isGenerator = 1;
	isSolarPanel = 1;
	burnRate = 0.00; //bodge: generator burns 0 fuel
	fuelType = ""; //bodge: will pass fuel check due to empty string
	generation = 4;

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

	brickFile = "./resources/power/solarpanel8x.blb";

	iconName = "Add-Ons/Server_Farming/icons/SolarPanel8x";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "addFuel";
	// activateFunction = "EthanolGeneratorInfo";
	placerItem = "SolarPanel8x8Item";
	callOnActivate = 1;
	isGenerator = 1;
	isSolarPanel = 1;
	burnRate = 0.00; //bodge: generator burns 0 fuel
	fuelType = ""; //bodge: will pass fuel check due to empty string
	generation = 20;

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
	uiName = "Indoor Light - 2x6";
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

	toolTip = "Places an indoor light";
	loopTip = "When powered, lets plants grow";
	placeBrick = "brickSolarPanel4x4Data";
};

datablock ItemData(SolarPanel8x8Item : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Indoor Light - 2x6";
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

	toolTip = "Places an indoor light";
	loopTip = "When powered, lets plants grow";
	placeBrick = "brickSolarPanel8x8Data";
};