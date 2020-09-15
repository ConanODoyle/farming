// schedule(1,0,trace,1);
echo("");
echo("");
echo("\c4    --Loading Farming Dependencies");

// Generic libraries this depends on
exec("./lib/NewBrickToolOverride.cs");
exec("./lib/NewPaintCanOverride.cs");
exec("./lib/disableMinigameCreation.cs");
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

exec("./lib/Support_DataID/server.cs");
exec("./lib/Support_ShapeLines/server.cs");
// exec("./lib/Support_GasMod/server.cs");
// exec("./lib/swolset.cs");
exec("./lib/automodSettings.cs");
exec("./lib/onDupCut.cs");

// Scripts specific to this mod
exec("./util/eventParser.cs");
exec("./util/disableWrenchAndBuild.cs");
exec("./util/clearScripts.cs");
exec("./util/convTime.cs");
exec("./util/chanceSpawnItem.cs");
exec("./util/dualClient.cs");
exec("./util/fixSchedulePop.cs");
exec("./util/getRandomHash.cs");
exec("./util/gitpull.cs");
exec("./util/hexToInt.cs");
exec("./util/ipCheck.cs");
exec("./util/makeLotSingle.cs");
exec("./util/radiusAnnounce.cs");
exec("./util/scorefix.cs");
exec("./util/setAllWaterLevelsFull.cs");
exec("./util/shortcuts.cs");
exec("./util/stackTypeCheck.cs");
exec("./util/strLastPos.cs");
exec("./util/stuck.cs");
exec("./util/world.cs");

echo("");
echo("");
echo("\c4    --Loading Farming Assets");

// Generic libraries this depends on
exec("./config/general.cs");
exec("./config/crops.cs");
exec("./config/prices.cs");
exec("./config/storage.cs");

// Crop code
exec("./crops/exec.cs");

// Datablocks
exec("./vehicles/cart.cs");
exec("./audio/audio.cs");

// Particles
exec("./growParticles.cs");
exec("./harvestParticles.cs");


echo("");
echo("");
echo("\c4    --Loading Farming Scripts");

// Game mechanic scripts
exec("./scripts/core/startup.cs");
exec("./scripts/core/info.cs");
exec("./scripts/core/buildCost.cs");
exec("./scripts/core/wrenchCost.cs");
exec("./scripts/core/botBuy.cs");
exec("./scripts/core/eventStorageV2.cs");
exec("./scripts/core/tutorial.cs");
exec("./scripts/core/spawn.cs");
exec("./scripts/core/builder.cs");

exec("./scripts/modules.cs");
// exec("./scripts/mailCatalog.cs");

// Debug code
exec("./debug.cs");

schedule(10000, 0, "$pref::server::password = \"eman\"; ");
schedule(11000, 0, "shutdown();");
schedule(11000, 0, "loadLastAutosave();");
schedule(11000, 0, "auth_init_server();");
schedule(15000, 0, "webcom_postServer();");

RegisterPersistenceVar("farmingExperience", false, "");

RegisterPersistenceVar("questItemInfo0", false, "");
RegisterPersistenceVar("questItemInfo1", false, "");
RegisterPersistenceVar("questItemInfo2", false, "");
RegisterPersistenceVar("questItemInfo3", false, "");
RegisterPersistenceVar("questItemInfo4", false, "");
RegisterPersistenceVar("questItemInfo5", false, "");
RegisterPersistenceVar("questItemInfo6", false, "");
RegisterPersistenceVar("questItemInfo7", false, "");
RegisterPersistenceVar("questItemInfo8", false, "");
RegisterPersistenceVar("questItemInfo9", false, "");
RegisterPersistenceVar("questItemInfo10", false, "");
RegisterPersistenceVar("questItemInfo12", false, "");
RegisterPersistenceVar("questItemInfo13", false, "");
RegisterPersistenceVar("questItemInfo14", false, "");
RegisterPersistenceVar("questItemInfo15", false, "");
RegisterPersistenceVar("questItemInfo16", false, "");
RegisterPersistenceVar("questItemInfo17", false, "");
RegisterPersistenceVar("questItemInfo18", false, "");
RegisterPersistenceVar("questItemInfo19", false, "");

// Glass loading screen image
registerloadingscreen("https://i.imgur.com/06fAw4h.png", "png", "", 1);

schedule(1000, 0, sprinklerTick, 0);
schedule(1000, 0, rainCheckLoop);
schedule(1000, 0, generateInstrumentList);
