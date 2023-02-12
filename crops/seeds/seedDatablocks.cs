exec("./datablocks.cs");

	// case 0: return 0;
	// case 1: return "overlap";
	// case 2: return "float";
	// case 3: return "stuck";
	// case 4: return "unstable";
	// case 5: return "buried";
	// default: return "forbidden";

function plantCrop(%image, %obj, %imageSlot, %remotePlacement)
{
	%cropType = %image.cropType;
	%brickDB = %image.cropBrick;
	%zOffset = %brickDB.brickSizeZ * 0.1;
	%isTree = %brickDB.isTree;
	%type = %image.item.stackType;
	%slot = %obj.currTool;
	%toolStackCount = %obj.toolStackCount[%obj.currTool];

	%expRequirement = getPlantData(%cropType, "experienceRequired");
	%expCost = getPlantData(%cropType, "experienceCost");
	%plantingLayer = getPlantData(%cropType, "plantingLayer");
	%plantingBoxDisabled = getPlantData(%cropType, "plantingBoxDisabled");
	%radius = getPlantData(%brickDB.cropType, "plantSpace");

	if (%obj.client.farmingExperience < %expCost)
	{
		pushSeedError(%obj, "You don't have enough experience to plant this crop!", 3);
		return 0;
	}

	if (%toolStackCount <= 0)
	{
		return 0;
	}

	if (%remotePlacement $= "")
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	}
	else
	{
		%start = vectorAdd(%remotePlacement, "0 0 0.1");
		%end = vectorAdd(%remotePlacement, "0 0 -0.1");
	}

	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);
	//check that we did hit a brick and confirm its normal
	if (!isObject(getWord(%ray, 0)) || vectorDist(getWords(%ray, 4, 6), "0 0 1") > 0.01)
	{
		return 0;
	}
	%hitLoc = getWords(%ray, 1, 3);

	//run raycasts to check the actual planting location, rather than just rely on the eye vector hit location
	%error = checkPlantLocationError(%hitLoc, %brickDB, %obj);

	switch (%error)
	{
		case 1: %errMsg = "Invalid position!";
		case 2: %errMsg = "You can only plant on dirt!";
		case 3: %errMsg = getField(%error, 1);
		case 4: %errMsg = getWord(%error, 1).getGroup().name @ " does not trust you enough to do that.";
	}

	if (%errMsg !$= "")
	{
		pushSeedError(%obj, %errMsg, 1);
		failPlantSeed(getBrickPlantPosition(%hitLoc, %brickDB), %brickDB);
		return 0;
	}
	%potFound = getWord(%error, 1);
	%planterFound = getWord(%error, 2);

	//get all the down bricks to calculate if its overlapping multiple lots
	%downBricks = getPlantDownBricks(%hitloc, %brickDB, %obj);
	for (%i = 0; %i < getWordCount(%downBricks); %i++)
	{
		%dirt = getWord(%downBricks, %i);
		%dirtLots = %dirt.getLotsUnderBrick();

		if (%currLotBLID $= "")
		{
			%currLotBLID = %dirtLots.getGroup().bl_id;
			continue;
		}

		if (%dirtLots $= "" || %dirtLots.getGroup().bl_id != %currLotBLID)
		{
			pushSeedError(%obj, "You cannot plant across lot boundaries!", 1);
			return failPlantSeed(getBrickPlantPosition(%hitLoc, %brickDB), %brickDB);
		}
	}


	//everything passed, time to make the brick
	//the plant error check and trust check is checked in getPlantDownBricks and checkPlantLocationError
	%pos = getBrickPlantPosition(%hitLoc, %brickDB);

	//check if this is in a greenhouse or not
	%light = lightRaycastCheck(roundToStudCenter(%hitLoc), %hit);
	%greenhouseFound = getWord(%light, 1);

	if (%greenhouseFound && %isTree)
	{
		if (isObject(%cl = %obj.client))
		{
			pushSeedError(%obj, "You cannot plant tall plants under a greenhouse!", 1);
		}
		return failPlantSeed(%pos, %brickDB);
	}

	//only do radius checks if not planting into a pot
	if (!%potFound)
	{
		%error = checkPlantRadiusError(%pos, %brickDB, %planterFound, %greenhouseFound);
		if (%error)
		{
			pushSeedError(%obj, getField(%error, 1), 1);
			return failPlantSeed(%pos, %brickDB);
		}
	}

	//plant successful, make plant brick
	%b = createPlantBrick(%pos, %brickDB, 1, "", %brickDB.defaultColor + 0);
	%b.plantedTime = $Sim::Time;
	%b.inGreenhouse = %inGreenhouse;

	%error = %b.plant();
	if (%error > 0 || %error $= "")
	{
		%b.delete();

		return failPlantSeed(%pos, %brickDB);
	}

	%b.setTrusted(1);
	if (!%isTree || %brickDB.noCollision)
	{
		%b.setColliding(0);
	}
	else
	{
		%b.setColliding(1);
	}
	%bg = getBrickgroupFromObject(%obj);
	%bg.add(%b);

	//update inventory item
	updateSeedCount(%image, %obj, %imageslot);

	return %b;
}

function updateSeedCount(%image, %obj, %imageslot)
{
	if (!isObject(%cl = %obj.client))
	{
		return;
	}

	%cropType = %image.cropType;
	%expReward = getPlantData(%cropType, "plantingExperience");
	%cl.addExperience(%expReward);

	%expCost = getPlantData(%cropType, "experienceCost");
	%cl.addExperience(-1 * %expCost);

	%toolStackCount = %obj.toolStackCount[%obj.currTool]--;
	%currTool = %obj.currTool;

	if (%toolStackCount <= 0) //no more seeds left, clear the item slot
	{
		messageClient(%cl, 'MsgItemPickup', '', %currTool, 0);
		%obj.tool[%currTool] = "";
		if (%imageSlot !$= "")
		{
			%obj.unmountImage(%imageSlot);
		}
	}
	else //some seeds are left, update item if needed
	{
		%bestItem = getStackTypeDatablock(%image.item.stackType, %toolStackCount);
		if (%bestItem.getID() != %obj.tool[%currTool].getID())
		{
			messageClient(%cl, 'MsgItemPickup', '', %currTool, %bestItem.getID());
			%obj.tool[%currTool] = %bestItem.getID();
			%obj.mountImage(%imageSlot, %bestItem.image);
		}
	}
}

//0 for no issue
function checkPlantRadiusError(%pos, %db, %planterFound, %inGreenhouse)
{
	%plantRadius = getPlantRadius(%db, %planterFound, %inGreenhouse);
	%plantingLayer = getPlantData(%db.cropType, "plantingLayer");

	//check around the brick for any other plants and make sure we dont violate their radius requirement
	//but exclude flowerpots since those root systems dont intersect with each other
	//fixed size to ensure we capture larger bricks that have longer-distance root systems compared to the current plant
	%box = "16 16" SPC (%db.brickSizeZ * 0.1);
	%base = vectorSub(%pos, "0 0 " @ %db.brickSizeZ * 0.1);
	%searchPos = vectorScale(vectorAdd(%pos, %base), 0.5);
	if ($debugPlanting)
	{
		createBoxMarker(%pos, "1 0 0 1", "0.1 0.1 0.1");
		createBoxMarker(%base, "1 0 0 1", "0.1 0.1 0.1");

		createBoxMarker(%searchPos, "1 1 0 1", "0.1 0.1 0.1");
		createBoxMarker(%searchPos, "1 0 0 0.1", vectorScale(%box, 0.5));
	}
	initContainerBoxSearch(%searchPos, %box, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		%nextDB = %next.dataBlock;
		if (!%nextDB.isPlant || %nextDB.isWeed)
		{
			continue;
		}

		%nextType = %next.dataBlock.cropType;
		if (%plantingLayer != getPlantData(%nextType, "plantingLayer")) //if they aren't both on the same layer, they don't interfere
		{
			continue;
		}

		%nextDirt = %next.getDownBrick(0); //we can assume it is only planted on one kind of dirt brick
		%nextDirtDB = %nextDirt.dataBlock;
		//ignore all pots
		if (%nextDirtDB.isPot)
		{
			continue;
		}

		//do not check overlap between planters and non planters
		if ((%nextDirtDB.isPlanter && !%planterFound) || (!%nextDirtDB.isPlanter && %planterFound))
		{
			continue;
		}

		//do not check overlap between plants inside/outside of a greenhouse
		if ((%next.inGreenhouse && !%inGreenhouse) || (!%next.inGreenhouse && %inGreenhouse))
		{
			continue;
		}

		//radius check
		%nextRadius = getPlantData(%nextType, "plantSpace");
		%nextPos = %next.getPosition();
		%xDiff = mAbs(getWord(%nextPos, 0) - getWord(%pos, 0));
		%yDiff = mAbs(getWord(%nextPos, 1) - getWord(%pos, 1));

		%nextPlantRadius = getPlantRadius(%nextDB, %nextDirtDB.isPlanter, %next.inGreenhouse);
		if ((%xDiff < %nextPlantRadius && %yDiff < %nextPlantRadius)
			|| (%xDiff < %plantRadius && %yDiff < %plantRadius)) //too close to another plant in the area
		{
			createPlantBrick(%nextPos, %nextDB, 0, %next.rotation).schedule(1000, delete); //show which plant its too close to
			return 1 TAB "Too close to a nearby " @ %nextType @ "!";
		}
	}
	return 0;
}

function getPlantRadius(%db, %planterFound, %inGreenhouse)
{
	%studRadius = getPlantData(%db.cropType, "plantSpace");
	return getMax((%studRadius - (%planterFound + %inGreenhouse)) * 0.5 + 0.49, 0.49);
}

//0 for no issue, any other number for a certain kind of issue
//%potFound in return word slot 1 and %planterFound in word slot 2
function checkPlantLocationError(%hitLoc, %db, %obj)
{
	%cropType = %db.cropType;
	%isTree = %db.isTree;
	%plantingBoxDisabled = getPlantData(%cropType, "plantingBoxDisabled");
	if (%db.brickSizeX % 2 == 1)
	{
		%base = roundToStudCenter(%hitLoc);
		%xDiff = 0;
		%yDiff = 0;
	}
	else
	{
		%base = roundToStudCenter(%hitLoc, 1);
		%xDiff = 0.25 SPC 0.25 SPC -0.25 SPC -0.25;
		%yDiff = 0.25 SPC -0.25 SPC 0.25 SPC -0.25;
	}

	for (%i = 0; %i < getWordCount(%xDiff); %i++)
	{
		%offset = getWord(%xDiff, %i) SPC getWord(%yDiff, %i) SPC 0.1;
		%start = vectorAdd(%base, %offset);
		%end = vectorAdd(%start, "0 0 -0.2");
		%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

		if (!isObject(%hit = getWord(%ray, 0)))
		{
			return 1;
		}
		else if (!%hit.dataBlock.isDirt)
		{
			return 2;
		}
		else if (getTrustLevel(%hit, %obj) < 2)
		{
			return 4 SPC %hit;
		}

		if (%hit.dataBlock.isPot)
		{
			%potFound = 1;
		}
		else if (%hit.dataBlock.isPlanter)
		{
			%planterFound = 1;
		}
		else
		{
			%dirtFound = 1;
		}
	}

	if (%dirtFound && (%potFound || %planterFound))
	{
		return 3 TAB "You cannot plant across dirt and a different brick at the same time!";
	}
	else if (%potFound && %planterFound)
	{
		return 3 TAB "You cannot plant across planters and pots!";
	}
	else if ((%plantingBoxDisabled || %isTree) && (%potFound || %planterFound))
	{
		return 3 TAB "You cannot plant this in pots/planters!";
	}

	return 0 SPC %potFound SPC %planterFound;
}

//gets list of down bricks for the plant
//returns "" if any down bricks aren't dirt
function getPlantDownBricks(%hitLoc, %db, %obj)
{
	%pos = getBrickPlantPosition(%hitLoc, %db);
	%b = createPlantBrick(%pos, %db);
	%error = %b.plant();
	if (%error)
	{
		return "";
	}

	%list = "";
	for (%i = 0; %i < %b.getNumDownBricks(); %i++)
	{
		%down = %b.getDownBrick(%i);
		if (!%down.dataBlock.isDirt || getTrustLevel(%down, %obj) < 2)
		{
			return "";
		}
		%list = %list SPC %down;
	}
	%b.delete();
	return trim(%list);
}

function getBrickPlantPosition(%hitloc, %db)
{
	if (%db.brickSizeX % 2)
	{
		%pos = roundToStudCenter(%hitLoc);
	}
	else
	{
		%pos = roundToStudCenter(%hitLoc, 1);
	}
	return vectorAdd(%pos, "0 0 " @ (%db.brickSizeZ * 0.1));
}

function failPlantSeed(%pos, %db)
{
	createPlantBrick(%pos, %db, 0).schedule(1000, delete);
	return 0;
}

//creates and returns a new brick
//isPlanted, rotation, colorID is optional
function createPlantBrick(%pos, %db, %isPlanted, %rotation, %colorID)
{
	return new fxDTSBrick() {
		isPlanted = %isPlanted;
		seedPlant = 1;
		colorID = 0;
		position = %pos;
		dataBlock = %db;
		rotation = (%rotation $= "" ? getRandomBrickOrthoRot() : %rotation);
	};
}

function seedLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%seedName = strReplace(getSubStr(%type, 0, strLen(%type) - 4), "_", " ");
		%cl.centerprint("<just:right><color:ffff00>-Seeds " @ %obj.currTool @ "- \n" @ %seedName @ "<color:ffffff>: " @ %count @ " \n\c0" @ %obj.seedError, 1);
	}

	if (%obj.seedErrorTimeout < $Sim::Time)
	{
		%obj.seedError = "";
	}
}

function pushSeedError(%obj, %msg, %time)
{
	%obj.seedError = %msg @ " ";
	%obj.seedErrorTimeout = $Sim::Time + %time;
}