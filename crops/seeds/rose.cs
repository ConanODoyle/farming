$Stackable_RoseSeed_StackedItem0 = "RoseSeed0Item 6";
$Stackable_RoseSeed_StackedItem1 = "RoseSeed1Item 12";
$Stackable_RoseSeed_StackedItem2 = "RoseSeed2Item 18";
$Stackable_RoseSeed_StackedItem3 = "RoseSeed3Item 24";
$Stackable_RoseSeed_StackedItemTotal = 4;

datablock ItemData(RoseSeedItem : HammerItem)
{
	shapeFile = "./seedsShort.dts";
	uiName = "Rose Seed";
	image = "RoseSeed0Image";
	colorShiftColor = "0.474 0.219 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "RoseSeed";
};

datablock ItemData(RoseSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Rose Seed1";
	image = "RoseSeed0Image";
	colorShiftColor = "0.568 0.06 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "RoseSeed";
};

datablock ItemData(RoseSeed1Item : RoseSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Rose Seed2";
	image = "RoseSeed1Image";
};

datablock ItemData(RoseSeed2Item : RoseSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Rose Seed3";
	image = "RoseSeed2Image";
};

datablock ItemData(RoseSeed3Item : RoseSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Rose Seed4";
	image = "RoseSeed3Image";
};

datablock ShapeBaseImageData(RoseSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = RoseSeed0Item.colorShiftColor;

	item = RoseSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickRose0CropData";
	cropType = "Rose";

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

datablock ShapeBaseImageData(RoseSeed1Image : RoseSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = RoseSeed1Item;
};

datablock ShapeBaseImageData(RoseSeed2Image : RoseSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = RoseSeed2Item;
};

datablock ShapeBaseImageData(RoseSeed3Image : RoseSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = RoseSeed3Item;
};



function RoseSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function RoseSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function RoseSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function RoseSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function RoseSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function RoseSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function RoseSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function RoseSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}