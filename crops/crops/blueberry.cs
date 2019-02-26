$Stackable_Blueberry_StackedItem0 = "BlueberryBasket0Item 14";
$Stackable_Blueberry_StackedItem1 = "BlueberryBasket1Item 28";
$Stackable_Blueberry_StackedItem2 = "BlueberryBasket2Item 42";
$Stackable_Blueberry_StackedItemTotal = 3;

datablock ItemData(BlueberryItem : HammerItem)
{
	shapeFile = "./Blueberry.dts";
	uiName = "Blueberry";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Blueberry";
};

datablock ItemData(BlueberryBasket0Item : HammerItem)
{
	shapeFile = "./BlueberryBasket0.dts";
	uiName = "Blueberry Basket";
	image = "BlueberryBasket0Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Blueberry";
};

datablock ItemData(BlueberryBasket1Item : HammerItem)
{
	shapeFile = "./BlueberryBasket1.dts";

	uiName = "Half Blueberry Basket";
	image = "BlueberryBasket1Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Blueberry";
};

datablock ItemData(BlueberryBasket2Item : HammerItem)
{
	shapeFile = "./BlueberryBasket2.dts";

	uiName = "Full Blueberry Basket";
	image = "BlueberryBasket2Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Blueberry";
};

datablock ShapeBaseImageData(BlueberryBasket0Image)
{
	shapeFile = "./BlueberryBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = BlueberryBasket0Item;
	
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

datablock ShapeBaseImageData(BlueberryBasket1Image : BlueberryBasket0Image) 
{
	shapeFile = "./BlueberryBasket1.dts";

	item = BlueberryBasket1Item;
};

datablock ShapeBaseImageData(BlueberryBasket2Image : BlueberryBasket0Image) 
{
	shapeFile = "./BlueberryBasket2.dts";

	item = BlueberryBasket2Item;
};

function BlueberryBasket0Image::onFire(%this, %obj, %slot)
{

}

function BlueberryBasket1Image::onFire(%this, %obj, %slot)
{

}

function BlueberryBasket2Image::onFire(%this, %obj, %slot)
{

}

function BlueberryBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function BlueberryBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function BlueberryBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function BlueberryBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function BlueberryBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function BlueberryBasket2Image::onUnmount(%this, %obj, %slot)
{

}

function BlueberryBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function BlueberryBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function BlueberryBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}