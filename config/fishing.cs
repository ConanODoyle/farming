
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
	FishingLootTableA.delete();
	FishingLootTableB.delete();
	FishingLootTableC.delete();
}
$FT = new ScriptObject(FishingLootTable) { class = "LootTableObject"; };
$FTA = new ScriptObject(FishingLootTableA) { class = "LootTableObject"; }; //fish focused
$FTB = new ScriptObject(FishingLootTableB) { class = "LootTableObject"; }; //seed/tool focused
$FTC = new ScriptObject(FishingLootTableC) { class = "LootTableObject"; }; //big fish focused

$fishWeight = 6;

FishingLootTable.addFishingLootDrop("Bucket", -5 * $fishWeight, 45 * $fishWeight, 0);
FishingLootTable.addFishingLootDrop("Old Boot", -5 * $fishWeight, 45 * $fishWeight, 0);
FishingLootTable.addFishingLootDrop("Glass Bottle", -5 * $fishWeight, 95 * $fishWeight, 0);
FishingLootTable.addFishingLootDrop("Wrench", 0, 20, 0);
FishingLootTable.addFishingLootDrop("Hammer ", 0, 20, 0);
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

FishingLootTable.addFishingLootDrop("Clipper", 3, -10, 3.2);
FishingLootTable.addFishingLootDrop("Trowel", 3, -10, 3.2);
FishingLootTable.addFishingLootDrop("Hoe", 1, -10, 3.2);
FishingLootTable.addFishingLootDrop("Sickle", 1, -10, 3.2);
FishingLootTable.addFishingLootDrop("CropTrak\x99 Upgrade Kit", 4, -50, 3.2);




FishingLootTableA.addFishingLootDrop("Bucket", 0 * $fishWeight, 35 * $fishWeight, 0);
FishingLootTableA.addFishingLootDrop("Old Boot", 0 * $fishWeight, 35 * $fishWeight, 0);
FishingLootTableA.addFishingLootDrop("Glass Bottle", 0 * $fishWeight, 75 * $fishWeight, 0);
// FishingLootTableA.addFishingLootDrop("Wrench", 0, 20, 0);
// FishingLootTableA.addFishingLootDrop("Hammer", 0, 20, 0);
// FishingLootTableA.addFishingLootDrop("Printer", 0, 20, 0);
// FishingLootTableA.addFishingLootDrop("Pickaxe", 0, 20, 0);
// FishingLootTableA.addFishingLootDrop("Potato Seed", 0, 50, 0.5);
// FishingLootTableA.addFishingLootDrop("Carrot Seed", 0, 50, 0.5);
// FishingLootTableA.addFishingLootDrop("Onion Seed", 0, 50, 0.5);

FishingLootTableA.addFishingLootDrop("Sardine", 170 * $fishWeight, 170 * $fishWeight, 1);
FishingLootTableA.addFishingLootDrop("Anchovy", 150 * $fishWeight, 80 * $fishWeight, 1);
// FishingLootTableA.addFishingLootDrop("Corn Seed", 0, 30, 1);
// FishingLootTableA.addFishingLootDrop("Wheat Seed", 0, 30, 1);
// FishingLootTableA.addFishingLootDrop("Cabbage Seed", 0, 30, 1);

FishingLootTableA.addFishingLootDrop("Minnow", 60 * $fishWeight, 30 * $fishWeight, 2);
FishingLootTableA.addFishingLootDrop("Catfish", 40 * $fishWeight, 20 * $fishWeight, 2);
// FishingLootTableA.addFishingLootDrop("Turnip Seed", 0, 30, 2.2);
// FishingLootTableA.addFishingLootDrop("Portobello Seed", 0, 30, 2.2);
// FishingLootTableA.addFishingLootDrop("Blueberry Seed", 0, 30, 2.2);

FishingLootTableA.addFishingLootDrop("Bass Fish", 20 * $fishWeight, -18 * $fishWeight, 2.85);
FishingLootTableA.addFishingLootDrop("Arowana", 20 * $fishWeight, -6 * $fishWeight, 2.85);

// FishingLootTableA.addFishingLootDrop("Rose Seed", 0, 30, 3);
// FishingLootTableA.addFishingLootDrop("Lily Seed", 0, 30, 3);
// FishingLootTableA.addFishingLootDrop("Daisy Seed", 0, 30, 3);

// FishingLootTableA.addFishingLootDrop("Clipper", 4, -10, 3.2);
// FishingLootTableA.addFishingLootDrop("Trowel", 4, -10, 3.2);
// FishingLootTableA.addFishingLootDrop("Hoe", 2, -10, 3.2);
// FishingLootTableA.addFishingLootDrop("Sickle", 2, -10, 3.2);
FishingLootTableA.addFishingLootDrop("CropTrak\x99 Upgrade Kit", 1, -50, 3.2);


$fishWeight = 4;

FishingLootTableB.addFishingLootDrop("Bucket", -5 * $fishWeight, 45 * $fishWeight, 0);
FishingLootTableB.addFishingLootDrop("Old Boot", -5 * $fishWeight, 45 * $fishWeight, 0);
FishingLootTableB.addFishingLootDrop("Glass Bottle", -5 * $fishWeight, 95 * $fishWeight, 0);
FishingLootTableB.addFishingLootDrop("Wrench", 0, 20, 0);
FishingLootTableB.addFishingLootDrop("Hammer ", 0, 20, 0);
FishingLootTableB.addFishingLootDrop("Printer", 0, 20, 0);
FishingLootTableB.addFishingLootDrop("Pickaxe", 0, 20, 0);
FishingLootTableB.addFishingLootDrop("Potato Seed", 0, 50, 0.5);
FishingLootTableB.addFishingLootDrop("Carrot Seed", 0, 50, 0.5);
FishingLootTableB.addFishingLootDrop("Onion Seed", 0, 50, 0.5);

FishingLootTableB.addFishingLootDrop("Sardine", 100 * $fishWeight, 100 * $fishWeight, 1);
FishingLootTableB.addFishingLootDrop("Anchovy", 100 * $fishWeight, 60 * $fishWeight, 1);
FishingLootTableB.addFishingLootDrop("Corn Seed", 0, 30, 1);
FishingLootTableB.addFishingLootDrop("Wheat Seed", 0, 30, 1);
FishingLootTableB.addFishingLootDrop("Cabbage Seed", 0, 30, 1);

FishingLootTableB.addFishingLootDrop("Minnow", 60 * $fishWeight, 30 * $fishWeight, 2);
FishingLootTableB.addFishingLootDrop("Catfish", 40 * $fishWeight, 20 * $fishWeight, 2);
FishingLootTableB.addFishingLootDrop("Turnip Seed", 0, 30, 2.2);
FishingLootTableB.addFishingLootDrop("Portobello Seed", 0, 30, 2.2);
FishingLootTableB.addFishingLootDrop("Blueberry Seed", 0, 30, 2.2);

FishingLootTableB.addFishingLootDrop("Bass Fish", 30 * $fishWeight, -18 * $fishWeight, 2.85);
FishingLootTableB.addFishingLootDrop("Arowana", 30 * $fishWeight, -6 * $fishWeight, 2.85);

FishingLootTableB.addFishingLootDrop("Rose Seed", 0, 30, 3);
FishingLootTableB.addFishingLootDrop("Lily Seed", 0, 30, 3);
FishingLootTableB.addFishingLootDrop("Daisy Seed", 0, 30, 3);

FishingLootTableB.addFishingLootDrop("Clipper", 3, -10, 3.2);
FishingLootTableB.addFishingLootDrop("Trowel", 3, -10, 3.2);
FishingLootTableB.addFishingLootDrop("Hoe", 1, -10, 3.2);
FishingLootTableB.addFishingLootDrop("Sickle", 1, -10, 3.2);
FishingLootTableB.addFishingLootDrop("CropTrak\x99 Upgrade Kit", 4, -50, 3.2);




FishingLootTableC.addFishingLootDrop("Bucket", -15 * $fishWeight, 35 * $fishWeight, 0);
FishingLootTableC.addFishingLootDrop("Old Boot", -15 * $fishWeight, 35 * $fishWeight, 0);
FishingLootTableC.addFishingLootDrop("Glass Bottle", -15 * $fishWeight, 75 * $fishWeight, 0);
// FishingLootTableC.addFishingLootDrop("Wrench", 0, 20, 0);
// FishingLootTableC.addFishingLootDrop("Hammer", 0, 20, 0);
// FishingLootTableC.addFishingLootDrop("Printer", 0, 20, 0);
// FishingLootTableC.addFishingLootDrop("Pickaxe", 0, 20, 0);
// FishingLootTableC.addFishingLootDrop("Potato Seed", 0, 50, 0.5);
// FishingLootTableC.addFishingLootDrop("Carrot Seed", 0, 50, 0.5);
// FishingLootTableC.addFishingLootDrop("Onion Seed", 0, 50, 0.5);

FishingLootTableC.addFishingLootDrop("Sardine", 120 * $fishWeight, 120 * $fishWeight, 1);
FishingLootTableC.addFishingLootDrop("Anchovy", 100 * $fishWeight, 60 * $fishWeight, 1);
// FishingLootTableC.addFishingLootDrop("Corn Seed", 0, 30, 1);
// FishingLootTableC.addFishingLootDrop("Wheat Seed", 0, 30, 1);
// FishingLootTableC.addFishingLootDrop("Cabbage Seed", 0, 30, 1);

FishingLootTableC.addFishingLootDrop("Minnow", 80 * $fishWeight, 30 * $fishWeight, 2);
FishingLootTableC.addFishingLootDrop("Catfish", 60 * $fishWeight, 20 * $fishWeight, 2);
// FishingLootTableC.addFishingLootDrop("Turnip Seed", 0, 30, 2.2);
// FishingLootTableC.addFishingLootDrop("Portobello Seed", 0, 30, 2.2);
// FishingLootTableC.addFishingLootDrop("Blueberry Seed", 0, 30, 2.2);

FishingLootTableC.addFishingLootDrop("Bass Fish", 40 * $fishWeight, 8 * $fishWeight, 2.85);
FishingLootTableC.addFishingLootDrop("Arowana", 40 * $fishWeight, 8 * $fishWeight, 2.85);

// FishingLootTableC.addFishingLootDrop("Rose Seed", 0, 30, 3);
// FishingLootTableC.addFishingLootDrop("Lily Seed", 0, 30, 3);
// FishingLootTableC.addFishingLootDrop("Daisy Seed", 0, 30, 3);

FishingLootTableC.addFishingLootDrop("Tuna", 8 * $fishWeight, -1 * $fishWeight, 3);
FishingLootTableC.addFishingLootDrop("Swordfish", 8 * $fishWeight, -1 * $fishWeight, 3);
// FishingLootTableC.addFishingLootDrop("Clipper", 4 * $fishWeight, 0, 3.2);
// FishingLootTableC.addFishingLootDrop("Trowel", 4, -10, 3.2);
// FishingLootTableC.addFishingLootDrop("Hoe", 2, -10, 3.2);
// FishingLootTableC.addFishingLootDrop("Sickle", 2, -10, 3.2);
FishingLootTableC.addFishingLootDrop("CropTrak\x99 Upgrade Kit", 2, -20, 3.2);
