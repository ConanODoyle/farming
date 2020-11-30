$SprinklerMaxDistance = 20;

if (!isObject(WaterSystemNameTable))
{
	$WaterSystemNameTable = new SimSet(WaterSystemNameTable);
}


package Sprinklers
{
	function fxDTSBrick::onAdd(%obj)
	{
		%db = %obj.getDatablock();
		if (%obj.isPlanted && (%db.isSprinkler || %db.isWaterTank))
		{
			WaterSystemNameTable.add(%obj);
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

	function fxDTSBrick::onRemove(%obj)
	{
		%db = %obj.getDatablock();

		return parent::onRemove(%obj);
	}

	function Armor::onTrigger(%this, %obj, %trig, %val)
	{
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

	if (WaterSystemNameTable.getCount() <= 0)
	{
		$masterSprinklerSchedule = schedule(33, 0, sprinklerTick, 0);
		return;
	}

	if (%index < 0)
	{
		%index = WaterSystemNameTable.getCount() - 1;
	}
	else if (%index > WaterSystemNameTable.getCount() - 1)
	{
		%index = WaterSystemNameTable.getCount() - 1;
	}

	%max = 8;
	for (%i = 0; %i < %max; %i++)
	{
		if (%i > %index) //we reached end of simset
		{
			break;
		}
		%brick = WaterSystemNameTable.getObject(%index - %i);

		//validate water object to add to table + branches list
		if (%brick.nextValidate < $Sim::Time)
		{
			validateWaterObject(%brick);
			%brick.nextValidate = $Sim::Time + 5;
		}

		//flow water to water tanks
		if (%brick.getDatablock().isWaterTank && %brick.nextDrawWater > $Sim::Time)
		{
			drawWater(%brick, 100);
			%brick.nextDrawWater = $Sim::Time + 1;
		}
		else if (%brick.getDatablock().isSprinkler)
		{
			//run sprinklers
			if (!%brick.isDead)
			{
				doSprinklerSearch(%brick);
				doSprinklerWater(%brick);
			}
		}
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

function drawWater(%waterObj, %targetAmt)
{
	%waterObjDB = %waterObj.getDatablock();
	%name = parseWaterDeviceName(%waterObj);
	%ufid = getField(%name, 1);
	%tank = WaterSystemNameTable.obj_[%ufid];

	if (!isObject(%tank) 
		|| (%waterObjDB.isWaterTank && %waterObj.waterLevel >= %waterObjDB.maxWater))
	{
		if (%waterObjDB.isWaterTank)
		{
			%waterObj.waterLevel = %waterObjDB.maxWater;
		}
		return 0;
	}

	%tankDB = tank.getDatablock();
	%maxGive = getMin(%tank.waterLevel, %targetAmt);
	%tank.setWaterLevel(%tank.waterLevel - %maxGive);

	//refill connecting tank from extra tank/infinite source
	if (%tankDB.isInfiniteWaterSource)
	{
		%tank.waterLevel = %tankDB.maxWater;
	}

	// if (%tank.waterLevel < %tankDB.maxWater)
	// {
	// 	drawWater(%tank, %tankDB.maxWater - %tank.waterLevel);
	// }
	return %maxGive;
}









//name format: selfID_targetID
function parseWaterDeviceName(%brick)
{
	%name = getSubStr(%brick.getName(), 1, 40);

	return strReplace(%name, "_", "\t");
}

function validateWaterObject(%flowObj)
{
	%name = parseWaterDeviceName(%flowObj);
	%id = getField(%name, 0);
	%ufid = getField(%name, 1);

	//update watersystem object name table + upFlowObj.branches
	WaterSystemNameTable.obj_[%id] = %flowObj;
	%ufLinks = " " @ WaterSystemNameTable.obj_[%ufid].branches @ " ";
	if (strPos(%ufLinks, " " @ %flowObj @ " ") < 0)
	{
		WaterSystemNameTable.obj_[%ufid].branches = trim($WaterSystemNameTable.obj_[%ufid].branches SPC %flowObj);
	}

	//correct self linklist - check for existence and under max distance
	for (%i = getWordCount(%flowObj.branches) - 1; %i >= 0; %i--)
	{
		%o = getWord(%flowObj.branches, %i);
		if (!isObject(%o)) //object doesnt exist anymore
		{
			%flowObj.branches = removeWord(%flowObj.branches, %i);
		}
		else if (vectorDist(%o.getPosition(), %flowObj.getPosition()) > $SprinklerMaxDistance)
		{
			%name = parseWaterDeviceName(%o);
			%o.setName("_" @ getField(%name, 0));
			%flowObj.branches = removeWord(%flowObj.branches, %i);
		}
	}

	//remove excess links
	%count = getWordCount(%flowObj.branches);
	%max = %flowObj.getDatablock().maxConnections;
	if (%max <= 0) //no links allowed, purge all
	{
		for (%i = 0; %i < %count; %i++)
		{
			%obj = getWord(%flowObj.branches, %i);
			%name = parseWaterDeviceName(%obj);
			%obj.setName("_" @ getField(%name, 0)); //removes the upstream field while preserving its own id
		}
		%flowObj.branches = "";
	}
	else if (%count > %max) //purge earliest links - latest links are appended
	{
		%remove = getWords(%flowObj.branches, 0, %count - %max - 1);
		for (%i = 0; %i < getWordCount(%remove); %i++)
		{
			%obj = getWord(%remove, %i);
			%name = parseWaterDeviceName(%obj);
			%obj.setName("_" @ getField(%name, 0)); //removes the upstream field while preserving its own id
		}
		%flowObj.branches = getWords(%flowObj.branches, %count - %max, %count - 1);
	}
}

function linkWaterObjects(%downFlowObj, %upFlowObj)
{
	%name = parseWaterDeviceName(%upFlowObj);
	%upFlowID = getField(%name, 0);
	%upFlowUFID = getField(%name, 1);

	%name = parseWaterDeviceName(%downFlowObj);
	%downFlowID = getField(%name, 0);

	%downFlowID = (%downFlowID $= "" ? getSubStr(getRandomHash(), 0, 12) @ "WS" : %downFlowID);
	%upFlowID = (%upFlowID $= "" ? getSubStr(getRandomHash(), 0, 12) @ "WS" : %upFlowID);
	
	%downName = "_" @ %downFlowID @ "_" @ %upFlowID;
	%upName = "_" @ %upFlowID @ "_" @ %upFlowUFID;

	%downFlowObj.setName(%downName);
	%upFlowObj.setName(%upName);
}

function unlinkWaterObjects(%downFlowObj, %upFlowObj)
{
	%name = parseWaterDeviceName(%upFlowObj);
	%upFlowID = getField(%name, 0);

	%name = parseWaterDeviceName(%downFlowObj);
	%downFlowID = getField(%name, 0);
	%downFlowUFID = getField(%name, 1);

	%branches = " " @ %upFlowObj.branches @ " ";
	%branches strReplace(%branches, " " @ %downFlowID @ " ", " ");
	%upFlowObj.branches = trim(%branches);

	%downFlowObj.setName("_" @ %downFlowID);
}

function canLinkWaterObjects(%downFlowObj, %upFlowObj)
{
	%dfdb = %downFlowObj.getDatablock();
	%ufdb = %upFlowObj.getDatablock();

	%name = parseWaterDeviceName(%upFlowObj);
	%upFlowID = getField(%name, 0);

	%name = parseWaterDeviceName(%downFlowObj);
	%downFlowID = getField(%name, 0);
	%downFlowUFID = getField(%name, 1);

	if (%ufdb.isSprinkler) //upflow cannot be sprinkler
	{
		return 0;
	}
	else if (%dfdb.isSprinkler && !%ufdb.canConnectSprinklers) //upflow has to support sprinkler if downflow is sprinkler
	{
		return 0;
	}
	else if (%dfdb.isWaterTank && %ufdb.isWaterTank && %dfdb.isOutflowTank >= %ufdb.isOutflowTank) //upflow must be higher "tier" than downflow
	{
		return 0;
	}
	else if (%downFlowUFID $= %upFlowID || strPos(" " @ %upFlowObj.branches @ " ", " " @ %downFlowID @ " ") >= 0)
	{
		return 0;
	}

	%branches = %upFlowObj.branches;
	if (getWordCount(%branches) >= %ufdb.maxConnections) //upflow has maxed out connections
	{
		return 0;
	}
	return 1;
}

function canUnlinkWaterObjects(%downFlowObj, %upFlowObj)
{
	%name = parseWaterDeviceName(%upFlowObj);
	%upFlowID = getField(%name, 0);

	%name = parseWaterDeviceName(%downFlowObj);
	%downFlowID = getField(%name, 0);
	%downFlowUFID = getField(%name, 1);

	if (%upFlowID != %downFlowUFID 
		&& strPos(" " @ %upFlowObj.branches @ " ", " " @ %downFlowID @ " ") < 0) //not linked?
	{
		return 0;
	}
	return 1;
}

















datablock ItemData(SprinklerLinkItem : HammerItem)
{
	iconName = "Add-Ons/Server_Farming/icons/sprinklerHose";
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

function SprinklerLinkImage::onUnmount(%this, %obj, %slot)
{
	%obj.waterLinkObj = "";
	if (isObject(%obj.displaySet))
	{
		%obj.displaySet.deleteAll();
		%obj.displaySet.delete();
	}
}

function SprinklerLinkImage::onLoop(%this, %obj, %slot)
{
	%waterLinkObj = %obj.waterLinkObj;
	%waterLinkObjDB = isObject(%waterLinkObj) ? %waterLinkObj.getDatablock() : "";

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	//determine what we're looking at
	//display water network if we're looking at something that's already part of one
	if (isObject(%hit))
	{
		%hitDB = %hit.getDatablock();

		if (%hitDB.isSprinkler)
		{
			%name = parseWaterDeviceName(%hit);
			%upFlowObj = WaterSystemNameTable.obj_[getField(%name, 2)];
			if (isObject(%upFlowObj))
			{
				%obj.displaySet = drawWaterNetwork(%hit, %obj.displaySet);
				}
			else
			{
				%clearWaterNetwork = 1;
			}
		}
		else if (%hitDB.isWaterTank)
		{
			%name = parseWaterDeviceName(%hit);
			%upFlowObj = WaterSystemNameTable.obj_[getField(%name, 2)];
			%branches = %hit.branches;

			if (isObject(%upFlowObj) || %branches !$= "")
			{
				%obj.displaySet = drawWaterNetwork(%tankWaterID, %obj.displaySet);
			}
			else
			{
				%clearWaterNetwork = 1;
			}
		}
		else if (isObject(%obj.displaySet))
		{
			%clearWaterNetwork = 1;
		}
	}
	else
	{
		%clearWaterNetwork = 1;
	}

	if (isObject(%obj.displaySet) && %clearWaterNetwork)
	{
		%obj.displaySet.deleteAll();
	}

	//build display mode string
	%centerprint = "<just:right>\c3Sprinkler Hose <br>";

	if (%obj.sprinklerSelectedObj)
	{
		%sprinklerSelectedObjDB = %obj.sprinklerSelectedObj.getDatablock();
		%selected = %sprinklerSelectedObjDB.uiName;
	}
	else
	{
		%selected = "None";
	}
	%centerprint = %centerprint @ "\c6Selection: \c3" @ %selected @ " <br>";

	if (isObject(%hit))
	{
		if (%obj.sprinklerSelectedObj == %hit)
		{
			%view = "Click to deselect selection";
		}
		else if (canLinkWaterObjects(%obj.sprinklerSelectedObj, %hit))
		{
			%view = "Click to link " @ %sprinklerSelectedObjDB.uiName @ " to " @ %hitDB.uiName @ "";
			%linkType = "objToHit";
		}
		else if (canLinkWaterObjects(%hit, %obj.sprinklerSelectedObj))
		{
			%view = "Click to link " @ %hitDB.uiName @ " to " @ %sprinklerSelectedObjDB.uiName @ ""; 
			%linkType = "hitToObj";
		}
		else if (canUnlinkWaterObjects(%obj.sprinklerSelectedObj, %hit))
		{
			%view = "\c0Click to unlink " @ %sprinklerSelectedObjDB.uiName @ " to " @ %hitDB.uiName @ "";
			%linkType = "objToHit";
		}
		else if (canLinkWaterObjects(%hit, %obj.sprinklerSelectedObj))
		{
			%view = "\c0Click to unlink " @ %hitDB.uiName @ " to " @ %sprinklerSelectedObjDB.uiName @ ""; 
			%linkType = "hitToObj";
		}
	}
	%centerprint = %centerprint @ "\c2[" @ %view @ "] <br>";

	if (isObject(%hit) && %hitDB.isWaterTank && %linkType $= "objToHit")
	{
		%connections = %hitDB.maxConnections;
		%count = getWordCount(%hit.branches);
		if (%connections > 0)
		{
			%linkspace = %hitDB.uiname @ " - " @ %count @ " / " @ %connections @ " connections";
		}
	}
	else if (isObject(%obj.sprinklerSelectedObj) && %sprinklerSelectedObjDB.isWaterTank && %linkType $= "objToHit")
	{
		%connections = %sprinklerSelectedObjDB.maxConnections;
		%count = getWordCount(%sprinklerSelectedObj.branches);
		if (%connections > 0)
		{
			%linkspace = %sprinklerSelectedObjDB.uiname @ " - " @ %count @ " / " @ %connections @ " connections used";
		}
	}
	%centerprint = %centerprint @ "\c2[" @ %view @ "] <br>";

	if (%obj.errorTicks > 0)
	{
		%obj.errorTicks--;
		%error = %obj.errorMessage;
	}
	else
	{
		%obj.errorMessage = "";
	}

	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint(%centerprint, 1);
	}
	return %hit;
}

function SprinklerLinkImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);
	%waterLinkObj = %obj.waterLinkObj;
	%waterLinkObjDB = isObject(%waterLinkObj) ? %waterLinkObj.getDatablock() : "";

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	//determine what we're looking at
	//display water network if we're looking at something that's already part of one
	if (isObject(%hit))
	{
		%hitDB = %hit.getDatablock();

		if (%hitDB.isSprinkler || %hitDB.isWaterTank)
		{
			if (%obj.waterLinkObj == %hit)
			{
				%obj.waterLinkObj = "";
				SprinklerLinkImage::onLoop(%this, %obj, %slot);
				return;
			}
			else if (%hitDB.isSprinkler)
			{
				%obj.waterLinkObj = %hit;
				SprinklerLinkImage::onLoop(%this, %obj, %slot);
				return;
			}
		}
	}
	else
	{
		return;
	}

	//actual linking
	if (%waterLinkObjDB.isWaterTank && %hitDB.isWaterTank)
	{
		if (%waterLinkObjDB.isOutflowTank) %outTank = %waterLinkObj;
		else %drawObj = %waterLinkObj;

		if (%hitDB.isOutflowTank) %outTank = %hit;
		else %drawObj = %hit;
	}
	else if (%waterLinkObjDB.isSprinkler && %hitDB.isWaterTank)
	{
		%outTank = %hit;
		%drawObj = %waterLinkObj;
	}
	else if (%waterLinkObjDB.isWaterTank && %hitDB.isSprinkler)
	{
		%outTank = %waterLinkObj;
		%drawObj = %hit;
	}

	if (!isObject(%outTank) || !isObject(%drawObj))
	{
		%obj.errorMessage = "Cannot link " @ %waterLinkObjDB.uiName @ " to " @ %hitDB.uiName @ "!";
		%obj.errorTicks = 20;
		return;
	}
	else
	{
		%outTankDB = %outTank.getDatablock();
		%drawObjDB = %drawObj.getDatablock();

		%outDataID = getWaterTankDataID(%outTank);
		if (%outDataID $= "")
		{
			%outDataID = getWaterSystemDataID();
			addWaterTank(%outDataID, %outTank);
		}

		if (%drawObjDB.isSprinkler && %outTankDB.maxSprinklers > 0 
			&& %outTank.sprinklerCount < %outTankDB.maxSprinklers)
		{
			if (getSprinklerTankHash(%drawObj) $= %outTank.posHash)
			{
				removeSprinkler(%outDataID, %drawObj);
			}
			else
			{
				addSprinkler(%outDataID, %outTank, %drawObj);
			}
		}
		else if (%drawObjDB.isWaterTank && %outTankDB.isOutflowTank)
		{
			if (getWaterTankDataID(%drawObj) $= %outDataID)
			{
				removeWaterTank(%outDataID, %drawObj);
			}
			else
			{
				addWaterTank(%outDataID, %drawObj);
			}
			%obj.waterLinkObj = "";
		}
		else
		{
			%obj.errorMessage = "Cannot link " @ %outTankDB.uiName @ " to " @ %drawObjDB.uiName @ "!";
			%obj.errorTicks = 20;
		}
	}
}




































































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
		%db = %obj.getDatablock();
		if (%obj.isPlanted && (%db.isSprinkler || %db.isWaterTank))
		{
			SprinklerSimSet.add(%obj);
			%obj.schedule(100, autoAddWaterSystem);
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
					removeSprinkler(%waterDataID, %obj, 1);
				}
				else
				{
					removeWaterTank(%waterDataID, %obj, 1);
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

	if ($restartCheck $= "")
	{
		$restartCheck = getSubStr(getRandomHash(), 0, 5);
	}

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


		//validation checks
		if (%brick.getDatablock().isSprinkler)
		{
			%waterDataID = getSprinklerDataID(%brick);
		}
		else if (%brick.getDatablock().isWaterTank)
		{
			%waterDataID = getWaterTankDataID(%brick);
		}
		if (%waterDataID $= "") //no water system attached, no point running waterflow functions
		{
            %totalBricksProcessed++;
			continue;
		}
		
		if (getDataIDArrayTagValue(%waterDataID, "lastValidated") + 5 < $Sim::Time 
			|| getDataIDArrayTagValue(%waterDataID, "restartCheck") !$= $restartCheck)
		{
			validateWaterSystem(%waterDataID);
			setDataIDArrayTagValue(%waterDataID, "lastValidated", $Sim::Time);
			setDataIDArrayTagValue(%waterDataID, "restartCheck", $restartCheck);
		}


		//flow water to water tanks
		if (%brick.getDatablock().isWaterTank && !%brick.getDatablock().isOutflowTank)
		{
			flowWater(%brick);
		}
		else if (%brick.getDatablock().isSprinkler)
		{
			//run sprinklers
			if (!%brick.isDead)
			{
				doSprinklerSearch(%brick);
				doSprinklerWater(%brick);
			}

		}
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

function fxDTSBrick::autoAddWaterSystem(%brick)
{
	if (%brick.getDatablock().isSprinkler && %brick.getName() !$= "")
	{
		%waterDataID = getSprinklerDataID(%brick);
		%posHash = getSprinklerTankHash(%brick);
		addSprinkler(%waterDataID, "", %brick, %posHash);
	}
	else if (%brick.getDatablock().isWaterTank && %brick.getName() !$= "")
	{
		%waterDataID = getWaterTankDataID(%brick);
		addWaterTank(%waterDataID, %brick);
	}
}






function getWaterSystemDataID()
{
	return getSubStr(getRandomHash(), 0, 10) @ "WaterSys";
}

function getWaterTankDataID(%tank)
{
	if (strLen(%tank.getName()) < 2)
	{
		return "";
	}
	return getSubStr(%tank.getName(), 1, 50);
}

function getSprinklerDataID(%sprinkler)
{
	%n = %sprinkler.getName();
	if (strLen(%n) < 2 || strPos(%n, "_", 1) < 0)
	{
		return "";
	}
	return getSubStr(%n, 1, strPos(%n, "_", 1) - 1);
}

function getSprinklerTankHash(%sprinkler)
{
	%n = %sprinkler.getName();
	if (strLen(%n) < 2 || strPos(%n, "_", 1) < 0)
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
	validateWaterSystem(%waterDataID);
}

function addSprinkler(%waterDataID, %tank, %sprinkler, %posHash)
{
	if (isObject(%tank))
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
	}
	else
	{
		%dataID = getSprinklerDataID(%sprinkler);
		if (%dataID $= "")
		{
			return 1;
		}
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

	validateWaterSystem(%waterDataID);
}

function removeWaterTank(%waterDataID, %tank, %skipUnsetName)
{
	%tanks = " " @ getDataIDArrayTagValue(%waterDataID, "tanks") @ " ";
	%tanks = strReplace(%tanks, " " @ %tank.getID() @ " ", " ");
	setDataIDArrayTagValue(%waterDataID, "tanks", trim(%tanks));
	if (!%skipUnsetName) //fix for insta crash due to unsetting name during onRemove
	{
		%tank.setName("");
		validateWaterSystem(%waterDataID);
	}
}

function removeSprinkler(%waterDataID, %sprinkler, %skipUnsetName)
{
	%sprinklers = " " @ getDataIDArrayTagValue(%waterDataID, "sprinklers") @ " ";
	%sprinklers = strReplace(%sprinklers, " " @ %sprinkler.getID() @ " ", " ");
	setDataIDArrayTagValue(%waterDataID, "sprinklers", trim(%sprinklers));
	if (!%skipUnsetName) //fix for insta crash due to unsetting name during onRemove
	{
		%sprinkler.setName("");
		validateWaterSystem(%waterDataID);
	}
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
		if (!isObject(%tank) || !%tank.getDatablock().isWaterTank 
			|| getWaterTankDataID(%tank) !$= %waterDataID)
		{
			continue;
		}

		%tankDB = %tank.getDatablock();
		if (%tankDB.maxSprinklers > 0)
		{
			if (%tank.posHash $= "")
			{
				%posHash = getSubStr(sha1(%tank.getPosition()), 0, 8);
				%tank.posHash = %posHash;
			}
			else
			{
				%posHash = %tank.posHash;
			}
			%sprinklerCount[%posHash] = %tankDB.maxSprinklers;
			%tank[%posHash] = %tank;
			%tank.sprinklerCount = 0;
		}
		%finalTanks = %finalTanks SPC %tank;
	}

	for (%i = 0; %i < %sprinklerCount; %i++)
	{
		%sprinkler = getWord(%sprinklers, %i);
		if (!isObject(%sprinkler) || !%sprinkler.getDatablock().isSprinkler
			|| getSprinklerDataID(%sprinkler) !$= %waterDataID)
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
			%tank[%posHash].sprinklerCount++;
			%finalSprinklers = %finalSprinklers SPC %sprinkler;
		}
	}
	%remove = trim(%remove);

	for (%i = 0; %i < getWordCount(%remove); %i++)
	{
		removeSprinkler(%waterDataID, getWord(%remove, %i));
	}
	setDataIDArrayTagValue(%waterDataID, "tanks", trim(%finalTanks));
	setDataIDArrayTagValue(%waterDataID, "sprinklers", trim(%finalSprinklers));
}

function drawWater(%sprinkler, %targetAmt, %rateModifier)
{
	%waterDataID = getSprinklerDataID(%sprinkler);
	%posHash = getSprinklerTankHash(%sprinkler);
	%tanks = getDataIDArrayTagValue(%waterDataID, "tanks");
	%tankCount = getWordCount(%tanks);

	//transfer water to connecting tank
	%maxOutflow = 0;
	%bestOutflow = 0;
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
		else if (%tank.getDatablock().isOutflowTank && %tank.waterLevel > %maxOutflow)
		{
			%maxOutflow = %tank.waterLevel;
			%bestOutflow = %tank;
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
	if ((%diff = %connectingTank.getDatablock().maxWater - %connectingTank.waterLevel) > 0 && isObject(%bestOutflow))
	{
		%give = getMin(%diff, %bestOutflow.waterLevel);
		%connectingTank.setWaterLevel(%connectingTank.waterLevel + %give);
		%bestOutflow.setWaterLevel(%bestOutflow.waterLevel - %give);
	}
	return %maxGive;
}

function flowWater(%tank)
{
	if (%tank.getDatablock().isOutflowTank || %tank.waterLevel == %tank.getDatablock().maxWater)
	{
		return;
	}

	%waterDataID = getWaterTankDataID(%tank);
	%tanks = getDataIDArrayTagValue(%waterDataID, "tanks");
	%tankCount = getWordCount(%tanks);

	%maxOutflow = 0;
	%bestOutflow = 0;
	for (%i = 0; %i < %tankCount; %i++)
	{
		%searchTank = getWord(%tanks, %i);
		if (!isObject(%searchTank) || %tank == %searchTank)
		{
			continue;
		}
		else if (%searchTank.getDatablock().isOutflowTank && %searchTank.waterLevel > %maxOutflow)
		{
			%maxOutflow = %searchTank.waterLevel;
			%bestOutflow = %searchTank;
		}
	}

	if (!isObject(%bestOutflow))
	{
		return;
	}

	if (%bestOutflow.getDatablock().isInfiniteWaterSource)
	{
		%bestOutflow.waterLevel = %bestOutflow.getDatablock().maxWater;
	}
	if ((%diff = %tank.getDatablock().maxWater - %tank.waterLevel) > 0 && isObject(%bestOutflow))
	{
		%give = getMin(%diff, %bestOutflow.waterLevel);
		%tank.setWaterLevel(%tank.waterLevel + %give);
		%bestOutflow.setWaterLevel(%bestOutflow.waterLevel - %give);
	}
}







datablock ItemData(SprinklerLinkItem : HammerItem)
{
	iconName = "Add-Ons/Server_Farming/icons/sprinklerHose";
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

function SprinklerLinkImage::onUnmount(%this, %obj, %slot)
{
	%obj.waterLinkObj = "";
	if (isObject(%obj.displaySet))
	{
		%obj.displaySet.deleteAll();
		%obj.displaySet.delete();
	}
}

function SprinklerLinkImage::onLoop(%this, %obj, %slot)
{
	%waterLinkObj = %obj.waterLinkObj;
	%waterLinkObjDB = isObject(%waterLinkObj) ? %waterLinkObj.getDatablock() : "";

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	//determine what we're looking at
	//display water network if we're looking at something that's already part of one
	if (isObject(%hit))
	{
		%hitDB = %hit.getDatablock();

		if (%hitDB.isSprinkler)
		{
			%sprinklerWaterID = getSprinklerDataID(%hit);
			if (%sprinklerWaterID !$= "")
			{
				%obj.displaySet = drawWaterNetwork(%sprinklerWaterID, %obj.displaySet, %hit);
				%hitStatus = "\c2Connected";
			}
			else
			{
				%hitStatus = "Not connected";
				if (isObject(%obj.displaySet))
				{
					%obj.displaySet.deleteAll();
				}
			}
		}
		else if (%hitDB.isWaterTank)
		{
			%tankWaterID = getWaterTankDataID(%hit);
			if (%tankWaterID !$= "")
			{
				if (%hitDB.isOutflowTank) %obj.displaySet = drawWaterNetwork(%tankWaterID, %obj.displaySet);
				else %obj.displaySet = drawWaterNetwork(%tankWaterID, %obj.displaySet, %hit);
				%hitStatus = "\c2Connected";
				if (%hitDB.maxSprinklers > 0)
				{
					%hitStatus = %hitStatus @ " - " @ %hit.sprinklerCount @ "/" @ %hitDB.maxSprinklers
						@ " sprinkler" @ (%hitDB.maxSprinklers > 0 ? "s" : "");
				}
			}
			else
			{
				%hitStatus = "\c0Not connected";

				if (isObject(%obj.displaySet))
				{
					%obj.displaySet.deleteAll();
				}
			}
		}
		else if (isObject(%obj.displaySet))
		{
			%obj.displaySet.deleteAll();
		}
	}
	else if (isObject(%obj.displaySet))
	{
		%obj.displaySet.deleteAll();
	}

	//build display mode string
	if (%waterLinkObjDB $= "")
	{
		%mode = "Selection: [Click to select]";
	}
	else if (%waterLinkObj == %hit)
	{
		%mode = "\c0Selection: [Click to deselect]";
	}
	else
	{
		%mode = "Selection: " @ %waterLinkObjDB.uiName;
		if (%waterLinkObjDB.isWaterTank)
		{
			if (%waterLinkObjDB.maxSprinklers > 0)
			{
				%mode = %mode @ " - " @ %waterLinkObj.sprinklerCount @ "/" @ %waterLinkObjDB.maxSprinklers
					@ " sprinkler" @ (%waterLinkObjDB.maxSprinklers > 0 ? "s" : "");
			}
			else if (%waterLinkObjDB.isOutflowTank)
			{
				%mode = %mode @ " - Outflow tank: connect to another tank";
			}
		}
		else if (%waterLinkObjDB.isSprinkler)
		{
			%mode = %mode @ ": connect to a tank";
		}
	}

	if (isObject(%hit))
	{
		%mode = %mode @ " \n\c6" @ %hitDB.uiName @ " \n" @ %hitStatus;
	}

	if (%obj.errorTicks > 0)
	{
		%obj.errorTicks--;
		%mode = %mode @ " \n\c0" @ %obj.errorMessage;
	}
	else
	{
		%obj.errorMessage = "";
	}

	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("\c2Water Linker: <br>\c3" @ %mode @ " ", 1);
	}
	return %hit;
}

function SprinklerLinkImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);
	%waterLinkObj = %obj.waterLinkObj;
	%waterLinkObjDB = isObject(%waterLinkObj) ? %waterLinkObj.getDatablock() : "";

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	//determine what we're looking at
	//display water network if we're looking at something that's already part of one
	if (isObject(%hit))
	{
		%hitDB = %hit.getDatablock();

		if (%hitDB.isSprinkler || %hitDB.isWaterTank)
		{
			if (%obj.waterLinkObj == %hit)
			{
				%obj.waterLinkObj = "";
				SprinklerLinkImage::onLoop(%this, %obj, %slot);
				return;
			}
			else if (%hitDB.isSprinkler)
			{
				%obj.waterLinkObj = %hit;
				SprinklerLinkImage::onLoop(%this, %obj, %slot);
				return;
			}
		}
	}
	else
	{
		return;
	}

	//actual linking
	if (%waterLinkObjDB.isWaterTank && %hitDB.isWaterTank)
	{
		if (%waterLinkObjDB.isOutflowTank) %outTank = %waterLinkObj;
		else %drawObj = %waterLinkObj;

		if (%hitDB.isOutflowTank) %outTank = %hit;
		else %drawObj = %hit;
	}
	else if (%waterLinkObjDB.isSprinkler && %hitDB.isWaterTank)
	{
		%outTank = %hit;
		%drawObj = %waterLinkObj;
	}
	else if (%waterLinkObjDB.isWaterTank && %hitDB.isSprinkler)
	{
		%outTank = %waterLinkObj;
		%drawObj = %hit;
	}

	if (!isObject(%outTank) || !isObject(%drawObj))
	{
		%obj.errorMessage = "Cannot link " @ %waterLinkObjDB.uiName @ " to " @ %hitDB.uiName @ "!";
		%obj.errorTicks = 20;
		return;
	}
	else
	{
		%outTankDB = %outTank.getDatablock();
		%drawObjDB = %drawObj.getDatablock();

		%outDataID = getWaterTankDataID(%outTank);
		if (%outDataID $= "")
		{
			%outDataID = getWaterSystemDataID();
			addWaterTank(%outDataID, %outTank);
		}

		if (%drawObjDB.isSprinkler && %outTankDB.maxSprinklers > 0 
			&& %outTank.sprinklerCount < %outTankDB.maxSprinklers)
		{
			if (getSprinklerTankHash(%drawObj) $= %outTank.posHash)
			{
				removeSprinkler(%outDataID, %drawObj);
			}
			else
			{
				addSprinkler(%outDataID, %outTank, %drawObj);
			}
		}
		else if (%drawObjDB.isWaterTank && %outTankDB.isOutflowTank)
		{
			if (getWaterTankDataID(%drawObj) $= %outDataID)
			{
				removeWaterTank(%outDataID, %drawObj);
			}
			else
			{
				addWaterTank(%outDataID, %drawObj);
			}
			%obj.waterLinkObj = "";
		}
		else
		{
			%obj.errorMessage = "Cannot link " @ %outTankDB.uiName @ " to " @ %drawObjDB.uiName @ "!";
			%obj.errorTicks = 20;
		}
	}
}

function drawWaterNetwork(%waterDataID, %simSet, %focusObj)
{
	if (!isObject(%simSet))
	{
		%simSet = new SimSet(WaterNetworkShapes);
	}

	for (%i = 0; %i < %simSet.getCount(); %i++)
	{
		%line[%i] = %simSet.getObject(%i);
	}
	%totalLines = %simSet.getCount();
	%currLine = 0;

	//determine focus object
	if (isObject(%focusObj))
	{
		%focusObjDB = %focusObj.getDatablock();
		if (%focusObjDB.isSprinkler)
		{
			%focusSprinkler = %focusObj;
		}
		else if (%focusObjDB.isWaterTank && !%focusObjDB.isOutflowTank)
		{
			%offset = "0 0 " @ %focusObjDB.brickSizeZ * 0.1;
			%focusTank = %focusObj;
			%focusTankPos = vectorAdd(%focusObj.getPosition(), %offset);
		}
	}

	%tanks = getDataIDArrayTagValue(%waterDataID, "tanks");
	%sprinklers = getDataIDArrayTagValue(%waterDataID, "sprinklers");
	%tankCount = getWordCount(%tanks);
	%sprinklerCount = getWordCount(%sprinklers);

	//collect tanks
	for (%i = 0; %i < %tankCount; %i++)
	{
		%tank = getWord(%tanks, %i);
		if (!isObject(%tank) || !%tank.getDatablock().isWaterTank
			|| (isObject(%focusTank) && %focusTank != %tank && !%tank.getDatablock().isOutflowTank))
		{
			continue;
		}

		%tankDB = %tank.getDatablock();
		if (%tank.posHash $= "")
		{
			%posHash = getSubStr(sha1(%tank.getPosition()), 0, 8);
			%tank.posHash = %posHash;
		}
		else
		{
			%posHash = %tank.posHash;
		}

		%offset = "0 0 " @ %tankDB.brickSizeZ * 0.1;
		%linePos = vectorAdd(%tank.getPosition(), %offset);
		if (%tankDB.maxSprinklers > 0)
		{
			%sprinklerTank[%sprinklerTankCount++ - 1] = %linePos;
		}
		else
		{
			%outflowTank[%outflowTankCount++ - 1] = %linePos;
		}

		%sprinklerCount[%posHash] = %tankDB.maxSprinklers;
		%position[%posHash] = %linePos;
		if (isObject(%focusTank) && %tank != %focusTank)
		{
			if (!isObject(%line[%currLine]))
			{
				%line[%currLine] = drawLine(%focusTankPos, %position[%posHash], "1 1 1 1", 0.05);
			}
			else
			{
				%line[%currLine].drawLine(%focusTankPos, %position[%posHash], "1 1 1 1", 0.05);
			}
			%currLine++;
		}
		%finalTanks = %finalTanks SPC %tank;
	}


	if (!isObject(%focusObj))
	{
		for (%i = 0; %i < %sprinklerTankCount; %i++)
		{
			%p1 = %sprinklerTank[%i];
			for (%j = 0; %j < %outflowTankCount; %j++)
			{
				%p2 = %outflowTank[%j];

				if (!isObject(%line[%currLine]))
				{
					%line[%currLine] = drawLine(%p1, %p2, "1 1 1 1", 0.05);
				}
				else
				{
					%line[%currLine].drawLine(%p1, %p2, "1 1 1 1", 0.05);
				}
				%currLine++;
			}
		}
	}


	//collect sprinklers
	for (%i = 0; %i < %sprinklerCount; %i++)
	{
		%sprinkler = getWord(%sprinklers, %i);
		if (!isObject(%sprinkler) || !%sprinkler.getDatablock().isSprinkler
			|| (isObject(%focusSprinkler) && %focusSprinkler != %sprinkler))
		{
			continue;
		}

		%posHash = getSprinklerTankHash(%sprinkler);
		%sprinklerPos = %sprinkler.getPosition();
		if (%sprinklerCount[%posHash] > 0)
		{
			%sprinklerCount[%posHash]--;
			if (!isObject(%line[%currLine]))
			{
				%line[%currLine] = drawLine(%sprinklerPos, %position[%posHash], "1 1 0 1", 0.05);
			}
			else
			{
				%line[%currLine].drawLine(%sprinklerPos, %position[%posHash], "1 1 0 1", 0.05);
			}
			%currLine++;
		}
	}

	//add lines to simset
	if ($waterNetworkDebug)
	{
		talk("totalLines: " @ %totalLines);
		talk("currLine: " @ %currLine);
		%q = 0;
		while (%line[%q] !$= "")
		{
			talk(%q @ ": " @ %line[%q]);
			%q++;
		}
	}
	%simSet.clear();
	for (%i = 0; %i < %currLine; %i++)
	{
		if ($waterNetworkDebug) talk("Adding " @ %line[%i]);
		%simSet.add(%line[%i]);
	}
	for (%i = %currLine; %i < %totalLines; %i++)
	{
		if ($waterNetworkDebug) talk("Deleting " @ %line[%i]);
		%line[%i].delete();
	}

	return %simSet;
}