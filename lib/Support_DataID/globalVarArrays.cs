//utility functions
function getSafeDataIDArrayName(%aid)
{
	if ($DataIDDebug) talk("getSafeDataIDArrayName");
	%aid = strReplace(%aid, " ", "_");
	return stripChars(%aid, "!@#$%^&*()-[]{},.<>;':\"");
}

function loadDataIDArray(%aid, %force)
{
	if (!$executedDataID[%aid] || %force)
	{
		if ($DataIDDebug) talk("loadDataIDArray");
		deleteVariables("$DataID_" @ %aid @ "*");
		if (isFile("config/server/DataIDs/" @ %aid @ ".cs"))
		{
			exec("config/server/DataIDs/" @ %aid @ ".cs");
		}
		else if (%force)
		{
			echo("No dataID file found for " @ %aid @ "!");
		}
	}
	$executedDataID[%aid] = 1;
}

function saveDataIDArray(%aid, %force)
{
	if ($DataIDDebug) talk("saveDataIDArray");
	getSafeDataIDArrayName(%aid);
	export("$DataID_" @ %aid @ "*", "config/server/DataIDs/" @ %aid @ ".cs");
}

function deleteDataIDArray(%aid)
{
	if ($DataIDDebug) talk("deleteDataIDArray");
	deleteVariables("$DataID_" @ %aid @ "*");
}

function printDataIDArray(%aid, %skipLoad)
{
	if ($DataIDDebug) talk("printDataIDArray");
	if (!%skipLoad)
	{
		loadDataIDArray(%aid);
		echo("Loaded [" @ %aid @ "] array");
	}
	echo("DataID Array [" @ %aid @ "]");

	%count = getDataIDArrayCount(%aid);
	echo("Count: " @ %count);
	for (%i = 0; %i < %count; %i++)
	{
		echo(%i @ ": " @$DataID_[%aid, %i]);
	}
}



//reads
function getDataIDArrayValue(%aid, %slot)
{
	if ($DataIDDebug) talk("getDataIDArrayValue");
	loadDataIDArray(%aid);

	%count = getDataIDArrayCount(%aid);
	if (%slot >= %count)
	{
		return "";
	}
	else
	{
		return $DataID_[%aid, %slot];
	}
}

function getDataIDArrayCount(%aid)
{
	if ($DataIDDebug) talk("getDataIDArrayCount");
	loadDataIDArray(%aid);

	$DataID_[%aid, "count"] += 0; //ensure its an integer rather than empty string

	return $DataID_[%aid, "count"];
}

function indexOfDataIDArray(%aid, %value, %startIndex)
{
	if ($DataIDDebug) talk("indexOfDataIDArray");
	loadDataIDArray(%aid);

	%count = getDataIDArrayCount(%aid);
	%startIndex = %startIndex + 0;
	for (%i = %startIndex; %i < %count; %i++)
	{
		if ($DataID_[%aid, %i] $= %value)
		{
			return %i;
		}
	}
	return -1;
}




//writes
//resize %aid to size %count
function setDataIDArrayCount(%aid, %count)
{
	if ($DataIDDebug) talk("setDataIDArrayCount");
	loadDataIDArray(%aid);

	if (%count == $DataID_[%aid, "count"])
	{
		return;
	}
	else if (%count > $DataID_[%aid, "count"])
	{
		//fill with empty strings
		for (%i = $DataID_[%aid, "count"]; %i < %count; %i++)
		{
			$DataID_[%aid, "count"] = "";
		}
	}
	else
	{
		//delete values
		for (%i = $DataID_[%aid, "count"]; %i > %count; %i--)
		{
			deleteVariables("$DataID_" @ %aid @ "_" @ %i);
		}
	}
	$DataID_[%aid, "count"] = %count + 0;

	saveDataIDArray(%aid);
}


//set %aid[%slot] to %value
//%slot clamped to %count (cannot insert past the end of a list)
function setDataIDArrayValue(%aid, %slot, %value)
{
	if ($DataIDDebug) talk("setDataIDArrayValue");
	loadDataIDArray(%aid);

	%slot = getMax(%slot + 0, 0); //ensure it's not empty string
	%count = getDataIDArrayCount(%aid);
	if (%slot >= %count)
	{
		setDataIDArrayCount(%aid, %slot + 1);
	}

	$DataID_[%aid, %slot] = %value;

	saveDataIDArray(%aid);
	return 0;
}

//add %value to first available slot in %aid. %start optional
function addToDataIDArray(%aid, %value, %start)
{
	if ($DataIDDebug) talk("addToDataIDArray");
	loadDataIDArray(%aid);

	%start = getMax(%start + 0, 0);
	%count = getDataIDArrayCount(%aid);
	
	//place in first available slot
	for (%i = %start; %i < %count; %i++)
	{
		if ($DataID_[%aid, %i] $= "")
		{
			$DataID_[%aid, %i] = %value;
			%returnIDX = %i;
			break;
		}
	}

	//if no slots available, return -1
	if (%returnIDX $= "")
	{
		return -1;
	}

	saveDataIDArray(%aid);
	return %returnIDX;
}

//removes value at %slot
function removeDataIDArrayValue(%aid, %slot)
{
	if ($DataIDDebug) talk("removeDataIDArrayValue");
	loadDataIDArray(%aid);

	%count = getDataIDArrayCount(%aid);
	$DataID_[%aid, %slot] = "";

	saveDataIDArray(%aid);
}

function clearDataIDArray(%aid)
{
	if ($DataIDDebug) talk("clearDataIDArray");
	deleteVariables("$DataID_" @ %aid @ "*");

	if (isFile("config/server/DataIDs/" @ %aid @ ".cs"))
	{
		fileDelete("config/server/DataIDs/" @ %aid @ ".cs");
	}
}