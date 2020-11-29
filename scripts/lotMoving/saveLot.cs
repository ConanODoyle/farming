$Pref::Farming::SaveLot::MaxBricksPerTick = 128;

function farmingSaveLotInitFile(%bl_id)
{
	%date = getDateTime();
	%save_date = strReplace(getWord(%date, 0), "/", "-");
	%save_time = stripChars(getWord(%date, 1), ":");

	%path = $Pref::Server::AS_["Directory"] @ "Lots/" @ %bl_id @ "/Lot Autosave (" @ %bl_id @ ") - " @ %save_date @ " at " @ %save_time @ ".bls";
	%file = new FileObject();
	%file.savingBL_ID = %bl_id;
	%file.savingGroup = "BrickGroup_" @ %bl_id;
	%file.path = %path;

	%file.openForWrite(%path);
	%file.writeLine("This is a Farming lot autosave! Don't modify it, you'll likely cause it to load improperly.");
	%file.writeLine("1");
	%file.writeLine("0 0 0"); // offset is 0

	for(%i = 0; %i < 64; %i++)
	{
		%color = getColorIDTable(%i);
		%file.writeLine(%color);
	}

	return %file;
}

function farmingSaveLotWriteBrick(%file, %brick)
{
	%brickData = %brick.getDataBlock();
	%uiName = %brickData.uiName;
	%trans = %brick.getTransform();
	%pos = vectorSub(getWords(%trans, 0, 2), $Farming::Temp::LotOrigin[%file]);
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

	%line = %uiName @ "\"" SPC %pos SPC %brick.getAngleID() SPC %brick.isBasePlate() SPC %brick.getColorID() SPC %printTexture SPC %brick.getColorFxID() SPC %brick.getShapeFxID() SPC %brick.isRayCasting() SPC %brick.isColliding() SPC %brick.isRendering();
	%file.writeLine(%line);

	if (%brick.getGroup().bl_id != %file.savingBL_ID)
	{
		%line = "+-OWNER" SPC %brick.getGroup().bl_id;
		%file.writeLine(%line);
	}

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

function farmingSaveLotEnd(%file)
{
	// clean up global variables
	deleteVariables("$Farming::Temp::LotOrigin" @ %file);

	$Farming::Temp::LotBrickSet[%file].delete();
	$Farming::Temp::LotBrickQueue[%file].delete();
	deleteVariables("$Farming::Temp::LotBrickSet" @ %file);
	deleteVariables("$Farming::Temp::LotBrickQueue" @ %file);

	$Pref::Farming::LastLotAutosave[%file.savingBL_ID] = %file.path;

	%file.savingGroup.isSaveClearingLot = false;

	messageAll('', "\c6Finished saving \c2" @ %file.savingGroup.name @ "\c6's lot.");

	%file.close();

	if ($Farming::ReloadLot[%file.savingBL_ID] !$= "")
	{
		%lot = getWord($Farming::ReloadLot[%file.savingBL_ID], 0);
		%rotation = getWord($Farming::ReloadLot[%file.savingBL_ID], 1);
		$Farming::ReloadLot[%file.savingBL_ID] = "";
		loadLot(%file.savingBL_ID, %lot, %rotation);
	}

	%file.delete(); // deletes the file object, *not* the file
}

function farmingSaveLotWriteSaveRecursive(%file, %delete, %brickIndex)
{
	%brickCount = $Farming::Temp::LotBrickSet[%file].getCount();
	if (%brickCount <= %brickIndex)
	{
		// save is written
		return farmingSaveLotEnd(%file);
	}

	// write the save!
	for (%bricksThisTick = 0; %bricksThisTick < $Pref::Farming::SaveLot::MaxBricksPerTick && $Farming::Temp::LotBrickSet[%file].getCount() > 0; %bricksThisTick++)
	{
		%brick = $Farming::Temp::LotBrickSet[%file].getObject(0);

		farmingSaveLotWriteBrick(%file, %brick);

		$Farming::Temp::LotBrickSet[%file].remove(%brick);

		if (%delete)
		{
			if (%brick.getDatablock().isLot)
			{
				BrickGroup_888888.add(%brick);
				BrickGroup_888888.lotList = BrickGroup_888888.lotList SPC %brick;

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
			else
			{
				%brick.skipSell = true;
				%brick.delete();
			}
		}
	}

	if ($Farming::Temp::LotBrickSet[%file].getCount() <= 0)
	{
		// save is written
		return farmingSaveLotEnd(%file, %delete);
	}

	// ran out of bricks this tick
	schedule(33, 0, farmingSaveLotWriteSaveRecursive, %file, %delete, %brickIndex);
}

function farmingSaveLotGatherSingleBrick(%file, %brick)
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

	$Farming::Temp::LotBrickSet[%file].add(%brick);
}

function farmingSaveLotGatherBricksRecursive(%file, %lots, %delete, %index, %rootBrick, %searchDown, %brickCount)
{
	// get bricks and save each recursively on a schedule
	if (!%searchDown)
	{
		// big scary for block that just loops over up bricks while we have MaxBricksPerTick to spare
		for (
			%brick = %rootBrick.getUpBrick(%upBrickIndex = 0);
			isObject(%brick) && %brickCount < $Pref::Farming::SaveLot::MaxBricksPerTick;
			%brick = %rootBrick.getUpBrick(%upBrickIndex++)
		)
		{
			$Farming::Temp::LotBrickQueue[%file].add(%brick);
			%brickCount++;
		}
	}

	%searchDown = true;

	// big scary for block that just loops over down bricks while we have MaxBricksPerTick to spare
	for (
		%brick = %rootBrick.getDownBrick(%downBrickIndex = 0);
		isObject(%brick) && %brickCount < $Pref::Farming::SaveLot::MaxBricksPerTick;
		%brick = %rootBrick.getDownBrick(%downBrickIndex++)
	)
	{
		$Farming::Temp::LotBrickQueue[%file].add(%brick);
		%brickCount++;
	}

	if (%brickCount >= $Pref::Farming::SaveLot::MaxBricksPerTick)
	{
		// ran out of bricks to work with this tick
		schedule(33, 0, farmingSaveLotGatherBricksRecursive, %file, %lots, %delete, %index, %rootBrick, %searchDown, 0);
		return;
	}
	else
	{
		// add the root to the processed bricks list because we've processed it
		farmingSaveLotGatherSingleBrick(%file, %rootBrick);

		// we finished, move to the next brick
		if ($Farming::Temp::LotBrickQueue[%file].getCount() > 0)
		{
			// don't re-process any bricks because duh
			for (
				%nextRoot = $Farming::Temp::LotBrickQueue[%file].getObject(0);
				$Farming::Temp::LotBrickSet[%file].isMember(%nextRoot);
				%nextRoot = $Farming::Temp::LotBrickQueue[%file].getObject(0)
			)
			{
				$Farming::Temp::LotBrickQueue[%file].remove(%nextRoot);
				if ($Farming::Temp::LotBrickQueue[%file].getCount() <= 0)
				{
					%nextRoot = "";
					break;
				}
			}
		}

		if (isObject(%nextRoot))
		{
			// we still have more bricks for this lot!
			farmingSaveLotGatherBricksRecursive(%file, %lots, %delete, %index, %nextRoot, false, %brickCount);
		}
		else
		{
			// no more bricks for this lot
			schedule(33, 0, farmingSaveLotGatherBricks, %file, %lots, %delete, %index + 1);
		}
	}
}

function farmingSaveLotGatherBricks(%file, %lots, %delete, %index)
{
	%lot = getWord(%lots, %index);
	if (isObject(%lot))
	{
		farmingSaveLotGatherBricksRecursive(%file, %lots, %delete, %index, %lot, false, 0);
		return;
	}
	else
	{
		%file.writeLine("Linecount " @ %file.linecount);
		messageAll('', "\c6Saving " @ (%delete ? "and unloading " : "") @ "\c2" @ %file.savingGroup.name @ "\c6's lot... (" @ $Farming::Temp::LotBrickSet[%file].getCount() @ " bricks)");
		farmingSaveLotWriteSaveRecursive(%file, %delete, 0);
	}
}

function farmingSaveLot(%bl_id, %delete)
{
	%brickGroup = "BrickGroup_" @ %bl_id;

	if (!isObject(%brickGroup))
	{
		error("ERROR: farmingSaveLot - Client has no brickgroup! " @ %client);
		echo("ERROR: farmingSaveLot - Client has no brickgroup! " @ %client);
		return -1;
	}

	%brickGroup.isSaveClearingLot = true;

	%lots = %brickGroup.lotList;
	%numLots = getWordCount(%lots);

	if (%numLots <= 0)
	{
		error("ERROR: farmingSaveLot - Client's brickgroup has no lots!");
		echo("ERROR: farmingSaveLot - Client's brickgroup has no lots!");
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
		error("ERROR: farmingSaveLot - Client's brickgroup doesn't have a center lot!");
		echo("ERROR: farmingSaveLot - Client's brickgroup doesn't have a center lot!");
		return -1;
	}

	%file = farmingSaveLotInitFile(%bl_id);

	$Farming::Temp::LotOrigin[%file] = %singleLot.position;
	$Farming::Temp::LotBrickQueue[%file] = new SimSet();
	$Farming::Temp::LotBrickSet[%file] = new SimSet();
	farmingSaveLotGatherBricks(%file, %lots, %delete, 0);
}