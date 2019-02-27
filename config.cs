$Game::Item::PopTime = 90000;

$startingAmount = 90;
$betaBonus = 200;

// Score grant tracking
if(isFile("config/Farming/scoreGrant.cs"))
  exec("config/Farming/scoreGrant.cs");

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
HorseArmor.uiName = "Horse - $250";

//Produce
$ProduceCount = 0;

$Produce::BuyCost_["Potato"] = 1.00;
$Produce::BuyCost_["Carrot"] = 2.00;
$Produce::BuyCost_["Tomato"] = 1.00;
$Produce::BuyCost_["Corn"] = 2.00;
$Produce::BuyCost_["Cabbage"] = 3.50;
$Produce::BuyCost_["Onion"] = 1.50;
$Produce::BuyCost_["Blueberry"] = 2.00;
$Produce::BuyCost_["Turnip"] = 10.00;
$Produce::BuyCost_["Apple"] = 12.00;
$Produce::BuyCost_["Mango"] = 16.00;

$ProduceList_[$ProduceCount++ - 1]  = "Potato" TAB    60;
$ProduceList_[$ProduceCount++ - 1]  = "Carrot" TAB    70;
$ProduceList_[$ProduceCount++ - 1]  = "Tomato" TAB    60;
$ProduceList_[$ProduceCount++ - 1]  = "Corn" TAB    30;
$ProduceList_[$ProduceCount++ - 1]  = "Cabbage" TAB   15;
$ProduceList_[$ProduceCount++ - 1]  = "Onion" TAB   40;
$ProduceList_[$ProduceCount++ - 1]  = "Blueberry" TAB 10;//10;
$ProduceList_[$ProduceCount++ - 1]  = "Turnip" TAB    6;//10;
$ProduceList_[$ProduceCount++ - 1]  = "Apple" TAB   6;
$ProduceList_[$ProduceCount++ - 1]  = "Mango" TAB   6;

//Seeds
$SeedCount = 0;
$Produce::BuyCost_["PotatoSeed"] = 1.00;
$Produce::BuyCost_["CarrotSeed"] = 1.00;
$Produce::BuyCost_["TomatoSeed"] = 10.00;
$Produce::BuyCost_["CornSeed"] = 1.50;
$Produce::BuyCost_["CabbageSeed"] = 1.00;
$Produce::BuyCost_["OnionSeed"] = 1.00;
$Produce::BuyCost_["BlueberrySeed"] = 2.00;
$Produce::BuyCost_["TurnipSeed"] = 2.00;
$Produce::BuyCost_["AppleSeed"] = 1200.00;
$Produce::BuyCost_["MangoSeed"] = 1600.00;

$Produce::BuyCost_["DaisySeed"] = 1.00;
$Produce::BuyCost_["LilySeed"] = 1.00;

$SeedList_[$SeedCount++ - 1]  = "PotatoSeedItem" TAB    60;
$SeedList_[$SeedCount++ - 1]  = "CarrotSeedItem" TAB    80;
$SeedList_[$SeedCount++ - 1]  = "TomatoSeedItem" TAB    40;
$SeedList_[$SeedCount++ - 1]  = "CornSeedItem" TAB    40;
$SeedList_[$SeedCount++ - 1]  = "CabbageSeedItem" TAB   30;
$SeedList_[$SeedCount++ - 1]  = "OnionSeedItem" TAB   60;
$SeedList_[$SeedCount++ - 1]  = "BlueberrySeedItem" TAB 15;
$SeedList_[$SeedCount++ - 1]  = "TurnipSeedItem" TAB    15;//5;
$SeedList_[$SeedCount++ - 1]  = "DaisySeedItem" TAB   25;//5;
$SeedList_[$SeedCount++ - 1]  = "LilySeedItem" TAB    25;//5;
$SeedList_[$SeedCount++ - 1]  = "AppleSeedItem" TAB   10;
$SeedList_[$SeedCount++ - 1]  = "MangoSeedItem" TAB   5;

//Extra stackable items

$Produce::BuyCost_["Fertilizer"] = 3.00;

//Normal item prices defined in their datablock
$ItemCount = 0;

$ItemList_[$ItemCount++ - 1]  = "WateringCan2Item" TAB  150;
$ItemList_[$ItemCount++ - 1]  = "WateringCan3Item" TAB  120;
$ItemList_[$ItemCount++ - 1]  = "HoseItem" TAB      50;
$ItemList_[$ItemCount++ - 1]  = "HoseV2Item" TAB      20;
$ItemList_[$ItemCount++ - 1]  = "TrowelItem" TAB      50;
$ItemList_[$ItemCount++ - 1]  = "ClipperItem" TAB     50;
$ItemList_[$ItemCount++ - 1]  = "HoeItem" TAB       35;
$ItemList_[$ItemCount++ - 1]  = "SickleItem" TAB      35;
$ItemList_[$ItemCount++ - 1]  = "PlanterItem" TAB     25;
$ItemList_[$ItemCount++ - 1]  = "PlanterV2Item" TAB   20;
$ItemList_[$ItemCount++ - 1]  = "ReclaimerItem" TAB   20;
$ItemList_[$ItemCount++ - 1]  = "WateringCatItem" TAB   5;

// Storage
$StorageMax_Tomato = 30;
$StorageMax_Potato = 30;
$StorageMax_Carrot = 45;
$StorageMax_Corn = 45;
$StorageMax_Cabbage = 45;
$StorageMax_Onion = 90;
$StorageMax_Blueberry = 84;
$StorageMax_Turnip = 8;
$StorageMax_Apple = 72;
$StorageMax_Mango = 30;

$StorageMax_TomatoSeed = 24;
$StorageMax_PotatoSeed = 60;
$StorageMax_CarrotSeed = 24;
$StorageMax_CornSeed = 36;
$StorageMax_CabbageSeed = 36;
$StorageMax_OnionSeed = 48;
$StorageMax_BlueberrySeed = 48;
$StorageMax_TurnipSeed = 24;
$StorageMax_AppleSeed = 4;
$StorageMax_MangoSeed = 4;

$StorageMax_DaisySeed = 48;
$StorageMax_LilySeed = 48;

$StorageMax_Fertilizer = 120;
