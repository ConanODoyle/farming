// TODO: brick restriction
// TODO: 1x2f 2x2f


// TODO: refactor holy crap
// TODO: tool rack?
// TODO: modifiable item positions via wrench without unrestricting everything else about the wrench


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
};
activatePackage(ItemStands);

function fxDTSBrick::updateItemStandDisplay(%brick)
{
	if (!%brick.storageObj.getDatablock().isItemStand)
	{
		return;
	}

    %dataID = %brick.eventOutputParameter[0, 1];
	%storageObj = %brick.storageObj;

	//get storage data
	%max = %storageObj.getDatablock().storageSlotCount;
	%start = 1; //slot 0 is information

	%count = 0;
	for (%i = %start; %i < %max + 1; %i++)
	{
		%data[%count] = validateStorageValue(getDataIDArrayValue(%dataID, %i));
		%dataBlock = getField(%data[%count], 0);
		%count++;
	}    

	%rotation = getWords(%brick.getTransform(), 3, 6);
	%rotation = getWords(%rotation, 0, 2) SPC (getWord(%rotation, 3) + $pi);
	for (%i = 0; %i < %count; %i++)
	{
		%currPos = %brick.getDatablock().itemPos[%i];
		switch(%brick.angleID)
		{
			case 0: %currPos = %currPos;
			// case 1: %currPos = getWord(%currPos, 1) SPC -1 * getWord(%currPos, 0) SPC getWord(%currPos, 2);
			// case 2: %currPos = -1 * getWord(%currPos, 0) SPC -1 * getWord(%currPos, 1) SPC getWord(%currPos, 2);
			// case 3: %currPos = -1 * getWord(%currPos, 1) SPC getWord(%currPos, 0) SPC getWord(%currPos, 2);
		}
		%currPos = vectorAdd(%brick.getPosition(), %currPos);
		%currPos = vectorAdd(%currPos, "0 0 0.2");
		// %p = createBoxMarker(%currPos, "0 0 1 1", "0.5 0.5 0");
		// %p.schedule(1000, delete);
		%dataBlock = getField(%data[%i], 0);
		// %itemCount = getField(%data[%i], 2);
		// %dataID = getField(%data[%i], 3);

		if (isObject(%dataBlock))
		{
			%item = %brick.itemStandDisplayItem[%i];
			if (!isObject(%item))
			{
				%item = %brick.itemStandDisplayItem[%i] = new Item(ItemStandDisplayItems)
				{
					dataBlock = %dataBlock;
					static = 1;
					isItemStandItem = 1;
				};
			}
			else
			{
				%brick.itemStandDisplayItem[%i].setDatablock(%dataBlock);
			}

			if (%dataBlock.doColorShift)
			{
				%item.setNodeColor("ALL", %dataBlock.colorShiftColor);
			}

			%realObjectBox = %item.getWorldBox();
			%realObjectBox = vectorSub(getWords(%realObjectBox, 3, 5), getWords(%realObjectBox, 0, 2));

			%offset = vectorSub(%item.getTransform(), %item.getWorldBoxCenter());
			%offset = vectorAdd(%offset, 0 SPC 0 SPC getWord(%realObjectBox, 2) / 2);

			//need to make the bottom of the item be the %currPos
			%item.setTransform(vectorAdd(%currPos, %offset) SPC %rotation);
		}
		else
		{
			if (isObject(%brick.itemStandDisplayItem[%i]))
			{
				%brick.itemStandDisplayItem[%i].delete();
			}
		}
	}
}
