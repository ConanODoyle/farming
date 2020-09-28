//todo: disable naming sprinkler and watertower bricks
//brickDB.directionalOffset = Vector3F TU;
//brickDB.boxSize = Point3f Size;
//brickDB.waterPerSecond = int;

exec("./bricks.cs");

$SprinklerMaxDistance = 20;
$SprinklerWaterTime = 500;
$DebugOn = 0;

if (!isObject(SprinklerSimSet))
{
	$SprinklerSimSet = new SimSet(SprinklerSimSet) {};
}


package Sprinklers
{
	function fxDTSBrick::onAdd(%obj)
	{
		if (%obj.getDatablock().isSprinkler && %obj.isPlanted)
		{
			SprinklerSimSet.add(%obj);
		}
		
		return parent::onAdd(%obj);
	}

	function fxDTSBrick::onRemove(%obj)
	{
		%db = %obj.getDatablock();
		if (%db.isSprinkler || %db.isWaterTank)
		{
			%waterDataID = %db.isSprinkler ? getSprinklerDataID(%obj) : getWaterTankDataID(%obj);
			if (%waterDataID !$= "")
			{
				if (%db.isSprinkler)
				{
					removeSprinkler(%waterDataID, %obj);
				}
				else
				{
					removeWaterTank(%waterDataID, %obj);
				}
			}
		}

		return parent::onRemove(%obj);
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
};
activatePackage(Sprinklers);

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

		%waterDataID = getSprinklerDataID(%brick);
		
		if (%waterDataID $= "") //no water system attached, no point running sprinkler functions
		{
			continue;
		}

		if ($restartCheck $= "")
		{
			$restartCheck = getSubStr(getRandomHash(), 0, 5);
		}

		if (getDataIDArrayTagValue(%waterDataID, "lastValidated") + 5 < $Sim::Time 
			|| getDataIDArrayTagValue(%waterDataID, "restartCheck") != $restartCheck)
		{
			validateWaterSystem(%waterDataID);
			setDataIDArrayTagValue(%waterDataID, "lastValidated", $Sim::Time);
			setDataIDArrayTagValue(%waterDataID, "restartCheck", $restartCheck);
		}

		if (!%brick.isDead)
		{
			doSprinklerSearch(%brick);
		}

		doSprinklerWater(%brick);

		%totalBricksProcessed++;
	}

	$masterSprinklerSchedule = schedule(33, 0, sprinklerTick, %index - %totalBricksProcessed);
}

function doSprinklerWater(%sprinkler)
{
	if ((%sprinkler.lastWater + $SprinklerWaterTime | 0) > getSimTime()) //add water every half second
	{
		return;
	}

	%list = %sprinkler.dirtList;
	%count = getWordCount(%list);
	%total = %sprinkler.getDatablock().waterPerSecond * ($SprinklerWaterTime / 1000);

	%thirstyDirtCount = 0;
	%totalThirst = 0;
	for (%i = 0; %i < %count; %i++)
	{
		%dirt = getWord(%list, %i);
		if (isObject(%dirt))
		{
			%diff = %dirt.getDatablock().maxWater - %dirt.waterLevel;
			%totalThirst += %diff;
			if (%diff > 0)
			{
				%dirt[%thirstyDirtCount] = %dirt;
				%dirtThirst[%thirstyDirtCount] = %diff;
				%thirstyDirtCount++;
			}
		}
	}

	for (%i = 0; %i < %thirstyDirtCount; %i++)
	{
		%dirt = %dirt[%i];
		%thirst = %dirtThirst[%i];
		%ratioWaterAmt = getMin(%thirst, mFloor(%thirst / %totalThirst * %total + 0.5));
		%debugStr = %debugStr @ "\n" @ %ratioWaterAmt SPC %thirst SPC %totalThirst;

		%amt = drawWater(%sprinkler, %ratioWaterAmt);
		%dirt.setWaterLevel(%dirt.waterLevel + %amt);
		if (%amt > 0)
		{
			%totalDispensed += %amt;
			$totalWaterDispensed += %amt;
			%dispensedCount++;
		}
	}

	if (%dispensedCount > 0)
	{
		%sprinkler.setEmitter("ChromePaintDropletEmitter");
	}
	else
	{
		%sprinkler.setEmitter("");
	}
	%sprinkler.lastWater = getSimTime();
	return %dispensedCount;
}

function doSprinklerSearch(%sprinkler)
{
	if (%sprinkler.lastSprinklerSearch + 3 > $Sim::Time)
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

	if ($debugOn && %sprinkler == $compare)
	{
		echo("    " @ %sprinkler @ " Box search");
		echo("    Box: " @ %box);
		echo("    Pos: " @ %sprinkler.getPosition());
		echo("    db: " @ %db);
	}
	%sprinkler.dirtList = "";
	initContainerBoxSearch(vectorAdd(%offset, %sprinkler.getPosition()), %box, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()) && %safety++ < 1500)
	{
		if (%next.getDatablock().isDirt && getTrustLevel(%next, %sprinkler) > 0)
		{
			%next.sprinkler = %sprinkler;
			%sprinkler.dirtList = %sprinkler.dirtList SPC %next;
			%count++;
		}
	}
	%sprinkler.dirtList = trim(%sprinkler.dirtList);
	if ($debugOn && %sprinkler == $compare)
		echo("    " @ %sprinkler @ " Box search complete (Safety: " @ %safety @ ")");

	if (%safety >= 1500)
	{
		talk("doSprinklerSearch bypassed safety");
		echo("doSprinklerSearch bypassed safety");
	}

	%sprinkler.lastSprinklerSearch = $Sim::Time;

	// SprinklerSimSet.add(%sprinkler); FUCK YOU PAST CONAN FOR PUTTING THIS UNNECESSARY LINE IN
	if (%db.noCollide)
	{
		%sprinkler.setColliding(0);
	}
	return "count: " @ %count;
}


function getWaterSystemDataID()
{
	return getSubStr(getRandomHash(), 0, 10) @ "WaterSys";
}

function getWaterTankDataID(%tank)
{
	if (strLen(%tank.getName()) < 2)
	{
		return 1;
	}
	return getSubStr(%tank.getName(), 1, 50);
}

function getSprinklerDataID(%sprinkler)
{
	%n = %sprinkler.getName();
	if (strLen(%n) < 2 || strPos(%n, "_", 1))
	{
		return 1;
	}
	return getSubStr(%n, 1, strPos(%n, "_", 1) - 1);
}

function getSprinklerTankHash(%sprinkler)
{
	%n = %sprinkler.getName();
	if (strLen(%n) < 2 || strPos(%n, "_", 1))
	{
		return 1;
	}
	return getSubStr(%n, strPos(%n, "_", 1) + 1, 50);
}

function addWaterTank(%waterDataID, %tank)
{
	%oldDataID = getWaterTankDataID(%tank);
	if (%oldDataID !$= "")
	{
		removeWaterTank(%oldDataID, %tank);
	}

	%tanks = getDataIDArrayTagValue(%waterDataID, "tanks");
	%tanks = trim(%tanks SPC %tank.getID());

	setDataIDArrayTagValue(%waterDataID, "tanks", %tanks);
	%tank.setName("_" @ %waterDataID);
	%tank.posHash = getSubStr(sha1(%tank.getPosition()), 0, 8);
}

function addSprinkler(%waterDataID, %tank, %sprinkler)
{
	if (vectorDist(%tank.getPosition(), %sprinkler.getPosition()) > $SprinklerMaxDistance)
	{
		return 1;
	}

	%dataID = getWaterTankDataID(%tank);
	if (%dataID !$= %waterDataID)
	{
		return 1;
	}

	%posHash = getSubStr(sha1(%tank.getPosition()), 0, 8);
	if (%tank.posHash !$= "" && %tank.posHash !$= %posHash)
	{
		talk("Tank position hash mismatch!");
		return 1;
	}
	else
	{
		%tank.posHash = %posHash;
	}

	%oldDataID = getSprinklerDataID(%sprinkler);
	if (%oldDataID !$= "")
	{
		removeSprinkler(%oldDataID, %sprinkler);
	}
	%sprinklers = getDataIDArrayTagValue(%waterDataID, "sprinklers");
	%sprinklers = trim(%sprinklers SPC %sprinkler.getID());

	setDataIDArrayTagValue(%waterDataID, "sprinklers", %sprinklers);
	%sprinkler.setName("_" @ %waterDataID @ "_" @ %posHash);
}

function removeWaterTank(%waterDataID, %tank)
{
	%tanks = " " @ getDataIDArrayTagValue(%waterDataID, "tanks") @ " ";
	%tanks = strReplace(%tanks, " " @ %tank.getID() @ " ", " ");
	setDataIDArrayTagValue(%waterDataID, "tanks", trim(%tanks));
	%tank.setName("");
}

function removeSprinkler(%waterDataID, %sprinkler)
{
	%sprinklers = " " @ getDataIDArrayTagValue(%waterDataID, "sprinklers") @ " ";
	%sprinklers = strReplace(%sprinklers, " " @ %sprinkler.getID() @ " ", " ");
	setDataIDArrayTagValue(%waterDataID, "sprinklers", trim(%sprinklers));
	%sprinkler.setName("");
}

function validateWaterSystem(%waterDataID)
{
	%tanks = getDataIDArrayTagValue(%waterDataID, "tanks");
	%sprinklers = getDataIDArrayTagValue(%waterDataID, "sprinklers");
	%tankCount = getWordCount(%tanks);
	%sprinklerCount = getWordCount(%sprinklers);

	for (%i = 0; %i < %tankCount; %i++)
	{
		%tank = getWord(%tanks, %i);
		if (!isObject(%tank))
		{
			continue;
		}

		%tankDB = %tank.getDatablock();
		if (%tankDB.maxSprinklers > 0)
		{
			%posHash = getSubStr(sha1(%tank.getPosition()), 0, 8);
			%tank.posHash = %posHash;
			%sprinklerCount[%posHash] = %tankDB.maxSprinklers;
		}
		%finalTanks = %finalTanks SPC %tank;
	}

	for (%i = 0; %i < %sprinklerCount; %i++)
	{
		%sprinkler = getWord(%sprinklers, %i);
		if (!isObject(%sprinkler))
		{
			continue;
		}

		%posHash = getSprinklerTankHash(%sprinkler);
		if (%sprinklerCount[%posHash] <= 0)
		{
			%remove = %remove SPC %sprinkler;
		}
		else
		{
			%sprinklerCount[%posHash]--;
			%finalSprinklers = %finalSprinklers SPC %sprinkler;
		}
	}
	%remove = trim(%remove);

	for (%i = 0; %i < getWordCount(%remove); %i++)
	{
		removeSprinkler(%waterDataID, getWord(%remove, %i));
	}
	setDataIDArrayTagValue(%waterDataID, "tanks", trim(%tanks));
	setDataIDArrayTagValue(%waterDataID, "sprinklers", trim(%sprinklers));
}

function drawWater(%sprinkler, %targetAmt, %rateModifier)
{
	%waterDataID = getSprinklerDataID(%sprinkler);
	%posHash = getSprinklerTankHash(%sprinkler);
	%tanks = getDataIDArrayTagValue(%waterDataID, "tanks");
	%tankCount = getWordCount(%tanks);

	//transfer water to connecting tank
	%maxExtra = 0;
	%bestExtra = 0;
	%connectingTank = 0;
	for (%i = 0; %i < %tankCount; %i++)
	{
		%tank = getWord(%tanks, %i);
		if (!isObject(%tank))
		{
			continue;
		}

		if (%tank.posHash $= %posHash)
		{
			%connectingTank = %tank;
		}
		else if (%tank.getDatablock().isExtraTank && %tank.waterLevel > %maxExtra)
		{
			%maxExtra = %tank.waterLevel;
			%bestExtra = %tank;
		}
	}

	//no connecting tank, this isn't a valid part of the system anymore, break
	if (!isObject(%connectingTank))
	{
		return 0;
	}

	//draw water based on water rate
	%sprinklerDB = %sprinkler.getDatablock();
	%maxGive = getMin(%connectingTank.waterLevel, %targetAmt);

	%connectingTank.setWaterLevel(%connectingTank.waterLevel - %maxGive);


	//refill connecting tank from extra tank
	if (%connectingTank.getDatablock().isInfiniteWaterSource)
	{
		%connectingTank.waterLevel = %connectingTank.getDatablock().maxWater;
	}
	if ((%diff = %connectingTank.getDatablock().maxWater - %connectingTank.waterLevel) > 0 && isObject(%bestExtra))
	{
		%give = getMin(%diff, %maxExtra);
		%connectingTank.setWaterLevel(%connectingTank.waterLevel + %give);
		%bestExtra.setWaterLevel(%bestExtra.waterLevel - %give);
	}
	return %maxGive;
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
			%postSuffix = %postSuffix @ "<color:cccccc> (" @ %hit.waterLevel + 0 @ "/" @ %db.maxWater @ ") \c2" @ %count @ "/" @ %db.maxSprinklers @ " Sprinklers Connected";
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
		%cl.centerprint("\c2Link Mode: <br><color:ffffff>" @ %sprinklerLinkMode @ %suffix @ %postSuffix @ %obj.postPostSuffix @ " ", 1);
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
						%postPostSuffix = %postPostSuffix @ "\c0Can't link - objects too far apart! Max: " @ %maxDist;
					}
					else if (getSprinklerCount(%tank) >= %tank.getDatablock().maxSprinklers)
					{
						%postPostSuffix = %postPostSuffix @ "\c0Can't link - tank has too many sprinklers! Max: " @ %tank.getDatablock().maxSprinklers;
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
						%postPostSuffix = %postPostSuffix @ "\c2Linked " @ %obj.sprinklerLinkObj.getDatablock().uiName @ " and " @ %db.uiName;
					}
				}
				else if (%obj.sprinklerLinkObj == %hit && %db.isSprinkler)
				{
					%hit.settingName = 1;
					%hit.setNTObjectName("");
					%hit.settingName = -1;

					%postPostSuffix = %postPostSuffix @ "\c2Cleared " @ %db.uiName @ " water tank link";
				}
				else if (%db.isSprinkler && %obj.sprinklerLinkObj.getDatablock().isSprinkler)
				{
					%obj.sprinklerLinkObj = %hit;
					%obj.sprinklerLinkMode = "Connecting " @ %db.uiName @ " - right click to cancel";
					%postPostSuffix = %postPostSuffix @ "\c2Updated currently selected sprinkler!";
				}
			default:
				%obj.sprinklerLinkMode = "Open - click to link";
				%obj.sprinklerLinkObj = "";
		}
	}
	else if (isObject(%hit) && !(%db.isWaterTank || %db.isSprinkler))
	{
		%postPostSuffix = %postPostSuffix @ "\c0Invalid object! Use on sprinklers or water tanks.";
		if (%obj.sprinklerLinkObj == %hit)
		{
			%obj.sprinklerLinkObj = "";
		}
	}
	else if (isObject(%hit) && getTrustLevel(%hit, %obj) <= 0)
	{
		%postPostSuffix = %postPostSuffix @ "\c0You need build trust for that!";
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
		%cl.centerprint("\c2Link Mode: <br><color:ffffff>" @ %sprinklerLinkMode @ %suffix @ %postSuffix @ %obj.postPostSuffix @ " ", 1);
	}
}
