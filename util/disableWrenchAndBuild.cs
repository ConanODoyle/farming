package disableWrenchData
{
	function serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4)
	{
		if (isObject(%client) && !%client.isAdmin)
		{
			return;
		}
		else
		{
			return parent::serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
		}
	}

	function serverCmdVehicleSpawn_Respawn(%cl, %data)
	{
		if (isObject(%cl.wrenchBrick))
		{
			if (isObject(%data) && (!isObject(%cl.wrenchBrick.vehicle) || %data != %cl.wrenchBrick.vehicle.getDatablock()))
			{
				%cl.wrenchBrick.respawnVehicle();
				return;
			}
		}
		parent::serverCmdVehicleSpawn_Respawn(%cl, %data);
	}

	function serverCmdSetWrenchData(%cl, %data) 
	{
		if (!%cl.isBuilder || %cl.name $= "")
		{
			if (isObject(%cl.wrenchBrick))
			{
				%db = %cl.wrenchBrick.getDatablock();
				if (%db.isPlant || %db.isStorageBrick || %db.isSprinkler || %db.isWaterTank || %db.isDirt || %db.isGreenhouse || %db.isLot
					|| %db.isShopLot || %db.isShopBrick || %db.isCompostBin || %db.isProcessor)
				{
					messageClient(%cl, '', "You cannot edit wrench data on special bricks!");
					return;
				}
			}
			for (%i = 0; %i < getFieldCount(%data); %i++)
			{
				%field = getField(%data, %i);
				%type = getWord(%field, 0);
				switch$ (%type)
				{
					case "IDB":
						if (!checkItemAllowed(getWord(%field, 1)))
						{
							%data = removeField(%data, %i);
							%i--;
							continue;
						}
						else if (!purchaseItem(%cl, getWord(%field, 1)))
						{
							%data = removeField(%data, %i);
							%i--;
							continue;
						}
					case "VDB":
						if (%cl.wrenchBrick.dataBlock.specialBrickType $= "VehicleSpawn" && !purchaseVehicle(%cl, getWord(%field, 1)))
						{
							%data = setField(%data, %i, "VDB 0");
							%i--;
							continue;
						}
					// put above in a vehicle purchase subscript
					case "SDB":
						if (%db.musicRange >= 16 || %db.musicRange $= "")
						{
							%data = removeField(%data, %i);
							messageClient(%cl, '', "You can only set music on short range bricks!");
							%i--;
							continue;
						}
				}
			}

			if (%cl.debug)
			{
				for (%i = 0; %i < getFieldCount(%data); %i++)
				{
					messageClient(%cl, '', "f: " @ getField(%data, %i));
				}
			}
		}

		return parent::serverCmdSetWrenchData(%cl, %data);
	}

	function fxDTSBrick::setRaycasting(%this, %bool)
	{
		%db = %this.getDatablock();
		if (%db.isPlant || %db.isStorageBrick || %db.isProcessor || %db.isSprinkler 
			|| %db.isShopLot || %db.isWaterTank || %db.isDirt || %db.isGreenhouse)
		{
			%bool = 1;
		}
		parent::setRaycasting(%this, %bool);
	}

	function fxDTSBrick::setRendering(%this, %bool)
	{
		%db = %this.getDatablock();
		if (%db.isPlant || %db.isStorageBrick || %db.isProcessor || %db.isSprinkler 
			|| %db.isShopLot || %db.isWaterTank || %db.isDirt || %db.isGreenhouse)
		{
			%bool = 1;
		}
		parent::setRendering(%this, %bool);
	}

	function fxDTSBrick::setColliding(%this, %bool)
	{
		%db = %this.getDatablock();
		if (%db.isStorageBrick || %db.isGreenhouse || %db.isWaterTank || %db.isDirt 
			|| %db.isShopLot || %db.isTree || %db.isProcessor)
		{
			if (%db.noCollision)
			{
				%bool = 0;
			}
			else
			{
				%bool = 1;
			}
		}
		else if (%db.isPlant)
		{
			%bool = 0;
		}
		parent::setColliding(%this, %bool);
	}

	function fxDTSBrick::setShapeFX(%this, %type)
	{
		%db = %this.getDatablock();
		if (!%this.bypassShapeFX &&	(%db.isStorageBrick || %db.isSprinkler || %db.isGreenhouse || %db.isWaterTank 
			|| %db.isShopLot || %db.isDirt || %db.isPlant || %db.isLot || %db.isProcessor))
		{
			%type = 0;
		}
		parent::setShapeFX(%this, %type);
	}

	function fxDTSBrick::setColorFX(%this, %type)
	{
		%db = %this.getDatablock();
		if (%db.isStorageBrick || %db.isSprinkler || %db.isGreenhouse || %db.isWaterTank 
			|| %db.isShopLot || %db.isDirt || %db.isPlant || %db.isLot || %db.isProcessor)
		{
			%type = 0;
		}
		parent::setColorFX(%this, %type);
	}

	function serverCmdPlantBrick(%cl)
	{
		if (isObject(%pl = %cl.player) && isObject(%pl.tempBrick) && !%cl.isBuilder)
		{
			%db = %pl.tempBrick.getDatablock();
			if ((%db.category $= "Baseplates" && %db.subCategory !$= "Plain") || %db.subCategory $= "Drinks" || %db.subCategory $= "Holes"
				|| %db.category $= "Zonebricks" || %db.uiName $= "Treasure Chest" || %db.isShopLot || %db.isLot || %db.isTeledoor || %db.isIllegal)
			{
				if (%db.subCategory $= "Cube" && getWord(%db.uiname, 0) < 16)
				{
					return parent::serverCmdPlantBrick(%cl);
				}
				messageClient(%cl, '', "You cannot plant " @ %db.uiname @ " bricks!");
				if (%db.isLot)
				{
					messageClient(%cl, '', "Find an empty single lot (red-colored) and do /buylot to purchase a lot!");
				}
				serverCmdCancelBrick(%cl);
				return;
			}
		}

		return parent::serverCmdPlantBrick(%cl);
	}

	function Armor::onCollision(%this, %obj, %col, %vec, %speed)
	{
		if (%col.getClassName() $= "Item" && %col.client == -1)
		{
			%col.client = "";
		}
		if (%col.getClassName() $= "Item" && getBrickgroupFromObject(%col).bl_id == 888888 && %col.isStatic() && %col.spawnBrick.getName() $= "")
		{
			return;
		}
		if (%col.cannotPickup)
		{
			return;
		}

		return parent::onCollision(%this, %obj, %col, %vec, %speed);
	}
};
schedule(1000, 0, activatePackage, disableWrenchData);

function fxDTSBrick::setItemPickupable(%brick, %bool)
{
	%brick.item.cannotPickup = !%bool;
}
registerOutputEvent("fxDTSBrick", "setItemPickupable", "bool 1");

function checkItemAllowed(%itemDB)
{
	if (%itemDB.isStackable || getBuyPrice(%itemDB) > 0 || %itemDB.cannotSpawn)
	{
		messageClient(%cl, '', %itemDB.uiname @ " cannot be spawned on a brick!");
		return 0;
	}
	return 1;
}

function checkBrickAllowed(%brick)
{
	if ((%db.category $= "Baseplates" && %db.subCategory !$= "Plain") || %db.subCategory $= "Drinks" || %db.subCategory $= "Holes"
				|| %db.category $= "Zonebricks" || %db.uiName $= "Treasure Chest" || %db.isShopLot || %db.isLot || %db.isTeledoor || %db.isIllegal)
	{
		if (%db.subCategory $= "Cube" && getWord(%db.uiname, 0) < 16)
		{
			return parent::serverCmdPlantBrick(%cl);
		}
		messageClient(%cl, '', "You cannot plant " @ %db.uiname @ " bricks!");
		serverCmdCancelBrick(%cl);
		return;
	}
}