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
	return aGreaterThanOrEqualTob(%cl.score, %money);
}

function GameConnection::getMoney(%cl)
{
	%money = %cl.score;
	return getSubStr(%money, 0, strLen(%money) - 2) @ "." @ getSubStr(%money, strLen(%money) - 2, 2);
}