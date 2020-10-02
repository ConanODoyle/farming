datablock ItemData(WeedWhackerItem : HammerItem)
{
	iconName = "Add-Ons/Server_Farming/crops/icons/weed_whacker";
	shapeFile = "./weed_whacker.dts";
	uiName = "Weed Cutter";

	image = "WeedWhackerImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;

	cost = 1200;
};

datablock ShapeBaseImageData(WeedWhackerImage)
{
	shapeFile = "./weed_whacker.dts";

	emap = true;
	armReady = true;

	item = WeedWhackerItem;
	doColorShift = fakse;
	colorShiftColor = WeedWhackerItem.colorShiftColor;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area remove and prevent weeds";

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
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";
};

function WeedWhackerImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
	%obj.playThread(0, plant);
}
