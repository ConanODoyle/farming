$Stackable_TurnipSeed_StackedItem0 = "TurnipSeed0Item 6";
$Stackable_TurnipSeed_StackedItem1 = "TurnipSeed1Item 12";
$Stackable_TurnipSeed_StackedItem2 = "TurnipSeed2Item 18";
$Stackable_TurnipSeed_StackedItem3 = "TurnipSeed3Item 24";
$Stackable_TurnipSeed_StackedItemTotal = 4;

datablock ItemData(TurnipSeedItem : HammerItem)
{
	shapeFile = "./seedsRound.dts";
	uiName = "Turnip Seed";
	image = "TurnipSeed0Image";
	colorShiftColor = "0.08 0.08 0.08 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "TurnipSeed";
};

datablock ItemData(TurnipSeed0Item : HammerItem)
{
	shapeFile = "./seed1.dts";
	uiName = "Turnip Seed1";
	image = "TurnipSeed0Image";
	colorShiftColor = "0.9 0.9 0.9 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "TurnipSeed";
};

datablock ItemData(TurnipSeed1Item : TurnipSeed0Item)
{
	shapeFile = "./seed2.dts";

	uiName = "Turnip Seed2";
	image = "TurnipSeed1Image";
};

datablock ItemData(TurnipSeed2Item : TurnipSeed0Item)
{
	shapeFile = "./seed3.dts";

	uiName = "Turnip Seed3";
	image = "TurnipSeed2Image";
};

datablock ItemData(TurnipSeed3Item : TurnipSeed0Item)
{
	shapeFile = "./seed4.dts";

	uiName = "Turnip Seed4";
	image = "TurnipSeed3Image";
};

datablock ShapeBaseImageData(TurnipSeed0Image)
{
	shapeFile = "./seed1.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = TurnipSeed0Item.colorShiftColor;

	item = TurnipSeed0Item;

	armReady = 1;

	offset = "";

	cropBrick = "brickTurnip0CropData";
	cropType = "Turnip";

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

datablock ShapeBaseImageData(TurnipSeed1Image : TurnipSeed0Image) 
{
	shapeFile = "./seed2.dts";

	item = TurnipSeed1Item;
};

datablock ShapeBaseImageData(TurnipSeed2Image : TurnipSeed0Image) 
{
	shapeFile = "./seed3.dts";

	item = TurnipSeed2Item;
};

datablock ShapeBaseImageData(TurnipSeed3Image : TurnipSeed0Image) 
{
	shapeFile = "./seed4.dts";

	item = TurnipSeed3Item;
};



function TurnipSeed0Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function TurnipSeed1Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function TurnipSeed2Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function TurnipSeed3Image::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}



function TurnipSeed0Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function TurnipSeed1Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function TurnipSeed2Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function TurnipSeed3Image::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}