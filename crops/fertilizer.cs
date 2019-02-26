$fertCount = 5;

function fertilizeCrop(%img, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyePoint();
	%end = vectorAdd(vectorScale(%obj.getEyeVector(), 4), %start);
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%brick = getWord(%ray, 0);

	if (!isObject(%brick) || !%brick.getDatablock().isPlant)
	{
		return;
	}

	%hitloc = getWords(%ray, 1, 3);
	%p = new Projectile() { dataBlock = PushBroomProjectile; initialPosition = %hitloc; };
	%p.setScale("0.5 0.5 0.5");
	%p.explode();

	%type = %brick.getDatablock().cropType;
	%stage = %brick.getDatablock().stage;

	if (getTrustLevel(%brick, %obj) < 1)
	{
		%obj.client.centerprint(%brick.getGroup().name @ "<color:ff0000> does not trust you enough to do that!", 1);
		return;
	}
	else if ($Farming::Crops::PlantData_[%type, %stage, "timePerTick"] <= 1)
	{
		%obj.client.centerprint("This plant already is fully grown!");
		return;
	}

	%brick.growTick += %img.bonusGrowTicks;
	%brick.nextGrow -= %img.bonusGrowTime;

	if (!isObject(%brick.emitter))
	{
		if (getRandom() < 0.015 + %obj.client.isDonator * 0.002)
		{
			%rand = getRandom();
			if (%rand < 0.05)
			{
				//gold plant
				%brick.setEmitter(goldenEmitter.getID());
				%type = "<color:faef00>Golden";
			}
			else if (%rand < 0.25)
			{
				//silver plant
				%brick.setEmitter(silverEmitter.getID());
				%type = "<color:fafafa>Silver";
			}
			else
			{
				//bronze plant
				%brick.setEmitter(bronzeEmitter.getID());
				%type = "<color:fafafa>Bronze";
			}

			if (isObject(%cl = %obj.client))
			{
				messageAll('MsgUploadStart', "<bitmap:base/client/ui/ci/star> \c3" @ 
					%cl.name @ "\c6 fertilized a " @ %type SPC %brick.getDatablock().cropType @ "\c6!");
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
		dataBlock = FertilizerBag2Item;
		sourceClient = %client;
		souceObject = %brick;
		count = getMax(1, %count);
	};
	MissionCleanup.add(%item);
	%item.setTransform(%top SPC getWords(%brick.getTransform(), 3, 6));
	%item.setVelocity(%vel);

	return %item;
}

package fertilizerMachine
{
	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%item = %pl.tool[%slot];
			if (%item.isStackable && %item.stackType !$= "")
			{
				%cropType = %item.stackType;
				%max = $Stackable_[%cropType, "stackedItemTotal"] - 1;
				%max = getWord($Stackable_[%cropType, "StackedItem" @ %max], 1);

				for (%i = 0; %i < $ProduceCount; %i++)
				{
					%produce = getWord($ProduceList_[%i], 0);
					if (%cropType $= %produce)
					{
						%isProduce = 1;
						break;
					}
				}
				if (!%isProduce)
				{
					return parent::serverCmdDropTool(%cl, %slot);
				}

				%start = %pl.getEyePoint();
				%end = vectorAdd(vectorScale(%pl.getEyeVector(), 6), %start);
				%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
				if (isObject(%hit) && %hit.getDatablock().isCompostBin && getTrustLevel(%hit, %pl) > 0)
				{
					if (%pl.toolStackCount[%slot] < %max)
					{
						serverCmdUnuseTool(%cl);
						%cl.centerprint("You need to have a full basket of produce to create fertilizer!");
						return;
					}
					%pl.tool[%slot] = "";
					%pl.toolStackCount[%slot] = 0;
					serverCmdUnuseTool(%cl);
					messageClient(%cl, 'MsgItemPickup', '', %slot, 0);

					// if (%cl.lastMadeFert < $Sim::Time - 2)
					// {
					// 	%cl.combo = 0;
					// }
					// %cl.lastMadeFert = $Sim::Time;
					createFertilizer(%hit, %cl, $fertCount + %cl.combo);
					%cl.centerprint("<color:ffffff>You made <color:ffff00>" @ $fertCount + %cl.combo @ "<color:ffffff> fertilizer out of <color:ffff00>" @ %cropType @ "<color:ffffff>!", 3);
					// %cl.combo++;
					// %cl.combo = getMin(%cl.combo, 7);
					return;
				}
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}

	function Player::activateStuff(%obj)
	{
		if (isObject(%cl = %obj.client))
		{
			%start = %obj.getEyeTransform();
			%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && ((%db = %hit.getDatablock()).isCompostBin))
			{
				%obj.client.centerprint("<color:ffffff>Drop (Ctrl-W) a full basket of produce in here to make " @ $fertCount @ " fertilizer!", 2);
			}
		}

		return parent::activateStuff(%obj);
	}
};
schedule(1000, 0, activatePackage, fertilizerMachine);

////////
//Item//
////////

$Stackable_Fertilizer_StackedItem0 = "FertilizerBag0Item 20";
$Stackable_Fertilizer_StackedItem1 = "FertilizerBag1Item 40";
$Stackable_Fertilizer_StackedItem2 = "FertilizerBag2Item 60";
$Stackable_Fertilizer_StackedItemTotal = 3;

datablock ItemData(FertilizerBag0Item : HammerItem)
{
	shapeFile = "./tools/fertilizerBag0.dts";
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
	shapeFile = "./tools/fertilizerBag0.dts";
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
	shapeFile = "./tools/fertilizerBag1.dts";
	image = "FertilizerBag1Image";
	uiName = "Half Fertilizer Bag";
};

datablock ShapeBaseImageData(FertilizerBag1Image : FertilizerBag0Image)
{
	shapeFile = "./tools/fertilizerBag1.dts";
	item = "FertilizerBag1Item";
};

datablock ItemData(FertilizerBag2Item : FertilizerBag0Item)
{
	shapeFile = "./tools/fertilizerBag2.dts";
	image = "FertilizerBag2Image";
	uiName = "Full Fertilizer Bag";
};

datablock ShapeBaseImageData(FertilizerBag2Image : FertilizerBag0Image)
{
	shapeFile = "./tools/fertilizerBag2.dts";
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
