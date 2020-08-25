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
		if ($displayNameOverride_[%displayName] $= "")
		{
			%displayName = strUpr(getSubStr(%storageType, 0, 1)) @ getSubStr(%storageType, 1, 20);
		}
		else
		{
			%displayName = $displayNameOverride_[%displayName];
		}
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
			echo("ERROR: storage datablock mismatch! Please inform the host! brick:[" @ %brick @ "] brickDB:[" 
				@ %brick.getDatablock().getName() @ "] dataID:[" @ %dataID @ "]");
			return 1;
		}
	}
	return 0;
}

//updates %brick.centerprintMenu - called via event
function fxDTSBrick::updateStorageMenu(%brick, %dataID)
{
	if (isObject(%brick.centerprintMenu) && %brick.centerprintMenu.nextUpdate > $Sim::Time)
	{
		//skip updating if its been very recently updated? or some other thing
		return;
	}

	//get storage data
	%max = %brick.getDatablock().storageCount + 1;
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

			menuOptionCount = %max;
			brick = %brick;
		};
		MissionCleanup.add(%brick.centerprintMenu);
	}

	for (%i = 0; %i < %count; %i++)
	{
		%db = getField(%data[%i], 0);
		%display = getField(%data[%i], 1);
		%itemCount = getField(%data[%i], 2);
		if (!isObject(%db))
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
function fxDTSBrick::insertIntoStorage(%brick, %dataID, %storeItemDB, %insertCount)
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
	%storageMax = %brick.getStorageMax(%storeItemDB);
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

function fxDTSBrick::getStorageMax(%brick, %itemDB)
{
	//TODO: complete implementation
	return 100;
}






















function fxDTSBrick::accessStorage(%brick, %dataID, %cl)
{
	%brick.updateStorageMenu(%dataID);

	%cl.startCenterprintMenu(%brick.centerprintMenu);
	storageLoop(%cl, %brick);
}
registerOutputEvent("fxDTSBrick", "accessStorage", "string 200 250", 1);

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

function addStorageEvent(%this, %botForm)
{
	for (%i = 1; %i < 5; %i++)
	{
		%param[%i] = %this.eventOutputParameter[0, %i];
	}
	%this.clearEvents();

	%enabled = 1;
	if (%botForm)
	{
		%inputEvent = "onBotActivated";
	}
	else
	{
		%inputEvent = "onActivate";	
	}
	%delay = 0;
	%target = "Self"; //Self
	%outputEvent = "accessStorage";
	%param1 = sha1(getRandom() @ getRandom() @ getRandom() @ getRandom());

	%this.storageAddEvent = 1;
	if (!isObject(%this.getGroup().client))
	{
		%this.getGroup().client = new ScriptObject(DummyClient)
		{
			isAdmin = 1;
			isSuperAdmin = 1;
			wrenchBrick = %this;
			bl_id = %this.getGroup().bl_id;
			brickGroup = %this.getGroup();
		};
		%this.getGroup().client.client = %this.getGroup().client;
		%dummy = 1;
	}
	%prev = %this.getGroup().client.isAdmin;
	%this.getGroup().client.isAdmin = 1;
	%this.addEvent(%enabled, %delay, %inputEvent, %target, %outputEvent, %param1, %param2, %param3, %param4);
	%this.storageAddEvent = -1;
	%this.getGroup().client.isAdmin = %prev;
	if (%dummy)
	{
		%this.getGroup().client.delete();
	}
}












package StorageBricks
{
	function fxDTSBrick::onAdd(%this)
	{
		if (%this.isPlanted && %this.getDatablock().isStorageBrick)
		{
			schedule(100, %this, addStorageEvent, %this);
		}

		return parent::onAdd(%this);
	}

	function serverCmdClearEvents(%client)
	{
		if (isObject(%client) && !%client.isAdmin)
		{
			return;
		}
		else
		{
			return parent::serverCmdClearEvents(%client);
		}
	}

	function serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4)
	{
		if (isObject(%b = %client.wrenchBrick) && %b.storageAddEvent == -1)
		{
			return;
		}
		else
		{
			return parent::serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
		}
	}

	function fxDTSBrick::onRemove(%this)
	{
		if (isObject(%this.centerprintMenu))
		{
			%this.centerprintMenu.delete();
		}

		%idx = %this.eventOutputIdx[0];
		if (%this.getDatablock().isStorageBrick || (%idx !$= "" && $OutputEvent_Name["fxDTSBrick", %idx] $= "displayStorageContents"))
		{
			if (!%this.droppedStoredItems)
			{
				%this.droppedStoredItems = 1;
				dropStoredItems(%this);
			}
		}

		return parent::onRemove(%this);
	}

	function fxDTSBrick::onDeath(%this) 
	{
		%idx = %this.eventOutputIdx[0];
		if (%this.getDatablock().isStorageBrick || (%idx !$= "" && $OutputEvent_Name["fxDTSBrick", %idx] $= "displayStorageContents"))
		{
			if (!%this.droppedStoredItems)
			{
				%this.droppedStoredItems = 1;
				dropStoredItems(%this);
			}
		}
		return parent::onDeath(%this);
	}

	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%item = %pl.tool[%slot];
			%start = %pl.getEyePoint();
			%end = vectorAdd(vectorScale(%pl.getEyeVector(), 6), %start);
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && %hit.getDatablock().isStorageBrick)
			{
				%success = %hit.insertIntoStorage(%hit.eventOutputParameter[0, 1], %item, %pl.toolStackCount[%slot]);
				if (%success)
				{
					return;
				}
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}
};
schedule(1000, 0, activatePackage, StorageBricks);
