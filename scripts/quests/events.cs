function GameConnection::getNewQuest(%client, %requestSlots, %requestTypes, %rewardSlots, %rewardTypes) {
    %player = %client.player;
    if (!isObject(%player)) {
        error("ERROR: No player to drop quest at!");
        return;
    }

    %slot = %player.getFirstEmptySlot();
    if (%slot == -1) {
        messageClient(%client, 'MsgUploadEnd', "Your inventory is full. Please make some space before trying to get a quest.");
        return;
    }

    %quest = generateQuest(%requestSlots, %requestTypes, %rewardSlots, %rewardTypes);

    %player.farmingAddItem(QuestItem);
    %player.toolDataID[%slot] = %quest;
}
