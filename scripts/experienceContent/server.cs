function Player::sacrificeKill(%pl, %modifier, %announcement)
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

		%exp = mCeil(%totalCost * %modifier);
		%cl.addExperience(%exp);
		if (%exp > 0)
			messageAll('', %prefix @ " \c3" @ %cl.name @ "\c6 has sacrificed themself for \c3" @ %exp @ " \c6experience...");
		else
			if (strLen(%announcement = trim(%announcement)) > 0)
				messageAll('', %prefix @ "\c3" @ %cl.name @ "\c6" @ %announcement);
			else
				messageAll('', %prefix @ " \c3" @ %cl.name @ "\c6 has sacrificed themself to the volcano god...");
	}
	%pl.kill();
}

registerOutputEvent("Player", "sacrificeKill", "float 0 2 0.1 0.5", "string 200 100");

function fxDTSBrick::checkExperience(%this, %check, %action, %amount, %cl)
{
	%exp = %cl.farmingExperience;
	if (%exp >= %check)
	{
		%this.onExperienceCheckPass(%cl);
	}
	else
	{
		%this.onExperienceCheckFail(%cl);
	}

	switch (%action)
	{
		case 1: %cl.addExperience(-1 * %amount);
		case 2: %cl.addExperience(%amount);
	}
}

registerOutputEvent("fxDTSBrick", "checkExperience", "int 0 100000 100" TAB "list None 0 SubtractEXP 1 AddEXP 2" TAB "int 0 100000", 1);

function fxDTSBrick::onExperienceCheckPass(%this, %cl)
{
	%pl = %cl.player;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onExperienceCheckPass", %client);
}

function fxDTSBrick::onExperienceCheckFail(%this, %cl)
{
	%pl = %cl.player;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onExperienceCheckFail", %client);	
}

registerInputEvent("fxDTSBrick", "onExperienceCheckPass", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onExperienceCheckFail", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");