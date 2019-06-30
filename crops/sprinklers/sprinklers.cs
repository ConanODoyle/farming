//todo: disable naming sprinkler and watertower bricks
//brickDB.directionalOffset = Vector3F TU;
//brickDB.boxSize = Point3f Size;
//brickDB.maxDispense = int;

exec("./bricks.cs");

$SprinklerMaxDistance = 20;
$DebugOn = 0;

if (!isObject(SprinklerSimSet))
{
	$SprinklerSimSet = new SimSet(SprinklerSimSet) {};
}

function sprinklerTick(%index)
{
	cancel($masterSprinklerSchedule);

	if (!isObject(MissionCleanup))
	{
		return;
	}

	if (SprinklerSimSet.getCount() <= 0)
	{
		$masterSprinklerSchedule = schedule(33, 0, sprinklerTick, 0);
		return;
	}

	if (%index < 0)
	{
		%index = SprinklerSimSet.getCount() - 1;
	}
	else if (%index > SprinklerSimSet.getCount() - 1)
	{
		%index = SprinklerSimSet.getCount() - 1;
	}

	%max = 8;
	for (%i = 0; %i < %max; %i++)
	{
		if (%i > %index) //we reached end of simset
		{
			break;
		}

		%brick = SprinklerSimSet.getObject(%index - %i);
		if ($debugOn && %brick == $compare)
			echo("Sprinkler: " @ %brick @ " " @ %brick.getDatablock().uiName);
		if (%brick.age $= "")
		{
			%brick.age = $Sim::Time;
			// echo("Initialized " @ %brick @ " age");
		}
		if (%brick.age + 60 < $Sim::Time && !$Server::AS["InUse"])
		{
			%name = %brick.getName();
			%brick.setName("CopySprinkler");
			%newbrick = new fxDTSBrick(replacement : CopySprinkler){}; //make a copy of the brick
			%newbrick.skipBuy = 1;
			getBrickgroupFromObject(%brick).add(%newbrick);
			%brick.sold = 1;
			%brick.delete();
			%newbrick.setName(%name);
			%newbrick.plant();
			%newbrick.setTrusted(1);
			SprinklerSimSet.add(%newbrick);
			doSprinklerSearch(%newbrick, 1);
			%i += 2; //process fewer this tick
			if ($debugOn && %brick == $compare)
				echo("Replaced Sprinkler: " @ %brick @ " with " @ %newbrick);
			%newbrick.age = $Sim::Time;
			continue;
		}
		else if (!%brick.isDead && isObject(%brick))
		{
			if ($debugOn && %brick == $compare)
				echo("    Not dead");
			doSprinklerSearch(%brick, 1);
		}
		%totalBricksProcessed++;
	}

	$masterSprinklerSchedule = schedule(33, 0, sprinklerTick, %index - %totalBricksProcessed);
}

function doSprinklerSearch(%sprinkler, %water)
{
	if (%sprinkler.lastSprinklerSearch + 2 > $Sim::Time)
	{
		return "time";
	}

	%db = %sprinkler.getDatablock();
	if (!%db.isSprinkler)
	{
		return "not sprinkler";
	}

	%offset = %db.directionalOffset;
	%box = vectorSub(%db.boxSize, "0.1 0.1 0.05");


	//rotate search box
	switch (%sprinkler.angleID)
	{
		case 0: //do nothing
		case 1: 
			%box = getWord(%box, 1) SPC getWord(%box, 0) SPC getWord(%box, 2);
			%offset = getWord(%offset, 1) SPC -1 * getWord(%offset, 0) SPC getWord(%offset, 2);
		case 2: 
			%offset = -1 * getWord(%offset, 0) SPC -1 * getWord(%offset, 1) SPC getWord(%offset, 2);
		case 3: 
			%box = getWord(%box, 1) SPC getWord(%box, 0) SPC getWord(%box, 2);
			%offset = -1 * getWord(%offset, 1) SPC getWord(%offset, 0) SPC getWord(%offset, 2);
	}

	// if (%db.uiName $= "Straight Sprinkler" && !%sprinkler.hasTalked)
	// {
	// 	talk("offset: " @ vectorAdd(%offset, %sprinkler.getPosition()));
	// 	talk("box: " @ %box);
	// 	%sprinkler.hasTalked = 1;
	// }

	if ($debugOn && %sprinkler == $compare)
	{
		echo("    " @ %sprinkler @ " Box search");
		echo("    Box: " @ %box);
		echo("    Pos: " @ %sprinkler.getPosition());
		echo("    db: " @ %db);
	}
	initContainerBoxSearch(vectorAdd(%offset, %sprinkler.getPosition()), %box, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()) && %safety++ < 1500)
	{
		if (%next.getDatablock().isDirt && getTrustLevel(%next, %sprinkler) > 0)
		{
			%next.sprinkler = %sprinkler;
			if (%water && (%diff = %next.getDatablock().maxWater - %next.waterLevel) > 0)
			{
				%amt = drawWater(%sprinkler, %diff);
				%next.setWaterLevel(%next.waterLevel + drawWater(%sprinkler, %diff));
				if (%amt > 0)
				{
					%dispensedWater = 1;
				}
			}
			%count++;
		}
	}
	if ($debugOn && %sprinkler == $compare)
		echo("    " @ %sprinkler @ " Box search complete (Safety: " @ %safety @ ")");

	if (%safety >= 1500)
	{
		talk("doSprinklerSearch bypassed safety");
		echo("doSprinklerSearch bypassed safety");
	}

	%sprinkler.lastSprinklerSearch = $Sim::Time;

	if (%dispensedWater)
	{
		%sprinkler.setEmitter("ChromePaintDropletEmitter");
		%sprinkler.schedule(1000, setEmitter, "");
	}

	SprinklerSimSet.add(%sprinkler);
	if (%db.noCollide)
	{
		%sprinkler.setColliding(0);
	}
	return "count: " @ %count;
}

package Sprinklers
{
	function doGrowCalculations(%brick, %db)
	{
		if (!isObject(%brick))
		{
			return parent::doGrowCalculations(%brick, %db);
		}
		%db = %brick.getDatablock();
		%pos = %brick.getPosition();
		%dirt = getWord(containerRaycast(%pos, vectorSub(%pos, "0 0" SPC %db.brickSizeZ), $TypeMasks::fxBrickObjectType, %brick), 0);
		if (!isObject(%dirt))
		{
			if ($debugOn)
				echo("no dirt");
			return parent::doGrowCalculations(%brick, %db);
		}
		%dirtDB = %dirt.getDatablock();
		if (%dirt.waterLevel < %dirtDB.maxWater && isObject(%dirt.sprinkler) && !%dirt.sprinkler.isDead)
		{
			%diff = %dirtDB.maxWater - %dirt.waterLevel;
			%amt = drawWater(%dirt.sprinkler, %diff);
			if (%amt > 0)
			{
				%dirt.setWaterLevel(%dirt.waterLevel + %amt);
			}
		}

		return parent::doGrowCalculations(%brick, %db);
	}

	function fxDTSBrick::onAdd(%obj)
	{
		if (%obj.getDatablock().isSprinkler && %obj.isPlanted)
		{
			schedule(33, %obj, doSprinklerSearch, %obj);
		}
		
		if (%obj.getDatablock().isWaterTank && %obj.isPlanted)
		{
			schedule(100, %obj, setWaterTankName, %obj);
		}

		return parent::onAdd(%obj);
	}

	function fxDTSBrick::setNTObjectName(%obj, %name)
	{
		if ((%obj.getDatablock().isWaterTank || %obj.getDatablock().isSprinkler) && %obj.settingName == -1)
		{
			return 0;
		}
		%obj.settingName = -1;
		return parent::setNTObjectName(%obj, %name);
	}

	function Armor::onTrigger(%this, %obj, %trig, %val)
	{
		if (isObject(%obj.getMountedImage(0)) && %obj.getMountedImage(0).getName() $= SprinklerLinkImage && %trig == 4 && %val == 1)
		{
			%obj.sprinklerLinkMode = "Open - click to link";
			%obj.sprinklerLinkObj = "";
			return;
		}
		return parent::onTrigger(%this, %obj, %trig, %val);
	}
	
	function fxDTSBrick::onDeath(%obj)
	{
		%ret = parent::onDeath(%obj);
		// %obj.isDead = 1;
		if (%obj.getDatablock().isSprinkler && %obj.isPlanted)
		{
			SprinklerSimSet.remove(%obj);
			%obj.removedFromSimset = 1;
		}
		return %ret;
	}

	function fxDTSBrick::onRemove(%obj)
	{
		%ret = parent::onRemove(%obj);
		if (%obj.getDatablock().isSprinkler && !%obj.removedFromSimset && %obj.isPlanted)
		{
			SprinklerSimSet.remove(%obj);
		}
		return %ret;
	}
};
activatePackage(Sprinklers);

function drawWater(%sprinkler, %targetAmt)
{
	%waterSource = %sprinkler.getName() @ "Tank";
	if (!isObject(%waterSource))
	{
		return 0;
	}

	if (%waterSource.getDatablock().isInfiniteWaterSource)
	{
		if (%waterSource.getDatablock().maxDispense !$= "")
		{
			return getMin(%waterSource.getDatablock().maxDispense, %targetAmt);
		}
		else
		{
			return %targetAmt;
		}
	}

	%max = getMin(%waterSource.waterLevel, %sprinkler.getDatablock().maxDispense);
	if (%max < %targetAmt)
	{
		%waterSource.setWaterLevel(%waterSource.waterLevel - %max);
		return %max;
	}
	else
	{
		%waterSource.setWaterLevel(%waterSource.waterLevel - %targetAmt);
		return %targetAmt;
	}
}

function setWaterTankName(%obj)
{
	if (%obj.getName() !$= "")
	{
		return;
	}

	%rand = "_" @ getSubStr(sha1(getRandom()), 0, 20) @ "Tank";
	while (isObject(%rand))
	{
		%rand = "_" @ getSubStr(sha1(getRandom()), 0, 20) @ "Tank";
	}
	%obj.settingName = 1;
	%obj.setNTObjectName(%rand);
	%obj.settingName = -1;
}

function getSprinklerCount(%brick)
{
	%db = %brick.getDatablock();
	if (!%db.isWaterTank)
	{
		return -1;
	}
	else if ($Sim::Time - %brick.lastCheckedSprinklerTime < 1)
	{
		return %brick.lastCheckedSprinklerCount TAB %brick.lastCheckedSprinklerList;
	}

	%name = %brick.getName();
	%name = getSubStr(%name, 0, strLen(%name) - 4); //get rid of the "Tank"

	%count = 0;
	while (isObject(%name)) //sprinkler names are the tank name minus "Tank"
	{
		%sprinkler[%count] = %name.getID();
		%name.setName("temp");
		%count++;
	}

	for (%i = 0; %i < %count; %i++)
	{
		%sprinkler[%i].setName(%name);
		%final = %final SPC %sprinkler[%i];
	}

	%brick.lastCheckedSprinklerCount = %count;
	%brick.lastCheckedSprinklerList = trim(%final);
	%brick.lastCheckedSprinklerTime = $Sim::Time;
	return %count TAB trim(%final);
}

datablock ItemData(SprinklerLinkItem : HammerItem)
{
	iconName = "Add-Ons/Server_Farming/crops/icons/sprinklerHose";
	shapeFile = "./sprinklerHoseItem.dts";
	uiName = "Sprinkler Hose";
	image = SprinklerLinkImage;

	doColorShift = 1;
	colorShiftColor = "0.75 0.56 0.35 1";
};

datablock ShapeBaseImageData(SprinklerLinkImage)
{
	shapeFile = "./sprinklerHose.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = SprinklerLinkItem.colorShiftColor;

	item = SprinklerLinkItem;

	armReady = 1;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";
	stateWaitForTimeout[1] = false;

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";
	stateWaitForTimeout[2] = false;

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "LoopA";
	stateTimeoutValue[3] = 0.1;
	stateWaitForTimeout[3] = true;
};

function SprinklerLinkImage::onLoop(%this, %obj, %slot)
{
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

	%suffix = " <br>";
	%postSuffix = " <br>";
	if (isObject(%hit = getWord(%ray, 0)))
	{
		%db = %hit.getDatablock();

		if (%db.isSprinkler)
		{
			%tank = %hit.getName() @ "Tank";
			%suffix = %suffix @ %db.uiName;
			if (isObject(%tank) && %tank.getDatablock().isWaterTank)
			{
				%tankDB = %tank.getDatablock();
				if (!isObject(%hit.tempShapeLine))
				{
					%hit.tempShapeLine = drawLine(%hit.getPosition(), %tank.getPosition(), "1 1 0 1", 0.05);
				}
				else
				{
					%hit.tempShapeLine.drawLine(%hit.getPosition(), %tank.getPosition(), "1 1 0 1", 0.05);
				}

				cancel(%hit.tempShapeLine.deleteSched);
				%hit.tempShapeLine.deleteSched = %hit.tempShapeLine.schedule(200, delete);

				%postSuffix = %postSuffix @ "Linked to " @ %tankDB.uiName @ "<color:cccccc> (" @ %tank.waterLevel + 0 @ "/" @ %tankDB.maxWater @ ")";
			}
		}
		else if (%db.isWaterTank)
		{
			%sprinklers = getSprinklerCount(%hit);
			%list = getField(%sprinklers, 1);
			%count = getWord(%sprinklers, 0);
			for (%i = 0; %i < getWordCount(%list); %i++)
			{
				%next = getWord(%list, %i);
				if (!isObject(%next) || %next.isDead)
				{
					continue;
				}
				if (!isObject(%next.tempShapeLine))
				{
					%next.tempShapeLine = drawLine(%next.getPosition(), %hit.getPosition(), "0 1 0 1", 0.05);
				}
				else
				{
					%next.tempShapeLine.drawLine(%next.getPosition(), %hit.getPosition(), "0 1 0 1", 0.05);
				}

				cancel(%next.tempShapeLine.deleteSched);
				%next.tempShapeLine.deleteSched = %next.tempShapeLine.schedule(200, delete);
			}

			%suffix = %suffix @ %db.uiName;
			%postSuffix = %postSuffix @ "<color:cccccc> (" @ %hit.waterLevel + 0 @ "/" @ %db.maxWater @ ") <color:ffff00>" @ %count @ "/" @ %db.maxSprinklers @ " Sprinklers Connected";
		}
	}

	%sprinklerLinkMode = %obj.sprinklerLinkMode;
	if (%sprinklerLinkMode $= "" || !isObject(%obj.sprinklerLinkObj))
	{
		%sprinklerLinkMode = "Open - click to link";
		%obj.sprinklerLinkObj = "";
		%obj.sprinklerLinkMode = %sprinklerLinkMode;
	}
	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("<color:ffff00>Link Mode: <br><color:ffffff>" @ %sprinklerLinkMode @ %suffix @ %postSuffix @ %obj.postPostSuffix @ " ", 1);
	}
	return %hit;
}

function SprinklerLinkImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 6));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);

	%suffix = " <br>";
	%postSuffix = " <br>";
	%postPostSuffix = " <br>";
	if (isObject(%hit = getWord(%ray, 0)))
	{
		%db = %hit.getDatablock();

		if (%db.isSprinkler)
		{
			%tank = %hit.getName() @ "Tank";
			%suffix = %suffix @ %db.uiName;
			if (isObject(%tank) && %tank.getDatablock().isWaterTank)
			{
				%tankDB = %tank.getDatablock();
				if (!isObject(%hit.tempShapeLine))
				{
					%hit.tempShapeLine = drawLine(%hit.getPosition(), %tank.getPosition(), "1 1 0 1", 0.05);
				}

				cancel(%hit.tempShapeLine.deleteSched);
				%hit.tempShapeLine.deleteSched = %hit.tempShapeLine.schedule(200, delete);

				%postSuffix = %postSuffix @ "Linked to " @ %tankDB.uiName @ "<color:cccccc> (" @ %tank.waterLevel + 0 @ "/" @ %tankDB.maxWater @ ")";
			}
		}
		else if (%db.isWaterTank)
		{
			%suffix = %suffix @ %db.uiName;
			%postSuffix = %postSuffix @ "<color:cccccc> (" @ %hit.waterLevel + 0 @ "/" @ %db.maxWater @ ")";
		}
	}

	if (isObject(%hit) && (%db.isWaterTank || %db.isSprinkler) && getTrustLevel(%hit, %obj) > 0)
	{
		%mode = getWord(%obj.sprinklerLinkMode, 0);
		switch$ (%mode)
		{
			case "Open":
				%obj.sprinklerLinkMode = "Connecting " @ %db.uiName @ " - right click to cancel";
				%obj.sprinklerLinkObj = %hit;
			case "Connecting":
				if (%obj.sprinklerLinkObj.getDatablock().isSprinkler != %db.isSprinkler)
				{
					%tank = %db.isSprinkler ? %obj.sprinklerLinkObj : %hit;
					%dist = vectorDist(getWords(%obj.sprinklerLinkObj.getPosition(), 0, 1), getWords(%hit.getPosition(), 0, 1));
					%maxDist = %tank.getDatablock().maxDistance <= 0 ? $SprinklerMaxDistance : %tank.getDatablock().maxDistance;
					if (%dist > %maxDist)
					{
						%postPostSuffix = %postPostSuffix @ "<color:ff0000>Can't link - objects too far apart! Max: " @ %maxDist;
					}
					else if (getSprinklerCount(%tank) >= %tank.getDatablock().maxSprinklers)
					{
						%postPostSuffix = %postPostSuffix @ "<color:ff0000>Can't link - tank has too many sprinklers! Max: " @ %tank.getDatablock().maxSprinklers;
					}
					else
					{
						if (%db.isSprinkler)
						{
							%hit.settingName = 1;
							%hit.setNTObjectName(strReplace(%obj.sprinklerLinkObj.getName(), "Tank", ""));
							%hit.settingName = -1;
						}
						else
						{
							%obj.sprinklerLinkObj.settingName = 1;
							%obj.sprinklerLinkObj.setNTObjectName(strReplace(%hit.getName(), "Tank", ""));
							%obj.sprinklerLinkObj.settingName = -1;
						}
						%postPostSuffix = %postPostSuffix @ "<color:ffff00>Linked " @ %obj.sprinklerLinkObj.getDatablock().uiName @ " and " @ %db.uiName;
					}
				}
				else if (%obj.sprinklerLinkObj == %hit && %db.isSprinkler)
				{
					%hit.settingName = 1;
					%hit.setNTObjectName("");
					%hit.settingName = -1;

					%postPostSuffix = %postPostSuffix @ "<color:ffff00>Cleared " @ %db.uiName @ " water tank link";
				}
				else if (%db.isSprinkler && %obj.sprinklerLinkObj.getDatablock().isSprinkler)
				{
					%obj.sprinklerLinkObj = %hit;
					%obj.sprinklerLinkMode = "Connecting " @ %db.uiName @ " - right click to cancel";
					%postPostSuffix = %postPostSuffix @ "<color:ffff00>Updated currently selected sprinkler!";
				}
			default:
				%obj.sprinklerLinkMode = "Open - click to link";
				%obj.sprinklerLinkObj = "";
		}
	}
	else if (isObject(%hit) && !(%db.isWaterTank || %db.isSprinkler))
	{
		%postPostSuffix = %postPostSuffix @ "<color:ff0000>Invalid object! Use on sprinklers or water tanks.";
		if (%obj.sprinklerLinkObj == %hit)
		{
			%obj.sprinklerLinkObj = "";
		}
	}
	else if (isObject(%hit) && getTrustLevel(%hit, %obj) <= 0)
	{
		%postPostSuffix = %postPostSuffix @ "<color:ff0000>You need build trust for that!";
	}

	%sprinklerLinkMode = %obj.sprinklerLinkMode;
	%obj.postPostSuffix = %postPostSuffix;
	cancel(%obj.postPostSuffixSchedule);
	%obj.postPostSuffixSchedule = schedule(1500, %obj, eval, %obj @ ".postPostSuffix = \"\";");
	if (%sprinklerLinkMode $= "" || !isObject(%obj.sprinklerLinkObj))
	{
		%sprinklerLinkMode = "Open - click to link";
		%obj.sprinklerLinkObj = "";
		%obj.sprinklerLinkMode = %sprinklerLinkMode;
	}
	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("<color:ffff00>Link Mode: <br><color:ffffff>" @ %sprinklerLinkMode @ %suffix @ %postSuffix @ %obj.postPostSuffix @ " ", 1);
	}
}
