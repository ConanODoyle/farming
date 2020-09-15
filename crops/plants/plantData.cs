//Reference//



$Farming::PlantData_["Example", 0, "timePerTick"]			= "ms";
$Farming::PlantData_["Example", 0, "nextStage"]				= "brickDB";
$Farming::PlantData_["Example", 0, "dryStage"]				= "brickDB";
$Farming::PlantData_["Example", 0, "waterPerTick"]			= "int";
$Farming::PlantData_["Example", 0, "numGrowTicks"]			= "int";
$Farming::PlantData_["Example", 0, "numDryTicks"]			= "int";

$Farming::PlantData_["Example", 0, "yield"]					= "intmin intmax";
$Farming::PlantData_["Example", 0, "item"]					= "itemDB";
$Farming::PlantData_["Example", 0, "dieOnHarvest"]			= "bool";
$Farming::PlantData_["Example", 0, "harvestTool"]			= "item uiname";
$Farming::PlantData_["Example", 0, "areaharvestTool"]		= "item uiname";
$Farming::PlantData_["Example", 0, "pruneTool"]				= "item uiname";
$Farming::PlantData_["Example", 0, "changeOnPrune"]			= "brickDB";
$Farming::PlantData_["Example", 0, "toolBuff"]				= "intminChange intmaxChange";
$Farming::PlantData_["Example", 0, "changeOnHarvest"]		= "brickDB";
$Farming::PlantData_["Example", 0, "maxHarvestTimes"]		= "intmin intmax";

$Farming::PlantData_["Example", "plantSpace"]				= "int";
$Farming::PlantData_["Example", "experienceRequired"]		= "int";
$Farming::PlantData_["Example", "experienceCost"]			= "int";
$Farming::PlantData_["Example", "harvestExperience"]			= "int";
$Farming::PlantData_["Example", "plantingExperience"]		= "int";

$Farming::PlantData_["Example", "heatWaveTimeModifier"]		= "float";
$Farming::PlantData_["Example", "heatWaveWaterModifier"]		= "float";
$Farming::PlantData_["Example", "rainTimeModifier"]			= "float";
$Farming::PlantData_["Example", "rainWaterModifier"]			= "float";

//flowers

exec("./plantData/lilyData.cs");
exec("./plantData/daisyData.cs");
exec("./plantData/roseData.cs");

//weeds

exec("./plantData/weedData.cs");

//coast

exec("./plantData/potatoData.cs");
exec("./plantData/carrotData.cs");
exec("./plantData/tomatoData.cs");
exec("./plantData/cornData.cs");
exec("./plantData/wheatData.cs");
exec("./plantData/cabbageData.cs");
exec("./plantData/onionData.cs");

exec("./plantData/blueberryData.cs");
exec("./plantData/turnipData.cs");
exec("./plantData/portobelloData.cs");

exec("./plantData/appleTreeData.cs");
exec("./plantData/mangoTreeData.cs");

//desert

exec("./plantData/chiliData.cs");
exec("./plantData/cactusData.cs");
exec("./plantData/watermelonData.cs");

exec("./plantData/dateTreeData.cs");
exec("./plantData/peachTreeData.cs");