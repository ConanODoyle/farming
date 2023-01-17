//returns if string contains word + the index it was found at
function containsWord(%string, %word)
{
	%count = getWordCount(%string);
	for (%i = 0; %i < %count; %i++)
	{
		if (getWord(%string, %i) $= %word)
		{
			return 1 SPC %i;
		}
	}
	return 0 SPC -1;
}

//returns if string contains field + the index it was found at
function containsField(%string, %field)
{
	%count = getFieldCount(%string);
	for (%i = 0; %i < %count; %i++)
	{
		if (getField(%string, %i) $= %field)
		{
			return 1 SPC %i;
		}
	}
	return 0 SPC -1;
}

//returns last found position of %search in %string
function strLastPos(%str, %search, %offset) {
	if(%offset > 0)
		%pos = %offset;
	else {
		%str = getSubStr(%str, 0, strLen(%str) + %offset);
		%pos = 0;
	}
	%lastPos = -1;
	while((%pos = strPos(%str, %search, %pos+1)) > 0) {
		%lastPos = %pos;
		if(%break++ >= 500) //Pretty sure strings have a max length shorter than this, should be fine
			return -2;
	}
	return %lastPos;
}

function capitalizeFirstChar(%string) 
{
	return strUpr(getSubStr(%string, 0, 1)) @ strLwr(getSubStr(%string, 1, -1));
}