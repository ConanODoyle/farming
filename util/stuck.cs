function serverCmdStuck(%cl)
{
	if (%cl.nextUnstuckTime < $Sim::Time)
	{
		if (isObject(%pl = %cl.player))
		{
			%start = vectorAdd(%pl.getPosition(), "0 0 3");
			%end = %pl.getPosition();
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
			}
			else
			{
				%cl.nextUnstuckTime = $Sim::Time + 1;
				%cl.chatMessage("You are not stuck.");
			}
		}
	}
	else
	{
		messageClient(%cl, '', "You have to wait to do /stuck again!");
	}
}
