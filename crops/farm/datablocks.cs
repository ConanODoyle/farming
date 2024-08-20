////////
//Dirt//
////////

datablock fxDTSBrickData(brick4x4DirtData : brick4x4Data)
{
	category = "Farming";
	subCategory = "Dirt";
	uiName = "4x4 Dirt";
	description = "(Holds 150 water, 80 nutrients)";

	cost = 10;
	isDirt = 1;
	maxWater = 150;
	maxNutrients = 80;
	maxWeedkiller = 200;
};

datablock fxDTSBrickData(brick8x8DirtData : brick8x8Data)
{
	category = "Farming";
	subCategory = "Dirt";
	uiName = "8x8 Dirt";
	description = "(Holds 1200 water, 300 nutrients)";

	cost = 80;
	isDirt = 1;
	maxWater = 1200;
	maxNutrients = 300;
	maxWeedkiller = 1000;
};


///////////////
//Water Tanks//
///////////////

datablock fxDTSBrickData(brickSmallWaterTankData)
{
	category = "Farming";
	subCategory = "Water Tanks";
	uiName = "Small Water Tank";
	description = "(Holds 10000 water)";

	brickFile = "./bricks/smallWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/icons/small_water_tank";

	cost = 150;
	isWaterTank = 1;
	maxWater = 10000;

	canConnectSprinklers = 1;
	maxConnections = 4;
};

datablock fxDTSBrickData(brickSmallWaterInfiniteTankData)
{
	uiName = "Infinite Small Water Tank";

	brickFile = "./bricks/smallWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/icons/small_water_tank";

	cost = -1;
	isInfiniteWaterSource = 1;
	isWaterTank = 1;
	maxWater = 10000;
	waterPerSecond = 50;

	canConnectSprinklers = 1;
	maxConnections = 100;
};

datablock fxDTSBrickData(brickMediumWaterTankData)
{
	category = "Farming";
	subCategory = "Water Tanks";
	uiName = "Medium Water Tank";
	description = "(Holds 50000 water)";

	brickFile = "./bricks/medWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/icons/medium_water_tank";

	cost = 600;
	isWaterTank = 1;
	maxWater = 50000;
	isOutflowTank = 1;

	canConnectSprinklers = 1;
	maxConnections = 4;
};

datablock fxDTSBrickData(brickLargeWaterTankData)
{
	category = "Farming";
	subCategory = "Water Tanks";
	uiName = "Large Water Tank";
	description = "(Holds 100000 water)";

	brickFile = "./bricks/largeWaterTank.blb";

	iconName = "Add-Ons/Server_Farming/icons/large_water_tank";

	cost = 1800;
	isWaterTank = 1;
	maxWater = 100000;
	maxSprinklers = 0;
	isOutflowTank = 2;
	
	maxConnections = 4;
};

// datablock fxDTSBrickData(brickMassiveWaterTankData)
// {
// 	category = "Farming";
// 	subCategory = "Water Tanks";
// 	uiName = "Massive Water Tank";

// 	brickFile = "./bricks/massiveWaterTank.blb";

// 	iconName = "Add-Ons/Server_Farming/icons/massive_water_tank";

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
	description = "(/help greenhouse for info)";

	brickFile = "./bricks/Greenhouse.blb";

	iconName = "Add-Ons/Server_Farming/icons/greenhouse";

	isGreenhouse = 1;

	cost = 3000;
};


///////////////
//Shop Bricks//
///////////////

datablock fxDTSBrickData(brickShopStallData)
{
	category = "Farming";
	subCategory = "Extra";
	uiName = "Shop Stall";
	description = "(Sell items to other players)";

	brickFile = "./bricks/shopBrick.blb";

	iconName = "Add-Ons/Server_Farming/icons/shopstall";

	isShop = 1;
	isStorageBrick = 1;
	storageSlotCount = 4;
	storageMultiplier = 1;
	itemStackCount = 2;

	cost = 250;
	itemPos0 = "1 0.7 0";
	itemPos1 = "1 -0.7 0";
	itemPos2 = "0 0.7 0.4";
	itemPos3 = "0 -0.7 0.4";
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

	iconName = "Add-Ons/Server_Farming/icons/storage_crate";

	isStorageBrick = 1;
	storageSlotCount = 4;
	storageMultiplier = 1;
	itemStackCount = 2;

	cost = 80;
};

datablock fxDTSBrickData(brickStorageCratePrintData : brickStorageCrateData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Storage Crate Print";
	description = "(4 slots, has print)";

	brickFile = "./bricks/storageCratePrint.blb";

	iconName = "Add-Ons/Server_Farming/icons/storage_crate";

	hasPrint = 1;
	printAspectRatio = "2x2f";
	orientationFix = 1;

	cost = 90;
};

datablock fxDTSBrickData(brickSiloData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Silo";
	description = "(1 slot, 45x storage, crops only)";

	brickFile = "./bricks/Silo.blb";

	iconName = "Add-Ons/Server_Farming/icons/silo";
	orientationFix = 2;

	isStorageBrick = 1;
	storageSlotCount = 1;
	storageMultiplier = 45;
	storageType = "Crops";
	itemStackCount = 0;

	cost = 500;
};

datablock fxDTSBrickData(brickLargeSiloData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Large Silo";
	description = "(1 slot, 120x storage, crops only)";

	brickFile = "./bricks/LargeSilo.blb";

	iconName = "Add-Ons/Server_Farming/icons/silo";
	orientationFix = 2;

	isStorageBrick = 1;
	storageSlotCount = 1;
	storageMultiplier = 120;
	storageType = "Crops";
	itemStackCount = 0;

	cost = 1100;
};

datablock fxDTSBrickData(brickLargeToolboxData)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Toolbox";
	description = "(8 slots, tools only)";

	brickFile = "./bricks/toolboxLargeFeatures.blb";

	iconName = "Add-Ons/Server_Farming/icons/toolbox";

	isStorageBrick = 1;
	storageSlotCount = 8;
	storageMultiplier = 0;
	storageType = "Tools";
	itemStackCount = 1;

	cost = 10;
};

datablock fxDTSBrickData(brickSafe3x3x3Data)
{
	category = "Farming";
	subCategory = "Storage";
	uiName = "Currency Safe";
	description = "(Stores currencies)";

	brickFile = "./bricks/safe3x3x3.blb";

	iconName = "";

	isStorageBrick = 1;
	storageSlotCount = 3;
	storageMultiplier = 10;
	storageType = "Currency";
	itemStackCount = 1;

	cost = 200;
};


///////////////////
//Money and Decor//
///////////////////

datablock fxDTSBrickData(brickGoldIngotData)
{
	category = "Farming";
	subCategory = "Extra";
	uiName = "Gold Ingot";
	description = "(Does not block light)";

	brickFile = "./bricks/goldingot.blb";

	iconName = "Add-Ons/Server_Farming/icons/Gold_Ingot";

	cost = 15000;
	customRefundCost = 15000;
};

datablock fxDTSBrickData(brickPergolaData)
{
	category = "Farming";
	subCategory = "Decor";
	uiName = "Pergola";
	description = "(Does not block light)";

	brickFile = "./bricks/pergola.blb";

	iconName = "Add-Ons/Server_Farming/icons/Pergola";

	allowLightThrough = 1;
	cost = 20;
	customRefundCost = 20;
};

datablock fxDTSBrickData(brickScarecrowData)
{
	category = "Farming";
	subCategory = "Decor";
	uiName = "Scarecrow";

	brickFile = "./bricks/scarecrow.blb";

	iconName = "Add-Ons/Server_Farming/icons/Scarecrow";

	allowLightThrough = 1;
	cost = 200;
	customRefundCost = 200;
	customRadius = 1.95;
};

datablock fxDTSBrickData (brickGarageDoor4xOpenData)
{
	brickFile = "./bricks/GarageDoor4xOpen.blb";
	uiName = "Farming Garage Door 4x";
	
	isDoor = 1;
	isOpen = 1;
	orientationFix = 4;
	
	closedCW = "brickGarageDoor4xClosedData";
	stage1CW = "brickGarageDoor4xHalfOpenData";
	stage1CWTime = 200;
	openCW = "brickGarageDoor4xOpenData";
	
	closedCCW = "brickGarageDoor4xClosedData";
	stage1CCW = "brickGarageDoor4xHalfOpenData";
	stage1CCWTime = 200;
	openCCW = "brickGarageDoor4xOpenData";
	
	orientationFix = 1;
};

datablock fxDTSBrickData (brickGarageDoor4xHalfOpenData : brickGarageDoor4xOpenData)
{
	brickFile = "./bricks/GarageDoor4xHalfOpen.blb";

	isOpen = 1;
};

datablock fxDTSBrickData (brickGarageDoor4xClosedData : brickGarageDoor4xOpenData)
{
	brickFile = "./bricks/GarageDoor4xClosed.blb";
	category = "Special";
	subCategory = "Doors";
	
	isOpen = 0;
};

datablock fxDTSBrickData (brickGarageDoor8xOpenData)
{
	brickFile = "./bricks/GarageDoor8xOpen.blb";
	uiName = "Farming Garage Door 8x";
	
	isDoor = 1;
	isOpen = 1;
	orientationFix = 4;
	
	closedCW = "brickGarageDoor8xClosedData";
	stage1CW = "brickGarageDoor8xHalfOpenData";
	stage1CWTime = 200;
	openCW = "brickGarageDoor8xOpenData";
	
	closedCCW = "brickGarageDoor8xClosedData";
	stage1CCW = "brickGarageDoor8xHalfOpenData";
	stage1CCWTime = 200;
	openCCW = "brickGarageDoor8xOpenData";
	
	orientationFix = 1;
};

datablock fxDTSBrickData (brickGarageDoor8xHalfOpenData : brickGarageDoor8xOpenData)
{
	brickFile = "./bricks/GarageDoor8xHalfOpen.blb";

	isOpen = 1;
};

datablock fxDTSBrickData (brickGarageDoor8xClosedData : brickGarageDoor8xOpenData)
{
	brickFile = "./bricks/GarageDoor8xClosed.blb";
	category = "Special";
	subCategory = "Doors";
	
	isOpen = 0;
};

datablock fxDTSBrickData (brickGarageDoor12xOpenData)
{
	brickFile = "./bricks/GarageDoor12xOpen.blb";
	uiName = "Farming Garage Door 12x";
	
	isDoor = 1;
	isOpen = 1;
	orientationFix = 4;
	
	closedCW = "brickGarageDoor12xClosedData";
	stage1CW = "brickGarageDoor12xHalfOpenData";
	stage1CWTime = 200;
	openCW = "brickGarageDoor12xOpenData";
	
	closedCCW = "brickGarageDoor12xClosedData";
	stage1CCW = "brickGarageDoor12xHalfOpenData";
	stage1CCWTime = 200;
	openCCW = "brickGarageDoor12xOpenData";
	
	orientationFix = 1;
};

datablock fxDTSBrickData (brickGarageDoor12xHalfOpenData : brickGarageDoor12xOpenData)
{
	brickFile = "./bricks/GarageDoor12xHalfOpen.blb";

	isOpen = 1;
};

datablock fxDTSBrickData (brickGarageDoor12xClosedData : brickGarageDoor12xOpenData)
{
	brickFile = "./bricks/GarageDoor12xClosed.blb";
	category = "Special";
	subCategory = "Doors";
	
	isOpen = 0;
};


//////////////////
//Quest Cabinets//
//////////////////

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

	cost = 200;

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

	cost = 200;

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
