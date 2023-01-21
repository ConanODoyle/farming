// ------------------------------------	//
// --- Arbitrary Math Library β 1.0 ---	//
// ------------------------------------	//
// --- Created by Ipquarx (BLID 9291)	//
// --- With assistance/input from:		//
// --- Lugnut	(BLID 16807)			//
// --- Greek2me	(BLID 11902)			//
// --- Xalos	(BLID 11239)			//
// ------------------------------------	//
// --- Module: Helper Methods			//
// ------------------------------------	//
// --- DON'T MODIFY UNLESS YOU KNOW ---	//
// ---		 WHAT YOU'RE DOING		---	//
// ------------------------------------	//

function aLessThanb(%a, %b)
{
	//If strings are the same, then the result is automatically false
	if(%a $= %b)
		return false;

	%c = strLen(%a);
	%d = strLen(%b);

	//Only do character-by-character comparisons if lengths are equal
	if(%c != %d)
		return %c < %d;

	for(%x = 0; %x < %c; %x += 9)
	{
		%y = getSubStr(%a, %x, 9) | 0; %z = getSubStr(%b, %x, 9) | 0;

		if((%y|0) < %z)
			return true;
		else if((%y|0) > %z)
			return false;
	}
}

function aGreaterThanb(%a, %b)
{
	return aLessThanb(%b, %a);
}

function aLessThanOrEqualTob(%a, %b)
{
	return !aLessThanb(%b, %a);
}

function aGreaterThanOrEqualTob(%a, %b)
{
	return !aLessThanb(%a, %b);
}

function getZeroes(%Num)
{
	if(%Num < 0)
		%Num = 0;

	if(%Num == 0)
		return "";

	if(%Num > 64)
	{
		%r = getZeroes(%Num - 64);
		%Num = 64;
	}

	return %r @ getsubstr("0000000000000000000000000000000000000000000000000000000000000000", 0, %Num);
}

function equalizeLengths(%num1, %num2, %place)
{
	%x = strLen(%num1); %y = strLen(%num2);

	if(!%place)
	{
		if(%x < %y)
			%num1 = getZeroes(%y - %x) @ %num1;
		else if(%x > %y)
			%num2 = getZeroes(%x - %y) @ %num2;
	}
	else
	{
		if(%x < %y)
			%num1 = %num1 @ getZeroes(%y - %x);
		else if(%x > %y)
			%num2 = %num2 @ getZeroes(%x - %y);
	}

	return %num1 SPC %num2;
}

function makePositive(%Num)
{
	if(getsubstr(%num, 0, 1) !$= "-")
		return %Num;

	return getsubstr(%Num, 1, strLen(%Num));
}