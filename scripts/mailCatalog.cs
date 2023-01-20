
while (isObject(MailOrderCatalog))
{
	MailOrderCatalog.delete();
}

if (!isObject(DeliverySlotSet))
{
	$DeliverySlotSet = new SimSet(DeliverySlotSet) {};
}

$DeliveryFee = 30;

package Delivery
{
	function fxDTSBrick::setNTObjectName(%brick, %name)
	{
		%ret = parent::setNTObjectName(%brick, %name);

		if (%brick.getGroup().bl_id == 888888 && %name $= "DeliverySlot")
		{
			$DeliverySlotSet.add(%brick);
			%brick.isDeliveryBrick = 1;
		}
		else if (%brick.isDeliveryBrick)
		{
			$DeliverySlotSet.remove(%brick);
			%brick.isDeliveryBrick = 0;
		}

		return %ret;
	}

	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%item = %pl.tool[%slot];
			if (%item.isDeliveryPackage)
			{
				dropDeliveryPackage(%cl, %slot);
				return;
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}

	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0 && isObject(%obj.client))
		{
			%itemDB = %col.getDatablock();
			if (%col.getClassName() $= "Item" && %itemDB.isDeliveryPackage)
			{
				%ret = deliveryPackageCanPickup(%obj, %col);

				// talk(%ret);

				if (%ret > 0)
				{
					pickupDeliveryPackage(%obj, %col, getWord(%ret, 1));
				}

				//we dont want to do normal item onCollision code with delivery packages
				return;
			}
		}

		return parent::onCollision(%db, %obj, %col, %vec, %speed);
	}

	function Item::schedulePop(%item)
	{
		if (%item.getDatablock().isDeliveryPackage && !%item.bypassDeliveryPop)
		{
			%item.schedule(60000, schedulePackagePop);
			return;
		}
		return parent::schedulePop(%item);
	}
};
activatePackage(Delivery);

//Post Office Packages

datablock ItemData(DeliveryPackageItem : HammerItem)
{
	shapeFile = "./crops/tools/package.dts";
	colorShiftColor = "0.787 0.577 0.289 1";
	uiName = "Delivery Package";
	iconName = "";

	image = DeliveryPackageImage;
};

datablock ShapeBaseImageData(DeliveryPackageImage)
{
	shapeFile = "./crops/tools/package.dts";
	emap = true;
	offset = "-0.53 0.1 -0.155";

	doColorshift = true;
	colorShiftColor = "0.787 0.577 0.289 1";

	item = DeliveryPackageItem;

	armReady = false;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "LoopA";
	stateTimeoutValue[3] = 0.1;
	stateWaitForTimeout[3] = true;
};

function DeliveryPackageImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyBoth);
}

function DeliveryPackageImage::onUnmount(%this, %obj, %slot)
{
	%obj.playThread(1, root);
}

function DeliveryPackageImage::onLoop(%this, %obj, %slot)
{
	%data = %obj.deliveryPackageInfo[%obj.currTool];
	if (getWord(%data, 0) $= "Delivered")
	{
		%owner = getWord(%data, 1);
		%contents = getWords(%data, 2, getWordCount(%data));
		%delivered = 1;
	}
	else
	{
		%owner = getWord(%data, 0);
		%contents = getWords(%data, 1, getWordCount(%data));
	}

	if (%owner $= "")
	{
		%obj.client.centerprint("This package has no recipient - contact an admin about this bug!", 1);
		return;
	}

	%name = ("Brickgroup_" @ %owner).name;
	%item = getWord(%contents, 0);
	%count = getWord(%contents, 1);
	%recipientStr = "<color:ffffff>Package addressed to <color:ffff00>" @ %name @ " ";
	%contentStr = "<color:ffffff>Contains <color:ffff00>" @ %count SPC %item.uiName @ " <br><color:ffffff>-Click to open- ";
	%rewardStr = "<color:ffffff>Deliver to the recipient for <color:00ff00>$" @ $DeliveryFee @ 
		" <br><color:ff0000>If you lose the package, you will be charged 2x the price of its contents";

	if (%obj.client.brickgroup.name $= %name)
	{
		%obj.client.centerprint(%recipientStr @ "<br>" @ %contentStr, 1);
	}
	else
	{
		if (!%delivered)
		{
			%obj.client.centerprint(%recipientStr @ "<br>" @ %rewardStr, 1);
		}
		else
		{
			%obj.client.centerprint(%recipientStr @ "<br><color:ff0000>This package can only be opened by the recipient, and has already been delivered. ", 1);
		}
	}
}

function DeliveryPackageImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, shiftUp);
	%data = %obj.deliveryPackageInfo[%obj.currTool];
	if (getWord(%data, 0) $= "Delivered")
	{
		%owner = getWord(%data, 1);
		%contents = getWords(%data, 2, getWordCount(%data));
	}
	else
	{
		%owner = getWord(%data, 0);
		%contents = getWords(%data, 1, getWordCount(%data));
	}

	if (%owner != %obj.client.bl_id)
	{
		return;
	}
	else
	{
		%item = getWord(%contents, 0);
		%count = getWord(%contents, 1);
		if (%item.isStackable) 
		{
			%i = new Item(PackageDrops) 
			{
				dataBlock = %item;
				count = %count;
			};
			MissionCleanup.add(%i);
			%i.schedule(60000, schedulePop);
		}
		else
		{
			for (%i = 0; %i < %count; %i++)
			{
				%i = new Item(PackageDrops)
				{
					dataBlock = %item;
				};
				MissionCleanup.add(%i);
				%i.schedule(60000, schedulePop);
			}
		}
	}
}

//code is an edited copy of the default drop item code
//added and edited lines are commented
function dropDeliveryPackage(%client, %position)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%item = %player.tool[%position];
	if (isObject(%item))
	{
		if (%item.canDrop == 1.0)
		{
			%zScale = getWord(%player.getScale(), 2);
			%muzzlepoint = VectorAdd(%player.getPosition(), "0 0" SPC 1.5 * %zScale);
			%muzzlevector = %player.getEyeVector();
			%muzzlepoint = VectorAdd(%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform(%player.getTransform());
			%thrownItem = new Item(""){
				dataBlock = %item;
				deliveryPackageInfo = %player.deliveryPackageInfo[%position]; //added line here
				sourceClient = %client; //added line here
				sourceBrickgroup = %client.brickGroup; //added line here
			};
			%thrownItem.setScale(%player.getScale());
			%player.deliveryPackageInfo[%position] = ""; //added line here
			MissionCleanup.add(%thrownItem);
			%thrownItem.setTransform(%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity(VectorScale(%muzzlevector, 20.0 * %zScale));
			// %thrownItem.schedulePop(); //commented out this line (dont want PO packages to despawn so quickly)
			%thrownItem.schedule(60000, schedulePop); //delay schedulePop function call since this costs money
			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID();
			%thrownItem.setCollisionTimeout(%player);
			if (%item.className $= "Weapon")
			{
				%player.weaponCount = %player.weaponCount - 1.0;
			}
			%player.tool[%position] = 0;
			messageClient(%client, 'MsgItemPickup', '', %position, 0);
			if (%player.getMountedImage(%item.image.mountPoint) > 0.0)
			{
				if (%player.getMountedImage(%item.image.mountPoint).getId() == %item.image.getId())
				{
					%player.unmountImage(%item.image.mountPoint);
				}
			}
		}
	}
}

function deliveryPackageCanPickup(%pl, %item)
{
	for (%i = 0; %i < %pl.getDatablock().maxTools; %i++)
	{
		if (%pl.tool[%i] == 0)
		{
			return 1 SPC %i;
		}
	}

	return 0;
}

function pickupDeliveryPackage(%pl, %item, %slot)
{
	%pl.tool[%slot] = %item.getDatablock();
	%pl.deliveryPackageInfo[%slot] = %item.deliveryPackageInfo;

	%targetClient = findClientByBL_ID(getWord(%pl.deliveryPackageInfo, 0));
	if (%targetClient !$= "Delivered" && %pl == %targetClient.player)
	{
		%sourceClient = %item.sourceClient;
		if (isObject(%sourceClient) && %sourceClient != %targetClient)
		{
			messageClient(%sourceClient, '', "\c6Package delivered! You received \c2$" @ $DeliveryFee @ "\c6 from the post office!");
			%sourceClient.addMoney($DeliveryFee);

			%pl.deliveryPackageInfo = "Delivered" SPC %pl.deliveryPackageInfo;
		}
	}
	messageClient(%cl, 'MsgItemPickup', '', %slot, %item.getDatablock());
	%item.delete();
}

function fxDTSBrick::giveDeliveryPackage(%brick, %deliveryInfo, %cl)
{
	if (!isObject(%pl = %cl.player) || !(%ret = deliveryPackageCanPickup(%pl)) || %deliveryInfo $= "")
	{
		return;
	}

	%slot = getWord(%ret, 1);
	%item = new Item(DeliveryPackage) { dataBlock = "DeliveryPackageItem"; deliveryPackageInfo = %deliveryInfo; };
	pickupDeliveryPackage(%pl, %item, %slot);
	%brick.setItem(0);
	%brick.clearEvents();
}

registerOutputEvent("fxDTSBrick", "giveDeliveryPackage", "string 200 100", 1);

function postOfficeSpaceLeft()
{
	%count = 0;
	for (%i = $DeliverySlotSet.getCount() - 1; %i >= 0; %i--)
	{
		%brick = $DeliverySlotSet.getObject(%i);
		if (!isObject(%brick.item))
		{
			%count++;
		}
	}
	return %count;
}

function spawnDeliveryPackage(%targetBL_ID, %item, %count, %cost)
{
	for (%i = 0; %i < $DeliverySlotSet.getCount(); %i++)
	{
		%brick = $DeliverySlotSet.getObject(%i);
		if (!isObject(%brick.item))
		{
			%possible[%count++ - 1] = %brick;
			if (getRandom() < 0.3) 
			{
				break;
			}
		}
	}

	%pick = getRandom(%count - 1);
	if (isObject(%possible[%pick]))
	{
		%pick.setItem(DeliveryPackageItem.getID());
		%pick.setItemPosition(-1); //puts item in center of brick
		%pick.clearEvents();

		%enabled = 1;
		%delay = 0;
		%inputEvent = "onActivate";
		%target = "Self"; //Self
		%outputEvent = "giveDeliveryPackage";

		if (!isObject(%pick.getGroup().client))
		{
			%pick.getGroup().client = new ScriptObject(DummyClient)
			{
				isAdmin = 1;
				isSuperAdmin = 1;
				wrenchBrick = %pick;
				bl_id = %pick.getGroup().bl_id;
				brickGroup = %pick.getGroup();
			};
			%pick.getGroup().client.client = %pick.getGroup().client;
			%dummy = 1;
		}
		%prev = %pick.getGroup().client.isAdmin;
		%pick.getGroup().client.isAdmin = 1;
		%param1 = %targetBL_ID SPC %item SPC %count SPC %cost;
		%pick.addEvent(%enabled, %delay, %inputEvent, %target, %outputEvent, %param1, %param2, %param3, %param4);
		%pick.getGroup().client.isAdmin = %prev;
		if (%dummy)
		{
			%pick.getGroup().client.delete();
		}
	}
}

function Item::schedulePackagePop(%item)
{
	if (%item.deliveryPackageInfo $= "" || getWord(%item.deliveryPackageInfo, 0) $= "Delivered")
	{
		%prevTime = $Game::Item::PopTime;
		$Game::Item::PopTime = 5000;
		%item.bypassDeliveryPop = 1;
		%item.schedulePop();
		$Game::Item::PopTime = %prevTime;
		return;
	}

	%cost = getWord(%item.deliveryPackageInfo, 2);
	%dropper = %item.sourceClient;
	%recipientBG = "Brickgroup_" @ getWord(%item.deliveryPackageInfo, 0);
	
	if (!isObject(%dropper))
	{
		%bg = %item.sourceBrickgroup;
		%bg.delayedScoreAdjustment -= %cost * 2;
	}
	else
	{
		%dropper.subMoney(%cost * 2);
		%bg = %dropper.brickGroup;
		messageClient(%dropper, '', "\c6You were charged \c0$" @ getMax(%cost * 2, 50) @ "\c6 for losing a package!");
	}

	if (isObject(%recipientBG.client))
	{
		%recipientBG.client.addMoney(%cost);
		messageClient(%recipientBG.client, '', "\c6You were refunded \c2$" @ %cost @ "\c6 due to \c3" @ %bg.name @ "\c6 losing your package.");
	}
	else
	{
		%recipientBG.delayedScoreAdjustment += %cost;
	}

	%item.delete();
}


//Mail order catalog

$MailOrderCatalog = new ScriptObject(MailOrderCatalog)
{
	isCenterprintMenu = 1;
	menuName = "Mail Order";

	menuOption[0] = "Order Seeds (50% off!)";
	menuOption[1] = "Order Tools";
	// menuOption[2] = "Order Consumables";
	// menuOption[3] = "Order Novelty";

	menuFunction[0] = "openSeedsMenu";
	menuFunction[1] = "openToolsMenu";
	// menuFunction[2] = "openConsumablesMenu";
	// menuFunction[3] = "openNoveltyMenu";

	menuOptionCount = 2;
};

$MailOrderSeeds = new ScriptObject(MailOrderCatalog)
{
	isCenterprintMenu = 1;
	menuName = "Order Seeds";

	menuOption[0] = "Potato";
	menuOption[1] = "Carrot";
	menuOption[2] = "Tomato";
	menuOption[3] = "Corn";
	menuOption[4] = "Cabbage";
	menuOption[5] = "Onion";
	menuOption[6] = "Blueberry";
	menuOption[7] = "Turnip";
	menuOption[8] = "Apple Tree";
	menuOption[9] = "-Go Back-";

	menuFunction[0] = "startOrderSeeds";
	menuFunction[1] = "startOrderSeeds";
	menuFunction[2] = "startOrderSeeds";
	menuFunction[3] = "startOrderSeeds";
	menuFunction[4] = "startOrderSeeds";
	menuFunction[5] = "startOrderSeeds";
	menuFunction[6] = "startOrderSeeds";
	menuFunction[7] = "startOrderSeeds";
	menuFunction[8] = "startOrderSeeds";
	menuFunction[9] = "openMailOrderCatalog";

	menuOptionCount = 10;
};

$MailOrderTools = new ScriptObject(MailOrderCatalog)
{
	isCenterprintMenu = 1;
	menuName = "Order Tools";

	menuOption[0] = "Watering Can v2";
	menuOption[1] = "Watering Can v3";
	menuOption[2] = "Hose";
	menuOption[3] = "Trowel";
	menuOption[4] = "Clipper";
	menuOption[5] = "Hoe";
	menuOption[6] = "Sickle";
	menuOption[7] = "Planter";
	menuOption[8] = "-Go Back-";

	menuFunction[0] = "order" @ getWord($ItemList_[0], 0);
	menuFunction[1] = "order" @ getWord($ItemList_[1], 0);
	menuFunction[2] = "order" @ getWord($ItemList_[2], 0);
	menuFunction[3] = "order" @ getWord($ItemList_[3], 0);
	menuFunction[4] = "order" @ getWord($ItemList_[4], 0);
	menuFunction[5] = "order" @ getWord($ItemList_[5], 0);
	menuFunction[6] = "order" @ getWord($ItemList_[6], 0);
	menuFunction[7] = "order" @ getWord($ItemList_[7], 0);
	menuFunction[8] = "openMailOrderCatalog";

	menuOptionCount = 9;
};

function openMailOrderCatalog(%cl)
{
	if (postOfficeSpaceLeft() < 1) 
	{
		%cl.centerprint("The post office is full - no further packages can be ordered!");
		return;
	}

	%cl.exitCenterprintMenu();
	%cl.startCenterprintMenu($MailOrderCatalog);
}

function openSeedsMenu(%cl)
{
	%cl.exitCenterprintMenu();
	%cl.startCenterprintMenu($MailOrderSeeds);
}

function openToolsMenu(%cl)
{
	%cl.exitCenterprintMenu();
	%cl.startCenterprintMenu($MailOrderTools);
}

function startOrderSeeds(%cl, %menu, %option)
{
	%type = %menu.menuOption[%option];
	switch$ (%type)
	{
		case "Apple Tree": %seed = "Apple_TreeSeedItem";
		default: %seed = %type @ "SeedItem";
	}

	if (!isObject(%seed))
	{
		return;
	}

	%type = getSubStr(%seed.getName(), 0, strPos(%seed, "Item"));
	%cost = getBuyPrice(%type) / 2;
	%max = 10 * $StorageMax_[%type];
	if (%max < 10)
	{
		%max = 10;
	}

	%menu = generateOrderMenu(%seed, %type, %cost, %max, "orderSeeds");
	%cl.startCenterprintMenu(%menu);
}

function orderSeeds(%cl, %menu, %option)
{
	if (postOfficeSpaceLeft() < 1) 
	{
		%cl.centerprint("The post office is full - no further packages can be ordered!");
		return;
	}
	%seed = %menu.seed;
	%cost = %menu.cost;
	%total = stripChars(getWord(%menu.menuOption[%option], 0), ":");

	if (!isObject(%seed))
	{
		%menu.delete();
		return;
	}

	if (%cl.checkMoney(%cost * %total))
	{
		placeOrder(%seed, %total, %cl, %cost * %total);
		%cl.subMoney(%cost * %total);
		messageClient(%cl, '', "\c6You have been deducted \c0$" @ mFloatLength(%cost * %total, 2) @ "\c6 for \c3" @ 
			%total SPC %menu.seedType @ " seeds\c6.");
	}
	else
	{
		%plural = %total > 1 ? "s" : "";
		messageClient(%cl, '', "You do not have enough money to order " @ %total SPC %menu.seedType @ " seed" @ %plural @ "!");
	}
}

function generateOrderMenu(%itemDB, %type, %cost, %max, %func)
{
	%menu = new ScriptObject(MailOrderCatalog)
	{
		isCenterprintMenu = 1;
		menuName = "Order " @ %type @ " Seeds";

		seed = %itemDB;
		cost = %cost;
		seedType = %type;

		deleteOnExit = 1;

		menuOptionCount = 0;
	};
	MissionCleanup.add(%menu);

	for (%i = 0; %i < %max; %i++)
	{
		%menu.menuOption[%i] = %i + 1 @ ": $" @ mFloatLength(%cost * (%i + 1), 2);
		%menu.menuFunction[%i] = %func;
		%menu.menuOptionCount++;
	}

	%menu.menuOption[%max] = "-Go Back-";
	%menu.menuFunction[%max] = "openSeedsMenu";
	%menu.menuOptionCount++;

	return %menu;
}

function placeOrder(%itemDB, %count, %cl, %cost)
{
	talk(%itemDB.uiname SPC %count @ " order placed by " @ %cl @ " costing $" @ %cost);
	spawnDeliveryPackage(%cl.bl_id, %itemDB, %count, %cost);
}