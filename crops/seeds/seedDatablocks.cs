exec("./potato.cs");
exec("./carrot.cs");
exec("./tomato.cs");
exec("./corn.cs");
exec("./wheat.cs");
exec("./cabbage.cs");
exec("./onion.cs");
exec("./blueberry.cs");
exec("./turnip.cs");
exec("./portobello.cs");
exec("./appleTree.cs");
exec("./mangoTree.cs");

exec("./chili.cs");
exec("./cactus.cs");
exec("./watermelon.cs");
exec("./dateTree.cs");
exec("./peachTree.cs");

exec("./daisy.cs");
exec("./lily.cs");
exec("./rose.cs");

exec("./planter.cs");
exec("./reclaimer.cs");
	// case 0: return 0;
	// case 1: return "overlap";
	// case 2: return "float";
	// case 3: return "stuck";
	// case 4: return "unstable";
	// case 5: return "buried";
	// default: return "forbidden";x

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
		%obj.client.centerprint("You don't have enough experience to plant this crop!", 3);
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
	if (!isObject(getWord(%ray, 0)) || vectorDist(getWords(%ray, 4, 6), "0 0 1") > 0.01)
	{
		return 0;
	}
	%hitLoc = getWords(%ray, 1, 3);

	//re-run raycast to check the actual planting location rather than just where the eye vector hit
	%planterFound = 0;
	%potFound = 0;
	if (%brickDB.brickSizeX % 2 == 1)
	{
		%base = roundToStudCenter(%hitLoc);
		%start = vectorAdd(%base, "0 0 0.1");
		%end = vectorAdd(%start, "0 0 -0.2");
		%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
		if (!isObject(%hit = getWord(%ray, 0)))
		{
			%obj.client.centerprint("Invalid position!", 1);
			%failure = 1;
		}
		else if (!%hit.getDatablock().isDirt)
		{
			%obj.client.centerprint("You can only plant on dirt!", 1);
			%failure = 1;
		}
		else if (getTrustLevel(%hit, %obj) < 2)
		{
			%obj.client.centerprint(%hit.getGroup().name @ " does not trust you enough to do that.", 1);
			%failure = 1;
		}

		if (%hit.getDatablock().isPlanter)
		{
			%planterFound = 1;
		}
		if (%hit.getDatablock().isPot)
		{
			%potFound = 1;
		}
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
				%obj.client.centerprint("You can only plant on dirt!", 1);
				%failure = 1;
				break;
			}

			if (%hit.getDatablock().isPot)
			{
				%potFound = 1;
			}
			else if (!%hit.getDatablock().isPot && %potFound)
			{
				%obj.client.centerprint("You cannot plant across a pot and a different brick!");
				%failure = 1;
				break;
			}

			if (%hit.getDatablock().isPlanter)
			{
				%planterFound = 1;
			}
			else if (!%hit.getDatablock().isPlanter && %planterFound)
			{
				%obj.client.centerprint("You cannot plant across a planter and a different brick!");
				%failure = 1;
				break;
			}
		}
	}
	%pos = vectorAdd(%base, "0 0 " @ %zOffset);
	if (%failure)
	{
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


	%hitDB = %hit.getDatablock();
	if ((%potFound || %planterFound) && (%plantingBoxDisabled || %isTree))
	{
		%obj.client.centerprint("You can only plant small plants on pots/planters!", 1);
		return 0;
	}


	//planting radius checks
	%plantRad = (%radius - %planterFound) * 0.5 - 0.01 + 0.5;
	if (!%potFound)
	{
		//check if this is in a greenhouse or not
		%greenhouseCheck = getWord(containerRaycast(%hitloc, vectorAdd(%hitloc, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType), 0);
		if (isObject(%greenhouseCheck) && %greenhouseCheck.getDatablock().isGreenhouse)
		{
			%inGreenhouse = 1;
			if (%isTree)
			{
				%obj.client.centerprint("You cannot plant tall plants in greenhouses!", 1);
				return 0;
			}
		}

		//check around the brick for any other plants and make sure we dont violate their radius requirement
		//but exclude flowerpots since those root systems dont intersect with each other
		//fixed size to ensure we capture larger bricks that have longer-distance root systems compared to the current plant
		%box = "8 8 1.2";
		if ($debugPlanting)
		{
			createBoxAt(%pos, "1 0 0 0.1", %box);
		}
		initContainerBoxSearch(%pos, %box, $Typemasks::fxBrickObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next.getDatablock().isPlant && !%next.getDatablock().isWeed)
			{
				%nextType = %next.getDatablock().cropType;
				%nextRadius = getPlantData(%nextType, "plantSpace");
				%nextPos = %next.getPosition();
				%nextDirt = %next.getDownBrick(0);

				if (%plantingLayer != getPlantData(%nextType, "plantingLayer")) //if they aren't both on the same layer, they don't interfere
				{
					continue;
				}
				else if (%nextDirt.getDatablock().isPot) //ignore all pots
				{
					continue;
				}

				if (!%next.greenhouseBonus)
				{
					%nextGreenhouseCheck = getWord(containerRaycast(%nextPos, vectorAdd(%nextPos, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType), 0);
					if (isObject(%nextGreenhouseCheck) && %nextGreenhouseCheck.getDatablock().isGreenhouse)
					{
						%next.greenhouseBonus = 1;
						%nextInGreenhouse = 1;
					}
				}
				else
				{
					%nextInGreenhouse = 1;
				}

				if (%inGreenhouse != %nextInGreenhouse) //ignore greenhouse plants vs outside plants, and vice versa
				{
					continue;
				}
				else if (%nextDirt.getDatablock().isPlanter == !%planterFound) //planters ignore non-planters and vice versa
				{
					continue;
				}

				%xDiff = mAbs(getWord(%nextPos, 0) - getWord(%pos, 0));
				%yDiff = mAbs(getWord(%nextPos, 1) - getWord(%pos, 1));

				//calculate next plant's radius
				%nextPlantRad = (%nextRadius - (%nextInGreenhouse || %nextDirt.getDatablock().isPlanter) * 0.5 - 0.01 + 0.5);
				if ((%xDiff < %nextPlantRad && %yDiff < %nextPlantRad)
					|| (%xDiff < %plantRad && %yDiff < %plantRad)) //too close to another plant in the area
				{
					%obj.client.centerprint("Too close to a nearby " @ %nextType @ "!", 1);
					%b = new fxDTSBrick()
					{
						seedPlant = 1;
						colorID = 0;
						position = %pos;
						dataBlock = %brickDB;
						rotation = getRandomBrickOrthoRot();
					};
					%b.schedule(1000, delete);

					%b2 = new fxDTSBrick()
					{
						seedPlant = 1;
						colorID = 0;
						position = %next.getPosition();
						dataBlock = %next.getDatablock();
						rotation = %next.rotation;
					};
					%b2.schedule(1000, delete);
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
	if (!%isTree || %brickDB.noCollision)
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
	%expReward = getPlantData(%cropType, "plantingExperience");
	%obj.client.addExperience(%expReward);

	%expCost = getPlantData(%cropType, "experienceCost");
	%obj.client.addExperience(-1 * %expCost);

	%toolStackCount = %obj.toolStackCount[%obj.currTool]--;
	%slot = %obj.currTool;
	
	if (%toolStackCount <= 0) //no more seeds left, clear the item slot
	{
		messageClient(%obj.client, 'MsgItemPickup', '', %slot, 0);
		%obj.tool[%slot] = "";
		if (%imageSlot !$= "")
		{
			%obj.unmountImage(%imageSlot);
		}
	}
	else //some seeds are left, update item if needed
	{
		%bestItem = getStackTypeDatablock(%type, %toolStackCount);
		messageClient(%obj.client, 'MsgItemPickup', '', %slot, %bestItem.getID());
		%obj.tool[%slot] = %bestItem.getID();
		%obj.mountImage(%imageSlot, %bestItem.image);
	}
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
		%cl.centerprint("<just:right><color:ffff00>-Seeds " @ %obj.currTool @ "- <br>" @ %seedName @ "<color:ffffff>: " @ %count @ " ", 1);
	}
}