function getPluralWord(%word)
{
	%lastChar = getSubStr(%word, strLen(%word) - 1, 1);
	%lastWord = getWord(%word, getWordCount(%word) - 1);
	%noPluralAppend = "wheat corn coal phosphate";
	%esAppend = "tomato potato";
	
	if (strPos(%noPluralAppend, strLwr(%lastWord)) >= 0)
	{
		return %word;
	}
	else if (%lastChar $= "y")
	{
		return getSubStr(%word, 0, strLen(%word) - 1) @ "ies";
	}
	else if (%lastChar $= "x" || %lastChar $= "s" || strPos(%esAppend, strLwr(%lastWord)) >= 0)
	{
		return %word @ "es";
	}
	else if (%lastWord $= "Cactus")
	{
		return getSubStr(%word, 0, strLen(%word) - 2) @ "i";
	}
	else
	{
		return %word @ "s";
	}
}