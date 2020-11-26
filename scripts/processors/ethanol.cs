
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
