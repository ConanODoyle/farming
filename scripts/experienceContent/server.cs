function Player::sacrificeKill(%pl, %modifier, %max, %announcement)
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
				%totalCost += getSellPrice(%type, %pl.toolStackCount[%i]);
				%pl.tool[%i] = "";
			}
			else if (getSellPrice(%pl.tool[%i]) > 0)
			{
				%totalCost += getSellPrice(%pl.tool[%i]) / 2;
				%pl.tool[%i] = "";
			}
			%pl.tool[%i] = "";
		}

		%exp = getMin(%max, mCeil(%totalCost * %modifier));
		%cl.addExperience(%exp);
		if (%exp > 0)
			messageAll('', %prefix @ " \c3" @ %cl.name @ "\c6 has sacrificed themselves for \c3" @ %exp @ " \c6experience...");
		else
			if (strLen(%announcement = trim(%announcement)) > 0)
				messageAll('', %prefix @ "\c3" @ %cl.name @ "\c6 " @ %announcement);
			else
				messageAll('', %prefix @ " \c3" @ %cl.name @ "\c6 has sacrificed themselves to the gods...");
	}
	%pl.kill();
}

registerOutputEvent("Player", "sacrificeKill", "float 0 2 0.1 0.5" TAB "int 0 100000 1000" TAB "string 200 100");

function fxDTSBrick::checkExperience(%this, %check, %cl)
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
}

registerOutputEvent("fxDTSBrick", "checkExperience", "int 0 100000 100", 1);

function fxDTSBrick::addExperience(%this, %amt, %underflow, %cl)
{
	if (%underflow && %amt * -1 > %cl.farmingExperience)
	{
		%cl.addExperience(-1 * %cl.farmingExperience);
	}
	else
	{
		%cl.addExperience(%amt);
	}
}
registerOutputEvent("fxDTSBrick", "addExperience", "int -100000 100000 0" TAB "bool", 1);

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