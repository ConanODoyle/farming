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
	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot)
	{
		if (%storageObj.getDatablock().isCompostBin)
		{
			return 2;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot);
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
			createCompost(%brick);
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
				%obj.client.centerprint(%crop.getGroup().name @ "\c0 does not trust you enough to do that!", 1);
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
			%obj.client.centerprint(%crop.getGroup().name @ "\c0 does not trust you enough to do that!", 1);
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
		%cl.centerprint("<just:right>\c3-Fertilizer Bag " @ %obj.currTool @ "- \nAmount\c6: " @ %count @ " ", 1);
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
		%canAddNit = (%img.fertilizerNitrogen + 0) != 0 && getWord(%nutrients, 0) < %brickDB.maxNutrients;
		%canAddPho = (%img.fertilizerPhosphate + 0) != 0 && getWord(%nutrients, 1) < %brickDB.maxNutrients;
		if (!%canAddNit && !%canAddPho)
		{
			if (%cl.lastMessagedNutrientsMaxedOut != %brick)
			{
				messageClient(%cl, '', "Nutrients are maxed out! Use a shovel to remove and recover some nutrients.");
				%cl.lastMessagedNutrientsMaxedOut = %brick;
			}
			return;
		}
		%nitAdd = %img.fertilizerNitrogen + 0;
		%phoAdd = %img.fertilizerPhosphate + 0;
		%weedAdd = %img.fertilizerWeedkiller + 0;

		%finalNutrients = vectorAdd(%nutrients, %nitAdd SPC %phoAdd SPC %weedAdd);
		%finalNit = getMin(getWord(%finalNutrients, 0), %brickDB.maxNutrients);
		%finalPho = getMin(getWord(%finalNutrients, 1), %brickDB.maxNutrients);
		%finalWeedkiller = getMin(getWord(%finalNutrients, 2), %brickDB.maxWeedkiller);
		
		%finalNutrients = %finalNit SPC %finalPho SPC %finalWeedkiller;
		%brick.setNutrients(%finalNit, %finalPho, %finalWeedkiller);

		//increase weed chance on the dirt
		// %brick.fertilizerWeedModifier += 1;
	}

	//fertilization successful, update item

	%count = %obj.toolStackCount[%obj.currTool]--;
	%slot = %obj.currTool;
	%type = %obj.tool[%slot].stackType;
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
		%cl.centerprint("<just:right>\c3-" @ %type @ " Bag " @ %obj.currTool @ "- \nAmount\c6: " @ %count @ " ", 1);
	}
}

function createCompost(%brick)
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
	if (getWord(%curr, 2) < %max)
	{
		%addAmt = getMin(%max - getWord(%curr, 2), %amt);
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

function processIntoCompost(%brick, %cl, %slot)
{
	if (getTrustLevel(%brick, %cl) < 1)
	{
		serverCmdUnuseTool(%cl);
		%cl.centerprint(getBrickgroupFromObject(%brick).name @ "\c0 does not trust you enough to do that!", 1);
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

		%cl.centerprint("\c6You started making \c3" @ %rand @ "\c6 compost out of \c3" @ %cropType @ "\c6!", 3);
		%cl.schedule(100, centerprint, "<color:cccccc>You started making \c3" @ %rand @ "<color:cccccc> compost out of \c3" @ %cropType @ "\c6!", 3);
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
	storageMultiplier = 1;
	processorFunction = "processIntoCompost";
	activateFunction = "compostBinInfo";
	placerItem = "CompostBinItem";
	callOnActivate = 1;

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
	storageMultiplier = 3;
	processorFunction = "processIntoCompost";
	activateFunction = "compostBinInfo";
	placerItem = "LargeCompostBinItem";
	callOnActivate = 1;

	tickTime = 8;
	tickAmt = 2;
};

datablock fxDTSBrickData(brickFertilizerMixerData)
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Fertilizer Mixer";

	brickFile = "./resources/fertilizer/Mixer.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	processorFunction = "addFertilizerIngredients";
	placerItem = "FertilizerMixerItem";
	callOnActivate = 1;

	isPoweredProcessor = 1;
	hasCustomMenu = 1;
	energyUse = 50;
	refineRate = 5;
	powerFunction = "processIntoFertilizer";

	isRecipeProcessor = 1;
	isFertilizerMixer = 1;

	isStorageBrick = 1;
	storageSlotCount = 3;
	itemStackCount = 0;
	storageMultiplier = 2;

	musicRange = 50;
	musicDescription = "AudioMusicLooping3d";
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



datablock ItemData(FertilizerMixerItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Fertilizer Mixer";
	image = "FertilizerMixerBrickImage";
	colorShiftColor = "0.5 0 0 1";

	iconName = "";
};

datablock ShapeBaseImageData(FertilizerMixerBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = FertilizerMixerItem;
	
	doColorshift = true;
	colorShiftColor = FertilizerMixerItem.colorShiftColor;

	toolTip = "Places a Fertilizer Mixer";
	loopTip = "Converts phosphate and compost into fertilizer";
	placeBrick = "brickFertilizerMixerData";
};



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
		%cl.centerprint("<just:right>\c3-Fertilizer Bag " @ %obj.currTool @ "- \nAmount\c6: " @ %count @ " ", 1);
	}
}








$Stackable_Compost_StackedItem0 = "CompostBag0Item 25";
$Stackable_Compost_StackedItem1 = "CompostBag1Item 50";
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
	stateTransitionOnTimeout[3]	= "LoopA";
	stateTimeoutValue[3] = 0.2;
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
		%cl.centerprint("<just:right>\c3-Compost Bag " @ %obj.currTool @ "- \nAmount\c6: " @ %count @ " ", 1);
	}
}








$Stackable_Phosphate_StackedItem0 = "PhosphateBag0Item 25";
$Stackable_Phosphate_StackedItem1 = "PhosphateBag1Item 50";
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
	stateTransitionOnTimeout[3]	= "LoopA";
	stateTimeoutValue[3] = 0.2;
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
		%cl.centerprint("<just:right>\c3-Phosphate Bag " @ %obj.currTool @ "- \nAmount\c6: " @ %count @ " ", 1);
	}
}








package FertilizerMixer
{
	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot)
	{
		//reject inserting if we've already determined we can't accept more
		if (%storageObj.dataBlock.isRecipeProcessor && !%storageObj.isAcceptingIngredients)
		{
			return 3;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID); //call parent since poweredProcessor has its own package
		%db = %brick.dataBlock;
		if (%db.isFertilizerMixer)
		{
			if (%brick.isPoweredOn())
			{
				%energyUse = %db.energyUse;
			}
			else
			{
				%energyUse = 0;
				%db.devicePower = 0;
			}

			%power = mFloor(%brick.devicePower * 100);
			if (%power < 50) %color = "\c0";
			else if (%power < 100) %color = "\c3";
			else %color = "\c2";
			
			%brick.centerprintMenu.menuOptionCount = 6; //add on/off toggle
			%brick.centerprintMenu.menuOption[0] = %brick.centerprintMenu.menuOption[0] SPC "(Input)";
			// %brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuOption[1] = %brick.centerprintMenu.menuOption[1] SPC "(Input)";
			// %brick.centerprintMenu.menuFunction[1] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuOption[2] = %brick.centerprintMenu.menuOption[2] SPC "(Output)";
			%brick.centerprintMenu.menuOption[3] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuFunction[3] = "togglePower";
			%brick.centerprintMenu.menuOption[4] = "Progress: " @ %brick.deviceProgress @ "% | Current Power: " @ %color @ mFloor(%brick.devicePower * 100) @ "%";
			%brick.centerprintMenu.menuOption[5] = "Uses " @ %energyUse @ " power per tick";
		}
		return %ret;
	}

	function fxDTSBrick::getEnergyUse(%brick)
	{
		if (%brick.getDatablock().isFertilizerMixer)
		{
			%db = %brick.getDatablock();
			%dataID = %brick.eventOutputParameter0_1;

			// check if it can process recipe - if not, draw no power
			if (!canCreateFertilizer(%brick))
			{
				return 0;
			}

			%brick.updateStorageMenu(%dataID);
		}
		return parent::getEnergyUse(%brick);
	}
};
activatePackage(FertilizerMixer);

function addFertilizerIngredients(%brick, %cl, %slot)
{
	if (getTrustLevel(%brick, %cl) < 1)
	{
		serverCmdUnuseTool(%cl);
		%cl.centerprint(getBrickgroupFromObject(%brick).name @ "\c0 does not trust you enough to do that!", 1);
		return;
	}

	%pl = %cl.player;
	%item = %pl.tool[%slot];
	//phosphate in slot 0, compost in slot 1
	if (%item.isStackable && %item.stackType !$= "")
	{
		switch$ (%item.stackType)
		{
			case "Compost": %compost = 1;
			case "Phosphate": %phosphate = 1;
			default:
				serverCmdUnuseTool(%cl);
				%cl.centerprint("This fertilizer mixer only accepts compost and phosphate!", 1);
				return;
		}
		
		//check for space for ingredients - only insert that much
		%dataID = %brick.eventOutputParameter[0, 1];
		%max = %brick.getStorageMax(%item);
		%slot0 = validateStorageValue(getDataIDArrayValue(%dataID, 1));
		%slot1 = validateStorageValue(getDataIDArrayValue(%dataID, 2));
		%itemCount0 = getField(%slot0, 2);
		%itemCount1 = getField(%slot1, 2);
		%space0 = getMin(%pl.toolStackCount[%slot], %max - %itemCount0);
		%space1 = getMin(%pl.toolStackCount[%slot], %max - %itemCount1);

		if (%compost)
		{
			if (%space0 <= 0)
			{
				return;
			}
			%pickedSlot = 0;
			%space = %space0;
		}

		if (%phosphate)
		{
			if (%space1 <= 0)
			{
				return;
			}
			%pickedSlot = 1;
			%space = %space1;
		}
		
		%brick.isAcceptingIngredients = 1;
		%success = %brick.insertIntoStorage(%dataID, 
										%item, 
										%space, 
										"",
										%pickedSlot);
		%brick.isAcceptingIngredients = 0;
		if (%success == 0) //complete insertion
		{
			%pl.toolStackCount[%slot] -= %space;
			if (%pl.toolStackCount[%slot] <= 0)
			{
				%pl.tool[%slot] = 0;
				%pl.toolStackCount[%slot] = 0;
				messageClient(%cl, 'MsgItemPickup', "", %slot, 0);
				if (%pl.currTool == %slot)
				{
					%pl.unmountImage(0);
				}
			}
			return;
		}
		else if (%success == 1) //partial insertion
		{
			%pl.toolStackCount[%slot] = getWord(%success, 1);
			%db = getStackTypeDatablock(%pl.tool[%slot].stackType, getWord(%success, 1)).getID();
			messageClient(%cl, 'MsgItemPickup', "", %slot, %db);
			%pl.tool[%slot] = %db;
			if (%pl.currTool == %slot)
			{
				%pl.mountImage(%db.image, 0);
			}
			return;
		}
	}
	else
	{
		%cl.centerprint("You cannot process this into fertilizer!", 1);
		return;
	}
}

$Fertilizer::InputCompostAmount = 5;
$Fertilizer::InputPhosphateAmount = 5;
$Fertilizer::OutputAmount = 1;

function canCreateFertilizer(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%inputCompost = getField(validateStorageValue(getDataIDArrayValue(%dataID, 1)), 2);
	%inputPhosphate = getField(validateStorageValue(getDataIDArrayValue(%dataID, 2)), 2);
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 3));
	%maxOutput = %brick.getStorageMax(getStackTypeDatablock("Fertilizer", 1));
	%outputCount = getField(%output, 2);
	%outputSpace = %maxOutput - %outputCount;

	if (%inputCompost < $Fertilizer::InputCompostAmount || %inputPhosphate < $Fertilizer::InputPhosphateAmount
		|| %outputSpace < $Fertilizer::OutputAmount)
	{
		return false;
	}
	return true;
}

function createFertilizer(%brick)
{
	%dataID = %brick.eventOutputParameter[0, 1];
	%inputCompost = getField(validateStorageValue(getDataIDArrayValue(%dataID, 1)), 2);
	%inputPhosphate = getField(validateStorageValue(getDataIDArrayValue(%dataID, 2)), 2);
	%output = validateStorageValue(getDataIDArrayValue(%dataID, 3));
	%outputCount = getField(%output, 2);

	if (%inputCompost - $fertilizer::InputCompostAmount <= 0)
	{
		%newInputC = "";
	}
	else
	{
		%newInputC = getStorageValue("Compost", %inputCompost - $Fertilizer::InputCompostAmount);
	}

	if (%inputPhosphate - $fertilizer::InputPhosphateAmount <= 0)
	{
		%newInputP = "";
	}
	else
	{
		%newInputP = getStorageValue("Phosphate", %inputPhosphate - $Fertilizer::InputPhosphateAmount);
	}
	%newOutput = getStorageValue("Fertilizer", %outputCount + $Fertilizer::OutputAmount);
	setDataIDArrayValue(%dataID, 1, %newInputC);
	setDataIDArrayValue(%dataID, 2, %newInputP);
	setDataIDArrayValue(%dataID, 3, %newOutput);
	%brick.updateStorageMenu(%dataID);
}

function processIntoFertilizer(%brick, %powerRatio)
{
	%db = %brick.getDatablock();
	%dataID = %brick.eventOutputParameter0_1;
	%rate = mFloatLength(%powerRatio * %db.refineRate, 1);
	%brick.devicePower = %powerRatio;

	if (canCreateFertilizer(%brick) && %brick.deviceProgress >= 100)
	{
		createFertilizer(%brick);
		%brick.deviceProgress = 0;
	}
	else if (canCreateFertilizer(%brick))
	{
		%brick.deviceProgress = getMin(100, %brick.deviceProgress + %rate);
	}
	else
	{
		%brick.deviceProgress = getMax(%brick.deviceProgress - 10, 0);
	}
}