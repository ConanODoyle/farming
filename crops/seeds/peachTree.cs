$Stackable_PeachSeed_StackedItem0 = "PeachSeed0Item 6";
$Stackable_PeachSeed_StackedItem1 = "PeachSeed1Item 12";
$Stackable_PeachSeed_StackedItem2 = "PeachSeed2Item 18";
$Stackable_PeachSeed_StackedItem3 = "PeachSeed3Item 24";
$Stackable_PeachSeed_StackedItemTotal = 4;

datablock ItemData(PeachSeedItem : HammerItem)
{
	shapeFile = "./seedsRound.dts";
	uiName = "Peach Seed";
	image = "PeachSeed0Image";
	colorShiftColor = "0.08 0.08 0.08 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "PeachSeed";
};

datablock ItemData(PeachSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Peach Seed1";
	image = "PeachSeed0Image";
	colorShiftColor = "0.9 0.9 0.9 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "PeachSeed";
};

datablock ItemData(PeachSeed1Item : PeachSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Peach Seed2";
	image = "PeachSeed1Image";
};

datablock ItemData(PeachSeed2Item : PeachSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Peach Seed3";
	image = "PeachSeed2Image";
};

datablock ItemData(PeachSeed3Item : PeachSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Peach Seed4";
	image = "PeachSeed3Image";
};

datablock ShapeBaseImageData(PeachSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = PeachSeed0Item.colorShiftColor;

	item = PeachSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickPeachTree0CropData";
	cropType = "Peach";

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

datablock ShapeBaseImageData(PeachSeed1Image : PeachSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = PeachSeed1Item;
};

datablock ShapeBaseImageData(PeachSeed2Image : PeachSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = PeachSeed2Item;
};

datablock ShapeBaseImageData(PeachSeed3Image : PeachSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = PeachSeed3Item;
};



function PeachSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PeachSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PeachSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PeachSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function PeachSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PeachSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PeachSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PeachSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}