$DailyItemCount = 100; //multiply by price

function generateRequirements(%dataID, %numTypes, %table)
{
	for (%i = 0; %i < %numTypes; %i++)
	{
		//tables taken from config/quests.cs
		%count = %table.count;
		%pick = getField(%table.option[getRandom(%count - 1)], 0);
		while (%picked[%pick]) //wish we had do while :(
		{
			%pick = getField(%table.option[getRandom(%count - 1)], 0);
		}
		%picked[%pick] = 1;
		addDataIDArrayValue(%dataID, %i, %pick);
		setDataIDArrayTagValue(%dataID, %pick, 1);
	}
	echo("Generated requirements " @ %numTypes @ " for " @ %dataID);
}

function addCropProgressToGoal(%dataID, %cl, %crop, %amt)
{
	%blid = %cl.bl_id;
	if (getDataIDArrayTagValue(%dataID, %crop) != 1)
	{
		return;
	}
	%tag = "Progress_" @ %blid @ "_" @ %crop;
	%currCount = getDataIDArrayTagValue(%dataID, %tag);
	setDataIDArrayTagValue(%dataID, %tag, %currCount + %amt);
}

function grantGoalReward(%cl, %rewardList)
{
	%packageDataID = "DailyPackage_" @ getRandomHash("dailypackage");
	
	%numRewards = getDataIDArrayTagValue(%questID, "numRewards");
	for (%i = 0; %i < %numRewards; %i++) {
		%reward = getDataIDArrayValue(%questID, %i);
		addToPackageArray(%packageDataID, %reward);
	}

	%cashReward = getDataIDArrayTagValue(%questID, "cashReward");
	addToPackageArray(%packageDataID, "cashReward" SPC %cashReward);
	//TODO: Mailbox dropoff
	createPackage(%packageDataID, %player, %player.position, %questID);

}