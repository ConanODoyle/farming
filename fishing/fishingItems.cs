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

	item = FishingPole1Item;
	doColorShift = true;
	colorShiftColor = FishingPole1Item.colorShiftColor;
	rotation = eulerToMatrix("-50 0 0");

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Fish in designated fishing zones";
};

function FishingPole1Image::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%img, %obj, %slot);

		%statTrak = %obj.getToolStatTrak();
		if (%statTrak !$= "")
		{
			%string = "\c4" @ %statTrak @ " ";
		}

		%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n" @ %string, 1);
	}
}

function FishingPole1Image::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);
}
