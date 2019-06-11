
datablock fxDTSBrickData(brickSuperFertilizerMachineData)
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Compost Bin";

	brickFile = "./resources/compostBin.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/compost_bin";

	cost = 0;
	isProcessor = 1;
	isCompostBin = 1;
	isStorageBrick = 1;
	processorFunction = "processIntoSuperFertilizer";
	activateFunction = "SuperFertilizerMachineInfo";
	placerItem = "SuperFertilizerMachineItem";
	keepActivate = 1;

	tickTime = 20;
	tickAmt = 1;
};

function processIntoSuperFertilizer(%brick, %cl, %slot)
{
	if (getTrustLevel(%brick, %cl) < 1)
	{
		serverCmdUnuseTool(%cl);
		%cl.centerprint(getBrickgroupFromObject(%brick).name @ "<color:ff0000> does not trust you enough to do that!", 1);
		return;
	}

	%pl = %cl.player;
	%item = %pl.tool[%slot];
	if (%item.isStackable && %item.stackType !$= "")
	{
		%cropType = %item.stackType;
		%max = $Stackable_[%cropType, "stackedItemTotal"] - 1;
		%max = getWord($Stackable_[%cropType, "StackedItem" @ %max], 1);

		if (!%cropType !$= "Fertilizer")
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("You cannot process this into super fertilizer!", 1);
			return;
		}

		%fertCount = %pl.toolStackCount[%slot];
		%pl.tool[%slot] = "";
		%pl.toolStackCount[%slot] = 0;
		serverCmdUnuseTool(%cl);
		messageClient(%cl, 'MsgItemPickup', '', %slot, 0);

		%superFert = %fertCount / 10;
		%count = getSubStr(%brick.getName(), 1, 100);
		%brick.setName("_" @ (%count + %superFert));
		%cl.centerprint("<color:ffffff>You queued <color:ffff00>" @ %superFert @ "<color:ffffff> super fertilizer for production!", 3);
		return;
	}
}

function SuperFertilizerMachineInfo(%brick, %pl)
{
	if (%pl.client.lastMessagedSuperFertilizerInfo + 5 < $Sim::Time)
	{
		messageClient(%pl.client, '', "\c3Drop (Ctrl-W) fertilizer in the hopper to make fertilizer! One super fertilizer takes ten fertilizer.", 2);
		%pl.client.lastMessagedSuperFertilizerInfo = $Sim::Time;
	}
}

package SuperFertilizerPackage
{
	function createFertilizer(%brick)
	{
		if (%brick.getDatablock().processorFunction $= "processIntoSuperFertilizer")
		{	
			%name = %brick.getName();
			%count = getSubStr(%name, 1, strLen(%name));

			if (%brick.nextCompostTime $= "")
			{
				%brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime;
				return;
			}

			%maxCount = %brick.getDatablock().tickAmt;
			if (mFloor(%count) > 0)
			{
				%amt = getMin(%maxCount, mFloor(%count));
				%origAmt = %amt;
			}
			else
			{
				return;
			}

			//check if there's space for new fertilizer, if yes, add
			%multiplier = %brick.getDatablock().storageBonus;
			%storageMax = $StorageMax_["SuperFertilizer"] * (%multiplier < 1 ? 1 : %multiplier);
			for (%i = 0; %i < 4; %i++) 
			{
				%curr = validateStorageContents(%brick.eventOutputParameter[0, %i + 1], %brick);
				if (getField(%curr, 1) < %storageMax)
				{
					%addAmt = getMin(%storageMax - getField(%curr, 1), %amt);
					%amt -= %addAmt;
					%brick.eventOutputParameter[0, %i + 1] = "SuperFertilizer\" " @ getField(%curr, 1) + %addAmt;
				}

				if (%amt <= 0)
				{
					break;
				}
			}

			%amtAdded = %origAmt - %amt;
			if (%amtAdded > 0)
			{
				%count = %count - %amtAdded;
				%brick.setName("_" @ %count);
			}
			%brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime;

			%brick.updateCenterprintMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
		}
		else
		{
			return parent::createFertilizer(%brick);
		}
	}
};
activatePackage(SuperFertilizerPackage);


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

	iconName = "Add-ons/Server_Farming/crops/icons/Super_Fertilizer_Bag";

	isStackable = 1;
	stackType = "SuperFertilizer";
};

datablock ShapeBaseImageData(SuperFertilizerBag0Image : FertilizerBag0Image)
{
	doColorShift = true;
	colorShiftColor = SuperFertilizerBag0Item.colorShiftColor;

	item = "SuperFertilizerBag0Item";
	toolTip = "Make plants grow instantly, chance of shiny";

	bonusGrowTicks = 100; //bonus grow tick per use (does not consume water)
	bonusGrowTime = 1000; //reduction in seconds to next grow tick
	shinyChance = 0.01;
};

datablock ItemData(SuperFertilizerBag1Item : SuperFertilizerBag0Item)
{
	shapeFile = "./resources/fertilizerBag1.dts";
	image = "SuperFertilizerBag1Image";
	uiName = "Half Super Fertilizer Bag";

	iconName = "Add-ons/Server_Farming/crops/icons/Super_Fertilizer_Bag_Half";
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

	iconName = "Add-ons/Server_Farming/crops/icons/Super_Fertilizer_Bag_Full";
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
