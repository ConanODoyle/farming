$Stackable_PotatoSeed_StackedItem0 = "PotatoSeed0Item 3";
$Stackable_PotatoSeed_StackedItem1 = "PotatoSeed1Item 6";
$Stackable_PotatoSeed_StackedItem2 = "PotatoSeed2Item 9";
$Stackable_PotatoSeed_StackedItem3 = "PotatoSeed3Item 12";
$Stackable_PotatoSeed_StackedItemTotal = 4;

datablock ItemData(PotatoSeedItem : HammerItem)
{
	shapeFile = "./seeds.dts";
	uiName = "Potato Seed";
	image = "PotatoSeed0Image";
	colorShiftColor = "0.9 0.68 0.12 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "PotatoSeed";
};

datablock ItemData(PotatoSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Potato Seed1";
	image = "PotatoSeed0Image";
	colorShiftColor = "0.568 0.329 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "PotatoSeed";
};

datablock ItemData(PotatoSeed1Item : PotatoSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Potato Seed2";
	image = "PotatoSeed1Image";
};

datablock ItemData(PotatoSeed2Item : PotatoSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Potato Seed3";
	image = "PotatoSeed2Image";
};

datablock ItemData(PotatoSeed3Item : PotatoSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Potato Seed4";
	image = "PotatoSeed3Image";
};

datablock ShapeBaseImageData(PotatoSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = PotatoSeed0Item.colorShiftColor;

	item = PotatoSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickPotato0CropData";
	cropType = "Potato";

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

datablock ShapeBaseImageData(PotatoSeed1Image : PotatoSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = PotatoSeed1Item;
};

datablock ShapeBaseImageData(PotatoSeed2Image : PotatoSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = PotatoSeed2Item;
};

datablock ShapeBaseImageData(PotatoSeed3Image : PotatoSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = PotatoSeed3Item;
};



function PotatoSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PotatoSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PotatoSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function PotatoSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function PotatoSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PotatoSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PotatoSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function PotatoSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}