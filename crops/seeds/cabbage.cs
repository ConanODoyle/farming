$Stackable_CabbageSeed_StackedItem0 = "CabbageSeed0Item 3";
$Stackable_CabbageSeed_StackedItem1 = "CabbageSeed1Item 6";
$Stackable_CabbageSeed_StackedItem2 = "CabbageSeed2Item 9";
$Stackable_CabbageSeed_StackedItem3 = "CabbageSeed3Item 12";
$Stackable_CabbageSeed_StackedItemTotal = 4;

datablock ItemData(CabbageSeedItem : HammerItem)
{
	shapeFile = "./seedsShort.dts";
	uiName = "Cabbage Seed";
	image = "CabbageSeed0Image";
	colorShiftColor = "0.2 0.1 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CabbageSeed";
};

datablock ItemData(CabbageSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Cabbage Seed1";
	image = "CabbageSeed0Image";
	colorShiftColor = "0 0.4 0.2 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CabbageSeed";
};

datablock ItemData(CabbageSeed1Item : CabbageSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Cabbage Seed2";
	image = "CabbageSeed1Image";
};

datablock ItemData(CabbageSeed2Item : CabbageSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Cabbage Seed3";
	image = "CabbageSeed2Image";
};

datablock ItemData(CabbageSeed3Item : CabbageSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Cabbage Seed4";
	image = "CabbageSeed3Image";
};

datablock ShapeBaseImageData(CabbageSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = CabbageSeed0Item.colorShiftColor;

	item = CabbageSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickCabbage0CropData";
	cropType = "Cabbage";

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

datablock ShapeBaseImageData(CabbageSeed1Image : CabbageSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = CabbageSeed1Item;
};

datablock ShapeBaseImageData(CabbageSeed2Image : CabbageSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = CabbageSeed2Item;
};

datablock ShapeBaseImageData(CabbageSeed3Image : CabbageSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = CabbageSeed3Item;
};



function CabbageSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CabbageSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CabbageSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CabbageSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function CabbageSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CabbageSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CabbageSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CabbageSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}