$Stackable_Weed_StackedItem0 = "Weed0Item 2";
$Stackable_Weed_StackedItem1 = "Weed1Item 6";
$Stackable_Weed_StackedItemTotal = 2;

datablock ItemData(WeedItem : HammerItem)
{
	shapeFile = "./Weed.dts";
	uiName = "Weed";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Weed";
};

datablock ItemData(Weed0Item : HammerItem)
{
	shapeFile = "./Weed0.dts";
	uiName = "Weed ";
	image = "Weed0Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/WeedLmao";

	isStackable = 1;
	stackType = "Weed";
};

datablock ItemData(Weed1Item : HammerItem)
{
	shapeFile = "./Weed1.dts";

	uiName = "Full Weed ";
	image = "Weed1Image";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/crops/icons/DudeWeedLmao";

	isStackable = 1;
	stackType = "Weed";
};

datablock ShapeBaseImageData(Weed0Image)
{
	shapeFile = "./Weed0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = Weed0Item;
	
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

datablock ShapeBaseImageData(Weed1Image : Weed0Image) 
{
	shapeFile = "./Weed1.dts";

	item = Weed1Item;
};

function Weed0Image::onFire(%this, %obj, %slot)
{

}

function Weed1Image::onFire(%this, %obj, %slot)
{

}

function Weed0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyRight");
}

function Weed1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyRight");
}

function Weed0Image::onUnmount(%this, %obj, %slot)
{

}

function Weed1Image::onUnmount(%this, %obj, %slot)
{

}

function Weed0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function Weed1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}
