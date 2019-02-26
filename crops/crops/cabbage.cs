$Stackable_Cabbage_StackedItem0 = "CabbageBasket0Item 7";
$Stackable_Cabbage_StackedItem1 = "CabbageBasket1Item 15";
$Stackable_Cabbage_StackedItemTotal = 2;

datablock ItemData(CabbageItem : HammerItem)
{
	shapeFile = "./Cabbage.dts";
	uiName = "Cabbage";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Cabbage";
};

datablock ItemData(CabbageBasket0Item : HammerItem)
{
	shapeFile = "./CabbageBasket0.dts";
	uiName = "Cabbage Basket";
	image = "CabbageBasket0Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Cabbage";
};

datablock ItemData(CabbageBasket1Item : HammerItem)
{
	shapeFile = "./CabbageBasket1.dts";

	uiName = "Full Cabbage Basket";
	image = "CabbageBasket1Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Cabbage";
};

datablock ShapeBaseImageData(CabbageBasket0Image)
{
	shapeFile = "./CabbageBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = CabbageBasket0Item;
	
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

datablock ShapeBaseImageData(CabbageBasket1Image : CabbageBasket0Image) 
{
	shapeFile = "./CabbageBasket1.dts";

	item = CabbageBasket1Item;
};

function CabbageBasket0Image::onFire(%this, %obj, %slot)
{

}

function CabbageBasket1Image::onFire(%this, %obj, %slot)
{

}

function CabbageBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CabbageBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CabbageBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function CabbageBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function CabbageBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CabbageBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}