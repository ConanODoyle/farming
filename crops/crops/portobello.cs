$Stackable_Portobello_StackedItem0 = "PortobelloBasket0Item 1";
$Stackable_Portobello_StackedItem1 = "PortobelloBasket1Item 2";
$Stackable_Portobello_StackedItem2 = "PortobelloBasket2Item 3";
$Stackable_Portobello_StackedItemTotal = 3;

datablock ItemData(PortobelloItem : HammerItem)
{
	shapeFile = "./Portobello.dts";
	uiName = "Portobello";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Portobello";
};

datablock ItemData(PortobelloBasket0Item : HammerItem)
{
	shapeFile = "./PortobelloBasket0.dts";
	uiName = "One-Third Portobello Basket";
	image = "PortobelloBasket0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Portobello_Basket_Quarter";

	isStackable = 1;
	stackType = "Portobello";
};

datablock ItemData(PortobelloBasket1Item : HammerItem)
{
	shapeFile = "./PortobelloBasket1.dts";

	uiName = "Two-Thirds Portobello Basket";
	image = "PortobelloBasket1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Portobello_Basket_Half";

	isStackable = 1;
	stackType = "Portobello";
};

datablock ItemData(PortobelloBasket2Item : HammerItem)
{
	shapeFile = "./PortobelloBasket2.dts";

	uiName = "Full Portobello Basket";
	image = "PortobelloBasket2Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/Portobello_Star";

	isStackable = 1;
	stackType = "Portobello";
};

datablock ShapeBaseImageData(PortobelloBasket0Image)
{
	shapeFile = "./PortobelloBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = PortobelloBasket0Item;
	
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

datablock ShapeBaseImageData(PortobelloBasket1Image : PortobelloBasket0Image) 
{
	shapeFile = "./PortobelloBasket1.dts";

	item = PortobelloBasket1Item;
};

datablock ShapeBaseImageData(PortobelloBasket2Image : PortobelloBasket0Image) 
{
	shapeFile = "./PortobelloBasket2.dts";

	item = PortobelloBasket2Item;
};

function PortobelloBasket0Image::onFire(%this, %obj, %slot)
{

}

function PortobelloBasket1Image::onFire(%this, %obj, %slot)
{

}

function PortobelloBasket2Image::onFire(%this, %obj, %slot)
{

}

function PortobelloBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function PortobelloBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function PortobelloBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function PortobelloBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function PortobelloBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function PortobelloBasket2Image::onUnmount(%this, %obj, %slot)
{

}

function PortobelloBasket3Image::onUnmount(%this, %obj, %slot)
{

}

function PortobelloBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function PortobelloBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function PortobelloBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}