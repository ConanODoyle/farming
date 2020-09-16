//Tomato//

$Farming::PlantData_["Tomato", "heatWaveTimeModifier"]			= "1";
$Farming::PlantData_["Tomato", "heatWaveWaterModifier"]			= "2";
$Farming::PlantData_["Tomato", "rainTimeModifier"]				= "1";
$Farming::PlantData_["Tomato", "rainWaterModifier"]				= "0.5";
$Farming::PlantData_["Tomato", "plantSpace"]					= "3";
$Farming::PlantData_["Tomato", "experienceRequired"]			= "5";
$Farming::PlantData_["Tomato", "experienceCost"]				= "1";
$Farming::PlantData_["Tomato", "harvestExperience"]			= "1";
$Farming::PlantData_["Tomato", "plantingExperience"]			= "0";
$Farming::PlantData_["Tomato", "loopStages"]					= "2 3 4";

$Farming::PlantData_["Tomato", 0, "tickTime"]				= "10000";
$Farming::PlantData_["Tomato", 0, "wetNextStage"]				= "brickTomato1CropData";
$Farming::PlantData_["Tomato", 0, "dryNextStage"]				= "";
$Farming::PlantData_["Tomato", 0, "waterPerTick"]			= "2";
$Farming::PlantData_["Tomato", 0, "numWetTicks"]			= "16";
$Farming::PlantData_["Tomato", 0, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Tomato", 1, "tickTime"]				= "10000";
$Farming::PlantData_["Tomato", 1, "wetNextStage"]				= "brickTomato2CropData";
$Farming::PlantData_["Tomato", 1, "dryNextStage"]				= "";
$Farming::PlantData_["Tomato", 1, "waterPerTick"]			= "3";
$Farming::PlantData_["Tomato", 1, "numWetTicks"]			= "20";
$Farming::PlantData_["Tomato", 1, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Tomato", 2, "tickTime"]				= "15000";
$Farming::PlantData_["Tomato", 2, "wetNextStage"]				= "brickTomato3CropData";
$Farming::PlantData_["Tomato", 2, "dryNextStage"]				= "";
$Farming::PlantData_["Tomato", 2, "waterPerTick"]			= "2";
$Farming::PlantData_["Tomato", 2, "numWetTicks"]			= "12";
$Farming::PlantData_["Tomato", 2, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Tomato", 3, "tickTime"]				= "12000";
$Farming::PlantData_["Tomato", 3, "wetNextStage"]				= "brickTomato4CropData";
$Farming::PlantData_["Tomato", 3, "dryNextStage"]				= "";
$Farming::PlantData_["Tomato", 3, "waterPerTick"]			= "1";
$Farming::PlantData_["Tomato", 3, "numWetTicks"]			= "15";
$Farming::PlantData_["Tomato", 3, "numDryTicks"]				= "-1";

//

$Farming::PlantData_["Tomato", 4, "tickTime"]				= "0";
$Farming::PlantData_["Tomato", 4, "wetNextStage"]				= "";
$Farming::PlantData_["Tomato", 4, "dryNextStage"]				= "";
$Farming::PlantData_["Tomato", 4, "waterPerTick"]			= "1";
$Farming::PlantData_["Tomato", 4, "numWetTicks"]			= "10";
$Farming::PlantData_["Tomato", 4, "numDryTicks"]				= "-1";

$Farming::PlantData_["Tomato", 4, "yield"]					= "2 5";
$Farming::PlantData_["Tomato", 4, "item"]					= "TomatoItem";
$Farming::PlantData_["Tomato", 4, "dieOnHarvest"]			= "0";
$Farming::PlantData_["Tomato", 4, "harvestTool"]				= "Clipper";
$Farming::PlantData_["Tomato", 4, "areaHarvestTool"]			= "Sickle";
$Farming::PlantData_["Tomato", 4, "toolBuff"]				= "1 1";
$Farming::PlantData_["Tomato", 4, "changeOnHarvest"]			= "brickTomato2CropData";
$Farming::PlantData_["Tomato", 4, "maxHarvestTimes"]			= "8 12";