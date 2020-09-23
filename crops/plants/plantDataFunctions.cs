function getPlantData(%type, %stage, %value)
{
	if (%stage $= "")
	{
		%stage = 0;
	}
	else if (stripChars(%stage, "0123456789") !$= "")
	{
		%value = %stage;
		%stage = 0;
	}
	
	return $Farming::PlantData_[%type, %stage, %value];
}

function parseCSV(%filePath)
{
	deleteVariables("$Farming::PlantData_*");

	%file = new FileObject();
	%file.openForRead(%filePath);

	%line = %file.readLine();

	%line = strReplace(%line, ",", "\t");
	%line = strReplace(%line, "----", "");
	
	//store column titles, access only those values rather than iterate over table and look for them
	%idxCount = 0;
	for (%i = 1; %i < getFieldCount(%line); %i++) //first column is Stage - not a variable to store
	{
		%val = getField(%line, %i);
		if (%val !$= "")
		{
			%idxName[%idxCount] = %val;
			%idxIndex[%idxCount] = %i;
			%idxCount++;
		}
	}

	//iterate over table
	while (!%file.isEOF())
	{
		%line = strReplace(%file.readLine(), ",", "\t");
		%crop = getField(%line, 0);
		if (%crop $= "")
		{
			continue;
		}
		%type = getWord(%crop, 0);
		%stage = getWord(%crop, 1);
		if (%stage $= "")
		{
			%type = stripChars(%crop, "0123456789");
			%stage = stripChars(%crop, %type);
		}

		for (%i = 0; %i < %idxCount; %i++)
		{
			%val = getField(%line, %idxIndex[%i]);
			if (trim(%val) !$= "")
			{
				$Farming::PlantData_[%type, %stage, %idxName[%i]] = %val;
			}
		}
	}

	%file.close();
	%file.delete();
}

parseCSV("Add-ons/Server_Farming/config/plantData.csv");