//////////////
//Flower Pot//
//////////////

datablock fxDTSBrickData(brickPlanterBoxData : brick1x1Data) 
{
	category = "";
	subCategory = "";
	uiName = "Planter Box";

	brickFile = "./resources/planterbox/PlanterBox.blb";

	iconName = "Add-Ons/Server_Farming/icons/4x_planter";

	cost = 0;
	isDirt = 1;
	maxWater = 150;
	maxNutrients = 50;
	maxWeedkiller = 80;
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

	iconName = "Add-ons/Server_Farming/icons/4x_planter";
};

datablock ShapeBaseImageData(PlanterBoxBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = PlanterBoxItem;
	
	doColorshift = true;
	colorShiftColor = PlanterBoxItem.colorShiftColor;

	toolTip = "Places a Planter Box";
	loopTip = "Allows a plant to ignore plants on dirt<br>Min planting radius -1";
	placeBrick = "brickPlanterBoxData";
};