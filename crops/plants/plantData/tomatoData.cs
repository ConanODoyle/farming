//Tomato//

$Farming::Crops::PlantData_["Tomato", "heatWaveTimeModifier"]			= "1";
$Farming::Crops::PlantData_["Tomato", "heatWaveWaterModifier"]			= "2";
$Farming::Crops::PlantData_["Tomato", "rainTimeModifier"]				= "1";
$Farming::Crops::PlantData_["Tomato", "rainWaterModifier"]				= "0.5";
$Farming::Crops::PlantData_["Tomato", "plantSpace"]					= "3";
$Farming::Crops::PlantData_["Tomato", "experienceRequired"]			= "5";
$Farming::Crops::PlantData_["Tomato", "experienceCost"]				= "1";
$Farming::Crops::PlantData_["Tomato", "harvestExperience"]			= "1";
$Farming::Crops::PlantData_["Tomato", "plantingExperience"]			= "0";
$Farming::Crops::PlantData_["Tomato", "loopStages"]					= "2 3 4";

$Farming::Crops::PlantData_["Tomato", 0, "timePerTick"]				= "10000";
$Farming::Crops::PlantData_["Tomato", 0, "nextStage"]				= "brickTomato1CropData";
$Farming::Crops::PlantData_["Tomato", 0, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 0, "waterPerTick"]			= "2";
$Farming::Crops::PlantData_["Tomato", 0, "numGrowTicks"]			= "16";
$Farming::Crops::PlantData_["Tomato", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 1, "timePerTick"]				= "10000";
$Farming::Crops::PlantData_["Tomato", 1, "nextStage"]				= "brickTomato2CropData";
$Farming::Crops::PlantData_["Tomato", 1, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 1, "waterPerTick"]			= "3";
$Farming::Crops::PlantData_["Tomato", 1, "numGrowTicks"]			= "20";
$Farming::Crops::PlantData_["Tomato", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 2, "timePerTick"]				= "15000";
$Farming::Crops::PlantData_["Tomato", 2, "nextStage"]				= "brickTomato3CropData";
$Farming::Crops::PlantData_["Tomato", 2, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 2, "waterPerTick"]			= "2";
$Farming::Crops::PlantData_["Tomato", 2, "numGrowTicks"]			= "12";
$Farming::Crops::PlantData_["Tomato", 2, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 3, "timePerTick"]				= "12000";
$Farming::Crops::PlantData_["Tomato", 3, "nextStage"]				= "brickTomato4CropData";
$Farming::Crops::PlantData_["Tomato", 3, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 3, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Tomato", 3, "numGrowTicks"]			= "15";
$Farming::Crops::PlantData_["Tomato", 3, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 4, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Tomato", 4, "nextStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 4, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 4, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Tomato", 4, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Tomato", 4, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Tomato", 4, "yield"]					= "2 5";
$Farming::Crops::PlantData_["Tomato", 4, "item"]					= "TomatoItem";
$Farming::Crops::PlantData_["Tomato", 4, "dieOnHarvest"]			= "0";
$Farming::Crops::PlantData_["Tomato", 4, "harvestTool"]				= "Clipper";
$Farming::Crops::PlantData_["Tomato", 4, "areaHarvestTool"]			= "Sickle";
$Farming::Crops::PlantData_["Tomato", 4, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Tomato", 4, "changeOnHarvest"]			= "brickTomato2CropData";
$Farming::Crops::PlantData_["Tomato", 4, "maxHarvestTimes"]			= "8 12";