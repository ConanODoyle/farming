datablock ItemData(WeedCutterItem : HammerItem)
{
	iconName = "Add-Ons/Server_Farming/icons/weed_whacker";
	shapeFile = "./weedWhacker/weed_whacker.dts";
	uiName = "Weed Cutter";

	image = "WeedCutterImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;

	hasDataID = 1;
	isDataIDTool = 1;
	toolType = "Weed Cutter";
	
	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 20;
	chanceDurability = 0.7;
	bonusDurability = 10;
};

datablock ShapeBaseImageData(WeedCutterImage : TrowelImage)
{
	shapeFile = "./weedWhacker/weed_whacker.dts";

	emap = true;
	armReady = true;

	item = WeedCutterItem;
	doColorShift = false;
	colorShiftColor = WeedCutterItem.colorShiftColor;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area remove weeds";

	harvestEffect = "FarmingHarvestAboveGroundPlantProjectile";
};