
function gameConnection::messageBoxYesNoLong(%this, %title, %message, %taggedOkCmd)
{
	%maxSplits = 4;
	%hardLimit = 255 * %maxSplits;

	if(strLen(%message) > %hardLimit)
	{
		talk("ERROR: messageBoxYesNoLong max length breached!");
		return;
	}

	for(%i = 0; %i < %maxSplits; %i++)
	{
		%splitString[%i] = getSubStr(%message, %i * 255, 255);
	}

	commandToClient(%this, 'messageBoxYesNo', %title, '%2%3%4%5', %taggedOkCmd, %splitString[0], %splitString[1], %splitString[2], %splitString[3]);
}

function gameConnection::messageBoxOKLong(%this, %title, %message)
{
	%maxSplits = 4;
	%hardLimit = 255 * %maxSplits;

	if(strLen(%message) > %hardLimit)
	{
		talk("ERROR: messageBoxOKLong max length breached!");
		return;
	}

	for(%i = 0; %i < %maxSplits; %i++)
	{
		%splitString[%i] = getSubStr(%message, %i * 255, 255);
	}

	commandToClient(%this, 'messageBoxOK', %title, '%1%2%3%4', %splitString[0], %splitString[1], %splitString[2], %splitString[3]);
}