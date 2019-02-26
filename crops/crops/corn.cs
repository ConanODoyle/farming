$Stackable_Corn_StackedItem0 = "CornBasket0Item 5";
$Stackable_Corn_StackedItem1 = "CornBasket1Item 10";
$Stackable_Corn_StackedItem2 = "CornBasket2Item 15";
$Stackable_Corn_StackedItemTotal = 3;

datablock ItemData(CornItem : HammerItem)
{
	shapeFile = "./Corn.dts";
	uiName = "Corn";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Corn";
};

datablock ItemData(CornBasket0Item : HammerItem)
{
	shapeFile = "./CornBasket0.dts";
	uiName = "Corn Basket";
	image = "CornBasket0Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Corn";
};

datablock ItemData(CornBasket1Item : HammerItem)
{
	shapeFile = "./CornBasket1.dts";

	uiName = "Half Corn Basket";
	image = "CornBasket1Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Corn";
};

datablock ItemData(CornBasket2Item : HammerItem)
{
	shapeFile = "./CornBasket2.dts";

	uiName = "Full Corn Basket";
	image = "CornBasket2Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Corn";
};

datablock ShapeBaseImageData(CornBasket0Image)
{
	shapeFile = "./CornBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = CornBasket0Item;

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

datablock ShapeBaseImageData(CornBasket1Image : CornBasket0Image) 
{
	shapeFile = "./CornBasket1.dts";

	item = CornBasket1Item;
};

datablock ShapeBaseImageData(CornBasket2Image : CornBasket0Image) 
{
	shapeFile = "./CornBasket2.dts";

	item = CornBasket2Item;
};

function CornBasket0Image::onFire(%this, %obj, %slot)
{
	
}

function CornBasket1Image::onFire(%this, %obj, %slot)
{

}

function CornBasket2Image::onFire(%this, %obj, %slot)
{

}

function CornBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CornBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CornBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CornBasket0Image::onUnmount(%this, %obj, %slot)
{
	
}

function CornBasket1Image::onUnmount(%this, %obj, %slot)
{
	
}

function CornBasket2Image::onUnmount(%this, %obj, %slot)
{
	
}

function CornBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CornBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CornBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}