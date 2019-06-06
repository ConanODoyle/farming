//Reference//



$Farming::Crops::PlantData_["Example", 0, "timePerTick"]			= "ms";
$Farming::Crops::PlantData_["Example", 0, "nextStage"]				= "brickDB";
$Farming::Crops::PlantData_["Example", 0, "dryStage"]				= "brickDB";
$Farming::Crops::PlantData_["Example", 0, "waterPerTick"]			= "int";
$Farming::Crops::PlantData_["Example", 0, "numGrowTicks"]			= "int";
$Farming::Crops::PlantData_["Example", 0, "numDryTicks"]			= "int";

$Farming::Crops::PlantData_["Example", 0, "yield"]					= "intmin intmax";
$Farming::Crops::PlantData_["Example", 0, "item"]					= "itemDB";
$Farming::Crops::PlantData_["Example", 0, "dieOnHarvest"]			= "bool";
$Farming::Crops::PlantData_["Example", 0, "harvestTool"]			= "item uiname";
$Farming::Crops::PlantData_["Example", 0, "areaharvestTool"]		= "item uiname";
$Farming::Crops::PlantData_["Example", 0, "pruneTool"]				= "item uiname";
$Farming::Crops::PlantData_["Example", 0, "changeOnPrune"]			= "brickDB";
$Farming::Crops::PlantData_["Example", 0, "toolBuff"]				= "intminChange intmaxChange";
$Farming::Crops::PlantData_["Example", 0, "changeOnHarvest"]		= "brickDB";

$Farming::Crops::PlantData_["Example", "plantSpace"]				= "int";
$Farming::Crops::PlantData_["Example", "experienceRequired"]		= "int";
$Farming::Crops::PlantData_["Example", "experienceCost"]			= "int";
$Farming::Crops::PlantData_["Example", "harvestExperience"]			= "int";
$Farming::Crops::PlantData_["Example", "plantingExperience"]		= "int";

$Farming::Crops::PlantData_["Example", "heatWaveTimeModifier"]		= "float";
$Farming::Crops::PlantData_["Example", "heatWaveWaterModifier"]		= "float";
$Farming::Crops::PlantData_["Example", "rainTimeModifier"]			= "float";
$Farming::Crops::PlantData_["Example", "rainWaterModifier"]			= "float";

//coast

exec("./plantData/potatoData.cs");
exec("./plantData/carrotData.cs");
exec("./plantData/tomatoData.cs");
exec("./plantData/cornData.cs");
exec("./plantData/cabbageData.cs");
exec("./plantData/onionData.cs");

exec("./plantData/blueberryData.cs");
exec("./plantData/turnipData.cs");

exec("./plantData/appleTreeData.cs");
exec("./plantData/mangoTreeData.cs");

//desert

exec("./plantData/chiliData.cs");
exec("./plantData/cactusData.cs");
// exec("./plantData/watermelonData.cs");

exec("./plantData/dateTreeData.cs");
exec("./plantData/peachTreeData.cs");