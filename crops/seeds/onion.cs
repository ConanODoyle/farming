$Stackable_OnionSeed_StackedItem0 = "OnionSeed0Item 6";
$Stackable_OnionSeed_StackedItem1 = "OnionSeed1Item 12";
$Stackable_OnionSeed_StackedItem2 = "OnionSeed2Item 18";
$Stackable_OnionSeed_StackedItem3 = "OnionSeed3Item 24";
$Stackable_OnionSeed_StackedItemTotal = 4;

datablock ItemData(OnionSeedItem : HammerItem)
{
	shapeFile = "./seedsFlat.dts";
	uiName = "Onion Seed";
	image = "OnionSeed0Image";
	colorShiftColor = "0.08 0.08 0.08 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "OnionSeed";
};

datablock ItemData(OnionSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Onion Seed1";
	image = "OnionSeed0Image";
	colorShiftColor = "1 0.96 0.74 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "OnionSeed";
};

datablock ItemData(OnionSeed1Item : OnionSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Onion Seed2";
	image = "OnionSeed1Image";
};

datablock ItemData(OnionSeed2Item : OnionSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Onion Seed3";
	image = "OnionSeed2Image";
};

datablock ItemData(OnionSeed3Item : OnionSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Onion Seed4";
	image = "OnionSeed3Image";
};

datablock ShapeBaseImageData(OnionSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = OnionSeed0Item.colorShiftColor;

	item = OnionSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickOnion0CropData";
	cropType = "Onion";

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

datablock ShapeBaseImageData(OnionSeed1Image : OnionSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = OnionSeed1Item;
};

datablock ShapeBaseImageData(OnionSeed2Image : OnionSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = OnionSeed2Item;
};

datablock ShapeBaseImageData(OnionSeed3Image : OnionSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = OnionSeed3Item;
};



function OnionSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function OnionSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function OnionSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function OnionSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function OnionSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function OnionSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function OnionSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function OnionSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}