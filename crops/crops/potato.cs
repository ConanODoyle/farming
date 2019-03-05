$Stackable_Potato_StackedItem0 = "PotatoBasket0Item 7";
$Stackable_Potato_StackedItem1 = "PotatoBasket1Item 15";
$Stackable_Potato_StackedItemTotal = 2;

datablock ItemData(PotatoItem : HammerItem)
{
	shapeFile = "./potato.dts";
	uiName = "Potato";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Potato";
};

datablock ItemData(PotatoBasket0Item : HammerItem)
{
	shapeFile = "./potatoBasket0.dts";
	uiName = "Potato Basket";
	image = "PotatoBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Potato_Basket";

	isStackable = 1;
	stackType = "Potato";
};

datablock ItemData(PotatoBasket1Item : HammerItem)
{
	shapeFile = "./potatoBasket1.dts";

	uiName = "Full Potato Basket";
	image = "PotatoBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Potato_Star";

	isStackable = 1;
	stackType = "Potato";
};

datablock ShapeBaseImageData(PotatoBasket0Image)
{
	shapeFile = "./potatoBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = PotatoBasket0Item;
	
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

datablock ShapeBaseImageData(PotatoBasket1Image : PotatoBasket0Image) 
{
	shapeFile = "./potatoBasket1.dts";

	item = PotatoBasket1Item;
};

function PotatoBasket0Image::onFire(%this, %obj, %slot)
{

}

function PotatoBasket1Image::onFire(%this, %obj, %slot)
{

}

function PotatoBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function PotatoBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function PotatoBasket0Image::onUnmount(%this, %obj, %slot)
{
	
}

function PotatoBasket1Image::onUnmount(%this, %obj, %slot)
{
	
}

function PotatoBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function PotatoBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}