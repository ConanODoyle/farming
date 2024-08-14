

function generateRequirements(%dataID, %numTypes, %table)
{
	setDataIDArrayCount(%dataID, %numTypes);
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
		addToDataIDArray(%dataID, %pick);
		setDataIDArrayTagValue(%dataID, %pick, 1);
	}
	echo("Generated " @ %numTypes @ " requirements for " @ %dataID);
}

function addCropProgressForGoal(%dataID, %cl, %crop, %amt)
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

function getCropProgressForGoal(%dataID, %cl, %crop)
{
	%blid = %cl.bl_id;
	if (getDataIDArrayTagValue(%dataID, %crop) != 1)
	{
		return -1;
	}
	%tag = "Progress_" @ %blid @ "_" @ %crop;
	return getDataIDArrayTagValue(%dataID, %tag) + 0;
}

function grantGoalReward(%cl, %rewardList, %sourceID)
{
	if (!isObject(%player = %cl.player))
	{
		commandToClient(%cl, 'MessageBoxOK', "Can't grant reward!", "You need to be spawned to redeem this reward!");
		return 0;
	}
	%packageDataID = "DailyPackage_" @ getRandomHash("dailypackage");
	
	for (%i = 0; %i < getFieldCount(%rewardList); %i++)
	{
		%reward = getField(%rewardList, %i);

		addToPackageArray(%packageDataID, %reward);
	}
	
	%package = createPackage(%packageDataID, %player, %player.position, %sourceID);
	return 1 SPC %package;
}