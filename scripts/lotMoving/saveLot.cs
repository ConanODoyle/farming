$Pref::Farming::SaveLot::MaxBricksPerTick = 128;

function farmingSaveInitFile(%bl_id, %type)
{
	%date = getDateTime();
	%save_date = strReplace(getWord(%date, 0), "/", "-");
	%save_time = stripChars(getWord(%date, 1), ":");

	%path = $Pref::Server::AS_["Directory"] @ %type @ "s/" @ %bl_id @ "/" @ %type @ " Autosave (" @ %bl_id @ ") - " @ %save_date @ " at " @ %save_time @ ".bls";
	%file = new FileObject();
	%file.savingBL_ID = %bl_id;
	%file.savingGroup = "brickGroup_" @ %bl_id;
	%file.path = %path;

	%file.openForWrite(%path);
	%file.writeLine("This is a Farming " @ strLwr(%type) @ " autosave! Don't modify it, you'll likely cause it to load improperly.");
	%file.writeLine("1");
	%file.writeLine("0 0 0"); // offset is 0

	for(%i = 0; %i < 64; %i++)
	{
		%color = getColorIDTable(%i);
		%file.writeLine(%color);
	}

	return %file;
}

function farmingSaveWriteBrick(%file, %brick)
{
	%brickData = %brick.getDataBlock();
	%uiName = %brickData.uiName;
	%trans = %brick.getTransform();
	%pos = vectorSub(getWords(%trans, 0, 2), $Farming::Temp::Origin[%file]);
	if (%brickData.hasPrint)
	{
		%filename = getPrintTexture(%brick.getPrintID());
		%fileBase = fileBase(%filename);
		%path = filePath(%filename);
		if (%path $= "" || %filename $= "base/data/shapes/bricks/brickTop.png")
		{
			%printTexture = "/";
		}
		else
		{
			%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
			%posA = strpos(%dirName, "_");
			%posB = strpos(%dirName, "_", %posA + 1);
			%aspectRatio = getSubStr(%dirName, %posA + 1, (%posB - %posA) - 1);
			%printTexture = %aspectRatio @ "/" @ %fileBase;
		}
	}
	else
	{
		%printTexture = "";
	}

	%isLot = %brickData.isLot || %brickData.isShopLot;
	%line = %uiName @ "\"" SPC %pos SPC %brick.getAngleID() SPC %isLot SPC %brick.getColorID() SPC %printTexture SPC %brick.getColorFxID() SPC %brick.getShapeFxID() SPC %brick.isRayCasting() SPC %brick.isColliding() SPC %brick.isRendering();
	%file.writeLine(%line);

	// if (%brick.getGroup().bl_id != 888888)
	// {
	%line = "+-OWNER" SPC %brick.getGroup().bl_id;
	%file.writeLine(%line);
	// }

	if (%brick.getName() !$= "")
	{
		%line = "+-NTOBJECTNAME" SPC %brick.getName();
		%file.writeLine(%line);
	}

	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%class = %brick.getClassName();
		%enabled = %brick.eventEnabled[%i];
		%inputName = $InputEvent_Name[%class, %brick.eventInputIdx[%i]];
		%delay = %brick.eventDelay[%i];
		if (%brick.eventTargetIdx[%i] == -1)
		{
			%targetName = -1;
			%NT = %brick.eventNT[%i];
			%targetClass = "FxDTSBrick";
		}
		else
		{
			%targetList = $InputEvent_TargetList[%class, %brick.eventInputIdx[%i]];
			%target = getField(%targetList, %brick.eventTargetIdx[%i]);
			%targetName = getWord(%target, 0);
			%targetClass = getWord(%target, 1);
			%NT = "";
		}
		%outputName = $OutputEvent_Name[%targetClass, %brick.eventOutputIdx[%i]];
		%line = "+-EVENT" TAB %i TAB %enabled TAB %inputName TAB %delay TAB %targetName TAB %NT TAB %outputName;
		%par1 = %brick.eventOutputParameter[%i, 1];
		%par2 = %brick.eventOutputParameter[%i, 2];
		%par3 = %brick.eventOutputParameter[%i, 3];
		%par4 = %brick.eventOutputParameter[%i, 4];
		%line = %line TAB %par1 TAB %par2 TAB %par3 TAB %par4;
		%file.writeLine(%line);
	}

	if (isObject(%brick.emitter))
	{
		%line = "+-EMITTER" SPC %brick.emitter.getEmitterDataBlock().uiName @ "\" " @ %brick.emitterDirection;
		%file.writeLine(%line);
	}
	else if (%brick.emitterDirection != 0)
	{
		%line = "+-EMITTER NONE\" " @ %brick.emitterDirection;
		%file.writeLine(%line);
	}

	if (isObject(%brick.light))
	{
		%line = "+-LIGHT" SPC %brick.light.getDataBlock().uiName @ "\"" SPC %brick.light.Enable;
		%file.writeLine(%line);
	}

	if (isObject(%brick.Item))
	{
		%line = "+-ITEM" SPC %brick.Item.getDataBlock().uiName @ "\" " @ %brick.itemPosition SPC %brick.itemDirection SPC %brick.itemRespawnTime;
		%file.writeLine(%line);
	}

	else if ((%brick.itemDirection != 2 && %brick.itemDirection !$= "") || %brick.itemPosition != 0 || (%brick.itemRespawnTime != 0 && %brick.itemRespawnTime != 4000))
	{
		%line = "+-ITEM NONE\" " @ %brick.itemPosition SPC %brick.itemDirection SPC %brick.itemRespawnTime;
		%file.writeLine(%line);
	}

	if (isObject(%brick.AudioEmitter))
	{
		%line = "+-AUDIOEMITTER" SPC %brick.AudioEmitter.getProfileId().uiName @ "\" ";
		%file.writeLine(%line);
	}

	if (isObject(%brick.VehicleSpawnMarker))
	{
		%line = "+-VEHICLE" SPC %brick.VehicleSpawnMarker.getUiName() @ "\" " @ %brick.VehicleSpawnMarker.getReColorVehicle();
		%file.writeLine(%line);
	}
}

function capitalizeFirstChar(%string) {
	return strUpr(getSubStr(%string, 0, 1)) @ strLwr(getSubStr(%string, 1, -1));
}

function farmingSaveEnd(%file, %type, %delete)
{
	// clean up global variables
	deleteVariables("$Farming::Temp::Origin" @ %file);

	$Farming::Temp::BrickSet[%file].delete();
	$Farming::Temp::BrickQueue[%file].delete();
	deleteVariables("$Farming::Temp::BrickSet" @ %file);
	deleteVariables("$Farming::Temp::BrickQueue" @ %file);

	$Pref::Farming::Last[%type @ "Autosave" @ %file.savingBL_ID] = %file.path;
	exportServerPrefs();

	%file.savingGroup.isSaveClearingLot = false;

	messageAll('', "\c6Finished saving \c2" @ %file.savingGroup.name @ "\c6's " @ strLwr(%type) @ ".");

	%file.close();

	if ($Farming::Reload[%file.savingBL_ID] !$= "")
	{
		%lot = getWord($Farming::Reload[%file.savingBL_ID], 0);
		%rotation = getWord($Farming::Reload[%file.savingBL_ID], 1);
		$Farming::Reload[%file.savingBL_ID] = "";
		call("load" @ %type, %file.savingBL_ID, %lot, %rotation);
	}

	%file.delete(); // deletes the file object, *not* the file
}

function farmingSaveWriteRecursive(%file, %type, %delete, %brickIndex)
{
	%brickCount = $Farming::Temp::BrickSet[%file].getCount();
	if (%brickCount <= %brickIndex)
	{
		// save is written
		return farmingSaveEnd(%file, %type, %delete);
	}

	// write the save!
	for (%bricksThisTick = 0; %bricksThisTick < $Pref::Farming::SaveLot::MaxBricksPerTick && $Farming::Temp::BrickSet[%file].getCount() > 0; %bricksThisTick++)
	{
		%brick = $Farming::Temp::BrickSet[%file].getObject(0);

		farmingSaveWriteBrick(%file, %brick, $Pref::Farming::SaveLot::MaxBricksPerTick);

		$Farming::Temp::BrickSet[%file].remove(%brick);

		if (%delete)
		{
			if (%type $= "Lot" && %brick.getDatablock().isLot)
			{
				brickGroup_888888.add(%brick);
				brickGroup_888888.lotList = brickGroup_888888.lotList SPC %brick;

				%numLots = getWordCount(%file.savingGroup.lotList);
				for (%i = 0; %i < %numLots; %i++)
				{
					%lot = getWord(%file.savingGroup.lotList, %i);
					if (%lot $= %brick)
					{
						%file.savingGroup.lotList = removeWord(%file.savingGroup.lotList, %i);
						%file.savingGroup.lotCount--;
						break;
					}
				}

				if (!%brick.getDatablock().isSingle)
				{
					%brick.setDatablock(brick32x32LotRaisedData);
				}

				fixLotColor(%brick);
			}
			else if (%type $= "Shop" && %brick.getDataBlock().isShopLot)
			{
				%brick.getGroup().shopLot = "";
				brickGroup_888888.add(%brick);

				fixShopLotColor(%brick);
			}
			else
			{
				%brick.skipSell = true;
				%brick.delete();
			}
		}
	}

	if ($Farming::Temp::BrickSet[%file].getCount() <= 0)
	{
		// save is written
		return farmingSaveEnd(%file, %type, %delete);
	}

	// ran out of bricks this tick
	schedule(33, 0, farmingSaveWriteRecursive, %file, %type, %delete, %brickIndex);
}

function farmingSaveGatherSingleBrick(%file, %brick)
{
	%file.linecount++;
	if (%brick.bl_id !$= "")
	{
		%file.linecount++;
	}
	if (%brick.getName() !$= "")
	{
		%file.linecount++;
	}

	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%file.linecount++;
	}

	if (isObject(%brick.emitter))
	{
		%file.linecount++;
	}
	else if (%brick.emitterDirection != 0)
	{
		%file.linecount++;
	}
	if (isObject(%brick.light))
	{
		%file.linecount++;
	}
	if (isObject(%brick.Item))
	{
		%file.linecount++;
	}
	else if ((%brick.itemDirection != 2 && %brick.itemDirection !$= "") || %brick.itemPosition != 0 || (%brick.itemRespawnTime != 0 && %brick.itemRespawnTime != 4000))
	{
		%file.linecount++;
	}
	if (isObject(%brick.AudioEmitter))
	{
		%file.linecount++;
	}
	if (isObject(%brick.VehicleSpawnMarker))
	{
		%file.linecount++;
	}

	$Farming::Temp::BrickSet[%file].add(%brick);
}

function farmingSaveGatherBricksRecursive(%file, %type, %lots, %delete, %index, %rootBrick, %searchDown, %iterPos, %brickCount)
{
	// get bricks and save each recursively on a schedule
	if (!%searchDown)
	{
		// big scary for block that just loops over up bricks while we have MaxBricksPerTick to spare
		for (
			%brick = %rootBrick.getUpBrick(%iterPos);
			isObject(%brick) && %brickCount < $Pref::Farming::SaveLot::MaxBricksPerTick;
			%brick = %rootBrick.getUpBrick(%iterPos++)
		)
		{
			if (%brick.getGroup() == brickGroup_888888.getID()) continue;
			$Farming::Temp::BrickQueue[%file].add(%brick);
			%brickCount++;
		}

		if (%brickCount >= $Pref::Farming::SaveLot::MaxBricksPerTick)
		{
			// ran out of bricks to work with this tick
			schedule(33, 0, farmingSaveGatherBricksRecursive, %file, %type, %lots, %delete, %index, %rootBrick, false, %iterPos, 0);
			return;
		}

		// if we got past the above, it means we got all up bricks and have more bricks to go - reset iteration position for down bricks
		%searchDown = true;
		%iterPos = 0;
	}

	// big scary for block that just loops over down bricks while we have MaxBricksPerTick to spare
	for (
		%brick = %rootBrick.getDownBrick(%iterPos);
		isObject(%brick) && %brickCount < $Pref::Farming::SaveLot::MaxBricksPerTick;
		%brick = %rootBrick.getDownBrick(%iterPos++)
	)
	{
		if (%brick.getGroup() == brickGroup_888888.getID()) continue;
		$Farming::Temp::BrickQueue[%file].add(%brick);
		%brickCount++;
	}

	if (%brickCount >= $Pref::Farming::SaveLot::MaxBricksPerTick)
	{
		// ran out of bricks to work with this tick
		schedule(33, 0, farmingSaveGatherBricksRecursive, %file, %type, %lots, %delete, %index, %rootBrick, true, %iterPos, 0);
		return;
	}

	// add the root to the processed bricks list because we've processed it
	farmingSaveGatherSingleBrick(%file, %rootBrick);

	// we finished, move to the next brick
	if ($Farming::Temp::BrickQueue[%file].getCount() > 0)
	{
		// don't re-process any bricks because duh
		for (
			%nextRoot = $Farming::Temp::BrickQueue[%file].getObject(0);
			$Farming::Temp::BrickSet[%file].isMember(%nextRoot);
			%nextRoot = $Farming::Temp::BrickQueue[%file].getObject(0)
		)
		{
			$Farming::Temp::BrickQueue[%file].remove(%nextRoot);
			if ($Farming::Temp::BrickQueue[%file].getCount() <= 0)
			{
				%nextRoot = "";
				break;
			}
		}
	}

	if (isObject(%nextRoot))
	{
		// we still have more bricks for this lot!
		farmingSaveGatherBricksRecursive(%file, %type, %lots, %delete, %index, %nextRoot, false, 0, %brickCount);
	}
	else
	{
		// no more bricks for this lot
		schedule(33, 0, farmingSaveGatherBricks, %file, %type, %lots, %delete, %index + 1);
	}
}

// pass me a single lot like farmingSaveGatherBricks(file, lots, delete, type) :)
function farmingSaveGatherBricks(%file, %type, %lots, %delete, %index)
{
	%lot = getWord(%lots, %index);
	if (isObject(%lot))
	{
		messageAll('', "\c6Gathering from \c3lot " @ (%index + 1) @ "\c6...");
		farmingSaveGatherBricksRecursive(%file, %type, %lots, %delete, %index, %lot);
		return;
	}
	else
	{
		if (%index < getWordCount(%lots))
		{
			messageAll('', "\c3Lot " @ (%index + 1) @ "\c0 not found!");
		}
		%file.writeLine("Linecount " @ %file.linecount);
		messageAll('', "\c6Saving " @ (%delete ? "and unloading " : "") @ "\c2" @ %file.savingGroup.name @ "\c6's " @ strLwr(%type) @ "... (" @ $Farming::Temp::BrickSet[%file].getCount() @ " bricks)");
		farmingSaveWriteRecursive(%file, %type, %delete);
	}
}

function farmingSaveLot(%bl_id, %delete)
{
	%brickGroup = "brickGroup_" @ %bl_id;

	if (!isObject(%brickGroup))
	{
		error("ERROR: farmingSaveLot - Client has no brickGroup! " @ %client);
		echo("ERROR: farmingSaveLot - Client has no brickGroup! " @ %client);
		return -1;
	}

	%brickGroup.refreshLotList();

	%lots = %brickGroup.lotList;
	%numLots = getWordCount(%lots);

	if (%numLots <= 0)
	{
		error("ERROR: farmingSaveLot - Client's brickGroup has no lots!");
		echo("ERROR: farmingSaveLot - Client's brickGroup has no lots!");
		return -1;
	}

	for (%i = 0; %i < %numLots; %i++)
	{
		%lot = getWord(%lots, %i);
		if (%lot.getDatablock().isSingle)
		{
			%singleLot = %lot;
			break;
		}
	}

	if (!isObject(%singleLot))
	{
		error("ERROR: farmingSaveLot - Client's brickGroup doesn't have a center lot!");
		echo("ERROR: farmingSaveLot - Client's brickGroup doesn't have a center lot!");
		return -1;
	}

	%brickGroup.isSaveClearingLot = true;

	%file = farmingSaveInitFile(%bl_id, "Lot");

	$Farming::Temp::Origin[%file] = %singleLot.position;
	$Farming::Temp::BrickQueue[%file] = new SimSet();
	$Farming::Temp::BrickSet[%file] = new SimSet();
	messageAll('', "\c6Gathering bricks from \c2" @ %brickGroup.name @ "'s brickGroup\c6, found \c3" @ getWordCount(%lots) @ " lots\c6...");
	farmingSaveGatherBricks(%file, "Lot", %lots, %delete);
}














$Pref::Farming::SaveLot::MaxBricksPerTick = 256;


// setup:
//	farmingSaveLot(%bl_id, %delete) - does basic checks/init, starts search for bricks
//	=> farmingSaveRecursiveCollectBricks(%col, %q, %vis) - runs loop until all bricks found
//	==> farmingSaveWriteSave(%col) - writes save given collection object
//	===> farmingSaveInitFile(%bl_id, %type) - initialized file with necessary data
//	===> postSaveClearLot(%collection) - (if delete on) clears all found bricks except the lot bricks

function postSaveClearLot(%collection)
{
	//ensure players dont get refunded for removed bricks
	//do not delete lots
	%lots = trim(%collection.foundLots SPC %collection.singleLots);
	%group = %collection.brickGroup;
	for (%i = 0; %i < getWordCount(%lots); %i++)
	{
		%lotBrick = getWord(%lots, %i);

		brickGroup_888888.add(%lotBrick);
		%collection.remove(%lotBrick);

		fixLotColor(%lotBrick);
		if(%lotBrick.getDatablock().isSingle)
		{
			
		}
		else if(%lotBrick.getDatablock().isLot)
		{
			%lotBrick.setDatablock(brick32x32LotRaisedData);
		}
	}

	%group.isSaveClearingLot = 1;
	%collection.deleteAll();
	%group.isSaveClearingLot = 0;
	%group.refreshLotList();

	if ($Farming::Reload[%collection.type @ %collection.bl_id] !$= "")
	{
		%lot = getWord($Farming::Reload[%collection.type @ %collection.bl_id], 0);
		%rotation = getWord($Farming::Reload[%collection.type @ %collection.bl_id], 1);
		deleteVariables("Farming::Reload" @ %collection.type @ %collection.bl_id);
		call("load" @ %collection.type, %collection.bl_id, %lot, %rotation);
	}

	%collection.delete();
}

function farmingSaveLot(%bl_id, %delete)
{
	%bg = "brickGroup_" @ %bl_id;
	if (!isObject(%bg))
	{
		error("ERROR: farmingSaveLot - brickGroup doesn't exist! " @ %bg);
		return 0;
	}
	%collection = new SimSet();
	%queue = new SimSet();
	%visited = new SimSet();
	if (%delete)
	{
		%collection.callbackOnComplete = "postSaveClearLot";
	}

	%bg.refreshLotList();

	%collection.bl_id = %bg.bl_id;
	%collection.brickGroup = %bg;
	%collection.lotList = %bg.lotList;
	%collection.type = "Lot"; //required for farmingSaveWriteSave/farmingSaveInitFile
	%queue.lotList = %bg.lotList;

	farmingSaveRecursiveCollectBricks(%collection, %queue, %visited);
}

function farmingSaveInitFile(%bl_id, %type)
{
	%date = getDateTime();
	%save_date = strReplace(getWord(%date, 0), "/", "-");
	%save_time = stripChars(getWord(%date, 1), ":");

	%path = $Pref::Server::AS_["Directory"] @ %type @ "s/" @ %bl_id @ "/" @ %type @ " Autosave (" @ %bl_id @ ") - " @ %save_date @ " at " @ %save_time @ ".bls";
	%file = new FileObject();
	%file.savingBL_ID = %bl_id;
	%file.savingGroup = "brickGroup_" @ %bl_id;
	%file.path = %path;

	if (!isObject(%file.savingGroup))
	{
		%file.delete();
		return 0;
	}

	%file.openForWrite(%path);
	%file.writeLine("This is a Farming " @ strLwr(%type) @ " autosave! Don't modify it, you'll likely cause it to load improperly.");
	%file.writeLine("1");
	%file.writeLine("0 0 0"); // offset is 0

	for(%i = 0; %i < 64; %i++)
	{
		%color = getColorIDTable(%i);
		%file.writeLine(%color);
	}

	return %file;
}

function farmingSaveWriteBrick(%file, %brick)
{
	%brickData = %brick.getDataBlock();
	%uiName = %brickData.uiName;
	%trans = %brick.getTransform();
	%pos = vectorSub(getWords(%trans, 0, 2), %file.lotOrigin);
	if (%brickData.hasPrint)
	{
		%filename = getPrintTexture(%brick.getPrintID());
		%fileBase = fileBase(%filename);
		%path = filePath(%filename);
		if (%path $= "" || %filename $= "base/data/shapes/bricks/brickTop.png")
		{
			%printTexture = "/";
		}
		else
		{
			%dirName = getSubStr(%path, strlen("Add-Ons/"), strlen(%path) - strlen("Add-Ons/"));
			%posA = strpos(%dirName, "_");
			%posB = strpos(%dirName, "_", %posA + 1);
			%aspectRatio = getSubStr(%dirName, %posA + 1, (%posB - %posA) - 1);
			%printTexture = %aspectRatio @ "/" @ %fileBase;
		}
	}
	else
	{
		%printTexture = "";
	}

	//use isLot/isShopLot for determining isBaseplate field
	%isLot = %brickData.isLot || %brickData.isShopLot;
	%line = %uiName @ "\"" SPC %pos SPC %brick.getAngleID() SPC %isLot SPC %brick.getColorID() SPC %printTexture SPC %brick.getColorFxID() SPC %brick.getShapeFxID() SPC %brick.isRayCasting() SPC %brick.isColliding() SPC %brick.isRendering();
	%file.writeLine(%line);

	//always write ownership
	%line = "+-OWNER" SPC %brick.getGroup().bl_id;
	%file.writeLine(%line);

	if (%brick.getName() !$= "")
	{
		%line = "+-NTOBJECTNAME" SPC %brick.getName();
		%file.writeLine(%line);
	}

	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%class = %brick.getClassName();
		%enabled = %brick.eventEnabled[%i];
		%inputName = $InputEvent_Name[%class, %brick.eventInputIdx[%i]];
		%delay = %brick.eventDelay[%i];
		if (%brick.eventTargetIdx[%i] == -1)
		{
			%targetName = -1;
			%NT = %brick.eventNT[%i];
			%targetClass = "FxDTSBrick";
		}
		else
		{
			%targetList = $InputEvent_TargetList[%class, %brick.eventInputIdx[%i]];
			%target = getField(%targetList, %brick.eventTargetIdx[%i]);
			%targetName = getWord(%target, 0);
			%targetClass = getWord(%target, 1);
			%NT = "";
		}
		%outputName = $OutputEvent_Name[%targetClass, %brick.eventOutputIdx[%i]];
		%line = "+-EVENT" TAB %i TAB %enabled TAB %inputName TAB %delay TAB %targetName TAB %NT TAB %outputName;
		%par1 = %brick.eventOutputParameter[%i, 1];
		%par2 = %brick.eventOutputParameter[%i, 2];
		%par3 = %brick.eventOutputParameter[%i, 3];
		%par4 = %brick.eventOutputParameter[%i, 4];
		%line = %line TAB %par1 TAB %par2 TAB %par3 TAB %par4;
		%file.writeLine(%line);
	}

	if (isObject(%brick.emitter))
	{
		%line = "+-EMITTER" SPC %brick.emitter.getEmitterDataBlock().uiName @ "\" " @ %brick.emitterDirection;
		%file.writeLine(%line);
	}
	else if (%brick.emitterDirection != 0)
	{
		%line = "+-EMITTER NONE\" " @ %brick.emitterDirection;
		%file.writeLine(%line);
	}

	if (isObject(%brick.light))
	{
		%line = "+-LIGHT" SPC %brick.light.getDataBlock().uiName @ "\"" SPC %brick.light.Enable;
		%file.writeLine(%line);
	}

	if (isObject(%brick.Item))
	{
		%line = "+-ITEM" SPC %brick.Item.getDataBlock().uiName @ "\" " @ %brick.itemPosition SPC %brick.itemDirection SPC %brick.itemRespawnTime;
		%file.writeLine(%line);
	}

	else if ((%brick.itemDirection != 2 && %brick.itemDirection !$= "") || %brick.itemPosition != 0 || (%brick.itemRespawnTime != 0 && %brick.itemRespawnTime != 4000))
	{
		%line = "+-ITEM NONE\" " @ %brick.itemPosition SPC %brick.itemDirection SPC %brick.itemRespawnTime;
		%file.writeLine(%line);
	}

	if (isObject(%brick.AudioEmitter))
	{
		%line = "+-AUDIOEMITTER" SPC %brick.AudioEmitter.getProfileId().uiName @ "\" ";
		%file.writeLine(%line);
	}

	if (isObject(%brick.VehicleSpawnMarker))
	{
		%line = "+-VEHICLE" SPC %brick.VehicleSpawnMarker.getUiName() @ "\" " @ %brick.VehicleSpawnMarker.getReColorVehicle();
		%file.writeLine(%line);
	}
}

// need this subcall to guarantee all lots are iterated over and collected
// each call to this function pops the first word off the lotlist and iterates over that
function farmingSaveRecursiveCollectBricks(%collection, %queue, %visited)
{
	if (getWordCount(%queue.lotList) <= 0)
	{
		%queue.clear();
		%visited.clear();
		%queue.delete();
		%visited.delete();

		%validated = validateLotLists(%collection.lotList, trim(%collection.foundLots SPC %collection.singleLots));
		if (%validated)
		{
			farmingSaveWriteSave(%collection);
		}
		else
		{
			echo("Cannot complete farming save, found lots were not the same as initial lot list!");
		}
		return;
	}

	%lot = getWord(%queue.lotList, 0);
	%queue.lotList = getWords(%queue.lotList, 1, 20);
	if (isObject(%lot) && !%visited.isMember(%lot)) //if %lot is in %visited, its been checked already
	{
		%queue.add(%lot);
		recursiveGatherBricks(%collection, %queue, %visited, "farmingSaveRecursiveCollectBricks");
		
		if (!%collection.isMember(%lot)) //add all bricks, including public if for some reason player bricks are above public ones
		{
			%lotDb = %lot.getDataBlock();
			%collection.add(%lot);
			if (%lotDb.isSingle) //record single lots for offsets + first brick saved
			{
				%collection.singleLots = trim(%collection.singleLots SPC %lot);
			}
			else if (%lotDb.isLot)
			{
				%collection.foundLots = trim(%collection.foundLots SPC %lot);
			}
		}
	}
	else
	{
		farmingSaveRecursiveCollectBricks(%collection, %queue, %visited);
	}
}

function farmingSaveWriteSave(%collection)
{
	%center = %collection.singleLots;
	if (getWordCount(%center) > 1 || !isObject(%center))
	{
		if (!isObject(%center))
		{
			talk("ERROR: farmingSaveWriteSave - no single lots found! " @ %center);
			error("ERROR: farmingSaveWriteSave - no single lots found! " @ %center);
		}
		else
		{
			talk("ERROR: farmingSaveWriteSave - multiple single lots found! " @ %center);
			error("ERROR: farmingSaveWriteSave - multiple single lots found! " @ %center);
		}

		%collection.clear();
		%collection.delete();
		return;
	}

	%file = farmingSaveInitFile(%collection.bl_id, %collection.type);
	%file.lotOrigin = %center.getPosition();
	
	//write single lot and normal lots first
	farmingSaveWriteBrick(%file, %center);
	%wrote[%center.getID()] = 1;
	for (%i = 0; %i < getWordCount(%collection.foundLots); %i++)
	{
		%lot = getWord(%collection.foundLots, %i);
		farmingSaveWriteBrick(%file, %lot);
		%wrote[%lot.getID()] = 1;
	}

	//write rest of the bricks
	%count = %collection.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%brick = %collection.getObject(%i);
		if (%wrote[%brick] == 1)
		{
			continue;
		}
		farmingSaveWriteBrick(%file, %brick);
	}

	$Pref::Farming::Last["lot" @ "Autosave" @ %collection.bl_id] = %file.path;
	%collection.fileName = %file.path;

	%file.close();
	%file.delete();

	if (isFunction(%collection.callbackOnComplete))
	{
		call(%collection.callbackOnComplete, %collection); //assume cleanup is handled in callback
	}
	else
	{
		%collection.clear();
		%collection.delete();
	}
}

function validateLotLists(%list1, %list2)
{
	%origList2 = %list2;
	for (%i1 = 0; %i1 < getWordCount(%list1); %i1++)
	{
		%lot1 = getWord(%list1, %i1);
		%foundLot = containsWord(%list2, %lot1);
		%index = getWord(%foundLot, 1);
		if (%foundLot)
		{
			%list2 = removeWord(%list2, %index);
		}
		else
		{
			talk("Cannot find " @ %lot1 @ "!");
			talk("list1: [" @ %list1 @ "]");
			talk("list2: [" @ %list2 @ "]");
			talk("origlist2: [" @ %origList2 @ "]");
			echo("Cannot find " @ %lot1 @ "!");
			echo("list1: [" @ %list1 @ "]");
			echo("list2: [" @ %list2 @ "]");
			echo("origlist2: [" @ %origList2 @ "]");
			return 0;
		}
	}
	return 1;
}







// external: (call with these parameters)
// 	%collection: SimSet - holds all found bricks
// 	%queue: SimSet - bricks that need to be checked
// 	%visited: Simset - holds all visited bricks (popped off queue and run)
//	%callback: function to call when done running the search, passes in %collection, %queue, %visited in order
// internal: (do NOT call with these parameters)
//	%rootBrick: current brick we're looking at
//	%index: current index of the upbricks we're on
//	%searchDown: searching downwards?
// skips public lotbricks
function recursiveGatherBricks(%collection, %queue, %visited, %callback, %rootBrick, %index, %searchDown)
{
	if (!isObject(%rootBrick) && %queue.getCount() <= 0) //base case, exit
	{
		if (isFunction(%callback))
		{
			call(%callback, %collection, %queue, %visited);
		}
		return;
	}
	else if (!isObject(%rootBrick)) //need a new brick to start searching on
	{
		%rootBrick = %queue.getObject(0);
		%queue.remove(%rootBrick);
		if (!%visited.isMember(%rootBrick)) //add to visited cause we're processing it now
		{
			%visited.add(%rootBrick);
		}
	}

	//search for bricks
	%index = %index + 0; //ensure value is not empty string
	%searchDown = %searchDown + 0;

	//i hate complex for loops cause the more complex the logic is the harder it is to be sure the logic is perfectly sound
	//for example, accidentally skipping the first brick due to off by one errors
	if (!%searchDown)
	{
		%func = "getUpBrick";
		%count = %rootBrick.getNumUpBricks();
	}
	else
	{
		%func = "getDownBrick";
		%count = %rootBrick.getNumDownBricks();
	}

	for (%i = 0; %i < $Pref::Farming::SaveLot::MaxBricksPerTick; %i++)
	{
		if (%index >= %count)
		{
			%index = 0;
			%searchDown++;
			break;
		}
		%next = eval("return " @ %rootbrick @ "." @ %func @ "(%index);");
		%nextGroup = %next.getGroup();
		%nextDB = %next.getDatablock();

		//skip checking/adding public bricks, and lots not owned by the brickGroup owner, AND lots not in the lotlist in %collection.lotList
		//last condition necessary for specifying what lots are valid to save
		if (%nextGroup.bl_id == 888888
			|| (%nextDB.isLot && %nextGroup.bl_id != %collection.bl_id && !containsWord(%collection.lotList, %next)))
		{
			%index++;
			continue;
		}

		if (!%collection.isMember(%next)) //add all bricks, including public if for some reason player bricks are above public ones
		{
			%collection.add(%next);
			if (%nextDB.isSingle) //record single lots for offsets + first brick saved
			{
				%collection.singleLots = trim(%collection.singleLots SPC %next);
			}
			else if (%nextDB.isLot)
			{
				%collection.foundLots = trim(%collection.foundLots SPC %next);
			}
		}
		if (!%nextDB.isPlant && !%visited.isMember(%next) && !%queue.isMember(%next)) //add non-visited, non-present, non-plant bricks to queue
		{
			%queue.add(%next);
		}

		%index++;
	}

	if (%searchDown >= 2) //done searching up (0) and down (1)
	{
		schedule(3, MissionCleanup, recursiveGatherBricks, %collection, %queue, %visited, %callback, 0, 0, 0);
	}
	else
	{
		schedule(3, MissionCleanup, recursiveGatherBricks, %collection, %queue, %visited, %callback, %rootBrick, %index, %searchDown);	
	}
}