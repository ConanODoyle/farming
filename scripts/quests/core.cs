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

function isQuest(%questID) {
	return getDataIDArrayTagValue(%questID, "isQuest");
}

function deleteQuest(%questID) {
	setDataIDArrayTagValue(%questID, "isQuest", false);
	return deleteDataIDArray(%questID);
}

function generateQuest(%requestSlots, %requestTypes, %rewardSlots, %rewardTypes) {
	%questID = $Farming::QuestPrefix @ getRandomHash("quest");
	setDataIDArrayTagValue(%questID, "isQuest", true);

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
	if (!isQuest(%questID)) {
		error("ERROR: Invalid questID!");
		return false;
	}

	%player = %client.player;
	if (!isObject(%player)) {
		error("ERROR: Player does not exist, can't deliver quest item!");
		return false;
	}

	if (%deliveredCount $= "") {
		%deliveredCount = 1;
	}

	%numRequests = getDataIDArrayTagValue(%questID, "numRequests");

	%alreadyDelivered = false;
	for (%i = 0; %i < %numRequests; %i++) {
		%request = getDataIDArrayValue(%questID, %i);
		%item = getWord(%request, 0);
		%count = getWord(%request, 1);
		%delivered = getWord(%request, 2);
		if (%count == %delivered) {
			if ((%deliveredItem.isStackable && %item.isStackable && %deliveredItem.stackType $= %item.stackType) || %deliveredItem.getID() == %item.getID()) {
				%alreadyDelivered = true;
			}
			continue;
		}

		if ((%deliveredItem.isStackable && %item.isStackable && %deliveredItem.stackType $= %item.stackType) || %deliveredItem.getID() == %item.getID()) {
			%overflow = getMax((%delivered + %deliveredCount) - %count, 0);

			if (%overflow > 0) {
				%delivered = %count;
			} else {
				%delivered += %deliveredCount;
			}

			setDataIDArrayValue(%questID, %i, %item SPC %count SPC %delivered);
			return true SPC %overflow;
		}
	}

	return false SPC %alreadyDelivered;
}

function GameConnection::checkQuestComplete(%client, %questID) {
	if (!isQuest(%questID)) {
		error("ERROR: Invalid questID!");
		return false;
	}

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

		if (%count > %delivered) {
			return false;
		}
	}
	return true;
}

function GameConnection::completeQuest(%client, %questID) {
	if (!isQuest(%questID)) {
		error("ERROR: Invalid questID!");
		return false;
	}

	// TODO: player variable and its check will be unnecessary once item packaging/storage is done
	%player = %client.player;

	if (!isObject(%player)) {
		error("ERROR: Client has no player to give rewards to");
		return false;
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

	deleteQuest(%questID);

	return true;
}

function GameConnection::displayQuest(%client, %questID, %displayRewards) {
	if (!getDataIDArrayValue(%questID, "isQuest")) {
		if (!%displayRewards) {
			%client.centerPrint("<just:right>\c6The note is blank... ", 1);
		} else {
			%client.centerPrint("<just:right>\c6...This side is blank too. ", 1);
		}
		return;
	}

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

package FarmingQuests {
	function serverCmdDropTool(%client, %slot) {
		if (isObject(%player = %client.player) && isObject(%player.tool[%slot])) {
			%item = %player.tool[%slot];
			%start = %player.getEyePoint();
			%end = vectorAdd(vectorScale(%player.getEyeVector(), 6), %start);
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && %hit.getDatablock().isQuestSubmissionPoint) {
				%brickQuest = %hit.questID;
				if (%item == QuestItem.getID()) {
					if (isQuest(%player.toolDataID[%slot])) {
						%playerQuest = %player.toolDataID[%slot];
						if (isQuest(%brickQuest) && %player.toolDataID[%slot] $= %brickQuest) {
							if (%client.checkQuestComplete(%playerQuest)) {
								%client.completeQuest(%playerQuest);
								%player.tool[%slot] = 0;
								%player.toolDataID[%slot] = "";
								%player.toolStackCount[%slot] = 0;
								messageClient(%client, 'MsgItemPickup', "", %slot, 0);
								if (%player.currTool == %slot) {
									%player.unmountImage(0);
								}
								%client.chatMessage("\c2Quest complete!\n\c3The rewards have been deposited in your inventory.");
							} else {
								%client.chatMessage("\c0This quest isn't complete yet.\n\c6Keep working on it and deposit the quest slip once it's done!");
							}
						} else {
						%client.chatMessage("\c2Quest assigned!\n\c6Now you can deliver quest items here to complete the quest.\nOnce it's complete, deposit the slip to get your rewards!");
							%hit.questID = %player.toolDataID[%slot];
						}
					} else {
						%client.chatMessage("This slip doesn't have a valid quest on it!\nYou need a valid quest slip.");
					}
					return;
				}
				if (!isQuest(%brickQuest)) {
					%client.chatMessage("There's no quest assigned here!\nThrow a valid quest slip to assign one.");
					return;
				}

				%questID = %hit.questID;

				%result = %client.questDeliverItem(%questID, %item, %player.toolStackCount[%slot] == 0 ? 1 : %player.toolStackCount[%slot]);
				%itemName = %item.isStackable ? ($displayNameOverride_[%item.stackType] !$= "" ? $displayNameOverride_[%item.stackType] : %item.stackType) : %item.uiName;
				if (!%result) {
					%alreadyDelivered = getWord(%result, 1);
					if (%alreadyDelivered) {
						%client.chatMessage("\c2You have already completed the \c3" @ trim(%itemName) @ "\c2 requirement for this quest!");
					} else {
						%client.chatMessage("\c3" @ trim(%itemName) @ "\c0 isn't required for this quest.");
					}
				} else {
					%originalCount = %player.toolStackCount[%slot];
					%overflow = getWord(%result, 1);
					if (%overflow > 0) {
						%newStackItem = getStackTypeDatablock(%player.tool[%slot].stackType, getWord(%success, 1)).getID();
						%player.tool[%slot] = %newStackItem;
						%player.toolStackCount[%slot] = %overflow;
						messageClient(%client, 'MsgItemPickup', "", %slot, %newStackItem);
						if (%player.currTool == %slot) {
							%player.mountImage(%newStackItem.image, 0);
						}
					} else {
						%player.tool[%slot] = 0;
						%player.toolStackCount[%slot] = 0;
						messageClient(%client, 'MsgItemPickup', "", %slot, 0);
						if (%player.currTool == %slot) {
							%player.unmountImage(0);
						}
					}
				}
				return;
			}
		}
		return parent::serverCmdDropTool(%client, %slot);
	}
};
schedule(1000, 0, activatePackage, FarmingQuests);
