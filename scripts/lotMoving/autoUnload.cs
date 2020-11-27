$minFreeLots = 3;

package LotMoving
{
	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);
		if (!%cl.hasSpawnedOnce)
		{
			checkFreeLots();
		}
		%cl.hasSpawnedOnce = 1;
		$Pref::LotMoving::LastOn[%cl.bl_id] = getRealTime();
		return %ret;
	}

	function GameConnection::onDrop(%cl)
	{
		if (%cl.hasSpawnedOnce)
		{
			$Pref::LotMoving::LastOn[%cl.bl_id] = getRealTime();
			checkFreeLots();
		}
		return parent::onDrop(%cl);
	}

	function fxDTSBrick::onAdd(%obj)
	{
		if (%obj.isPlanted && %obj.getDatablock().isLot && %obj.getDatablock().isSingle)
		{
			$SingleLotSimSet.add(%obj);
		}
		return parent::onAdd(%obj);
	}
};
activatePackage(LotMoving);

if (!isObject($SingleLotSimSet))
{
	$SingleLotSimSet = new SimSet(SingleLots);
}

function getFreeLotCount()
{
	%freeLots = 0;
	%count = $SingleLotSimSet.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%b = $SingleLotSimSet.getObject(%i);
		%bDB = %b.getDatablock();
		if (%b.getGroup().bl_id == 888888 && %bDB.isSingle)
		{
			%freeLots++;
		}
	}
	return %freeLots;
}

function checkFreeLots()
{
	%count = getFreeLotCount();
	if (%count < $minFreeLots)
	{
		%error = attemptUnloadOldestLot();
		if (%error)
		{
			echo("Could not free a lot!");
		}
		else
		{
			%lotFreed++;
			%count--;
		}
	}
	return %lotFreed;
}

function attemptUnloadOldestLot()
{
	%oldest = "";
	%oldestTime = "";
	%count = $SingleLotSimSet.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%brick = $SingleLotSimSet.getObject(%i);
		%group = %brick.getGroup();
		%blid = %group.bl_id;

		if (%blid == 888888) continue;

		if (%oldest $= "" || %oldestTime > $Pref::LotMoving::LastOn[%blid])
		{
			if (isObject(findClientByBL_ID(%blid)))
			{
				continue;
			}
			%oldestTime = $Pref::LotMoving::LastOn[%blid];
			%oldest = %blid;
		}
	}

	if (%oldest $= "" || $Server::AS["InUse"])
	{
		return 2;
	}
	else
	{
		unloadLot(%oldest);
		return 0;
	}
}