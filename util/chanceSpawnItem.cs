
function fxDTSBrick::onChanceSpawnItem(%this, %cl)
{
	if (%this.lastOnChanceSpawnItem + 1 > $Sim::Time)
	{
		return;
	}
	%pl = %cl.player;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.lastOnChanceSpawnItem = $Sim::Time;
	%this.processInputEvent("onChanceSpawnItem", %cl);
}
registerInputEvent("fxDTSBrick", "onChanceSpawnItem", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");


function fxDTSBrick::spawnItemChance(%this, %chance, %vec, %db, %cl)
{
	if (getRandom() < %chance)
	{
		%this.spawnItem(%vec, %db);
		%this.onChanceSpawnItem(%cl);
	}
}
registerOutputEvent("fxDTSBrick", "spawnItemChance", "string 200 50" TAB "vector 200" TAB "dataBlock ItemData", 1);