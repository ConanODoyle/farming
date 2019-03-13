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
HorseArmor.maxForwardSpeed = 9;
HorseArmor.maxForwardCrouchSpeed = 9;
HorseArmor.uiName = "Horse - $250";

//Produce
$ProduceCount = 0;

$Produce::BuyCost_["Potato"] = 0.75;
$Produce::BuyCost_["Carrot"] = 1.75;
$Produce::BuyCost_["Tomato"] = 1.00;
$Produce::BuyCost_["Corn"] = 1.75;
$Produce::BuyCost_["Cabbage"] = 4.00;
$Produce::BuyCost_["Onion"] = 1.25;
$Produce::BuyCost_["Blueberry"] = 2.50;
$Produce::BuyCost_["Turnip"] = 10.00;
$Produce::BuyCost_["Apple"] = 8.00;
$Produce::BuyCost_["Mango"] = 12.00;

$Produce::BuyCost_["Chili"] = 2.50;
$Produce::BuyCost_["Cactus"] = 4.00;
//big buyer
$ProduceList_[$ProduceCount++ - 1]  = "Potato" TAB    60;
$ProduceList_[$ProduceCount++ - 1]  = "Carrot" TAB    70;
$ProduceList_[$ProduceCount++ - 1]  = "Tomato" TAB    60;
$ProduceList_[$ProduceCount++ - 1]  = "Corn" TAB    40;
$ProduceList_[$ProduceCount++ - 1]  = "Cabbage" TAB   15;
$ProduceList_[$ProduceCount++ - 1]  = "Onion" TAB   50;
$ProduceList_[$ProduceCount++ - 1]  = "Blueberry" TAB 8;//10;
$ProduceList_[$ProduceCount++ - 1]  = "Turnip" TAB    8;//10;
$ProduceList_[$ProduceCount++ - 1]  = "Apple" TAB   8;
$ProduceList_[$ProduceCount++ - 1]  = "Mango" TAB   4;
$ProduceList_[$ProduceCount++ - 1]  = "Chili" TAB   0;
$ProduceList_[$ProduceCount++ - 1]  = "Cactus" TAB   0;

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
$Produce::BuyCost_["AppleSeed"] = 600.00;
$Produce::BuyCost_["MangoSeed"] = 800.00;

$Produce::BuyCost_["ChiliSeed"] = 4.00;
$Produce::BuyCost_["CactusSeed"] = 75.00;

$Produce::BuyCost_["DaisySeed"] = 1.00;
$Produce::BuyCost_["LilySeed"] = 1.00;

$SeedList_[$SeedCount++ - 1]  = "PotatoSeedItem" TAB    60;
$SeedList_[$SeedCount++ - 1]  = "CarrotSeedItem" TAB    60;
$SeedList_[$SeedCount++ - 1]  = "TomatoSeedItem" TAB    40;
$SeedList_[$SeedCount++ - 1]  = "CornSeedItem" TAB    40;
$SeedList_[$SeedCount++ - 1]  = "CabbageSeedItem" TAB   30;
$SeedList_[$SeedCount++ - 1]  = "OnionSeedItem" TAB   60;
$SeedList_[$SeedCount++ - 1]  = "BlueberrySeedItem" TAB 10;
$SeedList_[$SeedCount++ - 1]  = "TurnipSeedItem" TAB    10;//5;
$SeedList_[$SeedCount++ - 1]  = "DaisySeedItem" TAB   250;//5;
$SeedList_[$SeedCount++ - 1]  = "LilySeedItem" TAB    25;//5;
$SeedList_[$SeedCount++ - 1]  = "AppleSeedItem" TAB   8;
$SeedList_[$SeedCount++ - 1]  = "MangoSeedItem" TAB   4;
$SeedList_[$SeedCount++ - 1]  = "ChiliSeedItem" TAB   0;
$SeedList_[$SeedCount++ - 1]  = "CactusSeedItem" TAB   0;

//Extra stackable items

$Produce::BuyCost_["Fertilizer"] = 2.00;

//Normal item prices defined in their datablock
$ItemCount = 0;

$ItemList_[$ItemCount++ - 1]  = "WateringCan2Item" TAB  120;
$ItemList_[$ItemCount++ - 1]  = "WateringCan3Item" TAB  120;
$ItemList_[$ItemCount++ - 1]  = "HoseItem" TAB      50;
$ItemList_[$ItemCount++ - 1]  = "HoseV2Item" TAB      20;
$ItemList_[$ItemCount++ - 1]  = "TrowelItem" TAB      50;
$ItemList_[$ItemCount++ - 1]  = "ClipperItem" TAB     50;
$ItemList_[$ItemCount++ - 1]  = "HoeItem" TAB       35;
$ItemList_[$ItemCount++ - 1]  = "SickleItem" TAB      35;
$ItemList_[$ItemCount++ - 1]  = "PlanterItem" TAB     25;
$ItemList_[$ItemCount++ - 1]  = "PlanterV2Item" TAB   15;
$ItemList_[$ItemCount++ - 1]  = "ReclaimerItem" TAB   20;
$ItemList_[$ItemCount++ - 1]  = "FlowerPotItem" TAB   20;
$ItemList_[$ItemCount++ - 1]  = "CompostBinItem" TAB   20;
$ItemList_[$ItemCount++ - 1]  = "WateringCatItem" TAB   5;

// Storage
$StorageMax_Tomato = 30;
$StorageMax_Potato = 30;
$StorageMax_Carrot = 45;
$StorageMax_Corn = 45;
$StorageMax_Cabbage = 45;
$StorageMax_Onion = 135;
$StorageMax_Blueberry = 84;
$StorageMax_Turnip = 8;
$StorageMax_Apple = 72;
$StorageMax_Mango = 30;

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

$StorageMax_DaisySeed = 48;
$StorageMax_LilySeed = 48;

$StorageMax_Fertilizer = 120;
