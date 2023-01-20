function purchaseVehicle(%cl, %vehicle)
{
	if (%cl.isBuilder || %cl.brickgroup.isLoadingLot)
	{
		return 1;
	}
	
	if (!isObject(%vehicle))
	{
		if (isObject(%cl.wrenchBrick) && isObject(%cl.wrenchBrick.vehicle) && getSellPrice(%cl.wrenchBrick.vehicle.getDatablock()) > 0)
		{
			sellObject(%cl.wrenchBrick.vehicle);
			%cl.wrenchBrick.vehicle.delete();
		}
		return 1;
	}

	if (isObject(%cl.wrenchBrick) && isObject(%cl.wrenchBrick.vehicle))
	{
		%brickVehi = %cl.wrenchBrick.vehicle.getDatablock();
		if (%brickVehi.getID() != %vehicle.getID() && %vehicle.cost > 0)
		{
			sellObject(%cl.wrenchBrick.vehicle);
			%cl.wrenchBrick.vehicle.delete();
		}
		else if (%vehicle.cost <= 0)
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}

	%cost = getBuyPrice(%vehicle);
	if (%cost <= 0)
	{
		return 0;
	}
	if (%cl.checkMoney(%cost))
	{
		%cl.subMoney(%cost);
		%cl.centerPrint("\c0$" @ %cost @ "\c6 - " @ %vehicle.uiName, 1);
		%cl.schedule(50, centerPrint, "<color:cc0000>$" @ %cost @ "<color:cccccc> - " @ %vehicle.uiName, 2);
		messageClient(%cl, '', "\c6You purchased the \c3" @ %vehicle.uiName @ "\c6 for \c2$" @ %cost);
		return 1;
	}
	messageClient(%cl, '', "You cannot afford the " @ %vehicle.uiname @ "! (Cost: $" @ %cost @ ")");
	return 0;
}

function purchaseItem(%cl, %item)
{
	if (%cl.isBuilder)
	{
		return 1;
	}
	
	if (isObject(%item))
		messageClient(%cl, '', "You can't spawn items! Get tools from the market!");
	return 0;
}

function checkBrickDataCost(%brick)
{
	%cl = getBrickgroupFromObject(%brick).client;
	if (!isObject(%cl))
	{
		return 1;
	}

	if (isObject(%brick.item) && !purchaseItem(%cl, %brick.item.getDatablock()))
	{
		%brick.item.delete();
	}

	if (isObject(%brick.vehicle) && !purchaseVehicle(%cl, %brick.vehicle.getDatablock()))
	{
		%brick.vehicle.delete();
	}
}

package wrenchCostRefund
{
	function fxDTSBrick::onAdd(%obj) 
	{
		if (%obj.isPlanted && !(%obj.getGroup().isLoadingLot || %obj.getGroup().bl_id == 888888))
		{
			schedule(33, %obj, checkBrickDataCost, %obj);
		}
		return parent::onAdd(%obj);
	}

	function fxDTSBrick::onDeath(%obj)
	{
		if (isObject(%obj.vehicle) && getSellPrice(%obj.vehicle.getDatablock()) > 0)
		{
			sellObject(%obj.vehicle);
		}

		return parent::onDeath(%obj);
	}

	function fxDTSBrick::onRemove(%obj)
	{
		if (isObject(%obj.vehicle) && getSellPrice(%obj.vehicle.getDatablock()) > 0)
		{
			sellObject(%obj.vehicle);
		}

		return parent::onRemove(%obj);
	}
};
activatePackage(wrenchCostRefund);