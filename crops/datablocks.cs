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


/////////
//Crops//
/////////


//Potat
datablock fxDTSBrickData(brickPotato0CropData)
{
	uiName = "Potato0";

	brickFile = "./bricks/potato0.blb";
	cropType = "Potato";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickPotato1CropData)
{
	uiName = "Potato1";

	brickFile = "./bricks/potato1.blb";
	cropType = "Potato";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickPotato2CropData)
{
	uiName = "Potato2";

	brickFile = "./bricks/potato2.blb";
	cropType = "Potato";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickPotato3CropData)
{
	uiName = "Potato3";

	brickFile = "./bricks/potato3.blb";
	cropType = "Potato";
	stage = 3;
	isPlant = 1;
};


//Cawwot
datablock fxDTSBrickData(brickCarrot0CropData)
{
	uiName = "Carrot0";

	brickFile = "./bricks/carrot0.blb";
	cropType = "Carrot";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCarrot1CropData)
{
	uiName = "Carrot1";

	brickFile = "./bricks/carrot1.blb";
	cropType = "Carrot";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCarrot2CropData)
{
	uiName = "Carrot2";

	brickFile = "./bricks/carrot2.blb";
	cropType = "Carrot";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCarrot3CropData)
{
	uiName = "Carrot3";

	brickFile = "./bricks/carrot3.blb";
	cropType = "Carrot";
	stage = 3;
	isPlant = 1;
};


//Tomatt
datablock fxDTSBrickData(brickTomato0CropData)
{
	uiName = "Tomato0";

	brickFile = "./bricks/tomato0.blb";
	cropType = "Tomato";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTomato1CropData)
{
	uiName = "Tomato1";

	brickFile = "./bricks/tomato1.blb";
	cropType = "Tomato";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTomato2CropData)
{
	uiName = "Tomato2";

	brickFile = "./bricks/tomato2.blb";
	cropType = "Tomato";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTomato3CropData)
{
	uiName = "Tomato3";

	brickFile = "./bricks/tomato3.blb";
	cropType = "Tomato";
	stage = 3;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTomato4CropData)
{
	uiName = "Tomato4";

	brickFile = "./bricks/tomato4.blb";
	cropType = "Tomato";
	stage = 4;
	isPlant = 1;
};


//corn.
datablock fxDTSBrickData(brickCorn0CropData)
{
	uiName = "Corn0";

	brickFile = "./bricks/Corn0.blb";
	cropType = "Corn";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCorn1CropData)
{
	uiName = "Corn1";

	brickFile = "./bricks/Corn1.blb";
	cropType = "Corn";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCorn2CropData)
{
	uiName = "Corn2";

	brickFile = "./bricks/Corn2.blb";
	cropType = "Corn";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCorn3CropData)
{
	uiName = "Corn3";

	brickFile = "./bricks/Corn3.blb";
	cropType = "Corn";
	stage = 3;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCorn4CropData)
{
	uiName = "Corn4";

	brickFile = "./bricks/Corn4.blb";
	cropType = "Corn";
	stage = 4;
	isPlant = 1;
};


//cabg
datablock fxDTSBrickData(brickCabbage0CropData)
{
	uiName = "Cabbage0";

	brickFile = "./bricks/Cabbage0.blb";
	cropType = "Cabbage";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCabbage1CropData)
{
	uiName = "Cabbage1";

	brickFile = "./bricks/Cabbage1.blb";
	cropType = "Cabbage";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCabbage2CropData)
{
	uiName = "Cabbage2";

	brickFile = "./bricks/Cabbage2.blb";
	cropType = "Cabbage";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCabbage3CropData)
{
	uiName = "Cabbage3";

	brickFile = "./bricks/Cabbage3.blb";
	cropType = "Cabbage";
	stage = 3;
	isPlant = 1;
};

datablock fxDTSBrickData(brickCabbage4CropData)
{
	uiName = "Cabbage4";

	brickFile = "./bricks/Cabbage4.blb";
	cropType = "Cabbage";
	stage = 4;
	isPlant = 1;
};


//oneyon
datablock fxDTSBrickData(brickOnion0CropData)
{
	uiName = "Onion0";

	brickFile = "./bricks/Onion0.blb";
	cropType = "Onion";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickOnion1CropData)
{
	uiName = "Onion1";

	brickFile = "./bricks/Onion1.blb";
	cropType = "Onion";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickOnion2CropData)
{
	uiName = "Onion2";

	brickFile = "./bricks/Onion2.blb";
	cropType = "Onion";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickOnion3CropData)
{
	uiName = "Onion3";

	brickFile = "./bricks/Onion3.blb";
	cropType = "Onion";
	stage = 3;
	isPlant = 1;
};


//strobry
datablock fxDTSBrickData(brickBlueberry0CropData)
{
	uiName = "Blueberry0";

	brickFile = "./bricks/Blueberry0.blb";
	cropType = "Blueberry";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickBlueberry1CropData)
{
	uiName = "Blueberry1";

	brickFile = "./bricks/Blueberry1.blb";
	cropType = "Blueberry";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickBlueberry2CropData)
{
	uiName = "Blueberry2";

	brickFile = "./bricks/Blueberry2.blb";
	cropType = "Blueberry";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickBlueberry3CropData)
{
	uiName = "Blueberry3";

	brickFile = "./bricks/Blueberry3.blb";
	cropType = "Blueberry";
	stage = 3;
	isPlant = 1;
};

datablock fxDTSBrickData(brickBlueberry4CropData)
{
	uiName = "Blueberry4";

	brickFile = "./bricks/Blueberry4.blb";
	cropType = "Blueberry";
	stage = 4;
	isPlant = 1;
};


//tornip
datablock fxDTSBrickData(brickTurnip0CropData)
{
	uiName = "Turnip0";

	brickFile = "./bricks/Turnip0.blb";
	cropType = "Turnip";
	stage = 0;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTurnip1CropData)
{
	uiName = "Turnip1";

	brickFile = "./bricks/Turnip1.blb";
	cropType = "Turnip";
	stage = 1;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTurnip2CropData)
{
	uiName = "Turnip2";

	brickFile = "./bricks/Turnip2.blb";
	cropType = "Turnip";
	stage = 2;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTurnip3CropData)
{
	uiName = "Turnip3";

	brickFile = "./bricks/Turnip3.blb";
	cropType = "Turnip";
	stage = 3;
	isPlant = 1;
};

datablock fxDTSBrickData(brickTurnip4CropData)
{
	uiName = "Turnip4";

	brickFile = "./bricks/Turnip4.blb";
	cropType = "Turnip";
	stage = 4;
	isPlant = 1;
};










//FLOWERS



//dayz
datablock fxDTSBrickData(brickDaisy0CropData)
{
	uiName = "Daisy0";

	brickFile = "./bricks/Daisy0.blb";
	cropType = "Daisy";
	stage = 0;
	isPlant = 1;
	defaultColor = 45;
};

datablock fxDTSBrickData(brickDaisy1CropData)
{
	uiName = "Daisy1";

	brickFile = "./bricks/Daisy1.blb";
	cropType = "Daisy";
	stage = 1;
	isPlant = 1;
	defaultColor = 45;
};

datablock fxDTSBrickData(brickDaisy2CropData)
{
	uiName = "Daisy2";

	brickFile = "./bricks/Daisy2.blb";
	cropType = "Daisy";
	stage = 2;
	isPlant = 1;
	defaultColor = 45;
};


//lily
datablock fxDTSBrickData(brickLily0CropData)
{
	uiName = "Lily0";

	brickFile = "./bricks/Lily0.blb";
	cropType = "Lily";
	stage = 0;
	isPlant = 1;
	defaultColor = 45;
};

datablock fxDTSBrickData(brickLily1CropData)
{
	uiName = "Lily1";

	brickFile = "./bricks/Lily1.blb";
	cropType = "Lily";
	stage = 1;
	isPlant = 1;
	defaultColor = 45;
};

datablock fxDTSBrickData(brickLily2CropData)
{
	uiName = "Lily2";

	brickFile = "./bricks/Lily2.blb";
	cropType = "Lily";
	stage = 2;
	isPlant = 1;
	defaultColor = 45;
};













//TREES

//apl
datablock fxDTSBrickData(brickAppleTree0CropData)
{
	uiName = "AppleTree0";

	brickFile = "./bricks/AppleTree0.blb";
	cropType = "Apple";
	stage = 0;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree1CropData)
{
	uiName = "AppleTree1";

	brickFile = "./bricks/AppleTree1.blb";
	cropType = "Apple";
	stage = 1;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree2CropData)
{
	uiName = "AppleTree2";

	brickFile = "./bricks/AppleTree2.blb";
	cropType = "Apple";
	stage = 2;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree3CropData)
{
	uiName = "AppleTree3";

	brickFile = "./bricks/AppleTree3.blb";
	cropType = "Apple";
	stage = 3;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree4CropData)
{
	uiName = "AppleTree4";

	brickFile = "./bricks/AppleTree4.blb";
	cropType = "Apple";
	stage = 4;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree5CropData)
{
	uiName = "AppleTree5";

	brickFile = "./bricks/AppleTree5.blb";
	cropType = "Apple";
	stage = 5;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree6CropData)
{
	uiName = "AppleTree6";

	brickFile = "./bricks/AppleTree6.blb";
	cropType = "Apple";
	stage = 6;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree7CropData)
{
	uiName = "AppleTree7";

	brickFile = "./bricks/AppleTree7.blb";
	cropType = "Apple";
	stage = 7;
	isPlant = 1;
	isTree = 1;
};

//pruned split

datablock fxDTSBrickData(brickAppleTree10CropData)
{
	uiName = "AppleTree10";

	brickFile = "./bricks/AppleTreeUnpruned0.blb";
	cropType = "Apple";
	stage = 10;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree11CropData)
{
	uiName = "AppleTree11";

	brickFile = "./bricks/AppleTreeUnpruned1.blb";
	cropType = "Apple";
	stage = 11;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree12CropData)
{
	uiName = "AppleTree12";

	brickFile = "./bricks/AppleTreeUnpruned2.blb";
	cropType = "Apple";
	stage = 12;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree20CropData)
{
	uiName = "AppleTree20";

	brickFile = "./bricks/AppleTreePruned0.blb";
	cropType = "Apple";
	stage = 20;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree21CropData)
{
	uiName = "AppleTree21";

	brickFile = "./bricks/AppleTreePruned1.blb";
	cropType = "Apple";
	stage = 21;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickAppleTree22CropData)
{
	uiName = "AppleTree22";

	brickFile = "./bricks/AppleTreePruned2.blb";
	cropType = "Apple";
	stage = 22;
	isPlant = 1;
	isTree = 1;
};






//mangi
datablock fxDTSBrickData(brickMangoTree0CropData)
{
	uiName = "MangoTree0";

	brickFile = "./bricks/Mango0.blb";
	cropType = "Mango";
	stage = 0;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree1CropData)
{
	uiName = "MangoTree1";

	brickFile = "./bricks/Mango1.blb";
	cropType = "Mango";
	stage = 1;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree2CropData)
{
	uiName = "MangoTree2";

	brickFile = "./bricks/Mango2.blb";
	cropType = "Mango";
	stage = 2;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree3CropData)
{
	uiName = "MangoTree3";

	brickFile = "./bricks/Mango3.blb";
	cropType = "Mango";
	stage = 3;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree4CropData)
{
	uiName = "MangoTree4";

	brickFile = "./bricks/Mango4.blb";
	cropType = "Mango";
	stage = 4;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree5CropData)
{
	uiName = "MangoTree5";

	brickFile = "./bricks/Mango5.blb";
	cropType = "Mango";
	stage = 5;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree6CropData)
{
	uiName = "MangoTree6";

	brickFile = "./bricks/Mango6.blb";
	cropType = "Mango";
	stage = 6;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree7CropData)
{
	uiName = "MangoTree7";

	brickFile = "./bricks/Mango7.blb";
	cropType = "Mango";
	stage = 7;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree8CropData)
{
	uiName = "MangoTree8";

	brickFile = "./bricks/Mango8.blb";
	cropType = "Mango";
	stage = 8;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree9CropData)
{
	uiName = "MangoTree9";

	brickFile = "./bricks/Mango9.blb";
	cropType = "Mango";
	stage = 9;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree10CropData)
{
	uiName = "MangoTree10";

	brickFile = "./bricks/Mango10.blb";
	cropType = "Mango";
	stage = 10;
	isPlant = 1;
	isTree = 1;
};

datablock fxDTSBrickData(brickMangoTree11CropData)
{
	uiName = "MangoTree11";

	brickFile = "./bricks/Mango11.blb";
	cropType = "Mango";
	stage = 11;
	isPlant = 1;
	isTree = 1;
};