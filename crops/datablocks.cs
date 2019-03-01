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

datablock fxDTSBrickData(brickFlowerPotDirtData : brick1x1Data) 
{
	category = "Farming";
	subCategory = "Extra";
	uiName = "Flower Pot";

	brickFile = "./bricks/flowerPot.blb";

	iconName = "";

	cost = 80;
	isDirt = 1;
	maxWater = 30;
	isPot = 1;
	customRadius = -1.05;
};

datablock fxDTSBrickData(brick4xPlanterDirtData : brick1x1Data) 
{
	category = "Farming";
	subCategory = "Extra";
	uiName = "4x Planter";

	brickFile = "./bricks/4xPlanter.blb";

	iconName = "";

	cost = 150;
	isDirt = 1;
	maxWater = 150;
	// isPot = 1;
	isPlanter = 1;
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

	cost = 200;
	isWaterTank = 1;
	maxWater = 5000;

	maxSprinklers = 10;
};

datablock fxDTSBrickData(brickSmallWaterInfiniteTankData)
{
	uiName = "Infinite Small Water Tank";

	brickFile = "./bricks/smallWaterTank.blb";

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

	isGreenhouse = 1;

	cost = 4000;
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

	isStorageBrick = 1;
	storageBonus = 6;

	cost = 800;
};


//////////////
//Sprinklers//
//////////////

datablock fxDTSBrickData(brickSmallSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Small Sprinkler";

	brickFile = "./sprinklers/smallSprinkler.blb";

	cost = 200;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "4 4 2";
	maxDispense = 20;
};

datablock fxDTSBrickData(brickSmallSprinklerDownData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Small Sprinkler Down";

	brickFile = "./sprinklers/smallSprinklerDown.blb";

	cost = 200;
	isSprinkler = 1;
	directionalOffset = "0 0 -1.7";
	boxSize = "4 4 3";
	maxDispense = 20;
	noCollide = 1;
};

datablock fxDTSBrickData(brickMedSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Medium Sprinkler";

	brickFile = "./sprinklers/MedSprinkler.blb";

	cost = 800;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "8 8 2";
	maxDispense = 50;
};

datablock fxDTSBrickData(brickLargeSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Large Sprinkler";

	brickFile = "./sprinklers/LargeSprinkler.blb";

	cost = 1500;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.4";
	boxSize = "12 12 2";
	maxDispense = 50;
};

datablock fxDTSBrickData(brickDripLineData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Drip Line";

	brickFile = "./sprinklers/DripLine.blb";

	cost = 50;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.2";
	boxSize = "0.5 4 0.2";
	maxDispense = 15;
};

datablock fxDTSBrickData(brickStraightSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Straight Sprinkler";

	brickFile = "./sprinklers/StraightSprinkler.blb";

	cost = 600;
	isSprinkler = 1;
	directionalOffset = "4.5 0 -0.3";
	boxSize = "8 1 2";
	maxDispense = 30;
};

datablock fxDTSBrickData(brickSwaySprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Sway Sprinkler";

	brickFile = "./sprinklers/SwaySprinkler.blb";

	cost = 700;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "8 4 2";
	maxDispense = 50;
};