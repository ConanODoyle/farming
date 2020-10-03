datablock ItemData(ReclaimerItem : HammerItem)
{
	shapeFile = "./Reclaimer.dts";
	iconName = "Add-Ons/Server_Farming/crops/icons/Seed_Reclaimer";
	uiName = "Seed Reclaimer";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	colorShiftColor = "0.4 0 0 1";
	image = ReclaimerImage;
};

datablock ShapeBaseImageData(ReclaimerImage)
{
	shapeFile = "./Reclaimer.dts";
	emap = true;

	doColorshift = true;
	colorShiftColor = ReclaimerItem.colorShiftColor;

	item = ReclaimerItem;

	armReady = true;

	tooltip = "Reclaims seeds, rare chance for 2 if harvestable";

	min = 4;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "Ready";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.2;
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
	stateScript[3] = "onReady";
	stateTransitionOnTriggerDown[3] = "Fire";
};

function ReclaimerImage::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%img, %obj, %slot);
		%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability, 1);
	}
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
		if (getTrustLevel(%hit, %obj) < 2)
		{
			%cl.centerprint(getBrickgroupFromObject(%hit).name @ "<color:ff0000> does not trust you enough to do that.", 1);
			return;
		}
		if (getDurability(%this, %obj, %slot) == 0)
		{
			%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!", 1);
			return;
		}
		%db = %hit.getDatablock();
		%stage = %db.stage;
		%type = %db.cropType;
		useDurability(%this, %obj, %slot);
		
		%yield = getPlantData(%type, %stage, "yield");

		%itemDB = %type @ "SeedItem";

		if ((%yield !$= "" || vectorLen(%yield) > 0.1) && getRandom() < 0.2)
		{
			%amt = 2;
			messageClient(%obj.client, '', "<bitmap:base/client/ui/ci/star> \c6You reclaimed two seeds!");
		}
		else
		{
			%amt = 1;
		}

		for (%i = 0; %i < %amt; %i++)
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
		%hit.delete();

		if (getPlantData(%type, "experienceCost") > 0)
		{
			%experienceCost = mCeil(getPlantData(%type, "experienceCost") / 2);
			messageClient(%cl, '', "<bitmap:base/client/ui/ci/star> \c6You reclaimed the plant and received \c3" @ %experienceCost @ "\c6 experience back!");
			%cl.addExperience(%experienceCost);
		}
	}
}