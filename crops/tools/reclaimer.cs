datablock ItemData(ReclaimerItem : HammerItem)
{
	shapeFile = "./redtools/Reclaimer.dts";
	iconName = "Add-Ons/Server_Farming/icons/Seed_Reclaimer";
	uiName = "Seed Reclaimer";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 300;
	chanceDurability = 0.85;
	bonusDurability = 25;

	doColorshift = false;
	colorShiftColor = "0.4 0 0 1";
	image = ReclaimerImage;
};

datablock ShapeBaseImageData(ReclaimerImage)
{
	shapeFile = "./redtools/Reclaimer.dts";
	emap = true;

	doColorshift = false;
	colorShiftColor = ReclaimerItem.colorShiftColor;

	item = ReclaimerItem;

	armReady = true;

	tooltip = "Reclaims seeds";

	min = 4;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "Ready";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.2;
	stateWaitForTimeout[1] = 0;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";
	
	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 0.1;
	stateWaitForTimeout[2] = true;

	stateName[3] = "Ready2";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = 0;
	stateScript[3] = "onReady";
	stateTransitionOnTriggerDown[3] = "Fire";
};

//returns 0 if OK, 1 if cannot reclaim at all, 2 if cannot reclaim due to harvested before
function checkReclaim(%brick)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	%yield = getPlantData(%type, %stage, "yield");
	%timeSincePlanted = $Sim::Time - %brick.plantedTime;
	%harvestCount = %brick.getHarvestCount();

	//special case for flowers as they feed nutrients into soil, and never get harvested
	if (strPos("daisy lily rose", strLwr(%type)) >= 0)
	{
		return (%stage < 3) TAB "Flower fully grown";
	}

	if (%type $= "weed")
	{
		return 0 TAB "Weeds unreclaimable";
	}

	//normal plants
	//can reclaim if harvest count < 1
	// if (%harvestCount < 1 && (!%db.isTree || %timeSincePlanted < 60 * 5 || %stage < 3))
	
	if (%harvestCount > 0)
	{
		return 0 TAB "Harvested at least once";
	}
	return 1;
}

//returns exp value to return on reclaim
function getReclaimEXPValue(%brick)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	%yield = getPlantData(%type, %stage, "yield");
	%timeSincePlanted = $Sim::Time - %brick.plantedTime;
	%harvestCount = %brick.getHarvestCount();
	%plantExpCost = getPlantData(%type, "experienceCost");

	//special case for flowers as they feed nutrients into soil
	if (strPos("daisy lily rose", strLwr(%type)) >= 0)
	{
		return %plantExpCost SPC 1;
	}

	//normal plants
	if (%harvestCount > 0)
	{
		return 0;
	}

	//trees reclaimable as long as not harvested before
	//full exp only if recently planted or stage < 3
	if (%db.isTree)
	{
		if (%timeSincePlanted < 60 * 5 || %stage < 3)
		{
			return %plantExpCost SPC 1;
		}
		else
		{
			return (%plantExpCost * 0.85) SPC 0.85;
		}
	}

	//basic plants only reclaimable for exp if recently planted
	if (%timeSincePlanted < 40)
	{
		return %plantExpCost SPC 1;
	}
	%diff = getMax(0, 1 - (%timeSincePlanted - 40) / 120);

	return mFloor(%diff * %plantExpCost) SPC %diff;
}

//returns number of seeds to return on reclaim
function getReclaimSeedCount(%brick)
{
	%db = %brick.dataBlock;
	%stage = %db.stage;
	%type = %db.cropType;
	%yield = getPlantData(%type, %stage, "yield");

	//special case for flowers and trees - no double reclaims
	if (strPos("daisy lily rose ancientflower", strLwr(%type)) >= 0)
	{
		return 1;
	}

	//reclaim chance for double IF on a fruit-yielding stage
	if (%yield !$= "" && getRandom() < 0.2)
	{
		return 2;
	}
	return 1;
}

function ReclaimerImage::onReady(%this, %obj, %slot)
{
	if (!isObject(%cl = %obj.client))
	{
		return;
	}

	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

	if (isObject(%hit = getWord(%ray, 0)) && %hit.getDatablock().isPlant)
	{
		%canReclaim = checkReclaim(%hit);
		%reclaimEXP = getReclaimEXPValue(%hit);
		%reclaimEXPValue = getWord(%reclaimEXP, 0);
		%reclaimEXPPercent = getWord(%reclaimEXP, 1);

		if (!%canReclaim)
		{
			%reclaimInfo = getField(%canReclaim, 1);
			%reclaimString = "\c0Cannot reclaim: " @ %reclaimInfo;
		}
		else
		{
			%reclaimString = "\c2Can reclaim for " @ %reclaimEXPValue @ " EXP (" @ mFloor(%reclaimEXPPercent*100) @ "%)";
		}
	}

	%durability = getDurability(%img, %obj, %slot);
	%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n" @ %reclaimString, 1);
}

function ReclaimerImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, shiftDown);

	%cl = %obj.client;
	//actual fire check starts here
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

	if (isObject(%ray))
	{
		%p = new Projectile()
		{
			dataBlock = swordProjectile;
			initialPosition = getWords(%ray, 1, 3);
			initialVelocity = "0 0 0";
		};
		%p.setScale("0.5 0.5 0.5");
		%p.explode();
	}

	if (isObject(%hit = getWord(%ray, 0)) && %hit.getDatablock().isPlant)
	{
		%canReclaim = checkReclaim(%hit);
		%reclaimEXP = getReclaimEXPValue(%hit);
		%reclaimEXPValue = getWord(%reclaimEXP, 0);
		%reclaimEXPPercent = getWord(%reclaimEXP, 1);
		%reclaimSeedCount = getReclaimSeedCount(%hit);

		if (getDurability(%this, %obj, %slot) == 0)
		{
			%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!", 1);
			return;
		}
		else if (getTrustLevel(%hit, %obj) < 2)
		{
			%cl.centerprint(getBrickgroupFromObject(%hit).name @ "\c0 does not trust you enough to do that.", 3);
			return;
		}
		else if (!%canReclaim)
		{
			%reclaimInfo = getField(%canReclaim, 1);
			%reclaimString = "\c0Cannot reclaim: " @ %reclaimInfo;
			%durability = getDurability(%img, %obj, %slot);
			%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n" @ %reclaimString, 1);
			return;
		}

		if (%reclaimSeedCount == 2)
		{
			messageClient(%obj.client, '', "<bitmap:base/client/ui/ci/star> \c6You reclaimed two seeds!");
		}

		%db = %hit.dataBlock;
		%type = %db.cropType;
		%itemDB = %type @ "SeedItem";
		useDurability(%this, %obj, %slot);
		for (%i = 0; %i < %reclaimSeedCount; %i++)
		{
			%vel = (getRandom(6) - 3) / 2 SPC  (getRandom(6) - 3) / 2 SPC 6;
			%item = new Item(Seeds)
			{
				dataBlock = %itemDB;
				harvestedBG = %cl.brickgroup;
			};
			MissionCleanup.add(%item);
			%item.schedule(60 * 1000, schedulePop);
			%item.setTransform(%hit.getPosition() SPC getRandomRotation());
			%item.setVelocity(%vel);
		}

		%cl.addExperience(%reclaimEXPValue);
		if (%reclaimEXPValue > 0)
		{
			messageClient(%cl, '', "<bitmap:base/client/ui/ci/star> \c6You reclaimed the plant and received \c3" @ %reclaimEXPValue @ "\c6 experience back!");
		}
		%hit.delete();
	}
}