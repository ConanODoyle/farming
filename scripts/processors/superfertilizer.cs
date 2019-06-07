$Stackable_SuperFertilizer_StackedItem0 = "SuperFertilizerBag0Item 10";
$Stackable_SuperFertilizer_StackedItem1 = "SuperFertilizerBag1Item 20";
$Stackable_SuperFertilizer_StackedItem2 = "SuperFertilizerBag2Item 30";
$Stackable_SuperFertilizer_StackedItemTotal = 3;

datablock ItemData(SuperFertilizerBag0Item : HammerItem)
{
	shapeFile = "./resources/fertilizerBag0.dts";
	uiName = "Super Fertilizer Bag";
	image = "SuperFertilizerBag0Image";
	colorShiftColor = "1 0.7 0 1";
	doColorShift = true;

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag";

	isStackable = 1;
	stackType = "Fertilizer";
};

datablock ShapeBaseImageData(SuperFertilizerBag0Image : FertilizerBag0Image)
{
	doColorShift = true;
	colorShiftColor = SuperFertilizerBag0Item.colorShiftColor;

	item = "SuperFertilizerBag0Item";
	toolTip = "Make plants grow instantly, chance of shiny";

	bonusGrowTicks = 100; //bonus grow tick per use (does not consume water)
	bonusGrowTime = 1000; //reduction in seconds to next grow tick
	shinyChance = 0.008;
};

datablock ItemData(SuperFertilizerBag1Item : SuperFertilizerBag0Item)
{
	shapeFile = "./resources/fertilizerBag1.dts";
	image = "SuperFertilizerBag1Image";
	uiName = "Half Super Fertilizer Bag";

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag_Half";
};

datablock ShapeBaseImageData(SuperFertilizerBag1Image : SuperFertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag1.dts";
	item = "SuperFertilizerBag1Item";
};

datablock ItemData(SuperFertilizerBag2Item : SuperFertilizerBag0Item)
{
	shapeFile = "./resources/fertilizerBag2.dts";
	image = "SuperFertilizerBag2Image";
	uiName = "Full Super Fertilizer Bag";

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag_Full";
};

datablock ShapeBaseImageData(SuperFertilizerBag2Image : SuperFertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag2.dts";
	item = "SuperFertilizerBag2Item";
};


function SuperFertilizerBag0Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function SuperFertilizerBag1Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function SuperFertilizerBag2Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}



function SuperFertilizerBag0Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function SuperFertilizerBag1Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function SuperFertilizerBag2Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function fertilizerLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<color:ffff00>-Fertilizer Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}
