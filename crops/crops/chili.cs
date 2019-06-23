$Stackable_Chili_StackedItem0 = "ChiliBasket0Item 7";
$Stackable_Chili_StackedItem1 = "ChiliBasket1Item 14";
$Stackable_Chili_StackedItem2 = "ChiliBasket2Item 21";
$Stackable_Chili_StackedItem3 = "ChiliBasket3Item 28";
$Stackable_Chili_StackedItemTotal = 4;

datablock ItemData(ChiliItem : HammerItem)
{
	shapeFile = "./Chili.dts";
	uiName = "Chili";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Chili";
};

datablock ItemData(ChiliBasket0Item : HammerItem)
{
	shapeFile = "./ChiliBasket0.dts";
	uiName = "Quarter Chili Basket";
	image = "ChiliBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Chili_Basket_1";

	isStackable = 1;
	stackType = "Chili";
};

datablock ItemData(ChiliBasket1Item : HammerItem)
{
	shapeFile = "./ChiliBasket1.dts";

	uiName = "Half Chili Basket";
	image = "ChiliBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Chili_Basket_2";

	isStackable = 1;
	stackType = "Chili";
};

datablock ItemData(ChiliBasket2Item : HammerItem)
{
	shapeFile = "./ChiliBasket2.dts";

	uiName = "Three Quarter Chili Basket";
	image = "ChiliBasket2Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Chili_Basket_3";

	isStackable = 1;
	stackType = "Chili";
};

datablock ItemData(ChiliBasket3Item : HammerItem)
{
	shapeFile = "./ChiliBasket3.dts";

	uiName = "Full Chili Basket";
	image = "ChiliBasket3Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Chili_Star";

	isStackable = 1;
	stackType = "Chili";
};

datablock ShapeBaseImageData(ChiliBasket0Image)
{
	shapeFile = "./ChiliBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = ChiliBasket0Item;
	
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

datablock ShapeBaseImageData(ChiliBasket1Image : ChiliBasket0Image) 
{
	shapeFile = "./ChiliBasket1.dts";

	item = ChiliBasket1Item;
};

datablock ShapeBaseImageData(ChiliBasket2Image : ChiliBasket0Image) 
{
	shapeFile = "./ChiliBasket2.dts";

	item = ChiliBasket2Item;
};

datablock ShapeBaseImageData(ChiliBasket3Image : ChiliBasket0Image) 
{
	shapeFile = "./ChiliBasket3.dts";

	item = ChiliBasket3Item;
};

function ChiliBasket0Image::onFire(%this, %obj, %slot)
{

}

function ChiliBasket1Image::onFire(%this, %obj, %slot)
{

}

function ChiliBasket2Image::onFire(%this, %obj, %slot)
{

}

function ChiliBasket3Image::onFire(%this, %obj, %slot)
{

}

function ChiliBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function ChiliBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function ChiliBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function ChiliBasket3Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function ChiliBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function ChiliBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function ChiliBasket2Image::onUnmount(%this, %obj, %slot)
{

}

function ChiliBasket3Image::onUnmount(%this, %obj, %slot)
{

}

function ChiliBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function ChiliBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function ChiliBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function ChiliBasket3Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}