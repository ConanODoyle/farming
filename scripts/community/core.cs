$DailyItemCount = 100; //multiply by price

function generateDailyRequirements()
{
	%numTypes = 4;
	$YesterdayDailyQuestID = $CurrDailyQuestID;
	$CurrDailyQuestID = getSafeDataIDArrayName("DailyQuest_" @ strReplace(getWord(getDateTime(), 0), "/", "_"));

	for (%i = 0; %i < %numTypes; %i++)
	{
		//tables taken from config/quests.cs
		%count = CommonRequests.count;
		%pick = getField(CommonRequests.option[getRandom(%count - 1)], 0);
		while (%picked[%pick]) //wish we had do while :(
		{
			%pick = getField(CommonRequests.option[getRandom(%count - 1)], 0);
		}
		%picked[%pick] = 1;
		setDataIDArrayValue($CurrDailyQuestID, %i, %pick);
		setDataIDArrayTagValue($CurrDailyQuestID, %i, %pick);
	}
}

function addCropToDailies(%cl, %crop, %total)
{
	%blid = %cl.bl_id;
	if ([crop not in dailies])
	{
		return;
	}
	%currCount = getDataIDArrayTagValue("DailyQuest", "Contrib" @ %blid @ "_" @ %crop);
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
}

function checkDailyProgress(%cl)
{

}

function displayDailyProgress(%cl)
{

}

function grantDailyReward(%cl)
{

}