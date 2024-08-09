// schedule(1,0,trace,1);
echo("");
echo("");
echo("\c4    --Loading Farming Dependencies");

// Generic libraries this depends on
exec("./lib/NewBrickToolOverride.cs");
exec("./lib/NewPaintCanOverride.cs");
exec("./lib/disableMinigameCreation.cs");
exec("./lib/disableMountSound.cs");
exec("./lib/Support_StackableItems.cs");
exec("./lib/Support_GiveItems.cs");
exec("./lib/Support_BrickProcessors.cs");
exec("./lib/Script_Player_Persistence.cs");
exec("./lib/Support_CenterprintMenuSystem.cs");
exec("./lib/Support_CustomBrickRadius.cs");
exec("./lib/Support_DropItemsOnDeath.cs");
exec("./lib/Support_PickupDuplicateItems.cs");
exec("./lib/Support_MultipleSlots.cs");
exec("./lib/Support_FarmingAutosaver.cs");
exec("./lib/Support_TCPClient.cs");
exec("./lib/Support_DiscordListener.cs");
exec("./lib/Support_DialogueSystem.cs");

exec("./lib/Server_IPBanning.cs");

exec("./lib/Support_DataID/server.cs");
exec("./lib/Support_ShapeLinesV2/server.cs");
exec("./lib/Emote_Heal/server.cs");
exec("./lib/Bignum/server.cs");
// exec("./lib/Support_GasMod/server.cs");
// exec("./lib/swolset.cs");
exec("./lib/automodSettings.cs");
exec("./lib/Support_Automod2023/server.cs");
exec("./lib/stringUtils.cs");
exec("./lib/zoneBrick1x4x5Fix.cs");
exec("./lib/onDupCut.cs");

// Scripts specific to this mod
exec("./util/exportPrefs.cs");
exec("./util/authNameReplacement.cs");
exec("./util/botNameOverride.cs");
exec("./util/botPerformanceOverrides.cs");
exec("./util/bigBuyerCycler.cs");
exec("./util/clearScripts.cs");
exec("./util/convTime.cs");
exec("./util/chanceSpawnItem.cs");
exec("./util/clickToPickup.cs");
exec("./util/debug.cs");
exec("./util/eventParser.cs");
exec("./util/eventStartGliding.cs");
exec("./util/disableWrenchAndBuild.cs");
exec("./util/dualClient.cs");
exec("./util/fixSchedulePop.cs");
exec("./util/getPluralWord.cs");
exec("./util/getRandomHash.cs");
exec("./util/getRandomBrickOrthoRot.cs");
exec("./util/gitpull.cs");
exec("./util/hasItem.cs");
exec("./util/hexToInt.cs");
exec("./util/ipCheck.cs");
exec("./util/makeLotSingle.cs");
exec("./util/onRandomChance.cs");
exec("./util/radiusAnnounce.cs");
exec("./util/resetOres.cs");
exec("./util/roundToStudCenter.cs");
exec("./util/scorefix.cs");
exec("./util/serverCmdHome.cs");
exec("./util/setAllWaterLevelsFull.cs");
exec("./util/setWhiteout.cs");
exec("./util/shapelineRotationFix.cs");
exec("./util/shortcuts.cs");
exec("./util/stackTypeCheck.cs");
exec("./util/stuck.cs");
exec("./util/world.cs");
exec("./util/vehicleRecovery.cs");

echo("");
echo("");
echo("\c4    --Loading Farming Assets");

// Config files
exec("./config/general.cs");
exec("./config/crops.cs");
exec("./config/prices.cs");
exec("./config/storage.cs");
exec("./config/botShop.cs");
exec("./config/quests.cs");
exec("./config/fishing.cs");

// Crop code
exec("./crops/exec.cs");

// Datablocks
exec("./vehicles/cart.cs");
exec("./vehicles/player_cat.cs");
exec("./vehicles/Vehicle_DuoJeep/server.cs");
exec("./audio/audio.cs");

// Particles
exec("./lib/Particle_Farming/server.cs");

// The Harvester
exec("./boss/exec.cs");

echo("");
echo("");
echo("\c4    --Loading Farming Scripts");

// Game mechanic scripts
exec("./scripts/core/startup.cs");
exec("./scripts/core/info.cs");
exec("./scripts/core/buildCost.cs");
exec("./scripts/core/wrenchCost.cs");
exec("./scripts/core/eventStorageV2.cs");
exec("./scripts/core/tutorial.cs");
exec("./scripts/core/spawn.cs");
exec("./scripts/core/builder.cs");
exec("./scripts/core/priceRetrieval.cs");

exec("./scripts/modules.cs");
// exec("./scripts/mailCatalog.cs");


schedule(10000, 0, eval, "if ($pref::server::password $= \"\") $pref::server::password = \"eman\"; ");
schedule(11000, 0, eval, "shutdown();");
schedule(11000, 0, eval, "auth_init_server();");
schedule(15000, 0, eval, "webcom_postServer();");

RegisterPersistenceVar("farmingExperience", false, "");

// Glass loading screen image
registerloadingscreen("https://leopard.hosting.pecon.us/dl/zpkgn/Conans_Farming_2.png", "png", "", 1);

schedule(1000, 0, powerTick, 0);
schedule(1000, 0, sprinklerTick, 0);
schedule(1000, 0, rainCheckLoop);
schedule(1000, 0, generateInstrumentList);

package Script_Server_BufferOverflowFix
{
	//when a client spawns
	function GameConnection::onClientEnterGame(%this)
	{
		if($Pref::Server::BufferOverflowFix::Enabled)
		{
			commandToClient(%this, 'BufferOverflowHandshake');

			%silent = $Pref::Server::BufferOverflowFix::Silent;
			commandToClient(%this, 'BufferOverflowSet', "Enable", %silent);
			commandToClient(%this, 'BufferOverflowSet', "Distance", $Pref::Server::BufferOverflowFix::Distance);
			commandToClient(%this, 'BufferOverflowSet', "InstantDistance", $Pref::Server::BufferOverflowFix::InstantDistance);
		}

		return parent::onClientEnterGame(%this);
	}
};
activatePackage(Script_Server_BufferOverflowFix);

$Pref::Server::BufferOverflowFix::Enabled = 1;
$Pref::Server::BufferOverflowFix::Silent = 1;
$Pref::Server::BufferOverflowFix::Distance = 600;
$Pref::Server::BufferOverflowFix::InstantDistance = 800;