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

