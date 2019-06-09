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
};

datablock fxDTSBrickData(brick8x8DirtData : brick8x8Data) 
{
	category = "Farming";
	subCategory = "Dirt";
	uiName = "8x8 Dirt";

	cost = 120;
	isDirt = 1;
	maxWater = 1200;
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
	maxWater = 5000;

	maxSprinklers = 10;
};

datablock fxDTSBrickData(brickSmallWaterInfiniteTankData)
{
	uiName = "Infinite Small Water Tank";

	brickFile = "./bricks/smallWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/small_water_tank";

	cost = -1;
	isInfiniteWaterSource = 1;
	isWaterTank = 1;
	maxDispense = 50;

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
	maxWater = 25000;

	maxSprinklers = 10;
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
	maxWater = 75000;
	maxSprinklers = 4;
	maxDistance = 36;
};

datablock fxDTSBrickData(brickMassiveWaterTankData) 
{
	category = "Farming";
	subCategory = "Water Tanks";
	uiName = "Massive Water Tank";

	brickFile = "./bricks/massiveWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/massive_water_tank";

	cost = 4000;
	isWaterTank = 1;
	maxWater = 200000;
	maxSprinklers = 4;
	maxDistance = 68;
};


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
};


///////////
//Storage//
///////////

datablock fxDTSBrickData(brickStorageCrateData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Storage Crate";

	brickFile = "./bricks/storageCrate.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/storage_crate";

	isStorageBrick = 1;
	storageBonus = 1;

	cost = 100;
};

datablock fxDTSBrickData(brickSiloData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Silo (6x Storage)";

	brickFile = "./bricks/Silo.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/silo_(6x_storage)";

	isStorageBrick = 1;
	storageBonus = 6;

	cost = 800;
};
