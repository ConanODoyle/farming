//////////////
//Sprinklers//
//////////////

datablock fxDTSBrickData(brickSmallSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Small Sprinkler";

	brickFile = "./smallSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/icons/small_sprinkler";

	cost = 125;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "2 2 1";
	waterPerSecond = 30;
};

datablock fxDTSBrickData(brickSmallSprinklerDownData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Small Sprinkler Down";

	brickFile = "./smallSprinklerDown.blb";

	iconName = "Add-Ons/Server_Farming/icons/small_sprinkler_down";

	cost = 125;
	isSprinkler = 1;
	directionalOffset = "0 0 -1.9";
	boxSize = "4 4 3.4";
	waterPerSecond = 30;
	noCollide = 1;
};

datablock fxDTSBrickData(brickMedSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Medium Sprinkler";

	brickFile = "./MedSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/icons/medium_sprinkler";

	cost = 250;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "4 4 1";
	waterPerSecond = 45;
};

datablock fxDTSBrickData(brickLargeSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Large Sprinkler";

	brickFile = "./LargeSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/icons/large_sprinkler";

	cost = 400;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.4";
	boxSize = "6 6 1";
	waterPerSecond = 60;
};

datablock fxDTSBrickData(brickDripLineData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Drip Line";

	brickFile = "./DripLine.blb";

	iconName = "Add-Ons/Server_Farming/icons/drip_line";

	cost = 40;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.2";
	boxSize = "0.5 4 0.2";
	waterPerSecond = 20;
};

datablock fxDTSBrickData(brickStraightSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Straight Sprinkler";

	brickFile = "./StraightSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/icons/straight_sprinkler";

	cost = 250;
	isSprinkler = 1;
	directionalOffset = "4.5 0 -0.3";
	boxSize = "8 1 1";
	waterPerSecond = 30;
};

datablock fxDTSBrickData(brickSwaySprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Sway Sprinkler";

	brickFile = "./SwaySprinkler.blb";

	iconName = "Add-Ons/Server_Farming/icons/sway_sprinkler";

	cost = 450;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "8 2 1";
	waterPerSecond = 60;
};
