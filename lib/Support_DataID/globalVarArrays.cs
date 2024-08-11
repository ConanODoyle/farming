if (!isObject(LoadedDataIDs))
{
	new ScriptObject(LoadedDataIDs)
	{
		numActive = 0; //number of active data IDs
		listSize = 0; //list containing loaded data id's
		isLoaded = ""; //quick reference check if a data id is already loaded
		lastTouched = 0; //checks when the dataID was last touched
	};
}

function pushDataID(%dataID)
{
	if (%dataID $= "")
	{
		return;
	}

	LoadedDataIDs.lastTouched[%dataID] = getSimTime();
	if (LoadedDataIDs.isLoaded[%dataID])
	{
		return;
	}

	LoadedDataIDs.isLoaded[%dataID] = 1;
	LoadedDataIDs.numActive++;
	for (%i = 0; %i < LoadedDataIDs.listSize; %i++)
	{
		if (LoadedDataIDs.list[%i] $= "")
		{
			%foundSlot = 1;
			LoadedDataIDs.list[%i] = %dataID;
			break;
		}
	}

	if (!%foundSlot)
	{
		LoadedDataIDs.list[LoadedDataIDs.listSize] = %dataID;
		LoadedDataIDs.listSize++;
	}
}

function popDataID(%dataID)
{
	if (%dataID $= "")
	{
		return;
	}

	if (!LoadedDataIDs.isLoaded[%dataID])
	{
		return;
	}

	LoadedDataIDs.isLoaded[%dataID] = "";
	LoadedDataIDs.lastTouched[%dataID] = "";
	LoadedDataIDs.numActive--;
	for (%i = 0; %i < LoadedDataIDs.listSize; %i++)
	{
		// echo(" Comparing " @ LoadedDataIDs.list[%i] @ " to " @ %dataID);
		if (LoadedDataIDs.list[%i] $= %dataID)
		{
			%foundSlot = %i;
			LoadedDataIDs.list[%i] = "";
			break;
		}
	}

	if (%foundSlot $= "")
	{
		echo("ERROR: tried to pop dataID not loaded! dataID: " @ %dataID @ " size: " @ LoadedDataIDs.listSize);
	}

	if (%foundSlot == LoadedDataIDs.listSize - 1) //reduce list size if we happen to remove the last one in the list
	{
		LoadedDataIDs.listSize--;
	}
}

function getOldestDataID()
{
	for (%i = 0; %i < LoadedDataIDs.listSize; %i++)
	{
		%dataID = LoadedDataIDs.list[%i];
		if (%dataID !$= "" && (LoadedDataIDs.lastTouched[%dataID] < %oldestTime || %oldest $= ""))
		{
			%oldest = %dataID;
			%oldestTime = LoadedDataIDs.lastTouched[%dataID];
		}
	}
	return %oldest;
}

function isDataIDLoaded(%dataID)
{
	return LoadedDataIDs.isLoaded[%dataID];
}

function getLoadedDataIDCount(%dataID)
{
	return LoadedDataIDs.numActive;
}

function queueSaveDataIDArray(%aid)
{
	if (!isEventPending($scheduleSave[%aid]))
	{
		$scheduleSave[%aid] = schedule(16000, 0, popSaveDataIDArray, %aid);
	}
}

function popSaveDataIDArray(%aid)
{
	cancel($scheduleSave[%aid]);
	saveDataIDArray(%aid);
	deleteVariables("$scheduleSave" @ %aid);
}




//utility functions
function getSafeDataIDArrayName(%aid)
{
	if ($DataIDDebug) talk("getSafeDataIDArrayName");
	%aid = trim(%aid);
	%aid = strReplace(%aid, " ", "_");
	%aid = strReplace(%aid, "\t", "__");
	return stripChars(%aid, "!@#$%^&*()-+[]{},.<>;':\"\n");
}

function loadDataIDArray(%aid, %force)
{
	if (%aid $= "")
	{
		return %aid;
	}

	pruneDataIDArrays();

	%aid = getSafeDataIDArrayName(%aid);
	if (!isDataIDLoaded(%aid) || %force)
	{
		if ($DataIDDebug) talk("loadDataIDArray");
		if ($DataIDEcho) echo("DataID: Loading " @ %aid SPC getDateTime());
		deleteVariables("$DataID_" @ %aid @ "_*");
		deleteVariables("$executedDataID" @ %aid);
		if (isFile("config/server/DataIDs/" @ %aid @ ".cs"))
		{
			exec("config/server/DataIDs/" @ %aid @ ".cs");
		}
		else if (%force)
		{
			echo("No dataID file found for " @ %aid @ "!");
		}
	}
	pushDataID(%aid);

	return %aid;
}

function saveDataIDArray(%aid, %force)
{
	if ($DataIDDebug) talk("saveDataIDArray");
	%aid = getSafeDataIDArrayName(%aid);
	if ($DataIDEcho) echo("DataID: Saving " @ %aid SPC getDateTime());
	export("$DataID_" @ %aid @ "*", "config/server/DataIDs/" @ %aid @ ".cs");
	return %aid;
}

function unloadDataIDArray(%aid)
{
	if ($DataIDDebug) talk("unloadDataIDArray");
	%aid = getSafeDataIDArrayName(%aid);
	if ($DataIDEcho) echo("DataID: Unloading " @ %aid);
	if (!isDataIDLoaded(%aid))
	{
		return;
	}
	popSaveDataIDArray(%aid);
	deleteVariables("$DataID_" @ %aid @ "_*");
	popDataID(%aid);
	deleteVariables("$executedDataID" @ %aid);
}

function deleteDataIDArray(%aid)
{
	if ($DataIDDebug) talk("deleteDataIDArray");
	%aid = getSafeDataIDArrayName(%aid);
	if ($DataIDEcho) echo("DataID: Deleting " @ %aid SPC getDateTime());

	if(%aid $= "")
	{
		talk("DATAID ERROR: deleteDataIDArray has empty %aid, check backtrace");
		backTrace();
		return;
	}
	
	deleteVariables("$DataID_" @ %aid @ "_*");
	popDataID(%aid);
	deleteVariables("$executedDataID" @ %aid);
	fileDelete("config/server/DataIDs/" @ %aid @ ".cs");
	return %aid;
}

function pruneDataIDArrays()
{
	if ($DataIDDebug) talk("pruneDataIDArrays");
	if ($nextPruneDataIDArray > $Sim::Time)
	{
		return;
	}
	$nextPruneDataIDArray = $Sim::Time + 10 | 0;
	while (getLoadedDataIDCount() > 200 && %safety++ < 10)
	{
		%oldest = getOldestDataID();
		%oldestTime = LoadedDataIDS.lastTouched[%oldest];
		%age = (getSimTime() - %oldestTime | 0) / 1000;
		if (%age < 30 && getLoadedDataIDCount() < 600) //don't prune if its not actually aged at all
		{
			echo("    Hard exit due to young dataIDs");
			break;
		}
		%avgAge += %age;
		unloadDataIDArray(%oldest);

		%count++;
	}
	if ($DataIDDebug) talk("Pruned " @ %count + 0 @ " arrays (avg age " @ (%avgAge / %count) @ "s)");
	else if (%count > 0) echo("Pruned " @ %count + 0 @ " arrays (avg age " @ (%avgAge / %count) @ "s)");
}

function printDataIDArray(%aid, %skipLoad)
{
	if ($DataIDDebug) talk("printDataIDArray");
	%aid = getSafeDataIDArrayName(%aid);
	if (!%skipLoad)
	{
		%aid = loadDataIDArray(%aid);
		echo("Loaded [" @ %aid @ "] array");
	}
	echo("DataID Array [" @ %aid @ "]");

	%count = getDataIDArrayCount(%aid);
	echo("Count: " @ %count);
	for (%i = 0; %i < %count; %i++)
	{
		echo(%i @ ": " @ getDataIDArrayValue(%aid, %i));
	}

	%tags = getDataIDArrayTags(%aid);
	echo("Tags: " @ %tags);
	for (%i = 0; %i < getWordCount(%tags); %i++)
	{
		%tag = getWord(%tags, %i);
		echo(%tag @ ": " @ getDataIDArrayTagValue(%aid, %tag));
	}
}



//reads
function getDataIDArrayValue(%aid, %slot)
{
	if (%aid $= "") return;
	if ($DataIDDebug) talk("getDataIDArrayValue");
	%aid = loadDataIDArray(%aid);

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

function getDataIDArrayTagValue(%aid, %tag)
{
	if (%aid $= "") return;
	if ($DataIDDebug) talk("getDataIDArrayTagValue");
	%aid = loadDataIDArray(%aid);

	return $DataID_[%aid, %tag];
}

function getDataIDArrayCount(%aid)
{
	if (%aid $= "") return;
	if ($DataIDDebug) talk("getDataIDArrayCount");
	%aid = loadDataIDArray(%aid);

	$DataID_[%aid, "count"] += 0; //ensure its an integer rather than empty string

	return $DataID_[%aid, "count"];
}

function indexOfDataIDArray(%aid, %value, %startIndex)
{
	if (%aid $= "") return;
	if ($DataIDDebug) talk("indexOfDataIDArray");
	%aid = loadDataIDArray(%aid);

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

function getDataIDArrayTags(%aid)
{
	if (%aid $= "") return;
	if ($DataIDDebug) talk("getDataIDArrayTags");
	%aid = loadDataIDArray(%aid);

	return $DataID_[%aid, "tags"];
}




//writes
//resize %aid to size %count
function setDataIDArrayCount(%aid, %count)
{
	if ($DataIDDebug) talk("setDataIDArrayCount");
	%aid = loadDataIDArray(%aid);
	if ($DataIDEcho) echo("DataID: Setting " @ getSubStr(%aid, 0, 15) @ " count to " @ %count);

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

	queueSaveDataIDArray(%aid);
}


//set %aid[%slot] to %value
//%slot clamped to %count (cannot insert past the end of a list)
function setDataIDArrayValue(%aid, %slot, %value)
{
	if ($DataIDDebug) talk("setDataIDArrayValue");
	%aid = loadDataIDArray(%aid);
	if ($DataIDEcho) echo("DataID: Setting " @ getSubStr(%aid, 0, 15) @ "[" @ %slot @ "]: set [" @ %value @ "] " @ getDateTime());

	%slot = getMax(%slot + 0, 0); //ensure it's not empty string
	%count = getDataIDArrayCount(%aid);
	if (%slot >= %count)
	{
		setDataIDArrayCount(%aid, %slot + 1);
	}

	$DataID_[%aid, %slot] = %value;

	queueSaveDataIDArray(%aid);
}

//add %value to first available slot in %aid. %start optional
function addToDataIDArray(%aid, %value, %start)
{
	if ($DataIDDebug) talk("addToDataIDArray");
	%aid = loadDataIDArray(%aid);
	if ($DataIDEcho) echo("DataID: Setting " @ getSubStr(%aid, 0, 15) @ "[" @ %slot @ "]: add [" @ %value @ "] " @ getDateTime());

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

	queueSaveDataIDArray(%aid);
	return %returnIDX;
}

//sets %tag to %value, overriding any existing value
function setDataIDArrayTagValue(%aid, %tag, %value)
{
	if ($DataIDDebug) talk("setDataIDArrayTagValue");
	%aid = loadDataIDArray(%aid);
	if ($DataIDEcho && %tag !$= "durability") echo("DataID: Setting " @ getSubStr(%aid, 0, 15) @ "[" @ %tag @ "]: set [" @ %value @ "] " @ getDateTime());

	%tag = getSafeDataIDArrayName(%tag);
	if (%tag $= %tag + 0 || %tag $= "tags" || %tag $= "count" || %tag $= "")
	{
		error("ERROR: setDataIDArrayTagValue - tag is invalid!");
		return;
	}

	$DataID_[%aid, %tag] = %value;
	%currTags = " " @ $DataID_[%aid, "tags"] @ " ";
	if (strPos(%currTags, " " @ %tag @ " ") < 0)
	{
		$DataID_[%aid, "tags"] = trim($DataID_[%aid, "tags"] SPC %tag);
	}

	queueSaveDataIDArray(%aid);
}

//removes value at %slot
function removeDataIDArrayValue(%aid, %slot)
{
	if ($DataIDDebug) talk("removeDataIDArrayValue");
	%aid = loadDataIDArray(%aid);
	if ($DataIDEcho) echo("DataID: Setting " @ getSubStr(%aid, 0, 15) @ "[" @ %slot @ "]: removed");

	%count = getDataIDArrayCount(%aid);
	$DataID_[%aid, %slot] = "";

	queueSaveDataIDArray(%aid);
}

//removes value at %tag
function removeDataIDArrayTag(%aid, %tag) { removeDataIDArrayTagValue(%aid, %tag); }
function removeDataIDArrayTagValue(%aid, %tag)
{
	if ($DataIDDebug) talk("removeDataIDArrayTagValue");
	%aid = loadDataIDArray(%aid);
	if ($DataIDEcho) echo("DataID: Setting " @ getSubStr(%aid, 0, 15) @ "[" @ %tag @ "]: removed");

	%tag = getSafeDataIDArrayName(%tag);
	if (%tag $= %tag + 0 || %tag $= "tags" || %tag $= "count" || %tag $= "")
	{
		error("ERROR: removeDataIDArrayTagValue - tag is invalid!");
		return;
	}

	$DataID_[%aid, %tag] = "";
	%currTags = " " @ $DataID_[%aid, "tags"] @ " ";
	if ((%pos = strPos(%currTags, " " @ %tag @ " ")) >= 0)
	{
		%currTags = getSubStr(%currTags, 0, %pos) @ getSubStr(%currTags, strLen(%tag) + 1 + %pos, strLen(%currTags));
		$DataID_[%aid, "tags"] = trim(%currTags);
	}

	queueSaveDataIDArray(%aid);
}

function clearDataIDArray(%aid)
{
	if ($DataIDDebug) talk("clearDataIDArray");
	%aid = getSafeDataIDArrayName(%aid);
	if ($DataIDEcho) echo("DataID: Setting " @ %aid @ ": cleared");

	if(%aid $= "")
	{
		talk("DATAID ERROR: clearDataIDArray has empty %aid, check backtrace");
		backTrace();
		return;
	}

	deleteVariables("$DataID_" @ %aid @ "_*");
	popDataID(%aid);
	deleteVariables("$executedDataID" @ %aid);

	if (isFile("config/server/DataIDs/" @ %aid @ ".cs"))
	{
		fileDelete("config/server/DataIDs/" @ %aid @ ".cs");
	}
}







package DataIDBadIDDebug
{
	function loadDataIDArray(%aid, %force)
	{
		if (strlen(%aid) < 6)
		{
			talk("Attempting to load bad dataid (" @ %aid @ ")! Please ping Conan, and whoever triggered this message please say what you did in chat");
			return;
		}
		return parent::loadDataIDArray(%aid, %force);
	}
	function unloadDataIDArray(%aid)
	{
		if (strlen(%aid) < 6)
		{
			talk("Attempting to unload bad dataid (" @ %aid @ ")!");
			popDataID(%aid);
			deleteVariables("$executedDataID" @ %aid);
			return;
		}
		return parent::unloadDataIDArray(%aid, %force);
	}
};
activatePackage(DataIDBadIDDebug);