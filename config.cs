$Game::Item::PopTime = 90000;

$startingAmount = 200;
$betaBonus = 100;

// Score grant tracking
if(isFile("config/Farming/scoreGrant.cs"))
	exec("config/Farming/scoreGrant.cs");

//Weed chance
$WeedBaseChance = 0.003;
$WeedFertModifier = 0.00005;
$WeedTickLength = 5;
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

// Vehicle Costs
JeepVehicle.maxWheelSpeed = 18;
JeepVehicle.uiName = "Jeep - $1000";
JeepVehicle.cost = 1000;
HorseArmor.cost = 250;
HorseArmor.maxForwardSpeed = 9;
HorseArmor.maxForwardCrouchSpeed = 9;
HorseArmor.uiName = "Horse - $250";

//Produce
$Produce::BuyCost_["Potato"] = 0.50;
$Produce::BuyCost_["Carrot"] = 1.30;
$Produce::BuyCost_["Tomato"] = 0.50;
$Produce::BuyCost_["Corn"] = 1.30;
$Produce::BuyCost_["Cabbage"] = 4.00;
$Produce::BuyCost_["Onion"] = 1.20;
$Produce::BuyCost_["Blueberry"] = 1.90;
$Produce::BuyCost_["Turnip"] = 8.00;
$Produce::BuyCost_["Apple"] = 5.00;
$Produce::BuyCost_["Mango"] = 12.00;

$Produce::BuyCost_["Chili"] = 2.50;
$Produce::BuyCost_["Cactus"] = 2.50;
$Produce::BuyCost_["Watermelon"] = 8.00;
$Produce::BuyCost_["Date"] = 18.00;
$Produce::BuyCost_["Peach"] = 12.40;

//Seeds
$Produce::BuyCost_["PotatoSeed"] = 1.00;
$Produce::BuyCost_["CarrotSeed"] = 1.00;
$Produce::BuyCost_["TomatoSeed"] = 6.00;
$Produce::BuyCost_["CornSeed"] = 1.50;
$Produce::BuyCost_["CabbageSeed"] = 1.00;
$Produce::BuyCost_["OnionSeed"] = 1.00;
$Produce::BuyCost_["BlueberrySeed"] = 2.00;
$Produce::BuyCost_["TurnipSeed"] = 2.00;
$Produce::BuyCost_["AppleSeed"] = 600.00;
$Produce::BuyCost_["MangoSeed"] = 800.00;

$Produce::BuyCost_["ChiliSeed"] = 4.00;
$Produce::BuyCost_["CactusSeed"] = 35.00;
$Produce::BuyCost_["WatermelonSeed"] = 45.00;
$Produce::BuyCost_["DateSeed"] = 700.00;
$Produce::BuyCost_["PeachSeed"] = 900.00;

$Produce::BuyCost_["DaisySeed"] = 1.00;
$Produce::BuyCost_["LilySeed"] = 1.00;
$Produce::BuyCost_["RoseSeed"] = 1.00;

//Extra stackable items

$Produce::BuyCost_["Fertilizer"] = 1.60;
$Produce::BuyCost_["WeedKiller"] = 5.00;


// Storage
$StorageMax_Tomato = 15 * 2;
$StorageMax_Potato = 15 * 2;
$StorageMax_Carrot = 15 * 3;
$StorageMax_Corn = 15 * 3;
$StorageMax_Cabbage = 15 * 3;
$StorageMax_Onion = 45 * 3;
$StorageMax_Blueberry = 42 * 2;
$StorageMax_Turnip = 4 * 2;
$StorageMax_Apple = 24 * 3;
$StorageMax_Mango = 10 * 3;

$StorageMax_Chili = 28 * 2;
$StorageMax_Cactus = 16 * 4;
$StorageMax_Watermelon = 1 * 4;
$StorageMax_Date = 9 * 4;
$StorageMax_Peach = 16 * 3;

$StorageMax_TomatoSeed = 24;
$StorageMax_PotatoSeed = 48;
$StorageMax_CarrotSeed = 24;
$StorageMax_CornSeed = 36;
$StorageMax_CabbageSeed = 36;
$StorageMax_OnionSeed = 72;
$StorageMax_BlueberrySeed = 48;
$StorageMax_TurnipSeed = 24;
$StorageMax_AppleSeed = 4;
$StorageMax_MangoSeed = 4;

$StorageMax_ChiliSeed = 16 * 3;
$StorageMax_CactusSeed = 8 * 2;
$StorageMax_WatermelonSeed = 8 * 2;
$StorageMax_DateSeed = 4;
$StorageMax_PeachSeed = 4;

$StorageMax_DaisySeed = 48;
$StorageMax_LilySeed = 48;
$StorageMax_RoseSeed = 48;

$StorageMax_Fertilizer = 240;
$StorageMax_SuperFertilizer = 60;

$StorageMax_WeedKiller = 120;

//Fertilizer production
$FertCount_Potato = "12 16";
$FertCount_Carrot = "16 22";
$FertCount_Tomato = "18 24";

$FertCount_Default = "10 16";
$FertTickAmt = 1;
