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

$Produce::BuyCost_["Potato"] = 0.50;
$Produce::BuyCost_["Carrot"] = 1.30;
$Produce::BuyCost_["Tomato"] = 0.50;
$Produce::BuyCost_["Corn"] = 1.30;
$Produce::BuyCost_["Cabbage"] = 4.00;
$Produce::BuyCost_["Onion"] = 1.20;
$Produce::BuyCost_["Blueberry"] = 1.90;
$Produce::BuyCost_["Turnip"] = 8.00;
$Produce::BuyCost_["Apple"] = 8.80;
$Produce::BuyCost_["Mango"] = 12.00;

$Produce::BuyCost_["Chili"] = 2.50;
$Produce::BuyCost_["Cactus"] = 2.50;
$Produce::BuyCost_["Watermelon"] = 8.00;
$Produce::BuyCost_["Date"] = 18.00;
$Produce::BuyCost_["Peach"] = 12.40;
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
$ProduceList_[$ProduceCount++ - 1]  = "Watermelon" TAB   0;
$ProduceList_[$ProduceCount++ - 1]  = "Date" TAB   0;
$ProduceList_[$ProduceCount++ - 1]  = "Peach" TAB   0;

//Seeds
$SeedCount = 0;
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
$Produce::BuyCost_["CactusSeed"] = 45.00;
$Produce::BuyCost_["WatermelonSeed"] = 60.00;
$Produce::BuyCost_["DateSeed"] = 700.00;
$Produce::BuyCost_["PeachSeed"] = 800.00;

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
$SeedList_[$SeedCount++ - 1]  = "DaisySeedItem" TAB   25;//5;
$SeedList_[$SeedCount++ - 1]  = "LilySeedItem" TAB    25;//5;
$SeedList_[$SeedCount++ - 1]  = "AppleSeedItem" TAB   8;
$SeedList_[$SeedCount++ - 1]  = "MangoSeedItem" TAB   4;
$SeedList_[$SeedCount++ - 1]  = "ChiliSeedItem" TAB   0;
$SeedList_[$SeedCount++ - 1]  = "CactusSeedItem" TAB   0;
$SeedList_[$SeedCount++ - 1]  = "WatermelonSeedItem" TAB   0;
$SeedList_[$SeedCount++ - 1]  = "DateSeedItem" TAB   0;
$SeedList_[$SeedCount++ - 1]  = "PeachSeedItem" TAB   0;

//Extra stackable items

$Produce::BuyCost_["Fertilizer"] = 1.60;

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

$StorageMax_Fertilizer = 240;

//Fertilizer production
$FertCount_Potato = "12 16";
$FertCount_Carrot = "16 22";
$FertCount_Tomato = "8 12";

$FertCount_Default = "10 16";
$FertTickAmt = 1;