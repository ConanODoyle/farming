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
		if (%brick.getDatablock().isWaterTank && %brick.nextDrawWater < $Sim::Time)
		{
			%amt = drawWater(%brick, 100);
			%brick.setWaterLevel(%brick.waterLevel + %amt);
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

	if (%waterObjDB.isWaterTank && %waterObj.waterLevel >= %waterObjDB.maxWater)
	{
		%waterObj.waterLevel = %waterObjDB.maxWater;
		return 0;
	}

	if (!isObject(%tank))
	{
		return 0;
	}

	%tankDB = %tank.getDatablock();
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
	if (%id $= "" && %ufid $= "")
	{
		return;
	}

	//update watersystem object name table + upFlowObj.branches
	if (%id !$= "")
	{
		WaterSystemNameTable.obj_[%id] = %flowObj;
	}
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
	%branches = strReplace(%branches, " " @ %downFlowID @ " ", " ");
	%upFlowObj.branches = trim(%branches);

	%downFlowObj.setName("_" @ %downFlowID);
}

function canLinkWaterObjects(%downFlowObj, %upFlowObj)
{
	if (!isObject(%downFlowObj) || !isObject(%upFlowObj)) // these aren't objects!
	{
		return 0;
	}

	%dfdb = %downFlowObj.getDatablock();
	%ufdb = %upFlowObj.getDatablock();

	%name = parseWaterDeviceName(%upFlowObj);
	%upFlowID = getField(%name, 0);

	%name = parseWaterDeviceName(%downFlowObj);
	%downFlowID = getField(%name, 0);
	%downFlowUFID = getField(%name, 1);

	if (%ufdb.isSprinkler) //upflow cannot be sprinkler
	{
		return -1;
	}
	else if (%dfdb.isSprinkler && !%ufdb.canConnectSprinklers) //upflow has to support sprinkler if downflow is sprinkler
	{
		return -2;
	}
	else if (%dfdb.isWaterTank && %ufdb.isWaterTank && %dfdb.isOutflowTank >= %ufdb.isOutflowTank) //upflow must be higher "tier" than downflow
	{
		return -3;
	}
	else if ((%downFlowUFID $= %upFlowID || strPos(" " @ %upFlowObj.branches @ " ", " " @ %downFlowID @ " ") >= 0)
		&& %downFlowUFID !$= "")
	{
		return -4;
	}

	%branches = %upFlowObj.branches;
	if (getWordCount(%branches) >= %ufdb.maxConnections) //upflow has maxed out connections
	{
		return -5;
	}
	return 1;
}

function canUnlinkWaterObjects(%downFlowObj, %upFlowObj)
{
	if (!isObject(%downFlowObj) || !isObject(%upFlowObj))
	{
		return 0;
	}

	%name = parseWaterDeviceName(%upFlowObj);
	%upFlowID = getField(%name, 0);

	%name = parseWaterDeviceName(%downFlowObj);
	%downFlowID = getField(%name, 0);
	%downFlowUFID = getField(%name, 1);

	if ((%upFlowID != %downFlowUFID && strPos(" " @ %upFlowObj.branches @ " ", " " @ %downFlowID @ " ") < 0)
		|| %downFlowUFID $= "") //not linked?
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
			%upFlowObj = WaterSystemNameTable.obj_[getField(%name, 1)];

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
			%upFlowObj = WaterSystemNameTable.obj_[getField(%name, 1)];
			%branches = %hit.branches;

			if (isObject(%upFlowObj) || %branches !$= "")
			{
				%obj.displaySet = drawWaterNetwork(%hit, %obj.displaySet);
			}
			else
			{
				%clearWaterNetwork = 1;
			}
		}
		else if (isObject(%obj.displaySet))
		{
			%clearWaterNetwork = 1;
			%hit = "";
			%hitDB = "";
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

	if (isObject(%obj.displaySetUpflow) && %clearUpflowNetwork)
	{
		%obj.displaySetUpflow.deleteAll();
	}

	//build display mode string
	%centerprint = "<just:right>\c3Sprinkler Hose <br>";

	if (isObject(%obj.waterLinkObj))
	{
		%waterLinkObjDB = %obj.waterLinkObj.getDatablock();
		%selected = %waterLinkObjDB.uiName;
	}
	else
	{
		%selected = "None";
	}
	%centerprint = %centerprint @ "\c6Selection: \c3" @ %selected @ " <br>";

	if (isObject(%hit))
	{
		if (%obj.waterLinkObj == %hit)
		{
			%view = "\c4Click to deselect selection";
		}
		else if (canLinkWaterObjects(%obj.waterLinkObj, %hit) > 0)
		{
			%view = "Click to link " @ %waterLinkObjDB.uiName @ " to " @ %hitDB.uiName @ "";
			%linkType = "objToHit";
		}
		else if (canLinkWaterObjects(%hit, %obj.waterLinkObj) > 0)
		{
			%view = "Click to link " @ %hitDB.uiName @ " to " @ %waterLinkObjDB.uiName @ ""; 
			%linkType = "hitToObj";
		}
		else if (canUnlinkWaterObjects(%obj.waterLinkObj, %hit) > 0)
		{
			%view = "\c0Click to unlink " @ %waterLinkObjDB.uiName @ " to " @ %hitDB.uiName @ "";
			%linkType = "objToHit";
		}
		else if (canUnlinkWaterObjects(%hit, %obj.waterLinkObj) > 0)
		{
			%view = "\c0Click to unlink " @ %hitDB.uiName @ " to " @ %waterLinkObjDB.uiName @ ""; 
			%linkType = "hitToObj";
		}
		else if (isObject(%obj.waterLinkObj))
		{
			%view = "\c5Cannot link to this object!";
		}
	}
	%centerprint = %centerprint @ "\c2[" @ %view @ "] <br>";

	if (isObject(%hit) && %hitDB.isWaterTank && %linkType $= "objToHit")
	{
		%connections = %hitDB.maxConnections;
		%count = getWordCount(%hit.branches);
	}
	else if (isObject(%obj.waterLinkObj) && %waterLinkObjDB.isWaterTank && %linkType $= "objToHit")
	{
		%connections = %waterLinkObjDB.maxConnections;
		%count = getWordCount(%waterLinkObj.branches);
	}
	if (%connections > 0)
	{
		%linkspace = "\c2[" @ %hitDB.uiname @ ": " @ %count @ " / " @ %connections @ " connections]";
	}
	%centerprint = %centerprint @ %view @ " <br>";

	if (%obj.errorTicks > 0)
	{
		%obj.errorTicks--;
		%error = %obj.errorMessage;
	}
	else
	{
		%obj.errorMessage = "";
	}
	%centerprint = %centerprint @ %error;

	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint(%centerprint, 1);
	}
	return %hit;
}

function SprinklerLinkImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	//determine what we're looking at
	//display water network if we're looking at something that's already part of one
	if (isObject(%hit))
	{
		%hitDB = %hit.getDatablock();
		if (%hitDB.isWaterTank || %hitDB.isSprinkler)
		{
			if (!isObject(%obj.waterLinkObj))
			{
				%obj.waterLinkObj = %hit;
			}
			else if (%obj.waterLinkObj == %hit)
			{
				%obj.waterLinkObj = "";
			}
		}
		// SprinklerLinkImage::onLoop(%this, %obj, %slot);
	}
	else
	{
		return;
	}

	//actual linking
	if (%obj.waterLinkObj != %hit)
	{
		if (canLinkWaterObjects(%obj.waterLinkObj, %hit) > 0)
		{
			linkWaterObjects(%obj.waterLinkObj, %hit);
		}
		else if (canLinkWaterObjects(%hit, %obj.waterLinkObj) > 0)
		{
			linkWaterObjects(%hit, %obj.waterLinkObj);
		}
		else if (canUnlinkWaterObjects(%obj.waterLinkObj, %hit) > 0)
		{
			unlinkWaterObjects(%obj.waterLinkObj, %hit);
		}
		else if (canUnlinkWaterObjects(%hit, %obj.waterLinkObj) > 0)
		{
			unlinkWaterObjects(%hit, %obj.waterLinkObj);
		}
		else
		{
			%obj.errorMessage = "Cannot link to the object!";
			%obj.errorTicks = 20;
		}
	}
}





function drawWaterNetwork(%focusObj, %simSet)
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

	%branches = %focusObj.branches;
	%upFlowID = getField(parseWaterDeviceName(%focusObj), 1);
	%upFlowObj = WaterSystemNameTable.obj_[%upFlowID];
	%focusObjPos = %focusObj.getPosition();
	for (%i = 0; %i < getWordCount(%branches); %i++)
	{
		%downFlowObj = getWord(%branches, %i);
		if (isObject(%downFlowObj))
		{
			if (!isObject(%line[%currLine]))
			{
				%line[%currLine] = drawLine(%focusObjPos, %downFlowObj.getPosition(), "1 1 1 1", 0.05);
			}
			else
			{
				%line[%currLine].drawLine(%focusObjPos, %downFlowObj.getPosition(), "1 1 1 1", 0.05);
			}
			%currLine++;
		}
	}

	if (isObject(%upFlowObj))
	{
		if (!isObject(%line[%currLine]))
		{
			%line[%currLine] = drawLine(%focusObjPos, %upFlowObj.getPosition(), "1 1 1 1", 0.05);
		}
		else
		{
			%line[%currLine].drawLine(%focusObjPos, %upFlowObj.getPosition(), "1 1 1 1", 0.05);
		}
		%currLine++;
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
