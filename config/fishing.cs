
function LootTableObject::addFishingLootDrop(%table, %uiName, %aWeight, %bWeight, %tier)
{
	%count = %table.count + 0;
	%table.option[%count] = %uiName;
	%table.aWeight[%count] = %aWeight;
	%table.bWeight[%count] = %bWeight;
	%table.tier[%count] = %tier;
	%table.count++;
}

if (isObject(FishingLootTable))
{
	FishingLootTable.delete();
}
new ScriptObject(FishingLootTable) { class = "LootTableObject"; };

$fishWeight = 2;

// FishingLootTable.addFishingLootDrop("Old Boot", 0, 50, 0);
// FishingLootTable.addFishingLootDrop("Empty Can", 0, 50, 0);
FishingLootTable.addFishingLootDrop("Wrench", 0, 50, 0);
FishingLootTable.addFishingLootDrop("Hammer", 0, 50, 0);
FishingLootTable.addFishingLootDrop("PrintGun", 0, 50, 0);
FishingLootTable.addFishingLootDrop("PickaxeItem", 0, 50, 0);
FishingLootTable.addFishingLootDrop("PotatoSeedItem", 0, 50, 0);
FishingLootTable.addFishingLootDrop("CarrotSeedItem", 0, 50, 0);
FishingLootTable.addFishingLootDrop("OnionSeedItem", 0, 50, 0);

FishingLootTable.addFishingLootDrop("Sardine Fish", 100 * $fishWeight, 100 * $fishWeight, 1);
FishingLootTable.addFishingLootDrop("Anchovy Fish", 100 * $fishWeight, 60 * $fishWeight, 1);
FishingLootTable.addFishingLootDrop("CornSeedItem", 0, 50, 1);
FishingLootTable.addFishingLootDrop("WheatSeedItem", 0, 50, 1);
FishingLootTable.addFishingLootDrop("CabbageSeedItem", 0, 50, 1);

FishingLootTable.addFishingLootDrop("Minnow Fish", 60 * $fishWeight, 20 * $fishWeight, 2);
FishingLootTable.addFishingLootDrop("Catfish Fish", 40 * $fishWeight, 10 * $fishWeight, 2);
FishingLootTable.addFishingLootDrop("TurnipSeedItem", 0, 50, 2);
FishingLootTable.addFishingLootDrop("PortobelloSeedItem", 0, 50, 2);
FishingLootTable.addFishingLootDrop("BlueberrySeedItem", 0, 50, 2);

FishingLootTable.addFishingLootDrop("Bass Fish", 20 * $fishWeight, -20 * $fishWeight, 3);
FishingLootTable.addFishingLootDrop("Arowana Fish", 20 * $fishWeight, -20 * $fishWeight, 3);
FishingLootTable.addFishingLootDrop("RoseSeedItem", 0, 50, 3);
FishingLootTable.addFishingLootDrop("LilySeedItem", 0, 50, 3);
FishingLootTable.addFishingLootDrop("DaisySeedItem", 0, 50, 3);
FishingLootTable.addFishingLootDrop("Clipper", 5, -20, 3);
FishingLootTable.addFishingLootDrop("Clipper", 5, -20, 3);
FishingLootTable.addFishingLootDrop("Trowel", 5, -20, 3);
FishingLootTable.addFishingLootDrop("Hoe", 5, -20, 3);
FishingLootTable.addFishingLootDrop("Sickle", 5, -20, 3);
// FishingLootTable.addFishingLootDrop("CropTrak", 2, -20, 3);
