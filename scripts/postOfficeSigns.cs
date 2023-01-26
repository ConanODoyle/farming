function setPostOfficeString(%office, %string)
{
	%line0 = getField(%string, 0);
	%line0 = centerString(%line0, 6);

	%line1 = getField(%string, 1);
	if (strLen(%line1) % 2 == 1)
	{
		// center line1 towards the right if it's an odd length
		// that way for signs with two lines of odd lengths, they balance nicely
		%line1 = " " @ %line1;
	}
	%line1 = centerString(%line1, 6);

	for (%i = 0; %i < 2; %i++)
	{
		for (%j = 0; %j < 6; %j++)
		{
			%brick = "_postoffice_" @ %office @ "_" @ %i @ %j;
			if (!isObject(%brick)) continue;

			%printID = alphaNumericToPrintID(getSubStr(%line[%i], %j, 1));
			%brick.setPrint(%printID);
		}
	}
}

function addPostOfficeString(%string)
{
	$Farming::PostOfficeStrings = trim($Farming::PostOfficeStrings NL %string);
}

function removePostOfficeString(%index)
{
	if (%index $= "last")
	{
		%index = getLineCount($Farming::PostOfficeStrings) - 1;
	}

	if (%index < 0 || %index > getLineCount($Farming::PostOfficeStrings) || %index $= "")
	{
		return;
	}

	$Farming::PostOfficeStrings = removeLine($Farming::PostOfficeStrings, %index);
}

function setRandomPostOfficeString(%office)
{
	%index = getRandom(1, getLineCount($Farming::PostOfficeStrings)) - 1;
	%sentinel = 0;
	while ((%string = getLine($Farming::PostOfficeStrings, %index)) $= $Farming::LastPostOfficeString[%office])
	{
		if (%sentinel > 100) break;

		%index = getRandom(1, getLineCount($Farming::PostOfficeStrings)) - 1;
		%sentinel++;
	}

	setPostOfficeString(%office, %string);
	%Farming::LastPostOfficeString[%office] = %string;
}

function randomPostOfficeLoop()
{
	cancel($PostOfficeSchedule);

	for (%i = 0; %i < 3; %i++)
	{
		setRandomPostOfficeString(%i);
	}

	$PostOfficeSchedule = schedule($Farming::PostOfficeCycleTime, 0, randomPostOfficeLoop);
}