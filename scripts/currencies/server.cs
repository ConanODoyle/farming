
////////
//Item//
////////
$Stackable_Tix_StackedItem0 = "Tix0Item 5";
$Stackable_Tix_StackedItem1 = "Tix1Item 100";
$Stackable_Tix_StackedItemTotal = 2;
$Stackable_Bux_StackedItem0 = "Bux0Item 5";
$Stackable_Bux_StackedItem1 = "Bux1Item 100";
$Stackable_Bux_StackedItemTotal = 2;

datablock ItemData(Tix0Item : HammerItem)
{
	shapeFile = "./resources/tix.dts";
	uiName = "Tix";
	image = "Tix0Image";
	colorShiftColor = "1 1 1 1";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/tix";

	isStackable = 1;
	stackType = "Tix";
};

datablock ShapeBaseImageData(Tix0Image)
{
	shapeFile = "./resources/tix.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = Tix0Item.colorShiftColor;

	item = "Tix0Item";

	armReady = 1;

	offset = "";

	toolTip = "Tree crop quest reward currency";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	Statename[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";
};

datablock ItemData(Tix1Item : Tix0Item)
{
	shapeFile = "./resources/tixstack.dts";
	image = "Tix1Image";
	uiName = "Tix Stack";
};

datablock ShapeBaseImageData(Tix1Image : Tix0Image)
{
	shapeFile = "./resources/tixstack.dts";
	item = "Tix1Item";
};




datablock ItemData(Bux0Item : Tix0Item)
{
	shapeFile = "./resources/bux.dts";
	image = "Bux0Image";
	uiName = "Bux";

	stackType = "Bux";
};

datablock ShapeBaseImageData(Bux0Image : Tix0Image)
{
	shapeFile = "./resources/bux.dts";
	item = "Bux0Item";
	
	toolTip = "Crop quest reward currency";
};

datablock ItemData(Bux1Item : Bux0Item)
{
	shapeFile = "./resources/buxstack.dts";
	image = "Bux1Image";
	uiName = "Bux Stack";
};

datablock ShapeBaseImageData(Bux1Image : Bux0Image)
{
	shapeFile = "./resources/buxstack.dts";
	item = "Bux1Item";
	
	toolTip = "Crop quest reward currency";
};




datablock ItemData(FarmicoinItem : Tix0Item)
{
	shapeFile = "./resources/coin.dts";
	image = "FarmicoinImage";
	uiName = "Farmicoin";

	stackType = "";
	canPickupMultiple = 1;

	isStackable = 0;
};

datablock ShapeBaseImageData(FarmicoinImage : Tix0Image)
{
	shapeFile = "./resources/coin.dts";
	item = "FarmicoinItem";

	offset = "0 0.4 0";
	
	toolTip = "Community goal reward currency";
};


function Tix0Item::onAdd(%this, %item) { Parent::onAdd(%this, %item); %item.rotate = 1;}
function Tix1Item::onAdd(%this, %item) { Parent::onAdd(%this, %item); %item.rotate = 1;}
function Bux0Item::onAdd(%this, %item) { Parent::onAdd(%this, %item); %item.rotate = 1;}
function Bux1Item::onAdd(%this, %item) { Parent::onAdd(%this, %item); %item.rotate = 1;}
function FarmicoinItem::onAdd(%this, %item) { Parent::onAdd(%this, %item); %item.rotate = 1;}

function Tix0Image::onLoop(%this, %obj, %slot)
{
	tixLoop(%this, %obj);
}

function Tix1Image::onLoop(%this, %obj, %slot)
{
	tixLoop(%this, %obj);
}

function Bux0Image::onLoop(%this, %obj, %slot)
{
	buxLoop(%this, %obj);
}

function Bux1Image::onLoop(%this, %obj, %slot)
{
	buxLoop(%this, %obj);
}

function tixLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right>\c3-Farming Tix " @ %obj.currTool @ "- \n\c3Amount\c6: " @ %count @ " ", 1);
	}
}

function buxLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right>\c3-Farming Bux " @ %obj.currTool @ "- \n\c2Amount\c6: " @ %count @ " ", 1);
	}
}