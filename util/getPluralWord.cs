function getPluralWord(%word)
{
    %lastChar = getSubStr(%word, getMax(0, strLen(%word) - 1), 1);
    %lastWord = getWord(%word, getWordCount(%word) - 1);
    %noPluralAppend = "wheat corn coal phosphate sticks weed killer";
    %esAppend = "tomato potato peach fish";
    %sAppend = "gameboy";

    if (striPos(%noPluralAppend, %lastWord) >= 0)
    {
        return %word;
    }
    else if (striPos(%sAppend, %lastWord) >= 0) //prioritize custom cases
    {
        return %word @ "s";
    }
    else if (striPos(%esAppend, %lastWord) >= 0) //prioritize custom cases
    {
        return %word @ "es";
    }
    else if (%lastChar $= "y")
    {
        return getSubStr(%word, 0, strLen(%word) - 1) @ "ies";
    }
    else if (%lastChar $= "x" || %lastChar $= "s")
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