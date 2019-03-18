//shop owners auto-sell products at base market price + $1
//full trust users open the entire menu: all 4 storage slots + money. doesn't cost anything to take stuff out
//	money will show who last took out money, and how much. money would auto-remove all
//build trust and less users open the shop menu, where they can buy the items one at a time

//core is loaded first so we can use eventstorage functions
//in fact, we can just package the base event function displayStorageContents
//money left in shop brick will be saved in the brick name

package PlayerShops
{
	function fxDTSBrick::displayStorageContents(%this, %str1, %str2, %str3, %str4, %cl)
	{
		if (%this.getDatablock().isShopBrick)
		{
			%this.updateShopMenus(%str1, %str2, %str3, %str4);

			if (getTrustLevel(%cl, %this) < 2)
			{
				if (getBrickgroupFromObject(%this).bl_id != 888888)
				{
					//they're trying to buy stuff!!
					//open buy menu here
					%cl.startCenterprintMenu(%this.shopBuyerMenu);
					storageLoop(%cl, %this);
				}
				return;
			}

			%cl.startCenterprintMenu(%this.shopStorageMenu);

			//potential here for mobile shops!!!!!
			// if (isObject(%this.vehicle) && %this.vehicle.getDatablock().isStorageCart)
			// {
			// 	%this.shopStorageMenu.menuName = "Storage Cart";
			// 	cartStorageLoop(%cl, %this.vehicle);
			// }
			// else
			// {
			storageLoop(%cl, %this);
			// }
		}
		else
		{
			return parent::displayStorageContents(%this, %str1, %str2, %str3, %str4, %cl);
		}
	}

	function fxDTSBrick::setNTObjectName(%obj, %name)
	{
		if (%obj.getDatablock().isShopBrick && %obj.settingName != 1)
		{
			return 0;
		}
		%obj.settingName = 0;
		return parent::setNTObjectName(%obj, %name);
	}

	function attemptStorage(%brick, %cl, %slot, %multiplier)
	{
		%ret = parent::attemptStorage(%brick, %cl, %slot, %multiplier);
		
		if (%ret && %brick.getDatablock().isShopBrick)
		{
			%brick.updateShopMenus(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
			%brick.updateShopDisplay();
		}
		return %ret;
	}

	function fxDTSBrick::onDeath(%this, %obj)
	{
		if (%this.getDatablock().isShopBrick)
		{
			for (%i = 0; %i < 4; %i++)
			{
				if (isObject(%this.shopDisplayItem[%i]))
				{
					%this.shopDisplayItem[%i].delete();
				}
			}
		}
		return parent::onDeath(%this, %obj);
	}

	function fxDTSBrick::onRemove(%this, %obj)
	{
		if (%this.getDatablock().isShopBrick)
		{
			for (%i = 0; %i < 4; %i++)
			{
				if (isObject(%this.shopDisplayItem[%i]))
				{
					%this.shopDisplayItem[%i].delete();
				}
			}
		}
		return parent::onRemove(%this, %obj);
	}

	function fxDTSBrick::onAdd(%this, %obj)
	{
		if (%this.getDatablock().isShopBrick)
		{
			%this.schedule(1000, updateShopDisplay);
		}
		return parent::onAdd(%this, %obj);
	}

	function removeStack(%cl, %menu, %option)
	{
		%ret = parent::removeStack(%cl, %menu, %option);

		if (isObject(%menu.brick) && %menu.brick.getDatablock().isShopBrick)
		{
			%menu.brick.updateShopDisplay();
		}
	}
};
activatePackage(PlayerShops);


function fxDTSBrick::updateShopMenus(%this, %str1, %str2, %str3, %str4)
{
	for (%i = 1; %i < 5; %i++)
	{
		%str[%i] = validateStorageContents(%str[%i], %this);
	}

	if (!isObject(%this.shopStorageMenu))
	{
		%this.shopStorageMenu = new ScriptObject(ShopCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = "Shop Manager";

			menuOption[0] = "Empty";
			menuOption[1] = "Empty";
			menuOption[2] = "Empty";
			menuOption[3] = "Empty";
			menuOption[4] = "$0 - None";

			menuFunction[0] = "removeStack";
			menuFunction[1] = "removeStack";
			menuFunction[2] = "removeStack";
			menuFunction[3] = "removeStack";
			menuFunction[4] = "removeMoney";

			menuOptionCount = 5;
			brick = %this;
		};
		MissionCleanup.add(%this.shopStorageMenu);

		//%cl.currOption
	}
	if (!isObject(%this.shopBuyerMenu))
	{
		%this.shopBuyerMenu = new ScriptObject(ShopCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = getBrickgroupFromObject(%this).name @ "'s Shop";

			menuOption[0] = "Empty";
			menuOption[1] = "Empty";
			menuOption[2] = "Empty";
			menuOption[3] = "Empty";

			menuFunction[0] = "buyUnit";
			menuFunction[1] = "buyUnit";
			menuFunction[2] = "buyUnit";
			menuFunction[3] = "buyUnit";

			menuOptionCount = 4;
			brick = %this;
		};
		MissionCleanup.add(%this.shopBuyerMenu);
	}

	for (%i = 0; %i < 4; %i++)
	{
		%stackType = getField(%str[%i + 1], 0);
		%count = getField(%str[%i + 1], 1);
		if (%stackType !$= "")
		{
			if (!isObject(%stackType)) //this is a stackable item
			{
				%price = mFloatLength($Produce::BuyCost_[%stackType] + 1, 2);
				%this.shopStorageMenu.menuOption[%i] = "$" @ %price @ ": " @ %stackType @ " - " @ %count;
				%this.shopBuyerMenu.menuOption[%i] = "$" @ %price @ ": " @ %stackType @ " - " @ %count;
			}
			else //this is a normal item
			{
				%price = mCeil(%stackType.cost * 1.2);
				if (%price <= 10)
				{
					%price = 10;
				}
				%this.shopStorageMenu.menuOption[%i] = "$" @ %price @ ": " @ strUpr(getSubStr(%stackType.uiName, 0, 1)) @ getSubStr(%stackType.uiName, 1, 100);
				%this.shopBuyerMenu.menuOption[%i] = "$" @ %price @ ": " @ strUpr(getSubStr(%stackType.uiName, 0, 1)) @ getSubStr(%stackType.uiName, 1, 100);
			}
		}
		else
		{
			%this.shopStorageMenu.menuOption[%i] = "Empty";	
			%this.shopBuyerMenu.menuOption[%i] = "Empty";	
		}
	}

	%brickName = getSubStr(%this.getName(), 1, 64);
	%money = getSubStr(%brickName, 0, strPos(%brickName, "_"));
	%lastTakenBy = trim(getSubStr(%brickName, strPos(%brickName, "_") + 1, 30));
	if (%lastTakenBy $= "")
	{
		%lastTakenBy = "None";
	}
	%this.shopStorageMenu.menuOption[4] = "$" @ %money @ " - " @ %lastTakenBy;

	%this.updateShopDisplay();
}

//angleID 0
//1 +-0.7 0
//0 +- 0.7 0.4

function fxDTSBrick::updateShopDisplay(%this)
{
	if (!%this.getDatablock().isShopBrick)
	{
		return;
	}

	for (%i = 1; %i < 5; %i++)
	{
		%str[%i - 1] = validateStorageContents(%this.eventOutputParameter[0,%i], %this);
	}

	%rotation = getWords(%this.getTransform(), 3, 6);

	for (%i = 0; %i < 4; %i++)
	{
		%currPos = %this.getDatablock().itemPos[%i];
		switch(%this.angleID)
		{
			case 0: %currPos = %currPos;
			case 1: %currPos = getWord(%currPos, 1) SPC  -1 * getWord(%currPos, 0) SPC getWord(%currPos, 2);
			case 2: %currPos = -1 * getWord(%currPos, 0) SPC -1 * getWord(%currPos, 1) SPC getWord(%currPos, 2);
			case 3: %currPos = -1 * getWord(%currPos, 1) SPC getWord(%currPos, 0) SPC getWord(%currPos, 2);
		}
		%currPos = vectorAdd(%this.getPosition(), %currPos);
		// %p = createBoxAt(%currPos, "0 0 1 1", 0.1);
		// %p.schedule(1000, delete);
		%info = %str[%i];
		%stackType = getField(%info, 0);
		%count = getField(%info, 1);

		if (%stackType !$= "")
		{
			if (isObject(%stackType)) //not a stackable id
			{
				%itemDB = %stackType;
			}
			else
			{
				%itemDB = getWord($Stackable_[%stackType, "stackedItem0"], 0);
			}

			if (!isObject(%this.shopDisplayItem[%i]))
			{
				%item = %this.shopDisplayItem[%i] = new Item(ShopDisplayItems)
				{
					dataBlock = %itemDB;
					static = 1;
				};
				%item.setTransform(%item.getPosition() SPC %rotation);
				MissionCleanup.add(%item);
				%item.canPickup = 0;

				%height = %item.getWorldBox();
				%height = getWord(%height, 5) - getWord(%height, 2);

				switch$ (%item.getDatablock().uiName)
				{
					case "Hammer ": %height = %height - 0.6;
					case "Wrench": %height = %height - 0.8;
					case "Printer": %height = %height - 0.4;
					default: %height = %height;
				}

				// %item.setTransform(vectorAdd(%currPos, "0 0 " @ %height) SPC %rotation);
				
				%offset = %item.getWorldBox();
				%center = vectorScale(vectorAdd(getWords(%offset, 0, 1), getWords(%offset, 3, 4)), 0.5);
				%offset = getWords(vectorSub(%item.getTransform(), %center), 0, 1) SPC %height;
				%item.setTransform(vectorAdd(%currPos, %offset) SPC %rotation);
			}
			else //if (%this.shopDisplayItem[%i].getDatablock() != %itemDB.getID())
			{
				%item = %this.shopDisplayItem[%i];
				%this.shopDisplayItem[%i].setDatablock(%itemDB);
				if (%itemDB.doColorShift)
				{
					%item.setNodeColor("ALL", %itemDB.colorShiftColor);
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
		}
		else
		{
			if (isObject(%this.shopDisplayItem[%i]))
			{
				%this.shopDisplayItem[%i].delete();
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

	%brickName = getSubStr(%brick.getName(), 1, 64);
	// talk("raw: " @ getSubStr(%brickName, 0, strPos(%brickName, "_")));
	%money = getMax(getSubStr(%brickName, 0, strPos(%brickName, "_")) + 0, 0);
	%lastTakenBy = %cl.getPlayerName();
	
	%pre = %cl.score;
	%cl.setScore(%cl.score + %money);
	%post = %cl.score;
	%diff = %post - %pre;

	if (%diff <= 0)
	{
		messageClient(%cl, '', "\c6The cash register is empty!");
	}
	
	%money = %money - %diff;
	%brick.settingName = 1;
	// talk("M: " @ %money @ " name: " @ %cl.getPlayerName());
	%brick.setNTObjectName(%money SPC %cl.getPlayerName());

	messageClient(%cl, '', "\c6You removed \c2$" @ %diff @ "\c6 from the cash register!");

	%brick.updateShopMenus(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
}

function fxDTSBrick::storeMoney(%brick, %amount)
{
	if (!%brick.getDatablock().isShopBrick)
	{
		return;
	}

	%brickName = getSubStr(%brick.getName(), 1, 64);
	// talk("raw: " @ getSubStr(%brickName, 0, strPos(%brickName, "_")));
	%money = getMax(getSubStr(%brickName, 0, strPos(%brickName, "_")) + 0, 0);
	%lastTakenBy = getSubStr(%brickName, strPos(%brickName, "_"), 30);
	if (%lastTakenBy $= "")
	{
		%lastTakenBy = "_None";
	}
	
	%money = %money + %amount;
	%brick.settingName = 1;
	// talk("M: " @ %money @ " name: " @ %cl.getPlayerName());
	%brick.setNTObjectName(%money @ %lastTakenBy);

	%brick.updateShopMenus(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
}

function buyUnit(%cl, %menu, %option)
{
	if (!isObject(%cl.player))
	{
		%cl.centerprint("You are dead!", 1);
		return;
	}
	%brick = %menu.brick;

	if (!isObject(%brick.vehicle) && vectorDist(%brick.getPosition(), %cl.player.getHackPosition()) > 7)
	{
		%cl.centerprint("You are too far away from the shop!", 1);
		return;
	}

	if (%menu.menuOption[%option] $= "Empty")
	{
		if (%cl.nextMessageEmpty < $Sim::Time)
		{
			messageClient(%cl, '', "That slot is empty!", 1);
		}

		%cl.nextMessageEmpty = $Sim::Time + 2;

		%brick.displayStorageContents(%brick.eventOutputParameter0_1, %brick.eventOutputParameter0_2, %brick.eventOutputParameter0_3, %brick.eventOutputParameter0_4, %cl);
		%cl.displayCenterprintMenu(%option);
		return;
	}

	//check if they can buy here
	%storageSlot = %option + 1;
	%storageData = validateStorageContents(%brick.eventOutputParameter[0, %storageSlot], %this);
	%storageCount = getField(%storageData, 1);
	%stackType = getField(%storageData, 0);

	%total = 1;
	// if (%cl.lastPurchased[%stackType] + 0.3 > $Sim::Time)
	// {
	// 	if (%cl.purchaseCombo[%stackType] >= 5)
	// 	{
	// 		%total = 5;
	// 	}
	// }
	// else
	// {
	// 	%cl.purchaseCombo[%stackType] = 0;
	// }
	// %cl.lastPurchased[%stackType] = $Sim::Time;
	// %cl.purchaseCombo[%stackType]++;

	if (!isObject(%stackType)) //this is a stackable item
	{
		%price = mFloatLength($Produce::BuyCost_[%stackType] + 1, 2) * %total;
	}
	else //this is a normal item
	{
		%price = mCeil(%stackType.cost * 1.2);
		if (%price <= 10)
		{
			%price = 10;
		}
	}

	if (%price > %cl.score)
	{
		// if (%cl.nextMessageAfford[%stackType] < $Sim::Time)
		// {
			messageClient(%cl, '', "You can't afford this!");
		// }

		// %cl.nextMessageAfford[%stackType] = $Sim::Time + 2;

		%brick.displayStorageContents(%brick.eventOutputParameter0_1, %brick.eventOutputParameter0_2, %brick.eventOutputParameter0_3, %brick.eventOutputParameter0_4, %cl);
		%cl.displayCenterprintMenu(%option);
		return;
	}

	//can buy, bought!
	%cl.setScore(%cl.score - %price);
	purchasedMessageSchedule(%cl, %stackType, %total, %price);
	%menu.brick.storeMoney(%price);
	%menu.brick.updateShopDisplay();


	//actual removal of item
	if (!isObject(%stackType)) //stackable item
	{
		%amt = getMin(%total, %storageCount);
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
	%brick.updateShopMenus(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
	
	%i = new Item()
	{
		dataBlock = %itemDB;
		count = %amt;
		deliveryPackageInfo = %packageInfo;
		harvestedBG = getBrickgroupFromObject(%cl.player);
	};
	MissionCleanup.add(%i);
	%i.setTransform(%cl.player.getTransform());

	Armor::onCollision(%cl.player.getDatablock(), %cl.player, %i, %i.getPosition(), 0, 0);

	%brick.displayStorageContents(%brick.eventOutputParameter0_1, %brick.eventOutputParameter0_2, %brick.eventOutputParameter0_3, %brick.eventOutputParameter0_4, %cl);
	%cl.displayCenterprintMenu(%option);
}

function purchasedMessageSchedule(%cl, %stackType, %count, %amount)
{
	cancel(%cl.shopPurchaseSched[%stackType]);

	%cl.shopPurchaseSched[%stackType] = schedule(1000, %cl, messagePurchaseTotal, %cl, %stackType);
	%cl.shopPurchaseCount[%stackType] += %count;
	%cl.shopPurchaseAmount[%stackType] += %amount;
}

function messagePurchaseTotal(%cl, %stackType)
{
	cancel(%cl.shopPurchaseSched[%stackType]);

	%count = %cl.shopPurchaseCount[%stackType];
	%amount = %cl.shopPurchaseAmount[%stackType];
	%name = %stackType.uiName !$= "" ? %stackType.uiName : %stackType;
	messageClient(%cl, '', "\c6You purchased \c6" @ %count @ " " @ %name @ " for \c0$" @ mFloatLength(%amount, 2));

	%cl.shopPurchaseCount[%stackType] = "";
	%cl.shopPurchaseAmount[%stackType] = "";
	%cl.shopPurchaseSched[%stackType] = "";
}