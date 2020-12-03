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
	
	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 20;
	chanceDurability = 0.7;
	bonusDurability = 10;
};

datablock ShapeBaseImageData(WeedCutterImage)
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

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 0.2;
	stateTransitionOnTimeout[2] = "Repeat";
	stateWaitForTimeout[2] = 1;

	stateName[3] = "Repeat";
	stateTimeoutValue[3] = 0.12;
	stateTransitixonOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";
};

function WeedCutterImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
	%obj.playThread(0, plant);
}
