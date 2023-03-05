//attempts to harvest the target brick
//considers tool used (itemDB), extra bonus, and harvester
//returns 0 if harvest was not compelted, 1 SPC yield if harvest successful
//also does pruning
function harvestBrick(%brick, %tool, %harvester, %fixedBonus)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;

	if (!%db.isPlant)
	{
		return 0;
	}

	%itemDB = getPlantData(%type, %stage, "item");
	// %dieOnHarvest = getPlantData(%type, %stage, "dieOnHarvest");

	%totalHarvestCount = getWord(%brick.getNutrients(), 2);


	//check if the plant is being pruned
	if (canPrune(%brick, %tool))
	{
		return prunePlant(%brick, %harvester);
	}

	//check if plant has anything to harvest
	if (!isObject(%itemDB))
	{
		return 0;
	}

	//trust checks
	if (!canHarvest(%harvester, %brick))
	{
		return 0;
	}

	//harvest tool checks
	if (isObject(%tool))
	{
		%toolCanHarvest = canToolHarvest(%tool, %brick);
		if (!%toolCanHarvest)
		{
			%harvester.client.centerPrint("This is the wrong tool to use on this plant!", 2);
			return 0;
		}
		else if (%toolCanHarvest == 2)
		{
			%bonusYield += getToolBonusYield(%tool, %brick);
		}
	}

	//shiny plant and general extra yield checks
	%bonusYield += getBonusYield(%brick);
	%bonusYield += %fixedBonus;

	//spawn harvest + seeds
	%pickedTotal = getTotalYield(%brick, %bonusYield);

	spawnCrops(%brick, %pickedTotal, %harvester);
	spawnSeeds(%brick, %harvester);

	//harvest fx
	spawnHarvestFX(%brick, %tool);

	//change on harvest
	%brick.onPlantHarvested();

	if (%pickedTotal <= 0)
	{
		%harvester.client.centerprint("\c0The harvest yielded nothing...", 1);
		%harvester.client.schedule(150, centerprint, "<color:cc0000>The harvest yielded nothing...", 1);
	}

	//experience gain
	%harvestExperience = getPlantData(%type, %stage, "harvestExperience");
	if (%harvestExperience $= "")
	{
		%harvestExperience = getPlantData(%type, "harvestExperience");
	}
	%expReward = getWord(%harvestExperience, getRandom(getWordCount(%harvestExperience) - 1));
	%harvester.client.addExperience(%expReward);

	return 1 SPC %pickedTotal;
}

function canPrune(%brick, %tool)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	%pruneTool = getPlantData(%type, %stage, "pruneTool");
	%pruneTool = strReplace(%pruneTool, "_", " ");

	return %pruneTool !$= ""
		&& isObject(getPlantData(%type, %stage, "changeOnPrune"))
		&& striPos(%pruneTool, %tool.toolType) >= 0;
}

function prunePlant(%brick, %harvester)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;

	%pruneDB = getPlantData(%type, %stage, "changeOnPrune");
	if (isObject(%pruneDB))
	{
		%brick.setDatablock(%pruneDB);
		%harvester.client.centerPrint("\c2Plant pruned!", 1);
		return 1;
	}
	return 0;
}

function canHarvest(%harvester, %brick)
{
	%db = %brick.dataBlock;
	if (%brick.getGroup().bl_id == 888888)
	{
		return 0;
	}
	if (getTrustLevel(%brick, %harvester) < 2 && !%db.isWeed)
	{
		%harvester.client.centerPrint(%brick.getGroup().name @ "\c0 does not trust you enough to do that.", 1);
		return 0;
	}
	return 1;
}

//returns 0 for no, 1 for yes, 2 for yes with bonus yield
function canToolHarvest(%tool, %brick)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;

	%toolTypes = strReplace(getPlantData(%type, %stage, "harvestTool"), " ", "\t");
	%toolTypes = strReplace(%toolTypes, "_", " ");
	%toolType = getField(%toolTypes, 0);
	%areaToolType = getField(%toolTypes, 1);

	switch$ (%tool.toolType)
	{
		case %toolType: return 2;
		case %areaToolType: return 1;
		default: return 0;
	}
}

function getToolBonusYield(%tool, %brick)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	%buff = getPlantData(%type, %stage, "toolBuff");

	return getWord(%buff, getRandom(getWordCount(%buff) - 1)); 
}

function getBonusYield(%brick)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	%yield = getPlantData(%type, %stage, "yield");

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

		if (%brick.inGreenhouse)
		{
			%extraYield = vectorAdd(%extraYield, 0 SPC mFloor((getWord(%yield, 1) / 2)));
		}

		if (%brick.bonusYield !$= "")
		{
			%extraYield = vectorAdd(%extraYield, %brick.bonusYield);
			%brick.bonusYield = "";
		}

	}
	return getRandom(getWord(%extraYield, 0), getWord(%extraYield, 1));
}

function getTotalYield(%brick, %bonusYield)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	%yield = getPlantData(%type, %stage, "yield");

	%baseYield = getWord(%yield, getRandom(getWordCount(%yield) - 1));
	return %baseYield + %bonusYield;
}

function spawnCrops(%brick, %count, %harvester)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	
	%pos = %brick.getPosition();
	%bg = getBrickgroupFromObject(%harvester);
	%itemDB = getPlantData(%type, %stage, "item");
	for (%i = 0; %i < %count; %i++)
	{
		%vel = (getRandom(12) - 6) / 4 SPC  (getRandom(12) - 6) / 4 SPC 6;
		//jank tree check - trees/tree-like crops are colliding while normal crops arent
		//if "tree" then spawn higher up in the brick
		if (%brick.isColliding())
		{
			%dir = vectorNormalize(getWords(%vel, 0, 1));
			%vel = vectorAdd(%vel, vectorScale(%dir, 3));
			%offset = vectorScale(%dir, 0.75);
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
		%item.schedule(90 * 1000, schedulePop);
		%item.setTransform(vectorAdd(%pos, %offset) SPC getRandomRotation());
		%item.setVelocity(%vel);
	}
}

function spawnSeeds(%brick, %harvester)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;

	%seedDB = %type @ "SeedItem";
	%bg = getBrickgroupFromObject(%harvester);
	%seedDropChance = getPlantData(%type, %stage, "dropSeed");
	%pos = %brick.position;
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
		MissionCleanup.add(%item);
		%item.schedule(60 * 1000, schedulePop);
		%item.setTransform(%pos SPC getRandomRotation());
		%item.setVelocity(%vel);
		%p = new Projectile()
		{
			dataBlock = winStarProjectile;
			initialPosition = %item.getPosition();
			initialVelocity = "0 0 0";
		};
		%p.explode();
		serverPlay3D(rewardSound, %item.getPosition());
	}
}

function spawnHarvestFX(%brick, %tool)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;

	%class = getCropClass(%type);
	%effect = getPlantData(%type, "harvestEffect");
	if (%effect $= "")
	{
		%effect = %class;
	}

	switch$ (%effect)
	{
		case "Underground":
			%db = "FarmingHarvestBelowGroundPlantProjectile";
			%pos = %brick.getPosition();
		case "Aboveground":
			%db = "FarmingHarvestAboveGroundPlantProjectile";
			%pos = VectorSub(%brick.getPosition(), "0 0" SPC %brick.getDataBlock().brickSizeZ * 0.1);
		default: 
			%db = "FarmingHarvestAboveGroundPlantProjectile";
			%pos = VectorSub(%brick.getPosition(), "0 0" SPC %brick.getDataBlock().brickSizeZ * 0.1);
	}

	if (isObject(%tool.image.harvestEffect))
	{
		%db = %tool.image.harvestEffect;
	}

	%p = new Projectile()
	{
		dataBlock = %db;
		initialVelocity = "0 0 1";
		initialPosition = %pos;
	};
	%p.explode();
}

function fxDTSBrick::onPlantHarvested(%brick)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;

	if (!%db.isPlant)
	{
		return;
	}

	%changeOnHarvest = getPlantData(%type, %stage, "changeOnHarvest");
	%harvestMax = getPlantData(%type, %stage, "harvestMax");

	%totalHarvestCount = getWord(%brick.getNutrients(), 2);
	%totalHarvestCount++;
	if (%dieOnHarvest || %totalHarvestCount >= %harvestMax)
	{
		%brick.delete();
	}

	if (!isObject(%changeOnHarvest))
	{
		%name = stripChars(%brick.getDatablock().getName(), "0123456789");
		%name = getSubStr(%name, 0, strPos(strLwr(%name), "cropdata"));
		%changeOnHarvest = %name @ "0CropData";
	}

	if (!isObject(%changeOnHarvest))
	{
		%brick.delete();
	}

	%brick.setDatablock(%changeOnHarvest);	

	%brick.wetTicks = 0;
	%brick.dryTicks = 0;
	%brick.nextGrow = "";
	if (!PlantSimSet.isMember(%brick))
	{
		PlantSimSet.add(%brick);
	}
	//reset nutrients on harvest
	%brick.setNutrients(0, 0, %totalHarvestCount);	
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