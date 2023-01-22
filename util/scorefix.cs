//%money in dollars, eg $5.50 == 5.5
function GameConnection::addMoney(%cl, %money)
{
	%money = (%money * 100) | 0;
	%cl.score = IMath_Add(%cl.score, %money);
}

function GameConnection::subMoney(%cl, %money)
{
	%money = (%money * 100) | 0;
	%cl.score = IMath_Subtract(%cl.score, %money);
}

//returns 1 if has enough, 0 if not. $5.50 == 5.5
function GameConnection::checkMoney(%cl, %money)
{
	%money = (%money * 100) | 0;
	return aGreaterThanOrEqualTob(%cl.score, %money);
}

function GameConnection::getMoney(%cl)
{
	%money = %cl.score;
	%len = strLen(%money);
	%dollar = "0";
	%cent = %money;
	if(%len == 1)
	{
		%cent = "0" @ %money;
	}
	if(%len >= 2)
	{
		%cent = getSubStr(%money, %len - 2, 2);
	}
	if(%len > 2)
	{
		%dollar = getSubStr(%money, 0, %len - 2);
	}
	return %dollar @ "." @ %cent;
}