function Player::grantBRs(%this)
{
	%client = %this.client;

	if(!isObject(%client))
		return;

	%bl_id = %client.bl_id;
	%possible = "L0RemorseItem L2PenanceItem L3LastWordItem L4SilenceItem";

	while(getWordCount(%possible) > 0)
	{
		%index = getRandom(0, getWordCount(%possible) - 1);
		%check = getWord(%possible, %index);

		if(!containsWord($Pref::Farming::BossRewards[%bl_id], %check))
		{
			%reward = %check;
			break;
		}

		%possible = removeWord(%possible, %index);
	}

	if(isObject(%reward))
	{
		%this.farmingAddItem(nameToID(%reward));
		$Pref::Farming::BossRewards[%bl_id] = trim($Pref::Farming::BossRewards[%bl_id] SPC %reward);
		exportServerPrefs();
	}
	else
	{
		%this.farmingAddStackableItem(AncientFlowerSeed0Item, 1);
	}
}

registerOutputEvent("Player", grantBRs);