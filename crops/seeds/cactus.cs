$Stackable_CactusSeed_StackedItem0 = "CactusSeed0Item 2";
$Stackable_CactusSeed_StackedItem1 = "CactusSeed1Item 4";
$Stackable_CactusSeed_StackedItem2 = "CactusSeed2Item 6";
$Stackable_CactusSeed_StackedItem3 = "CactusSeed3Item 8";
$Stackable_CactusSeed_StackedItemTotal = 4;

datablock ItemData(CactusSeedItem : HammerItem)
{
	shapeFile = "./seedsRound.dts";
	uiName = "Cactus Seed";
	image = "CactusSeed0Image";
	colorShiftColor = "0.08 0.08 0.08 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CactusSeed";
};

datablock ItemData(CactusSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Cactus Seed1";
	image = "CactusSeed0Image";
	colorShiftColor = "0.9 0.9 0.9 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CactusSeed";
};

datablock ItemData(CactusSeed1Item : CactusSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Cactus Seed2";
	image = "CactusSeed1Image";
};

datablock ItemData(CactusSeed2Item : CactusSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Cactus Seed3";
	image = "CactusSeed2Image";
};

datablock ItemData(CactusSeed3Item : CactusSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Cactus Seed4";
	image = "CactusSeed3Image";
};

datablock ShapeBaseImageData(CactusSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = CactusSeed0Item.colorShiftColor;

	item = CactusSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickCactus0CropData";
	cropType = "Cactus";

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

datablock ShapeBaseImageData(CactusSeed1Image : CactusSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = CactusSeed1Item;
};

datablock ShapeBaseImageData(CactusSeed2Image : CactusSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = CactusSeed2Item;
};

datablock ShapeBaseImageData(CactusSeed3Image : CactusSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = CactusSeed3Item;
};



function CactusSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CactusSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CactusSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CactusSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function CactusSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CactusSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CactusSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CactusSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}