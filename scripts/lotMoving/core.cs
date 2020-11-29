function unloadLot(%bl_id)
{
	if (hasLoadedLot(%bl_id) != 1)
	{
		talk("ERROR: unloadLot - BLID " @ %bl_id @ " does not have a lot loaded!");
		error("ERROR: unloadLot - BLID " @ %bl_id @ " does not have a lot loaded!");
		return -2;
	}

	%bg = "Brickgroup_" @ %bl_id;
	if (!isObject(%bg))
	{
		talk("ERROR: unloadLot - no brickgroup with BLID " @ %bl_id @ " exists!");
		error("ERROR: unloadLot - no brickgroup with BLID " @ %bl_id @ " exists!");
		return;
	}

	if (farmingSaveLot(%bl_id, true) == -1)
	{
		return -1;
	}
}

function loadLot(%bl_id, %lot, %rotation)
{
	if (!isObject(%lot) || !%lot.getDatablock().isLot || !%lot.getDatablock().isSingle)
	{
		talk("ERROR: loadLot - invalid lot provided! " @ %lot);
		error("ERROR: loadLot - invalid lot provided! " @ %lot);
		return -1;
	}

	if (%lot.getGroup().bl_id != 888888)
	{
		error("ERROR: loadLot - lot is not public! " @ %lot);
		return -1;
	}

	if ($LotLoadingFlag)
	{
		error("ERROR: loadLot - lot currently loading!");
		return -2;
	}

	%adj = getField(getNumAdjacentLots(%lot, "all"), 1);
	%count = 0;
	%dataObj = new ScriptObject(LotLoadingDataObj);
	%rot = "1 0 0 0";
	%dataObj.pos[%count] = %lot.getPosition();
	%dataObj.obj[%count] = %lot;
	%count++;
	for (%i = 0; %i < getWordCount(%adj); %i++)
	{
		%adjLot = getWord(%adj, %i);
		if (%adjLot.getGroup().bl_id != 888888)
		{
			error("ERROR: loadLot - lot is not public! " @ %lot);
			return -1;
		}
		%dataObj.pos[%count] = %adjLot.getPosition();
		%dataObj.obj[%count] = %adjLot;
		%count++;
	}

	echo("Lots Found: " @ %count);

	for (%i = 0; %i < %count; %i++)
	{
		%dataObj.obj[%i].delete();
	}
	%dataObj.count = %count;

	loadLastLotAutosave(%bl_id, %dataObj, %rotation);
}

package lotMovingPackage
{
	function resetLotLoading()
	{
		%dataObj = $LotLoadDataObject;
		$LotLoadDataObject = "";
		schedule(1, MissionCleanup, restoreLotBricks, %dataObj);
		return parent::resetLotLoading();
	}

	function Autosaver_Save(%file)
	{
		%currBrickgroup = $CurrentLotSaving;
		%ret = parent::Autosaver_Save(%file);
		if (%currBrickgroup.isSaveClearingLot)
		{
			clearLots(%currBrickgroup);
			$CurrentLotSaving = "";
		}
		return %ret;
	}

	function serverCmdPlantBrick(%cl)
	{
		%bg = %cl.brickGroup;
		if (%bg.isSaveClearingLot)
		{
			messageClient(%cl, '', "You cannot place bricks while your lot is being unloaded!");
			return;
		}

		return parent::serverCmdPlantBrick(%cl);
	}
};
schedule(5000, 0, activatePackage, lotMovingPackage);

function clearLots(%bg)
{
	%count = %count + 0;
	%bg.isSaveClearingLot = 1;
	while (%bg.getCount() > 0 && %count < 1024)
	{
		%b = %bg.getObject(0);
		if (%b.getDatablock().isLot)
		{
			Brickgroup_888888.add(%b);
			fixLotColor(%b);

			if (!%b.getDatablock().isSingle)
			{
				%b.setDatablock("brick32x32LotRaisedData");
			}

			schedule(100, %b, clearLotRecursive, %b);
		}
		else
		{
			%b.delete(); //deleted objects arent sold
		}
	}

	if (%bg.getCount() == 0)
	{
		%bg.refreshLotList();
		%bg.isSaveClearingLot = 0;
		return;
	}
	else
	{
		schedule(1, %bg, clearLots, %bg);
	}
}

function restoreLotBricks(%dataObj)
{
	if (!isObject(%dataObj))
	{
		return;
	}
	
	for (%i = 0; %i < %dataObj.count; %i++)
	{
		if (%i == 0) %db = "brick32x32SingleLotData";
		else %db = "brick32x32LotRaisedData";

		%b = new fxDTSBrick() {
			dataBlock = %db;
			position = %dataObj.pos[%i];
			rotation = "1 0 0 0";
			isPlanted = 1;
			colorID = 0;
		};
		%err = %b.plant();
		if (%err != 2 && %err != 0)
		{
			%b.delete();
			continue;
		}

		if (%err == 2)
		{
			%b.isBaseplate = 1;
			%b.willCauseChainKill(); // recompute - thanks new duplicator
		}

		%b.setTrusted(1);
		Brickgroup_888888.add(%b);
	}
	%dataObj.delete();
}

function obtainAllOwnership(%lotBrick)
{
	%lotGroup = %lotBrick.getGroup();

	%top = vectorAdd(%lotBrick.getPosition(), "0 0 0.1");
	%pos = vectorAdd(%top, "0 0 " @ $maxLotBuildHeight / 2);
	%box = %lotBrick.getDatablock().brickSizeX * 0.5 - 0.05;
	%box = %box SPC %box SPC ($maxLotBuildHeight - 0.05);


	%lotBounds = getBrickBounds(%lotBrick, $maxLotBuildHeight);

	initContainerBoxSearch(%pos, %box, $TypeMasks::fxBrickAlwaysObjectType);
	for (%i = 0; %i < 1024 * 20; %i++)
	{
		if (!isObject(%next = containerSearchNext()))
		{
			//we're done
			break;
		}


		if (%next.getGroup() != %lotGroup
			&& isContainedInBounds(%next.getPosition() TAB %next.getPosition(), %lotBounds))
		{
			%lotGroup.add(%next);
		}
	}
}