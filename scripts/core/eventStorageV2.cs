//input: output from getStorageValue()
//output: itemDatablock TAB displayName (uiname or stacktype) TAB count TAB itemDataID
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
		if ($displayNameOverride_[%storageType] $= "")
		{
			%displayName = strUpr(getSubStr(%storageType, 0, 1)) @ getSubStr(%storageType, 1, 20);
		}
		else
		{
			%displayName = $displayNameOverride_[%storageType];
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
	else if (!isObject(%brick.storageObj))
	{
		return;
	}

	//get storage data
	%max = getMax(%brick.storageObj.getDatablock().storageSlotCount, 1);
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%count++;
	}

	if (!isObject(%brick.centerprintMenu))
	{
		%storeObjDB = %brick.storageObj.getDatablock();
		%brick.centerprintMenu = new ScriptObject(StorageCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = %storeObjDB.menuName $= "" ? %storeObjDB.uiName : %storeObjDB.menuName;

			menuOptionCount = %max;
			brick = %brick;
		};
		MissionCleanup.add(%brick.centerprintMenu);
	}
	else if (%max != %brick.centerprintMenu.menuOptionCount)
	{
		%brick.centerprintMenu.menuOptionCount = %max;
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
			if (%db.hasDataID !$= "")
			{
				%entry = trim(%display) @ " [" @ getSubStr(%itemDataID, strLen(%itemDataID) - 3, 3) @ "]";
			}
			else
			{
				%max = %brick.getStorageMax(%db);
				if (isObject(%brick.vehicle) && %max == 0)
				{
					if (%brick.vehicle.getClassName() $= "AIPlayer")
					{
						%max = %brick.vehicle.getStorageMax(%db);
					}
					else if (isObject(%brick.vehicle.storageBot))
					{
						%max = %brick.vehicle.storageBot.getStorageMax(%db);
					}
					else
					{
						%max = "???";
					}
				}
				%entry = trim(%display) @ " - " @ %itemCount @ "/" @ %max;
			}
		}
		%brick.centerprintMenu.menuOption[%i] = %entry;
		%brick.centerprintMenu.menuDatablock[%i] = %db;
		%brick.centerprintMenu.menuFunction[%i] = "removeStack";
	}
	%brick.centerprintMenu.storageDataID = %dataID;
}

function fxDTSBrick::insertIntoStorage(%brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %pickedSlot)
{
	return insertIntoStorage(%brick, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %pickedSlot);
}

function AIPlayer::insertIntoStorage(%bot, %dataID, %storeItemDB, %insertCount, %itemDataID)
{

	%output = insertIntoStorage(%bot, %bot.spawnBrick, %dataID, %storeItemDB, %insertCount, %itemDataID);

	if (%bot.getDatablock().hasStorageNodes && (%output == 0 || %output == 1))
	{
		%bot.updateStorageNodes(%dataID);
	}

	return %output;
}

//input: storage object, brick being stored on, dataID, item db, count of objects inserted, item data id
//output: error code (0 if no error/complete store, 1 SPC %excess if partial store, 2 if no store)
function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot)
{
	if (%insertCount <= 0 || !isObject(%storeItemDB))
	{
		return;
	}
	initializeStorage(%brick, %dataID);

	//get storage data
	%max = getMax(%storageObj.getDatablock().storageSlotCount, 1);
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%count++;
	}

	%stackType = %storeItemDB.stackType;
	%storageMax = %storageObj.getStorageMax(%storeItemDB);
	%brickStorageType = %storageObj.getStorageType();
	if (!%storeItemDB.hasDataID)
	{
		%itemDataID = "";
	}

	if (%storageMax <= 0 || !storageTypeAccepts(%brickStorageType, %storeItemDB)) //cannot store any at all
	{
		return 2;
	}

	%searchStart = 0;
	%searchEnd = %count;
	if (%specificSlot !$= "")
	{
		if (%count <= %specificSlot)
		{
			return 2;
		}
		%searchStart = %specificSlot;
		%searchEnd = %specificSlot + 1;
	}

	for (%i = %searchStart; %i < %searchEnd; %i++)
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

			%insertAmount = mFloor(getMin(%insertCount, %spaceAvailable));
			%insertCount -= %insertAmount;
			%total = %amountPresent + %insertAmount;
			%value = getStorageValue(%storeItemDB, %total, %itemDataID);
			setDataIDArrayValue(%dataID, %slot, %value);

			if (%insertCount == 0)
			{
				break;
			}
		}

		%brick.updateStorageMenu(%dataID);
		%result = 0;
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
			%insertAmount = mFloor(getMin(%insertCount, %spaceAvailable));
			%insertCount -= %insertAmount;
			%total = %amountPresent + %insertAmount;
			%value = getField(%value, 0) TAB %total TAB getField(%value, 2);
			setDataIDArrayValue(%dataID, %slot, %value);
		}
		%brick.updateStorageMenu(%dataID);
		%result = 1 SPC %insertCount;
	}
	else
	{
		%result = 2;
	}


	if (%result == 0 || %result == 1)
	{
		%storageObj.updateStorageDatablock(%dataID, true);
	}

	return %result;
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


	if (striPos(%menu.menuOption[%option], "Empty") == 0)
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
		talk("ERROR: removestack - storage data invalid! contents:[" @ strReplace(%storageData, "\t", "|") @ "] @ slot \"" @ %storageSlot @ "\"");
		talk("    dataID:[" @ %dataID @ "] user " @ %cl.name @ "");
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

	if (%brick.getDatablock().isStorageBrick)
	{
		%storageObj = %brick;
	}
	else if (isObject(%brick.vehicle))
	{
		if (%brick.vehicle.getDatablock().isStorageVehicle)
		{
			%storageObj = %brick.vehicle;
		}
		else if (isObject(%brick.vehicle.storageBot))
		{
			%storageObj = %brick.vehicle.storageBot;
		}
	}

	if (%storageObj.getDatablock().hasStorageNodes)
	{
		%storageObj.updateStorageNodes(%dataID);
	}

	if (isObject(%storageObj))
	{
		%storageObj.updateStorageDatablock(%dataID, true);
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
	%i.setTransform(%cl.player.getHackPosition());

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
		if (%brick.dataBlock.itemStackCount $= "0")
		{
			return 0;
		}
		return 1;
	}
	else
	{
		%total = getMax(0, %brickDB.itemStackCount);
	}
	return %total;
}

function AIPlayer::getStorageMax(%bot, %itemDB)
{
	%botDB = %bot.getDatablock();
	if (%itemDB.isStackable)
	{
		%stackType = %itemDB.stackType;
		%total = getMax(0, %botDB.storageMultiplier * $StorageMax_[%stackType]);
	}
	else if (%itemDB.hasDataID)
	{
		%total = 1;
	}
	else
	{
		%total = getMax(0, %botDB.itemStackCount);
	}
	return %total;
}

function fxDTSBrick::getStorageType(%brick)
{
	return %brick.getDatablock().storageType;
}

function AIPlayer::getStorageType(%bot)
{
	return %bot.getDatablock().storageType;
}

function fxDTSBrick::updateStorageDatablock(%brick, %dataID, %open)
{
	%count = 0;
	%max = getMax(%brick.getDatablock().storageSlotCount, 1);
	for (%i = 1; %i < %max + 1; %i++)
	{
		%storageType = getWord(validateStorageValue(getDataIDArrayValue(%dataID, %i)), 0);
		if (isObject(%storageType))
		{
			%count++;
		}
	}

	if (%brick.getDatablock().baseDatablockName !$= "")
	{
		%datablockName = "brick" @ %brick.getDatablock().baseDatablockName;
		if (%brick.getDatablock().isOpenable)
		{
			if (%open)
			{
				cancel(%brick.closeSchedule);
				%datablockName = %datablockName @ "Open";
				%brick.closeSchedule = %brick.schedule(1000, updateStorageDatablock, %dataID, false);
				%brick.playSound(brickChangeSound);
			}
			else
			{
				%datablockName = %datablockName @ "Closed";
				%brick.playSound(brickPlantSound);
			}
		}

		if (%brick.getDatablock().hasFillLevels)
		{
			%datablockName = %datablockName @ (%count + 0);
		}

		%brick.setDatablock(%datablockName @ "Data");
	}
}

function AIPlayer::updateStorageDatablock(%bot, %fillLevel, %open)
{
	if (%bot.getDatablock().baseDatablockName !$= "")
	{
		%datablockName = %bot.getDatablock().baseDatablockName;
		if (%bot.getDataBlock().isOpenable)
		{
			if (%open)
			{
				cancel(%bot.closeSchedule);
				%datablockName = %datablockName @ "Open";
				%bot.closeSchedule = %bot.schedule(1000, updateStorageDatablock, %fillLevel, false);
			}
			else
			{
				%datablockName = %datablockName @ "Closed";
			}
		}

		if (%bot.getDataBlock().hasFillLevels)
		{
			%datablockName = %datablockName @ (%fillLevel + 0);
		}

		%bot.setDatablock(%datablockName @ "Armor");
	}
}

function AIPlayer::hideStorageNodes(%bot)
{
	%botDatablock = %bot.getDatablock();
	%numStorageNodes = %botDatablock.numStorageNodes;
	%startNode = %botDatablock.storageNodeType;
	%prefix = %botDatablock.storageNodePrefix;
	for (%i = %startNode; %i < %numStorageNodes; %i++)
	{
		%bot.hideNode(%prefix @ %i);
	}
}

function AIPlayer::updateStorageNodes(%bot, %dataID)
{
	// we have to update the storage
	%bot.hideStorageNodes();
	%botDatablock = %bot.getDatablock();
	%prefix = %botDatablock.storageNodePrefix;

	%percentFull = 0;
	%slotCount = %botDatablock.storageSlotCount;
	for (%slot = 1; %slot < %slotCount + 1; %slot++)
	{
		%entry = validateStorageValue(getDataIDArrayValue(%dataID, %slot));
		%itemDatablock = getField(%entry, 0);

		%count = getField(%entry, 2);
		%max = %bot.getStorageMax(%itemDatablock);

		%percentFull += %count / %max;
	}
	%percentFull /= %slotCount;

	%numNodes = %botDatablock.numStorageNodes;
	%maxNode = mCeil(%percentFull * (%numNodes - 1));

	if (%botDatablock.storageNodeType == 0)
	{
		%bot.unhideNode(%prefix @ %maxNode);
	}
	else if (%botDatablock.storageNodeType == 1)
	{
		for (%i = 1; %i <= %maxNode; %i++)
		{
			%bot.unhideNode(%prefix @ %i);
		}
	}
}














function fxDTSBrick::accessStorage(%brick, %dataID, %cl)
{
	if (getTrustLevel(%brick, %cl) < 2)
	{
		return;
	}

	if (isObject(%brick.vehicle.storageBot))
	{
		%storageObj = %brick.vehicle.storageBot;
		if (isObject(%pl = %cl.player) && isObject(%mount = %pl.getObjectMount())
			&& %mount == %storageObj.getObjectMount())
		{
			return;
		}
	}
	else if (isObject(%brick.vehicle) && %brick.vehicle.getDatablock().isStorageVehicle)
	{
		%storageObj = %brick.vehicle;
	}
	else
	{
		%storageObj = %brick;
	}
	
	//we need to close the menu they had open
	if(isEventPending(%cl.storageSchedule))
	{
		%cl.isInCenterprintMenu = false;
		storageLoop(%cl, %cl.lastStorageObj);
	}
	
	%brick.storageObj = %storageObj;

	%brick.updateStorageMenu(%dataID);
	%cl.startCenterprintMenu(%brick.centerprintMenu);

	%openDatablock = %storageObj.getDatablock().storageOpenDatablock;
	if (isObject(%openDatablock))
	{
		if (%brick.viewer[%cl.bl_id] != 1)
		{
			%brick.viewerCount++;
			%brick.viewer[%cl.bl_id] = 1;
		}

		if (%storageObj.getDatablock().getID() != %openDatablock.getID())
		{
			%storageObj.setDatablock(%openDatablock);
			%storageObj.playSound(brickChangeSound);
		}
	}

	storageLoop(%cl, %storageObj);
}
registerOutputEvent("fxDTSBrick", "accessStorage", "string 200 250", 1);

function storageLoop(%cl, %obj)
{
	cancel(%cl.storageSchedule);
	%exitDatablock = %obj.getDatablock().storageClosedDatablock;

	if (!isObject(%obj) || !isObject(%cl.player) || !%cl.isInCenterprintMenu)
	{
		%exit = 1;
	}

	if (vectorDist(%obj.getPosition(), %cl.player.getPosition()) > 6)
	{
		%exit = 1;
	}

	%start = %cl.player.getEyePoint();
	%end = vectorAdd(vectorScale(%cl.player.getEyeVector(), 8), %start);
	if (%obj.getType() & $Typemasks::fxBrickObjectType)
	{
		%type = $Typemasks::fxBrickObjectType;
	}
	else
	{
		%type = %obj.getType();
	}
	if (containerRaycast(%start, %end, %type, %cl.player) != %obj)
	{
		%exit = 1;
	}

	if (%exit)
	{
		%cl.exitCenterprintMenu();
		%obj.viewerCount--;
		%obj.viewer[%cl.bl_id] = 0;
		if (%obj.viewerCount == 0 && isObject(%exitDatablock) && %obj.getDatablock().getID() != %exitDatablock.getID())
		{
			%obj.setDatablock(%exitDatablock);
			%obj.playSound(brickPlantSound);
		}
		return;
	}

	%cl.displayCenterprintMenu(0);
	%cl.lastStorageObj = %obj;

	%cl.storageSchedule = schedule(200, %cl, storageLoop, %cl, %obj);
}

function addStorageEvent(%this, %botForm)
{
	if (%this.getGroup().isLoadingLot)
	{
		return;
	}
	
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
	%target = "Self";
	%outputEvent = "accessStorage";
	if (%param1 $= "")
	{
		%param1 = getRandomHash();
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

function dropStoredItems(%brick, %storageSlotCount)
{
	%dataID = %brick.eventOutputParameter0_1;
	for (%i = 0; %i < %storageSlotCount; %i++)
	{
		%storageSlot = %i + 1;
		%storageData = validateStorageValue(getDataIDArrayValue(%dataID, %storageSlot));
		%itemDB = getField(%storageData, 0);
		%storageCount = getField(%storageData, 2);
		%itemDataID = getField(%storageData, 3);

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
				count = mFloor(%amt);
				dataID = %itemDataID;
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
	}
	//allows other things to continue accessing the dataID array prior to brick death code completing
	schedule(1000, 0, deleteDataIDArray, %dataID);
}













function isStorageType(%typeName)
{
	return $StorageType[%typeName @ "Exists"] || false;
}

function storageTypeAccepts(%typeName, %storeable)
{
	if (%storeable $= %storeable + 0)
	{
		%storeable = %storeable.getName();
	}

	if (isObject(%storeable) && %storeable.isStackable)
	{
		%storeable = %storeable.stackType;
		%requiredStorageType = $Stackable_[%storeable, "requiredStorageType"];
	}
	else if (isObject(%storeable))
	{
		%requiredStorageType = %storeable.requiredStorageType;
	}
	else
	{
		return false;
	}

	if (isStorageType(%typeName))
	{
		if (strPos("\t" @ strLwr($StorageType[%typeName @ "List"]) @ "\t", "\t" @ strLwr(%storeable) @ "\t") != -1)
		{
			if (%requiredStorageType $= %typeName || !isStorageType(%requiredStorageType))
			{
				return true;
			}
		}
	}
	else if (!isStorageType(%requiredStorageType))
	{
		return true;
	}

	return false;
}

function storageTypeMatches(%typeName, %storeable)
{
	if (%storeable $= %storeable + 0)
	{
		%storeable = %storeable.getName();
	}

	if (isObject(%storeable) && %storeable.isStackable)
	{
		%storeable = %storeable.stackType;
	}

	if (!isStorageType(%typeName)) return false;

	return strPos("\t" @ strLwr($StorageType[%typeName @ "List"]) @ "\t", "\t" @ strLwr(%storeable) @ "\t") != -1;
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
		if (isObject(%client) && !(%client.isAdmin || %client.isBuilder))
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
		if (isObject(%b = %client.wrenchBrick) && %b.storageAddEvent == -1 && !%client.isBuilder)
		{
			return;
		}
		else
		{
			return parent::serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
		}
	}

	function ndTrustCheckSelect(%obj, %group2, %bl_id, %admin)
	{
		%db = %obj.getDatablock();
		if ((%db.isStorageBrick || %db.specialBrickType $= "VehicleSpawn") && !findClientByBL_ID(%bl_id).isBuilder)
		{
			return false;
		}
		return parent::ndTrustCheckSelect(%obj, %group2, %bl_id, %admin);
	}

	function fxDTSBrick::onDupCut(%this)
	{
		if (isObject(%this.centerprintMenu))
		{
			%this.centerprintMenu.delete();
		}

		if (%this.eventOutput0 !$= "accessStorage")
		{
			return;
		}

		%storageDataID = %this.eventOutputParameter0_1;
		if (%this.getDatablock().isStorageBrick
			|| getWord(getDataIDArrayValue(%storageDataID, 0), 0) $= "info")
		{
			%count = 4;
			if (%this.getDatablock().storageSlotCount > 0)
			{
				%count = %this.getDatablock().storageSlotCount;
			}
			if (!%this.droppedStoredItems)
			{
				%this.droppedStoredItems = 1;
				dropStoredItems(%this, %count);
			}
		}

		return parent::onDupCut(%this);
	}

	function fxDTSBrick::onDeath(%this)
	{
		%storageDataID = %this.eventOutputParameter0_1;
		if (%this.getDatablock().isStorageBrick
			|| (%this.eventOutput0 $= "accessStorage" && getWord(getDataIDArrayValue(%storageDataID, 0), 0) $= "info"))
		{
			%count = 4;
			if (%this.getDatablock().storageSlotCount > 0)
			{
				%count = %this.getDatablock().storageSlotCount;
			}
			if (!%this.droppedStoredItems)
			{
				%this.droppedStoredItems = 1;
				dropStoredItems(%this, %count);
			}
		}
		return parent::onDeath(%this);
	}

	function fxDTSBrick::onLoadPlant(%this)
	{
		%result = Parent::onLoadPlant(%this);

		if (%this.getDatablock().isStorageBrick)
		{
			%dataID = %this.eventOutputParameter[0, 1];
			if (%dataID !$= "")
			{
				%this.updateStorageDatablock(%dataID);
			}
		}

		if (isObject(%this.vehicle) && %this.vehicle.getDatablock().isStorageVehicle)
		{
			%dataID = %this.eventOutputParameter[0, 1];
			if (%dataID !$= "")
			{
				%this.vehicle.updateStorageDatablock(%dataID);
			}
		}

		return %result;
	}

	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player) && !%pl.isDying)
		{
			%item = %pl.tool[%slot];
			%start = %pl.getEyePoint();
			%end = vectorAdd(vectorScale(%pl.getEyeVector(), 6), %start);
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && %hit.getDatablock().isStorageBrick && getTrustLevel(%hit, %cl) >= 2)
			{
				%toolID = %pl.toolDataID[%slot];
				%success = %hit.insertIntoStorage(%hit.eventOutputParameter[0, 1],
												%item,
												!%pl.tool[%slot].isStackable ? 1 : %pl.toolStackCount[%slot],
												%toolID);

				if ((%success == 0 || %success == 1)
					&& %item.hasDataID && %item.isDataIDTool && %toolID !$= "")
				{
					logItemAction(%toolID, "stored", %cl.bl_id);
				}

				if (%success == 0) //complete insertion
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
				else if (%success == 1) //partial insertion
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
				else if (isObject(%item) && !%pl.isDying)
				{
					commandToClient(%cl, 'MessageBoxOk', "Storage Full", "Cannot insert this item!");
					return;
				}
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}
};
schedule(1000, 0, activatePackage, StorageBricks);
schedule(1100, 0, deactivatePackage, Processors);
schedule(1200, 0, activatePackage, Processors);
