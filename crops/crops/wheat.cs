$Stackable_Wheat_StackedItem0 = "WheatBasket0Item 4";
$Stackable_Wheat_StackedItem1 = "WheatBasket1Item 8";
$Stackable_Wheat_StackedItem2 = "WheatBasket2Item 12";
$Stackable_Wheat_StackedItemTotal = 3;

datablock ItemData(WheatItem : HammerItem)
{
	shapeFile = "./Wheat.dts";
	uiName = "Wheat";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Wheat";
};

datablock ItemData(WheatBasket0Item : HammerItem)
{
	shapeFile = "./WheatBasket0.dts";
	uiName = "Wheat Basket";
	image = "WheatBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Wheat_Basket";

	isStackable = 1;
	stackType = "Wheat";
};

datablock ItemData(WheatBasket1Item : HammerItem)
{
	shapeFile = "./WheatBasket1.dts";

	uiName = "Half Wheat Basket";
	image = "WheatBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Wheat_Basket_Half";

	isStackable = 1;
	stackType = "Wheat";
};

datablock ItemData(WheatBasket2Item : HammerItem)
{
	shapeFile = "./WheatBasket2.dts";

	uiName = "Full Wheat Basket";
	image = "WheatBasket2Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Wheat_Star";

	isStackable = 1;
	stackType = "Wheat";
};

datablock ShapeBaseImageData(WheatBasket0Image)
{
	shapeFile = "./WheatBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = WheatBasket0Item;

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

datablock ShapeBaseImageData(WheatBasket1Image : WheatBasket0Image) 
{
	shapeFile = "./WheatBasket1.dts";

	item = WheatBasket1Item;
};

datablock ShapeBaseImageData(WheatBasket2Image : WheatBasket0Image) 
{
	shapeFile = "./WheatBasket2.dts";

	item = WheatBasket2Item;
};

function WheatBasket0Image::onFire(%this, %obj, %slot)
{
	
}

function WheatBasket1Image::onFire(%this, %obj, %slot)
{

}

function WheatBasket2Image::onFire(%this, %obj, %slot)
{

}

function WheatBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function WheatBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function WheatBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function WheatBasket0Image::onUnmount(%this, %obj, %slot)
{
	
}

function WheatBasket1Image::onUnmount(%this, %obj, %slot)
{
	
}

function WheatBasket2Image::onUnmount(%this, %obj, %slot)
{
	
}

function WheatBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function WheatBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function WheatBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}