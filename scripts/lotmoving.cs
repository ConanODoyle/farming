function unloadLot(%bl_id)
{
	%bg = "Brickgroup_" @ %bl_id;
	if (!isObject(%bg))
	{
		talk("ERROR: unloadLot - no brickgroup with BLID " @ %bl_id @ " exists!");
		error("ERROR: unloadLot - no brickgroup with BLID " @ %bl_id @ " exists!");
		return;
	}
	saveLotBLID(%bl_id);
	%bg.isSaveClearingLot = 1;
}

package lotMovingPackage
{
	function resetLotLoading()
	{
		%currBrickgroup = $CurrentLotLoading;
		parent::resetLotLoading();
		if (%currBrickgroup.isSaveClearingLot)
		{
			clearLots(%currBrickgroup);
		}
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