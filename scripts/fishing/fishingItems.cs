//rods have the following qualities:
//	quality of hook
//	throw force
//	avg time to bite
//	cast distance
//	reel timing forgiveness
//	durability

datablock ItemData(FishingPole1Item : HammerItem)
{
	iconName = "";
	shapeFile = "./fishingPole/pole.dts";
	uiName = "Fishing Pole";

	image = "FishingPole1Image";
	doColorShift = true;
	colorShiftColor = "0.40 0.25 0.12 1";

	hasDataID = 1;
	isDataIDTool = 1;
	
	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 100;
	chanceDurability = 0.7;
	bonusDurability = 10;
};

datablock ShapeBaseImageData(FishingPole1Image : TrowelImage)
{
	shapeFile = "./fishingPole/pole.dts";

	emap = true;
	armReady = true;

	className = "FishingPoleImage";

	item = FishingPole1Item;
	doColorShift = true;
	colorShiftColor = FishingPole1Item.colorShiftColor;
	rotation = eulerToMatrix("-50 0 0");

	fishingRange = 64;
	fishingForce = 20;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Fish in designated fishing zones";
};

function FishingPoleImage::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%img, %obj, %slot);

		%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " ", 1);
	}
}

function FishingPoleImage::onMount(%this, %obj, %slot)
{
	if (isObject(%obj.bobber))
	{
		cleanupBobber(%obj.bobber);
	}
}

function FishingPoleImage::onUnmount(%this, %obj, %slot)
{
	if (isObject(%obj.bobber))
	{
		cleanupBobber(%obj.bobber);
	}
}

function FishingPoleImage::onFire(%this, %obj, %slot)
{
	if (!isObject(%obj.bobber))
	{
		castFishingLine(%this, %obj, %slot);
	}
	else
	{
		reelFishingLine(%this, %obj, %slot);
	}
}

function reelFishingLine(%this, %obj, %slot)
{
	if (!isObject(%obj.bobber))
	{
		return;
	}
	%obj.playThread(2, shiftUp);

	reelBobber(%obj.bobber);
}

function castFishingLine(%this, %obj, %slot)
{
	if (isObject(%obj.bobber))
	{
		return;
	}
	startFish(%obj, %this.fishingRange, %this.fishingForce);
	%obj.playThread(2, shiftDown);
}