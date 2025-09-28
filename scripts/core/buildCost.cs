$createdTimeout = 4;

package BuildCost
{
	function fxDTSBrick::onAdd(%obj)
	{
		%db = %obj.getDatablock();
		if (%obj.isPlanted && (%db.cost > 0 || %db.cost == -1))
		{
			%obj.buySchedule = schedule(1, %obj, buyBrick, %obj);
		}

		return parent::onAdd(%obj);
	}

	function fxDTSBrickData::onTrustCheckFailed(%data, %obj)
	{
		%obj.trustFailed = 1;
		parent::onTrustCheckFailed(%data, %obj);
	}

	function fxDTSBrick::onDeath(%obj)
	{
		%db = %obj.getDatablock();
		if (%obj.isPlanted && %db.cost > 0 && !isEventPending(%obj.buySchedule) && !%obj.skipSell)
		{
			sellObject(%obj);
		}

		return parent::onDeath(%obj);
	}

	function fxDTSBrick::onDupCut(%brick)
	{	
		if (%brick.dataBlock.cost > 0)
		{
			sellObject(%brick);
		}
		parent::onDupCut(%brick);
	}
};
activatePackage(BuildCost);

function buyBrick(%b)
{
	%group = getBrickgroupFromObject(%b);
	if (!isObject(%group.client) || %group.client.isBuilder || %group.isLoadingLot || %b.skipBuy || %b.trustFailed)
	{
		return;
	}

	%cl = %group.client;
	%db = %b.getDatablock();
	if (%group.isSaveClearingLot)
	{
		%b.skipSell = 1;
		%b.schedule(1, delete);
		messageClient(%cl, '', "You cannot place bricks while your lot is being unloaded!");
		return;
	}
	if (%cl.checkMoney(%db.cost) && %db.cost >= 0)
	{
		%cl.subMoney(%db.cost);
		%cl.deducted += %db.cost;
		cancel(%cl.costMessageSchedule);
		%cl.centerPrint("<color:ff0000>$" @ mFloatLength(%db.cost, 2) @ "<color:ffffff> - " @ %db.uiName, 1);
		%cl.schedule(50, centerPrint, "<color:cc0000>$" @ mFloatLength(%db.cost, 2) @ "<color:cccccc> - " @ %db.uiName, 2);
		%cl.costMessageSchedule = schedule(800, 0, messageDeductedMoney, %cl);
	}
	else if (%db.cost > 0)
	{
		// %b.playSound(BrickBreakSound);
		%b.skipSell = 1;
		%b.schedule(1, delete);
		messageClient(%cl, '', "You cannot afford to place " @ %db.uiName @ "! (Cost: $" @ mFloatLength(%db.cost, 2) @ ")");
		return;
	}
	else if (%db.cost < 0)
	{
		// %b.playSound(BrickBreakSound);
		%b.skipSell = 1;
		%b.schedule(1, delete);
		messageClient(%cl, '', "You cannot place this brick!");
		return;
	}
	%b.skipSell = 0;
	%b.createdTimeout = $Sim::Time + $createdTimeout;
}

function messageDeductedMoney(%cl)
{
	if (%cl.deducted == 0)
	{
		return;
	}
	else if (%cl.deducted < %cl.added)
	{
		return;
	}

	messageClient(%cl, '', "\c0$" @ mFloatLength(%cl.deducted - %cl.added, 2) @ "\c6 has been deducted from your account");
	%cl.deducted = 0;
	%cl.added = 0;
}

function sellObject(%b)
{
	%group = getBrickgroupFromObject(%b);
	if (%b.sold || !isEventPending($masterGrowSchedule) || %group.isSaveClearingLot)
	{
		return;
	}

	%db = %b.getDatablock();
	%cl = %group.client;

	if (%cl.isBuilder || %cl.noRefund)
	{
		return;
	}

	%cost = %db.cost;
	
	%b.sold = 1;

	if (!isObject(%cl) || %cl.getClassName() !$= "GameConnection")
	{
		%group.delayedScoreAdjustment += %cost;
		return;
	}

	%cl.addMoney(%cost);
	%cl.added += %cost;
	cancel(%cl.addMoneyMessageSchedule);
	%cl.centerPrint("<color:00ff00>$" @ mFloatLength(%cost, 2) @ "<color:ffffff> - " @ %db.uiName, 2);
	%cl.schedule(50, centerPrint, "<color:00cc00>$" @ mFloatLength(%cost, 2) @ "<color:cccccc> - " @ %db.uiName, 2);
	%cl.addMoneyMessageSchedule = schedule(800, 0, messageAddedMoney, %cl);
}

function messageAddedMoney(%cl)
{
	if (%cl.added == 0)
	{
		return;
	}
	else if (%cl.added <= %cl.deducted)
	{
		return;
	}

	messageClient(%cl, '', "\c2$" @ mFloatLength(%cl.added - %cl.deducted, 2) @ "\c6 has been added to your account");
	%cl.deducted = 0;
	%cl.added = 0;
}