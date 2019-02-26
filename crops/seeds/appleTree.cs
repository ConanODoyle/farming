$Stackable_AppleSeed_StackedItem0 = "AppleSeed0Item 1";
$Stackable_AppleSeed_StackedItem1 = "AppleSeed1Item 2";
$Stackable_AppleSeed_StackedItem2 = "AppleSeed2Item 3";
$Stackable_AppleSeed_StackedItem3 = "AppleSeed3Item 4";
$Stackable_AppleSeed_StackedItemTotal = 4;

datablock ItemData(AppleSeedItem : HammerItem)
{
	shapeFile = "./seedsShort.dts";
	uiName = "Apple Seed";
	image = "AppleSeed0Image";
	colorShiftColor = "0.09 0.04 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "AppleSeed";
};

datablock ItemData(AppleSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Apple Seed1";
	image = "AppleSeed0Image";
	colorShiftColor = "1 0 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "AppleSeed";
};

datablock ItemData(AppleSeed1Item : AppleSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Apple Seed2";
	image = "AppleSeed1Image";
};

datablock ItemData(AppleSeed2Item : AppleSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Apple Seed3";
	image = "AppleSeed2Image";
};

datablock ItemData(AppleSeed3Item : AppleSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Apple Seed4";
	image = "AppleSeed3Image";
};

datablock ShapeBaseImageData(AppleSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = AppleSeed0Item.colorShiftColor;

	item = AppleSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickAppleTree0CropData";
	cropType = "Apple";

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

datablock ShapeBaseImageData(AppleSeed1Image : AppleSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = AppleSeed1Item;
};

datablock ShapeBaseImageData(AppleSeed2Image : AppleSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = AppleSeed2Item;
};

datablock ShapeBaseImageData(AppleSeed3Image : AppleSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = AppleSeed3Item;
};



function AppleSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function AppleSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function AppleSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function AppleSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function AppleSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function AppleSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function AppleSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function AppleSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}