
function fxDTSBrick::busStopPrompt(%brick, %destination, %cost, %cl)
{
	if (!isObject("_" @ getWord(%destination, 0)) || %cl.lastClickedBusStop + 1 > $Sim::Time)
	{
		return;
	}
	%target = "_" @ getWord(%destination, 0);

	%cl.busCost = %cost;
	%cl.destination = %target;
	%cl.destinationName = getWords(%destination, 1, 20);
	%cl.lastClickedBusStop = $Sim::Time;
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