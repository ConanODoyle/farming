



//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickGrinderData)
{
	uiName = "Grinder";

	brickFile = "./resources/grinder/grinder.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "grinderInfo";
	placerItem = "GrinderItem";
	keepActivate = 1;

	tickTime = 10;
	tickAmt = 1;
};

datablock fxDTSBrickData(brickLiquifierData)
{
	uiName = "Liquifier";

	brickFile = "./resources/liquifier/liquifier.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "liquifyContents";
	// activateFunction = "liquifierInfo";
	placerItem = "LiquifierItem";
	keepActivate = 1;

	tickTime = 10;
	tickAmt = 1;
};

datablock fxDTSBrickData(brickBaguetteMachineData)
{
	uiName = "BaguetteMachine";

	brickFile = "./resources/breadmaker/breadmaker.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "makeBread";
	// activateFunction = "BaguetteMachineInfo";
	placerItem = "BaguetteMachineItem";
	keepActivate = 1;

	tickTime = 10;
	tickAmt = 1;
};



///////////////
//Placer Item//
///////////////

datablock ItemData(GrinderItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Grinder";
	image = "GrinderBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/compost_bin";
	
	cost = 800;
};

datablock ShapeBaseImageData(GrinderBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = GrinderItem;
	
	doColorshift = true;
	colorShiftColor = GrinderItem.colorShiftColor;

	toolTip = "Places a Grinder";
	loopTip = "Converts produce into its ground form, when powered";
	placeBrick = "brickGrinderData";
};

function GrinderBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function GrinderBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function GrinderBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function GrinderBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(LiquifierItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Liquifier";
	image = "LiquifierBrickImage";
	colorShiftColor = "0.5 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/large_compost_bin";
	
	cost = 1600;
};

datablock ShapeBaseImageData(LiquifierBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = LiquifierItem;
	
	doColorshift = true;
	colorShiftColor = LiquifierItem.colorShiftColor;

	toolTip = "Places a Liquifier";
	loopTip = "Liquifies substances put in";
	placeBrick = "brickLiquifierData";
};

function LiquifierBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function LiquifierBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function LiquifierBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function LiquifierBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(BaguetteMachineItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Baguette Machine";
	image = "BaguetteMachineBrickImage";
	colorShiftColor = "0.5 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/large_compost_bin";
	
	cost = 1600;
};

datablock ShapeBaseImageData(BaguetteMachineBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = BaguetteMachineItem;
	
	doColorshift = true;
	colorShiftColor = BaguetteMachineItem.colorShiftColor;

	toolTip = "Places a Baguette Machine";
	loopTip = "Takes flour and makes baguettes";
	placeBrick = "brickBaguetteMachineData";
};

function BaguetteMachineBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function BaguetteMachineBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function BaguetteMachineBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function BaguetteMachineBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}







////////
//Item//
////////

$Stackable_Flour_StackedItem0 = "FlourBag0Item 4";
$Stackable_Flour_StackedItem1 = "FlourBag1Item 8";
$Stackable_Flour_StackedItem2 = "FlourBag2Item 12";
$Stackable_Flour_StackedItemTotal = 3;

datablock ItemData(FlourBag0Item : HammerItem)
{
	shapeFile = "./resources/Flour/FlourBag0.dts";
	uiName = "Flour Bag";
	image = "FlourBag0Image";
	colorShiftColor = "0.85 0.78 0.6 1";
	doColorShift = true;

	iconName = "Add-ons/Server_Farming/icons/Flour_Bag";

	isStackable = 1;
	stackType = "Flour";
};

datablock ShapeBaseImageData(FlourBag0Image)
{
	shapeFile = "./resources/Flour/FlourBag0.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = FlourBag0Item.colorShiftColor;

	item = "FlourBag0Item";

	armReady = 1;

	offset = "-0.1 0.3 -0.1";

	toolTip = "Flour";

	bonusGrowTicks = 0; //bonus grow tick per use (does not consume water)
	bonusGrowTime = 10; //reduction in seconds to next grow tick
	shinyChance = 0.004;

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
	stateTransitionOnTimeout[3]	= "LoopA";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = true;
};

datablock ItemData(FlourBag1Item : FlourBag0Item)
{
	shapeFile = "./resources/Flour/FlourBag1.dts";
	image = "FlourBag1Image";
	uiName = "Half Flour Bag";

	iconName = "Add-ons/Server_Farming/icons/Flour_Bag_Half";
};

datablock ShapeBaseImageData(FlourBag1Image : FlourBag0Image)
{
	shapeFile = "./resources/Flour/FlourBag1.dts";
	item = "FlourBag1Item";

	offset = "-0.05 0.3 -0.3";
};

datablock ItemData(FlourBag2Item : FlourBag0Item)
{
	shapeFile = "./resources/Flour/FlourBag2.dts";
	image = "FlourBag2Image";
	uiName = "Full Flour Bag";

	iconName = "Add-ons/Server_Farming/icons/Flour_Bag_Full";
};

datablock ShapeBaseImageData(FlourBag2Image : FlourBag0Image)
{
	shapeFile = "./resources/Flour/FlourBag2.dts";
	item = "FlourBag2Item";

	offset = "-0.05 0.3 -0.3";
};


function FlourBag0Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function FlourBag1Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function FlourBag2Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}



function FlourBag0Image::onLoop(%this, %obj, %slot)
{
	FlourLoop(%this, %obj);
}

function FlourBag1Image::onLoop(%this, %obj, %slot)
{
	FlourLoop(%this, %obj);
}

function FlourBag2Image::onLoop(%this, %obj, %slot)
{
	FlourLoop(%this, %obj);
}

function FlourLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right><color:ffff00>-Flour Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}