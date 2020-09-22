function roundToStudCenter(%pos, %even)
{
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%z = getWord(%pos, 2);

	//%x, %y need to be in increments of 0.25 + 0.5n
	//so add 0.25, round to closest 0.5, then subtract 0.25
	if (!%even)
	{
		%x += 0.25;
		%y += 0.25;
	}

	%x = mFloor(%x * 2 + 0.5) / 2;
	%y = mFloor(%y * 2 + 0.5) / 2;

	if (!%even)
	{
		%x -= 0.25;
		%y -= 0.25;
	}

	//%z needs to be rounded to nearest 0.1
	%z = mFloor(%z * 10 + 0.5) / 10;

	return %x SPC %y SPC %z;
}