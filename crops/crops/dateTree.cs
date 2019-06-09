$Stackable_Date_StackedItem0 = "DateBasket0Item 3";
$Stackable_Date_StackedItem1 = "DateBasket1Item 6";
$Stackable_Date_StackedItem2 = "DateBasket2Item 9";
$Stackable_Date_StackedItemTotal = 3;

datablock ItemData(DateItem : HammerItem)
{
	shapeFile = "./Date.dts";
	uiName = "Date";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Date";
};

datablock ItemData(DateBasket0Item : HammerItem)
{
	shapeFile = "./DateBasket0.dts";
	uiName = "Date Basket";
	image = "DateBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Date_Basket_1";

	isStackable = 1;
	stackType = "Date";
};

datablock ItemData(DateBasket1Item : HammerItem)
{
	shapeFile = "./DateBasket1.dts";

	uiName = "Half Date Basket";
	image = "DateBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Date_Basket_2";

	isStackable = 1;
	stackType = "Date";
};

datablock ItemData(DateBasket2Item : HammerItem)
{
	shapeFile = "./DateBasket2.dts";

	uiName = "Full Date Basket";
	image = "DateBasket2Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Date_Star";

	isStackable = 1;
	stackType = "Date";
};

datablock ShapeBaseImageData(DateBasket0Image)
{
	shapeFile = "./DateBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = DateBasket0Item;

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

datablock ShapeBaseImageData(DateBasket1Image : DateBasket0Image) 
{
	shapeFile = "./DateBasket1.dts";

	item = DateBasket1Item;
};

datablock ShapeBaseImageData(DateBasket2Image : DateBasket0Image) 
{
	shapeFile = "./DateBasket2.dts";

	item = DateBasket2Item;
};

function DateBasket0Image::onFire(%this, %obj, %slot)
{
	
}

function DateBasket1Image::onFire(%this, %obj, %slot)
{

}

function DateBasket2Image::onFire(%this, %obj, %slot)
{

}

function DateBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function DateBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function DateBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function DateBasket0Image::onUnmount(%this, %obj, %slot)
{
	
}

function DateBasket1Image::onUnmount(%this, %obj, %slot)
{
	
}

function DateBasket2Image::onUnmount(%this, %obj, %slot)
{
	
}

function DateBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function DateBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function DateBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}