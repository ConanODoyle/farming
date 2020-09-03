$Stackable_PortobelloSeed_StackedItem0 = "PortobelloSeed0Item 4";
$Stackable_PortobelloSeed_StackedItem1 = "PortobelloSeed1Item 8";
$Stackable_PortobelloSeed_StackedItem2 = "PortobelloSeed2Item 12";
$Stackable_PortobelloSeed_StackedItem3 = "PortobelloSeed3Item 26";
$Stackable_PortobelloSeed_StackedItemTotal = 4;

datablock ItemData(PortobelloSeedItem : HammerItem)
{
	shapeFile = "./seedsRound.dts";
	uiName = "Portobello Seed";
	image = "PortobelloSeed0Image";
	colorShiftColor = "0.08 0.08 0.08 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "PortobelloSeed";
};

datablock ItemData(PortobelloSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Portobello Seed1";
	image = "PortobelloSeed0Image";
	colorShiftColor = "0.9 0.9 0.9 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "PortobelloSeed";
};

datablock ItemData(PortobelloSeed1Item : PortobelloSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Portobello Seed2";
	image = "PortobelloSeed1Image";
};

datablock ItemData(PortobelloSeed2Item : PortobelloSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Portobello Seed3";
	image = "PortobelloSeed2Image";
};

datablock ItemData(PortobelloSeed3Item : PortobelloSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Portobello Seed4";
	image = "PortobelloSeed3Image";
};

datablock ShapeBaseImageData(PortobelloSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = PortobelloSeed0Item.colorShiftColor;

	item = PortobelloSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickPortobello0CropData";
	cropType = "Portobello";

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

datablock ShapeBaseImageData(PortobelloSeed1Image : PortobelloSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = PortobelloSeed1Item;
};

datablock ShapeBaseImageData(PortobelloSeed2Image : PortobelloSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = PortobelloSeed2Item;
};

datablock ShapeBaseImageData(PortobelloSeed3Image : PortobelloSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = PortobelloSeed3Item;
};



function PortobelloSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PortobelloSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PortobelloSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PortobelloSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function PortobelloSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PortobelloSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PortobelloSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PortobelloSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}