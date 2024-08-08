datablock ItemData(InventoryUpgradeItem : HammerItem)
{
	shapeFile = "./satchel/satchel.dts";
	uiName = "Inventory Upgrade Satchel";
	image = "InventoryUpgradeImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;

	iconName = "Add-ons/Server_Farming/icons/satchel";
};

datablock ShapeBaseImageData(InventoryUpgradeImage)
{
	shapeFile = "./satchel/satchel.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = RepairItem.colorShiftColor;

	offset = "-0.54 0 -0.1";
	eyeOffset = "0 1.4 -0.4";
	eyeRotation = eulerToMatrix("0 0 180");

	item = "InventoryUpgradeItem";

	armReady = 1;

	toolTip = "Upgrades your inventory, max 9";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "Ready";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.6;
	stateWaitForTimeout[1] = 0;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateWaitForTimeout[1] = false;

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 1;
	stateWaitForTimeout[2] = true;

	stateName[3] = "Ready2";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.6;
	stateWaitForTimeout[3] = 0;
	stateScript[3] = "onReady";
	stateTransitionOnTriggerDown[3] = "Fire";
	stateWaitForTimeout[3] = false;
};

function InventoryUpgradeImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function InventoryUpgradeImage::onUnmount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyRight");
}

function InventoryUpgradeImage::onReady(%this, %obj, %slot)
{
	%cl = %obj.client;
	if (!isObject(%cl))
	{
		return;
	}

	if (!isObject(%cl.playerDatablock) || %cl.playerDatablock.maxTools < 6)
	{
		%next = "Player6SlotArmor";
	}
	else
	{
		switch$ (%cl.playerDatablock.getName())
		{
			case "Player6SlotArmor": %next = "Player7SlotArmor";
			case "Player7SlotArmor": %next = "Player8SlotArmor";
			case "Player8SlotArmor": %next = "Player9SlotArmor";
			default: %next = 0;
		}
	}

	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("\c6Upgrade your maximum inventory to " @ %next.maxTools @ " slots! \n"
			@ "\c6Costs " @ %next.expCost @ " experience to use.", 1);
	}
}

function InventoryUpgradeImage::onFire(%this, %obj, %slot)
{
	%cl = %obj.client;
	if (!isObject(%cl))
	{
		return;
	}
	
	if (!isObject(%cl.playerDatablock) || %cl.playerDatablock.maxTools < 6)
	{
		%next = "Player6SlotArmor";
	}
	else
	{
		switch$ (%cl.playerDatablock.getName())
		{
			case "Player6SlotArmor": %next = "Player7SlotArmor";
			case "Player7SlotArmor": %next = "Player8SlotArmor";
			case "Player8SlotArmor": %next = "Player9SlotArmor";
			default: %next = 0;
		}
	}

	if (!isObject(%next))
	{
		messageClient(%cl, '', "You already have the biggest inventory upgrade available!");
		return 0;
	}
	else if (%cl.farmingExperience < %next.expCost)
	{
		messageClient(%cl, '', "You don't have enough exp to upgrade your inventory! (Cost: " @ %next.expCost @ ")");
		return 0;
	}

	%cl.addExperience(-1 * %next.expCost);
	%cl.playerDatablock = %next;
	%obj.setDatablock(%next);
	messageClient(%cl, '', "\c6Your inventory has been upgraded to \c3" @ %next.maxTools @ "\c6 slots!");
	%obj.farmingRemoveItem(%obj.currTool);
}