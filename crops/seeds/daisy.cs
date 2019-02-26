$Stackable_DaisySeed_StackedItem0 = "DaisySeed0Item 6";
$Stackable_DaisySeed_StackedItem1 = "DaisySeed1Item 12";
$Stackable_DaisySeed_StackedItem2 = "DaisySeed2Item 18";
$Stackable_DaisySeed_StackedItem3 = "DaisySeed3Item 24";
$Stackable_DaisySeed_StackedItemTotal = 4;

datablock ItemData(DaisySeedItem : HammerItem)
{
	shapeFile = "./seeds.dts";
	uiName = "Daisy Seed";
	image = "DaisySeed0Image";
	colorShiftColor = "0.9 0.68 0.12 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "DaisySeed";
};

datablock ItemData(DaisySeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Daisy Seed1";
	image = "DaisySeed0Image";
	colorShiftColor = "0.568 0.329 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "DaisySeed";
};

datablock ItemData(DaisySeed1Item : DaisySeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Daisy Seed2";
	image = "DaisySeed1Image";
};

datablock ItemData(DaisySeed2Item : DaisySeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Daisy Seed3";
	image = "DaisySeed2Image";
};

datablock ItemData(DaisySeed3Item : DaisySeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Daisy Seed4";
	image = "DaisySeed3Image";
};

datablock ShapeBaseImageData(DaisySeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = DaisySeed0Item.colorShiftColor;

	item = DaisySeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickDaisy0CropData";
	cropType = "Daisy";

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

datablock ShapeBaseImageData(DaisySeed1Image : DaisySeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = DaisySeed1Item;
};

datablock ShapeBaseImageData(DaisySeed2Image : DaisySeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = DaisySeed2Item;
};

datablock ShapeBaseImageData(DaisySeed3Image : DaisySeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = DaisySeed3Item;
};



function DaisySeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function DaisySeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function DaisySeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function DaisySeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function DaisySeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DaisySeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DaisySeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DaisySeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}