$Stackable_WheatSeed_StackedItem0 = "WheatSeed0Item 6";
$Stackable_WheatSeed_StackedItem1 = "WheatSeed1Item 12";
$Stackable_WheatSeed_StackedItem2 = "WheatSeed2Item 18";
$Stackable_WheatSeed_StackedItem3 = "WheatSeed3Item 24";
$Stackable_WheatSeed_StackedItemTotal = 4;

datablock ItemData(WheatSeedItem : HammerItem)
{
	shapeFile = "./seedsShort.dts";
	uiName = "Wheat Seed";
	image = "WheatSeed0Image";
	colorShiftColor = "0.854 0.647 0.122 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "WheatSeed";
};

datablock ItemData(WheatSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Wheat Seed1";
	image = "WheatSeed0Image";
	colorShiftColor = "1 0.96 0.74 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "WheatSeed";
};

datablock ItemData(WheatSeed1Item : WheatSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Wheat Seed2";
	image = "WheatSeed1Image";
};

datablock ItemData(WheatSeed2Item : WheatSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Wheat Seed3";
	image = "WheatSeed2Image";
};

datablock ItemData(WheatSeed3Item : WheatSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Wheat Seed4";
	image = "WheatSeed3Image";
};

datablock ShapeBaseImageData(WheatSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = WheatSeed0Item.colorShiftColor;

	item = WheatSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickWheat0CropData";
	cropType = "Wheat";

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

datablock ShapeBaseImageData(WheatSeed1Image : WheatSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = WheatSeed1Item;
};

datablock ShapeBaseImageData(WheatSeed2Image : WheatSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = WheatSeed2Item;
};

datablock ShapeBaseImageData(WheatSeed3Image : WheatSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = WheatSeed3Item;
};



function WheatSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function WheatSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function WheatSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function WheatSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function WheatSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function WheatSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function WheatSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function WheatSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}