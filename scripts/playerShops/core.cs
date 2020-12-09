package PlayerShops
{
	function fxDTSBrick::accessStorage(%brick, %dataID, %cl)
	{
		if (isObject(%brick.vehicle.storageBot))
		{
			%storageObj = %brick.vehicle.storageBot;
		}
		else if (isObject(%brick.vehicle) && %brick.vehicle.getDatablock().isStorageVehicle)
		{
			%storageObj = %brick.vehicle;
		}
		else
		{
			%storageObj = %brick;
		}
		%brick.storageObj = %storageObj;

		if (%storageObj.getDatablock().isShop)
		{
			%brick.updateShopMenus();

			if (getTrustLevel(%brick, %cl) >= 2)
			{
				%cl.startCenterprintMenu(%brick.shopStorageMenu);
			}
			else
			{
				%cl.startCenterprintMenu(%brick.shopBuyMenu);
			}

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
		else
		{
			return Parent::accessStorage(%brick, %dataID, %cl);
		}
	}

	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID)
	{
		if (!isObject(%storageObj) || !%storageObj.getDatablock().isShop) return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);

		%ret = parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);

		if (%ret == 2) return %ret; // if insertion fails, no need to update shop menus

		if (%storeItemDB.isStackable)
		{
			%pricePrefix = %storeItemDB.stackType;
		}
		else
		{
			%pricePrefix = %storeItemDB.getName();
		}
		%price[%count] = getDataIDArrayTagValue(%dataID, %pricePrefix @ "Price");

		if (%price $= "")
		{
			if (storageTypeMatches("Crops", %storeItemDB))
			{
				%price = mFloatLength(getSellPrice(%storeItemDB) * 2, 2);
			}
			else
			{
				%price = mFloatLength(getSellPrice(%storeItemDB) * 1.5, 2);
			}
			%price = getMax(%price, 0.01);
			setDataIDArrayTagValue(%dataID, %pricePrefix @ "Price", mFloatLength(%price, 2));
		}

		%brick.updateShopMenus();

		return %ret;
	}

	function removeStack(%cl, %menu, %option)
	{
		%storageObj = %menu.brick.storageObj;

		if (!isObject(%storageObj) || !%storageObj.getDatablock().isShop) return parent::removeStack(%cl, %menu, %option);

		%ret = parent::removeStack(%cl, %menu, %option);

		if (%ret == 2) return %ret; // if removal fails, no need to update shop menus

		%menu.brick.updateShopMenus();

		return %ret;
	}

	function serverCmdShiftBrick(%cl, %x, %y, %z)
	{
		if (%cl.isInCenterprintMenu && %cl.centerprintMenu == %cl.centerprintMenu.brick.shopStorageMenu && %z != 0)
		{
			%delta = ((%z > 0) ? 1 : -1) * ((mAbs(%z) > 1) ? 1 : 0.01);
			%storageField = %cl.currOption + 1;
			%brick = %cl.centerprintMenu.brick;
			%dataID = %brick.eventOutputParameter[0, 1];

			%entry = validateStorageValue(getDataIDArrayValue(%dataID, %storageField));
			%dataBlock = getField(%entry, 0);
			if (%dataBlock.isStackable)
			{
				%pricePrefix = %dataBlock.stackType;
			}
			else
			{
				%pricePrefix = %dataBlock.getName();
			}
			%price[%count] = getDataIDArrayTagValue(%dataID, %pricePrefix @ "Price");

			%price += %delta;
			if (%price < 0.01) %price = 0.01;

			setDataIDArrayTagValue(%dataID, %pricePrefix @ "Price", mFloatLength(%price, 2));

			%brick.updateShopMenus();

			%option = %cl.currOption;
			%cl.startCenterprintMenu(%cl.centerprintMenu);
			%cl.displayCenterprintMenu(%option);

			return;
		}
		return Parent::serverCmdShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdSuperShiftBrick(%cl, %x, %y, %z)
	{
		if (%cl.isInCenterprintMenu && %cl.centerprintMenu.brick.storageObj.getDatablock().isShop && %cl.centerprintMenu == %cl.centerprintMenu.brick.shopStorageMenu && %z != 0) {
			%delta = ((%z > 0) ? 1 : -1) * 10;
			%storageField = %cl.currOption + 1;
			%brick = %cl.centerprintMenu.brick;
			%dataID = %brick.eventOutputParameter[0, 1];

			%entry = validateStorageValue(getDataIDArrayValue(%dataID, %storageField));
			%dataBlock = getField(%entry, 0);
			if (%dataBlock.isStackable)
			{
				%pricePrefix = %dataBlock.stackType;
			}
			else
			{
				%pricePrefix = %dataBlock.getName();
			}
			%price[%count] = getDataIDArrayTagValue(%dataID, %pricePrefix @ "Price");

			%price += %delta;
			if (%price < 0.1) %price = 0.1;

			setDataIDArrayTagValue(%dataID, %pricePrefix @ "Price", mFloatLength(%price, 2));

			%brick.updateShopMenus();

			%option = %cl.currOption;
			%cl.startCenterprintMenu(%cl.centerprintMenu);
			%cl.displayCenterprintMenu(%option);

			return;
		}
		return Parent::serverCmdSuperShiftBrick(%cl, %x, %y, %z);
	}

	function fxDTSBrick::onDeath(%this, %obj)
	{
		if (isObject(%this.storageObj))
		{
			if (%this.storageObj.getDatablock().isShop)
			{
				for (%i = 0; %i < 4; %i++)
				{
					if (isObject(%this.shopDisplayItem[%i]))
					{
						%this.shopDisplayItem[%i].delete();
					}
				}
			}
		}
		return parent::onDeath(%this, %obj);
	}

	function fxDTSBrick::onRemove(%this, %obj)
	{
		if (isObject(%this.storageObj))
		{
			if (%this.storageObj.getDatablock().isShop)
			{
				for (%i = 0; %i < 4; %i++)
				{
					if (isObject(%this.shopDisplayItem[%i]))
					{
						%this.shopDisplayItem[%i].delete();
					}
				}
			}
		}
		return parent::onRemove(%this, %obj);
	}

	function fxDTSBrick::onAdd(%this, %obj)
	{
		if (isObject(%this.storageObj))
		{
			if (%this.storageObj.getDatablock().isShop)
			{
				%this.schedule(1000, updateShopMenus);
			}
		}
		return parent::onAdd(%this, %obj);
	}

	function Armor::onCollision(%this, %obj, %col, %vec, %speed)
	{
		if (%col.getClassName() $= "Item" && %col.isShopItem)
		{
			return;
		}
		return parent::onCollision(%this, %obj, %col, %vec, %speed);
	}
};
activatePackage(PlayerShops);

function fxDTSBrick::updateShopMenus(%brick)
{
	if (isObject(%brick.vehicle.storageBot))
	{
		%storageObj = %brick.vehicle.storageBot;
	}
	else if (isObject(%brick.vehicle) && %brick.vehicle.getDatablock().isStorageVehicle)
	{
		%storageObj = %brick.vehicle;
	}
	else
	{
		%storageObj = %brick;
	}
	%brick.storageObj = %storageObj;

	%dataID = %brick.eventOutputParameter[0, 1];

	//get storage data
	%max = %storageObj.getDatablock().storageSlotCount;
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%dataBlock = getField(%data[%count], 0);
		if (isObject(%dataBlock))
		{
			if (%dataBlock.isStackable)
			{
				%price[%count] = getDataIDArrayTagValue(%dataID, %dataBlock.stackType @ "Price");
			}
			else
			{
				%price[%count] = getDataIDArrayTagValue(%dataID, %dataBlock.getName() @ "Price");
			}
		}
		%count++;
	}

	%shopBuyMenu = %brick.shopBuyMenu;
	%shopStorageMenu = %brick.shopStorageMenu;

	if (!isObject(%shopBuyMenu))
	{
		%shopBuyMenu = new ScriptObject(ShopCenterprintMenu)
		{
			isCenterprintMenu = 1;
			menuName = getBrickgroupFromObject(%brick).name @ "'s Shop";

			menuOptionCount = %count;

			brick = %brick;
		};
		MissionCleanup.add(%shopBuyMenu);

		for (%i = 0; %i < %count; %i++)
		{
			%shopBuyMenu.menuOption[%i] = "Empty";
		}

		for (%i = 0; %i < %count; %i++)
		{
			%shopBuyMenu.menuFunction[%i] = "buyUnit";
		}

		%brick.shopBuyMenu = %shopBuyMenu;
	}

	if (!isObject(%shopStorageMenu))
	{
		%shopStorageMenu = new ScriptObject(ShopCenterprintMenu)
		{
			isCenterprintMenu = 1;
			menuName = getBrickgroupFromObject(%brick).name @ "'s Shop";

			menuOptionCount = %count + 1;

			storageDataID = %dataID;
			brick = %brick;
		};
		MissionCleanup.add(%shopStorageMenu);

		for (%i = 0; %i < %count; %i++)
		{
			%shopStorageMenu.menuOption[%i] = "Empty";
		}
		%shopStorageMenu.menuOption[%i] = "$0 - Last withdrawal: None";

		for (%i = 0; %i < %count; %i++)
		{
			%shopStorageMenu.menuFunction[%i] = "removeStack";
		}
		%shopStorageMenu.menuFunction[%i] = "removeMoney";

		%brick.shopStorageMenu = %shopStorageMenu;
	}

	for (%i = 0; %i < %count; %i++)
	{
		%dataBlock = getField(%data[%i], 0);
		%displayName = getField(%data[%i], 1);
		%itemCount = getField(%data[%i], 2);
		%price = %price[%i];
		if (isObject(%dataBlock))
		{
			%brick.shopStorageMenu.menuOption[%i] = "$" @ %price @ ": " @ %displayName @ " - " @ %itemCount;
			%brick.shopBuyMenu.menuOption[%i] = "$" @ %price @ ": " @ %displayName @ " - " @ %itemCount;
		}
		else
		{
			%brick.shopStorageMenu.menuOption[%i] = "Empty";
			%brick.shopBuyMenu.menuOption[%i] = "Empty";
		}
	}

	%moneyStored = getDataIDArrayTagValue(%dataID, "moneyStored");
	%lastWithdrawer = getDataIDArrayTagValue(%dataID, "lastWithdrawer");

	%brick.shopStorageMenu.menuOption[%count] = "$" @ mFloatLength(%moneyStored, 2) @ " - Last withdrawal: " @ (%lastWithdrawer $= "" ? "None" : %lastWithdrawer);

	%brick.updateShopDisplay();
}

function fxDTSBrick::updateShopDisplay(%brick)
{
	if (!%brick.storageObj.getDatablock().isShop)
	{
		return;
	}

	%dataID = %brick.eventOutputParameter[0, 1];
	%storageObj = %brick.storageObj;

	//get storage data
	%max = %storageObj.getDatablock().storageSlotCount;
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%dataBlock = getField(%data[%count], 0);
		if (isObject(%dataBlock))
		{
			if (%dataBlock.isStackable)
			{
				%price[%count] = getDataIDArrayTagValue(%dataID, %dataBlock.stackType @ "Price");
			}
			else
			{
				%price[%count] = getDataIDArrayTagValue(%dataID, %dataBlock.getName() @ "Price");
			}
		}
		%count++;
	}

	%rotation = getWords(%brick.getTransform(), 3, 6); // TODO: everything below this point assumes a brick. make it not assume a brick.
	%rotation = getWords(%rotation, 0, 2) SPC (getWord(%rotation, 3) + $pi);
	for (%i = 0; %i < %count; %i++)
	{
		%currPos = %brick.getDatablock().itemPos[%i];
		switch(%brick.angleID)
		{
			case 0: %currPos = %currPos;
			case 1: %currPos = getWord(%currPos, 1) SPC -1 * getWord(%currPos, 0) SPC getWord(%currPos, 2);
			case 2: %currPos = -1 * getWord(%currPos, 0) SPC -1 * getWord(%currPos, 1) SPC getWord(%currPos, 2);
			case 3: %currPos = -1 * getWord(%currPos, 1) SPC getWord(%currPos, 0) SPC getWord(%currPos, 2);
		}
		%currPos = vectorAdd(%brick.getPosition(), %currPos);
		// %p = createBoxAt(%currPos, "0 0 1 1", 0.1);
		// %p.schedule(1000, delete);
		%dataBlock = getField(%data[%i], 0);
		%count = getField(%data[%i], 2);
		%dataID = getField(%data[%i], 3);

		if (isObject(%dataBlock))
		{
			%item = %brick.shopDisplayItem[%i];
			if (!isObject(%item))
			{
				%item = %brick.shopDisplayItem[%i] = new Item(ShopDisplayItems)
				{
					dataBlock = %dataBlock;
					static = 1;
					isShopItem = 1;
				};
			}
			else
			{
				%brick.shopDisplayItem[%i].setDatablock(%dataBlock);
			}

			if (%dataBlock.doColorShift)
			{
				%item.setNodeColor("ALL", %dataBlock.colorShiftColor);
			}

			%height = %item.getWorldBox();
			%height = getWord(%height, 5) - getWord(%height, 2);

			switch$ (%item.getDatablock().uiName)
			{
				case "Hammer ": %height = %height - 0.6;
				case "Wrench": %height = %height - 0.8;
				case "Printer": %height = %height - 0.4;
				default: %height = %height;
			}

			%offset = %item.getWorldBox();
			%center = vectorScale(vectorAdd(getWords(%offset, 0, 1), getWords(%offset, 3, 4)), 0.5);
			%offset = getWords(vectorSub(%item.getTransform(), %center), 0, 1) SPC %height;
			%item.setTransform(vectorAdd(%currPos, %offset) SPC %rotation);
		}
		else
		{
			if (isObject(%brick.shopDisplayItem[%i]))
			{
				%brick.shopDisplayItem[%i].delete();
			}
		}
	}
}

function removeMoney(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (getTrustLevel(%cl, %brick) < 2)
	{
		return;
	}

	%dataID = %brick.eventOutputParameter[0, 1];

	%moneyStored = getDataIDArrayTagValue(%dataID, "moneyStored");
	if (%moneyStored == 0)
	{
		%cl.startCenterprintMenu(%menu);
		%cl.displayCenterprintMenu(%option);
		return;
	}

	setDataIDArrayTagValue(%dataID, "moneyStored", 0);

	%cl.incScore(%moneyStored);
	setDataIDArrayTagValue(%dataID, "lastWithdrawer", %cl.name SPC "(BL_ID " @ %cl.bl_id @ ")");

	%brick.updateShopMenus();

	%cl.startCenterprintMenu(%menu);
	%cl.displayCenterprintMenu(%option);
}

function fxDTSBrick::storeMoney(%brick, %amount)
{
	if (%amount <= 0) return; // use removeMoney if you want to remove any money >:(

	%dataID = %brick.eventOutputParameter[0, 1];

	%moneyStored = getDataIDArrayTagValue(%dataID, "moneyStored");
	setDataIDArrayTagValue(%dataID, "moneyStored", %moneyStored + %amount);

	%brick.updateShopMenus();
}

function buyUnit(%cl, %menu, %option)
{
	%pl = %cl.player;
	if (!isObject(%pl))
	{
		%cl.centerprint("You are dead!", 1);
		return;
	}

	%brick = %menu.brick;

	if (!isObject(%brick.vehicle) && vectorDist(%brick.getPosition(), %pl.getHackPosition()) > 7)
	{
		%cl.centerprint("You are too far away from the shop!", 1);
		return;
	}
	else if (isObject(%brick.vehicle) && vectorDist(%brick.vehicle.getPosition(), %pl.getHackPosition()) > 7)
	{
		%cl.centerprint("You are too far away from the shop!", 1);
		return;
	}

	%dataID = %brick.eventOutputParameter[0, 1];

	if (%menu.menuOption[%option] $= "Empty")
	{
		if (%cl.nextMessageEmpty < $Sim::Time)
		{
			messageClient(%cl, '', "That slot is empty!", 1);
		}

		%cl.nextMessageEmpty = $Sim::Time + 2;

		%cl.startCenterprintMenu(%menu);
		%cl.displayCenterprintMenu(%option);
		return;
	}

	%storageSlot = %option + 1;
	%entry = validateStorageValue(getDataIDArrayValue(%dataID, %storageSlot));
	%dataBlock = getField(%entry, 0);
	%displayName = getField(%entry, 1);
	%storageCount = getField(%entry, 2);
	if (%dataBlock.isStackable)
	{
		%price[%count] = getDataIDArrayTagValue(%dataID, %dataBlock.stackType @ "Price");
	}
	else
	{
		%price[%count] = getDataIDArrayTagValue(%dataID, %dataBlock.getName() @ "Price");
	}

	if (%cl.score < %price)
	{
		messageClient(%cl, '', "You can't afford this!", 1);

		%cl.startCenterprintMenu(%menu);
		%cl.displayCenterprintMenu(%option);

		return;
	}

	%cl.incScore(-%price);
	purchasedMessageSchedule(%cl, %dataBlock, %displayName, 1, %price);
	%brick.storeMoney(%price);

	// remove item
	%left = %storageCount - 1;

	if (%left > 0)
	{
		setDataIDArrayValue(%dataID, %storageSlot, getStorageValue(%dataBlock, %left));
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

	%brick.updateShopMenus();

	if (%storageObj.getDatablock().hasStorageNodes)
	{
		%storageObj.updateStorageNodes(%dataID);
	}

	if (isObject(%storageObj))
	{
		%storageObj.updateStorageDatablock(%dataID, true);
	}

	if (%dataBlock.isStackable)
	{
		%pl.farmingAddStackableItem(%dataBlock, 1);
	}
	else
	{
		%itemDataID = getField(%entry, 3);
		%pl.farmingAddItem(%dataBlock, %itemDataID);
	}

	%cl.startCenterprintMenu(%menu);
	%cl.displayCenterprintMenu(%option);
}

function purchasedMessageSchedule(%cl, %dataBlock, %displayName, %count, %amount)
{
	cancel(%cl.shopPurchaseSched[%dataBlock]);

	%cl.shopPurchaseSched[%dataBlock] = schedule(1000, %cl, messagePurchaseTotal, %cl, %dataBlock, %displayName);
	%cl.shopPurchaseCount[%dataBlock] += %count;
	%cl.shopPurchaseAmount[%dataBlock] += %amount;
}

function messagePurchaseTotal(%cl, %dataBlock, %displayName)
{
	cancel(%cl.shopPurchaseSched[%dataBlock]);

	%count = %cl.shopPurchaseCount[%dataBlock];
	%amount = %cl.shopPurchaseAmount[%dataBlock];
	messageClient(%cl, '', "\c6You purchased \c6" @ %count @ " " @ %displayName @ " for \c0$" @ mFloatLength(%amount, 2));

	%cl.shopPurchaseCount[%dataBlock] = "";
	%cl.shopPurchaseAmount[%dataBlock] = "";
	%cl.shopPurchaseSched[%dataBlock] = "";
}