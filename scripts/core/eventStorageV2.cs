//input: output from getStorageValue()
//output: itemDatablock TAB displayName (uiname or stacktype) TAB count
function validateStorageValue(%string)
{
	if (%string $= "")
	{
		return "";
	}

	%storageType = getField(%string, 0); //can be item DB, can be stackType string
	%count = getField(%string, 1);

	if (isObject(%storageType))
	{
		//is a normal item
		return %storageType TAB %storageType.uiName TAB %count;
	}
	else if (isObject(%db = getStackTypeDatablock(%storageType, %count)))
	{
		//is a stackable item
		%displayName = strUpr(getSubStr(%storageType, 0, 1)) @ getSubStr(%storageType, 1, 20);
		return %db TAB %displayName TAB %count;
	}

	return "";
}

//input: item db or stacktype string, count
//output: proper storage string
function getStorageValue(%storageType, %count)
{
	if (isObject(%storageType) && %storageType.isStackable)
	{
		//is stackable item, use stackType string instead of datablock name
		%storageType = %storageType.stackType;
	}
	else
	{
		//is normal item, ensure datablock name is stored and not item id
		%storageType = %storageType.getName();
	}
	return %storageType TAB %count;
}

//input: storage brick datablock, dataID
//output: error code (0 for no error, 1 for mismatch error)
function initializeStorage(%brick, %dataID)
{
	if (%brick.hasInitializedStorage_[%dataID])
	{
		return 0;
	}
	%brick.hasInitializedStorage_[%dataID] = 1;

	if (getWord(%info = getDataIDArrayValue(%dataID, 0), 0) !$= "info")
	{
		%info = "info " 
			TAB "blid " @ %brick.getGroup().bl_id 
			TAB "datablock " @ %brick.getDatablock().getName()
			TAB "tf " @ %brick.getTransform();
		setDataIDArrayValue(%dataID, 0, %info);
		return 0;
	}
	else
	{
		%datablock = getWord(getField(%info, 2), 1);
		if (%brick.getDatablock().getName() !$= %datablock)
		{
			error("Storage datablock mismatch! brick:[" @ %brick @ "] brickDB:[" 
				@ %brick.getDatablock().getName() @ "] dataID:[" @ %dataID @ "]");
			talk("ERROR: storage datablock mismatch! Please inform the host! brick:[" @ %brick @ "] brickDB:[" 
				@ %brick.getDatablock().getName() @ "] dataID:[" @ %dataID @ "]");
			return 1;
		}
	}
	return 0;
}

//updates %brick.centerprintMenu - called via event
function fxDTSBrick::updateStorageMenu(%brick, %dataID)
{
	if (%brick.centerprintMenu.nextUpdate < $Sim::Time)
	{
		//skip updating if its been very recently updated? or some other thing
		return;
	}
	initializeStorage(%brick, %dataID);

	//get storage data
	%max = getMin(getDataIDArrayCount(%dataID), %brick.getDatablock().storageCount);
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%count++;
	}

	if (!isObject(%brick.centerprintMenu))
	{
		%brick.centerprintMenu = new ScriptObject(StorageCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = %brick.getDatablock().uiName;

			menuOptionCount = 0;
			brick = %brick;
		};
		MissionCleanup.add(%brick.centerprintMenu);
	}

	for (%i = 0; %i < %count; %i++)
	{
		%db = getField(%data[%i], 0);
		%display = getField(%data[%i], 1);
		%itemCount = getField(%data[%i], 2);
		if (isObject(%db))
		{
			%entry = "Empty";
		}
		else
		{
			%entry = %display @ " - " @ %itemCount;
		}
		%brick.centerprintMenu.menuOption[%i] = %entry;
		%brick.centerprintMenu.menuDatablock[%i] = %db;
		%brick.centerprintMenu.menuFunction[%i] = "removeStack";
	}
}

//input: brick being stored on, dataID, item db (stackable or non stackable), count of objects inserted
//output: error code (0 if no error/complete store, 1 SPC %excess if partial store, 2 if no store)
function insertIntoStorage(%brick, %dataID, %storeItemDB, %insertCount)
{
	initializeStorage(%brick, %dataID);

	//get storage data
	%max = getMin(getDataIDArrayCount(%dataID), %brick.getDatablock().storageCount);
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%count++;
	}

	%stackType = %storeItemDB.stackType;
	%storageMax = getStorageMax(%storeItemDB); //TODO: Implement getStorageMax
	if (%storageMax <= 0 && %storageType.isStackable)
	{
		return 0;
	}

	for (%i = 0; %i < %count; %i++)
	{
		%db = getField(%data[%i], 0);
		%display = getField(%data[%i], 1);
		%itemCount = getField(%data[%i], 2);

		if (%db $= "" || !isObject(%db) || 
			(%storeItemDB.isStackable && %db.stackType $= %stackType && %itemCount < %storageMax) ||
			(%db.getID() == %storeItemDB.getID() && %itemCount < %storageMax))
		{
			//is empty, or has space and matches stacktype and is stackable, or has space and matches db
			%totalAvailableSpace += %storageMax - %itemCount;
			%availableSpace[%spaces++ - 1] = %i SPC %storageMax - %itemCount TAB (%itemCount + 0);
		}

		if (%totalAvailableSpace >= %insertCount)
		{
			break;
		}
	}

	if (%storeItemDB.isStackable)
	{
		//inserting stackable items
		if (%totalAvailableSpace >= %insertCount)
		{
			//enough space to insert everything
			for (%i = 0; %i < %spaces; %i++)
			{
				%slot = getWord(%availableSpace[%i], 0);
				%maxAdd = getWord(%availableSpace[%i], 1);
			}
			return 0;
		}
		else if (%totalAvailableSpace > 0)
		{
			//only enough space for some insertion
			return 1 SPC %insertCount - %totalAvailableSpace;
		}
		else
		{
			return 2;
		}
	}
	else
	{
		//inserting non-stackable items
		if (%totalAvailableSpace >= %insertCount)
		{
			//enough space to insert everything

			return 0;
		}
		else if (%totalAvailableSpace > 0)
		{
			//only enough space for some insertion
			return 1 SPC %insertCount - %totalAvailableSpace;
		}
		else
		{
			return 2;
		}
	}

	%brick.updateStorageMenu(%dataID);
}






















function fxDTSBrick::accessStorage(%brick, %dataID, %client)
{
	%brick.updateStorageMenu(%dataID);
}
registerOutputEvent("fxDTSBrick", "accessStorage", "string 200 50", 1);

function storageLoop(%cl, %brick)
{
	cancel(%cl.storageSchedule);

	if (!isObject(%brick) || !isObject(%cl.player) || !%cl.isInCenterprintMenu)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	if (vectorDist(%brick.getPosition(), %cl.player.getPosition()) > 6)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	%start = %cl.player.getEyePoint();
	%end = vectorAdd(vectorScale(%cl.player.getEyeVector(), 8), %start);
	if (containerRaycast(%start, %end, $Typemasks::fxBrickObjectType) != %brick)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	%cl.displayCenterprintMenu(0);

	%cl.storageSchedule = schedule(200, %cl, storageLoop, %cl, %brick);
}