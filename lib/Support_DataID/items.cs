package Support_DataIDItem
{
	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0 && isObject(%obj.client))
		{
			%itemDB = %col.getDatablock();
			if (%col.getClassName() $= "Item" && %itemDB.isDataIDObject)
			{
				%slot = %obj.getFirstEmptySlot();
				if (%slot != -1)
				{
					%obj.toolDataID[%slot] = %col.dataID;
				}
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
