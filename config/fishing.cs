
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

FishingLootTable.addFishingLootDrop("Bucket", -5 * $fishWeight, 45 * $fishWeight, 0);
FishingLootTable.addFishingLootDrop("Old Boot", -5 * $fishWeight, 45 * $fishWeight, 0);
FishingLootTable.addFishingLootDrop("Glass Bottle", -5 * $fishWeight, 95 * $fishWeight, 0);
FishingLootTable.addFishingLootDrop("Wrench", 0, 20, 0);
FishingLootTable.addFishingLootDrop("Hammer", 0, 20, 0);
FishingLootTable.addFishingLootDrop("Printer", 0, 20, 0);
FishingLootTable.addFishingLootDrop("Pickaxe", 0, 20, 0);
FishingLootTable.addFishingLootDrop("Potato Seed", 0, 50, 0.5);
FishingLootTable.addFishingLootDrop("Carrot Seed", 0, 50, 0.5);
FishingLootTable.addFishingLootDrop("Onion Seed", 0, 50, 0.5);

FishingLootTable.addFishingLootDrop("Sardine", 100 * $fishWeight, 100 * $fishWeight, 1);
FishingLootTable.addFishingLootDrop("Anchovy", 100 * $fishWeight, 60 * $fishWeight, 1);
FishingLootTable.addFishingLootDrop("Corn Seed", 0, 30, 1);
FishingLootTable.addFishingLootDrop("Wheat Seed", 0, 30, 1);
FishingLootTable.addFishingLootDrop("Cabbage Seed", 0, 30, 1);

FishingLootTable.addFishingLootDrop("Minnow", 60 * $fishWeight, 30 * $fishWeight, 2);
FishingLootTable.addFishingLootDrop("Catfish", 40 * $fishWeight, 20 * $fishWeight, 2);
FishingLootTable.addFishingLootDrop("Turnip Seed", 0, 30, 2.2);
FishingLootTable.addFishingLootDrop("Portobello Seed", 0, 30, 2.2);
FishingLootTable.addFishingLootDrop("Blueberry Seed", 0, 30, 2.2);

FishingLootTable.addFishingLootDrop("Bass Fish", 30 * $fishWeight, -18 * $fishWeight, 2.85);
FishingLootTable.addFishingLootDrop("Arowana", 30 * $fishWeight, -6 * $fishWeight, 2.85);

FishingLootTable.addFishingLootDrop("Rose Seed", 0, 30, 3);
FishingLootTable.addFishingLootDrop("Lily Seed", 0, 30, 3);
FishingLootTable.addFishingLootDrop("Daisy Seed", 0, 30, 3);

FishingLootTable.addFishingLootDrop("Clipper", 4, -10, 3.2);
FishingLootTable.addFishingLootDrop("Trowel", 4, -10, 3.2);
FishingLootTable.addFishingLootDrop("Hoe", 2, -10, 3.2);
FishingLootTable.addFishingLootDrop("Sickle", 2, -10, 3.2);
FishingLootTable.addFishingLootDrop("CropTrak", 6, -50, 3.2);
