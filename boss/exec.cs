$Harvester::Root = filePath($Con::File);

if(forceRequiredAddOn("Support_Hitboxes") == $Error::AddOn_NotFound)
{
	error("ERROR:" SPC fileName($Harvester::Root) SPC "- required add-on Support_Hitboxes not found");
	return;
}
else if(forceRequiredAddOn("Support_Hitscan") == $Error::AddOn_NotFound)
{
	error("ERROR:" SPC fileName($Harvester::Root) SPC "- required add-on Support_Hitscan not found");
	return;
}
else
{
	exec($Harvester::Root @ "/plugins/unitVectorFromAngles.cs");
	exec($Harvester::Root @ "/plugins/checkInZone.cs");
	
	exec($Harvester::Root @ "/properties.cs");
	
	exec($Harvester::Root @ "/classes/environment.cs");
	
	exec($Harvester::Root @ "/classes/HarvesterArmor.cs");
	exec($Harvester::Root @ "/classes/AncientWarrior.cs");
	exec($Harvester::Root @ "/classes/HarvesterBlade.cs");
	exec($Harvester::Root @ "/classes/AncientBlade.cs");
	exec($Harvester::Root @ "/classes/HarvesterClusterBomb.cs");
	exec($Harvester::Root @ "/classes/HarvesterBeamRifle.cs");
	exec($Harvester::Root @ "/classes/HarvesterSpike.cs");
	exec($Harvester::Root @ "/classes/HarvesterSpiritSummon.cs");
	
	exec($Harvester::Root @ "/classes/MasterKey.cs");
	exec($Harvester::Root @ "/classes/StasisMachine.cs");
	exec($Harvester::Root @ "/classes/VoidKey.cs");
	
	exec($Harvester::Root @ "/classes/L0Remorse.cs");
	exec($Harvester::Root @ "/classes/L3LastWord.cs");
	exec($Harvester::Root @ "/classes/L4Silence.cs");

	exec($Harvester::Root @ "/scripts/ai.cs");
	exec($Harvester::Root @ "/scripts/cutscene.cs");
	exec($Harvester::Root @ "/scripts/fight.cs");
	exec($Harvester::Root @ "/scripts/encounter.cs");
	exec($Harvester::Root @ "/scripts/reward.cs");

	exec($Harvester::Root @ "/npc/dialogue.cs");
}