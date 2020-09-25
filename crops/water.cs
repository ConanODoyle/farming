function fxDTSBrick::setWaterLevel(%b, %amt)
{
	if (!%b.getDatablock().isDirt && !%b.getDatablock().isWaterTank)
	{
		return;
	}

	%max = %b.getDatablock().maxWater;
	if (%amt !$= "" && %amt >= 0)
	{
		%b.waterLevel = mCeil(getMin(getMax(%amt, 0), %max));
	}

	if (%b.getDatablock().isDirt)
	{
		%step = %max / $DirtWaterColorCount;
		// talk("Step: " @ %step);

		%dist = 10000;
		for (%i = 0; %i < $DirtWaterColorCount; %i++)
		{
			%curr = %i * %step;
			%currDist = mAbs(%b.waterLevel - %curr);
			if (%currDist < %dist)
			{
				%best = %i;
				%dist = %currDist;
			}
		}
		%b.setWaterColor = 1;
		%b.setColor($DirtWaterColor[%best + 0]);
		%b.setWaterColor = 0;
	}
}


//watering cans//


datablock ItemData(WateringCanItem : HammerItem)
{
	iconName = "./icons/wateringCan";
	shapeFile = "./tools/wateringCan.dts";
	uiName = "Watering Can";

	image = "WateringCanImage";
	colorShiftColor = "0.6 0.6 0.6 1";
};

datablock ShapeBaseImageData(WateringCanImage)
{
	shapeFile = "./tools/wateringCan.dts";
	emap = true;

	item = WateringCanItem;
	doColorShift = true;
	colorShiftColor = "0.6 0.6 0.6 1";

	waterAmount = 12;
	tankBonus = 3;

	armReady = true;

	toolTip = "Waters Dirt: +12 | Tanks: +36";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";

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
};

function WateringCanImage::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

////

datablock ItemData(WateringCan2Item : WateringCanItem) 
{
	shapeFile = "./tools/wateringCan2.dts";
	uiName = "Watering Can v2";
	colorShiftColor = "0.75 0 0 1";
	image = "WateringCan2Image";

	iconName = "./icons/wateringCan2";

	cost = 400;
};

datablock ShapeBaseImageData(WateringCan2Image : WateringCanImage)
{
	shapeFile = "./tools/wateringCan2.dts";
	item = WateringCan2Item;
	colorShiftColor = WateringCan2Item.colorShiftColor;
	waterAmount = 30;
	tankBonus = 1.5;

	toolTip = "Waters Dirt: +30 | Tanks: +45";
};

function WateringCan2Image::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

////

datablock ItemData(WateringCan3Item : WateringCanItem) 
{
	shapeFile = "./tools/wateringCan3.dts";
	uiName = "Watering Can v3";
	colorShiftColor = "0 0.7 1 1";
	image = "WateringCan3Image";

	iconName = "./icons/wateringCan3";

	cost = 1200;
};

datablock ShapeBaseImageData(WateringCan3Image : WateringCanImage)
{
	shapeFile = "./tools/wateringCan3.dts";
	item = WateringCan3Item;
	colorShiftColor = WateringCan3Item.colorShiftColor;
	waterAmount = 50;
	tankBonus = 1.5;
	waterRange = 6;

	toolTip = "Waters Dirt: +50 | Tanks: +75";
};

function WateringCan3Image::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

////

datablock ItemData(WateringCatItem : WateringCanItem) 
{
	shapeFile = "./tools/wateringCat.dts";
	uiName = "Watering Cat";
	colorShiftColor = "0.1 0.1 0.1 1";
	image = "WateringCatImage";

	iconName = "./icons/wateringCat";

	cost = 3200;
};

datablock ShapeBaseImageData(WateringCatImage : WateringCanImage)
{
	shapeFile = "./tools/wateringCat.dts";
	item = WateringCatItem;
	colorShiftColor = WateringCatItem.colorShiftColor;
	waterAmount = 100;
	tankBonus = 2.5;
	waterRange = 6;

	toolTip = "Waters Dirt: +100 | Tanks: +250 :0";
};

function WateringCatImage::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

////

datablock ItemData(HoseItem : WateringCanItem) 
{
	shapeFile = "./tools/hoseItem.dts";
	uiName = "Hose";
	colorShiftColor = "0 0.5 0 1";
	image = "HoseImage";

	iconName = "./icons/hose";

	cost = 2800;
};

datablock ShapeBaseImageData(HoseImage : WateringCanImage)
{
	shapeFile = "./tools/hose.dts";
	item = HoseItem;
	colorShiftColor = HoseItem.colorShiftColor;
	waterAmount = 20;
	tankBonus = 10;

	toolTip = "Waters Dirt: +20 | Tanks: +200";
};

function HoseImage::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

////

datablock ItemData(HoseV2Item : WateringCanItem) 
{
	shapeFile = "./tools/hoseItem.dts";
	uiName = "Hose V2";
	colorShiftColor = "0 0 0.5 1";
	image = "HoseV2Image";

	iconName = "./icons/hose";

	cost = 4000;
};

datablock ShapeBaseImageData(HoseV2Image : WateringCanImage)
{
	shapeFile = "./tools/hoseV2.dts";
	item = HoseV2Item;
	colorShiftColor = HoseV2Item.colorShiftColor;
	waterAmount = 50;
	tankBonus = 4;
	tankRatio = 0.0075;

	toolTip = "Waters Dirt: +50 | Tanks: +0.75%";
};

function HoseV2Image::onFire(%this, %obj, %slot)
{
	waterCanFire(%this, %obj, %slot);
}

////

datablock AudioProfile(waterCanSound : exitWaterSound)
{
	fileName = "./tools/WaterCan01.wav";
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
            %start = %hit.getPosition();
            %end = vectorAdd(%start, "0 0 -10");
            %hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType, %hit), 0);
            %db = %hit.getDatablock();
        }

        if (%db.isDirt || %db.isWaterTank)
        {
            if (%db.isWaterTank)
            {
                if (%this.tankRatio > 0)
                {
                    %amt = mFloor(%db.maxWater * %this.tankRatio);
                }

                if (%amt < %this.waterAmount * getMax(%this.tankBonus, 1))
                {
                    %amt = %this.waterAmount * getMax(%this.tankBonus, 1);
                }
            }
            else
            {
                %amt = %this.waterAmount;
            }

            if (%amt < 40)
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
            if (%obj.waterCount >= 100)
            {
                %waterLevel = %waterLevel @ " <just:right>\c2Combo: " @ %obj.waterCount;
            }
            %obj.client.centerprint("<just:right><color:ffffff>Watering... (+" @ %dispensed @ "/" @ %amt @ ") <br>" @ %waterLevel @ " ", 1);
            %obj.client.schedule(50, centerprint, "<just:right><color:cccccc>Watering... (+" @ %dispensed @ "/" @ %amt @ ") <br>" @ %waterLevel @ " ", 1);
        }
    }

    if ($Sim::Time - %obj.lastWater >= 1)
    {
    	%obj.waterCount--;
        if (%obj.waterCount > $Pref::Server::maxWaterCombo)
        {
            announce("<bitmap:base/client/ui/ci/star>\c3" @ %obj.client.name @ "\c6 set a new watering combo count of \c3" @ %obj.waterCount @ "\c6!");
            $Pref::Server::maxWaterCombo = %obj.waterCount SPC "(" @ %obj.client.name @ ")";
        }
        %obj.waterCount = 0;
    }

    %obj.lastWater = $Sim::Time;
}

function serverCmdTopCombo(%cl)
{
	%cl.chatmessage("\c6<bitmap:base/client/ui/ci/star> The longest watering combo is \c3" @ $Pref::Server::maxWaterCombo @ "\c6!");
}

package DirtWaterColor
{
	function fxDTSBrick::onAdd(%obj)
	{
		if (%obj.getDatablock().isDirt)
		{
			%obj.schedule(33, setWaterLevel, "");
		}
		return parent::onAdd(%obj);
	}

	function fxDTSBrick::setColor(%obj, %color)
	{
		if (%obj.getDatablock().isDirt && !%obj.setWaterColor)
		{
			%obj.setWaterLevel("");
			return;
		}
		parent::setColor(%obj, %color);
	}
};
activatePackage(DirtWaterColor);
