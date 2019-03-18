$WorldCount = 0;
$World[$WorldCount++ - 1]	= "Coast"	TAB	"1000 1000"		TAB	"-1000 -1000";
$World[$WorldCount++ - 1]	= "Desert"	TAB	"-1000 1000"	TAB	"-2000 -1000";

function getWorld(%pos)
{
	//pos is large, small
	%px = getWord(%pos, 0);
	%py = getWord(%pos, 1);
	for (%i = 0; %i < $WorldCount; %i++)
	{
		%curr = $World[%i];
		%name = getField(%curr, 0);
		%largePos = getField(%curr, 1);
		%smallPos = getField(%curr, 2);

		%sx = getWord(%smallPos, 0);
		%sy = getWord(%smallPos, 1);
		%lx = getWord(%largePos, 0);
		%ly = getWord(%largePos, 1);

		if (%px >= %sx && %px <= %lx && %py >= %sy && %py <= %ly)
		{
			return %name;
		}
	}
	return "None";
}