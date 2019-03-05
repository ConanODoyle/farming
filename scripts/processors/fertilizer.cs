$fertCount = 6;

function fertilizeCrop(%img, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyePoint();
	%end = vectorAdd(vectorScale(%obj.getEyeVector(), 4), %start);
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%brick = getWord(%ray, 0);

	if (%brick.getDatablock().isPlant && !%brick.getDatablock().isTree)
	{
		%brick = %brick.getDownBrick(0);
	}

	if (!isObject(%brick) || !(%brick.getDatablock().isTree || %brick.getDatablock().isDirt))
	{
		return;
	}

	if (%brick.getDatablock().isDirt)
	{
		%numGrown = 0;
		%numCrops = 0;
		for (%i = 0; %i < %brick.getNumUpBricks(); %i++)
		{
			%crop = %brick.getUpBrick(%i);

			if (!%crop.getDatablock().isPlant || %crop.getDatablock().isTree)
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
			else if ($Farming::Crops::PlantData_[%type, %stage, "timePerTick"] <= 1)
			{
				%obj.client.centerprint("This plant already is fully grown!");
				%numGrown++;
				%numCrops++;
				continue;
			}
			
			%numCrops++;

			%hitloc = %crop.getPosition();
			%p = new Projectile() { dataBlock = PushBroomProjectile; initialPosition = %hitloc; };
			%p.setScale("0.5 0.5 0.5");
			%p.explode();

			%crop.growTick += %img.bonusGrowTicks;
			%crop.nextGrow -= %img.bonusGrowTime;

			if (!isObject(%crop.emitter))
			{
				if (getRandom() < 0.010)
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
		if (%numGrown == %numCrops)
		{
			%obj.client.centerprint("All of these plants are already fully grown!");
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
		else if ($Farming::Crops::PlantData_[%type, %stage, "timePerTick"] <= 1)
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
			if (getRandom() < 0.015)
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
	//plant successful, update item

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
	for (%i = 0; %i < $Stackable_[%type, "stackedItemTotal"]; %i++)
	{
		%currPair = $Stackable_[%type, "stackedItem" @ %i];
		// talk(%currPair);
		if (%count <= getWord(%currPair, 1))
		{
			%bestItem = getWord(%currPair, 0);
			break;
		}
	}

	messageClient(%obj.client, 'MsgItemPickup', '', %slot, %bestItem.getID());
	%obj.tool[%slot] = %bestItem.getID();
	%obj.mountImage(%imageSlot, %bestItem.image);


	%item = %img.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<color:ffff00>-Fertilizer Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}

function createFertilizer(%brick, %client, %count)
{
	%top = vectorAdd(%brick.getPosition(), "0 0 " @ %brick.getDatablock().brickSizeZ * 0.1);
	%vel = "0 0 5";

	%item = new Item(GeneratedFertilizer)
	{
		dataBlock = FertilizerBag0Item;
		count = getMax(1, %count);
		harvestedBG = getBrickgroupFromObject(%brick);
	};
	MissionCleanup.add(%item);
	%item.setTransform(%top SPC getWords(%brick.getTransform(), 3, 6));
	%item.setVelocity(%vel);

	return %item;
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
			%cl.centerprint("You cannot process this into fertilizer!", 1);
			return;
		}

		if (%pl.toolStackCount[%slot] < %max)
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("You need to have a full basket of produce to create fertilizer!", 1);
			return;
		}
		%pl.tool[%slot] = "";
		%pl.toolStackCount[%slot] = 0;
		serverCmdUnuseTool(%cl);
		messageClient(%cl, 'MsgItemPickup', '', %slot, 0);

		createFertilizer(%brick, %cl, $fertCount);
		%cl.centerprint("<color:ffffff>You made <color:ffff00>" @ $fertCount @ "<color:ffffff> fertilizer out of <color:ffff00>" @ %cropType @ "<color:ffffff>!", 3);
		return;
	}
}

function compostBinInfo(%brick, %pl)
{
	%pl.client.centerprint("<color:ffffff>Drop (Ctrl-W) a full basket of produce in here to make " @ $fertCount @ " fertilizer!", 2);
}


///////////////
//Compost Bin//
///////////////

datablock fxDTSBrickData(brickCompostBinData)
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Compost Bin";

	brickFile = "./resources/compostBin.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/compost_bin";

	cost = 0;
	isProcessor = 1;
	processorFunction = "processIntoFertilizer";
	activateFunction = "compostBinInfo";
	placerItem = "CompostBinItem";
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

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
	
	cost = 800;
};

datablock ShapeBaseImageData(CompostBinBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";

	item = CompostBinItem;
	
	doColorshift = true;
	colorShiftColor = CompostBinItem.colorShiftColor;

	toolTip = "Places a Compost Bin";
	placeBrick = "brickCompostBinData";
};

function FlowerpotBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function FlowerpotBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function FlowerpotBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function FlowerpotBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}

////////
//Item//
////////

$Stackable_Fertilizer_StackedItem0 = "FertilizerBag0Item 20";
$Stackable_Fertilizer_StackedItem1 = "FertilizerBag1Item 40";
$Stackable_Fertilizer_StackedItem2 = "FertilizerBag2Item 60";
$Stackable_Fertilizer_StackedItemTotal = 3;

datablock ItemData(FertilizerBag0Item : HammerItem)
{
	shapeFile = "./resources/fertilizerBag0.dts";
	uiName = "Fertilizer Bag";
	image = "FertilizerBag0Image";
	colorShiftColor = "0.5 0.3 0 1";
	doColorShift = true;

	iconName = "";

	isStackable = 1;
	stackType = "Fertilizer";
};

datablock ShapeBaseImageData(FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag0.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = FertilizerBag0Item.colorShiftColor;

	item = "FertilizerBag0Item";

	armReady = 1;

	offset = "";

	toolTip = "Make plants grow, chance for shiny";

	bonusGrowTicks = 2; //bonus grow tick per use (does not consume water)
	bonusGrowTime = 5; //reduction in seconds to next grow tick

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
	shapeFile = "./resources/fertilizerBag1.dts";
	image = "FertilizerBag1Image";
	uiName = "Half Fertilizer Bag";
};

datablock ShapeBaseImageData(FertilizerBag1Image : FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag1.dts";
	item = "FertilizerBag1Item";
};

datablock ItemData(FertilizerBag2Item : FertilizerBag0Item)
{
	shapeFile = "./resources/fertilizerBag2.dts";
	image = "FertilizerBag2Image";
	uiName = "Full Fertilizer Bag";
};

datablock ShapeBaseImageData(FertilizerBag2Image : FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag2.dts";
	item = "FertilizerBag2Item";
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
		%cl.centerprint("<color:ffff00>-Fertilizer Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}
