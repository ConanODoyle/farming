$Farming::QuestPrefix = "Quest_";

function getQuestItem(%table, %slots) {
	%item = farmingTableGetItem(%table);
	%count = %slots;

	if (%item.isStackable) {
		%perSlotCount = getMaxStack(%item.stackType);
		%count = %perSlotCount * (%slots - 1) + getRandom(mFloor(%perSlotCount / 2), %perSlotCount);
	}

	return %item SPC %count;
}

function generateQuest(%requestSlots, %requestTypes, %rewardSlots, %rewardTypes) {
	%questID = $Farming::QuestPrefix @ getRandomHash("quest");

	%numRequests = getMin(%requestSlots, getRandom(1, 3));
	for (%numSpareRequestSlots = %requestSlots - %numRequests; %numSpareRequestSlots > 0; %numSpareRequestSlots--) {
		%i = getRandom(0, %numRequests - 1);
		%extraRequestSlots[%i] += 1;
	}

	%numRewards = getMin(%rewardSlots, getRandom(1, 3));
	for (%numSpareRewardSlots = %rewardSlots - %numRewards; %numSpareRewardSlots > 0; %numSpareRewardSlots--) {
		%i = getRandom(0, %numRewards - 1);
		%extraRewardSlots[%i] += 1;
	}

	setDataIDArrayCount(%questID, %numRequests + %numRewards);
	setDataIDArrayTagValue(%questID, "numRequests", %numRequests);
	setDataIDArrayTagValue(%questID, "numRewards", %numRewards);

	for (%i = 0; %i < %numRequests; %i++) {
		%tableIndex = getRandom(0, getFieldCount(%requestTypes) - 1);
		%table = getField(%requestTypes, %tableIndex);
		%requestTypes = removeField(%requestTypes, %tableIndex);

		%request = getQuestItem(%table, 1 + %extraRequestSlots[%i]);

		addToDataIDArray(%questID, %request);
	}

	for (%i = 0; %i < %numRewards; %i++) {
		%tableIndex = getRandom(0, getFieldCount(%rewardTypes) - 1);
		%table = getField(%rewardTypes, %tableIndex);
		%rewardTypes = removeField(%rewardTypes, %tableIndex);

		%reward = getQuestItem(%table, 1 + %extraRewardSlots[%i]);

		addToDataIDArray(%questID, %reward);
	}

	// TODO: write cash reward system for quests
	setDataIDArrayTagValue(%questID, "cashReward", 0);

	return %questID;
}

function GameConnection::questDeliverItem(%client, %questID, %deliveredItem, %deliveredCount) {
	%player = %client.player;
	if (!isObject(%player)) {
		error("ERROR: Player does not exist, can't deliver quest item!");
		return;
	}

	if (%deliveredCount $= "") {
		%deliveredCount = 1;
	}

	%numRequests = getDataIDArrayTagValue(%questID, "numRequests");

	for (%i = 0; %i < %numRequests; %i++) {
		%request = getDataIDArrayValue(%questID, %i);
		%item = getWord(%request, 0);
		%count = getWord(%request, 1);
		%delivered = getWord(%request, 2);
		if (%count == %delivered) continue;

		if ((%deliveredItem.isStackable && %item.isStackable && %deliveredItem.stackType $= %item.stackType) || %deliveredItem.getID() == %item.getID()) {
			%overflow = (%delivered + %deliveredCount) - %count;

			if (%overflow > 0) {
				%delivered = %count;
				if (%item.isStackable) {
					%player.farmingAddStackableItem(%deliveredItem, %overflow);
				} else {
					for (%i = 0; %i < %overflow; %i++) {
						%player.farmingAddItem(%deliveredItem);
					}
				}
			} else {
				%delivered += %deliveredCount;
			}

			setDataIDArrayValue(%questID, %i, %item SPC %count SPC %delivered);
			%client.checkQuestComplete(%questID);
			return true;
		}
	}

	return false;
}

function GameConnection::checkQuestComplete(%client, %questID) {
	%player = %client.player;
	if (!isObject(%player)) {
		error("ERROR: Can't check quest completion - no player to give rewards to if it's complete!");
		return;
	}

	%numRequests = getDataIDArrayTagValue(%questID, "numRequests");

	for (%i = 0; %i < %numRequests; %i++) {
		%request = getDataIDArrayValue(%questID, %i);
		%count = getWord(%request, 1);
		%delivered = getWord(%request, 2);

		if (%count < %delivered) {
			return;
		}
	}

	%client.completeQuest(%questID);
}

function GameConnection::completeQuest(%client, %questID) {
	// TODO: player variable and its check will be unnecessary once item packaging/storage is done
	%player = %client.player;

	if (!isObject(%player)) {
		error("ERROR: Client has no player to give rewards to");
		return 0;
	}

	%rewardStart = getDataIDArrayTagValue(%questID, "numRequests");
	%numRewards = getDataIDArrayTagValue(%questID, "numRewards");
	for (%i = 0; %i < %numRewards; %i++) {
		%reward = getDataIDArrayValue(%questID, %rewardStart + %i);
		%item = getWord(%reward, 0);
		%count = getWord(%reward, 1);

		if (%item.isStackable) {
			%player.farmingAddStackableItem(%item, %count);
		} else {
			for (%j = 0; %j < %count; %j++) {
				%player.farmingAddItem(%item);
			}
		}
	}
	return 1;
}

function GameConnection::displayQuest(%client, %questID, %displayRewards) {
	%numRequests = getDataIDArrayTagValue(%questID, "numRequests"); // needed for offset
	if (%displayRewards) {
		%displayString = "<just:right>\c3-Quest Rewards- \n\c3";
		%numRewards = getDataIDArrayTagValue(%questID, "numRewards");
		for (%i = 0; %i < %numRewards; %i++) {
			%reward = getDataIDArrayValue(%questID, %numRequests + %i); // offset by number of requests into array
			%item = getWord(%reward, 0);
			%count = getWord(%reward, 1);

			%displayString = %displayString @ %item.uiName @ "\c6:" SPC %count @ " \n\c3";
		}
	} else {
		%displayString = "<just:right>\c3-Quest Requests- \n\c3";
		for (%i = 0; %i < %numRequests; %i++) {
			%request = getDataIDArrayValue(%questID, %i);
			%item = getWord(%request, 0);
			%count = getWord(%request, 1);
			%delivered = getWord(%request, 2) + 0;

			%displayString = %displayString @ %item.uiName @ "\c6:" SPC %delivered @ "/" @ %count @ " \n\c3";
		}
	}

	%client.centerPrint(trim(%displayString), 1);
}
