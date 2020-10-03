
//harvest tools

datablock ItemData(TrowelItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Trowel";
	shapeFile = "./trowel.dts";
	uiName = "Trowel";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 200;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "TrowelImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(TrowelImage)
{
	shapeFile = "./Trowel.dts";
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
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.2;
	stateScript[1] = "onReady";
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

	stateName[4] = "Ready2";
	stateTransitionOnTimeout[4] = "Ready";
	stateTimeoutValue[4] = 0.2;
	stateScript[4] = "onReady";
	stateTransitionOnTriggerDown[4] = "Fire";
};

function TrowelImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function TrowelImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(ClipperItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Clipper";
	shapeFile = "./Clipper.dts";
	uiName = "Clipper";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 200;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "ClipperImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(ClipperImage : TrowelImage)
{
	shapeFile = "./Clipper.dts";

	item = ClipperItem;
	doColorShift = true;

	projectile = "swordProjectile";

	toolTip = "+1 harvest to above ground crops";
};

function ClipperImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function ClipperImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(HoeItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Hoe";
	shapeFile = "./Hoe.dts";
	uiName = "Hoe";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "HoeImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(HoeImage : TrowelImage)
{
	shapeFile = "./Hoe.dts";

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

function HoeImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(SickleItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Sickle";
	shapeFile = "./Sickle.dts";
	uiName = "Sickle";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "SickleImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(SickleImage : TrowelImage)
{
	shapeFile = "./Sickle.dts";

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

function SickleImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(LongClipperItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/LongClipper";
	shapeFile = "./LongClipper.dts";
	uiName = "LongClipper";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "LongClipperImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(LongClipperImage : TrowelImage)
{
	shapeFile = "./LongClipper.dts";

	item = LongClipperItem;
	doColorShift = true;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest above ground crops";
};

function LongClipperImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function LongClipperImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

function harvestToolReady(%img, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%img, %obj, %slot);
		%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability, 1);
	}
}

function toolHarvest(%img, %obj, %slot)
{
	%item = %img.item;

	%obj.playThread(0, shiftDown);

	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::PlayerObjectType, %obj);

	if (isObject(%hit = getWord(%ray, 0)))
	{
		if (!%hit.getDatablock().isPlant)
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
	}

	%durability = getDurability(%img, %obj, %slot);
	if (%durability == 0 && isObject(%cl = %obj.client))
	{
		%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!", 1);
		return;
	}

	if (%img.areaHarvest > 0 && isObject(%hit))
	{
		initContainerRadiusSearch(getWords(%ray, 1, 3), %img.areaHarvest, $Typemasks::fxBrickObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next.getDatablock().isPlant)
			{
				%type = %next.getDatablock().cropType;
				%err = harvestBrick(%next, %item, %obj);
				if (%err)
				{
					%hasHarvested = 1;
					%dataID = %obj.toolDataID[%obj.currTool];
					%count = getDataIDArrayTagValue(%dataID, %type);
					setDataIDArrayTagValue(%dataID, %type, %count + 1);
				}
			}
		}
	}
	else
	{
		if (isObject(%hit) && %hit.getDatablock().isPlant)
		{
			%err = harvestBrick(%hit, %item, %obj);
			if (%err)
			{
				%hasHarvested = 1;
				%dataID = %obj.toolDataID[%obj.currTool];
				%count = getDataIDArrayTagValue(%dataID, %type);
				setDataIDArrayTagValue(%dataID, %type, %count + 1);
			}
		}
	}

	if (%hasHarvested)
	{
		useDurability(%img, %obj, %slot);
	}
}

function generateHarvestToolDurability(%item)
{
	%baseDurability = %item.durability > 0 ? %item.durability : 100;
	%bonusDurability = %item.extraDurability;
	%chanceDurability = %item.chanceDurability;

	while (getRandom() < %chanceDurability)
	{
		%baseDurability += %bonusDurability;
	}

	return %baseDurability;
}