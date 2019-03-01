function purchaseVehicle(%cl, %vehicle)
{
	if (!isObject(%vehicle))
	{
		if (isObject(%cl.wrenchBrick) && isObject(%cl.wrenchBrick.vehicle) && %cl.wrenchBrick.vehicle.getDatablock().cost > 0)
		{
			sellObject(%cl.wrenchBrick.vehicle);
		}
		return 1;
	}

	if (isObject(%cl.wrenchBrick) && isObject(%cl.wrenchBrick.vehicle))
	{
		%brickVehi = %cl.wrenchBrick.vehicle.getDatablock();
		if (%brickVehi.getID() != %vehicle.getID())
		{
			sellObject(%cl.wrenchBrick.vehicle);
		}
		else
		{
			return 1;
		}
	}

	%cost = %vehicle.cost;
	if (%cl.score >= %cost)
	{
		%cl.setScore(%cl.score - %cost);
		%cl.centerPrint("<color:ff0000>$" @ %vehicle.cost @ "<color:ffffff> - " @ %vehicle.uiName, 1);
		%cl.schedule(50, centerPrint, "<color:cc0000>$" @ %vehicle.cost @ "<color:cccccc> - " @ %vehicle.uiName, 2);
		messageClient(%cl, '', "\c6You purchased the \c3" @ %vehicle.uiName @ "\c6 for \c2$" @ %cost);
		return 1;
	}
	messageClient(%cl, '', "You cannot afford the " @ %vehicle.uiname @ "! (Cost: $" @ %vehicle.cost @ ")");
	return 0;
}

function purchaseItem(%cl, %item)
{
	if (%cl.bypassRestrictions)
	{
		return 1;
	}
	
	if (isObject(%item))
		messageClient(%cl, '', "You can't spawn items! Get tools from the market!");
	return 0;
	if (!isObject(%item))
	{
		if (isObject(%cl.wrenchBrick) && isObject(%cl.wrenchBrick.item) && %cl.wrenchBrick.item.getDatablock().cost > 0)
		{
			sellObject(%cl.wrenchBrick.item);
		}
		return 1;
	}

	%cost = %item.cost;
	if (%cl.score >= %cost)
	{
		%cl.setScore(%cl.score - %cost);
		%cl.centerPrint("<color:ff0000>$" @ %item.cost @ "<color:ffffff> - " @ %item.uiName, 1);
		%cl.schedule(50, centerPrint, "<color:cc0000>$" @ %item.cost @ "<color:cccccc> - " @ %item.uiName, 2);
		messageClient(%cl, '', "\c6You purchased the \c3" @ %item.uiName @ "\c6 for \c2$" @ %cost);
		return 1;
	}
	messageClient(%cl, '', "You cannot afford the " @ %item.uiname @ "! (Cost: $" @ %item.cost @ ")");
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
		if (%obj.isPlanted)
		{
			schedule(33, %obj, checkBrickDataCost, %obj);
		}
		return parent::onAdd(%obj);
	}

	function fxDTSBrick::onDeath(%obj)
	{
		if (isObject(%obj.vehicle) && %obj.vehicle.getDatablock().cost > 0)
		{
			sellObject(%obj.vehicle);
		}

		return parent::onDeath(%obj);
	}

	function fxDTSBrick::onRemove(%obj)
	{
		if (isObject(%obj.vehicle) && %obj.vehicle.getDatablock().cost > 0)
		{
			sellObject(%obj.vehicle);
		}

		return parent::onRemove(%obj);
	}
};
activatePackage(wrenchCostRefund);