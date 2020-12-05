if (!isObject(CompostBinSimSet)) 
{
	$CompostBinSimSet = new SimSet(CompostBinSimSet) {};
}

package CompostBinSimSetCollector
{
	function fxDTSBrick::onAdd(%brick) 
	{
		%ret = parent::onAdd(%brick);

		if (%brick.isPlanted && %brick.getDatablock().isCompostBin)
		{
			CompostBinSimSet.add(%brick);
		}
		return %ret;
	}
};
activatePackage(CompostBinSimSetCollector);

package CompostBinRetrieveOnly
{
	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID)
	{
		if (%storageObj.getDatablock().isCompostBin)
		{
			return 2;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);
	}

	function fxDTSBrick::accessStorage(%brick, %dataID, %cl)
	{
		return parent::accessStorage(%brick, %dataID, %cl);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID);
		if (%brick.getDatablock().isCompostBin)
		{
			%brick.centerprintMenu.menuOptionCount = 2;
			%brick.centerprintMenu.menuOption[1] = "Queued: " @ getDataIDArrayTagValue(%dataID, "compostQueue") + 0;
			%brick.centerprintMenu.menuFunction[1] = "";
		}
		return %ret;
	}
};
activatePackage(CompostBinRetrieveOnly);

function compostTick(%index)
{
	cancel($masterCompostTickSchedule);
	
	if (!isObject(MissionCleanup)) 
	{
		return;
	}

	//if no compost bins just skip everything
	%count = CompostBinSimSet.getCount();
	if (%count <= 0)
	{
		$masterCompostTickSchedule = schedule(100, 0, compostTick, %index);
		return;
	}

	for (%i = 0; %i < %count; %i++)
	{
		if (%index >= %count)
		{
			break;
		}
		%brick = CompostBinSimSet.getObject(%index);

		if (%brick.nextCompostTime < $Sim::Time)
		{
			createFertilizer(%brick);
		}
		%index++;
	}

	if (%index >= %count)
	{
		%index = 0;
	}

	$masterCompostTickSchedule = schedule(100, 0, compostTick, %index);
}

function fertilizeCrop(%img, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyePoint();
	%end = vectorAdd(vectorScale(%obj.getEyeVector(), 4), %start);
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%brick = getWord(%ray, 0);

	if (isObject(%brick) && %brick.getDatablock().isPlant && !%brick.getDatablock().isIndividuallyFertilized)
	{
		%brick = %brick.getDownBrick(0);
	}

	if (!isObject(%brick) || !(%brick.getDatablock().isIndividuallyFertilized || %brick.getDatablock().isDirt))
	{
		return;
	}

	if (%brick.getDatablock().isDirt)
	{
		%numGrown = 0;
		%numCrops = 0;

		%upBricks = %brick.getNumUpBricks();
		%mod = 1 + mFloor(%upBricks / 6); // Max of 6 explosions

		for (%i = 0; %i < %upBricks; %i++)
		{
			%crop = %brick.getUpBrick(%i);

			if (!%crop.getDatablock().isPlant || %crop.getDatablock().isIndividuallyFertilized)
			{
				continue;
			}

			%type = %crop.getDatablock().cropType;
			%stage = %crop.getDatablock().stage;

			if (getTrustLevel(%crop, %obj) < 1)
			{
				%obj.client.centerprint(%crop.getGroup().name @ "<color:ff0000> does not trust you enough to do that!", 1);
				continue;
			}
			else if (getPlantData(%type, %stage, "tickTime") <= 1)
			{
				%obj.client.centerprint("This plant already is fully grown!");
				%numGrown++;
				%numCrops++;
				continue;
			}
			
			%numCrops++;

			if(%i % %mod == 0)
			{
				%hitloc = %crop.getPosition();
				%p = new Projectile() { dataBlock = PushBroomProjectile; initialPosition = %hitloc; };
				%p.setScale("0.5 0.5 0.5");
				%p.explode();
			}

			%crop.growTick += %img.bonusGrowTicks;
			%crop.nextGrow -= %img.bonusGrowTime;

			if (!isObject(%crop.emitter))
			{
				if (getRandom() < %img.shinyChance)
				{
					%rand = getRandom();
					if (%rand < 0.025)
					{
						//gold plant
						%crop.setEmitter(goldenEmitter.getID());
						%type = "<color:faef00>Golden";
					}
					else if (%rand < 0.25)
					{
						//silver plant
						%crop.setEmitter(silverEmitter.getID());
						%type = "<color:fafafa>Silver";
					}
					else
					{
						//bronze plant
						%crop.setEmitter(bronzeEmitter.getID());
						%type = "<color:fafafa>Bronze";
					}

					if (isObject(%cl = %obj.client))
					{
						messageAll('MsgUploadStart', "<bitmap:base/client/ui/ci/star> \c3" @
							%cl.name @ "\c6 fertilized a " @ %type SPC %crop.getDatablock().cropType @ "\c6!");
					}
				}
			}
		}
		if (%numGrown == %numCrops)
		{
			%obj.client.centerprint("All of the small plants are already fully grown!");
			return;
		}
	}
	else
	{
		%crop = %brick;

		%type = %crop.getDatablock().cropType;
		%stage = %crop.getDatablock().stage;

		if (getTrustLevel(%crop, %obj) < 1)
		{
			%obj.client.centerprint(%crop.getGroup().name @ "<color:ff0000> does not trust you enough to do that!", 1);
			return;
		}
		else if (getPlantData(%type, %stage, "tickTime") <= 1)
		{
			%obj.client.centerprint("This plant already is fully grown!");
			return;
		}

		%hitloc = getWords(%ray, 1, 3);
		%p = new Projectile() { dataBlock = PushBroomProjectile; initialPosition = %hitloc; };
		%p.setScale("0.5 0.5 0.5");
		%p.explode();

		%crop.growTick += %img.bonusGrowTicks;
		%crop.nextGrow -= %img.bonusGrowTime;

		if (!isObject(%crop.emitter))
		{
			if (getRandom() < %img.shinyChance * 3)
			{
				%rand = getRandom();
				if (%rand < 0.05)
				{
					//gold plant
					%crop.setEmitter(goldenEmitter.getID());
					%type = "<color:faef00>Golden";
				}
				else if (%rand < 0.25)
				{
					//silver plant
					%crop.setEmitter(silverEmitter.getID());
					%type = "<color:fafafa>Silver";
				}
				else
				{
					//bronze plant
					%crop.setEmitter(bronzeEmitter.getID());
					%type = "<color:fafafa>Bronze";
				}

				if (isObject(%cl = %obj.client))
				{
					messageAll('MsgUploadStart', "<bitmap:base/client/ui/ci/star> \c3" @
						%cl.name @ "\c6 fertilized a " @ %type SPC %crop.getDatablock().cropType @ "\c6!");
				}
			}
		}
	}

	//increase weed chance on the dirt
	if (%brick.getDatablock().isDirt)
	{
		%brick.fertilizerWeedModifier += 1;
	}
	else
	{
		for (%i = 0; %i < %brick.getNumDownBricks(); %i++)
		{
			%brick.getDownBrick(%i).fertilizerWeedModifier += 1;
		}
	}

	//fertilization successful, update item

	%count = %obj.toolStackCount[%obj.currTool]--;
	%slot = %obj.currTool;
	%type = %obj.tool[%slot].stackType; //earlier it was set to the croptype of the brick
	if (%count <= 0) //no more seeds left, clear the item slot
	{
		messageClient(%obj.client, 'MsgItemPickup', '', %slot, 0);
		%obj.tool[%slot] = "";
		%obj.unmountImage(%imageSlot);
		return %b;
	}

	//some seeds are left, update item if needed
	%bestItem = getStackTypeDatablock(%type, %count);

	messageClient(%obj.client, 'MsgItemPickup', '', %slot, %bestItem.getID());
	%obj.tool[%slot] = %bestItem.getID();
	%obj.mountImage(%imageSlot, %bestItem.image);


	%item = %img.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right><color:ffff00>-Fertilizer Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}

function fertilizeDirt(%img, %obj, %slot)
{
	%obj.playThread(0, plant);
	%cl = %obj.client;

	%start = %obj.getEyePoint();
	%end = vectorAdd(vectorScale(%obj.getEyeVector(), 4), %start);
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%brick = getWord(%ray, 0);

	if (isObject(%brick) && %brick.getDatablock().isPlant)
	{
		%brick = %brick.getDownBrick(0);
	}

	if (!isObject(%brick) || !%brick.getDatablock().isDirt)
	{
		return;
	}

	%brickDB = %brick.getDatablock();

	if (%brickDB.isDirt)
	{
		%nutrients = %brick.getNutrients();
		if (getWord(%nutrients, 0) + getWord(%nutrients, 1) >= %brickDB.maxNutrients)
		{
			messageClient(%cl, '', "Nutrients are maxed out! Use a shovel to remove and recover some nutrients.");
			return;
		}
		%nitAdd = %img.fertilizerNitrogen + 0;
		%phoAdd = %img.fertilizerPhosphate + 0;
		%weedAdd = %img.fertilizerWeedkiller + 0;

		%finalNutrients = vectorAdd(%nutrients, %nitAdd SPC %phoAdd SPC %weedAdd);
		%finalNit = getWord(%finalNutrients, 0);
		%finalPho = getWord(%finalNutrients, 1);
		%finalWeedkiller = getWord(%finalNutrients, 2);
		if (%finalNit + %finalPho > %brickDB.maxNutrients)
		{
			%modFactor = %brickDB.maxNutrients / (%finalNit + %finalPho);
			%finalNit = mFloor((%finalNit * %modFactor) + 0.5);
			%finalPho = mFloor((%finalPho * %modFactor) + 0.5);
		}
		if (%finalWeedkiller > %brickDB.maxWeedkiller)
		{
			%finalWeedkiller = %brickDB.maxWeedkiller;
		}
		%finalNutrients = %finalNit SPC %finalPho SPC %finalWeedkiller;
		%brick.setNutrients(%finalNit, %finalPho, %finalWeedkiller);

		//increase weed chance on the dirt
		// %brick.fertilizerWeedModifier += 1;
	}

	//fertilization successful, update item

	%count = %obj.toolStackCount[%obj.currTool]--;
	%slot = %obj.currTool;
	%type = %obj.tool[%slot].stackType; //earlier it was set to the croptype of the brick
	if (%count <= 0) //no more seeds left, clear the item slot
	{
		messageClient(%cl, 'MsgItemPickup', '', %slot, 0);
		%obj.tool[%slot] = "";
		%obj.unmountImage(%imageSlot);
		return %b;
	}

	//some seeds are left, update item if needed
	%bestItem = getStackTypeDatablock(%type, %count);

	messageClient(%cl, 'MsgItemPickup', '', %slot, %bestItem.getID());
	%obj.tool[%slot] = %bestItem.getID();
	%obj.mountImage(%imageSlot, %bestItem.image);

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right><color:ffff00>-" @ %type @ " Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}

function createFertilizer(%brick)
{
    %dataID = %brick.eventOutputParameter0_1;
    %count = getDataIDArrayTagValue(%dataID, "compostQueue");

    if (%brick.nextCompostTime $= "")
    {
        %brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime;
        return;
    }

    %maxCount = %brick.getDatablock().tickAmt;
    if (%count > 0)
    {
        %amt = getMin(%maxCount, %count);
        %origAmt = %amt;
    }
    else
    {
        %brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime / 10;
        return;
    }

    //check if there's space for new fertilizer, if yes, add
    %curr = validateStorageValue(getDataIDArrayValue(%dataID, 1));
    %db = getStackTypeDatablock("Compost", 1);
    %max = %brick.getStorageMax(%db);
    if (getWord(%curr, 1) < %max)
    {
        %addAmt = getMin(%max - getWord(%curr, 1), %amt);
        %amt -= %addAmt;
        setDataIDArrayValue(%dataID, 1, getStorageValue(%db, getWord(%curr, 2) + %addAmt, ""));
    }

    %amtAdded = %origAmt - %amt;
    if (%amtAdded > 0)
    {
        %count = %count - %amtAdded;
        setDataIDArrayTagValue(%dataID, "compostQueue", %count);
    }
    %brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime;

    %brick.updateStorageMenu(%dataID);
}

function processIntoFertilizer(%brick, %cl, %slot)
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

		%isProduce = isProduce(%cropType);
		if (!%isProduce)
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("You cannot process this into compost!", 1);
			return;
		}

		if (%pl.toolStackCount[%slot] < %max)
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("You need to have a full basket of produce to create compost!", 1);
			return;
		}
		%pl.tool[%slot] = "";
		%pl.toolStackCount[%slot] = 0;
		serverCmdUnuseTool(%cl);
		messageClient(%cl, 'MsgItemPickup', '', %slot, 0);

		%fertRange = ($FertCount_[%cropType] > 0 ? $FertCount_[%cropType] : $FertCount_Default);
		%rand = getRandom(getWord(%fertRange, 0), getWord(%fertRange, 1));
		
		%dataID = %brick.eventOutputParameter0_1;
		initializeStorage(%brick, %dataID);
		setDataIDArrayTagValue(%dataID, "compostQueue", getDataIDArrayTagValue(%dataID, "compostQueue") + %rand);

		%cl.centerprint("<color:ffffff>You started making <color:ffff00>" @ %rand @ "<color:ffffff> compost out of <color:ffff00>" @ %cropType @ "<color:ffffff>!", 3);
		%cl.schedule(100, centerprint, "<color:cccccc>You started making <color:ffff00>" @ %rand @ "<color:cccccc> compost out of <color:ffff00>" @ %cropType @ "<color:ffffff>!", 3);
		return;
	}
	else
	{
		%cl.centerprint("You cannot process this into compost!", 1);
		return;
	}
}

function compostBinInfo(%brick, %pl)
{
	if (%pl.client.lastMessagedCompostBinInfo + 5 < $Sim::Time)
	{
		messageClient(%pl.client, '', "\c3Drop (Ctrl-W) a full basket of produce in the bin to make compost! Amount varies with produce type.", 2);
		%pl.client.lastMessagedCompostBinInfo = $Sim::Time;
	}
}



///////////////
//Compost Bin//
///////////////

datablock fxDTSBrickData(brickCompostBinData)
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Compost Bin";

	brickFile = "./resources/fertilizer/compostBin.blb";

	iconName = "Add-Ons/Server_Farming/icons/compost_bin";

	cost = 0;
	isProcessor = 1;
	isCompostBin = 1;
	isStorageBrick = 1;
	storageSlotCount = 1;
	itemStackCount = 0;
	storageMultiplier = 4;
	processorFunction = "processIntoFertilizer";
	activateFunction = "compostBinInfo";
	placerItem = "CompostBinItem";
	keepActivate = 1;

	tickTime = 10;
	tickAmt = 1;
};

datablock fxDTSBrickData(brickLargeCompostBinData)
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Large Compost Bin";

	brickFile = "./resources/fertilizer/largecompostBin.blb";

	iconName = "Add-Ons/Server_Farming/icons/large_compost_bin";

	cost = 0;
	isProcessor = 1;
	isCompostBin = 1;
	isStorageBrick = 1;
	storageSlotCount = 1;
	itemStackCount = 0;
	storageMultiplier = 12;
	processorFunction = "processIntoFertilizer";
	activateFunction = "compostBinInfo";
	placerItem = "LargeCompostBinItem";
	keepActivate = 1;

	tickTime = 8;
	tickAmt = 2;
};



///////////////
//Placer Item//
///////////////

datablock ItemData(CompostBinItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Compost Bin";
	image = "CompostBinBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/compost_bin";
};

datablock ShapeBaseImageData(CompostBinBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = CompostBinItem;
	
	doColorshift = true;
	colorShiftColor = CompostBinItem.colorShiftColor;

	toolTip = "Places a Compost Bin";
	loopTip = "Converts produce into fertilizer";
	placeBrick = "brickCompostBinData";
};

function CompostBinBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function CompostBinBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function CompostBinBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function CompostBinBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(LargeCompostBinItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Large Compost Bin";
	image = "LargeCompostBinBrickImage";
	colorShiftColor = "0.5 0 0 1";

	iconName = "Add-ons/Server_Farming/icons/large_compost_bin";
};

datablock ShapeBaseImageData(LargeCompostBinBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = LargeCompostBinItem;
	
	doColorshift = true;
	colorShiftColor = LargeCompostBinItem.colorShiftColor;

	toolTip = "Places a Large Compost Bin";
	loopTip = "Converts produce into fertilizer";
	placeBrick = "brickLargeCompostBinData";
};

function LargeCompostBinBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function LargeCompostBinBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function LargeCompostBinBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function LargeCompostBinBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}

////////
//Item//
////////

$Stackable_Fertilizer_StackedItem0 = "FertilizerBag0Item 20";
$Stackable_Fertilizer_StackedItem1 = "FertilizerBag1Item 40";
$Stackable_Fertilizer_StackedItem2 = "FertilizerBag2Item 80";
$Stackable_Fertilizer_StackedItemTotal = 3;

datablock ItemData(FertilizerBag0Item : HammerItem)
{
	shapeFile = "./resources/fertilizer/fertilizerBag0.dts";
	uiName = "Fertilizer Bag";
	image = "FertilizerBag0Image";
	colorShiftColor = "0.5 0.3 0 1";
	doColorShift = true;

	iconName = "Add-ons/Server_Farming/icons/Fertilizer_Bag";

	isStackable = 1;
	stackType = "Fertilizer";
};

datablock ShapeBaseImageData(FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizer/fertilizerBag0.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = FertilizerBag0Item.colorShiftColor;

	item = "FertilizerBag0Item";

	armReady = 1;

	offset = "-0.1 0.3 -0.1";

	toolTip = "Make plants grow faster, chance of shiny";

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

datablock ItemData(FertilizerBag1Item : FertilizerBag0Item)
{
	shapeFile = "./resources/fertilizer/fertilizerBag1.dts";
	image = "FertilizerBag1Image";
	uiName = "Half Fertilizer Bag";

	iconName = "Add-ons/Server_Farming/icons/Fertilizer_Bag_Half";
};

datablock ShapeBaseImageData(FertilizerBag1Image : FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizer/fertilizerBag1.dts";
	item = "FertilizerBag1Item";

	offset = "-0.05 0.3 -0.3";
};

datablock ItemData(FertilizerBag2Item : FertilizerBag0Item)
{
	shapeFile = "./resources/fertilizer/fertilizerBag2.dts";
	image = "FertilizerBag2Image";
	uiName = "Full Fertilizer Bag";

	iconName = "Add-ons/Server_Farming/icons/Fertilizer_Bag_Full";
};

datablock ShapeBaseImageData(FertilizerBag2Image : FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizer/fertilizerBag2.dts";
	item = "FertilizerBag2Item";

	offset = "-0.05 0.3 -0.3";
};


function FertilizerBag0Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function FertilizerBag1Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function FertilizerBag2Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}



function FertilizerBag0Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function FertilizerBag1Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function FertilizerBag2Image::onLoop(%this, %obj, %slot)
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
		%cl.centerprint("<just:right><color:ffff00>-Fertilizer Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}








$Stackable_Compost_StackedItem0 = "CompostBag0Item 20";
$Stackable_Compost_StackedItem1 = "CompostBag1Item 40";
$Stackable_Compost_StackedItem2 = "CompostBag2Item 80";
$Stackable_Compost_StackedItemTotal = 3;

datablock ItemData(CompostBag0Item : HammerItem)
{
	shapeFile = "./resources/Compost/CompostBag0.dts";
	uiName = "Compost Bag";
	image = "CompostBag0Image";
	colorShiftColor = "0.73 0.53 0.25 1";
	doColorShift = true;

	iconName = "Add-ons/Server_Farming/icons/Compost_Bag";

	isStackable = 1;
	stackType = "Compost";
};

datablock ShapeBaseImageData(CompostBag0Image)
{
	shapeFile = "./resources/Compost/CompostBag0.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = CompostBag0Item.colorShiftColor;

	item = "CompostBag0Item";

	armReady = 1;

	offset = "-0.1 0.3 -0.1";

	toolTip = "Add 2 nitrogen to soil";

	fertilizerNitrogen = 2;

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
	stateTimeoutValue[3] = 0.4;
	stateWaitForTimeout[3] = true;
};

datablock ItemData(CompostBag1Item : CompostBag0Item)
{
	shapeFile = "./resources/Compost/CompostBag1.dts";
	image = "CompostBag1Image";
	uiName = "Half Compost Bag";

	iconName = "Add-ons/Server_Farming/icons/Compost_Bag_Half";
};

datablock ShapeBaseImageData(CompostBag1Image : CompostBag0Image)
{
	shapeFile = "./resources/Compost/CompostBag1.dts";
	item = "CompostBag1Item";

	offset = "-0.05 0.3 -0.3";
};

datablock ItemData(CompostBag2Item : CompostBag0Item)
{
	shapeFile = "./resources/Compost/CompostBag2.dts";
	image = "CompostBag2Image";
	uiName = "Full Compost Bag";

	iconName = "Add-ons/Server_Farming/icons/Compost_Bag_Full";
};

datablock ShapeBaseImageData(CompostBag2Image : CompostBag0Image)
{
	shapeFile = "./resources/Compost/CompostBag2.dts";
	item = "CompostBag2Item";

	offset = "-0.05 0.3 -0.3";
};


function CompostBag0Image::onFire(%this, %obj, %slot)
{
	fertilizeDirt(%this, %obj, %slot);
}

function CompostBag1Image::onFire(%this, %obj, %slot)
{
	fertilizeDirt(%this, %obj, %slot);
}

function CompostBag2Image::onFire(%this, %obj, %slot)
{
	fertilizeDirt(%this, %obj, %slot);
}



function CompostBag0Image::onLoop(%this, %obj, %slot)
{
	CompostLoop(%this, %obj);
}

function CompostBag1Image::onLoop(%this, %obj, %slot)
{
	CompostLoop(%this, %obj);
}

function CompostBag2Image::onLoop(%this, %obj, %slot)
{
	CompostLoop(%this, %obj);
}

function CompostLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right><color:ffff00>-Compost Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}








$Stackable_Phosphate_StackedItem0 = "PhosphateBag0Item 20";
$Stackable_Phosphate_StackedItem1 = "PhosphateBag1Item 40";
$Stackable_Phosphate_StackedItem2 = "PhosphateBag2Item 80";
$Stackable_Phosphate_StackedItemTotal = 3;

datablock ItemData(PhosphateBag0Item : HammerItem)
{
	shapeFile = "./resources/Phosphate/PhosphateBag0.dts";
	uiName = "Phosphate Bag";
	image = "PhosphateBag0Image";
	colorShiftColor = "0.54 0.48 0.42 1";
	doColorShift = true;

	iconName = "Add-ons/Server_Farming/icons/Phosphate_Bag";

	isStackable = 1;
	stackType = "Phosphate";
};

datablock ShapeBaseImageData(PhosphateBag0Image)
{
	shapeFile = "./resources/Phosphate/PhosphateBag0.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = PhosphateBag0Item.colorShiftColor;

	item = "PhosphateBag0Item";

	armReady = 1;

	offset = "-0.1 0.3 -0.1";

	toolTip = "Add 5 phosphate to soil";

	fertilizerPhosphate = 5;

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
	stateTimeoutValue[3] = 0.4;
	stateWaitForTimeout[3] = true;
};

datablock ItemData(PhosphateBag1Item : PhosphateBag0Item)
{
	shapeFile = "./resources/Phosphate/PhosphateBag1.dts";
	image = "PhosphateBag1Image";
	uiName = "Half Phosphate Bag";

	iconName = "Add-ons/Server_Farming/icons/Phosphate_Bag_Half";
};

datablock ShapeBaseImageData(PhosphateBag1Image : PhosphateBag0Image)
{
	shapeFile = "./resources/Phosphate/PhosphateBag1.dts";
	item = "PhosphateBag1Item";

	offset = "-0.05 0.3 -0.3";
};

datablock ItemData(PhosphateBag2Item : PhosphateBag0Item)
{
	shapeFile = "./resources/Phosphate/PhosphateBag2.dts";
	image = "PhosphateBag2Image";
	uiName = "Full Phosphate Bag";

	iconName = "Add-ons/Server_Farming/icons/Phosphate_Bag_Full";
};

datablock ShapeBaseImageData(PhosphateBag2Image : PhosphateBag0Image)
{
	shapeFile = "./resources/Phosphate/PhosphateBag2.dts";
	item = "PhosphateBag2Item";

	offset = "-0.05 0.3 -0.3";
};


function PhosphateBag0Image::onFire(%this, %obj, %slot)
{
	fertilizeDirt(%this, %obj, %slot);
}

function PhosphateBag1Image::onFire(%this, %obj, %slot)
{
	fertilizeDirt(%this, %obj, %slot);
}

function PhosphateBag2Image::onFire(%this, %obj, %slot)
{
	fertilizeDirt(%this, %obj, %slot);
}



function PhosphateBag0Image::onLoop(%this, %obj, %slot)
{
	PhosphateLoop(%this, %obj);
}

function PhosphateBag1Image::onLoop(%this, %obj, %slot)
{
	PhosphateLoop(%this, %obj);
}

function PhosphateBag2Image::onLoop(%this, %obj, %slot)
{
	PhosphateLoop(%this, %obj);
}

function PhosphateLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right><color:ffff00>-Phosphate Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}