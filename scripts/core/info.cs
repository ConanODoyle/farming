function serverCmdGiveMoney(%cl, %amount, %t1, %t2, %t3, %t4)
{
	if (stripChars(%amount, "abcdefghijklmnopqrstuvwxyz") $= "" || %amount $= "")
	{
		messageClient(%cl, '', "\c6Usage: \c3/giveMoney [amount] [name]");
		messageClient(%cl, '', "\c6If you have at least $10000 in savings, you will be charged a \c3$50 fee\c6 per transaction");
		return;
	}

	%name = trim(%t1 SPC %t2 SPC %t3 SPC %t4);
	%target = findClientByName(%name);
	if (!isObject(%cl.player))
	{
		messageClient(%cl, '', "You cannot give money while dead!");
		return;
	}
	else if (!isObject(%target.player))
	{
		messageClient(%cl, '', "Cannot find recipient! They need to be alive and nearby.");
		return;
	}
	else if (mFloor(%amount * 10) < 1)
	{
		messageClient(%cl, '', "You cannot give less than $0.10!");
		return;
	}
	else if (mFloor(%amount * 10) / 10 > 10000)
	{
		messageClient(%cl, '', "You cannot give more than $10000 at a time!");
		return;
	}
	else if (%cl.nextGive > $Sim::Time)
	{
		messageClient(%cl, '', "You must wait a few seconds before giving money again!");
		return;
	}
	else if ((%amt = mFloor(%amount * 10) / 10) > %cl.score)
	{
		if (%amt < 1)
		{
			%amt = "0" @ %amt;
		}
		messageClient(%cl, '', "You don't have $" @ %amt @ "!");
		return;
	}
	else if (vectorDist(%target.player.getPosition(), %cl.player.getPosition()) > 8)
	{
		messageClient(%cl, '', %target.name @ " is too far away!");
		return;
	}

	%target.setScore(%target.score + %amt);
	if (%cl.score >= 10000)
	{
		%cl.setScore(%cl.score - 50);
	}
	%cl.setScore(%cl.score - %amt);
	messageClient(%cl, 'MsgUploadEnd', "\c6You gave \c3" @ %target.name @ "\c2 $" @ %amt);
	messageClient(%target, 'MsgUploadEnd', "\c3" @ %cl.name @ "\c6 gave you\c2 $" @ %amt);

	%cl.nextGive = $Sim::Time + getMax(mFloor(3 * %amount / 1000), 3);
}

package ScoreGrant
{
	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);
		bottomprintInfo(%cl);
		return %ret;
	}
};
activatePackage(ScoreGrant);

function bottomprintInfo(%cl)
{
	cancel(%cl.bottomprintMoneySched);

	%pl = %cl.player;
	if (%cl.ndMode !$= "NDM_Disabled" || %pl.tool[%pl.currTool].uiname $= "Toolgun" || isEventPending(%cl.NPC_hudSch))
	{
		%cl.bottomprintMoneySched = schedule(1000, %cl, bottomprintInfoLoop, %cl);
		return;
	}

	if (isObject(%pl.tempbrick) && %pl.tempbrick.getDatablock().cost > 0)
	{
		%db = %pl.tempbrick.getDatablock();
		%pre = "\c2" @ %db.uiname SPC %db.description @ " Cost: $" @ %db.cost;
		if (%db.isSprinkler)
		{
			%pre = %pre @ ", Water/Sec: " @ %db.waterPerSecond;
		}
	}
	else if (%client.infoPrefix $= "")
	{
		%experience = %cl.farmingExperience = mFloor(%cl.farmingExperience + 0);
		%pre = "\c2XP: " @ %experience SPC %cl.latestExp;
		
		if (isObject(%pl) && isObject(%image = %pl.getMountedImage(0)))
		{
			//show information on the item being held
			//if its a seed, show information on the seed type
			%cropType = %image.cropType;
			%expRequirement = getPlantData(%cropType, "experienceRequired");
			%expCost = getPlantData(%cropType, "experienceCost");

			if (%expRequirement !$= "")
			{
				if (%cl.farmingExperience < %expRequirement || %cl.farmingExperience < %expCost)
				{
					%color = "\c0";
				}
				else if (%expCost <= %cl.farmingExperience && %expCost > 0)
				{
					%color = "<color:00ffff>";
				}

				if (%expCost > 0)
				{
					%pre = %pre SPC %color @ "    [" @ %cropType @ ": Costs " @ %expCost @ " XP]";
				}
				else
				{
					%pre = %pre SPC %color @ "    [" @ %cropType @ ": " @ %expRequirement @ " Required]";
				}
			}
			else if (%image.toolTip !$= "")
			{
				%pre = %pre @ "\c6     [" @ %image.toolTip @ "]";
			}
			else
			{
				%start = %pl.getHackPosition();
				%end = vectorSub(%start, "0 0 " @ ($maxLotBuildHeight - 0.2 * 27));

				%ray = containerRaycast(%start, %end, $Typemasks::fxBrickAlwaysObjectType);

				while (isObject(%hit = getWord(%ray, 0)) && %safety++ < 100)
				{
					if (%hit.getDatablock().isLot || %hit.getDatablock().isShopLot)
					{
						%owner = getBrickgroupFromObject(%hit).name;
						%bl_id = getBrickgroupFromObject(%hit).bl_id;
						if (%hit.getDatablock().isSingle)
						{
							%prefix = "Center ";
						}

						if (%hit.getDatablock().isShopLot)
						{
							%prefix = "Shop ";
						}

						break;
					}
					else if (%hit.getGroup().bl_id == 888888)
					{
						break;
					}
					%ray = containerRaycast(vectorSub(getWords(%ray, 1, 3), "0 0 0.1"), %end, $Typemasks::fxBrickAlwaysObjectType, %hit);
				}

				if (%owner !$= "" && %bl_id != 888888 && %bl_id != 999999)
				{
					%pre = %pre @ "\c6      [" @ %prefix @ "Lot Owner: \c2" @ %owner @ "\c6]";
				}
				else if (%owner !$= "")
				{
					%pre = %pre @ "\c6      [" @ %prefix @ "Lot Owner: None]";
				}
			}
		}
		else if (isObject(%pl))
		{
			%start = %pl.getHackPosition();
			%end = getWords(%start, 0, 1) SPC 0;

			%ray = containerRaycast(%start, %end, $Typemasks::fxBrickAlwaysObjectType);

			while (isObject(%hit = getWord(%ray, 0)) && %safety++ < 100)
			{
				if (%hit.getDatablock().isLot || %hit.getDatablock().isShopLot)
				{
					%owner = getBrickgroupFromObject(%hit).name;
					%bl_id = getBrickgroupFromObject(%hit).bl_id;
					if (%hit.getDatablock().isSingle)
					{
						%prefix = "Center ";
					}

					if (%hit.getDatablock().isShopLot)
					{
						%prefix = "Shop ";
					}

					break;
				}
				%ray = containerRaycast(vectorSub(getWords(%ray, 1, 3), "0 0 0.1"), %end, $Typemasks::fxBrickAlwaysObjectType, %hit);
			}

			if (%owner !$= "" && %bl_id != 888888 && %bl_id != 999999)
			{
				%pre = %pre @ "\c6      [" @ %prefix @ "Lot Owner: \c2" @ %owner @ "\c6]";
			}
			else if (%owner !$= "")
			{
				%pre = %pre @ "\c6      [" @ %prefix @ "Lot Owner: None]";
			}
		}
	}

	if (%cl.isBuilder)
	{
		%amount = "<just:right>\c5Builder Mode \c4$" @ mFloatLength(%cl.score, 2);
	}
	else
	{
		%amount = "<just:right>\c3Money: $" @ mFloatLength(%cl.score, 2);
	}

	%cl.bottomprint(%pre @ %amount @ " ", 2, 0);
}

function bottomprintInfoLoop(%idx)
{
	cancel($masterBottomPrintInfoLoop);

	if (!isObject(MissionCleanup))
	{
		return;
	}

	%idx += 0;
	%count = ClientGroup.getCount();
	if (%idx >= %count)
	{
		%idx = 0;
	}

	%total = 0;
	for (%idx = %idx; %idx < %count; %idx++)
	{
		if (%total >= 16)
		{
			break;
		}
		%total++;

		if (%idx >= %count)
		{
			break;
		}

		bottomprintInfo(ClientGroup.getObject(%idx));
	}

	$masterBottomPrintInfoLoop = schedule(200, 0, bottomprintInfoLoop, %idx);
}

function GameConnection::addExperience(%cl, %exp)
{
	if (%exp != 0)
	{
		// if (%exp > 0 && %cl.isDonator)
		// {
		// 	%exp *= 2;
		// }
		cancel(%cl.resetRecentExpSched);
		%cl.recentEXP += %exp;
		%cl.farmingExperience += %exp;
		%cl.latestExp = (%cl.recentEXP < 0 ? "\c0" @ %cl.recentEXP + 0 @ "\c2" : "+" @ %cl.recentEXP + 0);
		bottomprintInfo(%cl); //force update bottomprint
		%cl.resetRecentExpSched = schedule(4000, %cl, eval, %cl @ ".recentEXP = 0; " @ %cl @ ".latestExp = \"\";");
	}
}