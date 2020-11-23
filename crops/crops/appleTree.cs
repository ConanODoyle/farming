$Stackable_Apple_StackedItem0 = "AppleBasket0Item 12";
$Stackable_Apple_StackedItem1 = "AppleBasket1Item 24";
$Stackable_Apple_StackedItemTotal = 2;

datablock ItemData(AppleItem : HammerItem)
{
	shapeFile = "./Apple.dts";
	uiName = "Apple";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Apple";
};

datablock ItemData(AppleBasket0Item : HammerItem)
{
	shapeFile = "./AppleBasket0.dts";
	uiName = "Apple Basket";
	image = "AppleBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Apple_Basket";

	isStackable = 1;
	stackType = "Apple";
};

datablock ItemData(AppleBasket1Item : HammerItem)
{
	shapeFile = "./AppleBasket1.dts";

	uiName = "Full Apple Basket";
	image = "AppleBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Apple_Star";

	isStackable = 1;
	stackType = "Apple";
};

datablock ShapeBaseImageData(AppleBasket0Image)
{
	shapeFile = "./AppleBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = AppleBasket0Item;
	
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

datablock ShapeBaseImageData(AppleBasket1Image : AppleBasket0Image) 
{
	shapeFile = "./AppleBasket1.dts";

	item = AppleBasket1Item;
};

function AppleBasket0Image::onFire(%this, %obj, %slot)
{

}

function AppleBasket1Image::onFire(%this, %obj, %slot)
{

}

function AppleBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function AppleBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function AppleBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function AppleBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function AppleBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function AppleBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}