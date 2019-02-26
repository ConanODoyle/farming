$Stackable_CornSeed_StackedItem0 = "CornSeed0Item 3";
$Stackable_CornSeed_StackedItem1 = "CornSeed1Item 6";
$Stackable_CornSeed_StackedItem2 = "CornSeed2Item 9";
$Stackable_CornSeed_StackedItem3 = "CornSeed3Item 12";
$Stackable_CornSeed_StackedItemTotal = 4;

datablock ItemData(CornSeedItem : HammerItem)
{
	shapeFile = "./seedsShort.dts";
	uiName = "Corn Seed";
	image = "CornSeed0Image";
	colorShiftColor = "0.95 0.85 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CornSeed";
};

datablock ItemData(CornSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Corn Seed1";
	image = "CornSeed0Image";
	colorShiftColor = "0.95 0.85 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CornSeed";
};

datablock ItemData(CornSeed1Item : CornSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Corn Seed2";
	image = "CornSeed1Image";
};

datablock ItemData(CornSeed2Item : CornSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Corn Seed3";
	image = "CornSeed2Image";
};

datablock ItemData(CornSeed3Item : CornSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Corn Seed4";
	image = "CornSeed3Image";
};

datablock ShapeBaseImageData(CornSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = CornSeed0Item.colorShiftColor;

	item = CornSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickCorn0CropData";
	cropType = "Corn";

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

datablock ShapeBaseImageData(CornSeed1Image : CornSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = CornSeed1Item;
};

datablock ShapeBaseImageData(CornSeed2Image : CornSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = CornSeed2Item;
};

datablock ShapeBaseImageData(CornSeed3Image : CornSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = CornSeed3Item;
};



function CornSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CornSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CornSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CornSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function CornSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CornSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CornSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CornSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}