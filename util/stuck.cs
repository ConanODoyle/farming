function serverCmdStuck(%cl)
{
	if (%cl.nextUnstuckTime < $Sim::Time)
	{
		if (isObject(%pl = %cl.player))
		{
			%xdiffs = "0.5 0.5 -0.5 -0.5 0";
			%ydiffs = "0.5 -0.5 0.5 -0.5 0";
			for (%i = 0; %i < getWordCount(%xdiffs); %i++)
			{
				%offset = getWord(%xdiffs, %i) SPC getWord(%ydiffs, %i) SPC 3;
				%start = vectorAdd(%pl.getPosition(), %offset);
				%end = vectorAdd(%start, "0 0 -3");
				%ray = containerRaycast(%start, %end, $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType, %pl);

				if(isObject(firstWord(%ray)))
				{
					%newPos = getWords(%ray, 1, 3);
					%rotation = getWords(%pl.getTransform(), 3, 6);
					%normal = getWords(%ray, 4, 7);

					// Adjust for sloped surfaces
					%dot = 1 - vectorDot("0 0 1", %normal);
					%newPos = vectorAdd(%newPos, "0 0" SPC %dot);

					%pl.setTransform(%newPos SPC %rotation);
					%cl.nextUnstuckTime = $Sim::Time + 3;
					return;
				}
			}

			//did not find anything
			%cl.nextUnstuckTime = $Sim::Time + 1;
			%cl.chatMessage("You are not stuck.");
		}
	}
	else
	{
		messageClient(%cl, '', "You have to wait to do /stuck again!");
	}
}
