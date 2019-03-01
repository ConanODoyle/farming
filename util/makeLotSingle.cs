function serverCmdMakeLotSingle(%cl)
{
	if (!%cl.isAdmin || !isObject(%pl = %cl.player))
	{
		return;
	}
	%start = %pl.getHackPosition();
	%end = getWords(%start, 0, 1) SPC 0;

	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickAlwaysObjectType);

	while (isObject(%hit = getWord(%ray, 0)) && %safety++ < 100)
	{
		if (%hit.getDatablock().isLot)
		{
			%owner = getBrickgroupFromObject(%hit).name;
			%bl_id = getBrickgroupFromObject(%hit).bl_id;
			if (!%hit.getDatablock().isSingle)
			{
				%hit.setDatablock(brick32x32SingleLotData);
				messageClient(%cl, '', "\c6Made " @ %owner @ "\c6's lot single!");
				return;
			}
			else
			{
				messageClient(%cl, '', %owner @ "\c6's lot is already single!");
				return;	
			}
			break;
		}
		%ray = containerRaycast(vectorSub(getWords(%ray, 1, 3), "0 0 0.1"), %end, $Typemasks::fxBrickAlwaysObjectType, %hit);
	}
	messageClient(%cl, '', "No lot found!");
}