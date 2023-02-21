function Player::grantBRs(%pl)
{
	%cl = %pl.client;
	if (!isObject(%cl))
	{
		return;
	}
	%blid = %cl.bl_id;

	%reward0 = "L0RemorseItem";
	%reward1 = "L3LastWordItem";
	%reward2 = "L4SilenceItem";

	%pick = getRandom(0, 2);
	while (getWord($Pref::Farming::BossReward[%blid], %pick) == 1 && %safety++ < 5)
	{
		%pick = (%pick + 1) % 3;
	}

	//all rewards already obtained
	if (%safety >= 5)
	{
		return;
	}

	//give picked reward
	%pl.farmingAddItem(%reward[%pick].getID());
	$Pref::Farming::BossReward[%blid] = setWord($Pref::Farming::BossReward[%blid], %pick, 1);
	exportServerPrefs();
}
registerOutputEvent("Player", "grantBRs");