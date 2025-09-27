function dropDataIDItem(%client, %slot)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%item = %player.tool[%slot];
	if (isObject(%item))
	{
		if (%item.canDrop)
		{
			%zScale = getWord(%player.getScale(), 2);
			%muzzlepoint = VectorAdd(%player.getPosition(), "0 0" SPC 1.5 * %zScale);
			%muzzlevector = %player.getEyeVector();
			%muzzlepoint = VectorAdd(%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform(%player.getTransform());
			%thrownItem = new Item(""){
				dataBlock = %item;
				dataID = %player.toolDataID[%slot]; //added line here
			};
			%thrownItem.setScale(%player.getScale());
			%player.toolDataID[%slot] = ""; //added line here
			MissionCleanup.add(%thrownItem);
			%thrownItem.setTransform(%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity(VectorScale(%muzzlevector, 20.0 * %zScale));
			%thrownItem.schedulePop();
			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID();
			%thrownItem.setCollisionTimeout(%player);
			if (%item.className $= "Weapon")
			{
				%player.weaponCount = %player.weaponCount - 1.0;
			}
			%player.tool[%slot] = 0;
			messageClient(%client, 'MsgItemPickup', '', %slot, 0);
			if (%player.getMountedImage(%item.image.mountPoint) > 0.0)
			{
				if (%player.getMountedImage(%item.image.mountPoint).getId() == %item.image.getId())
				{
					%player.unmountImage(%item.image.mountPoint);
				}
			}

			logItemAction(%thrownItem.dataID, "dropped", %client.bl_id); //added line here
		}
	}
}

function logItemAction(%dataID, %action, %blid)
{
	if (%dataID $= "" || getWordCount(%action) != 1 || %blid !$= atoi(%blid))
		return false;
	
	%actionString = getSafeVariableName(%action @ "_" @ %blid) @ "_" @ strReplace(getDateTime(), " ", "_");
	%actionLog = getDataIDArrayTagValue(%dataID, "actionLog");
	%newActionLog = getWords(%actionString SPC %actionLog, 0, 3);
	setDataIDArrayTagValue(%dataID, "actionLog", %newActionLog);

	return %newActionLog;
}

package Support_DataIDItem
{
	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0 && isObject(%obj.client))
		{
			%itemDB = %col.getDatablock();
			if (%col.getClassName() $= "Item" && %itemDB.hasDataID)
			{
				%slot = %obj.getFirstEmptySlot();
				if (%slot != -1)
				{
					%obj.toolDataID[%slot] = %col.dataID;
					//this logs item retrievals from chests too
					logItemAction(%col.dataID, "pickedUp", %obj.client.bl_id);
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
			if (%item.hasDataID)
			{
				dropDataIDItem(%cl, %slot);
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
