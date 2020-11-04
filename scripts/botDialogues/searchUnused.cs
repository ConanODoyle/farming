function getBasicDialogueCounts(%dialogueObj)
{
	if (!isObject(%dialogueObj))
	{
		return 0;
	}

	%name = %dialogueObj.getName();
	for (%i = 0; %i < MainBrickGroup.getCount(); %i++)
	{
		%bg = MainBrickGroup.getObject(%i);
		for (%j = 0; %j < %bg.getCount(); %j++)
		{
			%brick = %bg.getObject(%j);
			if (%brick.hBot && isObject(%brick.hBot))
			{
				for (%k = 0; %k < %brick.numEvents; %k++)
				{
					if (%brick.eventOutputParameter[%k, 1] $= %name)
					{
						%count++;
						%list = %list SPC %brick;
						break; //dont count multiple times in the same brick event
					}
				}
			}
		}
	}
	return %count + 0 TAB trim(%list);
}

function getAllDialogueCounts()
{
	messageAll('', "\c6Dialogue Counts:");
	for (%i = 1; %i < 40; %i++)
	{
		if (!isObject($BotDialogue[%i]))
		{
			break;
		}
		%obj = $BotDialogue[%i];
		%count = getBasicDialogueCounts(%obj);

		messageAll('', "\c5" @ %obj.getName() @ "\c6: " @ %count);
	}
}