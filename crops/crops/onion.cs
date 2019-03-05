$Stackable_Onion_StackedItem0 = "OnionBasket0Item 15";
$Stackable_Onion_StackedItem1 = "OnionBasket1Item 30";
$Stackable_Onion_StackedItem2 = "OnionBasket2Item 45";
$Stackable_Onion_StackedItemTotal = 3;

datablock ItemData(OnionItem : HammerItem)
{
	shapeFile = "./Onion.dts";
	uiName = "Onion";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Onion";
};

datablock ItemData(OnionBasket0Item : HammerItem)
{
	shapeFile = "./OnionBasket0.dts";
	uiName = "Onion Basket";
	image = "OnionBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Onion_Basket";

	isStackable = 1;
	stackType = "Onion";
};

datablock ItemData(OnionBasket1Item : HammerItem)
{
	shapeFile = "./OnionBasket1.dts";

	uiName = "Half Onion Basket";
	image = "OnionBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Onion_Basket_Half";

	isStackable = 1;
	stackType = "Onion";
};

datablock ItemData(OnionBasket2Item : HammerItem)
{
	shapeFile = "./OnionBasket2.dts";

	uiName = "Full Onion Basket";
	image = "OnionBasket2Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/Onion_Star";

	isStackable = 1;
	stackType = "Onion";
};

datablock ShapeBaseImageData(OnionBasket0Image)
{
	shapeFile = "./OnionBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = OnionBasket0Item;

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

datablock ShapeBaseImageData(OnionBasket1Image : OnionBasket0Image) 
{
	shapeFile = "./OnionBasket1.dts";

	item = OnionBasket1Item;
};

datablock ShapeBaseImageData(OnionBasket2Image : OnionBasket0Image) 
{
	shapeFile = "./OnionBasket2.dts";

	item = OnionBasket2Item;
};

function OnionBasket0Image::onFire(%this, %obj, %slot)
{
	
}

function OnionBasket1Image::onFire(%this, %obj, %slot)
{

}

function OnionBasket2Image::onFire(%this, %obj, %slot)
{

}

function OnionBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function OnionBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function OnionBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function OnionBasket0Image::onUnmount(%this, %obj, %slot)
{
	
}

function OnionBasket1Image::onUnmount(%this, %obj, %slot)
{
	
}

function OnionBasket2Image::onUnmount(%this, %obj, %slot)
{
	
}

function OnionBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function OnionBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function OnionBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}