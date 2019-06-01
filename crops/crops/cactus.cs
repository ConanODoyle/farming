$Stackable_Cactus_StackedItem0 = "CactusBasket0Item 8";
$Stackable_Cactus_StackedItem1 = "CactusBasket1Item 16";
$Stackable_Cactus_StackedItemTotal = 2;

datablock ItemData(CactusItem : HammerItem)
{
	shapeFile = "./Cactus.dts";
	uiName = "Cactus";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Cactus";
};

datablock ItemData(CactusBasket0Item : HammerItem)
{
	shapeFile = "./CactusBasket0.dts";
	uiName = "Cactus Basket";
	image = "CactusBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Cactus_Basket";

	isStackable = 1;
	stackType = "Cactus";
};

datablock ItemData(CactusBasket1Item : HammerItem)
{
	shapeFile = "./CactusBasket1.dts";

	uiName = "Full Cactus Basket";
	image = "CactusBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Cactus_Star";

	isStackable = 1;
	stackType = "Cactus";
};

datablock ShapeBaseImageData(CactusBasket0Image)
{
	shapeFile = "./CactusBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = CactusBasket0Item;
	
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

datablock ShapeBaseImageData(CactusBasket1Image : CactusBasket0Image) 
{
	shapeFile = "./CactusBasket1.dts";

	item = CactusBasket1Item;
};

function CactusBasket0Image::onFire(%this, %obj, %slot)
{

}

function CactusBasket1Image::onFire(%this, %obj, %slot)
{

}

function CactusBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CactusBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CactusBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function CactusBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function CactusBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CactusBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}