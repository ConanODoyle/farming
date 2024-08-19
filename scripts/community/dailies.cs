$numDailyRequirements = 4;
$DailyItemCount = 100; //divide by sqrt(price)

function dailyRefreshSchedule()
{
	cancel($dailyRefreshSchedule);

	%timeLeft = getDailyTimeLeft();
	%timeLeftMS = getField(%timeLeft, 1);
	%timeLeftReadable = getField(%timeLeft, 0);

	$time = 600000;
	if (%timeLeft > %timeLeftMS)
	{
		%time = getMax(%timeLeft / 2, 60000);
	}

	%nowDay = getWord(getDateTime(), 0);
	if ($lastDay !$= %nowDay)
	{
		generateDailyGoal();
		AIConsole.name = "Challenge Manager";
		AIConsole.bl_id = ":trophy:";
		talk("A new daily goal has been issued! Use /dailyProgress to see your progress.");
		AIConsole.name = "Console";
		AIConsole.bl_id = ":robot:";
	}

	$lastDay = %nowDay;
	$dailyRefreshSchedule = schedule(%timeLeft, 0, dailyRefreshSchedule);
}

function generateDailyGoal()
{
	$CurrDailyGoalID = getSafeDataIDArrayName("DailyGoal_" @ strReplace(getWord(getDateTime(), 0), "/", "_"));
	if (getDataIDArrayTagValue($CurrDailyGoalID, "generated"))
	{
		warn("Already generated daily goal for " @ $CurrDailyGoalID @ ", exiting...");
		return;
	}
	generateRequirements($CurrDailyGoalID, $numDailyRequirements, CommonRequests);
	setDataIDArrayTagValue($CurrDailyGoalID, "generated", 1);
}

function getDailiesCompleted(%cl)
{
	%blid = %cl.bl_id;
	%dataID = "TotalDailiesCompleted";
	return getDataIDArrayValue(%dataID, %blid) + 0;
}

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

function checkDailyGoalProgress(%dataID, %cl)
{
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
		%text = %text @ "\n\nYou have completed the daily goal! Go to the clock tower at HTP to claim your reward!";
	}
	%cl.messageBoxOKLong(%title, %text);
}

function grantDailyReward(%cl)
{
	if (!isObject(%cl))
	{
		return;
	}
	%blid = %cl.bl_id;
	%tag = getSafeDataIDArrayName("Completed_" @ %cl.bl_id);
	
	if (getDataIDArrayTagValue($CurrDailyGoalID, %tag))
	{
		commandToClient(%cl, 'MessageBoxOK', "Can't grant reward!", "You already redeemed your daily goal reward!");
		return;
	}
	%reward = "Tix 100" TAB "Bux 10" TAB "cashReward 100";
	%item = grantGoalReward(%cl, %reward, $CurrDailyGoalID);
	if (!isObject(item))
	{
		return;
	}
	setDataIDArrayTagValue($CurrDailyGoalID, %tag, 1);
}

function getDailiesCompleted(%cl)
{
	%blid = %cl.bl_id;
	%aid = "TotalDailiesCompleted";
	return getDataIDArrayValue(%aid, %blid) + 0;
}
