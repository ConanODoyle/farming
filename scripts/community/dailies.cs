
function dailyRefreshSchedule()
{
	cancel($dailyRefreshSchedule);
}

function generateDailyGoal()
{
	$YesterdayDailyGoalID = $CurrDailyGoalID;
	$CurrDailyGoalID = getSafeDataIDArrayName("DailyGoal_" @ strReplace(getWord(getDateTime(), 0), "/", "_"));
	generateRequirements($CurrDailyGoalID, 4, CommonReGoals);
}

function addCropToDailies(%cl, %crop, %total)
{
	%blid = %cl.bl_id;
	if ([crop not in dailies])
	{
		return;
	}
	%currCount = getDataIDArrayTagValue("DailyGoal", "Contrib" @ %blid @ "_" @ %crop);
}

function getDailiesCompleted(%cl)
{
	%blid = %cl.bl_id;
	%aid = "TotalDailiesCompleted";
	return getDataIDArrayValue(%aid, %blid) + 0;
}

function getDailyTimeLeft()
{
	%time = getWord(getDateTime(), 1);
	%time = strReplace(%time, ":", "\t");
	%hrLeft = 23 - getField(%time, 0);
	%minLeft = 59 - getField(%time, 1);
	%secLeft = 59 - getField(%time, 2);
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

function dailyRefreshSchedule()
{
	cancel($dailyRefreshSchedule);
}

function displayDailyProgress(%cl)
{

}

function grantDailyReward(%cl)
{

}

function getDailiesCompleted(%cl)
{
	%blid = %cl.bl_id;
	%aid = "TotalDailiesCompleted";
	return getDataIDArrayValue(%aid, %blid) + 0;
}
