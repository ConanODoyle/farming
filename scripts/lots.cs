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

function lotBrickProcess(%brick)
{
	%group = %brick.getGroup();
	if (!isObject(%group))
	{
		return;
	}

	%group.lotBrick[%group.lotCount++ - 1] = %brick;
}

function checkPlant2(%cl)
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
	if (%z > 70)
	{
		return 0;
	}
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
	for (%i = 0; %i < %count; %i++)
	{
		%currBounds = %lots[%i];
		%bLarge = getWords(getField(%currBounds, 0), 0, 1);
		%bSmall = getWords(getField(%currBounds, 1), 0, 1);

		%bLx = getWord(%bLarge, 0);
		%bLy = getWord(%bLarge, 1);
		%bSx = getWord(%bSmall, 0);
		%bSy = getWord(%bSmall, 1);

		if (%bLx > %tLx) %bLx = %tLx;
		if (%bLy > %tLy) %bLy = %tLy;
		if (%bSx < %tSx) %bSx = %tSx;
		if (%bSy < %tSy) %bSy = %tSy;

		%totalArea += (%bLx - %bSx) * (%bLy - %bSy);
	}

	if (mAbs(%targetArea - %totalArea) < 0.05)
	{
		return 1;
	}
	else
	{
		return 0;
	}
}

// datablock fxDTSBrickData(brick64x64LotData : brick64x64fData)
// {
// 	category = "Farming";
// 	subcategory = "Lots";
// 	uiName = "64x64 Lot";

// 	cost = 500;
// 	isLot = 1;
// };

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

// function brick64x64LotData::onAdd(%brick)
// {
// 	schedule(10, %brick, lotBrickProcess, %brick);
// }

// function brick32x32LotData::onAdd(%brick)
// {
// 	schedule(10, %brick, lotBrickProcess, %brick);
// }

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
		if (!isObject(%cl.player) || (!isObject(%cl.player.tempBrick) && %cl.ndMode !$= "NDM_PlantCopy") || %cl.bypassRestrictions)
		{
			return parent::serverCmdPlantBrick(%cl);
		}

		%canPlant = checkPlant2(%cl);
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
			%this.schedule(100, updateLotCount, 1);
		}

		return parent::onAdd(%this);
	}

	function fxDTSBrick::onRemove(%this)
	{
		if (%this.isPlanted && %this.getDatablock().isLot)
		{
			%this.updateLotCount(-1);
		}
		return parent::onRemove(%this);
	}

	function ndTrustCheckModify(%brick, %group, %bl_id, %admin)
	{
		if (%brick.getDatablock().isLot)
		{
			return 0;
		}

		return parent::ndTrustCheckModify(%brick, %group, %bl_id, %admin);
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

	%count = getLotCount(%cl.brickGroup);
	if (%count >= 4)
	{
		%cost = mPow(2, getLotCount(%cl.brickGroup) - 1) * 3000;
	}
	else if (%count > 0)
	{
		%cost = mPow(2, getLotCount(%cl.brickGroup)) * 1000;
	}
	else
	{
		if (%hit.getDatablock().isLot && !%hit.getDatablock().isSingle && getNumAdjacentLots(%hit) > 0)
		{
			%cost = 1000;
		}
		else
		{
			%cost = 0;
		}
	}

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
	else if (%cl.score < %cost || (!%hit.getDatablock().isSingle && %cl.farmingExperience < 100))
	{
		if (!%hit.getDatablock().isSingle)
		{
			%cost = %cost @ ", 100 EXP";
		}
		messageClient(%cl, '', "You cannot afford this lot! (Cost: $" @ %cost @ ")");

		if (%cost == 1000 && %cl.timePlayed < 100 && !%cl.newbieMessageLot)
		{
			%cl.newbieMessageLot = 1;
			messageClient(%cl, '', "<font:Palatino Linotype:32>\c5If you're just starting out, find a red empty lot - those are free!");
		}
		return;
	}
	else if (getLotCount(%cl.brickGroup) <= 0 && %hit.getDatablock().brickSizeX != 32)
	{
		messageClient(%cl, '', "You can only purchase small lots for free!");
		return;
	}
	else if (!isValidLotPurchase(%cl.brickGroup, %hit))
	{
		messageClient(%cl, '', "You can only have one single lot, or one connected group of normal lots!");
		return;
	}
	else if (%cl.repeatBuyLot != %hit)
	{
		if (!%hit.getDatablock().isSingle)
		{
			%cost = %cost SPC "and 100 EXP";
		}
		messageClient(%cl, '', "\c5This lot will cost you \c3$" @ %cost @ "\c5. Repeat /buylot to confirm.");
		%cl.repeatBuyLot = %hit;
		cancel(%cl.clearRepeatBuyLotSched);
		%cl.clearRepeatBuyLotSched = schedule(5000, %cl, eval, %cl @ ".repeatBuyLot = 0;");
		return;
	}

	%cl.setScore(%cl.score - %cost);
	if (!%hit.getDatablock().isSingle)
	{
		%cl.addExperience(-100);
		%cost = %cost SPC "and 100 EXP";
	}
	clearLotRecursive(%hit, %cl);
	%cl.brickGroup.add(%hit);
	%hit.updateLotCount(1);
	%hit.setTrusted(1);
	%cl.repeatBuyLot = 0;

	messageClient(%cl, '', "\c5You bought a lot for \c0$" @ %cost @ "\c5!");
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

	// %start = %cl.player.getHackPosition();
	// %end = vectorAdd(%start, "0 0 -10");
	// %ray = containerRaycast(%start, %end, $TypeMasks::fxBrickObjectType);
	// %hit = getWord(%ray, 0);

	%start = %pl.getHackPosition();
	%end = getWords(%start, 0, 1) SPC 0;

	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickAlwaysObjectType);

	while (isObject(%hit = getWord(%ray, 0)) && %safety++ < 100)
	{
		if (%hit.getDatablock().isLot)
		{
			%owner = getBrickgroupFromObject(%hit).name;
			%bl_id = getBrickgroupFromObject(%hit).bl_id;
			break;
		}
		%ray = containerRaycast(vectorSub(getWords(%ray, 1, 3), "0 0 0.1"), %end, $Typemasks::fxBrickAlwaysObjectType, %hit);
	}

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
		%count = getLotCount(%cl.brickGroup);
		if (%count > 4)
		{
			%cl.sellPrice = mPow(2, getLotCount(%cl.brickGroup) - 2) * 3000;
		}
		else if (%count > 1)
		{
			%cl.sellPrice = mPow(2, getLotCount(%cl.brickGroup) - 1) * 1000;
		}
		else
		{
			if (getNumAdjacentLots(%hit) > 0)
			{
				%cl.sellPrice = 1000;
			}
			else
			{
				%cl.sellPrice = 0;
			}
		}

		if (!%force)
		{
			messageClient(%cl, '', "\c5Are you sure you want to sell this lot? Any bricks above it will be removed with 90% refund.");
			getSellPriceSingleWrapper(%hit, %cl, (%hit.getDatablock().isSingle ? 0 : 100));
		}
		else
			messageClient(%cl, '', "\c5Are you sure you want to force sell this lot? Any bricks above it will be removed with 90% refund. Type /sellLot 1 to confirm.");
		%cl.repeatSellLot = %hit;
		cancel(%cl.clearRepeatSellLotSched);
		%cl.clearRepeatSellLotSched = schedule(5000, %cl, eval, %cl @ ".repeatSellLot = 0;");
		return;
	}

	cancel(%cl.clearRepeatSellLotSched);

	%count = getLotCount(%cl.brickGroup);
	if (%count > 4)
	{
		%cost = mPow(2, getLotCount(%cl.brickGroup) - 2) * 3000;
	}
	else if (%count > 1)
	{
		%cost = mPow(2, getLotCount(%cl.brickGroup) - 1) * 1000;
	}
	else
	{
		if (getNumAdjacentLots(%hit) > 0)
		{
			%cost = 1000;
		}
		else
		{
			%cost = 0;
		}
	}

	if (!%force)
	{
		%cl.setScore(%cl.score + %cost);
	}

	//bonus refund
	%cl.refundRatio = 0.9;
	%hit.updateLotCount(-1);
	BrickGroup_888888.add(%hit);
	clearLotRecursive(%hit, %cl);
	fixLotColor(%hit);
	// %cl.refundRatio = 0;
	%cl.repeatSellLot = 0;

	if (!%hit.getDatablock().isSingle && !%force)
	{
		%cl.addExperience(100);
		%cost = %cost SPC "and 100 EXP";
	}

	if (!%force)
	{
		messageClient(%cl, '', "\c5You sold a lot for \c2$" @ %cost @ "\c5!");
	}
	else
	{
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
		fixLotList(%bg);
		%list = %bg.lotList;

		%cl.sellPrice = 0;
		%countCopy = %count;
		if (%countCopy > 1 || getNumAdjacentLots(%list) > 0)
		{
			%cl.sellPrice += 1000; //multiple lots, must have been adjacent, first must have cost 1k
			%sellExperience = 1;
			%totalExpRefund = 100;
		}
		while (%countCopy > 1)
		{
			if (%countCopy > 4)
				%cl.sellPrice += mPow(2, %countCopy - 2) * 3000;
			else
				%cl.sellPrice += mPow(2, %countCopy - 1) * 1000;

			if (%sellExperience)
				%totalExpRefund += 100;

			%countCopy--;
		}

		for (%i = 0; %i < getWordCount(%list); %i++)
		{
			%lot = getWord(%list, %i);
		}

		messageClient(%cl, '', "\c5Are you sure you want to sell ALL your lots? Any bricks above them will be removed with 90% refund.");
		getSellPriceMultiWrapper(%cl, 0, %totalExpRefund);
		%cl.repeatSellAllLots = 1;
		cancel(%cl.clearRepeatSellAllLotsSched);
		%cl.clearRepeatSellAllLotsSched = schedule(5000, %cl, eval, %cl @ ".repeatSellAllLots = 0;");
		return;
	}

	cancel(%cl.clearRepeatSellAllLotsSched);

	fixLotList(%bg);
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


	%cl.refundRatio = 0.9;
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

	//bonus refund

	%bg.lotCount = 0;
	fixLotList(%bg);

	%plural = %count > 1 ? "s" : "";
	if (%sellExperience)
	{
		%totalRefund = %totalRefund SPC "and " @ (%i * 100) @ " EXP";
	}
	messageClient(%cl, '', "\c5You sold " @ %count @ " lot" @ %plural @ " for \c2$" @ %totalRefund @ "\c5!");
}

function serverCmdForceSellAllLots(%cl, %bl_id)
{
	if (!isObject(%bg = "Brickgroup_" @ %bl_id) && !isObject(%bg = getBrickgroupFromObject(findClientByName(%bl_id))))
	{
		return;
	}

	%count = getLotCount(%bg);

	if (%count <= 0)
	{
		messageClient(%cl, '', %bg.name @ " has no lots to sell!");
		return;
	}

	fixLotList(%bg);
	%list = %bg.lotList;

	for (%i = 0; %i < getWordCount(%list); %i++)
	{
		%lot = getWord(%list, %i);
		clearLotRecursive(%lot);
		BrickGroup_888888.add(%lot);
		fixLotColor(%lot);
	}

	%bg.lotCount = 0;
	fixLotList(%bg);

	%plural = %count > 1 ? "s" : "";
	messageClient(%cl, '', "\c5You cleared " @ %count @ " lots owned by " @ %bg.name @ "\c5!");
}

function fxDTSBrick::updateLotCount(%brick, %amt)
{
	%bg = getBrickgroupFromObject(%brick);
	%bg.lotCount += %amt;

	if (%amt > 0)
	{
		%bg.lotList = trim(%bg.lotList SPC %brick);
	}

	schedule(100, %bg, fixLotList, %bg);

	if (%bg.getName() $= "BrickGroup_888888")
	{
		fixLotColor(%brick);
	}
}

function fixLotList(%bg)
{
	%orig = trim(strReplace(%bg.lotList, "  ", " "));

	for (%i = 0; %i < getWordCount(%orig); %i++)
	{
		%id = getWord(%orig, %i);
		if (isObject(%id) && !%isFound[%id] && %id.getDatablock().isLot && getBrickgroupFromObject(%id) == %bg)
		{
			%final = %final SPC %id;
		}
		%isFound[%id] = 1;
	}

	%bg.lotList = trim(%final);
	%bg.lastLotList = %bg.lotList;
	if (getWordCount(%bg.lotList) != %bg.lotCount)
	{
		talk(%bg.name @ "\c5 lot count inconsistency: Actual " @ getWordCount(%bg.lotList) @ " compared to " @ %bg.lotCount);
		echo(%bg.name @ "\c5 lot count inconsistency: Actual " @ getWordCount(%bg.lotList) @ " compared to " @ %bg.lotCount);
		%bg.lotCount = getWordCount(%bg.lotList);
	}
}

function isValidLotPurchase(%bg, %brick)
{
	//if %brick adjacent to anything in lotList, its ok
	//otherwise everything else in lotlist must be in one contiguous group
	//(Max two contiguous groups per user) (one if the early return is not commented out)

	//force an update to lotlist
	fixLotList(%bg);

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
		if (%curr.getDatablock().isSingle)
		{
			return 0;
		}
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

	//two group code
	//%brick isn't adjacent to any lotList brick, check to see if all the bricks in %lotList are contiguous
	for (%i = 0; %i < getWordCount(%lotList); %i++)
	{
		%lotBrick = getWord(%lotList, %i);

		%width = %lotBrick.getDatablock().brickSizeX * 0.5 + 0.1;
		%box1 = %width SPC 0 SPC 0.1; //dont want to get corner-contiguous
		%box2 = 0 SPC %width SPC 0.1;

		for (%i = 1; %i <= 2; %i++)
		{
			%valid = 0;
			initContainerBoxSearch(%lotBrick.getPosition(), %box[%i], $TypeMasks::fxBrickAlwaysObjectType);
			while (isObject(%next = containerSearchNext()))
			{
				if (strPos(" " @ %next @ " ", " " @ %lotList @ " ") >= 0) //its adjacent to a lotlist brick
				{
					%valid = 1;
					break;
				}
			}
			if (%valid == 0) //lotbrick isnt adjacent to any other lotlist bricks!
			{
				return 0;
			}
		}
	}

	//all bricks in lotlist are adjacent to another lotlist brick
	return 1;
}

function getNumAdjacentLots(%brick)
{
	if (%brick.getDatablock().isSingle)
	{
		return 0;
	}

	%width = %brick.getDatablock().brickSizeX * 0.5 + 0.1;
	%box1 = %width SPC 0 SPC 0.1; //dont want to get corner-contiguous
	%box2 = 0 SPC %width SPC 0.1;

	%count = 0;
	for (%i = 1; %i <= 2; %i++)
	{
		initContainerBoxSearch(%brick.getPosition(), %box[%i], $TypeMasks::fxBrickAlwaysObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next != %brick && %next.getDatablock().isLot && !%next.getDatablock().isSingle)
				%count++;
		}
	}
	return %count;
}

function getLotCount(%bg)
{
	return %bg.lotCount;
}

function clearLotRecursive(%lotBrick, %client)
{
	%top = vectorAdd(%lotBrick.getPosition(), "0 0 0.1");
	%pos = vectorAdd(%top, "0 0 100");
	%box = %lotBrick.getDatablock().brickSizeX * 0.5 - 0.05;
	%box = %box SPC %box SPC 199.95;


	%lotBounds = getBrickBounds(%lotBrick, 70);

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
			if (%next.getDatablock().cost > 0)
			{
				sellObject(%next);
			}
			%next.delete();
		}
	}
	schedule(1, 0, clearLotRecursive, %lotBrick, %client);
}

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
	%lotBounds = getBrickBounds(%lotBrick, 70);
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
				%cl.sellPrice += %next.getDatablock().cost * 0.9;
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

	%lotBounds = getBrickBounds(getWord(%cl.brickGroup.lotList, %num), 70);
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
				%cl.sellPrice += %next.getDatablock().cost * 0.9;
			}
		}
	}
	// the for loop ended and we're not done yet, keep going in a moment
	schedule(1, 0, getSellPriceMultiRecursive, %cl, %num, %exp);
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
