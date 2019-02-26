$Stackable_MangoSeed_StackedItem0 = "MangoSeed0Item 1";
$Stackable_MangoSeed_StackedItem1 = "MangoSeed1Item 2";
$Stackable_MangoSeed_StackedItem2 = "MangoSeed2Item 3";
$Stackable_MangoSeed_StackedItem3 = "MangoSeed3Item 4";
$Stackable_MangoSeed_StackedItemTotal = 4;

datablock ItemData(MangoSeedItem : HammerItem)
{
	shapeFile = "./seedsShort.dts";
	uiName = "Mango Seed";
	image = "MangoSeed0Image";
	colorShiftColor = "0.09 0.04 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "MangoSeed";
};

datablock ItemData(MangoSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Mango Seed1";
	image = "MangoSeed0Image";
	colorShiftColor = "1 0.83 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "MangoSeed";
};

datablock ItemData(MangoSeed1Item : MangoSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Mango Seed2";
	image = "MangoSeed1Image";
};

datablock ItemData(MangoSeed2Item : MangoSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Mango Seed3";
	image = "MangoSeed2Image";
};

datablock ItemData(MangoSeed3Item : MangoSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Mango Seed4";
	image = "MangoSeed3Image";
};

datablock ShapeBaseImageData(MangoSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = MangoSeed0Item.colorShiftColor;

	item = MangoSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickMangoTree0CropData";
	cropType = "Mango";

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

datablock ShapeBaseImageData(MangoSeed1Image : MangoSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = MangoSeed1Item;
};

datablock ShapeBaseImageData(MangoSeed2Image : MangoSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = MangoSeed2Item;
};

datablock ShapeBaseImageData(MangoSeed3Image : MangoSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = MangoSeed3Item;
};



function MangoSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function MangoSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function MangoSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function MangoSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function MangoSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function MangoSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function MangoSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function MangoSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}