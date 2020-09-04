
function fxDTSBrick::onChanceSpawnItem(%this, %cl)
{
	%pl = %cl.player;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onChanceSpawnItem", %client);
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
registerOutputEvent("fxDTSBrick", "spawnItemChance", "float 0 1 0.05 0.1" TAB "vector 200" TAB "dataBlock ItemData", 1);