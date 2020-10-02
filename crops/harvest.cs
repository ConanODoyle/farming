//also does pruning
function harvestBrick(%brick, %tool, %harvester)
{
	%db = %brick.getDatablock();
	%stage = %db.stage;
	%type = %db.cropType;

	if (!%db.isPlant)
	{
		return 0;
	}
	
	%buff = getPlantData(%type, %stage, "toolBuff");
	%yield = getPlantData(%type, %stage, "yield");
	%itemDB = getPlantData(%type, %stage, "item");
	%toolType = strReplace(getPlantData(%type, %stage, "harvestTool"), " ", "\t");
	%toolType = strReplace(%toolType, "_", " ");
	%areaToolType = getField(%toolType, 1);
	%toolType = getField(%toolType, 0);

	%changeOnHarvest = getPlantData(%type, %stage, "changeOnHarvest");
	// %dieOnHarvest = getPlantData(%type, %stage, "dieOnHarvest");
	%seedDropChance = getPlantData(%type, %stage, "dropSeed");
	%harvestMax = getPlantData(%type, %stage, "harvestMax");
	%pruneTool = getPlantData(%type, %stage, "pruneTool");
	%harvestExperience = getPlantData(%type, "harvestExperience");

	%totalHarvestCount = getWord(%brick.getNutrients(), 2);


	//check if the plant is being pruned
	if (%pruneTool !$= "" && %pruneTool $= %tool.uiname)
	{
		%pruneDB = getPlantData(%type, %stage, "changeOnPrune");
		if (isObject(%pruneDB))
		{
			%brick.setDatablock(%pruneDB);
			%harvester.client.centerPrint("<color:ffff00>Plant pruned!", 1);
			return 0;
		}
	}

	//check if plant has anything to harvest
	if (!isObject(%itemDB))
	{
		return 0;
	}


	//trust checks
	if (%db.isWeed)
	{
		%bypassTrustRequirement = 1;
	}
	if (getTrustLevel(%brick, %harvester) < 2 && !%bypassTrustRequirement)
	{
		if (getBrickgroupFromObject(%brick).bl_id != 888888)
		{
			%harvester.client.centerPrint(%brick.getGroup().name @ "<color:ff0000> does not trust you enough to do that.", 1);
		}
		return 0;
	}


	//harvest tool checks
	if (%tool.uiName $= %toolType)
	{
		%toolYield = getWord(%buff, getRandom(getWordCount(%buff) - 1));
	}
	else if (%tool.uiName !$= "" && %tool.uiName !$= %areaToolType)
	{
		%harvester.client.centerPrint("This is the wrong tool to use on this plant!", 1);
		return 0;
	}


	//shiny plant and general extra yield checks
	if (%brick.bonusYield !$= "" || isObject(%brick.emitter))
	{
		if (isObject(%brick.emitter))
		{
			switch$ (%brick.emitter.emitter.getName())
			{
				case "goldenEmitter": %extraYield = vectorAdd(%extraYield, "goldenEmitter".bonusYield);
				case "silverEmitter": %extraYield = vectorAdd(%extraYield, "silverEmitter".bonusYield);
				case "bronzeEmitter": %extraYield = vectorAdd(%extraYield, "bronzeEmitter".bonusYield);
			}
			%brick.setEmitter(0);
		}

		if (%brick.greenhouseBonus)
		{
			%extraYield = vectorAdd(%extraYield, 0 SPC vectorScale(getWord(%buff, 1), 2));
		}

		if (%brick.bonusYield !$= "")
		{
			%extraYield = vectorAdd(%extraYield, %brick.bonusYield);
			%brick.bonusYield = "";
		}

		%extraYield = getRandom(getWord(%extraYield, 0), getWord(%extraYield, 1));
	}


	//spawn harvest + seeds
	%pickedTotal = getWord(%yield, getRandom(getWordCount(%yield) - 1)) + %toolYield + extraYield;

	%pos = %brick.getPosition();
	%bg = !%bypassTrustRequirement ? getBrickgroupFromObject(%brick) : "";
	for (%i = 0; %i < %pickedTotal; %i++)
	{
		%vel = (getRandom(12) - 6) / 4 SPC  (getRandom(12) - 6) / 4 SPC 6;
		if (%brick.isColliding())
		{
			%vel = vectorAdd(%vel, vectorScale(vectorNormalize(getWords(%vel, 0, 1)), 3));
		}

		if (getRandom() < 0.005 && isObject(%itemDB.alt))
		{
			%itemDB = %itemDB.alt;
		}

		%item = new Item(Crop)
		{
			dataBlock = %itemDB;
			harvestedBG = %bg;
			canVacuum = 1;
		};
		MissionCleanup.add(%item);
		%item.schedule(60 * 1000, schedulePop);
		%item.setTransform(%pos SPC getRandomRotation());
		%item.setVelocity(%vel);
	}

	%seedDB = %type @ "SeedItem";
	if (getRandom() < %seedDropChance)
	{
		%vel = (getRandom(12) - 6) / 4 SPC  (getRandom(12) - 6) / 4 SPC 6;
		if (%brick.isColliding())
		{
			%vel = vectorAdd(%vel, vectorScale(vectorNormalize(getWords(%vel, 0, 1)), 3));
		}

		%item = new Item(SeedDrop)
		{
			dataBlock = %seedDB;
			harvestedBG = %bg;
			canVacuum = 1;
		};
		%p = new Projectile()
		{
			dataBlock = winStarProjectile;
			initialPosition = %item.getPosition();
			initialVelocity = "0 0 0";
		};
		%p.explode();
		serverPlay3D(rewardSound, %item.getPosition());
		MissionCleanup.add(%item);
		%item.schedule(60 * 1000, schedulePop);
		%item.setTransform(%pos SPC getRandomRotation());
		%item.setVelocity(%vel);
	}


	//harvest fx
	if (%toolType $= "Trowel" || %toolType $= "Hoe")
	{
		%p = new Projectile()
		{
			dataBlock = "FarmingHarvestBelowGroundPlantProjectile";
			initialVelocity = "0 0 1";
			initialPosition = VectorSub(%brick.getPosition(), "0 0" SPC %brick.getDataBlock().brickSizeZ * 0.1);
		};
		%p.explode();
	}
	else if (%toolType $= "Clipper" || %toolType $= "Sickle" || %toolType $= "Weed Cutter")
	{
		%p = new Projectile()
		{
			dataBlock = "FarmingHarvestAboveGroundPlantProjectile";
			initialVelocity = "0 0 1";
			initialPosition = %brick.getPosition();
		};
		%p.explode();
	}


	//change on harvest
	if (%harvestMax > 0 && !isObject(%changeOnHarvest))
	{
		%name = stripChars(%brick.getDatablock().getName(), "0123456789");
		%name = getSubStr(%name, 0, strPos(strLwr(%name), "cropdata"));
		%changeOnHarvest = %name @ "0CropData";
	}
	%totalHarvestCount++;
	if (%dieOnHarvest || %totalHarvestCount >= %harvestMax)
	{
		%brick.delete();
	}
	else if (isObject(%changeOnHarvest))
	{
		%brick.setDatablock(%changeOnHarvest);
		%brick.growTicks = 0;
		%brick.dryTicks = 0;
		%brick.nextGrow = "";
		PlantSimSet.add(%brick);
		//reset nutrients on harvest
		%brick.setNutrients(0, 0, %totalHarvestCount);
	}

	if (%pickedTotal <= 0)
	{
		%harvester.client.centerprint("<color:ff0000>The harvest yielded nothing...", 1);
		%harvester.client.schedule(150, centerprint, "<color:cc0000>The harvest yielded nothing...", 1);
	}

	%expReward = getWord(%harvestExperience, getRandom(getWordCount(%harvestExperience) - 1));
	%harvester.client.addExperience(%expReward);

	return 1 SPC %pickedTotal;
}

function getRandomRotation()
{
	%angle = getRandom(360) / 180 * 3.1415926;
	return "0 0 1" SPC %angle;
}

package ClickToHarvest
{
	function Player::activateStuff(%obj)
	{
		if (isObject(%cl = %obj.client))
		{
			%start = %obj.getEyeTransform();
			%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && %harvest = harvestBrick(%hit, 0, %obj))
			{
				%total = getWord(%harvest, 1);
			}
		}

		return parent::activateStuff(%obj);
	}
};
activatePackage(ClickToHarvest);