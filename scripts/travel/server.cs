
function fxDTSBrick::busStopPrompt(%brick, %destination, %cost, %cl)
{
	if (!isObject("_" @ getWord(%destination, 0)) || %cl.lastClickedTransit + 1 > $Sim::Time)
	{
		return;
	}
	%target = "_" @ getWord(%destination, 0);

	%cl.busCost = %cost;
	%cl.destination = %target;
	%cl.destinationName = getWords(%destination, 1, 20);
	%cl.lastClickedTransit = $Sim::Time;
	%cl.busStopBrick = %brick;

	%head = "Travel Prompt";
	%text = "Travel to " @ %cl.destinationName @ "?<br><br>Cost: $" @ mFloatLength(%cost, 2);

	commandToClient(%cl, 'MessageBoxYesNo', %head, %text, 'goToBusStopObscure');
}

registerOutputEvent("fxDTSBrick", "busStopPrompt", "string 200 100" TAB "string 200 100", 1);

function serverCmdGoToBusStopObscure(%cl)
{
	if (%cl.score < %cl.busCost)
	{
		commandToClient(%cl, 'messageBoxOK', "Can't ride bus!", "You can't afford the bus ride!");
	}
	else if (!isObject(%cl.player))
	{
		commandToClient(%cl, 'messageBoxOK', "Can't ride bus!", "You can't ride the bus while dead!");
	}
	else if (vectorDist(%cl.player.position, %cl.busStopBrick.getPosition()) > 8)
	{
		commandToClient(%cl, 'messageBoxOK', "Can't ride bus!", "You're too far away!");
	}
	else
	{
		%cl.setScore(%cl.score - %cl.busCost);
		%cl.player.dismount();
		%cl.player.position = %cl.destination.getPosition();
		%cl.player.schedule(100, spawnProjectile, "", "spawnProjectile", "", 1);

		commandToClient(%cl, 'messageBoxOK', "Arrived!", "You have arrived at " @ %cl.destinationName @ "!");
	}
	%cl.busCost = "";
	%cl.destination = "";
	%cl.destinationName = "";
	%cl.busStopBrick = "";
}

function fxDTSBrick::changeWorld(%brick, %destination, %cl)
{
	if (!isObject("_" @ getWord(%destination, 0)) || %cl.lastClickedTransit + 1 > $Sim::Time)
	{
		return;
	}
	%target = "_" @ getWord(%destination, 0);

	%cl.destination = %target;
	%cl.destinationName = getWords(%destination, 1, 20);
	%cl.lastClickedTransit = $Sim::Time;
	%cl.ticketBrick = %brick;

	%head = "Travel Prompt";
	%text = "Travel to " @ %cl.destinationName @ "?<br><br><font:Palatino Linotype:24><color:bb0000>Your money and lots will be reset due to customs and land ownership regulations! You may keep up to $800.";

	commandToClient(%cl, 'MessageBoxYesNo', %head, %text, 'changeWorld');
}

registerOutputEvent("fxDTSBrick", "changeWorld", "string 200 100", 1);

function serverCmdChangeWorld(%cl)
{
	else if (!isObject(%cl.player))
	{
		commandToClient(%cl, 'messageBoxOK', "Can't travel!", "You can't travel while dead!");
	}
	else if (vectorDist(%cl.player.position, %cl.ticketBrick.getPosition()) > 8)
	{
		commandToClient(%cl, 'messageBoxOK', "Can't travel!", "You're too far away!");
	}
	else
	{
		%cl.repeatSellAllLots = 1;
		%cl.noRefund = 1;
		serverCmdSellAllLots(%cl);

		schedule(20000, 0, eval, %cl @ ".noRefund = 0;");

		%cl.setScore(getMin(%cl.score, 800));
		%cl.player.dismount();
		%cl.player.position = %cl.destination.getPosition();
		%cl.player.schedule(100, spawnProjectile, "", "spawnProjectile", "", 1);

		messageAll('', "<bitmap:base/client/ui/ci/star> \c3" @ %cl.name @ "\c6 has traveled to " %cl.destinationName);
		commandToClient(%cl, 'messageBoxOK', "Arrived!", "You have arrived at " @ %cl.destinationName @ "!<br><br>You may want to rejoin to unghost the bricks from the map you came from.");
	}
	%cl.destination = "";
	%cl.destinationName = "";
	%cl.ticketBrick = "";
}


function collectTeledoors(%group, %i, %g, %c)
{
	if (%group $= "")
	{
		announce("Starting search for Teledoors...");
		deleteVariables("$VehicleSpawns*");
		%group = MainBrickGroup.getObject(0);
		%i = 0;
		%g = 0;
		%c = 0;
		$VehicleSpawns::Count = 0;
	} 

	%count = %group.getCount();
	for (%idx = %i; %idx < 128 + %i; %idx++)
	{
		if (%idx >= %count)
		{
			if (%g == MainBrickGroup.getCount() -1)
			{
				announce("Vehicle spawn search is complete. Found " @ %c);
				return;
			}
			%group = MainBrickGroup.getObject(%g++);
			schedule(1, %group, collectTeledoors, %group, 0, %g, %c);
			return;
		}
		%brick = %group.getObject(%idx);

		if (%brick.getDatablock().getName() $= "brickTeledoorData")
		{
			$VehicleSpawns::Spawn[%c] = %brick;
			$VehicleSpawns::Count++;
			%c++;
		}
	}
	%i = %idx;
	schedule(0, %group, collectTeledoors, %group, %i, %g, %c);
	$currGroup = %group;
}

function serverCmdNextTeledoor(%cl)
{
	if (!%cl.isAdmin == 1)
	{
		return;
	}

	%cl.currVehicleSpawn = (%cl.currVehicleSpawn + 1) % $VehicleSpawns::Count;
	
	if (isObject(%pl = %cl.player) && isObject($VehicleSpawns::Spawn[%cl.currVehicleSpawn]))
	{
		%pl.position = $VehicleSpawns::Spawn[%cl.currVehicleSpawn].getPosition();
	}
}

function serverCmdCollectTeledoors(%cl)
{
	if (!%cl.isAdmin == 1)
	{
		return;
	}

	collectTeledoors();
}