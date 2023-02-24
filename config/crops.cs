	
//Weed chance
$WeedBaseChance = 0.001;
$WeedFertModifier = 0.00005;
$WeedSpawnTickTime = 5;
$WeedSearchRadius = 1;
$WeedWeatherFactor = 10;

// Dirt colors
$DirtWaterColor0 = 54;
$DirtWaterColor1 = 55;
$DirtWaterColor2 = 56;
$DirtWaterColor3 = 57;
$DirtWaterColor4 = 58;
$DirtWaterColor5 = 59;
$DirtWaterColor6 = 60;
$DirtWaterColorCount = 7;

//Fertilizer production
$FertCount_Potato = "12 16";
$FertCount_Carrot = "16 22";
$FertCount_Tomato = "18 24";
$FertCount_Weed = "1 3";

$FertCount_Default = "10 16";
$FertTickAmt = 1;

//List of produce
$ProduceCheck_["Potato"] 		= 1;
$ProduceCheck_["Carrot"] 		= 1;
$ProduceCheck_["Tomato"] 		= 1;
$ProduceCheck_["Corn"] 			= 1;
$ProduceCheck_["Cabbage"] 		= 1;
$ProduceCheck_["Onion"] 		= 1;
$ProduceCheck_["Blueberry"] 	= 1;
$ProduceCheck_["Turnip"] 		= 1;
$ProduceCheck_["Apple"] 		= 1;
$ProduceCheck_["Mango"] 		= 1;
$ProduceCheck_["Chili"] 		= 1;
$ProduceCheck_["Cactus"] 		= 1;
$ProduceCheck_["Watermelon"]	= 1;
$ProduceCheck_["Date"] 			= 1;
$ProduceCheck_["Peach"] 		= 1;
$ProduceCheck_["Weed"] 			= 1;
$ProduceCheck_["Wheat"] 		= 1;
$ProduceCheck_["Portobello"] 	= 1;

$UndergroundCropsList = "Potato\tCarrot\tOnion\tTurnip\tPortobello";
$AbovegroundCropsList = "Tomato\tCorn\tWheat\tCabbage\tBlueberry\tChili\tWatermelon";
$TreeCropsList = "Apple\tMango\tCactus\tPeach\tDate";
$ProduceList = $UndergroundCropsList TAB $AbovegroundCropsList TAB $TreeCropsList TAB "Rose\tLily\tDaisy\tLilies\tDaisies";
$SellProduceList = $UndergroundCropsList TAB $AbovegroundCropsList TAB $TreeCropsList;
//$SellFishList auto generated in scripts/fishing/fish/datablocks.cs

function getCropClass(%cropName)
{
	//special cases
	if (%cropName $= "Weed")
	{
		return "Underground";
	}
	
	%under = "\t" @ $UndergroundCropsList @ "\t";
	%over = "\t" @ $AbovegroundCropsList @ "\t";
	%tree = "\t" @ $TreeCropsList @ "\t";
	%cropName = "\t" @ trim(%cropName) @ "\t";

	if (stripos(%under, %cropName) >= 0)
	{
		return "Underground";
	}
	if (stripos(%over, %cropName) >= 0)
	{
		return "Aboveground";
	}
	if (stripos(%tree, %cropName) >= 0)
	{
		return "Tree";
	}
	return "";
}