//Watermelon//

$Farming::PlantData_["Watermelon", "heatWaveTimeModifier"]			= "0.5";
$Farming::PlantData_["Watermelon", "heatWaveWaterModifier"]			= "1";
$Farming::PlantData_["Watermelon", "rainTimeModifier"]				= "4";
$Farming::PlantData_["Watermelon", "rainWaterModifier"]				= "1";
$Farming::PlantData_["Watermelon", "plantSpace"]					= "3";
$Farming::PlantData_["Watermelon", "experienceRequired"]			= "100";
$Farming::PlantData_["Watermelon", "experienceCost"]				= "100";
$Farming::PlantData_["Watermelon", "harvestExperience"]			= "2";
$Farming::PlantData_["Watermelon", "plantingExperience"]			= "0";
$Farming::PlantData_["Watermelon", "loopStages"]					= "3 4 5";

$Farming::PlantData_["Watermelon", 0, "tickTime"]				= "6000";
$Farming::PlantData_["Watermelon", 0, "nextStage"]				= "brickWatermelon1CropData";
$Farming::PlantData_["Watermelon", 0, "dryStage"]				= "";
$Farming::PlantData_["Watermelon", 0, "waterPerTick"]			= "3";
$Farming::PlantData_["Watermelon", 0, "numWetTicks"]			= "15";
$Farming::PlantData_["Watermelon", 0, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Watermelon", 1, "tickTime"]				= "7000";
$Farming::PlantData_["Watermelon", 1, "nextStage"]				= "brickWatermelon2CropData";
$Farming::PlantData_["Watermelon", 1, "dryStage"]				= "";
$Farming::PlantData_["Watermelon", 1, "waterPerTick"]			= "4";
$Farming::PlantData_["Watermelon", 1, "numWetTicks"]			= "20";
$Farming::PlantData_["Watermelon", 1, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Watermelon", 2, "tickTime"]				= "8000";
$Farming::PlantData_["Watermelon", 2, "nextStage"]				= "brickWatermelon3CropData";
$Farming::PlantData_["Watermelon", 2, "dryStage"]				= "";
$Farming::PlantData_["Watermelon", 2, "waterPerTick"]			= "5";
$Farming::PlantData_["Watermelon", 2, "numWetTicks"]			= "20";
$Farming::PlantData_["Watermelon", 2, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Watermelon", 3, "tickTime"]				= "16000";
$Farming::PlantData_["Watermelon", 3, "nextStage"]				= "brickWatermelon4CropData";
$Farming::PlantData_["Watermelon", 3, "dryStage"]				= "";
$Farming::PlantData_["Watermelon", 3, "waterPerTick"]			= "8";
$Farming::PlantData_["Watermelon", 3, "numWetTicks"]			= "25";
$Farming::PlantData_["Watermelon", 3, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Watermelon", 4, "tickTime"]				= "6000";
$Farming::PlantData_["Watermelon", 4, "nextStage"]				= "brickWatermelon5CropData";
$Farming::PlantData_["Watermelon", 4, "dryStage"]				= "brickWatermelon3CropData";
$Farming::PlantData_["Watermelon", 4, "waterPerTick"]			= "30";
$Farming::PlantData_["Watermelon", 4, "numWetTicks"]			= "60";
$Farming::PlantData_["Watermelon", 4, "numDryTicks"]				= "10";

//

$Farming::PlantData_["Watermelon", 5, "tickTime"]				= "0";
$Farming::PlantData_["Watermelon", 5, "nextStage"]				= "";
$Farming::PlantData_["Watermelon", 5, "dryStage"]				= "";
$Farming::PlantData_["Watermelon", 5, "waterPerTick"]			= "1";
$Farming::PlantData_["Watermelon", 5, "numWetTicks"]			= "3";
$Farming::PlantData_["Watermelon", 5, "numDryTicks"]				= "-1";

$Farming::PlantData_["Watermelon", 5, "yield"]					= "1 2";
$Farming::PlantData_["Watermelon", 5, "item"]					= "WatermelonItem";
$Farming::PlantData_["Watermelon", 5, "dieOnHarvest"]			= "0";
$Farming::PlantData_["Watermelon", 5, "harvestTool"]				= "Clipper";
$Farming::PlantData_["Watermelon", 5, "areaHarvestTool"]			= "";
$Farming::PlantData_["Watermelon", 5, "toolBuff"]				= "0 1";
$Farming::PlantData_["Watermelon", 5, "changeOnHarvest"]			= "brickWatermelon3CropData";
$Farming::PlantData_["Watermelon", 5, "maxHarvestTimes"]			= "10 24";
