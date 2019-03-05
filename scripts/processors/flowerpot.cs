//////////////
//Flower Pot//
//////////////

datablock fxDTSBrickData(brickFlowerPotData : brick1x1Data) 
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Flower Pot";

	brickFile = "./resources/flowerPot.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/flower_pot";

	cost = 0;
	isDirt = 1;
	maxWater = 30;
	isPot = 1;
	customRadius = -1.05;
	placerItem = "FlowerPotItem";
	isProcessor = 1;
};


////////
//Item//
////////

datablock ItemData(FlowerPotItem : brickPlacerItem)
{
	shapeFile = "./resources/FlowerpotItem.dts";
	uiName = "Flower Pot";
	image = "FlowerpotBrickImage";
	colorShiftColor = "0.5 0.5 0.5 1";

	iconName = "Add-ons/Server_Farming/crops/icons/flower_pot";

	cost = 100;
};

datablock ShapeBaseImageData(FlowerpotBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/FlowerpotItem.dts";
	
	offset = "-0.56 0 -0.25";
	eyeOffset = "0 0 0";

	item = FlowerpotItem;
	
	doColorshift = true;
	colorShiftColor = FlowerpotItem.colorShiftColor;

	toolTip = "Places a Flower Pot";
	placeBrick = "brickFlowerpotData";
};

function FlowerpotBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function FlowerpotBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function FlowerpotBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function FlowerpotBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}