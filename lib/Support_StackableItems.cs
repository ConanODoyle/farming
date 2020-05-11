//item:
//isStackable = 1;
//stackType = string;

//$Stackable_stackType_stackedItem0 = datablock SPC max;
//$Stackable_stackType_stackedItem1 = datablock SPC max;
//$Stackable_stackType_stackedItem2 = datablock SPC max;
//etc...
//$Stackable_stackType_stackedItemTotal = 3;
function getMaxStack(%stackType)
{
	%idxMax = $Stackable_[%stackType, "stackedItemTotal"];
	return getWord($Stackable_[%stackType, "stackedItem" @ %idxMax - 1], 1);
}

function getMaxPickup(%pl, %stackType)
{
	%absoluteMax = getMaxStack(%stackType);

	if (%absoluteMax $= "")
	{
		%requireEmpty = 1;
	}

	%total = 0;
	for (%i = 0; %i < %pl.getDatablock().maxTools; %i++)
	{
		%curr = %pl.tool[%i];
		if (!isObject(%curr))
		{
			if (%requireEmpty)
			{
				%totalEmpty++;
			}

			%total += %absoluteMax;
		}

		if (isObject(%curr) && %curr.stackType $= %stackType && !%requireEmpty)
		{
			//stacked item detected, check if it can carry more items
			%amt = %pl.toolStackCount[%i];
			if (%amt < %absoluteMax)
			{
				%total += %absoluteMax - %amt;
			}
		}
	}

	if (%requireEmpty)
	{
		return %totalEmpty TAB 1;
	}
	else
	{
		return %total;
	}
}

//returns 0 for cannot pickup
//returns 1 SPC %slot for can pickup completely
//returns 2 SPC %slot SPC %amt for partial pickup (overflow)
function stackedCanPickup(%pl, %item)
{
	%plDB = %pl.getDatablock();
	%itemDB = %item.getDatablock();

	if (!%itemDB.isStackable || !%item.canPickup)
	{
		return 0;
	}

	if (%itemDB.stackType $= "" || $Stackable_[%itemDB.stackType, "stackedItemTotal"] $= "")
	{
		error("ERROR: " @ %itemDB.stackType @ " stackable item type config is incomplete!");
		return 0;
	}

	%count = %item.count;
	if (%count <= 0)
	{
		%item.count = 1;
		%count = 1;
	}

	%absoluteMax = getMaxStack(%itemDB.stackType);

	for (%i = 0; %i < %plDB.maxTools; %i++)
	{
		%curr = %pl.tool[%i];
		if (!isObject(%curr) && %empty $= "")
		{
			%empty = %i;
			%pl.toolStackCount[%i] = 0;
		}

		if (isObject(%curr) && %curr.stackType $= %itemDB.stackType)
		{
			//stacked item detected, check if it can carry more items
			%total = %pl.toolStackCount[%i];
			if (%total < %absoluteMax)
			{
				//can hold more, save this slot
				%stackedSlot = %i;
				//break since we found a valid spot
				break;
			}
		}
	}

	if (%empty $= "" && %stackedSlot $= "")
	{
		//no valid spaces
		return 0;
	}
	else if (%stackedSlot !$= "")
	{
		//stack item slot is available
		//absmax = 500, total = 490, count = 10
		%total = %pl.toolStackCount[%stackedSlot];
		if (%count <= %absoluteMax - %total)
		{
			//enough space is available to take the entire item stack
			return 1 SPC %stackedSlot SPC getMin(%count, %absoluteMax);
		}
		else
		{
			//not enough space, include the difference in return val
			return 2 SPC %stackedSlot SPC %absoluteMax - %total;
		}
	}
	else
	{
		//no stack item slot is available, but empty slot is available
		return 1 SPC %empty SPC getMin(%count, %absoluteMax);
	}
}

function getItemFromStack(%stackType, %count) {
	%itemCount = $Stackable_[%stackType, "stackedItemTotal"];
	for (%i = 0; %i < %itemCount; %i++)
	{
		%currPair = $Stackable_[%stackType, "stackedItem" @ %i];
		if (%count <= getWord(%currPair, 1))
		{
			return getWord(%currPair, 0);
		}
	}
	return getWord($Stackable_[%stackType, "stackedItem" @ %itemCount - 1], 0);
}

function pickupStackableItem(%pl, %item, %slot, %amt)
{
	if ((%pl.tool[%slot].stackType !$= %item.getDatablock().stackType && isObject(%pl.tool[%slot])) || %item.getDatablock().stackType $= "")
	{
		return;
	}

	if (!isObject(%pl.tool[%slot]))
	{
		%pl.toolStackCount[%slot] = 0;
	}

	%type = %item.getDatablock().stackType;

	%pl.toolStackCount[%slot] += %amt;
	//figure out which item to give to the player
	%bestItem = getItemFromStack(%type, %amt);

	// talk(%bestItem.getID() @ " vs " @ %pl.tool[%slot]);
	if (!isObject(%bestItem))
	{
		talk("ERROR: BestItem not found! " @ %pl.client.name SPC %item SPC %slot SPC %amt);
		return;
	}
	if (%bestItem.getID() != %pl.tool[%slot])
	{
		%pl.tool[%slot] = %bestItem.getID();
		messageClient(%pl.client, 'MsgItemPickup', '', %slot, %bestItem.getID());

		if (%pl.currTool == %slot)
		{
			%pl.mountImage(%bestItem.image, 0);
		}
	}
	else
	{
		messageClient(%pl.client, 'MsgItemPickup', '');
	}

	%item.count -= %amt;
	if (%item.count == 0)
	{
		if (%item.isStatic())
		{
			%item.respawn();
		}
		else
		{
			%item.delete();
			return;
		}
	}
	else if (%item.count < 0 && !%item.isStatic())
	{
		error("ERROR: Player picked up more amount than item has! Count: " @ %item.count + %amt @ " Amt: " @ %amt);
		%item.canPickup = 0;
	}
	else
	{
		//figure out which itemDB to set the dropped item to
		%bestItem = getItemFromStack(%type, %item.count);

		if (isObject(%bestItem))
		{
			%item.setDatablock(%bestItem);
			if (%bestItem.doColorShift)
			{
				%item.setNodeColor("ALL", %bestItem.colorShiftColor);
			}
		}
	}
	//item still has count left, leave it in existence
	%item.setCollisionTimeout(%pl);
}

//code copied from default serverCmdDropTool
//added lines are marked
function dropStackableItem(%client, %position)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%item = %player.tool[%position];
	if (isObject(%item))
	{
		if (%item.canDrop == 1.0)
		{
			%zScale = getWord(%player.getScale(), 2);
			%muzzlepoint = VectorAdd(%player.getPosition(), "0 0" SPC 1.5 * %zScale);
			%muzzlevector = %player.getEyeVector();
			%muzzlepoint = VectorAdd(%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform(%player.getTransform());
			%thrownItem = new Item(""){
				dataBlock = %item;
				count = %player.toolStackCount[%position]; //added line here
			};
			%thrownItem.setScale(%player.getScale());
			%player.toolStackCount[%position] = 0; //added line here
			MissionCleanup.add(%thrownItem);
			%thrownItem.setTransform(%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity(VectorScale(%muzzlevector, 20.0 * %zScale));
			// %thrownItem.schedulePop(); //commented out this line (dont want crops to despawn so quickly)
			%thrownItem.schedule(60000, schedulePop); //added this to delay stacked item despawn
			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID();
			%thrownItem.setCollisionTimeout(%player);
			if (%item.className $= "Weapon")
			{
				%player.weaponCount = %player.weaponCount - 1.0;
			}
			%player.tool[%position] = 0;
			messageClient(%client, 'MsgItemPickup', '', %position, 0);
			if (%player.getMountedImage(%item.image.mountPoint) > 0.0)
			{
				if (%player.getMountedImage(%item.image.mountPoint).getId() == %item.image.getId())
				{
					%player.unmountImage(%item.image.mountPoint);
				}
			}
		}
	}
}

package Support_StackableItems
{
	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0 && isObject(%obj.client))
		{
			%itemDB = %col.getDatablock();
			if (%col.getClassName() $= "Item" && %itemDB.isStackable)
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
activatePackage(Support_StackableItems);

function checkGroupStackable(%item, %times)
{
    if (%times > 3)
    {
        return;
    }

    %pos = %item.getPosition();
    %radius = 1;
    %stackType = %item.getDatablock().stackType;
    if (%stackType $= "")
    {
        return;
    }

    if (%item.count <= 0)
    {
    	%item.count = 1;
    }

    initContainerRadiusSearch(%pos, %radius, $TypeMasks::ItemObjectType);
    while (isObject(%next = containerSearchNext()))
    {
        if (%next == %item)
        {
            continue;
        }

        if (!%next.static && %next.getDatablock().stackType $= %stackType)
        {
        	if (%next.count <= 0)
        	{
        		%next.count = 1;
        	}

            %item.count += %next.count;
            %next.delete();
        }
    }

    schedule(1000, %item, checkGroupStackable, %item, %times++);
}
