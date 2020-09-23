function hexToInt(%str)
{
	%len = strLen(%str);
	for (%i = 0; %i < %len; %i++)
	{
		%result = %result + _singleHexToInt(getSubStr(%str, %i, 1), %len - 1 - %i);
	}
	if (%result $= "")
	{
		return 0;
	}
	return %result;
}

function _singleHexToInt(%c, %pv)
{
	%num = "0123456789abcdef";
	%i = strPos(%num, %c);
	return mPow(16, %pv) * %i;
}

function _singleIntToHex(%i)
{
	return getSubStr("0123456789abcdef", %i, 1);
}

function intToHex(%num)
{
	while (%num > 0)
	{
		%c = _singleIntToHex(%num % 16);
		%ret = %c @ %ret;
		%num = mFloor(%num / 16);
	}
	if (%ret $= "")
	{
		return 0;
	}
	return %ret;
}