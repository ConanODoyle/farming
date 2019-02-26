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

$ProduceList_[$ProduceCount++ - 1]	= "Potato" TAB		60;
$ProduceList_[$ProduceCount++ - 1]	= "Carrot" TAB		70;
$ProduceList_[$ProduceCount++ - 1]	= "Tomato" TAB		60;
$ProduceList_[$ProduceCount++ - 1]	= "Corn" TAB		30;
$ProduceList_[$ProduceCount++ - 1]	= "Cabbage" TAB		15;
$ProduceList_[$ProduceCount++ - 1]	= "Onion" TAB		40;
$ProduceList_[$ProduceCount++ - 1]	= "Blueberry" TAB	10;//10;
$ProduceList_[$ProduceCount++ - 1]	= "Turnip" TAB		6;//10;
$ProduceList_[$ProduceCount++ - 1]	= "Apple" TAB		6;
$ProduceList_[$ProduceCount++ - 1]	= "Mango" TAB		6;



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

$SeedList_[$SeedCount++ - 1] 	= "PotatoSeedItem" TAB		60;
$SeedList_[$SeedCount++ - 1] 	= "CarrotSeedItem" TAB		80;
$SeedList_[$SeedCount++ - 1] 	= "TomatoSeedItem" TAB		40;
$SeedList_[$SeedCount++ - 1] 	= "CornSeedItem" TAB		40;
$SeedList_[$SeedCount++ - 1] 	= "CabbageSeedItem" TAB		30;
$SeedList_[$SeedCount++ - 1] 	= "OnionSeedItem" TAB		60;
$SeedList_[$SeedCount++ - 1] 	= "BlueberrySeedItem" TAB	15;
$SeedList_[$SeedCount++ - 1] 	= "TurnipSeedItem" TAB		15;//5;
$SeedList_[$SeedCount++ - 1] 	= "DaisySeedItem" TAB		25;//5;
$SeedList_[$SeedCount++ - 1] 	= "LilySeedItem" TAB		25;//5;
$SeedList_[$SeedCount++ - 1] 	= "AppleSeedItem" TAB		10;
$SeedList_[$SeedCount++ - 1] 	= "MangoSeedItem" TAB		5;



//Extra stackable items

$Produce::BuyCost_["Fertilizer"] = 3.00;



//Normal item prices defined in their datablock
$ItemCount = 0;

$ItemList_[$ItemCount++ - 1] 	= "WateringCan2Item" TAB	150;
$ItemList_[$ItemCount++ - 1] 	= "WateringCan3Item" TAB	120;
$ItemList_[$ItemCount++ - 1]	= "HoseItem" TAB			50;
$ItemList_[$ItemCount++ - 1]	= "HoseV2Item" TAB			20;
$ItemList_[$ItemCount++ - 1]	= "TrowelItem" TAB			50;
$ItemList_[$ItemCount++ - 1]	= "ClipperItem" TAB			50;
$ItemList_[$ItemCount++ - 1]	= "HoeItem" TAB				35;
$ItemList_[$ItemCount++ - 1]	= "SickleItem" TAB			35;
$ItemList_[$ItemCount++ - 1]	= "PlanterItem" TAB			25;
$ItemList_[$ItemCount++ - 1]	= "PlanterV2Item" TAB		20;
$ItemList_[$ItemCount++ - 1]	= "ReclaimerItem" TAB		20;
$ItemList_[$ItemCount++ - 1]	= "WateringCatItem" TAB		5;



function getRandomSpecialItem()
{
	//calculate total weight
	%weight = 0;
	for (%i = 0; %i < $ItemCount; %i++)
	{
		%weight += getField($ItemList_[%i], 1);
	}

	%rand = getRandom();

	for (%i = 0; %i < $ItemCount; %i++)
	{
		%curr = $ItemList_[%i];
		%item = getField(%curr, 0);
		%total += getField(%curr, 1) / %weight;

		if (%rand < %total)
		{
			return %item;
		}
	}

	return 0;
}

function getRandomSeedType()
{
	%weight = 0;
	for (%i = 0; %i < $SeedCount; %i++)
	{
		%weight += getField($SeedList_[%i], 1);
	}

	%rand = getRandom();

	for (%i = 0; %i < $SeedCount; %i++)
	{
		%curr = $SeedList_[%i];
		%seed = getField(%curr, 0);
		%total += getField(%curr, 1) / %weight;

		if (%rand < %total)
		{
			return %seed;
		}
	}

	return 0;
}

function getRandomProduceType()
{
	%weight = 0;
	for (%i = 0; %i < $ProduceCount; %i++)
	{
		%weight += getField($ProduceList_[%i], 1);
	}

	%rand = getRandom();

	for (%i = 0; %i < $ProduceCount; %i++)
	{
		%curr = $ProduceList_[%i];
		%type = getField(%curr, 0);
		%total += getField(%curr, 1) / %weight;

		if (%rand < %total)
		{
			return %type;
		}
	}

	return 0;
}

function generateInstrumentList()
{
	$InstrumentCount = 0;
	deleteVariables("$InstrumentList_*");

	for (%i = 0; %i < DatablockGroup.getCount(); %i++)
	{
		%db = DatablockGroup.getObject(%i);
		if (isObject(%db.image) && %db.image.instrumentType !$= "")
		{
			$InstrumentList_[$InstrumentCount + 0] = %db;
			$InstrumentCount++;

			if (%db.uiName $= "Keytar")
			{
				%keytarFound = 1;
				%keytarIDX = $InstrumentCount - 1;
			}
		}
	}


	%prob = 1 / $InstrumentCount;

	%nonKeytarProb = (2 * $InstrumentCount) / (2 * $InstrumentCount - 1);
	%keytarProb = %nonKeytarProb / 2;
	if (%keytarFound)
	{
		for (%i = 0; %i < $InstrumentCount; %i++)
		{
			if (%i == %keytarIDX)
			{
				$InstrumentList_[%i] = $InstrumentList_[%i] TAB %keytarProb;
			}
			else
			{
				$InstrumentList_[%i] = $InstrumentList_[%i] TAB %nonKeytarProb;
			}
		}
	}
	else
	{
		for (%i = 0; %i < $InstrumentCount; %i++)
		{
			$InstrumentList_[%i] = $InstrumentList_[%i] TAB %prob;
		}
	}

	echo("Generated Instrument list: Found " @ $InstrumentCount @ " instruments");
	if (%keytarFound)
	{
		echo("    Keytar found");
	}
}

function getRandomInstrument() 
{
	%rand = getRandom();

	for (%i = 0; %i < $InstrumentCount; %i++)
	{
		%curr = $InstrumentList_[%i];
		%item = getField(%curr, 0);
		%total += getField(%curr, 1);

		if (%rand < %total)
		{
			return %item;
		}
	}

	return 0;
}

schedule(1000, 0, generateInstrumentList);