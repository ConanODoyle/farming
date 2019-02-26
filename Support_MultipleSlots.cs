//core code ripped from the 7SlotPlayer add-on

//Setting the support so we can see all Slots
package InventorySlotAdjustment
{
	function Armor::onNewDatablock(%data,%this)
	{
		Parent::onNewDatablock(%data,%this);
		if(isObject(%this.client) && %data.maxTools != %this.client.lastMaxTools)
		{
			%this.client.lastMaxTools = %data.maxTools;
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
			for(%i=0;%i<%data.maxTools;%i++)
			{
				if(isObject(%this.tool[%i]))
					messageClient(%this.client,'MsgItemPickup',"",%i,%this.tool[%i].getID(),1);
				else
					messageClient(%this.client,'MsgItemPickup',"",%i,0,1);
			}
		}
	}
	function GameConnection::setControlObject(%this,%obj)
	{
		Parent::setControlObject(%this,%obj);
		if(%obj == %this.player && %obj.getDatablock().maxTools != %this.lastMaxTools)
		{
			%this.lastMaxTools = %obj.getDatablock().maxTools;
			commandToClient(%this,'PlayGui_CreateToolHud',%obj.getDatablock().maxTools);
		}
	}
	function Player::changeDatablock(%this,%data,%client)
	{
		if(%data != %this.getDatablock() && %data.maxTools != %this.client.lastMaxTools)
		{
			%this.client.lastMaxTools = %data.maxTools;
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
		}
		Parent::changeDatablock(%this,%data,%client);
	}
};
activatePackage(InventorySlotAdjustment);

//7 slot player.
exec("Add-ons/Player_No_Jet/server.cs");
datablock PlayerData(Player6SlotArmor : PlayerNoJet)
{
	canJet = 0;
	uiName = "6 Slot Player";
	maxTools = 6;
	maxWeapons = 6;

	expCost = 7000;
};

datablock PlayerData(Player7SlotArmor : PlayerNoJet)
{
	canJet = 0;
	uiName = "7 Slot Player";
	maxTools = 7;
	maxWeapons = 7;

	expCost = 8000;
};

datablock PlayerData(Player8SlotArmor : PlayerNoJet)
{
	canJet = 0;
	uiName = "8 Slot Player";
	maxTools = 8;
	maxWeapons = 8;

	expCost = 9000;
};

function serverCmdUpgradeInventory(%cl)
{
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
			default: %next = 0;
		}
	}

	if (!isObject(%next))
	{
		messageClient(%cl, '', "You already have the biggest inventory upgrade available!");
		return;
	}
	else if (%cl.farmingExperience < %next.expCost)
	{
		messageClient(%cl, '', "You don't have enough exp to upgrade your inventory! (Cost: " @ %next.expCost @ ")");
		return;
	}

	if (%cl.farmingExperience >= %next.expCost && %cl.upgradeInventoryTimeout < $Sim::Time)
	{
		messageClient(%cl, '', "\c6The \c3" @ %next.maxTools @ "-Slot\c6 inventory upgrade will cost \c3" @ %next.expCost @ "\c6 EXP. Repeat command to confirm");
		%cl.upgradeInventoryTimeout = $Sim::Time + 5;
		return;
	}
	else
	{
		%cl.addExperience(-1 * %next.expCost);
		%cl.playerDatablock = %next;
		if (isObject(%cl.player))
		{
			%cl.player.setDatablock(%next);
		}
		messageClient(%cl, '', "\c6Your inventory has been upgraded to \c3" @ %next.maxTools @ "\c6 slots!");
		%cl.upgradeInventoryTimeout = 0;
	}
}