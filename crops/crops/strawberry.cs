$Stackable_Strawberry_StackedItem0 = "StrawberryBasket0Item 8";
$Stackable_Strawberry_StackedItem1 = "StrawberryBasket1Item 15";
$Stackable_Strawberry_StackedItemTotal = 2;

datablock ItemData(StrawberryItem : HammerItem)
{
	shapeFile = "./Strawberry.dts";
	uiName = "Strawberry";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Strawberry";
};

datablock ItemData(StrawberryBasket0Item : HammerItem)
{
	shapeFile = "./StrawberryBasket0.dts";
	uiName = "Strawberry Basket";
	image = "StrawberryBasket0Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Strawberry";
};

datablock ItemData(StrawberryBasket1Item : HammerItem)
{
	shapeFile = "./StrawberryBasket1.dts";

	uiName = "Full Strawberry Basket";
	image = "StrawberryBasket1Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Strawberry";
};

datablock ShapeBaseImageData(StrawberryBasket0Image)
{
	shapeFile = "./StrawberryBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = StrawberryBasket0Item;
	
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

datablock ShapeBaseImageData(StrawberryBasket1Image : StrawberryBasket0Image) 
{
	shapeFile = "./StrawberryBasket1.dts";

	item = StrawberryBasket1Item;
};

function StrawberryBasket0Image::onFire(%this, %obj, %slot)
{

}

function StrawberryBasket1Image::onFire(%this, %obj, %slot)
{

}

function StrawberryBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function StrawberryBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function StrawberryBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function StrawberryBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function StrawberryBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function StrawberryBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}