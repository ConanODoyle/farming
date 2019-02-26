$Stackable_CarrotSeed_StackedItem0 = "CarrotSeed0Item 3";
$Stackable_CarrotSeed_StackedItem1 = "CarrotSeed1Item 6";
$Stackable_CarrotSeed_StackedItem2 = "CarrotSeed2Item 9";
$Stackable_CarrotSeed_StackedItem3 = "CarrotSeed3Item 12";
$Stackable_CarrotSeed_StackedItemTotal = 4;

datablock ItemData(CarrotSeedItem : HammerItem)
{
	shapeFile = "./seeds.dts";
	uiName = "Carrot Seed";
	image = "CarrotSeed0Image";
	colorShiftColor = "0.39 0.34 0.30 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CarrotSeed";
};

datablock ItemData(CarrotSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Carrot Seed1";
	image = "CarrotSeed0Image";
	colorShiftColor = "1 0.502 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "CarrotSeed";
};

datablock ItemData(CarrotSeed1Item : CarrotSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Carrot Seed2";
	image = "CarrotSeed1Image";
};

datablock ItemData(CarrotSeed2Item : CarrotSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Carrot Seed3";
	image = "CarrotSeed2Image";
};

datablock ItemData(CarrotSeed3Item : CarrotSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Carrot Seed4";
	image = "CarrotSeed3Image";
};

datablock ShapeBaseImageData(CarrotSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = CarrotSeed0Item.colorShiftColor;

	item = CarrotSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickCarrot0CropData";
	cropType = "Carrot";

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

datablock ShapeBaseImageData(CarrotSeed1Image : CarrotSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = CarrotSeed1Item;
};

datablock ShapeBaseImageData(CarrotSeed2Image : CarrotSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = CarrotSeed2Item;
};

datablock ShapeBaseImageData(CarrotSeed3Image : CarrotSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = CarrotSeed3Item;
};



function CarrotSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CarrotSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CarrotSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function CarrotSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function CarrotSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CarrotSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CarrotSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function CarrotSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}