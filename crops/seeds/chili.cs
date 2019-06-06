$Stackable_ChiliSeed_StackedItem0 = "ChiliSeed0Item 4";
$Stackable_ChiliSeed_StackedItem1 = "ChiliSeed1Item 8";
$Stackable_ChiliSeed_StackedItem2 = "ChiliSeed2Item 12";
$Stackable_ChiliSeed_StackedItem3 = "ChiliSeed3Item 16";
$Stackable_ChiliSeed_StackedItemTotal = 4;

datablock ItemData(ChiliSeedItem : HammerItem)
{
	shapeFile = "./seedsRound.dts";
	uiName = "Chili Seed";
	image = "ChiliSeed0Image";
	colorShiftColor = "0.08 0.08 0.08 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "ChiliSeed";
};

datablock ItemData(ChiliSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Chili Seed1";
	image = "ChiliSeed0Image";
	colorShiftColor = "0.9 0.9 0.9 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "ChiliSeed";
};

datablock ItemData(ChiliSeed1Item : ChiliSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Chili Seed2";
	image = "ChiliSeed1Image";
};

datablock ItemData(ChiliSeed2Item : ChiliSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Chili Seed3";
	image = "ChiliSeed2Image";
};

datablock ItemData(ChiliSeed3Item : ChiliSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Chili Seed4";
	image = "ChiliSeed3Image";
};

datablock ShapeBaseImageData(ChiliSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = ChiliSeed0Item.colorShiftColor;

	item = ChiliSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickChili0CropData";
	cropType = "Chili";

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

datablock ShapeBaseImageData(ChiliSeed1Image : ChiliSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = ChiliSeed1Item;
};

datablock ShapeBaseImageData(ChiliSeed2Image : ChiliSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = ChiliSeed2Item;
};

datablock ShapeBaseImageData(ChiliSeed3Image : ChiliSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = ChiliSeed3Item;
};



function ChiliSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function ChiliSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function ChiliSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function ChiliSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function ChiliSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function ChiliSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function ChiliSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function ChiliSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}