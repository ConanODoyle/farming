////////
//Dirt//
////////

datablock fxDTSBrickData(brick4x4DirtData : brick4x4Data)
{
	category = "Farming";
	subCategory = "Dirt";
	uiName = "4x4 Dirt";

	cost = 20;
	isDirt = 1;
	maxWater = 150;
	maxNutrients = 30;
	maxWeedkiller = 20;
};

datablock fxDTSBrickData(brick8x8DirtData : brick8x8Data)
{
	category = "Farming";
	subCategory = "Dirt";
	uiName = "8x8 Dirt";

	cost = 120;
	isDirt = 1;
	maxWater = 1200;
	maxNutrients = 150;
	maxWeedkiller = 80;
};


///////////////
//Water Tanks//
///////////////

datablock fxDTSBrickData(brickSmallWaterTankData)
{
	category = "Farming";
	subCategory = "Water Tanks";
	uiName = "Small Water Tank";

	brickFile = "./bricks/smallWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/small_water_tank";

	cost = 200;
	isWaterTank = 1;
	maxWater = 10000;

	maxSprinklers = 4;
};

datablock fxDTSBrickData(brickSmallWaterInfiniteTankData)
{
	uiName = "Infinite Small Water Tank";

	brickFile = "./bricks/smallWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/small_water_tank";

	cost = -1;
	isInfiniteWaterSource = 1;
	isWaterTank = 1;
	maxWater = 10000;
	waterPerSecond = 50;

	maxSprinklers = 100;
};

datablock fxDTSBrickData(brickMediumWaterTankData)
{
	category = "Farming";
	subCategory = "Water Tanks";
	uiName = "Medium Water Tank";

	brickFile = "./bricks/medWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/medium_water_tank";

	cost = 800;
	isWaterTank = 1;
	maxWater = 50000;

	maxSprinklers = 4;
};

datablock fxDTSBrickData(brickLargeWaterTankData)
{
	category = "Farming";
	subCategory = "Water Tanks";
	uiName = "Large Water Tank";

	brickFile = "./bricks/largeWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/large_water_tank";

	cost = 2200;
	isWaterTank = 1;
	maxWater = 100000;
	maxSprinklers = 0;
	isOutflowTank = 1;
};

// datablock fxDTSBrickData(brickMassiveWaterTankData)
// {
// 	category = "Farming";
// 	subCategory = "Water Tanks";
// 	uiName = "Massive Water Tank";

// 	brickFile = "./bricks/massiveWaterTank.blb";

// 	iconName = "Add-Ons/Server_Farming/crops/icons/massive_water_tank";

// 	cost = 4000;
// 	isWaterTank = 1;
// 	maxWater = 200000;
// 	maxSprinklers = 0;
// 	maxDistance = 68;
// };


//////////////
//Greenhouse//
//////////////

datablock fxDTSBrickData(brickGreenhouseData)
{
	category = "Farming";
	subCategory = "Extra";
	uiName = "Greenhouse";

	brickFile = "./bricks/Greenhouse.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/greenhouse";

	isGreenhouse = 1;

	cost = 4000;
};


///////////////
//Shop Bricks//
///////////////

datablock fxDTSBrickData(brickShopStallData)
{
	category = "Farming";
	subCategory = "Extra";
	uiName = "Shop Stall";

	brickFile = "./bricks/shopBrick.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/shopstall";

	isShopBrick = 1;
	isStorageBrick = 1;

	cost = 200;
	itemPos0 = "1 0.7 0";
	itemPos1 = "1 -0.7 0";
	itemPos2 = "0 0.7 0.4";
	itemPos3 = "0 -0.7 0.4";
};


////////////////
//Money Bricks//
////////////////

datablock fxDTSBrickData(brickGoldIngotData)
{
	category = "Farming";
	subCategory = "Extra";
	uiName = "Gold Ingot";

	brickFile = "./bricks/goldingot.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/Gold_Ingot";

	isGreenhouse = 1;

	cost = 5000;
	customRefundCost = 5000;
};


///////////
//Storage//
///////////

datablock fxDTSBrickData(brickStorageCrateData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Storage Crate";
	description = "(4 slots)";

	brickFile = "./bricks/storageCrate.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/storage_crate";

	isStorageBrick = 1;
	storageSlotCount = 4;
	storageMultiplier = 1;
	itemStackCount = 2;

	cost = 100;
};

datablock fxDTSBrickData(brickSiloData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Silo";
	description = "(1 slot, 30x storage, crops only)";

	brickFile = "./bricks/Silo.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/silo";

	isStorageBrick = 1;
	storageSlotCount = 1;
	storageMultiplier = 30;
	storageType = "Crops";
	itemStackCount = 0;

	cost = 600;
};

datablock fxDTSBrickData(brickLargeToolboxData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Toolbox";
	description = "(8 slots, tools only)";

	brickFile = "./bricks/toolboxLargeFeatures.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/toolbox";

	isStorageBrick = 1;
	storageSlotCount = 8;
	storageMultiplier = 0;
	storageType = "Tools";
	itemStackCount = 1;

	cost = 150;
};

datablock fxDTSBrickData(brickLeftQuestCabinetClosed0Data)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Quest Cabinet (Left)";
	description = "(4 slots, quests only)";

	brickFile = "./bricks/leftQuestCabinetClosed0.blb";

	iconName = "";

	isStorageBrick = 1;
	storageSlotCount = 4;
	storageMultiplier = 0;
	storageType = "Quests";
	itemStackCount = 1;

	cost = 150;

	baseDatablockName = "LeftQuestCabinet";
	IsOpenable = true;
	HasFillLevels = true;
};

datablock fxDTSBrickData(brickLeftQuestCabinetOpen0Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetOpen0.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetClosed1Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetClosed1.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetOpen1Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetOpen1.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetClosed2Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetClosed2.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetOpen2Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetOpen2.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetClosed3Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetClosed3.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetOpen3Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetOpen3.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetClosed4Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetClosed4.blb";
};

datablock fxDTSBrickData(brickLeftQuestCabinetOpen4Data : brickLeftQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/leftQuestCabinetOpen4.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetClosed0Data)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Quest Cabinet (Right)";
	description = "(4 slots, quests only)";

	brickFile = "./bricks/rightQuestCabinetClosed0.blb";

	iconName = "";

	isStorageBrick = 1;
	storageSlotCount = 4;
	storageMultiplier = 0;
	storageType = "Quests";
	itemStackCount = 1;

	cost = 150;

	baseDatablockName = "RightQuestCabinet";
	isOpenable = true;
	hasFillLevels = true;
};

datablock fxDTSBrickData(brickRightQuestCabinetOpen0Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetOpen0.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetClosed1Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetClosed1.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetOpen1Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetOpen1.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetClosed2Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetClosed2.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetOpen2Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetOpen2.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetClosed3Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetClosed3.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetOpen3Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetOpen3.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetClosed4Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetClosed4.blb";
};

datablock fxDTSBrickData(brickRightQuestCabinetOpen4Data : brickRightQuestCabinetClosed0Data)
{
	category = "";
	subCategory = "";

	brickFile = "./bricks/rightQuestCabinetOpen4.blb";
};
