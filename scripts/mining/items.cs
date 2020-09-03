$Stackable_Coal_StackedItem0 = "CoalBasket0Item 4";
$Stackable_Coal_StackedItem1 = "CoalBasket1Item 8";
$Stackable_Coal_StackedItem2 = "CoalBasket2Item 12";
$Stackable_Coal_StackedItemTotal = 3;

datablock ItemData(CoalItem : HammerItem)
{
	shapeFile = "./resources/Coal.dts";
	uiName = "Coal";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Coal";
};

datablock ItemData(CoalBasket0Item : HammerItem)
{
	shapeFile = "./resources/CoalBasket0.dts";
	uiName = "Coal Basket";
	image = "CoalBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Coal_Basket";

	isStackable = 1;
	stackType = "Coal";
};

datablock ItemData(CoalBasket1Item : HammerItem)
{
	shapeFile = "./resources/CoalBasket1.dts";

	uiName = "Half Coal Basket";
	image = "CoalBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Coal_Basket_Half";

	isStackable = 1;
	stackType = "Coal";
};

datablock ItemData(CoalBasket2Item : HammerItem)
{
	shapeFile = "./resources/CoalBasket2.dts";

	uiName = "Full Coal Basket";
	image = "CoalBasket2Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Coal_Star";

	isStackable = 1;
	stackType = "Coal";
};

datablock ShapeBaseImageData(CoalBasket0Image)
{
	shapeFile = "./resources/CoalBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = CoalBasket0Item;
	
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

datablock ShapeBaseImageData(CoalBasket1Image : CoalBasket0Image) 
{
	shapeFile = "./resources/CoalBasket1.dts";

	item = CoalBasket1Item;
};

datablock ShapeBaseImageData(CoalBasket2Image : CoalBasket0Image) 
{
	shapeFile = "./resources/CoalBasket2.dts";

	item = CoalBasket2Item;
};

function CoalBasket0Image::onFire(%this, %obj, %slot)
{

}

function CoalBasket1Image::onFire(%this, %obj, %slot)
{

}

function CoalBasket2Image::onFire(%this, %obj, %slot)
{

}

function CoalBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CoalBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CoalBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CoalBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function CoalBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function CoalBasket2Image::onUnmount(%this, %obj, %slot)
{

}

function CoalBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CoalBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CoalBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}