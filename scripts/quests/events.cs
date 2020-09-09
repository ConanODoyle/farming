$Farming::QuestCooldown = 600;
$Farming::QuestAcceptTime = 15;
$Farming::QuestCompleteCooldown = 300;

function fxDTSBrick::getNewQuest(%this, %requestSlots, %requestTypes, %rewardSlots, %rewardTypes, %client) {
    if (%this.nextQuestTime[%client.bl_id] > $Sim::Time) {
        if (%this.questRetrieved[%client.bl_id]) {
            %timeToWait = convTime(%this.nextQuestTime[%client.bl_id] - $Sim::Time);
            commandToClient(%client, 'MessageBoxOK', "Cooldown", "You already retrieved a quest recently!\nTry again in " @ %timeToWait @ ".");
        } else if (isQuest(%this.quest[%client.bl_id])) {
            %client.promptGetQuest(%this, %this.quest[%client.bl_id]);
        } else {
            commandToClient(%client, 'MessageBoxOK', "Error", "Something went wrong. Please try again.");
            %this.nextQuestTime[%client.bl_id] = 0;
        }
    } else {
        %quest = generateQuest(%requestSlots, %requestTypes, %rewardSlots, %rewardTypes);
        %this.quest[%client.bl_id] = %quest;
        %this.nextQuestTime[%client.bl_id] = $Sim::Time + $Farming::QuestCooldown;
        %client.promptGetQuest(%this, %this.quest[%client.bl_id]);
    }
}

function GameConnection::promptGetQuest(%client, %brick, %quest) {
    commandToClient(%client, 'MessageBoxYesNo', "Quest", "Do you want to take this quest?\nYou have " @ convTime($Farming::QuestAcceptTime) @ " to accept this quest.\n\n" @ getQuestString(%quest), 'AcceptQuest');
    %client.questToGet = %quest;
    %client.questGetExpireTime = $Sim::Time + $Farming::QuestAcceptTime;
    %client.questCooldownTime = $Sim::Time + $Farming::QuestCooldown;
    %client.deleteQuestSchedule = schedule($Farming::QuestCooldown * 1000, 0, deleteQuest, %quest);
    %client.questSourceBrick = %brick;
}

function serverCmdAcceptQuest(%client) {
    if (%client.questToGet $= "") {
        commandToClient(%client, 'MessageBoxOK', "A Secret", "Aren't you clever?\nYou found the server command for accepting quests. Too bad it doesn't do anything without a quest to accept...");
        return;
    }
    if ($Sim::Time > %client.questCooldownTime || !isQuest(%client.questToGet)) {
        commandToClient(%client, 'MessageBoxOK', "Quest Expired", "This quest has expired!\nYou must accept quests within " @ convTime($Farming::QuestCooldown) @ ".");
        %client.questToGet = "";
        return;
    }
    if ($Sim::Time > %client.questGetExpireTime) {
        commandToClient(%client, 'MessageBoxOK', "Timeout", "You took too long to accept this quest!\nYou need to accept quests within " @ convTime($Farming::QuestAcceptTime) @ " of opening the prompt.\nThe quest will still be there if you return within " @ convTime($Sim::Time - %client.questCooldownTime) @ ".");
        return;
    }

    %player = %client.player;
    if (!isObject(%player)) {
        commandToClient(%client, 'MessageBoxOK', "No Player", "You need to have spawned to accept quests!\nTry respawning or contact an admin if this message persists.");
        return;
    }

    %slot = %player.getFirstEmptySlot();
    if (%slot == -1) {
        commandToClient(%client, 'MessageBoxOK', "Inventory Full", "Your inventory is full!\nClear some space for the quest slip before trying again.");
        return;
    }

    %quest = %client.questToGet;

    cancel(%client.deleteQuestSchedule);
    %player.farmingAddItem(QuestItem);
    %player.toolDataID[%slot] = %quest;
    %client.questSourceBrick.questRetrieved[%client.bl_id] = true;
}

registerOutputEvent("fxDTSBrick", "getNewQuest", "int 1 20 3" TAB "string 200 50" TAB "int 1 20 3" TAB "string 200 50", true);
