function serverCmdStuck(%cl)
{
	if (%cl.nextUnstuckTime < $Sim::Time)
	{
		if (isObject(%pl = %cl.player))
		{
			%pl.setTransform(vectorAdd(%pl.getTransform(), "0 0 2.5") SPC getWords(%pl.getTransform(), 3, 6));
			%cl.nextUnstuckTime = $Sim::Time + 3;
		}
	}
	else
	{
		messageClient(%cl, '', "You have to wait to do /stuck again!");
	}
}
