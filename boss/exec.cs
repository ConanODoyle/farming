$Harvester::Root = filePath($Con::File);

exec("./npc/dialogue.cs");

exec("./classes/environment.cs");
exec("./classes/HarvesterArmor.cs");
exec("./classes/HarvesterBlade.cs");
exec("./classes/HarvesterClusterBomb.cs");
exec("./classes/HarvesterBeamRifle.cs");
exec("./classes/HarvesterSpike.cs");
exec("./classes/MasterKey.cs");
exec("./classes/VoidKey.cs");

exec("./scripts/ai.cs");
exec("./scripts/cutscene.cs");
exec("./scripts/fight.cs");