//override ::hLoop to increase performance + allow management of bot spawns more easily
//assumptions:
//	bots not mounted
//	bots not wandering/moving around
//	bots not looking for enemies

// BlockheadHoleBot.hSpawnClose = 1;
// BlockheadHoleBot.hSpawnCRange = 100;
BlockheadHoleBot.hWander = 0;
BlockheadHoleBot.hStrafe = 0;
BlockheadHoleBot.hGridWander = 0;


function minDistanceToPlayer(%obj)
{
	if (!isObject(%obj))
	{
		return -1;
	}

	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%player = ClientGroup.getObject(%i).player;
		if (isObject(%player))
		{
			//get the distance from the player in brick units
			%dist = VectorDist(%obj.position, %player.position);
			if (%dist < %min || %min $= "")
			{
				%min = %dist SPC %player;
			}
		}
	}
	return %min;
}

package DespawnDistantBots
{
	function AIPlayer::hLoop(%obj)
	{
		if (%obj.spawnBrick.hSpawnClose)
		{
			%spawnBrick = %obj.spawnBrick;
			%dist = minDistanceToPlayer(%spawnBrick);
			if (%dist > %spawnBrick.hSpawnDistance)
			{
				%spawnBrick.unSpawnHoleBot();
				cancel(%spawnBrick.hSpawnDetectSchedule);
				%spawnBrick.hSpawnDetectSchedule = %spawnBrick.scheduleNoQuota(5000, spawnHoleBot);
				return;
			}
		}
		return parent::hLoop(%obj);
	}

	function fxDTSBrick::spawnHoleBot(%brick)
	{
		if (%brick.hSpawnClose && !distanceSpawnCheck(%brick))
		{
			cancel(%brick.hSpawnDetectSchedule);
			%brick.hSpawnDetectSchedule = %brick.scheduleNoQuota(5000, spawnHoleBot);
			return 0;
		}
		return parent::spawnHoleBot(%brick);
	}
};
activatePackage(DespawnDistantBots);

function distanceSpawnCheck(%brick)
{	
	if (%brick.hSpawnClose)
	{
		%dist = minDistanceToPlayer(%brick);
		if (%dist < %brick.hSpawnDistance)
		{
			return 1;
		}
	}
	return 0;
}

function fxDTSBrick::setHSpawnClose(%obj, %bool, %distance)
{
	%obj.hSpawnClose = %bool;
	%obj.hSpawnDistance = %distance;
}
registerOutputEvent("fxDTSBrick", "setHSpawnClose", "bool 1" TAB "int 0 10000 100");