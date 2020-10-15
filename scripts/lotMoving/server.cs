package LotMoving
{
	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);

		%cl.hasSpawnedOnce = 1;
		$Pref::LotMoving::LastOn[%cl.bl_id] = getRealTime();
		return %ret;
	}

	function GameConnection::onDrop(%cl)
	{
		if (%cl.hasSpawnedOnce)
		{
			$Pref::LotMoving::LastOn[%cl.bl_id] = getRealTime();
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
	%count = $SingleLotSimSet.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%b = $SingleLotSimSet.getObject(%i);
		%bDB = getDatablock();
		if (%bDB.getGroup().bl_id == 888888)
		{
			%count++;
		}
	}
	return %count;
}

function checkFreeLots()
{
	%count = getFreeLotCount();
	while (%count < $minFreeLots)
	{
		%error = attemptUnloadOldestLot();
		if (%error)
		{
			echo("Could not free any more lots! Total freed: " @ (%lotFreed + 0));
			break;
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
	%count = $SingleLotSimSet.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		
	}
}