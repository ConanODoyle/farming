$minFreeLots = 5;

package LotMoving
{
	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);
		if (!%cl.hasSpawnedOnce)
		{
			checkFreeLots();
			unloadEmptyLots();
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
			unloadEmptyLots();
		}
		return parent::onDrop(%cl);
	}

	function fxDTSBrick::onAdd(%obj)
	{
		if (%obj.isPlanted)
		{
			if (%obj.getDatablock().isLot && %obj.getDatablock().isSingle)
			{
				$SingleLotSimSet.add(%obj);
			}
			else if (%obj.getDatablock().isShopLot)
			{
				$ShopLotSimSet.add(%obj);
			}
		}
		return parent::onAdd(%obj);
	}
};
activatePackage(LotMoving);

if (!isObject($SingleLotSimSet))
{
	$SingleLotSimSet = new SimSet(SingleLots);
}

if (!isObject($ShopLotSimSet))
{
	$ShopLotSimSet = new SimSet(ShopLots);
}

function getFreeShopLotCount()
{
	return getNumPublicLots($ShopLotSimSet);
}

function getFreeLotCount()
{
	return getNumPublicLots($SingleLotSimSet);
}

function getNumPublicLots(%set)
{
	%freeLots = 0;
	%count = %set.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%b = %set.getObject(%i);
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

	schedule(5000, MissionCleanup, checkFreeShopLots);
	return %lotFreed;
}

function checkFreeShopLots()
{
	%count = getFreeShopLotCount();
	if (%count < $minFreeLots)
	{
		%error = attemptUnloadOldestShoplot();
		if (%error)
		{
			echo("Could not free a shoplot!");
		}
		else
		{
			%lotFreed++;
			%count--;
		}
	}
}

function attemptUnloadOldestLot()
{
	%oldestBLID = getOldestLotBLID($SingleLotSimSet);

	if (%oldestBLID $= "" || $Server::AS["InUse"])
	{
		return 2;
	}
	else
	{
		unloadLot(%oldestBLID);
		return 0;
	}
}

function attemptUnloadOldestShoplot()
{
	%oldestBLID = getOldestLotBLID($ShopLotSimSet);

	if (%oldestBLID $= "" || $Server::AS["InUse"])
	{
		return 2;
	}
	else
	{
		unloadShop(%oldestBLID);
		return 0;
	}
}

function getOldestLotBLID(%set)
{
	%oldestBLID = "";
	%oldestBLIDTime = "";
	%count = %set.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%brick = %set.getObject(%i);
		%group = %brick.getGroup();
		%blid = %group.bl_id;

		if (%blid == 888888) continue;

		if (%oldestBLID $= "" || %oldestBLIDTime > $Pref::LotMoving::LastOn[%blid])
		{
			if (isObject(findClientByBL_ID(%blid)))
			{
				continue;
			}
			%oldestBLIDTime = $Pref::LotMoving::LastOn[%blid];
			%oldestBLID = %blid;
		}
	}
	return %oldestBLID;
}

function unloadEmptyLots() 
{
	%numBrickGroups = mainBrickGroup.getCount();
	for (%i = 0; %i < %numBrickGroups; %i++)
	{
		%brickGroup = mainBrickGroup.getObject(%i);
		if (isObject(BrickGroup_888888) && %brickGroup == BrickGroup_888888.getID()) continue;
		if (isObject(%brickGroup.client)) continue;

		%lotsAreEmpty = true;
		%lotCount = getWordCount(%brickGroup.lotList);
		for (%j = 0; %j < %lotCount; %j++)
		{
			%lot = getWord(%brickGroup.lotList, %j);
			if (%lot.getNumUpBricks() > 0 || %lot.getNumDownBricks() > 0)
			{
				%lotsAreEmpty = false;
				break;
			}

			if (!%lotsAreEmpty) break;
		}

		if (%lotsAreEmpty && %lotCount > 0)
		{
			unloadLot(%brickGroup.bl_id);
		}
	}
}