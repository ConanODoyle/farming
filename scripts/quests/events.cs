function GameConnection::getNewQuest(%client, %requestSlots, %requestTypes, %rewardSlots, %rewardTypes) {
    %player = %client.player;
    if (!isObject(%player)) {
        error("ERROR: No player to drop quest at!");
        return;
    }

    %slot = %player.getFirstEmptySlot();
    if (%slot == -1) {
        %client.chatMessage("Your inventory is full!");
        %client.chatMessage("Make some space for the quest slip before trying again.");
        return;
    }

    %quest = generateQuest(%requestSlots, %requestTypes, %rewardSlots, %rewardTypes);

    %player.farmingAddItem(QuestItem);
    %player.toolDataID[%slot] = %quest;
}

registerOutputEvent("GameConnection", "getNewQuest", "int 1 20 3" TAB "string 200 50" TAB "int 1 20 3" TAB "string 200 50");
