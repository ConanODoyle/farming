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

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
	
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

	iconName = "Add-ons/Server_Farming/crops/icons/large_compost_bin";
	
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

	iconName = "Add-ons/Server_Farming/crops/icons/large_compost_bin";
	
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
