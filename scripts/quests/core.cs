function getQuestItem(%table, %slots) {
	%item = farmingTableGetItem(%table);
	%count = %slots;

	if (%item.isStackable) {
		%perSlotCount = getMaxStack(%item.stackType);
		%count = %perSlotCount * (%slots - 1) + getRandom(mFloor(%perSlotCount / 2), %perSlotCount);
	}

	return %item SPC %count;
}

// slots: 1-5, types: field-separated list of metatables
function generateQuest(%requestSlots, %requestTypes, %rewardSlots, %rewardTypes) {
	%requests = generatePart(%difficulty, %requestSlots, %requestTypes);
	%rewards = generatePart(%difficulty, %rewardSlots, %rewardTypes);
	return %requests @ "\n" @ %rewards;
}

function generatePart(%slots, %types) {
	%numItems = min(%slots, getRandom(1, 3));
	for (%numSpareSlots = %slots - %numItems; %numSpareSlots > 0; %numSpareSlots--) {
		%i = getRandom(0, %numItems - 1);
		%extraSlots[%i] += 1;
	}

	for (%i = 0; %i < %numItems; %i++) {
		%tableIndex = getRandom(1, getFieldCount(%types));
		%table = getField(%types, %tableIndex);
		removeField(%types, %tableIndex); // try to ensure items are unique

		%item[%i] = getQuestItem(%table, 1 + %extraSlots[%i]);
	}
}

function GameConnection::completeQuest(%client, %index) {
	// TODO: player variable and its check will be unnecessary once item packaging/storage is done
	%player = %client.player;

	if (!isObject(%player)) {
		error("ERROR: Client has no player to give rewards to");
		return 0;
	}

	if (%index $= "") {
		if (%client.activeQuest $= "") {
			error("ERROR: Client has no active quest and no quest index was provided");
			return 0;
		}
		%index = %client.activeQuest;
	}

	%quest = %client.quest[%index];

	if (%quest $= "") {
		error("ERROR: Client has no quest of index " @ %index);
		return 0;
	}

	%rewards = getLine(%quest, 1);

	if (%rewards $= "") {
		error("ERROR: Quest has no rewards");
		return 0;
	}

	// TODO: once item packaging is done, generate a package id for the player to retrieve
	// %client.pendingReward0...however many
	// use an "array" to store them in case of multiple
	for (%reward = getField(%rewards, %numRewards = 0); %reward !$= ""; %reward = getField(%rewards, %numRewards++)) {
		%item = getWord(%reward, 0);
		%count = getWord(%reward, 1);

		if (%item.isStackable) {
			%player.farmingAddStackableItem(%item, %count);
		} else {
			for (%i = 0; %i < %count; %i++) {
				%player.farmingAddItem(%item);
			}
		}
	}
	return 1;
}

// TODO: make this nicer - centerprint? centerprint menu?
function GameConnection::displayQuest(%client, %questData) {
	%requests = getLine(%questData, 0);
	%rewards = getLine(%questData, 1);

	talk("Requests:");
	for (%field = getField(%requests, %numRequests = 0); %field !$= ""; %field = getField(%requests, %numRequests++)) {
		%item = getWord(%field, 0);
		%count = getWord(%field, 1);
		%delivered = getWord(%field, 2) + 0;

		%deliverable[%numRequests] = %item @ ":" SPC %delivered @ "/" @ %count;
		talk(%deliverable[%numRequests]);
	}

	talk("Rewards:");
	for (%field = getField(%rewards, %numRewards = 0); %field !$= ""; %field = getField(%rewards, %numRewards++)) {
		%item = getWord(%field, 0);
		%count = getWord(%field, 1);

		%reward[%numRewards] = %item @ ":" SPC %count;
		talk(%reward[%numRewards]);
	}
}
