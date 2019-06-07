//////////////
//Flower Pot//
//////////////

datablock fxDTSBrickData(brickPlanterBoxData : brick1x1Data) 
{
	category = "";
	subCategory = "";
	uiName = "Planter Box";

	brickFile = "./resources/PlanterBox.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/4x_planter";

	cost = 0;
	isDirt = 1;
	maxWater = 150;
	// isPot = 1;
	isPlanter = 1;
	placerItem = "PlanterBoxItem";
	isProcessor = 1;
};


////////
//Item//
////////

datablock ItemData(PlanterBoxItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Planter Box";
	image = "PlanterBoxBrickImage";
	colorShiftColor = "0.90 0.48 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/4x_planter";

	cost = 150;
};

datablock ShapeBaseImageData(PlanterBoxBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 -0.25";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = PlanterBoxItem;
	
	doColorshift = true;
	colorShiftColor = PlanterBoxItem.colorShiftColor;

	toolTip = "Places a Planter Box";
	loopTip = "Allows a plant to ignore plants on dirt, min planting radius -1";
	placeBrick = "brickPlanterBoxData";
};

function PlanterBoxBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function PlanterBoxBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function PlanterBoxBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function PlanterBoxBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}