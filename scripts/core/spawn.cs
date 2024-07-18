package FarmingSpawn
{
	function GameConnection::spawnPlayer(%cl) 
	{
		%ret = parent::spawnPlayer(%cl);
		%pl = %cl.player;

		if (!isObject(%cl.playerDatablock))
		{
			%pl.setDatablock(PlayerNoJet);
		}
		else if (isObject(%cl.playerDatablock))
		{
			%pl.setDatablock(%cl.playerDatablock);
		}

		if (isObject(_globalSpawn) && !isObject(%cl.checkpointBrick))
		{
			%cl.lastRespawned = $Sim::Time;
			%pl.setTransform(getRandomGlobalSpawn().getTransform());
		}

		if (%cl.grantCheck)
		{
			return %ret;
		}
		
		%cl.grantCheck = 1;
		if ($Pref::Farming::ScoreGrant[%cl.bl_id] <= 0 && !%cl.checkMoney($startingAmount))
		{
			$Pref::Farming::ScoreGrant[%cl.bl_id] = 1 TAB %cl.name;
			messageClient(%cl, '', "\c6You received \c2$" @ $startingAmount @ "\c6 to start off with!");
			schedule(3000, %cl, serverCmdTutorial, %cl, "");
			%cl.schedule(1000, addMoney, $startingAmount);

			if (%cl.isBetaTester)
			{
				messageClient(%cl, '', "\c2You received a bonus of $" @ $betaBonus @ " for being a beta tester!");
				%cl.schedule(1000, addMoney, $betaBonus);
			}
			exportServerPrefs(1);
		}
		else if (%cl.checkMoney($startingAmount))
		{
			$Pref::Farming::ScoreGrant[%cl.bl_id] = 1 TAB %cl.name;
			messageClient(%cl, '', "\c6You already have more than \c2$" @ $startingAmount @ "!");
			%cl.addMoney($startingAmount);
			exportServerPrefs(1);
		}

		return %ret;
	}

	function GameConnection::applyPersistence(%client, %gotPlayer, %gotCamera)
	{
		%ret = parent::applyPersistence(%client, %gotPlayer, %gotCamera);

		if (%client.brickgroup.delayedScoreAdjustment != 0)
		{
			%client.addMoney(%client.brickgroup.delayedScoreAdjustment);
			%amt = %client.brickgroup.delayedScoreAdjustment;
			%client.brickgroup.delayedScoreAdjustment = 0;

			%word = %amt > 0 ? "refunded" : "charged";
			%color = %amt > 0 ? "\c2" : "\c0";

			messageClient(%client, '', "\c6You were " @ %word @ %color @ " $" @ %amt @ "\c6 for build/vehicle changes while you were offline.");
		}

		return %ret;
	}

	function serverCmdSuicide(%cl)
	{
		if ($Sim::Time - %cl.lastRespawned > 0.2  && $Sim::Time - %cl.lastRespawned < 2)
		{
			%cl.instantRespawn();
			return;
		}
		parent::serverCmdSuicide(%cl);
	}
};
activatePackage(FarmingSpawn);

RegisterPersistenceVar("playerDatablock", false, "");

function getRandomGlobalSpawn()
{
	%count = 0;
	while (isObject(_globalSpawn) && %count < 100)
	{
		%b = _globalSpawn.getID();
		%brick[%count] = %b;
		%b.setName("");
		%count++;
	}

	if (%count >= 100)
	{
		talk("> 100 global spawns!");
	}

	for (%i = 0; %i < %count; %i++)
	{
		%brick[%i].setName("_globalSpawn");
	}

	return %brick[getRandom(%count - 1)];
}

function timeLoop()
{
	cancel($masterTimeLoopSchedule);

	if (!isObject(MissionCleanup))
	{
		return;
	}

	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%cl.timePlayed += 1;
		$Pref::Server::Farming::TimePlayed_[%cl.bl_id] = getTimeString(mFloor(%cl.timePlayed / 6));
	}

	$masterTimeLoopSchedule = schedule(10000, 0, timeLoop);
}

RegisterPersistenceVar("timePlayed", false, "");

schedule(1000, 0, timeLoop);

function serverCmdTimePlayed(%cl)
{
	%time = %cl.timePlayed / 6;
	%hr = mFloor(%time / 60);
	%min = %time % 60;

	%hrPlural = %hr > 1 ? "s" : "";
	%minPlural = %min > 1 ? "s" : "";
	messageClient(%cl, '', "\c6You've played for \c3" @ %hr @ "\c6 hour" @ %hrPlural @ " and \c3" @ %min @ " \c6minute" @ %minPlural @ "\c6!");
}