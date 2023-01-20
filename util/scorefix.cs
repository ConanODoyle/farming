//%money in dollars, eg $5.50 == 5.5
function GameConnection::addMoney(%cl, %money)
{
	%money = (%money * 100) | 0;
	%cl.score = (%cl.score + %money) | 0;
}

function GameConnection::subMoney(%cl, %money)
{
	%money = (%money * 100) | 0;
	%cl.score = (%cl.score - %money) | 0;
}

//returns 1 if has enough, 0 if not. $5.50 == 5.5
function GameConnection::checkMoney(%cl, %money)
{
	%money = (%money * 100) | 0;
	return %cl.score >= %money;
}

function GameConnection::getMoney(%cl)
{
	%money = %cl.score | 0;
	return getSubStr(%money, 0, strLen(%money) - 2) @ "." @ getSubStr(%money, strLen(%money) - 2, 2);
}