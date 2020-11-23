$Stackable_Peach_StackedItem0 = "PeachBasket0Item 8";
$Stackable_Peach_StackedItem1 = "PeachBasket1Item 16";
$Stackable_Peach_StackedItemTotal = 2;

datablock ItemData(PeachItem : HammerItem)
{
	shapeFile = "./Peach.dts";
	uiName = "Peach";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Peach";
};

datablock ItemData(PeachBasket0Item : HammerItem)
{
	shapeFile = "./PeachBasket0.dts";
	uiName = "Peach Basket";
	image = "PeachBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Peach_Basket";

	isStackable = 1;
	stackType = "Peach";
};

datablock ItemData(PeachBasket1Item : HammerItem)
{
	shapeFile = "./PeachBasket1.dts";

	uiName = "Full Peach Basket";
	image = "PeachBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Peach_Star";

	isStackable = 1;
	stackType = "Peach";
};

datablock ShapeBaseImageData(PeachBasket0Image)
{
	shapeFile = "./PeachBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = PeachBasket0Item;
	
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

datablock ShapeBaseImageData(PeachBasket1Image : PeachBasket0Image) 
{
	shapeFile = "./PeachBasket1.dts";

	item = PeachBasket1Item;
};

function PeachBasket0Image::onFire(%this, %obj, %slot)
{

}

function PeachBasket1Image::onFire(%this, %obj, %slot)
{

}

function PeachBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function PeachBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function PeachBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function PeachBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function PeachBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function PeachBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}