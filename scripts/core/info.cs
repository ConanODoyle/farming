function serverCmdGiveMoney(%cl, %amount, %t1, %t2, %t3, %t4)
{
	if (stripChars(%amount, "abcdefghijklmnopqrstuvwxyz") $= "" || %amount $= "")
	{
		messageClient(%cl, '', "\c6Usage: \c3/giveMoney [amount] [name]");
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
	else if (mFloor(%amount * 4) / 4 < 1)
	{
		messageClient(%cl, '', "You cannot give less than $0.25!");
		return;
	}
	else if (mFloor(%amount * 4) / 4 > 1000)
	{
		messageClient(%cl, '', "You cannot give more than $1000 at a time!");
		return;
	}
	else if (%cl.lastGive + 3 > $Sim::Time)
	{
		messageClient(%cl, '', "You must wait a few seconds before giving money again!");
		return;
	}
	else if ((%amt = mFloor(%amount * 4) / 4) > %cl.score)
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
	%cl.setScore(%cl.score - %amt);
	messageClient(%cl, 'MsgUploadEnd', "\c6You gave \c3" @ %target.name @ "\c2 $" @ %amt);
	messageClient(%target, 'MsgUploadEnd', "\c3" @ %cl.name @ "\c6 gave you\c2 $" @ %amt);

	%cl.lastGive = $Sim::Time;
}

package ScoreGrant
{
	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);
		bottomprintInfoLoop(%cl);
		return %ret;
	}
};
activatePackage(ScoreGrant);

function bottomprintInfoLoop(%cl)
{
	cancel(%cl.bottomprintMoneySched);

	if (%cl.ndMode !$= "NDM_Disabled")
	{
		%cl.bottomprintMoneySched = schedule(1000, %cl, bottomprintInfoLoop, %cl);
		return;
	}

	%pl = %cl.player;
	if (isObject(%pl.tempbrick) && %pl.tempbrick.getDatablock().cost > 0)
	{
		%db = %pl.tempbrick.getDatablock();
		%pre = "<color:ffff00>" @ %db.uiname @ " Cost: $" @ %db.cost;
	}
	else if (%client.infoPrefix $= "")
	{
		%experience = %cl.farmingExperience = mFloor(%cl.farmingExperience + 0);
		%pre = "<color:ffff00>XP: " @ %experience SPC %cl.latestExp;
		
		if (isObject(%pl) && isObject(%image = %pl.getMountedImage(0)))
		{
			//show information on the item being held
			//if its a seed, show information on the seed type
			%cropType = %image.cropType;
			%expRequirement = $Farming::Crops::PlantData_[%cropType, "experienceRequired"];
			%expCost = $Farming::Crops::PlantData_[%cropType, "experienceCost"];

			if (%expRequirement !$= "")
			{
				if (%cl.farmingExperience < %expRequirement || %cl.farmingExperience < %expCost)
				{
					%color = "<color:ff0000>";
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
				%pre = %pre @ "<color:ffffff>     [" @ %image.toolTip @ "]";
			}
			else
			{
				%start = %pl.getHackPosition();
				%end = getWords(%start, 0, 1) SPC 0;

				%ray = containerRaycast(%start, %end, $Typemasks::fxBrickAlwaysObjectType);

				while (isObject(%hit = getWord(%ray, 0)) && %safety++ < 100)
				{
					if (%hit.getDatablock().isLot)
					{
						%owner = getBrickgroupFromObject(%hit).name;
						%bl_id = getBrickgroupFromObject(%hit).bl_id;
						if (%hit.getDatablock().isSingle)
						{
							%isSingle = 1;
							%single = "Single ";
						}
						break;
					}
					%ray = containerRaycast(vectorSub(getWords(%ray, 1, 3), "0 0 0.1"), %end, $Typemasks::fxBrickAlwaysObjectType, %hit);
				}

				if (%owner !$= "" && %bl_id != 888888 && %bl_id != 999999)
				{
					%pre = %pre @ "<color:ffffff>      [" @ %single @ "Lot Owner: <color:ffff00>" @ %owner @ "<color:ffffff>]";
				}
				else if (%owner !$= "")
				{
					%pre = %pre @ "<color:ffffff>      [" @ %single @ "Lot Owner: None]";
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
				if (%hit.getDatablock().isLot)
				{
					%owner = getBrickgroupFromObject(%hit).name;
					%bl_id = getBrickgroupFromObject(%hit).bl_id;
					if (%hit.getDatablock().isSingle)
					{
						%isSingle = 1;
						%single = "Single ";
					}
					break;
				}
				%ray = containerRaycast(vectorSub(getWords(%ray, 1, 3), "0 0 0.1"), %end, $Typemasks::fxBrickAlwaysObjectType, %hit);
			}

			if (%owner !$= "" && %bl_id != 888888 && %bl_id != 999999)
			{
				%pre = %pre @ "<color:ffffff>      [" @ %single @ "Lot Owner: <color:ffff00>" @ %owner @ "<color:ffffff>]";
			}
			else if (%owner !$= "")
			{
				%pre = %pre @ "<color:ffffff>      [" @ %single @ "Lot Owner: None]";
			}
		}
	}

	%amount = "<just:right><color:ffff00>Money: $" @ mFloatLength(%cl.score, 2);

	%cl.bottomprint(%pre @ %amount @ " ", 2, 0);

	%cl.bottomprintMoneySched = schedule(300, %cl, bottomprintInfoLoop, %cl);
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
		%cl.latestExp = (%cl.recentEXP < 0 ? "<color:ff0000>" @ %cl.recentEXP + 0 @ "<color:ffff00>" : "+" @ %cl.recentEXP + 0);
		bottomprintInfoLoop(%cl); //force update bottomprint
		%cl.resetRecentExpSched = schedule(4000, %cl, eval, %cl @ ".recentEXP = 0; " @ %cl @ ".latestExp = \"\";");
	}
}