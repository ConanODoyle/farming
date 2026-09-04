function serverCmdResetAllProgress(%cl)
{
	if (hasLoadedLot(%cl.bl_id))
	{
		%cl.isResettingProgress = 1;
		unloadLot(%cl.bl_id);
		messageClient(%cl, '', "Please wait for your lot to unload");
		return;
	}
	%blid = %cl.bl_id;
	%cl.delete("Rejoin to finalize reset");

	schedule(100, 0, clearClientData, %blid);
}

function clearClientData(%blid)
{
	$Pref::Farming::BossReward[%blid] = "";
	$Pref::Farming::LastLotAutosave[%blid] = "";
	$Pref::Farming::License[%blid] = "";
	$Pref::Farming::ScoreGrant[%blid] = "";
	
	fileDelete("config/server/persistence/" @ %blid @ ".txt");
}
