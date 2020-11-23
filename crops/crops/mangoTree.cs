$Stackable_Mango_StackedItem0 = "MangoBasket0Item 5";
$Stackable_Mango_StackedItem1 = "MangoBasket1Item 10";
$Stackable_Mango_StackedItemTotal = 2;

datablock ItemData(MangoItem : HammerItem)
{
	shapeFile = "./Mango.dts";
	uiName = "Mango";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Mango";
};

datablock ItemData(MangoBasket0Item : HammerItem)
{
	shapeFile = "./MangoBasket0.dts";
	uiName = "Mango Basket";
	image = "MangoBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Mango_Basket";

	isStackable = 1;
	stackType = "Mango";
};

datablock ItemData(MangoBasket1Item : HammerItem)
{
	shapeFile = "./MangoBasket1.dts";

	uiName = "Full Mango Basket";
	image = "MangoBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Mango_Star";

	isStackable = 1;
	stackType = "Mango";
};

datablock ShapeBaseImageData(MangoBasket0Image)
{
	shapeFile = "./MangoBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = MangoBasket0Item;
	
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

datablock ShapeBaseImageData(MangoBasket1Image : MangoBasket0Image) 
{
	shapeFile = "./MangoBasket1.dts";

	item = MangoBasket1Item;
};

function MangoBasket0Image::onFire(%this, %obj, %slot)
{

}

function MangoBasket1Image::onFire(%this, %obj, %slot)
{

}

function MangoBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function MangoBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function MangoBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function MangoBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function MangoBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function MangoBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}