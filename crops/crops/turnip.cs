$Stackable_Turnip_StackedItem0 = "TurnipBasket0Item 1";
$Stackable_Turnip_StackedItem1 = "TurnipBasket1Item 2";
$Stackable_Turnip_StackedItem2 = "TurnipBasket2Item 3";
$Stackable_Turnip_StackedItem3 = "TurnipBasket3Item 4";
$Stackable_Turnip_StackedItemTotal = 4;

datablock ItemData(TurnipItem : HammerItem)
{
	shapeFile = "./Turnip.dts";
	uiName = "Turnip";
	image = "";
	doColorShift = false;

	alt = TurnipFaceItem;

	isStackable = 1;
	stackType = "Turnip";
};

datablock ItemData(TurnipFaceItem : TurnipItem)
{
	shapeFile = "./TurnipFace.dts";
	uiName = "";
};

datablock ItemData(TurnipBasket0Item : HammerItem)
{
	shapeFile = "./TurnipBasket0.dts";
	uiName = "Quarter Turnip Basket";
	image = "TurnipBasket0Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Turnip";
};

datablock ItemData(TurnipBasket1Item : HammerItem)
{
	shapeFile = "./TurnipBasket1.dts";

	uiName = "Half Turnip Basket";
	image = "TurnipBasket1Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Turnip";
};

datablock ItemData(TurnipBasket2Item : HammerItem)
{
	shapeFile = "./TurnipBasket2.dts";

	uiName = "Three Quarter Turnip Basket";
	image = "TurnipBasket2Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Turnip";
};

datablock ItemData(TurnipBasket3Item : HammerItem)
{
	shapeFile = "./TurnipBasket3.dts";

	uiName = "Full Turnip Basket";
	image = "TurnipBasket3Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Turnip";
};

datablock ShapeBaseImageData(TurnipBasket0Image)
{
	shapeFile = "./TurnipBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = TurnipBasket0Item;
	
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

datablock ShapeBaseImageData(TurnipBasket1Image : TurnipBasket0Image) 
{
	shapeFile = "./TurnipBasket1.dts";

	item = TurnipBasket1Item;
};

datablock ShapeBaseImageData(TurnipBasket2Image : TurnipBasket0Image) 
{
	shapeFile = "./TurnipBasket2.dts";

	item = TurnipBasket2Item;
};

datablock ShapeBaseImageData(TurnipBasket3Image : TurnipBasket0Image) 
{
	shapeFile = "./TurnipBasket3.dts";

	item = TurnipBasket3Item;
};

function TurnipBasket0Image::onFire(%this, %obj, %slot)
{

}

function TurnipBasket1Image::onFire(%this, %obj, %slot)
{

}

function TurnipBasket2Image::onFire(%this, %obj, %slot)
{

}

function TurnipBasket3Image::onFire(%this, %obj, %slot)
{

}

function TurnipBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function TurnipBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function TurnipBasket2Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function TurnipBasket3Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function TurnipBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function TurnipBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function TurnipBasket2Image::onUnmount(%this, %obj, %slot)
{

}

function TurnipBasket3Image::onUnmount(%this, %obj, %slot)
{

}

function TurnipBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function TurnipBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function TurnipBasket2Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function TurnipBasket3Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}