// item should have the ID of the desired item
function Player::farmingSetItem(%player, %datablock, %slot) {
    if (%player.getDatablock().maxTools <= %slot) {
        return 0;
    }

    %player.tool[%slot] = %datablock;

    if (isObject(%client = %player.client)) {
        messageClient(%client, 'MsgItemPickup', "", %slot, %datablock);

        if (!isObject(%player.tool[%player.currTool]) || %player.currTool == -1) {
            serverCmdUnUseTool(%client);
        } else {
            serverCmdUseTool(%client, %player.currTool);
        }
    }
}

function Player::getFirstEmptySlot(%player) {
    %maxTools = %player.getDatablock().maxTools;
    for (%i = 0; %i < %maxTools; %i++) {
        if (!isObject(%player.tool[%i])) {
            return %i;
        }
    }
    return "";
}

function farmingItemOverflow(%player, %datablock) {
    %overflowItem = new Item() {
        dataBlock = %datablock;
    };
    MissionCleanup.add(%overflowItem);
    %overflowItem.setTransform(%player.getTransform());
    %overflowItem.schedulePop();
    %overflowItem.miniGame = %player.client.miniGame;
    %overflowItem.bl_id = %player.client.bl_id;
    %overflowItem.setCollisionTimeout(%player);
    if (%dataBlock.doColorShift) {
        %overflowItem.setNodeColor("ALL", %datablock.colorShiftColor);
    }
    return %overflowItem;
}

function Player::farmingAddItem(%player, %datablock, %ignoreOverflow) {
    %emptySlot = %player.getFirstEmptySlot();
    if (%emptySlot $= "") {
        if (!%ignoreOverflow) {
            farmingItemOverflow(%player, %datablock);
        } else {
            return 0;
        }
    }

    return %player.farmingSetItem(%player, %datablock, %emptySlot);
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
    %itemDatablock = getItemFromStack(%stackType, %count);
    %overflowItem = farmingItemOverflow(%player, %itemDatablock);
    %overflowItem.count = %count;
    return %overflowItem;
}

// return values:
// 0: can't pick any up, dropped all
// 1: picked all up
// 2: picked some up, dropped the rest
function Player::farmingAddStackableItem(%player, %datablock, %count, %ignoreOverflow) {
    if (!%datablock.isStackable) {
        error("ERROR: Attempted to treat unstackable item as stackable when adding to player!");
        return 0;
    }

    %stackType = %datablock.stackType;
    %maxStackSize = getMaxStack(%stackType);

    for (%slot = %player.getFirstStackableSlot(%stackType); %count > 0 && %slot !$= ""; %slot = %player.getFirstStackableSlot(%stackType, %slot + 1)) {
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

        %tool = getItemFromStack(%stackType, %player.toolStackCount[%slot]).getID();

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

    if (%count > 0) {
        return 2;
    } else {
        return 1;
    }
}
