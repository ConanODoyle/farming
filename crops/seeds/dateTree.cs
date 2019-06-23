$Stackable_DateSeed_StackedItem0 = "DateSeed0Item 1";
$Stackable_DateSeed_StackedItem1 = "DateSeed1Item 2";
$Stackable_DateSeed_StackedItem2 = "DateSeed2Item 3";
$Stackable_DateSeed_StackedItem3 = "DateSeed3Item 4";
$Stackable_DateSeed_StackedItemTotal = 4;

datablock ItemData(DateSeedItem : HammerItem)
{
	shapeFile = "./seeds.dts";
	uiName = "Date Seed";
	image = "DateSeed0Image";
	colorShiftColor = "0.392 0.192 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "DateSeed";
};

datablock ItemData(DateSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Date Seed1";
	image = "DateSeed0Image";
	colorShiftColor = "0.412 0.192 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "DateSeed";
};

datablock ItemData(DateSeed1Item : DateSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Date Seed2";
	image = "DateSeed1Image";
};

datablock ItemData(DateSeed2Item : DateSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Date Seed3";
	image = "DateSeed2Image";
};

datablock ItemData(DateSeed3Item : DateSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Date Seed4";
	image = "DateSeed3Image";
};

datablock ShapeBaseImageData(DateSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = DateSeed0Item.colorShiftColor;

	item = DateSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickDateTree0CropData";
	cropType = "Date";

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

datablock ShapeBaseImageData(DateSeed1Image : DateSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = DateSeed1Item;
};

datablock ShapeBaseImageData(DateSeed2Image : DateSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = DateSeed2Item;
};

datablock ShapeBaseImageData(DateSeed3Image : DateSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = DateSeed3Item;
};



function DateSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function DateSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function DateSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function DateSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function DateSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DateSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DateSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DateSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}