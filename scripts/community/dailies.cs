$numDailyRequirements = 4;
$DailyItemCount = 100; //divide by sqrt(price)

function dailyRefreshSchedule()
{
	cancel($dailyRefreshSchedule);

	%timeLeft = getDailyTimeLeft();
	%timeLeftSec = getField(%timeLeft, 1);
	%timeLeftReadable = getField(%timeLeft, 0);

	$time = 600000;
	if (%time / 1000 > %timeLeftSec)
	{
		%time = getMax(%timeLeftSec * 500, 120000);
	}

	%nowDay = getWord(getDateTime(), 0);
	if ($lastDay !$= %nowDay)
	{
		generateDailyGoal();
		AIConsole.name = "[DAILY]";
		AIConsole.bl_id = ":sun:";
		talk("A new daily request has been issued! Use /dailyProgress to see your progress.");
		AIConsole.name = "Console";
		AIConsole.bl_id = ":robot:";
	}
	else if (%timeLeftSec < 3600)
	{
		messageAll('', "<font:Palatino Linotype:28>\c3[DAILY]\c6 There is \c3" @ %timeLeftReadable @ "\c6 left before the daily requests reset");
	}

	$lastDay = %nowDay;
	$dailyRefreshSchedule = schedule(%timeLeft, 0, dailyRefreshSchedule);
}

function generateDailyGoal()
{
	$CurrDailyGoalID = getSafeDataIDArrayName("DailyGoal_" @ strReplace(getWord(getDateTime(), 0), "/", "_"));
	if (getDataIDArrayTagValue($CurrDailyGoalID, "generated"))
	{
		warn("Already generated daily request for " @ $CurrDailyGoalID @ ", exiting...");
		return;
	}
	generateRequirements($CurrDailyGoalID, $numDailyRequirements, CommonRequests);
	setDataIDArrayTagValue($CurrDailyGoalID, "generated", 1);
}

function getDailiesCompleted(%cl)
{
	return %cl.dailiesCompleted;
}

function incDailiesCompleted(%cl)
{
	%cl.dailiesCompleted++;
}
RegisterPersistenceVar("dailiesCompleted", false, "");

function getDailyTimeLeft()
{
	%time = getWord(getDateTime(), 1);
	%time = strReplace(%time, ":", "\t");
	%hrLeft = 23 - getField(%time, 0);
	%minLeft = 59 - getField(%time, 1);
	%secLeft = 59 - getField(%time, 2);

	return %hrLeft @ "h " @ %minLeft @ "m " @ %secLeft @ "s" TAB (%hrLeft * 60 * 60 + %minLeft * 60 + %secLeft);
}

function getDailyGoalCropRequirement(%crop)
{
	%price = getSellPrice(%crop);
	return mFloor($DailyItemCount / mSqrt(%price));
}

function checkDailyGoalProgress(%cl)
{
	%dataID = $CurrDailyGoalID;
	if (%dataID $= "")
	{
		talk("Cannot check daily progress: No daily generated!");
		error("Cannot check daily progress: No daily generated!");
		return 0;
	}
	%blid = %cl.bl_id;
	%count = getDataIDArrayCount(%dataID);
	for (%i = 0; %i < %count; %i++)
	{
		%crop = getDataIDArrayValue(%dataID, %i);
		%amount = getGoalCropRequirement(%crop);
		%tag = "Progress_" @ %blid @ "_" @ %crop;
		%currCount = getDataIDArrayTagValue(%dataID, %tag);

		if (%currCount < %amount)
		{
			return 0;
		}
	}
	return 1;
}

function serverCmdDailyProgress(%cl)
{
	displayDailyProgress(%cl);
}

function displayDailyProgress(%cl)
{
	//called by dialogue set directly, so get actual client accordingly
	if (%cl.class $= "DialogueData")
	{
		%cl = %cl.player.client;
	}

	if (!isObject(%cl) || $CurrDailyGoalID $= "")
	{
		talk("Unable to display daily requirements to " @ %cl.name @ "!");
		error("Unable to display daily requirements to " @ %cl.name @ "!");
		return;
	}

	%title = "Daily Progress " @ getWord(getDateTime(), 0);
	
	%header = "<font:Palatino Linotype:24>";
	%body = "<font:Arial:20>";

	%text = %header @ "Time left: " @ getField(getDailyTimeLeft(), 0) @ "\n\n";

	%text = %text @ "Progress:" @ %body;
	for (%i = 0; %i < $numDailyRequirements; %i++)
	{
		%requiredType = getDataIDArrayValue($CurrDailyGoalID, %i);
		%amount = getDailyGoalCropRequirement(%requiredType);
		%completed = getCropProgressForGoal($CurrDailyGoalID, %cl, %requiredType);
		if (%amount > %completed)
		{
			%finished++;
		}
		%suffix = %amount > %completed ? "" : "\xab";
		%prefix = %amount > %completed ? "" : "\xbb";
		%text = %text @ "\n" @ %requiredType @ ": " @ %prefix SPC %completed @ " / " @ %amount SPC %suffix;
	}

	if (%finished == $numDailyRequirements)
	{
		%text = %text @ "\n\nYou have completed the daily request! Go to the clock tower at HTP to claim your reward!";
	}
	%cl.messageBoxOKLong(%title, %text);
}

function grantDailyReward(%cl)
{
	//called by dialogue set directly, so get actual client accordingly
	if (%cl.class $= "DialogueData")
	{
		%cl = %cl.player.client;
	}

	if (!isObject(%cl) || $CurrDailyGoalID $= "")
	{
		return;
	}
	%blid = %cl.bl_id;
	%tag = getSafeDataIDArrayName("Completed_" @ %cl.bl_id);
	
	if (getDataIDArrayTagValue($CurrDailyGoalID, %tag))
	{
		commandToClient(%cl, 'MessageBoxOK', "Can't grant reward!", "You already redeemed your daily request reward!");
		return;
	}
	%reward = "Tix 100" TAB "Bux 10" TAB "cashReward 100";
	%item = grantGoalReward(%cl, %reward, $CurrDailyGoalID);
	if (!isObject(%item))
	{
		return;
	}
	setDataIDArrayTagValue($CurrDailyGoalID, %tag, 1);
	incDailiesCompleted(%cl);
}

function hasClaimedDailyReward(%cl)
{
	if (!isObject(%cl))
	{
		return 0;
	}
	else if ($CurrDailyGoalID $= "")
	{
		talk("Cannot check claimed daily progress: No daily generated!");
		error("Cannot check claimed daily progress: No daily generated!");
		return 0;
	}
	%tag = getSafeDataIDArrayName("Completed_" @ %cl.bl_id);
	
	return getDataIDArrayTagValue($CurrDailyGoalID, %tag);
}

package DailyGoals
{
	function AIPlayer::attemptBuy(%bot, %item)
	{
		%db = %item.dataBlock;
		%count = %item.count;
		%type = %db.stackType;
		%blid = %item.bl_id;
		%client = findClientByBL_ID(%blid);

		%ret = parent::attemptBuy(%bot, %item);
		if (!%ret || %count <= 0 || !%item.isStackable || !isObject(%client)
			|| $CurrDailyGoalID $= "" //no daily generated
			|| getDataIDArrayTagValue($CurrDailyGoalID, %type) != 1) //not part of daily
		{
			return %ret;
		}

		addCropProgressForGoal($CurrDailyGoalID, %client, %type, %amt);
		if (checkDailyGoalProgress(%client) && !hasClaimedDailyReward(%client))
		{
			commandToClient(%client, 'MessageBoxOK', "Daily Complete", 
				"You have completed today's daily requests! Head to the clock tower at HTP to claim your reward!");
		}

		return %ret;
	}
};
activatePackage(DailyGoals);