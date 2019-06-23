//also does pruning
function harvestBrick(%brick, %tool, %harvester)
{
	%db = %brick.getDatablock();
	%stage = %db.stage;
	%type = %db.cropType;
	
	%buff = $Farming::Crops::PlantData_[%type, %stage, "toolBuff"];
	%yield = $Farming::Crops::PlantData_[%type, %stage, "yield"];
	%itemDB = $Farming::Crops::PlantData_[%type, %stage, "item"];
	%toolType = $Farming::Crops::PlantData_[%type, %stage, "harvestTool"];
	%areaToolType = $Farming::Crops::PlantData_[%type, %stage, "areaHarvestTool"];

	%changeOnHarvest = $Farming::Crops::PlantData_[%type, %stage, "changeOnHarvest"];
	%dieOnHarvest = $Farming::Crops::PlantData_[%type, %stage, "dieOnHarvest"];
	%harvestMaxRange = $Farming::Crops::PlantData_[%type, %stage, "maxHarvestTimes"];

	//check if we pruning
	%pruneTool = $Farming::Crops::PlantData_[%type, %stage, "pruneTool"];
	if (%pruneTool !$= "" && %pruneTool $= %tool.uiname)
	{
		%pruneDB = $Farming::Crops::PlantData_[%type, %stage, "changeOnPrune"];
		if (isObject(%pruneDB))
		{
			//its a prune, so we dont gotta do anything else except change db
			%brick.setDatablock(%pruneDB);
			%harvester.client.centerPrint("<color:ffff00>Plant pruned!", 1);
			return;
		}
	}

	if (!isObject(%itemDB))
	{
		return 0;
	}

	if (getWordCount(%harvestMaxRange) == 2 && %brick.maxHarvestTimes <= 0)
	{
		%brick.maxHarvestTimes = getRandom(getWord(%harvestMaxRange, 0), getWord(%harvestMaxRange, 1));
	}
	%brick.harvestTimes++;
	
	if (getTrustLevel(%brick, %harvester) < 2)
	{
		if (getBrickgroupFromObject(%brick).bl_id != 888888)
		{
			%harvester.client.centerPrint(%brick.getGroup().name @ "<color:ff0000> does not trust you enough to do that.", 1);
		}
		return 0;
	}

	if (%tool.uiName $= %toolType)
	{
		%totalYield = getWord(%yield, 0) + getWord(%buff, 0) SPC getWord(%yield, 1) + getWord(%buff, 1);
	}
	else if (%tool.uiName !$= "" && %tool.uiName !$= %areaToolType)
	{
		%harvester.client.centerPrint("This is the wrong tool to use on this plant!", 1);
		return 0;
	}
	else
	{
		%totalYield = %yield;
	}

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

		%extraYield = vectorAdd(%extraYield, %brick.bonusYield);

		%totalYield = getWord(%yield, 0) + getWord(%extraYield, 0) SPC getWord(%yield, 1) + getWord(%extraYield, 1);
		%brick.bonusYield = "";
	}

	%pickedTotal = getRandom(getWord(%totalYield, 0), getWord(%totalYield, 1));

	%pos = %brick.getPosition();
	%bg = getBrickgroupFromObject(%brick);
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

	//harvest fx
	if (%toolType $= "Trowel")
	{
		%p = new Projectile()
		{
			dataBlock = "FarmingHarvestBelowGroundPlantProjectile";
			initialVelocity = "0 0 1";
			initialPosition = VectorSub(%brick.position, "0 0" SPC %brick.getDataBlock().brickSizeZ / 10);
		};
		
		if (isObject(%p))
		{
			MissionCleanup.add(%p);
			%p.explode();
		}
	}
	else
	{
		%p = new Projectile()
		{
			dataBlock = "FarmingHarvestAboveGroundPlantProjectile";
			initialVelocity = "0 0 1";
			
			// brick height is in plates, so brick height divided by 5 is brick height in TU
			// brick height in TU must be divided by half to get the center point, so
			// height / 5 / 2 == height / 10
			initialPosition = %brick.position;
		};
		
		if (isObject(%p))
		{
			MissionCleanup.add(%p);
			%p.explode();
		}
	}

	//change on harvest
	if (%dieOnHarvest)
	{
		%brick.delete();
	} 
	else if (%brick.harvestTimes >= %brick.maxHarvestTimes && %brick.maxHarvestTimes !$= "")
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
	}

	if (%pickedTotal <= 0)
	{
		messageClient(%harvester.client, '', "The harvest yielded nothing...");
	}

	%expReward = $Farming::Crops::PlantData_[%type, "harvestExperience"];
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



//harvest tools

datablock ItemData(TrowelItem : HammerItem)
{
	iconName = "./icons/Trowel";
	shapeFile = "./tools/trowel.dts";
	uiName = "Trowel";

	image = "TrowelImage";
	colorShiftColor = "0.4 0 0 1";

	cost = 1000;
};

datablock ShapeBaseImageData(TrowelImage)
{
	shapeFile = "./tools/Trowel.dts";
	emap = true;

	item = TrowelItem;
	doColorShift = true;
	colorShiftColor = "0.4 0 0 1";

	armReady = true;

	toolTip = "+1 harvest to below ground crops";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 0.2;
	stateTransitionOnTimeout[2] = "Repeat";
	stateWaitForTimeout[2] = 1;

	stateName[3] = "Repeat";
	stateTimeoutValue[3] = 0.12;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";
};

function TrowelImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

////

datablock ItemData(ClipperItem : HammerItem)
{
	iconName = "./icons/Clipper";
	shapeFile = "./tools/Clipper.dts";
	uiName = "Clipper";

	image = "ClipperImage";
	colorShiftColor = "0.4 0 0 1";

	cost = 1600;
};

datablock ExplosionData(ClipperExplosion : swordExplosion) 
{
	soundProfile = "ClipperSound";
};

datablock ProjectileData(ClipperProjectile : swordProjectile) 
{
	explosion = "ClipperExplosion";
};

datablock ShapeBaseImageData(ClipperImage : TrowelImage)
{
	shapeFile = "./tools/Clipper.dts";

	item = ClipperItem;
	doColorShift = true;

	projectile = "ClipperProjectile";

	toolTip = "+1 harvest to above ground crops";
};

function ClipperImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

////

datablock ItemData(HoeItem : HammerItem)
{
	iconName = "./icons/Hoe";
	shapeFile = "./tools/Hoe.dts";
	uiName = "Hoe";

	image = "HoeImage";
	colorShiftColor = "0.4 0 0 1";

	cost = 1300;
};

datablock ShapeBaseImageData(HoeImage : TrowelImage)
{
	shapeFile = "./tools/Hoe.dts";

	item = HoeItem;
	doColorShift = true;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest below ground crops";
};

function HoeImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

////

datablock ItemData(SickleItem : HammerItem)
{
	iconName = "./icons/Sickle";
	shapeFile = "./tools/Sickle.dts";
	uiName = "Sickle";

	image = "SickleImage";
	colorShiftColor = "0.4 0 0 1";

	cost = 1800;
};

datablock ShapeBaseImageData(SickleImage : TrowelImage)
{
	shapeFile = "./tools/Sickle.dts";

	item = SickleItem;
	doColorShift = true;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest above ground crops";
};

function SickleImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

////

function toolHarvest(%img, %obj, %slot)
{
	%item = %img.item;

	%obj.playThread(0, shiftDown);

	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::PlayerObjectType, %obj);

	if (isObject(%ray))
	{
		if (!isObject(%img.projectile))
		{
			%db = swordProjectile;
		}
		else
		{
			%db = %img.projectile;
		}

		%p = new Projectile()
		{
			dataBlock = %db;
			initialPosition = getWords(%ray, 1, 3);
			initialVelocity = "0 0 0";
		};
		%p.setScale("0.5 0.5 0.5");
		%p.explode();
	}

	if (%img.areaHarvest > 0 && isObject(%ray))
	{
		initContainerRadiusSearch(getWords(%ray, 1, 3), %img.areaHarvest, $Typemasks::fxBrickObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next.getDatablock().isPlant)
			{
				%err = harvestBrick(%next, %item, %obj);
			}
		}
	}
	else
	{
		if (isObject(%hit = getWord(%ray, 0)) && %hit.getDatablock().isPlant)
		{
			%err = harvestBrick(%hit, %item, %obj);
		}
	}
}
