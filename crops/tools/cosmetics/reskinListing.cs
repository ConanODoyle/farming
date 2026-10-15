function fxDTSBrick::listReskinnableTools(%brick, %cl)
{
	if ($generatedReskinnableList $= "")
	{
		%str = "<font:Arial:18>";

        %count = getFieldCount($Farming::ReskinnableTools);

        for (%i = 0; %i < %count; %i++)
        {
            %tool = getField($Farming::ReskinnableTools, %i);
			%str = %str @ %tool.uiName @ "\n";
        }

		$generatedReskinnableList = trim(%str);
	}

	for (%i = 0; %i < 4; %i++)
	{
		%str[%i] = getSubStr($generatedReskinnableList, %i * 250, 250);
	}
	commandToClient(%cl, 'MessageBoxOK', "All Reskinnable Tools", '%1%2%3%4', %str0, %str1, %str2, %str3);
}

registerOutputEvent("fxDTSBrick", "listReskinnableTools", "", 1);

// function fxDTSBrick::listReskinnableTools(%brick, %cl)
// {
// 	if ($generatedReskinnableList $= "")
// 	{
// 		%str = "<font:Palatino Linotype:28>Reskinnable tools:\n";

//         %count = getFieldCount($Farming::ReskinnableTools);

//         for (%i = 0; %i < %count; %i++)
//         {
//             %tool = getField($Farming::ReskinnableTools, %i);
//             %str = %str @ "<bitmap:" @ %tool.iconName @ ">";
//         }

// 		$generatedReskinnableList = %str;
// 	}

// 	for (%i = 0; %i < 4; %i++)
// 	{
// 		%str[%i] = getSubStr($generatedReskinnableList, %i * 250, 250);
// 	}
// 	commandToClient(%cl, 'MessageBoxOK', "Sell Prices", '%1%2%3%4', %str0, %str1, %str2, %str3);
// }