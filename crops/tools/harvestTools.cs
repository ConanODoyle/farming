
//harvest tools

datablock ItemData(TrowelItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Trowel";
	shapeFile = "./trowel.dts";
	uiName = "Trowel";

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
	iconName = "Add-ons/Server_Farming/crops/icons/Clipper";
	shapeFile = "./Clipper.dts";
	uiName = "Clipper";

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

////

datablock ItemData(HoeItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Hoe";
	shapeFile = "./Hoe.dts";
	uiName = "Hoe";

	image = "HoeImage";
	colorShiftColor = "0.4 0 0 1";

	cost = 1300;
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

////

datablock ItemData(SickleItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Sickle";
	shapeFile = "./Sickle.dts";
	uiName = "Sickle";

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

////

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

	if (%img.areaHarvest > 0 && isObject(%hit))
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
		if (isObject(%hit) && %hit.getDatablock().isPlant)
		{
			%err = harvestBrick(%hit, %item, %obj);
		}
	}
}
