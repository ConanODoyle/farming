function GameConnection::getNewQuest(%client, %requestSlots, %requestTypes, %rewardSlots, %rewardTypes) {
    %player = %client.player;
    if (!isObject(%player)) {
        error("ERROR: No player to drop quest at!");
        return;
    }

    %slot = %player.getFirstEmptySlot();
    if (%slot == -1) {
        %client.centerPrint("Your inventory is full!\nMake some space for the quest slip before trying again.", 3);
        return;
    }

    %quest = generateQuest(%requestSlots, %requestTypes, %rewardSlots, %rewardTypes);

    %player.farmingAddItem(QuestItem);
    %player.toolDataID[%slot] = %quest;
}
