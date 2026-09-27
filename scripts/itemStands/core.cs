// TODO: brick restriction
// TODO: tool rack?

// TODO: offset bug on wrenchdata: probably due to order of calling calcitempos and calcitemdir

$Farming::MaxItemStands = 30;

// shoddy work rip from shop stalls im sorry
package ItemStands
{
	function fxDTSBrick::accessStorage(%brick, %dataID, %cl)
	{
		if (isObject(%brick.vehicle.storageBot))
		{
			%storageObj = %brick.vehicle.storageBot;
		}
		else if (isObject(%brick.vehicle) && %brick.vehicle.getDatablock().isStorageVehicle)
		{
			%storageObj = %brick.vehicle;
		}
		else
		{
			%storageObj = %brick;
		}
		%brick.storageObj = %storageObj;

        %brick.updateItemStandDisplay();

		Parent::accessStorage(%brick, %dataID, %cl);
	}

	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot) 
	{
		if (!isObject(%storageObj) || !%storageObj.getDatablock().isItemStand) return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot);

		%ret = parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID, %specificSlot);

		if (%ret == 2) 
        {
            return %ret;
        }
        
        %brick.storageObj = %storageObj;

		%brick.updateItemStandDisplay();
        %brick.updateItemStandDisplay(); // BUG: item position is offset unless this is called twice???????????? find  a proper fix?????

		return %ret;
	}

	function removeStack(%cl, %menu, %option)
	{
		%storageObj = %menu.brick.storageObj;

		if (!isObject(%storageObj) || !%storageObj.getDatablock().isItemStand) return parent::removeStack(%cl, %menu, %option);

		%ret = parent::removeStack(%cl, %menu, %option);

		if (%ret == 2) return %ret;

		%menu.brick.updateItemStandDisplay();

		return %ret;
	}

	function fxDTSBrick::onDeath(%this, %obj)
	{
		if (isObject(%this.storageObj))
		{
			if (%this.storageObj.getDatablock().isItemStand)
			{
                %count = %this.storageObj.getDatablock().storageSlotCount;
				for (%i = 0; %i < %count; %i++)
				{
					if (isObject(%this.itemStandDisplayItem[%i]))
					{
						%this.itemStandDisplayItem[%i].delete();
					}
				}
			}
		}
		return parent::onDeath(%this, %obj);
	}

	function fxDTSBrick::onRemove(%this, %obj)
	{
		if (isObject(%this.storageObj))
		{
			if (%this.storageObj.getDatablock().isItemStand)
			{
				for (%i = 0; %i < %this.storageObj.getDatablock().storageSlotCount; %i++)
				{
					if (isObject(%this.itemStandDisplayItem[%i]))
					{
						%this.itemStandDisplayItem[%i].delete();
					}
				}
			}
		}
		return parent::onRemove(%this, %obj);
	}

	function fxDTSBrick::onAdd(%this, %obj)
	{
		if (isObject(%this.storageObj))
		{
			if (%this.storageObj.getDatablock().isItemStand)
			{
				%this.schedule(1000, updateItemStandDisplay);
			}
		}
		return parent::onAdd(%this, %obj);
	}

	function Armor::onCollision(%this, %obj, %col, %vec, %speed)
	{
		if (%col.getClassName() $= "Item" && %col.isItemStandItem)
		{
			return;
		}
		return parent::onCollision(%this, %obj, %col, %vec, %speed);
	}

	function serverCmdSetWrenchData(%cl, %data)
	{
		parent::serverCmdSetWrenchData(%cl, %data);

		if (isObject(%cl.wrenchBrick))
		{
			%db = %cl.wrenchBrick.getDatablock();
			if (%db.isItemStand)
			{
				%cl.wrenchBrick.updateItemStandDisplay(); // BUG: same bug, offset without double calling?
				%cl.wrenchBrick.updateItemStandDisplay();
			}
		}
	}
};
activatePackage(ItemStands);

function fxDTSBrick::updateItemStandDisplay(%brick, %dir)
{
	if (!%brick.storageObj.getDatablock().isItemStand)
	{
		return;
	}

    %dataID = %brick.eventOutputParameter[0, 1];
	%storageObj = %brick.storageObj;

	//get storage data
	%max = %storageObj.getDatablock().storageSlotCount;
	%start = 1; //slot 0 is information+

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%count++;
	} 

	if (%count == 1)
	{	
		%dataBlock = getField(%data[0], 0);
		if (!isObject(%dataBlock) && isObject(%brick.itemStandDisplayItem[0]))
		{
			%brick.itemStandDisplayItem[0].delete();
		}
		else
		{
			%dataBlock = getField(%data[0], 0);
			if (isObject(%dataBlock))
			{
				%item = %brick.itemStandDisplayItem[0];
				if (!isObject(%item))
				{
					%item = %brick.itemStandDisplayItem[0] = new Item(ItemStandDisplayItems)
					{
						dataBlock = %dataBlock;
						static = 1;
						isItemStandItem = 1;
					};
				}
				else
				{

					%brick.itemStandDisplayItem[0].setDatablock(%dataBlock);
				}

				if (%dataBlock.doColorShift)
				{
					%item.setNodeColor("ALL", %dataBlock.colorShiftColor);
				}
				%item.setTransform(%brick.calcItemPosition(%item));
			}
		}
	}
}

function GameConnection::itemStandLimitCheck(%cl)
{
	if (!isObject(%cl))
	{
		return 2;
	}

	%timeSinceCheck = $sim::time - %cl.lastLimitCheckTime;
	if (%timeSinceCheck < 8)
	{
		if (%cl.activeItemStands >= $Farming::MaxItemStands)
		{
			return 2;
		}
		else if (%cl.activeItemStands > 0 && %cl.activeItemStands < $Farming::MaxItemStands)
		{
			%cl.activeItemStands++;
			return 1;
		}
	}

	%cl.activeItemStands = 0;
	%cl.lastLimitCheckTime = $sim::time;

	%brickGroup = %cl.brickGroup;
	for (%i = 0; %i < %brickGroup.getCount(); %i++)
	{
		%db = %brickGroup.getObject(%i).getDatablock();
		if (%db.isItemStand)
		{
			%cl.activeItemStands++;
		}

		if (%cl.activeItemStands >= $Farming::MaxItemStands)
		{
			return 2;
		}
	}
	return 1;
}

function fxDTSBrick::calcItemPosition(%obj, %item)
{
	%dir = %obj.itemPosition;
	// %obj.itemPosition = %dir;
	if (!isObject(%item))
	{
		return;
	}
	%itemBox = %item.getWorldBox();
	%itemBoxX = mAbs(getWord(%itemBox, 0) - getWord(%itemBox, 3)) / 2;
	%itemBoxY = mAbs(getWord(%itemBox, 1) - getWord(%itemBox, 4)) / 2;
	%itemBoxZ = mAbs(getWord(%itemBox, 2) - getWord(%itemBox, 5)) / 2;
	%itemBoxCenter = %item.getWorldBoxCenter();
	%itemCenter = %item.getPosition();
	%itemOffset = VectorSub(%itemCenter, %itemBoxCenter);
	%brickBox = %obj.getWorldBox();
	%brickBoxX = mAbs(getWord(%brickBox, 0) - getWord(%brickBox, 3)) / 2;
	%brickBoxY = mAbs(getWord(%brickBox, 1) - getWord(%brickBox, 4)) / 2;
	%brickBoxZ = mAbs(getWord(%brickBox, 2) - getWord(%brickBox, 5)) / 2;
	%pos = %obj.getPosition();
	%pos = VectorAdd(%pos, %itemOffset);
	%posX = getWord(%pos, 0);
	%posY = getWord(%pos, 1);
	%posZ = getWord(%pos, 2);
	%rot = %obj.calcItemDirection(%item);
	if (%dir == 0)
	{
		%posZ += %itemBoxZ + %brickBoxZ;
	}
	else if (%dir == 1)
	{
		%posZ -= %itemBoxZ + %brickBoxZ;
	}
	else if (%dir == 2)
	{
		%posY += %itemBoxY + %brickBoxY;
	}
	else if (%dir == 3)
	{
		%posX += %itemBoxX + %brickBoxX;
	}
	else if (%dir == 4)
	{
		%posY -= %itemBoxY + %brickBoxY;
	}
	else if (%dir == 5)
	{
		%posX -= %itemBoxX + %brickBoxX;
	}
	return %posX SPC %posY SPC %posZ SPC %rot;
}

function fxDTSBrick::calcItemDirection(%obj, %item)
{
	%dir = %obj.itemDirection;
	if (!isObject(%item))
	{
		return;
	}
	%pos = getWords(%item.getTransform(), 0, 2);
	if (%dir == 2)
	{
		%rot = "0 0 1 0";
	}
	else if (%dir == 3)
	{
		%rot = "0 0 1 " @ $piOver2;
	}
	else if (%dir == 4)
	{
		%rot = "0 0 -1 " @ $pi;
	}
	else if (%dir == 5)
	{
		%rot = "0 0 -1 " @ $piOver2;
	}
	else
	{
		%rot = "0 0 1 0";
	}
	return %rot;
}