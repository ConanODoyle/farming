

//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickIndoorLightData)
{
	uiName = "Indoor Light 2x6";

	brickFile = "./resources/power/controlBoxClosed.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "IndoorLightInfo";
	placerItem = "IndoorLightItem";
	keepActivate = 1;
	isPoweredProcessor = 1;
	energyUse = 2;
	powerFunction = "";

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
};



///////////////
//Placer Item//
///////////////

datablock ItemData(IndoorLightItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Coal Generator";
	image = "IndoorLightBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
	
	cost = 800;
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

function IndoorLightBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function IndoorLightBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function IndoorLightBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function IndoorLightBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}