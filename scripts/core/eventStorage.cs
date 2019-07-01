function validateStorageContents(%string, %brick)
{
	if (%string $= "")
	{
		return "";
	}

	%delimit = strPos(%string, "\"");
	%stackType = getSubStr(%string, 0, %delimit);
	%count = trim(getSubStr(%string, %delimit + 1, strLen(%string)));

	if ($Stackable_[%stackType, "stackedItemTotal"] $= "" && !isObject(%stackType))
	{
		return "";
	}

	return %stackType TAB %count;
}

function attemptStorage(%brick, %cl, %slot, %multiplier)
{
	if (!isObject(%cl.player) || getTrustLevel(%brick, %cl) < 2)
	{
		return 0;
	}

	%pl = %cl.player;
	%itemDB = %pl.tool[%slot];
	%itemCount = %pl.toolStackCount[%slot];
	%stackType = %itemDB.stackType;

	if (%multiplier $= "")
	{
		%multiplier = 1;
	}

	%storageMax = mCeil($StorageMax_[%stackType] * %multiplier);

	if (!isObject(%itemDB))
	{
		return;
	}

	if (%storageMax <= 0 && %itemDB.isStackable)
	{
		return 0;
	}

	//check for space on the storage brick
	for (%i = 1; %i < 5; %i++)
	{
		%string = %brick.eventOutputParameter[0, %i];

		if (%string $= "" && %empty $= "")
		{
			%empty = %i;
		}
		else if (%string !$= "" && %itemDB.isStackable)
		{
			%delimit = strPos(%string, "\"");
			%currStackType = getSubStr(%string, 0, %delimit);
			%count = trim(getSubStr(%string, %delimit + 1, strLen(%string)));
			if (%currStackType $= %stackType && %count < %storageMax)
			{
				%availableSpace[%spaces++ - 1] = %i SPC %storageMax - %count TAB %count;
				%totalAvailableSpace += %storageMax - %count;
				if (%totalAvailableSpace >= %itemCount)
				{
					break;
				}
			}
		}
	}

	if (%spaces > 0 && %itemDB.isStackable)
	{
		for (%i = 0; %i < %spaces; %i++)
		{
			%storageSlot = getWord(%availableSpace[%i], 0);
			%space = getWord(%availableSpace[%i], 1);
			%existingAmt = getField(%availableSpace[%i], 1);
			%amt = getMin(%space, %itemCount);

			%brick.eventOutputParameter[0, %storageSlot] = %stackType @ "\"" @ %existingAmt + %amt;
			%itemCount -= %amt;

			if (%itemCount <= 0)
			{
				%pl.tool[%slot] = "";
				%pl.toolStackCount[%slot] = 0;
				messageClient(%cl, 'MsgItemPickup', '', %slot, 0);
				if (%pl.currTool == %slot)
				{
					%pl.unMountImage(0);
				}

				//update centerprint menu
				%brick.updateCenterprintMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);

				return 1;
			}
		}
	}

	if (%empty !$= "" && %itemCount > 0 && %itemDB.isStackable)
	{
		%amt = %itemCount; //assume that the amount a slot can take is always more than the max of a stack
		%brick.eventOutputParameter[0, %empty] = %stacktype @ "\"" @ %amt;

		%pl.tool[%slot] = "";
		%pl.toolStackCount[%slot] = 0;
		messageClient(%cl, 'MsgItemPickup', '', %slot, 0);
		if (%pl.currTool == %slot)
		{
			%pl.unMountImage(0);
		}
		
		//update centerprint menu
		%brick.updateCenterprintMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);

		return 1;
	}
	else if (%empty !$= "" && !%itemDB.isStackable)
	{
		%brick.eventOutputParameter[0, %empty] = %itemDB.getName() @ "\"" @ %pl.deliveryPackageInfo[%slot];
		
		//just stored a non-stackable item
		messageClient(%pl.client, 'MsgItemPickup', '', %slot, 0);
		%pl.tool[%slot] = 0;
		%pl.deliveryPackageInfo[%slot] = "";
		serverCmdUnuseTool(%pl.client);

		%brick.updateCenterprintMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);

		return 1;
	}

	if (%itemDB.isStackable)
	{
		%pl.toolStackCount[%slot] = %itemCount;
		for (%i = 0; %i < $Stackable_[%stackType, "stackedItemTotal"]; %i++)
		{
			%currPair = $Stackable_[%stackType, "stackedItem" @ %i];
			// talk(%currPair);
			if (%pl.toolStackCount[%slot] <= getWord(%currPair, 1))
			{
				%bestItem = getWord(%currPair, 0);
				break;
			}
		}

	// talk(%bestItem.getID() @ " vs " @ %pl.tool[%slot]);
		if (!isObject(%bestItem))
		{
			talk("ERROR: BestItem not found! (attemptStorage) " @ %pl.client.name SPC %item SPC %slot SPC %amt);
			return;
		}
		if (%bestItem.getID() != %pl.tool[%slot])
		{
			%pl.tool[%slot] = %bestItem.getID();
			messageClient(%pl.client, 'MsgItemPickup', '', %slot, %bestItem.getID());

			if (%pl.currTool == %slot)
			{
				%pl.mountImage(%bestItem.image, 0);
			}
		}
		else
		{
			messageClient(%pl.client, 'MsgItemPickup', '');
		}
	}

	//update centerprint menu
	%brick.updateCenterprintMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);

	return 1;
}

function fxDTSBrick::displayStorageContents(%this, %str1, %str2, %str3, %str4, %cl)
{
	if (getTrustLevel(%cl, %this) < 2)
	{
		if (getBrickgroupFromObject(%this).bl_id != 888888)
		{
			%cl.centerprint(%this.getGroup().name @ " does not trust you enough to do that!", 1);
		}
		return;
	}

	%this.updateCenterprintMenu(%str1, %str2, %str3, %str4);

	%cl.startCenterprintMenu(%this.centerprintMenu);

	if (isObject(%this.vehicle) && %this.vehicle.getDatablock().isStorageCart)
	{
		%this.centerprintMenu.menuName = "Storage Cart";
		cartStorageLoop(%cl, %this.vehicle);
	}
	else
	{
		storageLoop(%cl, %this);
	}
}

function fxDTSBrick::updateCenterprintMenu(%this, %str1, %str2, %str3, %str4)
{
	for (%i = 1; %i < 5; %i++)
	{
		%str[%i] = validateStorageContents(%str[%i], %this);
	}

	if (!isObject(%this.centerprintMenu))
	{
		%this.centerprintMenu = new ScriptObject(StorageCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = %this.getDatablock().uiName;

			menuOption[0] = "Empty";
			menuOption[1] = "Empty";
			menuOption[2] = "Empty";
			menuOption[3] = "Empty";

			menuFunction[0] = "removeStack";
			menuFunction[1] = "removeStack";
			menuFunction[2] = "removeStack";
			menuFunction[3] = "removeStack";

			menuOptionCount = 4;
			brick = %this;
		};
		MissionCleanup.add(%this.centerprintMenu);

		//%cl.currOption
	}

	for (%i = 0; %i < 4; %i++)
	{
		%stackType = getField(%str[%i + 1], 0);
		%count = getField(%str[%i + 1], 1);
		if (%stackType !$= "")
		{
			if (!isObject(%stackType))
			{
				%this.centerprintMenu.menuOption[%i] = %stackType @ " - " @ %count;
			}
			else
			{
				%this.centerprintMenu.menuOption[%i] = strUpr(getSubStr(%stackType.uiName, 0, 1)) @ getSubStr(%stackType.uiName, 1, 100);
			}
		}
		else
		{
			%this.centerprintMenu.menuOption[%i] = "Empty";	
		}
	}
}

registerOutputEvent("fxDTSBrick", "displayStorageContents", "string 200 50" TAB "string 200 50" TAB "string 200 50" TAB "string 200 50", 1);

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

function cartStorageLoop(%cl, %bot)
{
	cancel(%cl.storageSchedule);

	if (!isObject(%bot) || !isObject(%cl.player) || !%cl.isInCenterprintMenu)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	if (vectorDist(%bot.getPosition(), %cl.player.getPosition()) > 6)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	%start = %cl.player.getEyePoint();
	%end = vectorAdd(vectorScale(%cl.player.getEyeVector(), 8), %start);
	if (containerRaycast(%start, %end, $Typemasks::PlayerObjectType, %cl.player) != %bot)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	%cl.displayCenterprintMenu(0);

	%cl.storageSchedule = schedule(200, %cl, cartStorageLoop, %cl, %bot);
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

		%brick.displayStorageContents(%brick.eventOutputParameter0_1, %brick.eventOutputParameter0_2, %brick.eventOutputParameter0_3, %brick.eventOutputParameter0_4, %cl);
		%cl.displayCenterprintMenu(%option);
		return;
	}

	%storageSlot = %option + 1;
	%storageData = validateStorageContents(%brick.eventOutputParameter[0, %storageSlot], %brick);
	%storageCount = getField(%storageData, 1);
	%stackType = getField(%storageData, 0);

	if (!isObject(%stackType))
	{
		%max = $Stackable_[%stackType, "stackedItemTotal"] - 1;
		%max = getWord($Stackable_[%stackType, "stackedItem" @ %max], 1);

		%amt = getMin(%max, %storageCount);
		%left = %storageCount - %amt;
		if (%left > 0)
		{
			%brick.eventOutputParameter[0, %storageSlot] = %stackType @ "\"" @ %left;
		}
		else
		{
			%brick.eventOutputParameter[0, %storageSlot] = "";
		}
	}

	if (!isObject(%stackType))
	{
		%itemDB = getWord($Stackable_[%stackType, "stackedItem0"], 0);
	}
	else
	{
		%itemDB = %stackType;
		%packageInfo = getSubStr(%storageData, strPos(%storageData, "\"") + 1, strLen(%storageData));
		%brick.eventOutputParameter[0, %storageSlot] = "";
	}


	//update centerprint menu
	%brick.updateCenterprintMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
	
	%i = new Item()
	{
		dataBlock = %itemDB;
		count = %amt;
		deliveryPackageInfo = %packageInfo;
		harvestedBG = getBrickgroupFromObject(%brick);
	};
	MissionCleanup.add(%i);
	%i.setTransform(%cl.player.getTransform());

	%brick.displayStorageContents(%brick.eventOutputParameter0_1, %brick.eventOutputParameter0_2, %brick.eventOutputParameter0_3, %brick.eventOutputParameter0_4, %cl);
	%cl.displayCenterprintMenu(%option);
}

function dropStoredItems(%brick)
{
	for (%i = 0; %i < 4; %i++)
	{
		%storageSlot = %i + 1;
		%storageData = validateStorageContents(%brick.eventOutputParameter[0, %storageSlot], %brick);
		%storageCount = getField(%storageData, 1);
		%stackType = getField(%storageData, 0);
		%packageInfo = getSubStr(%storageData, strPos(%storageData, "\"") + 1, strLen(%storageData));

		%max = $Stackable_[%stackType, "stackedItemTotal"] - 1;

		%amt = %storageCount;
		// talk("stackType: " @ %stackType @ " max: " @ %max @ " | " @ $Stackable_[%stackType, "stackedItem" @ %max]);

		if (!isObject(%stackType))
		{
			%itemDB = getWord($Stackable_[%stackType, "stackedItem" @ %max], 0);
		}
		else
		{
			%itemDB = %stackType;
		}
		if (!isObject(%itemDB))
		{
			continue;
		}
		%item = new Item()
		{
			dataBlock = %itemDB;
			count = %amt;
			deliveryPackageInfo = %packageInfo;
			sourceClient = getBrickgroupFromObject(%brick).client;
			sourceBrickgrou = getBrickgroupFromObject(%brick).client;
		};
		MissionCleanup.add(%item);
		%vel = (getRandom(6) - 3) / 2 SPC  (getRandom(6) - 3) / 2 SPC 6;
		%item.setTransform(%brick.getPosition());
		%item.setVelocity(%vel);
		%item.schedule(60000, schedulePop);
	}
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
	%outputEvent = "displayStorageContents";

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
			// if (%item.isStackable)
			// {
				%start = %pl.getEyePoint();
				%end = vectorAdd(vectorScale(%pl.getEyeVector(), 6), %start);
				%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
				if (isObject(%hit) && %hit.getDatablock().isStorageBrick)
				{
					%success = attemptStorage(%hit, %cl, %slot, %hit.getDatablock().storageBonus);
					if (%success)
					{
						return;
					}
				}
			// }
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}
};
schedule(1000, 0, activatePackage, StorageBricks);
