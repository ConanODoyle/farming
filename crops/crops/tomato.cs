$Stackable_Tomato_StackedItem0 = "TomatoBasket0Item 8";
$Stackable_Tomato_StackedItem1 = "TomatoBasket1Item 15";
$Stackable_Tomato_StackedItemTotal = 2;

datablock ItemData(TomatoItem : HammerItem)
{
	shapeFile = "./tomato.dts";
	uiName = "Tomato";
	image = "";
	doColorShift = false;

	isStackable = 1;
	stackType = "Tomato";
};

datablock ItemData(TomatoBasket0Item : HammerItem)
{
	shapeFile = "./tomatoBasket0.dts";
	uiName = "Tomato Basket";
	image = "TomatoBasket0Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Tomato";
};

datablock ItemData(TomatoBasket1Item : HammerItem)
{
	shapeFile = "./tomatoBasket1.dts";

	uiName = "Full Tomato Basket";
	image = "TomatoBasket1Image";
	doColorShift = false;

	iconName = "";

	isStackable = 1;
	stackType = "Tomato";
};

datablock ShapeBaseImageData(TomatoBasket0Image)
{
	shapeFile = "./tomatoBasket0.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = TomatoBasket0Item;
	
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

datablock ShapeBaseImageData(TomatoBasket1Image : TomatoBasket0Image) 
{
	shapeFile = "./tomatoBasket1.dts";

	item = TomatoBasket1Item;
};

function TomatoBasket0Image::onFire(%this, %obj, %slot)
{

}

function TomatoBasket1Image::onFire(%this, %obj, %slot)
{

}

function TomatoBasket0Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function TomatoBasket1Image::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function TomatoBasket0Image::onUnmount(%this, %obj, %slot)
{

}

function TomatoBasket1Image::onUnmount(%this, %obj, %slot)
{

}

function TomatoBasket0Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}

function TomatoBasket1Image::onLoop(%this, %obj, %slot)
{
	foodLoop(%this, %obj);
}