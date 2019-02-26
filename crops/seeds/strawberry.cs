$Stackable_StrawberrySeed_StackedItem0 = "StrawberrySeed0Item 3";
$Stackable_StrawberrySeed_StackedItem1 = "StrawberrySeed1Item 6";
$Stackable_StrawberrySeed_StackedItem2 = "StrawberrySeed2Item 9";
$Stackable_StrawberrySeed_StackedItem3 = "StrawberrySeed3Item 12";
$Stackable_StrawberrySeed_StackedItemTotal = 4;

datablock ItemData(StrawberrySeedItem : HammerItem)
{
	shapeFile = "./seedsFlat.dts";
	uiName = "Strawberry Seed";
	image = "StrawberrySeed0Image";
	colorShiftColor = "0.71 0.8 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "StrawberrySeed";
};

datablock ItemData(StrawberrySeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Strawberry Seed1";
	image = "StrawberrySeed0Image";
	colorShiftColor = "1 0 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "StrawberrySeed";
};

datablock ItemData(StrawberrySeed1Item : StrawberrySeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Strawberry Seed2";
	image = "StrawberrySeed1Image";
};

datablock ItemData(StrawberrySeed2Item : StrawberrySeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Strawberry Seed3";
	image = "StrawberrySeed2Image";
};

datablock ItemData(StrawberrySeed3Item : StrawberrySeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Strawberry Seed4";
	image = "StrawberrySeed3Image";
};

datablock ShapeBaseImageData(StrawberrySeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = StrawberrySeed0Item.colorShiftColor;

	item = StrawberrySeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickStrawberry0CropData";
	cropType = "Strawberry";

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

datablock ShapeBaseImageData(StrawberrySeed1Image : StrawberrySeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = StrawberrySeed1Item;
};

datablock ShapeBaseImageData(StrawberrySeed2Image : StrawberrySeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = StrawberrySeed2Item;
};

datablock ShapeBaseImageData(StrawberrySeed3Image : StrawberrySeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = StrawberrySeed3Item;
};



function StrawberrySeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function StrawberrySeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function StrawberrySeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function StrawberrySeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function StrawberrySeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function StrawberrySeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function StrawberrySeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function StrawberrySeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}