exec("./busStop.cs");


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