//building & brickgroup

$maxLotBuildHeight = 30;

datablock fxDTSBrickData(brick32x32LotData : brick32x32fData)
{
	category = "Baseplates";
	subcategory = "Lots";
	uiName = "32x32 Lot";

	cost = -1;
	isLot = 1;
};

datablock fxDTSBrickData(brick32x32SingleLotData : brick32x32fData)
{
	category = "Baseplates";
	subcategory = "Lots";
	uiName = "32x32 Single Lot";

	cost = -1;
	isLot = 1;
	isSingle = 1;
};

function GameConnection::getTempBrickBounds(%cl)
{
	if (!isObject(%pl = %cl.player))
	{
		return "";
	}

	%temp = %pl.tempBrick;
	return getBrickBounds(%temp);
}

function getBrickBounds(%temp, %zExtra)
{
	%db = %temp.getDatablock();

	%pos = %temp.getPosition();
	%rot = %temp.angleID;
	%xMod = %db.brickSizeX * 0.25;
	%yMod = %db.brickSizeY * 0.25;
	if (%rot % 2 == 1)
	{
		%t = %xMod;
		%xMod = %yMod;
		%yMod = %t;
	}
	%zMod = %db.brickSizeZ * 0.1;

	%mod = %xMod SPC %yMod SPC %zMod;
	%c1 = vectorAdd(vectorAdd(%pos, %mod), "0 0 " @ %zExtra);
	%c2 = vectorSub(%pos, %mod);

	return %c1 TAB %c2;
}

//assumes %bounds in format of lrgVec TAB smlVec
function isContainedInBounds(%box, %bounds)
{
	%b1 = getField(%bounds, 0);
	%b2 = getField(%bounds, 1);
	%bOffset = vectorSub(%b1, %b2);

	%bx = getWord(%bOffset, 0);
	%by = getWord(%bOffset, 1);
	%bz = getWord(%bOffset, 2);

	%o1 = getField(%box, 0);
	%o2 = getField(%box, 1);

	for (%i = 1; %i <= 2; %i++)
	{
		%pos = vectorSub(%o[%i], %b2);
		%x = getWord(%pos, 0);
		%y = getWord(%pos, 1);
		%z = getWord(%pos, 2);

		if (%x < 0 || %x > %bx ||
				%y < 0 || %y > %by ||
				%z < 0 || %z > %bz)
		{
			return 0;
		}
	}
	return 1;
}

function lotCheckPlant(%cl)
{
	%pl = %cl.player;
	%temp = %pl.tempBrick;

	if (%cl.ndMode $= "NDM_PlantCopy")
	{
		%box = %cl.ndSelection.highlightBox;
		%p1 = %box.point1;
		%p2 = %box.point2;
		%end = vectorScale(vectorAdd(%p1, %p2), 0.5);
		%start = getWords(%end, 0, 1) SPC -1;
		%tempBounds = %p2 TAB %p1;
	}
	else
	{
		if (!isObject(%pl) || !isObject(%temp) || %temp.getDatablock().isLot)
		{
			return 1;
		}

		%start = getWords(%temp.getPosition(), 0, 1) SPC -1;
		%end = %temp.getPosition();
		%tempBounds = getBrickBounds(%temp);
	}

	%z = getWord(getField(%tempBounds, 1), 2);

	%tempPos = vectorScale(vectorAdd(getField(%tempBounds, 0), getField(%tempBounds, 1)), 0.5);
	%xP = getWord(%tempPos, 0);
	%yP = getWord(%tempPos, 1);
	%zP = %z / 2;
	%tempSize = vectorSub(getField(%tempBounds, 0), getField(%tempBounds, 1));
	%xS = getWord(%tempSize, 0) - 0.01;
	%yS = getWord(%tempSize, 1) - 0.01;
	%zS = %z - 0.01;

	%bSearchPos = %xP SPC %yP SPC %zP;
	%bSearchSize = %xS SPC %yS SPC %zS;

	%xyLarge = getWords(getField(%tempBounds, 0), 0, 1);
	%xySmall = getWords(getField(%tempBounds, 1), 0, 1);

	initContainerBoxSearch(%bSearchPos, %bSearchSize, $TypeMasks::fxBrickObjectType);
	%count = 0;
	while (isObject(%next = containerSearchNext()))
	{
		if (%next.isPlanted && %next.getDatablock().isLot)
		{
			if (getTrustLevel(%cl, %next) < 1)
			{
				return 0;
			}
			else
			{
				%lots[%count++ - 1] = getBrickBounds(%next);
			}
		}
	}

	%totalArea = 0;
	%tLx = getWord(%xyLarge, 0);
	%tLy = getWord(%xyLarge, 1);
	%tSx = getWord(%xySmall, 0);
	%tSy = getWord(%xySmall, 1);
	%targetArea = (%tLx - %tSx) * (%tLy - %tSy);
	%lowestZ = 100000;
	for (%i = 0; %i < %count; %i++)
	{
		%currBounds = %lots[%i];
		%bLarge = getField(%currBounds, 0);
		%bSmall = getField(%currBounds, 1);

		%bLx = getWord(%bLarge, 0);
		%bLy = getWord(%bLarge, 1);
		%bSx = getWord(%bSmall, 0);
		%bSy = getWord(%bSmall, 1);
		%lowestZ = %lowestZ < getWord(%bSmall, 2) ? %lowestZ : getWord(%bSmall, 2);

		if (%bLx > %tLx) %bLx = %tLx;
		if (%bLy > %tLy) %bLy = %tLy;
		if (%bSx < %tSx) %bSx = %tSx;
		if (%bSy < %tSy) %bSy = %tSy;

		%totalArea += (%bLx - %bSx) * (%bLy - %bSy);
	}

	%zDiff = %z - %lowestZ;

	if (mAbs(%targetArea - %totalArea) < 0.05 && %zDiff < $maxLotBuildHeight && %zDiff > 0)
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

function addLotToBrickgroup(%bg, %lot)
{
	if (!isObject(%lot) || !isObject(%bg))
	{
		talk("ERROR: addLotToBrickgroup - parameters invalid! " @ %bg SPC %lot);
		echo("ERROR: addLotToBrickgroup - parameters invalid! " @ %bg SPC %lot);
		return;
	}
}

function fxDTSBrick::updateGroupLotCount(%brick, %amt)
{
	%bg = getBrickgroupFromObject(%brick);
	%bg.lotCount += %amt;

	if (%amt > 0)
	{
		%bg.lotList = trim(%bg.lotList SPC %brick);
	}

	%bg.schedule(100, refreshLotList);

	if (%bg.getName() $= "BrickGroup_888888")
	{
		fixLotColor(%brick);
	}
}

function SimGroup::refreshLotList(%bg)
{
	if (%bg.bl_id == 888888 || %bg.bl_id $= "") //no need to manage lots in public brickgroup
	{
		return;
	}
	%count = %bg.getCount();
	for (%i = 0; %i < %count; %i++)
	{
		%b = %bg.getObject(%i);
		if (%b.getDatablock().isLot)
		{
			%str = %str SPC %b;
		}
	}

	%bg.lotList = trim(%str);
	%bg.lotCount = getWordCount(%bg.lotList);
}

function fixLotColor(%brick)
{
	%xEven = mAbs(mFloor(getWord(%brick.getPosition(), 0) / 16)) % 2;
	%yEven = mAbs(mFloor(getWord(%brick.getPosition(), 1) / 16)) % 2;

	if (%xEven == %yEven)
	{
		if (%brick.getDatablock().isSingle)
		{
			%brick.setColor(27); //single lots colored red
		}
		else
		{
			%brick.setColor(47);
		}
	}
	else
	{
		if (%brick.getDatablock().isSingle)
		{
			%brick.setColor(28); //single lots colored red
		}
		else
		{
			%brick.setColor(48);
		}
	}
	%brick.setShapeFX(0);
	%brick.setColorFX(0);
}

package lotBuild
{
	function serverCmdClearBricks(%cl)
	{
		if (%cl.brickgroup.lotCount > 0)
		{
			messageClient(%cl, '', "Cannot clear bricks - sell your lots first! Use /sellAllLots to sell all your lots at once.");
			return;
		}
		return parent::serverCmdClearBricks(%cl);
	}

	function serverCmdPlantBrick(%cl)
	{
		if (!isObject(%cl.player) || (!isObject(%cl.player.tempBrick) && %cl.ndMode !$= "NDM_PlantCopy") || %cl.isBuilder)
		{
			return parent::serverCmdPlantBrick(%cl);
		}

		%canPlant = lotCheckPlant(%cl);
		if (%canPlant)
		{
			return parent::serverCmdPlantBrick(%cl);
		}
		else
		{
			messageClient(%cl, '', "You cannot plant outside of your lot!");
		}
		return 0;
	}

	function fxDTSBrick::onAdd(%this)
	{
		if (%this.isPlanted && %this.getDatablock().isLot)
		{
			%this.schedule(100, updateGroupLotCount, 1);
		}

		return parent::onAdd(%this);
	}

	function fxDTSBrick::onRemove(%this)
	{
		if (%this.isPlanted && %this.getDatablock().isLot)
		{
			%this.updateGroupLotCount(-1);
		}
		return parent::onRemove(%this);
	}

	function ndTrustCheckSelect(%brick, %group, %bl_id, %admin)
	{
		if (%brick.getDatablock().isLot)
		{
			return 0;
		}

		return parent::ndTrustCheckSelect(%brick, %group, %bl_id, %admin);
	}

	function getTrustLevel(%obj1, %obj2)
	{
		if ((%obj1.getClassName() $= "fxDTSBrick" && %obj1.getDatablock().isLot)
			|| (%obj2.getClassName() $= "fxDTSBrick" && %obj2.getDatablock().isLot))
		{
			if (%obj1.getClassName() $= "Player" || %obj2.getClassName() $= "Player")
			{
				return 0;
			}
		}
		return parent::getTrustLevel(%obj1, %obj2);
	}
};
schedule(1000, 0, activatePackage, lotBuild);











//purchasing

//returns cost of purchasing %lot
function getLotCost(%count, %lot)
{
	if (!isObject(%lot))
	{
		return "";
	}
	if (%count >= 4)
	{
		%cost = mPow(2, %count - 1) * 3000;
	}
	else if (%count > 0)
	{
		%cost = mPow(2, %count) * 1000;
	}
	else
	{
		if (%lot.getDatablock().isLot && !%lot.getDatablock().isSingle && getNumAdjacentLots(%lot) > 0)
		{
			%cost = 1000;
		}
		else
		{
			%cost = 0;
		}
	}

	if (!%lot.getDatablock().isSingle)
	{
		%expCost = 100 * mFloor(mPow(%count, 1.5));
	}
	return %cost SPC %expCost;
}

function serverCmdBuyLot(%cl)
{
	if (!isObject(%pl = %cl.player))
	{
		messageClient(%cl, '', "You are dead!");
		return;
	}

	%start = %cl.player.getHackPosition();
	%end = vectorAdd(%start, "0 0 -10");
	%ray = containerRaycast(%start, %end, $TypeMasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	%cost = getLotCost(getLotCount(%cl.brickGroup), %hit);
	%costMoney = getWord(%cost, 0);
	%costExp = getWord(%cost, 1);
	%costString = "$" @ %costMoney @ (%costExp > 0 ? ", " @ %costExp @ " EXP" : "");

	if (!%hit.getDatablock().isLot)
	{
		messageClient(%cl, '', "You have to be standing on a lot!");
		return;
	}
	else if (%hit.getGroup() == %cl.brickGroup)
	{
		messageClient(%cl, '', "You already own this lot!");
		return;
	}
	else if (%hit.getGroup().getName() !$= "BrickGroup_888888")
	{
		messageClient(%cl, '', "This lot is claimed by " @ %hit.getGroup().name @ "!");
		return;
	}
	else if (%cl.score < %costMoney || %cl.farmingExperience < %costExp)
	{
		messageClient(%cl, '', "You cannot afford this lot! (Cost: " @ %costString @ ")");

		if (%cost == 1000 && %cl.timePlayed < 100 && !%cl.newbieMessageLot)
		{
			%cl.newbieMessageLot = 1;
			messageClient(%cl, '', "<font:Palatino Linotype:32>\c5If you're just starting out, find a red empty lot - those are free!");
		}
		return;
	}
	else if (getLotCount(%cl.brickGroup) <= 0 && !%hit.getDatablock().isSingle)
	{
		messageClient(%cl, '', "You can only purchase unclaimed red lots for free!");
		return;
	}
	else if (!isValidLotPurchase(%cl.brickGroup, %hit))
	{
		messageClient(%cl, '', "You can only have one connected group of lots!");
		return;
	}
	else if (%cl.repeatBuyLot != %hit)
	{
		messageClient(%cl, '', "\c5This lot will cost you \c3" @ %costString @ "\c5. Repeat /buylot to confirm.");
		%cl.repeatBuyLot = %hit;
		cancel(%cl.clearRepeatBuyLotSched);
		%cl.clearRepeatBuyLotSched = schedule(5000, %cl, eval, %cl @ ".repeatBuyLot = 0;");
		return;
	}

	%cl.setScore(%cl.score - %costMoney);
	%cl.addExperience(-1 * %costExp);
	clearLotRecursive(%hit, %cl);
	%cl.brickGroup.add(%hit);
	%hit.updateGroupLotCount(1);
	%hit.setTrusted(1);
	%cl.repeatBuyLot = 0;

	messageClient(%cl, '', "\c5You bought a lot for \c0" @ %costString @ "\c5!");
}

function serverCmdSellLot(%cl, %force)
{
	if (!isObject(%pl = %cl.player))
	{
		messageClient(%cl, '', "You are dead!");
		return;
	}

	if (!%cl.isSuperAdmin)
	{
		%force = 0;
	}

	%start = %pl.getHackPosition();
	%end = vectorAdd(%start, "0 0 -10");
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	if (!isObject(%hit) || !%hit.getDatablock().isLot)
	{
		messageClient(%cl, '', "You have to be standing on a lot!");
		return;
	}
	else if (%hit.getGroup() != %cl.brickGroup && !%force)
	{
		messageClient(%cl, '', "You do not own this lot!");
		return;
	}
	else if (%cl.repeatSellLot != %hit)
	{
		%cost = getLotCost(getLotCount(%cl.brickGroup) - 1, %hit);
		%costMoney = getWord(%cost, 0);
		%costExp = getWord(%cost, 1);
		%costString = "$" @ %costMoney @ (%costExp > 0 ? ", " @ %costExp @ " EXP" : "");
		%cl.sellPrice = %costMoney;

		if (!%force)
		{
			messageClient(%cl, '', "\c5Are you sure you want to sell this lot? Any bricks above it will be removed with 75% refund. Repeat this command to confirm.");
			getSellPriceSingleWrapper(%hit, %cl, %costExp);
		}
		else
			messageClient(%cl, '', "\c5Are you sure you want to force sell this lot? Any bricks above it will be removed. Type /sellLot 1 to confirm.");
		%cl.repeatSellLot = %hit;
		cancel(%cl.clearRepeatSellLotSched);
		%cl.clearRepeatSellLotSched = schedule(5000, %cl, eval, %cl @ ".repeatSellLot = 0;");
		return;
	}

	cancel(%cl.clearRepeatSellLotSched);

	%cost = getLotCost(getLotCount(%cl.brickGroup) - 1, %hit);
	%costMoney = getWord(%cost, 0);
	%costExp = getWord(%cost, 1);
	%costString = "$" @ %costMoney @ (%costExp > 0 ? ", " @ %costExp @ " EXP" : "");

	if (!%force)
	{
		%cl.setScore(%cl.score + %costMoney);
		%cl.addExperience(%costExp);
	}

	%hit.updateGroupLotCount(-1);
	BrickGroup_888888.add(%hit);
	clearLotRecursive(%hit, %cl);
	fixLotColor(%hit);
	// %cl.refundRatio = 0;
	%cl.repeatSellLot = 0;

	if (!%force)
	{
		messageClient(%cl, '', "\c5You sold a lot for \c2" @ %costString @ "\c5!");
	}
	else
	{
		%owner = getBrickgroupFromObject(%hit).name;
		messageClient(%cl, '', "\c5You \c0force\c5 sold \c3" @ %owner @ "\c5's lot!");
	}
}

function serverCmdSellAllLots(%cl)
{
	if (!isObject(%bg = %cl.brickGroup))
	{
		return;
	}

	%count = getLotCount(%bg);

	if (%count <= 0)
	{
		messageClient(%cl, '', "You have no lots to sell!");
		return;
	}
	else if (%cl.repeatSellAllLots != 1)
	{
		%bg.refreshLotList();
		%list = %bg.lotList;

		%cl.sellPrice = 0;
		%countCopy = %count;
		%totalCostRefund = 0;
		%totalExpRefund = 0;
		while (%countCopy > 1)
		{
			%lot = getWord(%list, %countCopy - 1);
			%cost = getLotCost(%countCopy - 1, %lot);
			
			%totalCostRefund += getWord(%cost, 0);
			%totalExpRefund += getWord(%cost, 1);

			%countCopy--;
		}

		for (%i = 0; %i < getWordCount(%list); %i++)
		{
			%lot = getWord(%list, %i);
		}

		messageClient(%cl, '', "\c5Are you sure you want to sell ALL your lots? Any bricks above them will be removed with 75% refund.");
		getSellPriceMultiWrapper(%cl, 0, %totalExpRefund);
		%cl.repeatSellAllLots = 1;
		cancel(%cl.clearRepeatSellAllLotsSched);
		%cl.clearRepeatSellAllLotsSched = schedule(5000, %cl, eval, %cl @ ".repeatSellAllLots = 0;");
		return;
	}

	cancel(%cl.clearRepeatSellAllLotsSched);

	%bg.refreshLotList();
	%list = %bg.lotList;

	%totalRefund = 0;
	%countCopy = %count;
	if (%countCopy > 1 || getNumAdjacentLots(%list) > 0)
	{
		%totalRefund += 1000; //multiple lots, must have been adjacent, first must have cost 1k
		%sellExperience = 1;
	}
	while (%countCopy > 1)
	{
		if (%countCopy > 4)
			%totalRefund += mPow(2, %countCopy - 2) * 3000;
		else
			%totalRefund += mPow(2, %countCopy - 1) * 1000;
		%countCopy--;
	}

	%cl.setScore(%cl.score + %totalRefund);

	for (%i = 0; %i < getWordCount(%list); %i++)
	{
		%lot = getWord(%list, %i);
		clearLotRecursive(%lot, %cl);
		BrickGroup_888888.add(%lot);
		fixLotColor(%lot);
		if (%sellExperience)
		{
			%cl.addExperience(100);
		}
	}
	%cl.repeatSellAllLots = 0;

	%bg.lotCount = 0;
	%bg.refreshLotList();

	%plural = %count > 1 ? "s" : "";
	if (%sellExperience)
	{
		%totalRefund = %totalRefund SPC "and " @ (%i * 100) @ " EXP";
	}
	messageClient(%cl, '', "\c5You sold " @ %count @ " lot" @ %plural @ " for \c2$" @ %totalRefund @ "\c5!");
}

function isValidLotPurchase(%bg, %brick)
{
	//if %brick adjacent to anything in lotList, its ok
	//otherwise everything else in lotlist must be in one contiguous group
	//(Max two contiguous groups per user) (one if the early return is not commented out)

	//force an update to lotlist
	%bg.refreshLotList();

	if (%bg.lotCount == 0) //no other lots in this group, always allow
	{
		return 1;
	}
	else if (%brick.getDatablock().isSingle && %bg.lotCount > 0) //cant buy single lots if you own other ones!
	{
		return 0;
	}

	%lotList = %bg.lotList;

	for (%i = 0; %i < getWordCount(%lotList); %i++)
	{
		%curr = getWord(%lotList, %i);
		%inList_[getWord(%lotList, %i)] = 1;
	}

	//first start with searching around %brick (the new lot) and see if its in lotlist
	%width = %brick.getDatablock().brickSizeX * 0.5 + 0.1;
	%box1 = %width SPC 0 SPC 0.1; //dont want to get corner-contiguous
	%box2 = 0 SPC %width SPC 0.1;

	for (%i = 1; %i <= 2; %i++)
	{
		initContainerBoxSearch(%brick.getPosition(), %box[%i], $TypeMasks::fxBrickAlwaysObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%inList_[%next]) //its adjacent to a lotlist brick
			{
				return 1;
			}
		}
	}

	//only allowing one lot group - must sell current lot if they want to move to a new general spot!
	return 0;
}

function getNumAdjacentLots(%brick, %mode)
{
	%width = %brick.getDatablock().brickSizeX * 0.5 + 0.1;

	switch$ (%mode)
	{
		case "all":
			%box1 = %width SPC %width SPC 0.1;
		default:
			%box1 = %width SPC 0 SPC 0.1; //dont want to get corner-contiguous
			%box2 = 0 SPC %width SPC 0.1;
	}

	%count = 0;
	for (%i = 1; %i <= 2; %i++)
	{
		if (%box[%i] $= "")
		{
			continue;
		}

		initContainerBoxSearch(%brick.getPosition(), %box[%i], $TypeMasks::fxBrickAlwaysObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next != %brick && %next.getDatablock().isLot && !%next.getDatablock().isSingle)
			{
				%count++;
				%list = %list SPC %next;
			}
		}
	}
	return %count TAB trim(%list);
}

function getLotCount(%bg)
{
	return %bg.lotCount;
}

function clearLotRecursive(%lotBrick, %client)
{
	%top = vectorAdd(%lotBrick.getPosition(), "0 0 0.1");
	%pos = vectorAdd(%top, "0 0 " @ $maxLotBuildHeight / 2);
	%box = %lotBrick.getDatablock().brickSizeX * 0.5 - 0.05;
	%box = %box SPC %box SPC ($maxLotBuildHeight - 0.05);


	%lotBounds = getBrickBounds(%lotBrick, $maxLotBuildHeight);

	initContainerBoxSearch(%pos, %box, $TypeMasks::fxBrickAlwaysObjectType);
	for (%i = 0; %i < 1024; %i++)
	{
		if (!isObject(%next = containerSearchNext()))
		{
			//we're done
			%client.refundRatio = 0;
			return;
		}


		if (isContainedInBounds(%next.getPosition() TAB %next.getPosition(), %lotBounds))
		{
			if (%next.getDatablock().cost > 0 && isObject(%client))
			{
				sellObject(%next);
			}
			%next.delete();
		}
	}
	schedule(1, 0, clearLotRecursive, %lotBrick, %client);
}










//sell price estimator

function getSellPriceSingleWrapper(%lotBrick, %client, %exp)
{
	%top = vectorAdd(%lotBrick.getPosition(), "0 0 0.1");
	%pos = vectorAdd(%top, "0 0 100");
	%box = %lotBrick.getDatablock().brickSizeX * 0.5 - 0.05;
	%box = %box SPC %box SPC 199.95;

	initContainerBoxSearch(%pos, %box, $TypeMasks::fxBrickAlwaysObjectType);
	getSellPriceSingleRecursive(%lotBrick, %client, %exp);
}

function getSellPriceSingleRecursive(%lotBrick, %cl, %exp)
{
	%lotBounds = getBrickBounds(%lotBrick, $maxLotBuildHeight);
	for (%i = 0; %i < 1024; %i++)
	{
		if (!isObject(%next = containerSearchNext()))
		{
			//we're done
			messageClient(%cl, '', "\c5- You will receive \c2$" @ %cl.sellPrice @ (%exp > 0 ? ("\c5 and \c3" @ %exp @ " XP\c5") : "\c5") @ " for selling this lot. Repeat /sellLot to confirm.");
			return;
		}


		if (isContainedInBounds(%next.getPosition() TAB %next.getPosition(), %lotBounds))
		{
			if (%next.getDatablock().cost > 0)
			{
				%cl.sellPrice += %next.getDatablock().cost * 0.75;
			}
		}
	}
	schedule(1, 0, getSellPriceSingleRecursive, %lotBrick, %cl, %exp);
}

function getSellPriceMultiWrapper(%cl, %num, %exp)
{
	if (%num > 200)
	{
		talk("Sentinel went off. Something's wrong.");
		return;
	}
	%lotBrick = getWord(%cl.brickGroup.lotList, %num);
	%top = vectorAdd(%lotBrick.getPosition(), "0 0 0.1");
	%pos = vectorAdd(%top, "0 0 100");
	%box = %lotBrick.getDatablock().brickSizeX * 0.5 - 0.05;
	%box = %box SPC %box SPC 199.95;

	initContainerBoxSearch(%pos, %box, $TypeMasks::fxBrickAlwaysObjectType);
	getSellPriceMultiRecursive(%cl, %num, %exp);
}

function getSellPriceMultiRecursive(%cl, %num, %exp)
{
	if (%num > 200)
	{
		talk("Sentinel went off. Something's wrong.");
		return;
	}
	if (%num == getLotCount(%cl.brickGroup) - 1)
		%final = 1;

	%lotBounds = getBrickBounds(getWord(%cl.brickGroup.lotList, %num), $maxLotBuildHeight);
	for (%i = 0; %i < 1024; %i++)
	{
		if (!isObject(%next = containerSearchNext()))
		{
			// we've run out of stuff on this lot!
			if (%final)
			{
				// we've run out of lots to check!
				messageClient(%cl, '', "\c5- You will receive \c2$" @ %cl.sellPrice @ (%exp > 0 ? ("\c5 and \c3" @ %exp @ " XP\c5 ") : "\c5") @ " for selling your lots. Repeat /sellAllLots to confirm.");
				%cl.sellPrice = 0;
				return;
			}
			else
			{
				// let's go back to the wrapper, still more to go
				getSellPriceMultiWrapper(%cl, %num + 1, %exp);
				return;
			}
		}

		if (isContainedInBounds(%next.getPosition() TAB %next.getPosition(), %lotBounds))
		{
			if (%next.getDatablock().cost > 0)
			{
				%cl.sellPrice += %next.getDatablock().cost * 0.75;
			}
		}
	}
	// the for loop ended and we're not done yet, keep going in a moment
	schedule(1, 0, getSellPriceMultiRecursive, %cl, %num, %exp);
}