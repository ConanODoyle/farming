
//watering cans//


datablock ItemData(WateringCanItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/wateringCan";
	shapeFile = "./waterCan/wateringCan.dts";
	uiName = "Watering Can";

	image = "WateringCanImage";
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;
	colorShiftColor = "0.6 0.6 0.6 1";
	doColorShift = false;
};

datablock ShapeBaseImageData(WateringCanImage)
{
	shapeFile = "./waterCan/wateringCan.dts";
	emap = true;

	item = WateringCanItem;
	doColorShift = false;
	colorShiftColor = "0.6 0.6 0.6 1";

	waterAmount = 12;
	tankAmount = 25;

	armReady = true;

	toolTip = "Waters Dirt: +12 | Tanks: +25";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.2;
	stateWaitForTimeout[1] = 0;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateWaitForTimeout[1] = false;

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 0.12;
	stateTransitionOnTimeout[2] = "Repeat";
	// stateWaitForTimeout[2] = 1;

	stateName[3] = "Repeat";
	stateTimeoutValue[3] = 0.12;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";

	stateName[4] = "Ready2";
	stateTransitionOnTimeout[4] = "Ready";
	stateTimeoutValue[4] = 0.2;
	stateWaitForTimeout[4] = 0;
	stateScript[4] = "onReady";
	stateTransitionOnTriggerDown[4] = "Fire";
	stateWaitForTimeout[4] = false;
};

function WateringCanImage::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

function WateringCanImage::onReady(%this, %obj, %slot)
{
	wateringCanReady(%this, %obj, %slot);
}

////

datablock ItemData(WateringCan2Item : WateringCanItem) 
{
	shapeFile = "./waterCan2/wateringCan2.dts";
	uiName = "Watering Can v2";
	colorShiftColor = "0.75 0 0 1";
	image = "WateringCan2Image";
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;

	iconName = "Add-ons/Server_Farming/icons/wateringCan2";
};

datablock ShapeBaseImageData(WateringCan2Image : WateringCanImage)
{
	shapeFile = "./waterCan2/wateringCan2.dts";
	item = WateringCan2Item;
	colorShiftColor = WateringCan2Item.colorShiftColor;
	waterAmount = 30;
	tankAmount = 50;

	toolTip = "Waters Dirt: +30 | Tanks: +50";
};

function WateringCan2Image::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

function WateringCan2Image::onReady(%this, %obj, %slot)
{
	wateringCanReady(%this, %obj, %slot);
}

////

datablock ItemData(WateringCan3Item : WateringCanItem) 
{
	shapeFile = "./waterCan3/wateringCan3.dts";
	uiName = "Watering Can v3";
	colorShiftColor = "0 0.7 1 1";
	image = "WateringCan3Image";
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;

	iconName = "Add-ons/Server_Farming/icons/wateringCan3";
};

datablock ShapeBaseImageData(WateringCan3Image : WateringCanImage)
{
	shapeFile = "./waterCan3/wateringCan3.dts";
	item = WateringCan3Item;
	colorShiftColor = WateringCan3Item.colorShiftColor;
	waterAmount = 50;
	tankAmount = 75;
	waterRange = 6;

	toolTip = "Waters Dirt: +50 | Tanks: +75";
};

function WateringCan3Image::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

function WateringCan3Image::onReady(%this, %obj, %slot)
{
	wateringCanReady(%this, %obj, %slot);
}

////

datablock ItemData(WateringCatItem : WateringCanItem) 
{
	shapeFile = "./waterCat/wateringCat.dts";
	uiName = "Watering Cat";
	colorShiftColor = "0.1 0.1 0.1 1";
	image = "WateringCatImage";
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;

	iconName = "Add-ons/Server_Farming/icons/wateringCat";
};

datablock ShapeBaseImageData(WateringCatImage : WateringCanImage)
{
	shapeFile = "./waterCat/wateringCat.dts";
	item = WateringCatItem;
	colorShiftColor = WateringCatItem.colorShiftColor;
	waterAmount = 150;
	tankAmount = 250;
	waterRange = 6;

	toolTip = "Waters Dirt: +150 | Tanks: +250 :0";
};

function WateringCatImage::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

function WateringCatImage::onReady(%this, %obj, %slot)
{
	wateringCanReady(%this, %obj, %slot);
}

////

datablock ItemData(HoseItem : WateringCanItem) 
{
	shapeFile = "./greentools/hoseItem.dts";
	uiName = "Hose";
	colorShiftColor = "0 0.5 0 1";
	image = "HoseImage";
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;

	iconName = "Add-ons/Server_Farming/icons/hose";
};

datablock ShapeBaseImageData(HoseImage : WateringCanImage)
{
	shapeFile = "./greentools/hose.dts";
	item = HoseItem;
	colorShiftColor = HoseItem.colorShiftColor;
	waterAmount = 30;
	tankAmount = 200;

	toolTip = "Waters Dirt: +30 | Tanks: +200";
};

function HoseImage::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

function HoseImage::onReady(%this, %obj, %slot)
{
	wateringCanReady(%this, %obj, %slot);
}

////

datablock ItemData(HoseV2Item : WateringCanItem) 
{
	shapeFile = "./bluetools/hoseItem.dts";
	uiName = "Hose V2";
	colorShiftColor = "0 0 0.5 1";
	image = "HoseV2Image";
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;

	iconName = "Add-ons/Server_Farming/icons/hose";
};

datablock ShapeBaseImageData(HoseV2Image : WateringCanImage)
{
	shapeFile = "./bluetools/hoseV2.dts";
	item = HoseV2Item;
	colorShiftColor = HoseV2Item.colorShiftColor;
	waterAmount = 50;
	tankAmount = 400;

	toolTip = "Waters Dirt: +50 | Tanks: +400";
};

function HoseV2Image::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

function HoseV2Image::onReady(%this, %obj, %slot)
{
	wateringCanReady(%this, %obj, %slot);
}

////

datablock ItemData(WateringSnakeItem : WateringCanItem) 
{
	shapeFile = "./wateringSnake/wateringSnakeItem.dts";
	uiName = "Watering Snake";
	colorShiftColor = "0 0.5 0 1";
	image = "WateringSnakeImage";
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;

	iconName = "Add-ons/Server_Farming/icons/WateringSnake";
};

datablock ShapeBaseImageData(WateringSnakeImage : WateringCanImage)
{
	shapeFile = "./wateringSnake/wateringSnake.dts";
	item = WateringSnakeItem;
	colorShiftColor = WateringSnakeItem.colorShiftColor;
	waterAmount = 100;
	tankAmount = 600;

	toolTip = "Waters Dirt: +100 | Tanks: +600";
};

function WateringSnakeImage::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

function WateringSnakeImage::onReady(%this, %obj, %slot)
{
	wateringCanReady(%this, %obj, %slot);
}





////





datablock AudioProfile(waterCanSound : exitWaterSound)
{
	fileName = "./WaterCan01.wav";
	description = AudioClosest3d;
};

datablock AudioDescription(AudioWatering3D : AudioClosest3d)
{
	volume = 0.3;
};

datablock AudioProfile(waterCanLotsSound : exitWaterSound)
{
	description = AudioWatering3D;
};

function wateringCanReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%this, %obj, %slot);
		%cl.centerprint("\n<just:right><color:cccccc>Durability: " @ %durability @ " ", 1);
	}
}

function waterCanFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), getMax(4, %this.waterRange)));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

	if (isObject(%hit = getWord(%ray, 0)))
	{
		%db = %hit.getDatablock();

		if (%db.isPlant)
		{
			for (%i = 0; %i < %hit.getNumDownBricks(); %i++)
			{
				%list = %list SPC %hit.getDownBrick(%i);
			}
			%list = trim(%list);
			%hit = getWord(%list, getRandom(0, %i - 1));
			%db = %hit.getDatablock();
		}

		if (%db.isDirt || %db.isWaterTank)
		{
			%durability = useDurability(%this, %obj, %slot);
			if (%db.isWaterTank && %durability > 0)
			{
				%amt = %this.tankAmount;
			}
			else if (%durability > 0)
			{
				%amt = %this.waterAmount;
			}
			else
			{
				%amt = 0;
			}

			if (%amt < 1)
			{
				serverPlay3D(pushBroomHitSound, %obj.getMuzzlePoint(%slot));
			}
			else if (%amt < 40)
			{
				serverPlay3D(waterCanSound, %obj.getMuzzlePoint(%slot));
			}
			else
			{
				serverPlay3D(waterCanLotsSound, %obj.getMuzzlePoint(%slot));
			}

			%pre = %hit.waterLevel;
			%hit.setWaterLevel(%hit.waterLevel + %amt);
			%post = %hit.waterLevel;
			%dispensed = %post - %pre;
			%waterLevel = %hit.waterLevel + 0 @ "/" @ %hit.getDatablock().maxWater;

			%obj.waterCount++;
			if (%obj.waterCount >= 10)
			{
				%waterLevel = %waterLevel @ " <just:right>\c2Combo: " @ %obj.waterCount;
			}
			%waterLevel = %waterLevel @ " \n";

			%waterString = "Watering... (+" @ %dispensed @ "/" @ %amt @ ") \n";
			if (%durability == 0)
			{
				%durability = %durability @ " \n\c0This tool needs repairs!";
			}
			%durabilityString = "Durability: " @ %durability @ " \n";

			%obj.client.centerprint("<just:right><color:ffffff>" @ %waterString @ %durabilityString @ %waterLevel, 1);
			%obj.client.schedule(50, centerprint, "<just:right><color:cccccc>" @ %waterString @ %durabilityString @ %waterLevel, 1);

			cancel(%obj.client.waterComboSchedule);
			%obj.client.waterComboSchedule = schedule(1000, 0, checkWaterCombo, %obj, %obj.client.bl_id, %obj.client.name, %obj.waterCount);
		}
	}
}

function checkWaterCombo(%obj, %blid, %name, %waterCount)
{
	if (%waterCount > $Pref::Server::maxWaterCombo)
	{
		announce("<bitmap:base/client/ui/ci/star>\c3" @ %name @ "\c6 set a new watering combo count of \c3" @ %waterCount @ "\c6!");
		$Pref::Server::maxWaterCombo = %obj.waterCount SPC "(" @ %name @ ")";
	}
	else if (isObject(%obj.client) && %waterCount > 500)
	{
		messageClient(%obj.client, '', "<bitmap:base/client/ui/ci/star>\c3You achieved a watering combo of "@ %waterCount);
	}
	%obj.waterCount = 0;
}

function serverCmdTopCombo(%cl)
{
	%cl.chatmessage("\c6<bitmap:base/client/ui/ci/star> The longest watering combo is \c3" @ $Pref::Server::maxWaterCombo @ "\c6!");
}