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
		$Pref::LotMoving::LastOn[%cl.bl_id] = getDateInMinutes(getDateTime());
		return %ret;
	}

	function GameConnection::onDrop(%cl)
	{
		if (%cl.hasSpawnedOnce)
		{
			checkFreeLots();
			unloadEmptyLots();
			$Pref::LotMoving::LastOn[%cl.bl_id] = getDateInMinutes(getDateTime());
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

function loopSaveLots(%i)
{
	cancel($loopSaveLotSchedule);

	if (!isObject(LastSavedLot))
	{
		%obj = new ScriptObject(LastSavedLot);
	}
	
	%loopTime = 10000;
	while (%safety++ < 10)
	{
		if (%i >= $SingleLotSimSet.getCount())
		{
			%i = 0;
		}

		%lot = $SingleLotSimSet.getObject(%i);
		%group = %lot.getGroup();
		%timeSince = LastSavedLot.autosaveTicksLeft_[%group.bl_id];
		if (%group.bl_id == 888888 || %timeSince > 0)
		{
			LastSavedLot.autosaveTicksLeft_[%group.bl_id]--;
			%i++;
			continue;
		}
		echo("Autosaving " @ %group.name @ "'s lot (BLID " @ %group.bl_id @ ")");
		if (isObject(%group.client))
		{
			messageClient(%group.client, 'MsgUploadEnd', "\c3[INFO] \c6Autosaving your lot...");
		}
		if ($loopSaveLotDebug)
		{
			messageClient(fcn(Conan), '', "Autosaving " @ %group.name @ "'s lot (BLID " @ %group.bl_id @ ")");
		}
		farmingSaveLot(%group.bl_id, 0);

		//change time between lot autosaves depending on if player is on server
		if (isObject(%group.client))
		{
			LastSavedLot.autosaveTicksLeft_[%group.bl_id] = 720;
		}
		else
		{
			LastSavedLot.autosaveTicksLeft_[%group.bl_id] = 2048;
		}

		%loopTime = 30000;
		break;
	}
	%i++;

	$loopSaveLotSchedule = schedule(30000, $SingleLotSimSet, loopSaveLots, %i);
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
		if (%b.getGroup().bl_id == 888888 && %bDB.isSingle || %bDB.isShopLot)
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
		talk("Auto unloading " @ ("Brickgroup_" @ %oldestBLID).name @ "'s lot");
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

		if ($Pref::LotMoving::LastOn[%blid] > getDateInMinutes(getDateTime())
			|| $Pref::LotMoving::LastOn[%blid] < 0)
		{
			$Pref::LotMoving::LastOn[%blid] = 0;
		}
		
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

//by Buddy
//rough estimate of date to minutes starting from 2024
function getDateInMinutes(%DT)
{
    %date = getWord(%DT, 0);
    %time = getWord(%DT, 1);

    %token = nextToken(%date , "month"        , "/"); //%month
    %token = nextToken(%token, "dayOfMonth" , "/"); //%day
    %token = nextToken(%token, "Year"        , "/");    //%year

    %token = nextToken(%time , "hour"        , ":"); //%hour
    %token = nextToken(%token, "minute"        , ":");    //%minute
    //ignore seconds

    %dayNumber = getDayOfYear(%month, %dayOfMonth);
    //this number cannot exceed ~525600
    %minutes  = %minute + %hour * 60 + %dayNumber * 60 * 24;
    %yearsInMinutes = (%Year - 24) * 525600;
    return ((%minutes | 0) + (%yearsInMinutes | 0)) | 0;
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