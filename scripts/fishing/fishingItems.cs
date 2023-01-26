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








datablock ItemData(TackleBoxItem : HammerItem)
{
	shapeFile = "./fishingPole/tacklebox.dts";

	iconName = "";
	uiName = "Tackle Box";
	image = "TackleBoxImage";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	showFishingStats = 1;

	description = "\c6Allows you to see how quickly you reeled a fish in";
};

datablock ShapeBaseImageData(TackleBoxImage)
{
	shapeFile = "./fishingPole/tacklebox.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = "TackleBoxItem";

	offset = "-0.5017 0.04 -0.17389";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";
};

function TackleBoxImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyBoth);
}

function TackleBoxImage::onUnmount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyRight);
}

function TackleBoxImage::onLoop(%this, %obj, %slot)
{
	%item = %this.item;
	%description = %item.description;
	%cl = %obj.client;

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right>\c3-" @ %item.uiName @ "- \n" @ %description @ " ", 1);
	}
}

datablock ItemData(FishFinderItem : HammerItem)
{
	shapeFile = "./fishingPole/FishFinder.dts";

	iconName = "";
	uiName = "Fish Finder";
	image = "FishFinderImage";
	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	showFishingStats = 2;

	description = "\c6Allows you to see the quality of your \n\c6pull & activity level of a fishing spot";
};

datablock ShapeBaseImageData(FishFinderImage)
{
	shapeFile = "./fishingPole/FishFinder.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = "1 1 1 1";

	item = "FishFinderItem";

	offset = "-0.56 0 0";
	rotation = eulerToMatrix("-20 0 0");

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";
};

function FishFinderImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyBoth);
}

function FishFinderImage::onUnmount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyRight);
}

function FishFinderImage::onLoop(%this, %obj, %slot)
{
	%item = %this.item;
	%description = %item.description;
	%cl = %obj.client;

	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 100));
	%ray = containerRaycast(%start, %end, $Typemasks::FxBrickAlwaysObjectType);
	if (isObject(%hit = getWord(%ray, 0)) && %hit.dataBlock.isFishingSpot)
	{
		%description = "\c3Fishing Spot Activity: \c6" @ mFloor(%hit.fish * 100) @ "%";
	}

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right>\c3-" @ %item.uiName @ "- \n" @ %description @ " ", 1);
	}
}