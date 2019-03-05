$Stackable_Carrot_StackedItem0 = "CarrotBasket0Item 5";
$Stackable_Carrot_StackedItem1 = "CarrotBasket1Item 10";
$Stackable_Carrot_StackedItem2 = "CarrotBasket2Item 15";
$Stackable_Carrot_StackedItemTotal = 3;

datablock ItemData(CarrotItem : HammerItem)
{
	shapeFile = "./carrot.dts";
	uiName = "Carrot";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Carrot";
};

datablock ItemData(CarrotBasket0Item : HammerItem)
{
	shapeFile = "./carrotBasket0.dts";
	uiName = "Carrot Basket";
	image = "CarrotBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Carrot_Basket";

	isStackable = 1;
	stackType = "Carrot";
};

datablock ItemData(CarrotBasket1Item : HammerItem)
{
	shapeFile = "./carrotBasket1.dts";

	uiName = "Half Carrot Basket";
	image = "CarrotBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Carrot_Basket_Half";

	isStackable = 1;
	stackType = "Carrot";
};

datablock ItemData(CarrotBasket2Item : HammerItem)
{
	shapeFile = "./carrotBasket2.dts";

	uiName = "Full Carrot Basket";
	image = "CarrotBasket2Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Carrot_Star";

	isStackable = 1;
	stackType = "Carrot";
};

datablock ShapeBaseImageData(CarrotBasket0Image)
{
	shapeFile = "./carrotBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = CarrotBasket0Item;

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

datablock ShapeBaseImageData(CarrotBasket1Image : CarrotBasket0Image) 
{
	shapeFile = "./carrotBasket1.dts";

	item = CarrotBasket1Item;
};

datablock ShapeBaseImageData(CarrotBasket2Image : CarrotBasket0Image) 
{
	shapeFile = "./carrotBasket2.dts";

	item = CarrotBasket2Item;
};

function CarrotBasket0Image::onFire(%this, %obj, %slot)
{
	
}

function CarrotBasket1Image::onFire(%this, %obj, %slot)
{

}

function CarrotBasket2Image::onFire(%this, %obj, %slot)
{

}

function CarrotBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CarrotBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CarrotBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CarrotBasket0Image::onUnmount(%this, %obj, %slot)
{
	
}

function CarrotBasket1Image::onUnmount(%this, %obj, %slot)
{
	
}

function CarrotBasket2Image::onUnmount(%this, %obj, %slot)
{
	
}

function CarrotBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CarrotBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function CarrotBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}