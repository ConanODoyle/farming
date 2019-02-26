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