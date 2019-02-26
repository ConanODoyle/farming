$Stackable_TomatoSeed_StackedItem0 = "TomatoSeed0Item 3";
$Stackable_TomatoSeed_StackedItem1 = "TomatoSeed1Item 6";
$Stackable_TomatoSeed_StackedItem2 = "TomatoSeed2Item 9";
$Stackable_TomatoSeed_StackedItem3 = "TomatoSeed3Item 12";
$Stackable_TomatoSeed_StackedItemTotal = 4;

datablock ItemData(TomatoSeedItem : HammerItem)
{
	shapeFile = "./seedsFlat.dts";
	uiName = "Tomato Seed";
	image = "TomatoSeed0Image";
	colorShiftColor = "0.8 0.66 0.48 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "TomatoSeed";
};

datablock ItemData(TomatoSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Tomato Seed1";
	image = "TomatoSeed0Image";
	colorShiftColor = "1 0 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "TomatoSeed";
};

datablock ItemData(TomatoSeed1Item : TomatoSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Tomato Seed2";
	image = "TomatoSeed1Image";
};

datablock ItemData(TomatoSeed2Item : TomatoSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Tomato Seed3";
	image = "TomatoSeed2Image";
};

datablock ItemData(TomatoSeed3Item : TomatoSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Tomato Seed4";
	image = "TomatoSeed3Image";
};

datablock ShapeBaseImageData(TomatoSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = TomatoSeed0Item.colorShiftColor;

	item = TomatoSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickTomato0CropData";
	cropType = "Tomato";

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

datablock ShapeBaseImageData(TomatoSeed1Image : TomatoSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = TomatoSeed1Item;
};

datablock ShapeBaseImageData(TomatoSeed2Image : TomatoSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = TomatoSeed2Item;
};

datablock ShapeBaseImageData(TomatoSeed3Image : TomatoSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = TomatoSeed3Item;
};



function TomatoSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function TomatoSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function TomatoSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function TomatoSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function TomatoSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function TomatoSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function TomatoSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function TomatoSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}