
function fxDTSBrick::onRandomTrue(%this, %cl)
{
	%pl = %cl.player;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onRandomTrue", %cl);
}
registerInputEvent("fxDTSBrick", "onRandomTrue", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

function fxDTSBrick::onRandomFalse(%this, %cl)
{
	%pl = %cl.player;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onRandomFalse", %cl);
}
registerInputEvent("fxDTSBrick", "onRandomFalse", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");


function fxDTSBrick::doRandomChance(%this, %chance, %cl)
{
	if (getRandom() < %chance)
	{
		%this.onRandomTrue(%cl);
	}
	else
	{
		%this.onRandomFalse(%cl);
	}
}
registerOutputEvent("fxDTSBrick", "doRandomChance", "string 200 200", 1);