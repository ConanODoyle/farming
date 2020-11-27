// item should have the ID of the desired item
function Player::farmingSetItem(%player, %datablock, %slot, %dataID) {
    if (%player.getDatablock().maxTools <= %slot || %slot == -1) {
        return 0;
    }

    if (!isObject(%datablock)) {
        error("ERROR: Datablock" SPC %datablock SPC "is invalid");
        return 0;
    }

    if (%datablock + 0 == 0) {
        %datablock = %datablock.getID();
    }

    %player.tool[%slot] = %datablock;
    %player.toolDataID[%slot] = "";
    if (%dataID !$= "")
    {
        %player.toolDataID[%slot] = %dataID;
    }

    if (isObject(%client = %player.client)) {
        messageClient(%client, 'MsgItemPickup', "", %slot, %datablock);

        if (!isObject(%player.tool[%player.currTool]) || %player.currTool == -1) {
            serverCmdUnUseTool(%client);
        } else {
            serverCmdUseTool(%client, %player.currTool);
        }
    }
    return 1;
}

function Player::getFirstEmptySlot(%player) {
    %maxTools = %player.getDatablock().maxTools;
    for (%i = 0; %i < %maxTools; %i++) {
        if (!isObject(%player.tool[%i])) {
            return %i;
        }
    }
    return -1;
}

function farmingItemOverflow(%player, %datablock, %dataID) {
    %overflowItem = new Item() {
        dataBlock = %datablock;
    };
    MissionCleanup.add(%overflowItem);
    %overflowItem.setTransform(%player.getTransform());
    %overflowItem.schedulePop();
    %overflowItem.miniGame = %player.client.miniGame;
    %overflowItem.bl_id = %player.client.bl_id;
    %overflowItem.dataID = %dataID;
    %overflowItem.setCollisionTimeout(%player);
    if (%dataBlock.doColorShift) {
        %overflowItem.setNodeColor("ALL", %datablock.colorShiftColor);
    }
    return %overflowItem;
}

function Player::farmingAddItem(%player, %datablock, %dataID, %ignoreOverflow) {
    if (!isObject(%datablock)) {
        error("ERROR: Datablock" SPC %datablock SPC "is invalid");
        return 0;
    }

    if (%datablock + 0 == 0) {
        %datablock = %datablock.getID();
    }

    %emptySlot = %player.getFirstEmptySlot();
    if (%emptySlot == -1) {
        if (!%ignoreOverflow) {
            return farmingItemOverflow(%player, %datablock, %dataID);
        } else {
            return 0;
        }
    }

    return %player.farmingSetItem(%datablock, %emptySlot, %dataID);
}

function Player::getFirstStackableSlot(%player, %stackType, %from) {
    %maxTools = %player.getDatablock().maxTools;
    for (%i = %from + 0; %i < %maxTools; %i++) {
        if (!isObject(%toolDatablock = %player.tool[%i])) {
            return %i;
        } else if (%toolDatablock.stackType $= %stackType) {
            %maxStackSize = getMaxStack(%stackType);
            if (%player.toolStackCount[%i] < %maxStackSize) {
                return %i;
            }
        }
    }
    return "";
}

function farmingStackableItemOverflow(%player, %stackType, %count) {
    %itemDatablock = getStackTypeDatablock(%stackType, %count);
    %overflowItem = farmingItemOverflow(%player, %itemDatablock);
    %overflowItem.count = %count;
    return %overflowItem;
}

// return values:
// 0: picked none up or errored
// 1: picked all up
// 2: picked some up
function Player::farmingAddStackableItem(%player, %item, %count, %ignoreOverflow) {
    if (!(isStackType(%item) || (isObject(%item) && %item.isStackable))) {
        error("ERROR: Attempted to treat unstackable item as stackable when adding to player!");
        return 0;
    }

    %stackType = isStackType(%item) ? %item : %item.stackType;
    %maxStackSize = getMaxStack(%stackType);

    for (%slot = %player.getFirstStackableSlot(%stackType); %count > 0 && %slot !$= ""; %slot = %player.getFirstStackableSlot(%stackType, %slot + 1)) {
        %success = 1;
        %oldTool = %player.tool[%slot];

        if (!isObject(%oldTool)) {
            %player.toolStackCount[%slot] = 0;
        }

        %diff = %maxStackSize - %player.toolStackCount[%slot];
        %left = %count - %diff;

        if (%left < 0) {
            %player.toolStackCount[%slot] += %count;
            %count = 0;
        } else {
            %player.toolStackCount[%slot] = %maxStackSize;
            %count = %left;
        }

        %tool = getStackTypeDatablock(%stackType, %player.toolStackCount[%slot]).getID();

        if (%oldTool != %tool) {
            %player.tool[%slot] = %tool;
            messageClient(%player.client, 'MsgItemPickup', "", %slot, %tool);
        } else {
            messageClient(%player.client, 'MsgItemPickup');
        }
    }

    if (!isObject(%player.tool[%player.currTool]) || %player.currTool == -1) {
        serverCmdUnUseTool(%client);
    } else {
        serverCmdUseTool(%client, %player.currTool);
    }

    if (%count > 0 && !%ignoreOverflow) {
        farmingStackableItemOverflow(%player, %stackType, %count);
    }

    if (!%success) {
        return 0;
    }

    if (%count > 0) {
        return 2;
    }

    return 1;
}
