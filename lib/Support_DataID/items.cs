package Support_DataIDItem
{
	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0 && isObject(%obj.client))
		{
			%itemDB = %col.getDatablock();
			if (%col.getClassName() $= "Item" && %itemDB.isDataIDObject)
			{
                // if (%col.nextPickupAttempt > $Sim::Time)
                // {
                //     return;
                // }
                // %col.nextPickupAttempt = $Sim::Time + getRandom(1, 2);

				%ret = stackedCanPickup(%obj, %col);

				// talk(%ret);

				if (!isObject(%col.harvestedBG) || getTrustLevel(%col.harvestedBG, %obj) > 1)
				{
					if (%ret > 0)
					{
						%type = getWord(%ret, 0);
						%slot = getWord(%ret, 1);
						%amt = getWord(%ret, 2);

						pickupStackableItem(%obj, %col, %slot, %amt);
					}
				}
				else
				{
					%obj.client.centerprint(%col.harvestedBG.name @ "<color:ff0000> does not trust you enough to do that.", 1);
				}
				//we dont want to do normal item onCollision code with stackable items
				return;
			}
		}

		return parent::onCollision(%db, %obj, %col, %vec, %speed);
	}

	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%item = %pl.tool[%slot];
			if (%item.isStackable)
			{
				dropStackableItem(%cl, %slot);
				return;
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}

	function ItemData::onAdd(%this, %obj)
	{
		schedule(1000, %obj, checkGroupStackable, %obj, 0);
		return Parent::onAdd(%this, %obj);
	}
};
activatePackage(Support_DataIDItem);

RegisterPersistenceVar("toolDataID0", false, "");
RegisterPersistenceVar("toolDataID1", false, "");
RegisterPersistenceVar("toolDataID2", false, "");
RegisterPersistenceVar("toolDataID3", false, "");
RegisterPersistenceVar("toolDataID4", false, "");
RegisterPersistenceVar("toolDataID5", false, "");
RegisterPersistenceVar("toolDataID6", false, "");
RegisterPersistenceVar("toolDataID7", false, "");
RegisterPersistenceVar("toolDataID8", false, "");
RegisterPersistenceVar("toolDataID9", false, "");
RegisterPersistenceVar("toolDataID10", false, "");
RegisterPersistenceVar("toolDataID12", false, "");
RegisterPersistenceVar("toolDataID13", false, "");
RegisterPersistenceVar("toolDataID14", false, "");
RegisterPersistenceVar("toolDataID15", false, "");
RegisterPersistenceVar("toolDataID16", false, "");
RegisterPersistenceVar("toolDataID17", false, "");
RegisterPersistenceVar("toolDataID18", false, "");
RegisterPersistenceVar("toolDataID19", false, "");