exec("./potato.cs");
exec("./carrot.cs");
exec("./tomato.cs");
exec("./corn.cs");
exec("./cabbage.cs");
exec("./onion.cs");
exec("./blueberry.cs");
exec("./turnip.cs");
exec("./appleTree.cs");
exec("./mangoTree.cs");

exec("./daisy.cs");
exec("./lily.cs");

exec("./planter.cs");
exec("./reclaimer.cs");
	// case 0: return 0;
	// case 1: return "overlap";
	// case 2: return "float";
	// case 3: return "stuck";
	// case 4: return "unstable";
	// case 5: return "buried";
	// default: return "forbidden";x

function plantCrop(%image, %obj, %imageSlot, %pos)
{
	%cropType = %image.cropType;
	%expRequirement = $Farming::Crops::PlantData_[%cropType, "experienceRequired"];
	if (%obj.client.farmingExperience < %expRequirement)
	{
		%obj.client.centerprint("You don't have enough experience to plant this crop!", 3);
		return 0;
	}

	%type = %image.item.stackType;
	%slot = %obj.currTool;
	%count = %obj.toolStackCount[%obj.currTool];
	if (%count <= 0) //no more seeds left
	{
		return 0;
	}

	if (%pos $= "")
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	}
	else
	{
		%start = vectorAdd(%pos, "0 0 0.1");
		%end = vectorAdd(%pos, "0 0 -0.1");
	}
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

	if (!isObject(%hit = getWord(%ray, 0)) || vectorDist(getWords(%ray, 4, 6), "0 0 1") > 0.01)
	{
		return 0;
	}
	else if (!%hit.getDatablock().isDirt)
	{
		messageClient(%obj.client, '', "You can only plant on dirt!");
		return 0;
	}
	else if (getTrustLevel(%hit, %obj) < 2)
	{
		messageClient(%obj.client, '', %hit.getGroup().name @ " does not trust you enough to do that.", 1);
		return 0;
	}

	%hitLoc = getWords(%ray, 1, 3);
	%brickDB = %image.cropBrick;
	%zOffset = %brickDB.brickSizeZ * 0.1;
	%isTree = %brickDB.isTree;

	if (!%isTree || %brickDB.brickSizeX % 2 == 1)
	{
		%base = roundToStudCenter(%hitLoc);
	}
	else
	{
		%base = roundToStudCenter(%hitLoc, 1);
		
		//second check for dirt under the center 2x2 of the plant
		%xDiff = 0.25 SPC 0.25 SPC -0.25 SPC -0.25;
		%yDiff = 0.25 SPC -0.25 SPC 0.25 SPC -0.25;
		for (%i = 0; %i < 4; %i++)
		{
			%offset = getWord(%xDiff, %i) SPC getWord(%yDiff, %i) SPC 0.1;
			%start = vectorAdd(%base, %offset);
			%end = vectorAdd(%start, "0 0 -0.2");
			%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

			if (!isObject(%hit = getWord(%ray, 0)) || !%hit.getDatablock().isDirt)
			{
				messageClient(%obj.client, '', "You can only plant on dirt!");
				return 0;
			}
		}
	}
	%pos = vectorAdd(%base, "0 0 " @ %zOffset);
	

	%plantRad = $Farming::Crops::PlantData_[%brickDB.cropType, "plantSpace"] * 0.5 - 0.01 + 0.5;
	%hitDB = %hit.getDatablock();
	if ((%hitDB.isPot || %hitDB.isPlanter) && %brickDB.isTree)
	{
		messageClient(%obj.client, '', "You can only plant trees on dirt!");
		return 0;
	}

	if (!%hitDB.isPot)
	{
		//check if this is in a greenhouse or not
		%greenhouseCheck = getWord(containerRaycast(%hitloc, vectorAdd(%hitloc, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType), 0);
		if (isObject(%greenhouseCheck) && %greenhouseCheck.getDatablock().isGreenhouse)
		{
			%inGreenhouse = 1;
		}

		//check around the brick for any other plants and make sure we dont violate their radius requirement
		//but exclude flowerpots since those root systems dont intersect with each other
		initContainerRadiusSearch(%pos, 10, $Typemasks::fxBrickObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next.getDatablock().isPlant)
			{
				%nextType = %next.getDatablock().cropType;
				%rad = $Farming::Crops::PlantData_[%nextType, "plantSpace"] * 0.5 - 0.01 + 0.5;
				%nextPos = %next.getPosition();
				%nextDirt = getWord(containerRaycast(%nextPos, vectorAdd(%nextPos, "0 0 -50"), $TypeMasks::fxBrickObjectType, %next), 0);

				if (%next.getDatablock().isTree != %brickDB.isTree)
				{
					continue;
					// %rad = ($Farming::Crops::PlantData_[%nextType, "plantSpace"] - 3) * 0.5 - 0.01 + 0.5;
				}

				%nextGreenhouseCheck = getWord(containerRaycast(%nextPos, vectorAdd(%nextPos, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType), 0);
				if (isObject(%nextGreenhouseCheck) && %nextGreenhouseCheck.getDatablock().isGreenhouse)
				{
					%next.greenhouseBonus = 1;
					%nextInGreenhouse = 1;
				}

				if (%inGreenhouse != %nextInGreenhouse) //ignore greenhouse plants vs outside plants, and vice versa
				{
					continue;
				}
				else if (%nextDirt.getDatablock().isPot) //ignore all pots (we wouldn't be here if we planted on a pot)
				{
					continue;
				}
				else if (%nextDirt.getDatablock().isPlanter == !%hitDB.isPlanter) //planters ignore non-planters and vice versa
				{
					continue;
				}

				%xDiff = mAbs(getWord(%nextPos, 0) - getWord(%pos, 0)) + (%inGreenhouse | %hitDB.isPlanter) * 0.5;
				%yDiff = mAbs(getWord(%nextPos, 1) - getWord(%pos, 1)) + (%inGreenhouse | %hitDB.isPlanter) * 0.5;

				// talk(%xDiff SPC %yDiff);
				// talk("rad: " @ %rad @ " pRad: " @ %plantRad);

				if ((%xDiff < %rad && %yDiff < %rad)
					|| (%xDiff < %plantRad && %yDiff < %plantRad)) //too close to another plant in the area
				{
					messageClient(%obj.client, '', "Too close to a nearby " @ %nextType @ "!");
					%b = new fxDTSBrick()
					{
						seedPlant = 1;
						colorID = 0;
						position = %pos;
						dataBlock = %brickDB;
						rotation = getRandomBrickOrthoRot();
					};
					%b.schedule(1000, delete);
					return 0;
				}
			}
		}
	}

	%b = new fxDTSBrick()
	{
		seedPlant = 1;
		colorID = %brickDB.defaultColor + 0;
		position = %pos;
		isPlanted = 1;
		dataBlock = %brickDB;
		rotation = getRandomBrickOrthoRot();
	};
	%error = %b.plant();
	if (%error > 0 || %error $= "")
	{
		%b.delete();

		%b = new fxDTSBrick()
		{
			seedPlant = 1;
			colorID = 0;
			position = %pos;
			dataBlock = %brickDB;
			rotation = getRandomBrickOrthoRot();
		};
		%b.schedule(1000, delete);
		
		// messageClient(%obj.client, '', "Cannot plant there!");
		return 0;
	}

	%b.setTrusted(1);
	if (!%isTree && !%brickDB.noCollision)
	{
		%b.setColliding(0);
	}
	else
	{
		%b.setColliding(1);
	}
	%bg = %obj.client.brickGroup;

	%bg.add(%b);

	//plant successful, update item

	%expReward = $Farming::Crops::PlantData_[%cropType, "plantingExperience"];
	%obj.client.addExperience(%expReward);

	%expCost = $Farming::Crops::PlantData_[%cropType, "experienceCost"];
	%obj.client.addExperience(-1 * %expCost);

	%count = %obj.toolStackCount[%obj.currTool]--;
	%slot = %obj.currTool;
	if (%count <= 0) //no more seeds left, clear the item slot
	{
		messageClient(%obj.client, 'MsgItemPickup', '', %slot, 0);
		%obj.tool[%slot] = "";
		if (%imageSlot !$= "")
		{
			%obj.unmountImage(%imageSlot);
		}
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
	return %b;
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
		%cl.centerprint("<color:ffff00>-Seeds " @ %obj.currTool @ "- <br>" @ %seedName @ "<color:ffffff>: " @ %count @ " ", 1);
	}
}

function roundToStudCenter(%pos, %even)
{
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%z = getWord(%pos, 2);

	//%x, %y need to be in increments of 0.25 + 0.5n
	//so add 0.25, round to closest 0.5, then subtract 0.25
	if (!%even)
	{
		%x += 0.25;
		%y += 0.25;
	}
	
	%x = mFloor(%x * 2 + 0.5) / 2;
	%y = mFloor(%y * 2 + 0.5) / 2;
	
	if (!%even)
	{
		%x -= 0.25;
		%y -= 0.25;
	}

	//%z needs to be rounded to nearest 0.1
	%z = mFloor(%z * 10 + 0.5) / 10;

	return %x SPC %y SPC %z;
}

function getRandomBrickOrthoRot() 
{
	%rand = getRandom(0, 3);
	switch (%rand)
	{
		case 0: return "1 0 0 0";
		case 1: return "0 0 1 " @ 90;
		case 2: return "0 0 1 " @ 180;
		case 3: return "0 0 -1 " @ 90;
	}
}