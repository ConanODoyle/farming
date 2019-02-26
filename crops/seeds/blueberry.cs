$Stackable_BlueberrySeed_StackedItem0 = "BlueberrySeed0Item 3";
$Stackable_BlueberrySeed_StackedItem1 = "BlueberrySeed1Item 6";
$Stackable_BlueberrySeed_StackedItem2 = "BlueberrySeed2Item 9";
$Stackable_BlueberrySeed_StackedItem3 = "BlueberrySeed3Item 12";
$Stackable_BlueberrySeed_StackedItemTotal = 4;

datablock ItemData(BlueberrySeedItem : HammerItem)
{
	shapeFile = "./seedsround.dts";
	uiName = "Blueberry Seed";
	image = "BlueberrySeed0Image";
	colorShiftColor = "0.71 0.4 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "BlueberrySeed";
};

datablock ItemData(BlueberrySeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Blueberry Seed1";
	image = "BlueberrySeed0Image";
	colorShiftColor = "0.24 0.12 0.487 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "BlueberrySeed";
};

datablock ItemData(BlueberrySeed1Item : BlueberrySeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Blueberry Seed2";
	image = "BlueberrySeed1Image";
};

datablock ItemData(BlueberrySeed2Item : BlueberrySeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Blueberry Seed3";
	image = "BlueberrySeed2Image";
};

datablock ItemData(BlueberrySeed3Item : BlueberrySeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Blueberry Seed4";
	image = "BlueberrySeed3Image";
};

datablock ShapeBaseImageData(BlueberrySeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = BlueberrySeed0Item.colorShiftColor;

	item = BlueberrySeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickBlueberry0CropData";
	cropType = "Blueberry";

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

datablock ShapeBaseImageData(BlueberrySeed1Image : BlueberrySeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = BlueberrySeed1Item;
};

datablock ShapeBaseImageData(BlueberrySeed2Image : BlueberrySeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = BlueberrySeed2Item;
};

datablock ShapeBaseImageData(BlueberrySeed3Image : BlueberrySeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = BlueberrySeed3Item;
};



function BlueberrySeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function BlueberrySeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function BlueberrySeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function BlueberrySeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function BlueberrySeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function BlueberrySeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function BlueberrySeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function BlueberrySeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}