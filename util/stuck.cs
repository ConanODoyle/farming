function serverCmdStuck(%cl)
{
	if (%cl.nextUnstuckTime < $Sim::Time)
	{
		if (isObject(%pl = %cl.player))
		{
			%bot = new AIPlayer(stuckBot) {
				dataBlock = %pl.getDatablock();
			};
			%bot.setTransform("0 0 1000");
			%bot.hideNode("ALL");
			%bot.setScale(%pl.getScale());
			%bot.setTransform(%pl.getTransform());
			%bot.schedule(20, setJumping, 1);
			%bot.schedule(1000, delete);
			cancel(%pl.checkSchedule);
			%pl.checkSchedule = schedule(33, %pl, checkStuck, %pl, %bot);
		}
	}
	else
	{
		messageClient(%cl, '', "You have to wait to do /stuck again!");
	}
}

function checkStuck(%pl, %bot)
{
	%cl = %pl.client;
	if (%cl.isAdmin)
	{
		messageClient(%cl, '', %bot.getVelocity());
	}
	if (vectorLen(%bot.getVelocity()) < 0.1)
	{
		%bot.delete();
		%pl.position = vectorAdd(%pl.position, "0 0 2.75");
		%pl.setVelocity("0 0 0");
		%cl.nextUnstuckTime = $Sim::Time + 3;
	}
	else
	{
		%bot.delete();
		%cl.nextUnstuckTime = $Sim::Time + 1;
		%cl.chatMessage("You are not stuck.");
	}
}