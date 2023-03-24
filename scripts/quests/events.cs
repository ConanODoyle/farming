//////////////////////////////

function selectQuestTable(%list)
{
    %questListLength = getWordCount(%list);
    for (%i = 0; %i < %questListLength; %i++)
    {
        %weight += getWord(%list, %i).questWeight;
    }

    %currWeight = 0;
    %selectedWeight = getRandom() * %weight;
    for (%i = 0; %i < %questListLength; %i++)
    {
        %currWeight += getWord(%list, %i).questWeight;
        if (%selectedWeight < %currWeight)
        {
            return getWord(%list, %i);
        }
    }
    return "";
}

function fxDTSBrick::getNewQuest(%this, %questTables, %timeout, %client) {
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
        %questTable = selectQuestTable(%questTables);
        if (!isObject(%questTable) || %questTable.class !$= "QuestType") {
            talk("ERROR - getNewQuest - Quest brick " @ %this @ " has bad quest table list \"" @ %questTable @ "\"");
            error("ERROR - getNewQuest - Quest brick " @ %this @ " has bad quest table list \"" @ %questTable @ "\"");
            commandToClient(%client, 'MessageBoxOK', "Error", "Something went wrong. Please notify an admin.");
            return;
        }
        %quest = %questTable.generateQuest();

        %this.quest[%client.bl_id] = %quest;
        if (%timeout <= 0)
        {
            %timeout = $Farming::QuestCooldown;
        }
        %this.nextQuestTime[%client.bl_id] = $Sim::Time + %timeout;
        %this.deleteQuestSchedule[%client.bl_id] = schedule($Farming::QuestCooldown * 1000, 0, deleteQuest, %quest);
        %client.promptGetQuest(%this, %this.quest[%client.bl_id]);
        %client.questSourceBrick.questRetrieved[%client.bl_id] = false;
    }
}

function GameConnection::promptGetQuest(%client, %brick, %quest) {
    commandToClient(%client, 'MessageBoxYesNo', "Quest", "Here's the current quest.\n\n" @ getQuestString(%quest) @ "\n\nDo you want to take this quest?\nYou have " @ convTime($Farming::QuestAcceptTime) @ " to accept it.", 'AcceptQuest');
    %client.questToGet = %quest;
    %client.questGetExpireTime = $Sim::Time + $Farming::QuestAcceptTime;
    %client.questCooldownTime = $Sim::Time + $Farming::QuestCooldown;
    %client.questSourceBrick = %brick;
}

function serverCmdAcceptQuest(%client) {
    %player = %client.player;
    %slot = %player.getFirstEmptySlot();
    if (%client.questToGet $= "") {
        commandToClient(%client, 'MessageBoxOK', "A Secret", "Aren't you clever?\nYou found the server command for accepting quests. Too bad it doesn't do anything without a quest to accept...");
    } else if ($Sim::Time > %client.questCooldownTime || !isQuest(%client.questToGet)) {
        commandToClient(%client, 'MessageBoxOK', "Quest Expired", "This quest has expired!\nYou must accept quests within " @ convTime($Farming::QuestCooldown) @ " of generating them.");
    } else if ($Sim::Time > %client.questGetExpireTime) {
        commandToClient(%client, 'MessageBoxOK', "Timeout", "You took too long to accept this quest!\nYou need to accept quests within " @ convTime($Farming::QuestAcceptTime) @ " of opening the prompt. The quest will still be there if you return within " @ convTime(%client.questCooldownTime - $Sim::Time) @ ".");
    } else if (!isObject(%player)) {
        commandToClient(%client, 'MessageBoxOK', "No Player", "You need to have spawned to accept quests!\nTry respawning or contact an admin if this message persists.");
    } else if (%slot == -1) {
        commandToClient(%client, 'MessageBoxOK', "Inventory Full", "Your inventory is full!\nClear some space for the quest slip before trying again.");
    } else {
        %quest = %client.questToGet;

        cancel(%client.questSourceBrick.deleteQuestSchedule[%client.bl_id]);
        %player.farmingAddItem(QuestItem);
        %player.toolDataID[%slot] = %quest;
        %client.questSourceBrick.questRetrieved[%client.bl_id] = true;
    }
    %client.questToGet = "";
}

registerOutputEvent("fxDTSBrick", "getNewQuest", "string 200 150" TAB "int 0 100000 0", true);

//////////////////////////////

function fxDTSBrick::displayActiveQuest(%this, %depositBoxArray, %client) {
    if (%depositBoxArray $= "") {
        %this.EventOutputParameter[0, 1] = $Farming::QuestDepositPointPrefix @ getRandomHash("depositPoint");
    }
    %depositBoxArray = %this.EventOutputParameter[0, 1];
    %quest = getDataIDArrayTagValue(%depositBoxArray, "BL_ID" @ %client.bl_id @ "Quest");
    if (!isQuest(%quest)) {
        commandToClient(%client, 'MessageBoxOK', "No Assigned Quest", "There's no quest assigned to this deposit box.\nDrop a quest here to assign it to the box!");
        return;
    }
    %str = "Here's the quest you've assigned to this deposit box.\n\n" @ getQuestString(%quest, true) @ "\n\nDo you want a new copy of this quest?\nYou have " @ convTime($Farming::QuestAcceptTime) @ " to get a copy.";
    for (%i = 0; %i < 4; %i++)
    {
        %str[%i] = getSubStr(%str, %i * 250, 250);
    }
    commandToClient(%client, 'MessageBoxYesNo', "Current Quest", '%2%3%4%5', 'getQuestCopy', %str0, %str1, %str2, %str3);
    %client.questToCopy = %quest;
    %client.questCopyTimeout = $Sim::Time + $Farming::QuestAcceptTime;
}

function serverCmdGetQuestCopy(%client) {
    %player = %client.player;
    %slot = %player.getFirstEmptySlot();
    if (%client.questCopyTimeout $= "" || %client.questToCopy $= "") {
        commandToClient(%client, 'MessageBoxOK', "A Secret", "Hey, you found a server command!\nThis one's for copying quests, but it won't do anything without a quest to copy...");
    } else if ($Sim::Time > %client.questCopyTimeout) {
        commandToClient(%client, 'MessageBoxOK', "Timeout", "You took too long to copy the quest!\nYou have to accept within " @ convTime($Farming::QuestAcceptTime) @ " to get a copy of a quest.");
    } else if (!isObject(%player)) {
        commandToClient(%client, 'MessageBoxOK', "No Player", "You need to have spawned to copy quests!\nTry respawning or contact an admin if this message persists.");
    } else if (%slot == -1) {
        commandToClient(%client, 'MessageBoxOK', "Inventory Full", "Your inventory is full!\nClear some space for the quest slip before trying again.");
    } else if (!isQuest(%client.questToCopy)) {
        commandToClient(%client, 'MessageBoxOK', "Invalid Quest", "This quest is invalid!\nTell an admin about this message if you see it - something's gone wrong.");
    } else {
        %player.farmingAddItem(QuestItem);
        %player.toolDataID[%slot] = %client.questToCopy;
    }
    %client.questCopyTimeout = "";
    %client.questToCopy = "";
    return;
}

registerOutputEvent("fxDTSBrick", "displayActiveQuest", "string 200 500", true);

//////////////////////////////
