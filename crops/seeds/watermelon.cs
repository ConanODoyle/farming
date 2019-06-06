$Stackable_WatermelonSeed_StackedItem0 = "WatermelonSeed0Item 2";
$Stackable_WatermelonSeed_StackedItem1 = "WatermelonSeed1Item 4";
$Stackable_WatermelonSeed_StackedItem2 = "WatermelonSeed2Item 6";
$Stackable_WatermelonSeed_StackedItem3 = "WatermelonSeed3Item 8";
$Stackable_WatermelonSeed_StackedItemTotal = 4;

datablock ItemData(WatermelonSeedItem : HammerItem)
{
	shapeFile = "./seedsRound.dts";
	uiName = "Watermelon Seed";
	image = "WatermelonSeed0Image";
	colorShiftColor = "0.08 0.08 0.08 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "WatermelonSeed";
};

datablock ItemData(WatermelonSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Watermelon Seed1";
	image = "WatermelonSeed0Image";
	colorShiftColor = "0.9 0.9 0.9 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "WatermelonSeed";
};

datablock ItemData(WatermelonSeed1Item : WatermelonSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Watermelon Seed2";
	image = "WatermelonSeed1Image";
};

datablock ItemData(WatermelonSeed2Item : WatermelonSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Watermelon Seed3";
	image = "WatermelonSeed2Image";
};

datablock ItemData(WatermelonSeed3Item : WatermelonSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Watermelon Seed4";
	image = "WatermelonSeed3Image";
};

datablock ShapeBaseImageData(WatermelonSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = WatermelonSeed0Item.colorShiftColor;

	item = WatermelonSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickWatermelon0CropData";
	cropType = "Watermelon";

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

datablock ShapeBaseImageData(WatermelonSeed1Image : WatermelonSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = WatermelonSeed1Item;
};

datablock ShapeBaseImageData(WatermelonSeed2Image : WatermelonSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = WatermelonSeed2Item;
};

datablock ShapeBaseImageData(WatermelonSeed3Image : WatermelonSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = WatermelonSeed3Item;
};



function WatermelonSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function WatermelonSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function WatermelonSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function WatermelonSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function WatermelonSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function WatermelonSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function WatermelonSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function WatermelonSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}