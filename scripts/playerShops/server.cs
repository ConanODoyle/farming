//shop owners auto-sell products at base market price + $1
//full trust users open the entire menu: all 4 storage slots + money. doesn't cost anything to take stuff out
//	money will show who last took out money, and how much. money would auto-remove all
//build trust and less users open the shop menu, where they can buy the items one at a time

//core is loaded first so we can use eventstorage functions
//in fact, we can just package the base event function displayStorageContents
//money left in shop brick will be saved in the brick name

//function validateStorageContents(%string, %brick)
//

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

					storageLoop(%cl, %this);
				}
				return;
			}

			%cl.startShopMenu(%this.shopStorageMenu);

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
				if (%price <= 0)
				{
					%price = 1;
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
	%money = getWord(%brickName, 0);
	%lastTakenBy = trim(getSubStr(getWords(%brickName, 1, 3), 0, 20));
	if (%lastTakenBy $= "")
	{
		%lastTakenBy = "None";
	}
	%this.shopStorageMenu.menuOption[4] = "$" @ %money @ " - " @ %lastTakenBy;
}

function removeMoney(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (getTrustLevel(%cl, %brick) < 2)
	{
		return;
	}

	%brickName = getSubStr(%this.getName(), 1, 64);
	%money = getWord(%brickName, 0);
	%lastTakenBy = %cl.getPlayerName();
	
	%pre = %cl.score;
	%post = %cl.setScore(%cl.score + %money);
	%diff = %post - %pre;
	
	%money = %money - %diff;
	%brick.settingName = 1;
	%brick.setNTObjectName(%money SPC %cl.getPlayerName());

	%brick.updateCenterprintMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
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
			messageClient(%cl, '', "That slot is empty!", 1);

		%cl.nextMessageEmpty = $Sim::Time + 2;

		%brick.displayStorageContents(%brick.eventOutputParameter0_1, %brick.eventOutputParameter0_2, %brick.eventOutputParameter0_3, %brick.eventOutputParameter0_4, %cl);
		%cl.displayCenterprintMenu(%option);
		return;
	}

	//check if they can buy here




	//actual removal of item
	%storageSlot = %option + 1;
	%storageData = validateStorageContents(%brick.eventOutputParameter[0, %storageSlot], %this);
	%storageCount = getField(%storageData, 1);
	%stackType = getField(%storageData, 0);

	if (!isObject(%stackType)) //stackable item
	{
		%max = $Stackable_[%stackType, "stackedItemTotal"] - 1;
		%max = getWord($Stackable_[%stackType, "stackedItem" @ %max], 1);

		%amt = getMin(1, %storageCount);
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
		harvestedBG = getBrickgroupFromObject(%cl.player);
	};
	MissionCleanup.add(%i);
	%i.setTransform(%cl.player.getTransform());

	Armor::onCollision(%cl.player.getDatablock(), %cl.player, %i, %i.getPosition(), 0, 0);

	%brick.displayStorageContents(%brick.eventOutputParameter0_1, %brick.eventOutputParameter0_2, %brick.eventOutputParameter0_3, %brick.eventOutputParameter0_4, %cl);
	%cl.displayCenterprintMenu(%option);
}