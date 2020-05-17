//utility functions
function getSafeDataIDArrayName(%aid)
{
	%aid = strReplace(%aid, " ", "_");
	return stripChars(%aid, "!@#$%^&*()-[]{},.<>;':\"");
}

function loadDataIDArray(%aid, %force)
{
	if (!$executedDataID[%aid] || %force)
	{
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
	getSafeDataIDArrayName(%aid);
	export("$DataID_" @ %aid @ "*", "config/server/DataIDs/" @ %aid @ ".cs");
}

function printDataIDArray(%aid, %skipLoad)
{
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
	loadDataIDArray(%aid);

	$DataID_[%aid, "count"] += 0; //ensure its an integer rather than empty string

	return $DataID_[%aid, %slot];
}

function indexOfDataIDArray(%aid, %value, %startIndex)
{
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
//only call this internally, does *not* save since it assumes any functions calling it will save
function _setDataIDArrayCount(%aid, %count)
{
	if (%count == $DataID_[%aid, "count"])
	{
		return;
	}

	loadDataIDArray(%aid);

	$DataID_[%aid, "count"] = %count;
}

function setDataIDArrayValue(%aid, %slot, %value)
{
	loadDataIDArray(%aid);

	%count = getDataIDArrayCount(%aid);
	if (%slot >= %count)
	{
		//clear any persisting values in case of %aid reuse
		while (%count <= %slot)
		{
			$DataID_[%aid, %count] = "";
			%count++;
		}
	}

	$DataID_[%aid, %slot] = %value;
	_setDataIDArrayCount(%aid, %count);

	saveDataIDArray(%aid);
}

function addToDataIDArray(%aid, %value, %slot)
{
	loadDataIDArray(%aid);

	if (%slot $= "")
	{
		//place in first available slot
		%count = getDataIDArrayCount(%aid);
		for (%i = 0; %i < %count; %i++)
		{
			if ($DataID_[%aid, %i] $= "")
			{
				$DataID_[%aid, %i] = %value;
				%returnIDX = %i;
				break;
			}
		}

		if (%returnIDX $= "")
		{
			$DataID_[%aid, %count] = %value;
			%returnIDX = %count;
			%count = %count + 1;
			_setDataIDArrayCount(%aid, %count);
		}
	}
	else
	{
		$DataID_[%aid, %slot] = %value;
		if (%slot >= getDataIDArrayCount(%aid))
		{
			_setDataIDArrayCount(%aid, %slot + 1);
		}
		%returnIDX = %slot;
	}

	saveDataIDArray(%aid);
	return %returnIDX;
}

function removeDataIDArrayValue(%aid, %slot)
{
	loadDataIDArray(%aid);

	%count = getDataIDArrayCount(%aid);
	$DataID_[%aid, %slot] = "";
	if (%slot == %count - 1)
	{
		_setDataIDArrayCount(%aid, %count - 1);
	}

	saveDataIDArray(%aid);
}

function clearDataIDArray(%aid)
{
	deleteVariables("$DataID_" @ %aid @ "*");

	saveDataIDArray(%aid);
}