$Stackable_LilySeed_StackedItem0 = "LilySeed0Item 6";
$Stackable_LilySeed_StackedItem1 = "LilySeed1Item 12";
$Stackable_LilySeed_StackedItem2 = "LilySeed2Item 18";
$Stackable_LilySeed_StackedItem3 = "LilySeed3Item 24";
$Stackable_LilySeed_StackedItemTotal = 4;

datablock ItemData(LilySeedItem : HammerItem)
{
	shapeFile = "./seeds.dts";
	uiName = "Lily Seed";
	image = "LilySeed0Image";
	colorShiftColor = "0.9 0.68 0.12 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "LilySeed";
};

datablock ItemData(LilySeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Lily Seed1";
	image = "LilySeed0Image";
	colorShiftColor = "0.568 0.329 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "LilySeed";
};

datablock ItemData(LilySeed1Item : LilySeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Lily Seed2";
	image = "LilySeed1Image";
};

datablock ItemData(LilySeed2Item : LilySeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Lily Seed3";
	image = "LilySeed2Image";
};

datablock ItemData(LilySeed3Item : LilySeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Lily Seed4";
	image = "LilySeed3Image";
};

datablock ShapeBaseImageData(LilySeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = LilySeed0Item.colorShiftColor;

	item = LilySeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickLily0CropData";
	cropType = "Lily";

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

datablock ShapeBaseImageData(LilySeed1Image : LilySeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = LilySeed1Item;
};

datablock ShapeBaseImageData(LilySeed2Image : LilySeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = LilySeed2Item;
};

datablock ShapeBaseImageData(LilySeed3Image : LilySeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = LilySeed3Item;
};



function LilySeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function LilySeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function LilySeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function LilySeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function LilySeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function LilySeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function LilySeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function LilySeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}