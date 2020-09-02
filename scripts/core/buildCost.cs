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

	function fxDTSBrick::onDeath(%obj)
	{
		%db = %obj.getDatablock();
		if (%obj.isPlanted && %db.cost > 0 && !isEventPending(%obj.buySchedule))
		{
			sellObject(%obj);
		}

		return parent::onDeath(%obj);
	}

	function ND_Selection::tickCutting(%this)
	{
		cancel(%this.cutSchedule);

		//Get bounds for this tick
		%start = %this.cutIndex;
		%end = %start + $Pref::Server::ND::ProcessPerTick;

		if(%end > %this.brickCount)
			%end = %this.brickCount;

		%cutSuccessCount = %this.cutSuccessCount;
		%cutFailCount = %this.cutFailCount;

		%admin = %this.client.isAdmin;
		%group = %this.client.brickGroup.getId();
		%bl_id = %this.client.bl_id;

		//Cut bricks
		for(%i = %start; %i < %end; %i++)
		{
			%brick = $NS[%this, "B", %i];

			if(!isObject(%brick))
				continue;

			if(!ndTrustCheckModify(%brick, %group, %bl_id, %admin))
			{
				%cutFailCount++;
				continue;
			}

			// Support for hole bots
			if(isObject(%brick.hBot))
			{
				%brick.hBot.spawnProjectile("audio2d", "deathProjectile", "0 0 0", 1);
				%brick.hBot.delete();
			}

			//START OF CHANGES
			if (%brick.dataBlock.cost > 0)
			{
				sellObject(%brick);
			}
			//END OF CHANGES
			%brick.delete();
			%cutSuccessCount++;
		}

		//Save how far we got
		%this.cutIndex = %i;

		%this.cutSuccessCount = %cutSuccessCount;
		%this.cutFailCount = %cutFailCount;

		//Tell the client how much we cut this tick
		if(%this.client.ndLastMessageTime + 0.1 < $Sim::Time)
		{
			%this.client.ndUpdateBottomPrint();
			%this.client.ndLastMessageTime = $Sim::Time;
		}

		if(%i >= %this.brickCount)
			%this.finishCutting();
		else
			%this.cutSchedule = %this.schedule(30, tickCutting);
	}
};
activatePackage(BuildCost);

function buyBrick(%b)
{
	%group = getBrickgroupFromObject(%b);
	if (!isObject(%group.client) || %group.client.isBuilder || %group.isLoadingLot || %b.skipBuy)
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
	if (%cl.score >= %db.cost && %db.cost >= 0)
	{
		%cl.setScore(%cl.score - %db.cost);
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

	%group = getBrickgroupFromObject(%b);
	%db = %b.getDatablock();
	%cl = %group.client;

	if (%cl.isBuilder || %cl.noRefund)
	{
		return;
	}

	if (%b.createdTimeout < $Sim::Time)
	{
		if (%db.customRefundCost > 0)
		{
			%cost = %db.customRefundCost;
		}
		else if (%cl.refundRatio <= 0.75)
		{
			%cost = %db.cost * 3 / 4;
		}
		else
		{
			%cost = %db.cost * %cl.refundRatio;
		}
	}
	else //full refund for bad placement (destroying within $createdTimeout seconds after purchase)
	{
		%cost = %db.cost;
	}

	%b.sold = 1;

	if (!isObject(%cl) || %cl.getClassName() !$= "GameConnection")
	{
		%group.delayedScoreAdjustment += %cost;
		return;
	}

	%cl.setScore(%cl.score + %cost);
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