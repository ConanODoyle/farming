////////////
//Function//
////////////
function killWeeds(%img, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyePoint();
	%end = vectorAdd(vectorScale(%obj.getEyeVector(), 4), %start);
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%brick = getWord(%ray, 0);

	if (%brick.getDatablock().isPlant)
	{
		%brick = %brick.getDownBrick(0);
	}

	if (!isObject(%brick) || !%brick.getDatablock().isDirt)
	{
		return;
	}

	if (%brick.getDatablock().isDirt)
	{
		%numWeeds = 0;
		%upBrickCount = %brick.getNumUpBricks();
		for (%i = 0; %i < %upBrickCount; %i++)
		{
			%checkBrick = %brick.getUpBrick(%i);
			if (%checkBrick.getDatablock().isWeed)
			{
				%weed[%numWeeds] = %checkBrick;
				%numWeeds++;
			}
		}

		for (%i = 0; %i < %numWeeds; %i++)
		{
			%hitloc = %weed[%i].getPosition();
			%p = new Projectile() { dataBlock = PushBroomProjectile; initialPosition = %hitloc; }; // replace this with a nicer emitter
			%p.setScale("0.5 0.5 0.5");
			%p.explode();

			%weed[%i].delete();
		}
	}

	// set weed chance on the dirt to 0
	if (%brick.getDatablock().isDirt)
	{
		%brick.fertilizerWeedModifier = 0;

		%ticksAdded = mFloor(%img.weedRepelBaseDuration * mPow(weedRepelDiminishFactor, %brick.weedImmunityTicks/%img.weedRepelBaseDuration));
		%brick.weedImmunityTicks += %ticksAdded;
		%obj.client.centerprint("\c6You added \c2" @ convTime(%ticksAdded * $WeedTickLength) @ "\c6 of weed killer!", 1);
	}

	// de-weeded, update the item

	%count = %obj.toolStackCount[%obj.currTool]--;
	%slot = %obj.currTool; // why do we do this? %slot doesn't get changed, does it?
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
}

////////
//Item//
////////
$Stackable_WeedKiller_StackedItem0 = "WeedKiller0Item 10";
$Stackable_WeedKiller_StackedItem1 = "WeedKiller1Item 20";
$Stackable_WeedKiller_StackedItem2 = "WeedKiller2Item 30";
$Stackable_WeedKiller_StackedItemTotal = 3;

datablock ItemData(WeedKiller0Item : HammerItem)
{
	shapeFile = "./resources/WeedKillerItem.dts";
	uiName = "Weed Killer";
	image = "WeedKiller0Image";
	colorShiftColor = "1 1 1 1";
	doColorShift = true;

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag";

	isStackable = 1;
	stackType = "WeedKiller";
	cost = 5;
};

datablock ShapeBaseImageData(WeedKiller0Image)
{
	shapeFile = "./resources/WeedKillerImage.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = WeedKiller0Item.colorShiftColor;

	item = "WeedKiller0Item";

	armReady = 1;

	offset = "";

	toolTip = "Kill weeds, prevent them for a set time";

	weedRepelBaseDuration = 120; // ticks - 10 minutes @ 66ms/tick
	weedRepelDiminishFactor = 0.5; // amount to diminish returns by per base duration
	// so for example: we have 9091 ticks left of repellent on our dirt
	// we use the weedkiller
	// diminishAmount = ticksRemaining/weedRepelBaseDuration = 9091/9091 = 1 base duration
	// so we add weedRepelBaseDuration * mPow(weedRepelDiminishFactor, diminishAmount)
	// to the weed repel ticks left on the dirt, which here is 0.5 * 9091 = 4546 (rounded)

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	Statename[1] = "LoopA";
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

datablock ItemData(WeedKiller1Item : WeedKiller0Item)
{
	shapeFile = "./resources/WeedKillerItem.dts";
	image = "WeedKiller1Image";
	uiName = "Half Weed Killer";

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag_Half";
};

datablock ShapeBaseImageData(WeedKiller1Image : WeedKiller0Image)
{
	shapeFile = "./resources/WeedKillerImage.dts";
	item = "WeedKiller1Item";
};

datablock ItemData(WeedKiller2Item : WeedKiller0Item)
{
	shapeFile = "./resources/WeedKillerItem.dts";
	image = "WeedKiller2Image";
	uiName = "Full Weed Killer";

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag_Full";
};

datablock ShapeBaseImageData(WeedKiller2Image : WeedKiller0Image)
{
	shapeFile = "./resources/WeedKillerImage.dts";
	item = "WeedKiller2Item";
};


function WeedKiller0Image::onFire(%this, %obj, %slot)
{
	killWeeds(%this, %obj, %slot);
}

function WeedKiller1Image::onFire(%this, %obj, %slot)
{
	killWeeds(%this, %obj, %slot);
}

function WeedKiller2Image::onFire(%this, %obj, %slot)
{
	killWeeds(%this, %obj, %slot);
}



function WeedKiller0Image::onLoop(%this, %obj, %slot)
{
	weedKillerLoop(%this, %obj);
}

function WeedKiller1Image::onLoop(%this, %obj, %slot)
{
	weedKillerLoop(%this, %obj);
}

function WeedKiller2Image::onLoop(%this, %obj, %slot)
{
	weedKillerLoop(%this, %obj);
}

function weedKillerLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<color:ffff00>-Weed Killer " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}
