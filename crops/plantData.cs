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



//Potato//

$Farming::Crops::PlantData_["Potato", "plantSpace"]					= "1";
$Farming::Crops::PlantData_["Potato", "experienceRequired"]			= "0";
$Farming::Crops::PlantData_["Potato", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Potato", "plantingExperience"]			= "2";

$Farming::Crops::PlantData_["Potato", 0, "timePerTick"]				= "4000";
$Farming::Crops::PlantData_["Potato", 0, "nextStage"]				= "brickPotato1CropData";
$Farming::Crops::PlantData_["Potato", 0, "dryStage"]				= "";
$Farming::Crops::PlantData_["Potato", 0, "waterPerTick"]			= "2";
$Farming::Crops::PlantData_["Potato", 0, "numGrowTicks"]			= "5";
$Farming::Crops::PlantData_["Potato", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Potato", 1, "timePerTick"]				= "4000";
$Farming::Crops::PlantData_["Potato", 1, "nextStage"]				= "brickPotato2CropData";
$Farming::Crops::PlantData_["Potato", 1, "dryStage"]				= "";
$Farming::Crops::PlantData_["Potato", 1, "waterPerTick"]			= "2";
$Farming::Crops::PlantData_["Potato", 1, "numGrowTicks"]			= "5";
$Farming::Crops::PlantData_["Potato", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Potato", 2, "timePerTick"]				= "6000";
$Farming::Crops::PlantData_["Potato", 2, "nextStage"]				= "brickPotato3CropData";
$Farming::Crops::PlantData_["Potato", 2, "dryStage"]				= "";
$Farming::Crops::PlantData_["Potato", 2, "waterPerTick"]			= "5";
$Farming::Crops::PlantData_["Potato", 2, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Potato", 2, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Potato", 2, "yield"]					= "0 2";
$Farming::Crops::PlantData_["Potato", 2, "item"]					= "PotatoItem";
$Farming::Crops::PlantData_["Potato", 2, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Potato", 2, "harvestTool"]				= "Trowel";
$Farming::Crops::PlantData_["Potato", 2, "areaHarvestTool"]			= "Hoe";
$Farming::Crops::PlantData_["Potato", 2, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Potato", 2, "changeOnHarvest"]			= "";

//

$Farming::Crops::PlantData_["Potato", 3, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Potato", 3, "nextStage"]				= "brickPotato3CropData";
$Farming::Crops::PlantData_["Potato", 3, "dryStage"]				= "";
$Farming::Crops::PlantData_["Potato", 3, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Potato", 3, "numGrowTicks"]			= "3";
$Farming::Crops::PlantData_["Potato", 3, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Potato", 3, "yield"]					= "1 3";
$Farming::Crops::PlantData_["Potato", 3, "item"]					= "PotatoItem";
$Farming::Crops::PlantData_["Potato", 3, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Potato", 3, "harvestTool"]				= "Trowel";
$Farming::Crops::PlantData_["Potato", 3, "areaHarvestTool"]			= "Hoe";
$Farming::Crops::PlantData_["Potato", 3, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Potato", 3, "changeOnHarvest"]			= "";



//Carrot//

$Farming::Crops::PlantData_["Carrot", "plantSpace"]					= "1";
$Farming::Crops::PlantData_["Carrot", "experienceRequired"]			= "0";
$Farming::Crops::PlantData_["Carrot", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Carrot", "plantingExperience"]			= "1";

$Farming::Crops::PlantData_["Carrot", 0, "timePerTick"]				= "6000";
$Farming::Crops::PlantData_["Carrot", 0, "nextStage"]				= "brickCarrot1CropData";
$Farming::Crops::PlantData_["Carrot", 0, "dryStage"]				= "";
$Farming::Crops::PlantData_["Carrot", 0, "waterPerTick"]			= "5";
$Farming::Crops::PlantData_["Carrot", 0, "numGrowTicks"]			= "5";
$Farming::Crops::PlantData_["Carrot", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Carrot", 1, "timePerTick"]				= "6000";
$Farming::Crops::PlantData_["Carrot", 1, "nextStage"]				= "brickCarrot2CropData";
$Farming::Crops::PlantData_["Carrot", 1, "dryStage"]				= "";
$Farming::Crops::PlantData_["Carrot", 1, "waterPerTick"]			= "8";
$Farming::Crops::PlantData_["Carrot", 1, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Carrot", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Carrot", 2, "timePerTick"]				= "7000";
$Farming::Crops::PlantData_["Carrot", 2, "nextStage"]				= "brickCarrot3CropData";
$Farming::Crops::PlantData_["Carrot", 2, "dryStage"]				= "";
$Farming::Crops::PlantData_["Carrot", 2, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Carrot", 2, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Carrot", 2, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Carrot", 2, "yield"]					= "0 2";
$Farming::Crops::PlantData_["Carrot", 2, "item"]					= "CarrotItem";
$Farming::Crops::PlantData_["Carrot", 2, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Carrot", 2, "harvestTool"]				= "Trowel";
$Farming::Crops::PlantData_["Carrot", 2, "areaHarvestTool"]			= "Hoe";
$Farming::Crops::PlantData_["Carrot", 2, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Carrot", 2, "changeOnHarvest"]			= "";

//

$Farming::Crops::PlantData_["Carrot", 3, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Carrot", 3, "nextStage"]				= "brickCarrot3CropData";
$Farming::Crops::PlantData_["Carrot", 3, "dryStage"]				= "";
$Farming::Crops::PlantData_["Carrot", 3, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Carrot", 3, "numGrowTicks"]			= "3";
$Farming::Crops::PlantData_["Carrot", 3, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Carrot", 3, "yield"]					= "1 2";
$Farming::Crops::PlantData_["Carrot", 3, "item"]					= "CarrotItem";
$Farming::Crops::PlantData_["Carrot", 3, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Carrot", 3, "harvestTool"]				= "Trowel";
$Farming::Crops::PlantData_["Carrot", 3, "areaHarvestTool"]			= "Hoe";
$Farming::Crops::PlantData_["Carrot", 3, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Carrot", 3, "changeOnHarvest"]			= "";



//Tomato//

$Farming::Crops::PlantData_["Tomato", "plantSpace"]					= "2";
$Farming::Crops::PlantData_["Tomato", "experienceRequired"]			= "120";
$Farming::Crops::PlantData_["Tomato", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Tomato", "plantingExperience"]			= "1";

$Farming::Crops::PlantData_["Tomato", 0, "timePerTick"]				= "10000";
$Farming::Crops::PlantData_["Tomato", 0, "nextStage"]				= "brickTomato1CropData";
$Farming::Crops::PlantData_["Tomato", 0, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 0, "waterPerTick"]			= "5";
$Farming::Crops::PlantData_["Tomato", 0, "numGrowTicks"]			= "16";
$Farming::Crops::PlantData_["Tomato", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 1, "timePerTick"]				= "10000";
$Farming::Crops::PlantData_["Tomato", 1, "nextStage"]				= "brickTomato2CropData";
$Farming::Crops::PlantData_["Tomato", 1, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 1, "waterPerTick"]			= "5";
$Farming::Crops::PlantData_["Tomato", 1, "numGrowTicks"]			= "20";
$Farming::Crops::PlantData_["Tomato", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 2, "timePerTick"]				= "16000";
$Farming::Crops::PlantData_["Tomato", 2, "nextStage"]				= "brickTomato3CropData";
$Farming::Crops::PlantData_["Tomato", 2, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 2, "waterPerTick"]			= "2";
$Farming::Crops::PlantData_["Tomato", 2, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Tomato", 2, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 3, "timePerTick"]				= "16000";
$Farming::Crops::PlantData_["Tomato", 3, "nextStage"]				= "brickTomato4CropData";
$Farming::Crops::PlantData_["Tomato", 3, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 3, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Tomato", 3, "numGrowTicks"]			= "16";
$Farming::Crops::PlantData_["Tomato", 3, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Tomato", 4, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Tomato", 4, "nextStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 4, "dryStage"]				= "";
$Farming::Crops::PlantData_["Tomato", 4, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Tomato", 4, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Tomato", 4, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Tomato", 4, "yield"]					= "1 4";
$Farming::Crops::PlantData_["Tomato", 4, "item"]					= "TomatoItem";
$Farming::Crops::PlantData_["Tomato", 4, "dieOnHarvest"]			= "0";
$Farming::Crops::PlantData_["Tomato", 4, "harvestTool"]				= "Clipper";
$Farming::Crops::PlantData_["Tomato", 4, "areaHarvestTool"]			= "Sickle";
$Farming::Crops::PlantData_["Tomato", 4, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Tomato", 4, "changeOnHarvest"]			= "brickTomato2CropData";



//Corn//

$Farming::Crops::PlantData_["Corn", "plantSpace"]					= "1";
$Farming::Crops::PlantData_["Corn", "experienceRequired"]			= "400";
$Farming::Crops::PlantData_["Corn", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Corn", "plantingExperience"]			= "1";

$Farming::Crops::PlantData_["Corn", 0, "timePerTick"]				= "7000";
$Farming::Crops::PlantData_["Corn", 0, "nextStage"]					= "brickCorn1CropData";
$Farming::Crops::PlantData_["Corn", 0, "dryStage"]					= "";
$Farming::Crops::PlantData_["Corn", 0, "waterPerTick"]				= "7";
$Farming::Crops::PlantData_["Corn", 0, "numGrowTicks"]				= "5";
$Farming::Crops::PlantData_["Corn", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Corn", 1, "timePerTick"]				= "7000";
$Farming::Crops::PlantData_["Corn", 1, "nextStage"]					= "brickCorn2CropData";
$Farming::Crops::PlantData_["Corn", 1, "dryStage"]					= "";
$Farming::Crops::PlantData_["Corn", 1, "waterPerTick"]				= "4";
$Farming::Crops::PlantData_["Corn", 1, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Corn", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Corn", 2, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Corn", 2, "nextStage"]					= "brickCorn3CropData";
$Farming::Crops::PlantData_["Corn", 2, "dryStage"]					= "";
$Farming::Crops::PlantData_["Corn", 2, "waterPerTick"]				= "4";
$Farming::Crops::PlantData_["Corn", 2, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Corn", 2, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Corn", 3, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Corn", 3, "nextStage"]					= "brickCorn4CropData";
$Farming::Crops::PlantData_["Corn", 3, "dryStage"]					= "";
$Farming::Crops::PlantData_["Corn", 3, "waterPerTick"]				= "4";
$Farming::Crops::PlantData_["Corn", 3, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Corn", 3, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Corn", 3, "yield"]						= "0 2";
$Farming::Crops::PlantData_["Corn", 3, "item"]						= "CornItem";
$Farming::Crops::PlantData_["Corn", 3, "dieOnHarvest"]				= "1";
$Farming::Crops::PlantData_["Corn", 3, "harvestTool"]				= "Clipper";
$Farming::Crops::PlantData_["Corn", 3, "areaHarvestTool"]			= "Sickle";
$Farming::Crops::PlantData_["Corn", 3, "toolBuff"]					= "1 1";
$Farming::Crops::PlantData_["Corn", 3, "changeOnHarvest"]			= "";

//

$Farming::Crops::PlantData_["Corn", 4, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Corn", 4, "nextStage"]					= "";
$Farming::Crops::PlantData_["Corn", 4, "dryStage"]					= "";
$Farming::Crops::PlantData_["Corn", 4, "waterPerTick"]				= "1";
$Farming::Crops::PlantData_["Corn", 4, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Corn", 4, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Corn", 4, "yield"]						= "1 3";
$Farming::Crops::PlantData_["Corn", 4, "item"]						= "CornItem";
$Farming::Crops::PlantData_["Corn", 4, "dieOnHarvest"]				= "1";
$Farming::Crops::PlantData_["Corn", 4, "harvestTool"]				= "Clipper";
$Farming::Crops::PlantData_["Corn", 4, "areaHarvestTool"]			= "Sickle";
$Farming::Crops::PlantData_["Corn", 4, "toolBuff"]					= "1 1";
$Farming::Crops::PlantData_["Corn", 4, "changeOnHarvest"]			= "";



//Cabbage//

$Farming::Crops::PlantData_["Cabbage", "plantSpace"]				= "2";
$Farming::Crops::PlantData_["Cabbage", "experienceRequired"]		= "400";
$Farming::Crops::PlantData_["Cabbage", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Cabbage", "plantingExperience"]		= "1";

$Farming::Crops::PlantData_["Cabbage", 0, "timePerTick"]			= "5500";
$Farming::Crops::PlantData_["Cabbage", 0, "nextStage"]				= "brickCabbage1CropData";
$Farming::Crops::PlantData_["Cabbage", 0, "dryStage"]				= "";
$Farming::Crops::PlantData_["Cabbage", 0, "waterPerTick"]			= "15";
$Farming::Crops::PlantData_["Cabbage", 0, "numGrowTicks"]			= "6";
$Farming::Crops::PlantData_["Cabbage", 0, "numDryTicks"]			= "-1";

//

$Farming::Crops::PlantData_["Cabbage", 1, "timePerTick"]			= "5500";
$Farming::Crops::PlantData_["Cabbage", 1, "nextStage"]				= "brickCabbage2CropData";
$Farming::Crops::PlantData_["Cabbage", 1, "dryStage"]				= "";
$Farming::Crops::PlantData_["Cabbage", 1, "waterPerTick"]			= "15";
$Farming::Crops::PlantData_["Cabbage", 1, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Cabbage", 1, "numDryTicks"]			= "-1";

//

$Farming::Crops::PlantData_["Cabbage", 2, "timePerTick"]			= "5500";
$Farming::Crops::PlantData_["Cabbage", 2, "nextStage"]				= "brickCabbage3CropData";
$Farming::Crops::PlantData_["Cabbage", 2, "dryStage"]				= "";
$Farming::Crops::PlantData_["Cabbage", 2, "waterPerTick"]			= "20";
$Farming::Crops::PlantData_["Cabbage", 2, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Cabbage", 2, "numDryTicks"]			= "-1";

//

$Farming::Crops::PlantData_["Cabbage", 3, "timePerTick"]			= "5500";
$Farming::Crops::PlantData_["Cabbage", 3, "nextStage"]				= "brickCabbage4CropData";
$Farming::Crops::PlantData_["Cabbage", 3, "dryStage"]				= "";
$Farming::Crops::PlantData_["Cabbage", 3, "waterPerTick"]			= "20";
$Farming::Crops::PlantData_["Cabbage", 3, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Cabbage", 3, "numDryTicks"]			= "-1";

$Farming::Crops::PlantData_["Cabbage", 3, "yield"]					= "0 1";
$Farming::Crops::PlantData_["Cabbage", 3, "item"]					= "CabbageItem";
$Farming::Crops::PlantData_["Cabbage", 3, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Cabbage", 3, "harvestTool"]			= "Clipper";
$Farming::Crops::PlantData_["Cabbage", 3, "areaHarvestTool"]		= "Sickle";
$Farming::Crops::PlantData_["Cabbage", 3, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Cabbage", 3, "changeOnHarvest"]		= "";

//

$Farming::Crops::PlantData_["Cabbage", 4, "timePerTick"]			= "0";
$Farming::Crops::PlantData_["Cabbage", 4, "nextStage"]				= "";
$Farming::Crops::PlantData_["Cabbage", 4, "dryStage"]				= "";
$Farming::Crops::PlantData_["Cabbage", 4, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Cabbage", 4, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Cabbage", 4, "numDryTicks"]			= "-1";

$Farming::Crops::PlantData_["Cabbage", 4, "yield"]					= "1 3";
$Farming::Crops::PlantData_["Cabbage", 4, "item"]					= "CabbageItem";
$Farming::Crops::PlantData_["Cabbage", 4, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Cabbage", 4, "harvestTool"]			= "Clipper";
$Farming::Crops::PlantData_["Cabbage", 4, "areaHarvestTool"]		= "Sickle";
$Farming::Crops::PlantData_["Cabbage", 4, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Cabbage", 4, "changeOnHarvest"]		= "";



//Onion//

$Farming::Crops::PlantData_["Onion", "plantSpace"]					= "1";
$Farming::Crops::PlantData_["Onion", "experienceRequired"]			= "250";
$Farming::Crops::PlantData_["Onion", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Onion", "plantingExperience"]			= "1";

$Farming::Crops::PlantData_["Onion", 0, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Onion", 0, "nextStage"]				= "brickOnion1CropData";
$Farming::Crops::PlantData_["Onion", 0, "dryStage"]					= "";
$Farming::Crops::PlantData_["Onion", 0, "waterPerTick"]				= "5";
$Farming::Crops::PlantData_["Onion", 0, "numGrowTicks"]				= "8";
$Farming::Crops::PlantData_["Onion", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Onion", 1, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Onion", 1, "nextStage"]				= "brickOnion2CropData";
$Farming::Crops::PlantData_["Onion", 1, "dryStage"]					= "";
$Farming::Crops::PlantData_["Onion", 1, "waterPerTick"]				= "5";
$Farming::Crops::PlantData_["Onion", 1, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Onion", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Onion", 2, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Onion", 2, "nextStage"]				= "brickOnion3CropData";
$Farming::Crops::PlantData_["Onion", 2, "dryStage"]					= "";
$Farming::Crops::PlantData_["Onion", 2, "waterPerTick"]				= "5";
$Farming::Crops::PlantData_["Onion", 2, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Onion", 2, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Onion", 2, "yield"]					= "0 2";
$Farming::Crops::PlantData_["Onion", 2, "item"]						= "OnionItem";
$Farming::Crops::PlantData_["Onion", 2, "dieOnHarvest"]				= "1";
$Farming::Crops::PlantData_["Onion", 2, "harvestTool"]				= "Trowel";
$Farming::Crops::PlantData_["Onion", 2, "areaHarvestTool"]			= "Hoe";
$Farming::Crops::PlantData_["Onion", 2, "toolBuff"]					= "1 1";
$Farming::Crops::PlantData_["Onion", 2, "changeOnHarvest"]			= "";

//

$Farming::Crops::PlantData_["Onion", 3, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Onion", 3, "nextStage"]				= "";
$Farming::Crops::PlantData_["Onion", 3, "dryStage"]					= "";
$Farming::Crops::PlantData_["Onion", 3, "waterPerTick"]				= "1";
$Farming::Crops::PlantData_["Onion", 3, "numGrowTicks"]				= "3";
$Farming::Crops::PlantData_["Onion", 3, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Onion", 3, "yield"]					= "1 3";
$Farming::Crops::PlantData_["Onion", 3, "item"]						= "OnionItem";
$Farming::Crops::PlantData_["Onion", 3, "dieOnHarvest"]				= "1";
$Farming::Crops::PlantData_["Onion", 3, "harvestTool"]				= "Trowel";
$Farming::Crops::PlantData_["Onion", 3, "areaHarvestTool"]			= "Hoe";
$Farming::Crops::PlantData_["Onion", 3, "toolBuff"]					= "1 1";
$Farming::Crops::PlantData_["Onion", 3, "changeOnHarvest"]			= "";



//Blueberry//

$Farming::Crops::PlantData_["Blueberry", "plantSpace"]				= "2";
$Farming::Crops::PlantData_["Blueberry", "experienceRequired"]		= "100";
$Farming::Crops::PlantData_["Blueberry", "experienceCost"]			= "1";
$Farming::Crops::PlantData_["Blueberry", "harvestExperience"]		= "0";
$Farming::Crops::PlantData_["Blueberry", "plantingExperience"]		= "0";

$Farming::Crops::PlantData_["Blueberry", 0, "timePerTick"]			= "9000";
$Farming::Crops::PlantData_["Blueberry", 0, "nextStage"]			= "brickBlueberry1CropData";
$Farming::Crops::PlantData_["Blueberry", 0, "dryStage"]				= "";
$Farming::Crops::PlantData_["Blueberry", 0, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Blueberry", 0, "numGrowTicks"]			= "8";
$Farming::Crops::PlantData_["Blueberry", 0, "numDryTicks"]			= "-1";

//

$Farming::Crops::PlantData_["Blueberry", 1, "timePerTick"]			= "9000";
$Farming::Crops::PlantData_["Blueberry", 1, "nextStage"]			= "brickBlueberry2CropData";
$Farming::Crops::PlantData_["Blueberry", 1, "dryStage"]				= "";
$Farming::Crops::PlantData_["Blueberry", 1, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Blueberry", 1, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Blueberry", 1, "numDryTicks"]			= "-1";

//

$Farming::Crops::PlantData_["Blueberry", 2, "timePerTick"]			= "10000";
$Farming::Crops::PlantData_["Blueberry", 2, "nextStage"]			= "brickBlueberry3CropData";
$Farming::Crops::PlantData_["Blueberry", 2, "dryStage"]				= "";
$Farming::Crops::PlantData_["Blueberry", 2, "waterPerTick"]			= "15";
$Farming::Crops::PlantData_["Blueberry", 2, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Blueberry", 2, "numDryTicks"]			= "-1";

//

$Farming::Crops::PlantData_["Blueberry", 3, "timePerTick"]			= "10000";
$Farming::Crops::PlantData_["Blueberry", 3, "nextStage"]			= "brickBlueberry4CropData";
$Farming::Crops::PlantData_["Blueberry", 3, "dryStage"]				= "";
$Farming::Crops::PlantData_["Blueberry", 3, "waterPerTick"]			= "15";
$Farming::Crops::PlantData_["Blueberry", 3, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Blueberry", 3, "numDryTicks"]			= "-1";

//

$Farming::Crops::PlantData_["Blueberry", 4, "timePerTick"]			= "0";
$Farming::Crops::PlantData_["Blueberry", 4, "nextStage"]			= "";
$Farming::Crops::PlantData_["Blueberry", 4, "dryStage"]				= "";
$Farming::Crops::PlantData_["Blueberry", 4, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Blueberry", 4, "numGrowTicks"]			= "3";
$Farming::Crops::PlantData_["Blueberry", 4, "numDryTicks"]			= "-1";

$Farming::Crops::PlantData_["Blueberry", 4, "yield"]				= "3 6";
$Farming::Crops::PlantData_["Blueberry", 4, "item"]					= "BlueberryItem";
$Farming::Crops::PlantData_["Blueberry", 4, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Blueberry", 4, "harvestTool"]			= "Clipper";
$Farming::Crops::PlantData_["Blueberry", 4, "areaHarvestTool"]		= "Sickle";
$Farming::Crops::PlantData_["Blueberry", 4, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Blueberry", 4, "changeOnHarvest"]		= "";



//Turnip//

$Farming::Crops::PlantData_["Turnip", "plantSpace"]					= "2";
$Farming::Crops::PlantData_["Turnip", "experienceRequired"]			= "100";
$Farming::Crops::PlantData_["Turnip", "experienceCost"]				= "1";
$Farming::Crops::PlantData_["Turnip", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Turnip", "plantingExperience"]			= "0";

$Farming::Crops::PlantData_["Turnip", 0, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Turnip", 0, "nextStage"]				= "brickTurnip1CropData";
$Farming::Crops::PlantData_["Turnip", 0, "dryStage"]				= "";
$Farming::Crops::PlantData_["Turnip", 0, "waterPerTick"]			= "5";
$Farming::Crops::PlantData_["Turnip", 0, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Turnip", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Turnip", 1, "timePerTick"]				= "9000";
$Farming::Crops::PlantData_["Turnip", 1, "nextStage"]				= "brickTurnip2CropData";
$Farming::Crops::PlantData_["Turnip", 1, "dryStage"]				= "";
$Farming::Crops::PlantData_["Turnip", 1, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Turnip", 1, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Turnip", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Turnip", 2, "timePerTick"]				= "10000";
$Farming::Crops::PlantData_["Turnip", 2, "nextStage"]				= "brickTurnip3CropData";
$Farming::Crops::PlantData_["Turnip", 2, "dryStage"]				= "";
$Farming::Crops::PlantData_["Turnip", 2, "waterPerTick"]			= "15";
$Farming::Crops::PlantData_["Turnip", 2, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Turnip", 2, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Turnip", 3, "timePerTick"]				= "10000";
$Farming::Crops::PlantData_["Turnip", 3, "nextStage"]				= "brickTurnip4CropData";
$Farming::Crops::PlantData_["Turnip", 3, "dryStage"]				= "";
$Farming::Crops::PlantData_["Turnip", 3, "waterPerTick"]			= "20";
$Farming::Crops::PlantData_["Turnip", 3, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Turnip", 3, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Turnip", 4, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Turnip", 4, "nextStage"]				= "";
$Farming::Crops::PlantData_["Turnip", 4, "dryStage"]				= "";
$Farming::Crops::PlantData_["Turnip", 4, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Turnip", 4, "numGrowTicks"]			= "3";
$Farming::Crops::PlantData_["Turnip", 4, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Turnip", 4, "yield"]					= "1 1";
$Farming::Crops::PlantData_["Turnip", 4, "item"]					= "TurnipItem";
$Farming::Crops::PlantData_["Turnip", 4, "dieOnHarvest"]			= "1";
$Farming::Crops::PlantData_["Turnip", 4, "harvestTool"]				= "Trowel";
$Farming::Crops::PlantData_["Turnip", 4, "areaHarvestTool"]			= "Hoe";
$Farming::Crops::PlantData_["Turnip", 4, "toolBuff"]				= "0 1";
$Farming::Crops::PlantData_["Turnip", 4, "changeOnHarvest"]			= "";










//FLOWERs//


//Daisy//

$Farming::Crops::PlantData_["Daisy", "plantSpace"]					= "0";
$Farming::Crops::PlantData_["Daisy", "experienceRequired"]			= "100";
$Farming::Crops::PlantData_["Daisy", "experienceCost"]				= "1";
$Farming::Crops::PlantData_["Daisy", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Daisy", "plantingExperience"]			= "0";

$Farming::Crops::PlantData_["Daisy", 0, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Daisy", 0, "nextStage"]				= "brickDaisy1CropData";
$Farming::Crops::PlantData_["Daisy", 0, "dryStage"]					= "";
$Farming::Crops::PlantData_["Daisy", 0, "waterPerTick"]				= "2";
$Farming::Crops::PlantData_["Daisy", 0, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Daisy", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Daisy", 1, "timePerTick"]				= "9000";
$Farming::Crops::PlantData_["Daisy", 1, "nextStage"]				= "brickDaisy2CropData";
$Farming::Crops::PlantData_["Daisy", 1, "dryStage"]					= "";
$Farming::Crops::PlantData_["Daisy", 1, "waterPerTick"]				= "2";
$Farming::Crops::PlantData_["Daisy", 1, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Daisy", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Daisy", 2, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Daisy", 2, "nextStage"]				= "";
$Farming::Crops::PlantData_["Daisy", 2, "dryStage"]					= "";
$Farming::Crops::PlantData_["Daisy", 2, "waterPerTick"]				= "1";
$Farming::Crops::PlantData_["Daisy", 2, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Daisy", 2, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Daisy", 2, "yield"]					= "0 1";
$Farming::Crops::PlantData_["Daisy", 2, "item"]						= "DaisySeedItem";
$Farming::Crops::PlantData_["Daisy", 2, "dieOnHarvest"]				= "0";
$Farming::Crops::PlantData_["Daisy", 2, "changeOnHarvest"]			= "brickDaisy0CropData";


//Lily//

$Farming::Crops::PlantData_["Lily", "plantSpace"]					= "0";
$Farming::Crops::PlantData_["Lily", "experienceRequired"]			= "100";
$Farming::Crops::PlantData_["Lily", "experienceCost"]				= "1";
$Farming::Crops::PlantData_["Lily", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Lily", "plantingExperience"]			= "0";

$Farming::Crops::PlantData_["Lily", 0, "timePerTick"]				= "8000";
$Farming::Crops::PlantData_["Lily", 0, "nextStage"]					= "brickLily1CropData";
$Farming::Crops::PlantData_["Lily", 0, "dryStage"]					= "";
$Farming::Crops::PlantData_["Lily", 0, "waterPerTick"]				= "2";
$Farming::Crops::PlantData_["Lily", 0, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Lily", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Lily", 1, "timePerTick"]				= "9000";
$Farming::Crops::PlantData_["Lily", 1, "nextStage"]					= "brickLily2CropData";
$Farming::Crops::PlantData_["Lily", 1, "dryStage"]					= "";
$Farming::Crops::PlantData_["Lily", 1, "waterPerTick"]				= "2";
$Farming::Crops::PlantData_["Lily", 1, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Lily", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Lily", 2, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Lily", 2, "nextStage"]					= "";
$Farming::Crops::PlantData_["Lily", 2, "dryStage"]					= "";
$Farming::Crops::PlantData_["Lily", 2, "waterPerTick"]				= "1";
$Farming::Crops::PlantData_["Lily", 2, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Lily", 2, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Lily", 2, "yield"]						= "0 1";
$Farming::Crops::PlantData_["Lily", 2, "item"]						= "LilySeedItem";
$Farming::Crops::PlantData_["Lily", 2, "dieOnHarvest"]				= "0";
$Farming::Crops::PlantData_["Lily", 2, "changeOnHarvest"]			= "brickLily0CropData";













//TREES//


//Apple Tree//

$Farming::Crops::PlantData_["Apple", "plantSpace"]					= "7";
$Farming::Crops::PlantData_["Apple", "experienceRequired"]			= "1000";
$Farming::Crops::PlantData_["Apple", "experienceCost"]				= "1000";
$Farming::Crops::PlantData_["Apple", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Apple", "plantingExperience"]			= "0";

$Farming::Crops::PlantData_["Apple", 0, "timePerTick"]				= "20000";
$Farming::Crops::PlantData_["Apple", 0, "nextStage"]				= "brickAppleTree1CropData";
$Farming::Crops::PlantData_["Apple", 0, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 0, "waterPerTick"]				= "15";
$Farming::Crops::PlantData_["Apple", 0, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Apple", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 1, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Apple", 1, "nextStage"]				= "brickAppleTree2CropData";
$Farming::Crops::PlantData_["Apple", 1, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 1, "waterPerTick"]				= "15";
$Farming::Crops::PlantData_["Apple", 1, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Apple", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 2, "timePerTick"]				= "30000";
$Farming::Crops::PlantData_["Apple", 2, "nextStage"]				= "brickAppleTree3CropData";
$Farming::Crops::PlantData_["Apple", 2, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 2, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Apple", 2, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Apple", 2, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 3, "timePerTick"]				= "35000";
$Farming::Crops::PlantData_["Apple", 3, "nextStage"]				= "brickAppleTree4CropData";
$Farming::Crops::PlantData_["Apple", 3, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 3, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Apple", 3, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Apple", 3, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 4, "timePerTick"]				= "40000";
$Farming::Crops::PlantData_["Apple", 4, "nextStage"]				= "brickAppleTree5CropData";
$Farming::Crops::PlantData_["Apple", 4, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 4, "waterPerTick"]				= "25";
$Farming::Crops::PlantData_["Apple", 4, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Apple", 4, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 5, "timePerTick"]				= "40000";
$Farming::Crops::PlantData_["Apple", 5, "nextStage"]				= "brickAppleTree6CropData";
$Farming::Crops::PlantData_["Apple", 5, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 5, "waterPerTick"]				= "25";
$Farming::Crops::PlantData_["Apple", 5, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Apple", 5, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 6, "timePerTick"]				= "40000";
$Farming::Crops::PlantData_["Apple", 6, "nextStage"]				= "brickAppleTree7CropData";
$Farming::Crops::PlantData_["Apple", 6, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 6, "waterPerTick"]				= "25";
$Farming::Crops::PlantData_["Apple", 6, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Apple", 6, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 7, "timePerTick"]				= "10000";
$Farming::Crops::PlantData_["Apple", 7, "nextStage"]				= "brickAppleTree10CropData";
$Farming::Crops::PlantData_["Apple", 7, "dryStage"]					= "";
$Farming::Crops::PlantData_["Apple", 7, "waterPerTick"]				= "30";
$Farming::Crops::PlantData_["Apple", 7, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Apple", 7, "numDryTicks"]				= "-1";

// Unpruned

$Farming::Crops::PlantData_["Apple", 10, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Apple", 10, "nextStage"]				= "brickAppleTree11CropData";
$Farming::Crops::PlantData_["Apple", 10, "dryStage"]				= "";
$Farming::Crops::PlantData_["Apple", 10, "waterPerTick"]			= "5";
$Farming::Crops::PlantData_["Apple", 10, "numGrowTicks"]			= "30";
$Farming::Crops::PlantData_["Apple", 10, "numDryTicks"]				= "-1";
$Farming::Crops::PlantData_["Apple", 10, "pruneTool"]				= "Clipper";
$Farming::Crops::PlantData_["Apple", 10, "changeOnPrune"]			= "brickAppleTree20CropData";

//

$Farming::Crops::PlantData_["Apple", 11, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Apple", 11, "nextStage"]				= "brickAppleTree12CropData";
$Farming::Crops::PlantData_["Apple", 11, "dryStage"]				= "";
$Farming::Crops::PlantData_["Apple", 11, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Apple", 11, "numGrowTicks"]			= "15";
$Farming::Crops::PlantData_["Apple", 11, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 12, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Apple", 12, "nextStage"]				= "";
$Farming::Crops::PlantData_["Apple", 12, "dryStage"]				= "";
$Farming::Crops::PlantData_["Apple", 12, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Apple", 12, "numGrowTicks"]			= "15";
$Farming::Crops::PlantData_["Apple", 12, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Apple", 12, "yield"]					= "6 12";
$Farming::Crops::PlantData_["Apple", 12, "item"]					= "AppleItem";
$Farming::Crops::PlantData_["Apple", 12, "dieOnHarvest"]			= "0";
$Farming::Crops::PlantData_["Apple", 12, "harvestTool"]				= "Clipper";
$Farming::Crops::PlantData_["Apple", 12, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Apple", 12, "changeOnHarvest"]			= "brickAppleTree7CropData";

// Pruned

$Farming::Crops::PlantData_["Apple", 20, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Apple", 20, "nextStage"]				= "brickAppleTree21CropData";
$Farming::Crops::PlantData_["Apple", 20, "dryStage"]				= "";
$Farming::Crops::PlantData_["Apple", 20, "waterPerTick"]			= "5";
$Farming::Crops::PlantData_["Apple", 20, "numGrowTicks"]			= "30";
$Farming::Crops::PlantData_["Apple", 20, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 21, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Apple", 21, "nextStage"]				= "brickAppleTree22CropData";
$Farming::Crops::PlantData_["Apple", 21, "dryStage"]				= "";
$Farming::Crops::PlantData_["Apple", 21, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Apple", 21, "numGrowTicks"]			= "15";
$Farming::Crops::PlantData_["Apple", 21, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Apple", 22, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Apple", 22, "nextStage"]				= "";
$Farming::Crops::PlantData_["Apple", 22, "dryStage"]				= "";
$Farming::Crops::PlantData_["Apple", 22, "waterPerTick"]			= "10";
$Farming::Crops::PlantData_["Apple", 22, "numGrowTicks"]			= "15";
$Farming::Crops::PlantData_["Apple", 22, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Apple", 22, "yield"]					= "10 14";
$Farming::Crops::PlantData_["Apple", 22, "item"]					= "AppleItem";
$Farming::Crops::PlantData_["Apple", 22, "dieOnHarvest"]			= "0";
$Farming::Crops::PlantData_["Apple", 22, "harvestTool"]				= "Clipper";
$Farming::Crops::PlantData_["Apple", 22, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Apple", 22, "changeOnHarvest"]			= "brickAppleTree7CropData";




//Mango Tree//

$Farming::Crops::PlantData_["Mango", "plantSpace"]					= "15";
$Farming::Crops::PlantData_["Mango", "experienceRequired"]			= "1500";
$Farming::Crops::PlantData_["Mango", "experienceCost"]				= "1500";
$Farming::Crops::PlantData_["Mango", "harvestExperience"]			= "0";
$Farming::Crops::PlantData_["Mango", "plantingExperience"]			= "0";

$Farming::Crops::PlantData_["Mango", 0, "timePerTick"]				= "20000";
$Farming::Crops::PlantData_["Mango", 0, "nextStage"]				= "brickMangoTree1CropData";
$Farming::Crops::PlantData_["Mango", 0, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 0, "waterPerTick"]				= "45";
$Farming::Crops::PlantData_["Mango", 0, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 0, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 1, "timePerTick"]				= "20000";
$Farming::Crops::PlantData_["Mango", 1, "nextStage"]				= "brickMangoTree2CropData";
$Farming::Crops::PlantData_["Mango", 1, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 1, "waterPerTick"]				= "35";
$Farming::Crops::PlantData_["Mango", 1, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 1, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 2, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Mango", 2, "nextStage"]				= "brickMangoTree3CropData";
$Farming::Crops::PlantData_["Mango", 2, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 2, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Mango", 2, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 2, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 3, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Mango", 3, "nextStage"]				= "brickMangoTree4CropData";
$Farming::Crops::PlantData_["Mango", 3, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 3, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Mango", 3, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 3, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 4, "timePerTick"]				= "30000";
$Farming::Crops::PlantData_["Mango", 4, "nextStage"]				= "brickMangoTree5CropData";
$Farming::Crops::PlantData_["Mango", 4, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 4, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Mango", 4, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 4, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 5, "timePerTick"]				= "30000";
$Farming::Crops::PlantData_["Mango", 5, "nextStage"]				= "brickMangoTree6CropData";
$Farming::Crops::PlantData_["Mango", 5, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 5, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Mango", 5, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 5, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 6, "timePerTick"]				= "35000";
$Farming::Crops::PlantData_["Mango", 6, "nextStage"]				= "brickMangoTree7CropData";
$Farming::Crops::PlantData_["Mango", 6, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 6, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Mango", 6, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 6, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 7, "timePerTick"]				= "35000";
$Farming::Crops::PlantData_["Mango", 7, "nextStage"]				= "brickMangoTree8CropData";
$Farming::Crops::PlantData_["Mango", 7, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 7, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Mango", 7, "numGrowTicks"]				= "15";
$Farming::Crops::PlantData_["Mango", 7, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 8, "timePerTick"]				= "25000";
$Farming::Crops::PlantData_["Mango", 8, "nextStage"]				= "brickMangoTree9CropData";
$Farming::Crops::PlantData_["Mango", 8, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 8, "waterPerTick"]				= "10";
$Farming::Crops::PlantData_["Mango", 8, "numGrowTicks"]				= "20";
$Farming::Crops::PlantData_["Mango", 8, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 9, "timePerTick"]				= "35000";
$Farming::Crops::PlantData_["Mango", 9, "nextStage"]				= "brickMangoTree10CropData";
$Farming::Crops::PlantData_["Mango", 9, "dryStage"]					= "";
$Farming::Crops::PlantData_["Mango", 9, "waterPerTick"]				= "20";
$Farming::Crops::PlantData_["Mango", 9, "numGrowTicks"]				= "10";
$Farming::Crops::PlantData_["Mango", 9, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 10, "timePerTick"]				= "35000";
$Farming::Crops::PlantData_["Mango", 10, "nextStage"]				= "brickMangoTree11CropData";
$Farming::Crops::PlantData_["Mango", 10, "dryStage"]				= "";
$Farming::Crops::PlantData_["Mango", 10, "waterPerTick"]			= "20";
$Farming::Crops::PlantData_["Mango", 10, "numGrowTicks"]			= "10";
$Farming::Crops::PlantData_["Mango", 10, "numDryTicks"]				= "-1";

//

$Farming::Crops::PlantData_["Mango", 11, "timePerTick"]				= "0";
$Farming::Crops::PlantData_["Mango", 11, "nextStage"]				= "";
$Farming::Crops::PlantData_["Mango", 11, "dryStage"]				= "";
$Farming::Crops::PlantData_["Mango", 11, "waterPerTick"]			= "1";
$Farming::Crops::PlantData_["Mango", 11, "numGrowTicks"]			= "1";
$Farming::Crops::PlantData_["Mango", 11, "numDryTicks"]				= "-1";

$Farming::Crops::PlantData_["Mango", 11, "yield"]					= "12 18";
$Farming::Crops::PlantData_["Mango", 11, "item"]					= "MangoItem";
$Farming::Crops::PlantData_["Mango", 11, "dieOnHarvest"]			= "0";
$Farming::Crops::PlantData_["Mango", 11, "harvestTool"]				= "Clipper";
$Farming::Crops::PlantData_["Mango", 11, "toolBuff"]				= "1 1";
$Farming::Crops::PlantData_["Mango", 11, "changeOnHarvest"]			= "brickMangoTree8CropData";
