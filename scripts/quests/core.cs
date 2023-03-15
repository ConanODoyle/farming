$Farming::QuestPrefix = "Quest_";
$Farming::QuestDepositPointPrefix = "DepositPoint_";

function isQuest(%questID) {
	return getDataIDArrayTagValue(%questID, "isQuest");
}

function deleteQuest(%questID) {
	setDataIDArrayTagValue(%questID, "isQuest", false);
	return deleteDataIDArray(%questID);
}

function QuestType::addQuestItems(%this, %questID, %maxBudget, %mode) {
	if (%mode $= "Requests") {
		%maxBudget /= %this.minBonusFactor + (getRandom() * (%this.maxBonusFactor - %this.minBonusFactor));

		%table = %this.requestTable;

		if (%this.budgetPerRequestItem == 0) {
			%this.budgetPerRequestItem = -1;
		}

		%maxItems = %this.maxRequestItems;
		%minItems = mClamp(1, %maxItems, mCeil(%maxBudget / %this.budgetPerRequestItem));
		%numItems = getRandom(%minItems, %maxItems);
	} else if (%mode $= "Rewards") {
		%table = %this.rewardTable;

		if (%this.rewardsItems) {
			%maxItems = %this.maxRewardItems;
			%numItems = getRandom(1, %maxItems);
		} else {
			%numItems = 0;
		}
	} else {
		error("ERROR - QuestType::addQuestItems - Invalid mode! Mode must be either \"Requests\" or \"Rewards\"");
		return;
	}

	%remainingBudget = %maxBudget;

	if (%mode $= "Rewards") {
		%cashReward = %this.cashRewardIncrement * getRandom(%this.minCashReward, %this.maxCashReward);
		setDataIDArrayTagValue(%questID, "cashReward", %cashReward);
		%remainingBudget -= %cashReward;
	}

	%maxBudgetPerItem = %remainingBudget / %numItems;

	%itemList = "";
	%items = 0;
	%stackableItemList = "";
	%stackableItems = 0;
	for (%i = 0; %i < %numItems; %i++) {
		%selectUniqueItemAttempts = 0;
		while (%itemCount[%item = %table.selectRandom()] !$= "") { // do our best to add only unique items
			%selectUniqueItemAttempts++;
			if (%selectUniqueItemAttempts >= 100) break;
		}

		%itemPrice = (%mode $= "Rewards") ? getBuyPrice(%item) : getSellPrice(%item);

		if (isStackType(%item) || (isObject(%item) && %item.isStackable)) {
			if (isObject(%item)) {
				%item = %item.stackType;
			}

			%itemCount = getMax(mCeil(getMaxStack(%item) / 4), mFloor(%maxBudgetPerItem / %itemPrice));
			%itemPrice = %itemPrice * %itemCount;
			%stackableItemList = trim(%stackableItemList SPC %item);
			%allStackableItemsCost += %itemPrice;
			%stackableItems++;
		} else if (isObject(%item)) {
			%itemCount = 1;
		} else { // handle the case when no item is selected
			continue;
		}

		if (%remainingBudget - %itemPrice >= 0) {
			%remainingBudget -= %itemPrice;
		} else {
			continue;
		}

		if (%itemCount[%item] !$= "") { // just in case we get a non-unique item
			%itemCount[%item] += %itemCount;
		} else {
			%itemCount[%item] = %itemCount;
			%itemList = trim(%itemList SPC %item);
			%items++;
		}

		if (%remainingBudget == 0) break;
	}

	if (%items == 0 && %remainingBudget == %maxBudget)
	{
		talk("ERROR: " @ %this.getName() @ " has insufficient budget for " @ %numItems @ " items! Budget: " @ %maxBudget @ " Mode: " @ %mode);
		talk("    Automatically increasing budget by 500...");
		%this.maxBudget += 500;
		
		if ($safety++ > 10)
		{
			$safety = 0;
			talk("goddamnit irrel i HATE recursive solutions because its the hardest to insert safeties on");
			return "";
		}
		return %this.addQuestItems(%questID, %this.maxBudget, %mode);
	}

	%maxBudgetPerStep = mClamp(0, %remainingBudget, %remainingBudget / %items);
	for (%i = 0; %i < %items; %i++) { // handle extra items
		%item = getWord(%itemList, %i);
		%itemPrice = (%mode $= "Rewards") ? getBuyPrice(%item) : getSellPrice(%item);

		if (isStackType(%item) || (isObject(%item) && %item.isStackable)) {
			%maxExtra = mFloor(getMin(%remainingBudget, %maxBudgetPerStep) / %itemPrice); // if we have any budget left, try adding more items
			%extra = getRandom(0, %maxExtra);

			%itemCount[%item] += %extra;
			%remainingBudget -= %extra * %itemPrice;
		}
	}

	if (%mode $= "Requests") { // handle ensuring sufficient bonus
		if (%maxBudget - %remainingBudget < %maxBudget) { // if we don't meet the minimum bonus
			while (getWordCount(%stackableItemList) > 0) { // while we have some stackable items to mess with
				// get and remove the least expensive item from the list
				%minItemPrice = getSellPrice(getWord(%stackableItemList, 0));
				%itemIndex = 0;
				for (%i = 1; %i < %stackableItems; %i++) {
					%item = getWord(%stackableItemList, %i);
					%itemPrice = getSellPrice(%item);
					if (%itemPrice < %minItemPrice) {
						%minItemPrice = %itemPrice;
						%itemIndex = %i;
					}
				}
				%item = getWord(%stackableItemList, %itemIndex);
				%stackableItemList = removeWord(%stackableItemList, %itemIndex);
				%stackableItems--;

				// we want to add at least as many more expensive as less expensive ones
				%extra = mFloor(%remainingBudget / %allStackableItemsCost);
				%itemCount[%item] += %extra;

				%remainingBudget -= %extra * %minItemPrice;
				%allStackableItemsCost -= %minItemPrice;
			}
		}
	}

	setDataIDArrayCount(%questID, getDataIDArrayCount(%questID) + %items);
	setDataIDArrayTagValue(%questID, "num" @ %mode, %items);
	for (%i = 0; %i < %items; %i++) {
		%item = getWord(%itemList, %i);
		%entry = %item SPC %itemCount[%item];
		addToDataIDArray(%questID, %entry);
	}


	return %maxBudget - %remainingBudget; // get overall cost
}

function QuestType::selectRewards(%this, %questID) {
	$safety = 0;
	return %this.addQuestItems(%questID, %this.maxBudget, "Rewards");
}

function QuestType::selectRequests(%this, %questID, %maxBudget) {
	$safety = 0;
	%this.addQuestItems(%questID, %maxBudget, "Requests");
}

function QuestType::generateQuest(%this) {
	%questID = $Farming::QuestPrefix @ getRandomHash("quest");
	setDataIDArrayTagValue(%questID, "isQuest", true);

	%budget = %this.selectRewards(%questID);
	%this.selectRequests(%questID, %budget);

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
	%requestStart = getDataIDArrayTagValue(%questID, "numRewards");

	%alreadyDelivered = false;
	for (%i = 0; %i < %numRequests; %i++) {
		%request = getDataIDArrayValue(%questID, %requestStart + %i);
		%item = getWord(%request, 0);
		%count = getWord(%request, 1);
		%delivered = getWord(%request, 2);

		%isSameItem = (
			(%deliveredItem.isStackable && (
				(%item.isStackable && %deliveredItem.stackType $= %item.stackType)
				|| (isStackType(%item) && %deliveredItem.stackType $= %item)
			))
			|| isObject(%item) && (%deliveredItem.getID() == %item.getID())
		);

		if (%count == %delivered) {
			if (%isSameItem) {
				%alreadyDelivered = true;
			}
			continue;
		}

		if (%isSameItem) {
			%overflow = getMax((%delivered + %deliveredCount) - %count, 0);

			if (%overflow > 0) {
				%delivered = %count;
			} else {
				%delivered += %deliveredCount;
			}

			setDataIDArrayValue(%questID, %requestStart + %i, %item SPC %count SPC %delivered);
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
	%requestStart = getDataIDArrayTagValue(%questID, "numRewards");

	for (%i = 0; %i < %numRequests; %i++) {
		%request = getDataIDArrayValue(%questID, %requestStart + %i);
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

	%player = %client.player;

	//TODO: Mailbox dropoff
	if (!isObject(%player)) {
		error("ERROR: Client has no player to give rewards to");
		return false;
	}

	%packageDataID = "Package_" @ getRandomHash("package");

	%numRewards = getDataIDArrayTagValue(%questID, "numRewards");
	for (%i = 0; %i < %numRewards; %i++) {
		%reward = getDataIDArrayValue(%questID, %i);
		addToPackageArray(%packageDataID, %reward);
	}

	%cashReward = getDataIDArrayTagValue(%questID, "cashReward");
	addToPackageArray(%packageDataID, "cashReward" SPC %cashReward);
	//TODO: Mailbox dropoff
	createPackage(%packageDataID, %player, %player.position);

	deleteQuest(%questID);

	return true;
}

function GameConnection::displayQuest(%client, %questID, %displayRewards) {
	if (!getDataIDArrayTagValue(%questID, "isQuest")) {
		if (!%displayRewards) {
			%client.centerPrint("<just:right>\c6The note is blank... ", 1);
		} else {
			%client.centerPrint("<just:right>\c6...This side is blank too. ", 1);
		}
		return;
	}

	%numRewards = getDataIDArrayTagValue(%questID, "numRewards"); // needed for offset
	if (%displayRewards) {
		%displayString = "<just:right>\c3-Quest Rewards- \n\c3";
		for (%i = 0; %i < %numRewards; %i++) {
			%reward = getDataIDArrayValue(%questID, %i); // offset by number of requests into array
			%item = getWord(%reward, 0);
			%count = getWord(%reward, 1);

			%itemName = isStackType(%item) ? %item : %item.uiName;

			%displayString = %displayString @ %itemName @ "\c6: " @ %count @ " \n\c3";
		}

		%cashReward = getDataIDArrayTagValue(%questID, "cashReward");
		if (%cashReward > 0) {
			%displayString = %displayString @ "Money\c6: $" @ mFloatLength(%cashReward, 2);
		}
	} else {
		%displayString = "<just:right>\c3-Quest Requests- \n\c3";
		%numRequests = getDataIDArrayTagValue(%questID, "numRequests");
		for (%i = 0; %i < %numRequests; %i++) {
			%request = getDataIDArrayValue(%questID, %numRewards + %i);
			%item = getWord(%request, 0);
			%count = getWord(%request, 1);
			%delivered = getWord(%request, 2) + 0;

			%itemName = isStackType(%item) ? %item : %item.uiName;

			%displayString = %displayString @ %itemName @ "\c6: " @ %delivered @ "/" @ %count @ " \n\c3";
		}
	}

	%client.centerPrint(trim(%displayString));
}

function QuestRequestValue(%questID)
{
	%price = 0;
	%numRewards = getDataIDArrayTagValue(%questID, "numRewards"); // needed for offset
	%numRequests = getDataIDArrayTagValue(%questID, "numRequests");
	for (%i = 0; %i < %numRequests; %i++) {
		%request = getDataIDArrayValue(%questID, %numRewards + %i);
		%item = getWord(%request, 0);
		%count = getWord(%request, 1);
		%price += getSellPrice(%item,%count);
	}
	return %price;
}

function QuestRewardValue(%questID)
{
	%price = 0;
	%numRewards = getDataIDArrayTagValue(%questID, "numRewards"); // needed for offset
	for (%i = 0; %i < %numRewards; %i++) {
		%reward = getDataIDArrayValue(%questID, %i); // offset by number of requests into array
		%item = getWord(%reward, 0);
		%count = getWord(%reward, 1);
		%price += getBuyPrice(%item,%count);
	}
	return %price + getDataIDArrayTagValue(%questID, "cashReward");
}

function QuestProfitValue(%questID)
{
	if (!getDataIDArrayTagValue(%questID, "isQuest"))
	{
		return;
	}
	return QuestRewardValue(%questID) - QuestRequestValue(%questID);
}

function getQuestString(%questID, %showDelivered) {
	%string = "Requests:\n";
	%numRequests = getDataIDArrayTagValue(%questID, "numRequests");
	%numRewards = getDataIDArrayTagValue(%questID, "numRewards");
	for (%i = 0; %i < %numRequests; %i++) {
		%request = getDataIDArrayValue(%questID, %numRewards + %i);
		%item = getWord(%request, 0);
		%count = getWord(%request, 1);
		%delivered = getWord(%request, 2) + 0;

		%itemName = isStackType(%item) ? %item : %item.uiName;

		%string = %string @ %itemName @ ": " @ (%showDelivered ? %delivered @ "/" : "") @ %count @ "\n";
	}

	%string = %string @ "\nRewards:\n";
	for (%i = 0; %i < %numRewards; %i++) {
		%reward = getDataIDArrayValue(%questID, %i); // offset by number of requests into array
		%item = getWord(%reward, 0);
		%count = getWord(%reward, 1);

		%itemName = isStackType(%item) ? %item : %item.uiName;

		%string = %string @ %itemName @ ": " @ %count @ "\n";
	}

	%cashReward =  getDataIDArrayTagValue(%questID, "cashReward");
	if (%cashReward > 0) {
		%string = %string @ "Money\c6: $" @ mFloatLength(%cashReward, 2) @ "\n";
	}

	%profit = mFloatLength(QuestProfitValue(%questID),2);
	%profitString = "$" @ %profit;
	if(%profit <= 0)
	{
		%profitString = "priceless";
	}
	%string = %string @ "\nTotal Profit: " @ %profitString;

	return trim(%string);
}

package FarmingQuests { // TODO: wow this is dense, let's break this up a little bit
	function serverCmdDropTool(%client, %slot) {
		if (isObject(%player = %client.player) && isObject(%player.tool[%slot])) {
			%item = %player.tool[%slot];
			%start = %player.getEyePoint();
			%end = vectorAdd(vectorScale(%player.getEyeVector(), 6), %start);
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && %hit.getDatablock().isQuestSubmissionPoint) {
				if (%hit.EventOutputParameter[0, 1] $= "") {
					%hit.EventOutputParameter[0, 1] = $Farming::QuestDepositPointPrefix @ getRandomHash("depositPoint");
				}
				%depositBoxArray = %hit.EventOutputParameter[0, 1];
				%brickQuest = getDataIDArrayTagValue(%depositBoxArray, "BL_ID" @ %client.bl_id @ "Quest");
				if (%item == QuestItem.getID()) {
					if (isQuest(%player.toolDataID[%slot])) {
						%playerQuest = %player.toolDataID[%slot];
						if (isQuest(%brickQuest) && %player.toolDataID[%slot] $= %brickQuest) {
							if (%client.checkQuestComplete(%playerQuest)) {
								%player.tool[%slot] = 0;
								%player.toolDataID[%slot] = "";
								%player.toolStackCount[%slot] = 0;
								messageClient(%client, 'MsgItemPickup', "", %slot, 0);
								%client.completeQuest(%playerQuest);
								if (%player.currTool == %slot) {
									%player.unmountImage(0);
								}
								commandToClient(%client, 'MessageBoxOK', "Quest Complete!", "Quest complete!\nThe rewards have been deposited in your inventory.");
							} else {
								commandToClient(%client, 'MessageBoxOK', "Quest Incomplete", "This quest isn't complete yet.\nKeep working on it and deposit the quest slip once it's done!");
							}
						} else {
							commandToClient(%client, 'MessageBoxOK', "Quest Assigned", "Quest assigned!\nNow you can deliver quest items here to complete the quest.\nOnce it's complete, deposit the slip to get your rewards!");
							setDataIDArrayTagValue(%depositBoxArray, "BL_ID" @ %client.bl_id @ "Quest", %player.toolDataID[%slot]);
						}
					} else {
						commandToClient(%client, 'MessageBoxOK', "Invalid Quest", "This slip doesn't have a valid quest on it!\nYou need a valid quest slip. You can also safely discard the invalid quest slip.");
					}
					return;
				}
				if (!isQuest(%brickQuest)) {
					%client.chatMessage("There's no quest assigned here!");
					%client.chatMessage("Throw a valid quest slip to assign one.");
					return;
				}

				%result = %client.questDeliverItem(%brickQuest, %item, %player.toolStackCount[%slot] == 0 ? 1 : %player.toolStackCount[%slot]);
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

					if (isObject(%hit.getDatablock().openDatablock)) {
						cancel(%hit.closeSchedule);
						%hit.closeSchedule = %hit.schedule(1000, "setDatablock", %hit.getDatablock());
						%hit.setDatablock(%hit.getDatablock().openDatablock);
						serverPlay3D(brickChangeSound, %hit.position);
					} else if (isObject(%hit.getDatablock().closedDatablock)) {
						cancel(%hit.closeSchedule);
						%hit.closeSchedule = %hit.schedule(1000, "setDatablock", %hit.getDatablock().closedDatablock);
						serverPlay3D(brickChangeSound, %hit.position);
					}
				}
				return;
			}
		}
		return parent::serverCmdDropTool(%client, %slot);
	}
};
schedule(1000, 0, activatePackage, FarmingQuests);
