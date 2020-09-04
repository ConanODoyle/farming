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
	%itemDataID = getField(%string, 2);

	if (isObject(%storageType) && !%storageType.isStackable)
	{
		%db = %storageType;
		%displayName = %storageType.uiName;
		//is a normal item
	}
	else if (isObject(%db = getStackTypeDatablock(%storageType, %count)) || (isObject(%storageType) && %storageType.isStackable))
	{
		if (isObject(%storageType))
		{
			%db = %storageType;
		}
		//is a stackable item
		if ($displayNameOverride_[%displayName] $= "")
		{
			%displayName = strUpr(getSubStr(%storageType, 0, 1)) @ getSubStr(%storageType, 1, 20);
		}
		else
		{
			%displayName = $displayNameOverride_[%displayName];
		}
	}
	return %db TAB %displayName TAB %count TAB %itemDataID;
}

//input: item db or stacktype string, count, item dataID id
//output: proper storage string - (itemDB if not stackable/stackType if stackable, count)
function getStorageValue(%storageType, %count, %itemDataID)
{
	if (isObject(%storageType) && %storageType.isStackable)
	{
		//is stackable item, use stackType string instead of datablock name
		%storageType = %storageType.stackType;
	}
	else if (isObject(%storageType))
	{
		//is normal item, ensure datablock name is stored and not item id
		%storageType = %storageType.getName();
	}
	return %storageType TAB %count TAB %itemDataID;
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
	if (isObject(%brick.centerprintMenu) && %brick.centerprintMenu.nextUpdate > $Sim::Time)
	{
		//skip updating if its been very recently updated? or some other thing
		return;
	}

	//get storage data
	%max = getMax(%brick.getDatablock().storageSlotCount, 1);
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
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
		%itemDataID = getField(%data[%i], 3);
		if (!isObject(%db))
		{
			%entry = "Empty";
		}
		else
		{
			if (%itemDataID !$= "")
			{
				%entry = %display @ " - " @ getSubStr(%itemDataID, 0, 3);
			}
			else
			{
				%entry = %display @ " - " @ %itemCount;
			}
		}
		%brick.centerprintMenu.menuOption[%i] = %entry;
		%brick.centerprintMenu.menuDatablock[%i] = %db;
		%brick.centerprintMenu.menuFunction[%i] = "removeStack";
	}
	%brick.centerprintMenu.storageDataID = %dataID;
}

//input: brick being stored on, dataID, item db (stackable or non stackable), count of objects inserted
//output: error code (0 if no error/complete store, 1 SPC %excess if partial store, 2 if no store)
function fxDTSBrick::insertIntoStorage(%brick, %dataID, %storeItemDB, %insertCount, %itemDataID)
{
	initializeStorage(%brick, %dataID);

	//get storage data
	%max = getMax(%brick.getDatablock().storageSlotCount, 1);
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%count++;
	}

	%stackType = %storeItemDB.stackType;
	%storageMax = %brick.getStorageMax(%storeItemDB);
	if (%storageMax <= 0) //cannot store any at all
	{
		return 2;
	}

	for (%i = 0; %i < %count; %i++)
	{
		%storageType = getField(%data[%i], 0);
		%display = getField(%data[%i], 1);
		%itemCount = getField(%data[%i], 2);

		if (%storageType $= "" || !isObject(%storageType) || 
			(%storeItemDB.isStackable && %storageType.stackType $= %stackType && %itemCount < %storageMax) ||
			(%storageType.getID() == %storeItemDB.getID() && %itemCount < %storageMax))
		{
			//is empty, or has space and matches stacktype and is stackable, or has space and matches db
			%totalAvailableSpace += %storageMax - %itemCount;
			%availableSpace[%spaces++ - 1] = %i + 1 SPC %storageMax - %itemCount SPC (%itemCount + 0);
		}

		if (%totalAvailableSpace >= %insertCount)
		{
			break;
		}
	}

	if (%totalAvailableSpace >= %insertCount)
	{
		//enough space to insert everything
		for (%i = 0; %i < %spaces; %i++)
		{
			%slot = getWord(%availableSpace[%i], 0);
			%spaceAvailable = getWord(%availableSpace[%i], 1);
			%amountPresent = getWord(%availableSpace[%i], 2);

			%value = getStorageValue(%storeItemDB, %insertCount, %itemDataID);
			%insertAmount = getMin(%insertCount, %spaceAvailable);
			%insertCount -= %insertAmount;
			%total = %amountPresent + %insertAmount;
			%value = getField(%value, 0) TAB %total;
			setDataIDArrayValue(%dataID, %slot, %value);

			if (%insertCount == 0)
			{
				break;
			}
		}

		%brick.updateStorageMenu(%dataID);
		return 0;
	}
	else if (%totalAvailableSpace > 0)
	{
		//only enough space for some insertion
		for (%i = 0; %i < %spaces; %i++)
		{
			%slot = getWord(%availableSpace[%i], 0);
			%spaceAvailable = getWord(%availableSpace[%i], 1);
			%amountPresent = getWord(%availableSpace[%i], 2);

			%value = getStorageValue(%storeItemDB, %insertCount, %itemDataID);
			%insertAmount = getMin(%insertCount, %spaceAvailable);
			%insertCount -= %insertAmount;
			%total = %amountPresent + %insertAmount;
			%value = getField(%value, 0) TAB %total;
			setDataIDArrayValue(%dataID, %slot, %value);
		}

		%brick.updateStorageMenu(%dataID);
		return 1 SPC %insertCount;
	}
	else
	{
		return 2;
	}
}

function removeStack(%cl, %menu, %option)
{
	if (!isObject(%cl.player))
	{
		%cl.centerprint("You are dead!", 1);
		return;
	}
	%brick = %menu.brick;

	if (!isObject(%brick.vehicle) && vectorDist(%brick.getPosition(), %cl.player.getHackPosition()) > 7)
	{
		%cl.centerprint("You are too far away from the crate!", 1);
		return;
	}
	else if (isObject(%brick.vehicle) && vectorDist(%brick.vehicle.getPosition(), %cl.player.getHackPosition()) > 7)
	{
		%cl.centerprint("You are too far away from the cart!", 1);
		return;
	}


	if (%menu.menuOption[%option] $= "Empty")
	{
		if (%cl.nextMessageEmpty < $Sim::Time)
			messageClient(%cl, '', "That slot is empty!", 1);

		%cl.nextMessageEmpty = $Sim::Time + 2;

		%cl.startCenterprintMenu(%menu);
		%cl.displayCenterprintMenu(%option);
		return;
	}

	%dataID = %menu.storageDataID;
	%storageSlot = %option + 1;
	%storageData = validateStorageValue(getDataIDArrayValue(%dataID, %storageSlot));
	%datablock = getField(%storageData, 0);
	%displayName = getField(%storageData, 1);
	%storageCount = getField(%storageData, 2);
	%itemDataID = getField(%storageData, 3);
	if (!isObject(%datablock))
	{
		talk("ERROR: removestack - storage data invalid! contents:[" @ strReplace(%storageData, "\t", "|") @ "]");
		talk("Please notify an admin of this error!");
		return;
	}

	if (%datablock.isStackable)
	{
		%stackType = %datablock.stackType;
		%maxID = $Stackable_[%stackType, "stackedItemTotal"] - 1;
		%max = getWord($Stackable_[%stackType, "stackedItem" @ %maxID], 1);

		%amt = getMin(%max, %storageCount);
	}
	else
	{
		%amt = 1;
	}
	%left = %storageCount - %amt;

	if (%left > 0)
	{
		setDataIDArrayValue(%dataID, %storageSlot, getStorageValue(%datablock, %left));
	}
	else
	{
		setDataIDArrayValue(%dataID, %storageSlot, "");
	}

	%brick.updateStorageMenu(%dataID);

	%i = new Item()
	{
		dataBlock = %datablock;
		count = %amt;
		harvestedBG = getBrickgroupFromObject(%brick);
		dataID = %itemDataID;
	};
	MissionCleanup.add(%i);
	%i.setTransform(%cl.player.getTransform());

	%cl.startCenterprintMenu(%menu);
	%cl.displayCenterprintMenu(%option);
}

function fxDTSBrick::getStorageMax(%brick, %itemDB)
{
	%brickDB = %brick.getDatablock();
	if (%itemDB.isStackable)
	{
		%stackType = %itemDB.stackType;
		%total = getMax(0, %brickDB.storageMultiplier * $StorageMax_[%stackType]);
	}
	else if (%itemDB.hasDataID)
	{
		%total = 1;
	}
	else
	{
		%total = getMax(0, %brickDB.itemStackCount);
	}
	return %total;
}














function fxDTSBrick::accessStorage(%brick, %dataID, %cl)
{
	%brick.updateStorageMenu(%dataID);

	%cl.startCenterprintMenu(%brick.centerprintMenu);
	storageLoop(%cl, %brick);
}
registerOutputEvent("fxDTSBrick", "accessStorage", "string 200 250", 1);

function AIPlayer::accessStorage(%bot, %dataID, %cl)
{
	%bot.spawnbrick.updateStorageMenu(%dataID);

	%cl.startCenterprintMenu(%bot.spawnbrick.centerprintMenu);
	storageLoop(%cl, %bot);
}
registerOutputEvent("AIPlayer", "accessStorage", "string 200 250", 1);

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
	if (containerRaycast(%start, %end, %brick.getType()) != %brick)
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
	if (%param1 $= "")
	{
		%param1 = sha1(getRandom() @ getRandom() @ getRandom() @ getRandom());
	}

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

function dropStoredItems(%brick)
{
	%db = %brick.getDatablock();
	%dataID = %brick.eventOutputParameter0_1;
	for (%i = 0; %i < %db.storageSlotCount; %i++)
	{
		%storageSlot = %i + 1;
		%storageData = validateStorageValue(getDataIDArrayValue(%dataID, %storageSlot));
		%itemDB = getField(%storageData, 0);
		%storageCount = getField(%storageData, 2);
		%packageInfo = getField(%storageData, 3);

		%stackType = %itemDB.stackType;

		%max = $Stackable_[%stackType, "stackedItemTotal"] - 1;
		%amt = %storageCount;
		// talk("stackType: " @ %stackType @ " max: " @ %max @ " | " @ $Stackable_[%stackType, "stackedItem" @ %max]);

		if (!isObject(%itemDB))
		{
			continue;
		}

		while (%amt > 0)
		{
			%item = new Item()
			{
				dataBlock = %itemDB;
				count = %amt;
				deliveryPackageInfo = %packageInfo;
				sourceClient = getBrickgroupFromObject(%brick).client;
				sourceBrickgroup = getBrickgroupFromObject(%brick).client;
			};
			MissionCleanup.add(%item);
			%vel = (getRandom(6) - 3) / 2 SPC  (getRandom(6) - 3) / 2 SPC 6;
			%item.setTransform(%brick.getPosition());
			%item.setVelocity(%vel);
			%item.schedule(60000, schedulePop);
			if (%stackType $= "")
			{
				%amt--; //not normally stackable (tool)
			}
			else
			{
				%amt = 0; //included the full stack in the first drop
			}
		}
		setDataIDArrayValue(%dataID, %storageSlot, "");
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

	function fxDTSBrick::onDupCut(%this)
	{
		if (isObject(%this.centerprintMenu))
		{
			%this.centerprintMenu.delete();
		}

		if (%this.getDatablock().isStorageBrick)
		{
			if (!%this.droppedStoredItems)
			{
				%this.droppedStoredItems = 1; 
				dropStoredItems(%this);
			}
		}

		return parent::onDupCut(%this);
	}

	function fxDTSBrick::onDeath(%this) 
	{
		if (%this.getDatablock().isStorageBrick)
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
				%success = %hit.insertIntoStorage(%hit.eventOutputParameter[0, 1], %item, %pl.toolStackCount[%slot] == 0 ? 1 : %pl.toolStackCount[%slot], %pl.toolDataID[%slot]);
				if (%success < 1) //complete insertion
				{
					%pl.toolStackCount[%slot] = 0;
					%pl.tool[%slot] = 0;
					messageClient(%cl, 'MsgItemPickup', "", %slot, 0);
					if (%pl.currTool == %slot)
					{
						%pl.unmountImage(0);
					}
					return;
				}
				else if (%success < 2) //partial insertion
				{
					%pl.toolStackCount[%slot] = getWord(%success, 1);
					%db = getStackTypeDatablock(%pl.tool[%slot].stackType, getWord(%success, 1)).getID();
					messageClient(%cl, 'MsgItemPickup', "", %slot, %db);
					%pl.tool[%slot] = %db;
					if (%pl.currTool == %slot)
					{
						%pl.mountImage(%db.image, 0);
					}
					return;
				}
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}
};
schedule(1000, 0, activatePackage, StorageBricks);
