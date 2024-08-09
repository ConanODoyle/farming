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

if (!isObject(BotHoleSimSet))
{
	new SimSet(BotHoleSimSet);
}

function botHoleGhostLoop(%idx)
{
	cancel($BotHoleGhostLoopSchedule);
	%idx = %idx + 0;

	if (BotHoleSimSet.getCount() <= 0)
	{
		return;
	}

	//ghost/unghost up to 5 bots each tick
	for (%i = 0; %i < 5; %i++)
	{
		if (%idx >= BotHoleSimSet.getCount())
		{
			break;
		}

		%bot = BotHoleSimSet.getObject(%idx);
		for (%j = 0; %j < ClientGroup.getCount(); %j++)
		{
			%cl = ClientGroup.getObject(%j);
			%pl = %cl.player;

			if (%bot.spawnBrick.getName() $= "_bigbuyer")
			{
				%bot.scopeToClient(%cl);
			}
			else if (!isObject(%pl) || vectorDist(%pl.position, %bot.position) > 150)
			{
				%bot.clearScopeToClient(%cl);
			}
			else
			{
				%bot.scopeToClient(%cl);
			}
		}

		%idx++;
	}

	if (%idx >= BotHoleSimSet.getCount())
	{
		%idx = 0;
	}

	$BotHoleGhostLoopSchedule = schedule(33, BotHoleSimSet, botHoleGhostLoop, %idx);
}

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
		if (vectorDist(%obj.spawnBrick.position, %obj.position) > 1)
		{
			if (isObject(%obj.getObjectMount()))
			{
				%obj.dismount();
			}
			%obj.setTransform(%obj.spawnBrick.getTransform());
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
		%ret = parent::spawnHoleBot(%brick);

		if (isObject(%bot = %brick.hBot) && !BotHoleSimSet.isMember(%brick.hBot))
		{
			BotHoleSimSet.add(%bot);
			%bot.setNetFlag(6, true);
			if (GhostAlwaysSet.isMember(%bot))
				%bot.clearScopeAlways();

			botHoleGhostLoop();
		}
		return %ret;
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