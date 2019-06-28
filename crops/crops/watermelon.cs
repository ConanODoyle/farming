$Stackable_Watermelon_StackedItem0 = "WatermelonBasket0Item 1";
$Stackable_Watermelon_StackedItemTotal = 1;

datablock ItemData(WatermelonItem : HammerItem)
{
	shapeFile = "./Watermelon.dts";
	uiName = "Watermelon";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Watermelon";
};

datablock ItemData(WatermelonBasket0Item : HammerItem)
{
	shapeFile = "./WatermelonBasket0.dts";
	uiName = "Watermelon Basket";
	image = "WatermelonBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Watermelon_Star";

	isStackable = 1;
	stackType = "Watermelon";
};

datablock ShapeBaseImageData(WatermelonBasket0Image)
{
	shapeFile = "./WatermelonBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = WatermelonBasket0Item;
	
	armReady = 1;

	offset = "-0.5017 0.04 -0.17389";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "LoopA";
	stateTimeoutValue[3] = 0.1;
	stateWaitForTimeout[3] = true;
};

function WatermelonBasket0Image::onFire(%this, %obj, %slot)
{

}

function WatermelonBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function WatermelonBasket0Image::onUnmount(%this, %obj, %slot)
{

}
function WatermelonBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}
