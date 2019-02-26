function Player::sacrificeKill(%pl)
{
	%prefix = "<bitmap:base/data/particles/exclamation><bitmap:base/client/ui/ci/star>";
	if (isObject(%cl = %pl.client))
	{
		for (%i = 0; %i < %pl.getDatablock().maxTools; %i++)
		{
			if (%pl.toolStackCount[%i] > 0)
			{
				%itemDB = %pl.tool[%i];
				%type = %pl.tool[%i].stackType;
				%totalCost += $Produce::BuyCost_[%type] * %pl.toolStackCount[%i];
				%pl.tool[%i] = "";
			}
			else if (%pl.tool[%i].cost > 0)
			{
				%totalCost += %pl.tool[%i].cost / 3 * 2;
				%pl.tool[%i] = "";
			}
		}

		%exp = mFloor(%totalCost / 2);
		%cl.addExperience(%exp);
		if (%exp > 0)
			messageAll('', %prefix @ " \c3" @ %cl.name @ "\c6 has sacrificed themself for \c3" @ %exp @ " \c6experience...");
		else
			messageAll('', %prefix @ " \c3" @ %cl.name @ "\c6 has sacrificed themself to the volcano god...");
	}
	%pl.kill();
}

registerOutputEvent("Player", "sacrificeKill");