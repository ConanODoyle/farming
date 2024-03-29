if(!isObject(HarvesterDeathSet))
{
	new SimSet(HarvesterDeathSet);
}

function Player::addPlayerToHF(%pl, %cl)
{
	if (!isObject(%cl))
	{
		return;
	}
	if (isObject(Harvester) || isObject(CutsceneHarvester))
	{
		%cl.centerprint("A fight is in progress. Please wait.", 5);
		return;
	}

	HarvesterFightSet.add(%pl);
	%pl.setTransform(_harvesterPlayerSpawn.getTransform());
	%pl.setWhiteout(1);

	//update antechamber timer	
	if (HarvesterFightSet.getCount() <= 1)
	{
		HarvesterFightSet.countdown = 60;
	}
	else
	{
		HarvesterFightSet.countdown = getMin(HarvesterFightSet.countdown + 30, 60);
	}

	enableBossWalls();

	startAntechamberCountdown();
}
registerOutputEvent("Player", "addPlayerToHF", "", 1);

function startAntechamberCountdown()
{
	cancel($AntechamberCountdownSchedule);

	if (HarvesterFightSet.countdown < 0)
	{
		onAntechamberTimerFinish();
		return;
	}
	for (%i = 0; %i < HarvesterFightSet.getCount(); %i++)
	{
		%cl = HarvesterFightSet.getObject(%i).client;
		if (isObject(%cl))
		{
			%cl.centerprint("<font:Palatino Linotype:32>\n\n\c7[ " @ HarvesterFightSet.countdown @ " ]", 1.1);
		}
	}
	HarvesterFightSet.countdown--;

	$AntechamberCountdownSchedule = schedule(1000, HarvesterFightSet, startAntechamberCountdown);
}

function onAntechamberTimerFinish()
{
	//start fight, start loop checks
	//reset/set any variables needed
	// HarvesterFightSet.countdown = 60;
	$lastLoopCheckStatus = "";
	bossfightStateLoopCheck();
	HarvesterFightSet.startCount = HarvesterFightSet.getCount();
	harvesterIntroCutscene(0);
}

function setRingEmitter(%emitter)
{
	%bg = BrickGroup_888888;
	for (%i = 0; %i < %bg.NTObjectCount["_harvesterFlameWallBase"]; %i++)
	{
		%b = %bg.NTObject["_harvesterFlameWallBase", %i];
		%b.setEmitter(%emitter);
	}
}

function setRingColliding(%collide)
{
	%bg = BrickGroup_888888;
	for (%i = 0; %i < %bg.NTObjectCount["_harvesterFlameWallBase"]; %i++)
	{
		%b = %bg.NTObject["_harvesterFlameWallBase", %i];
		%b.setColliding(%collide);
	}
	for (%i = 0; %i < %bg.NTObjectCount["_harvesterFlameWall"]; %i++)
	{
		%b = %bg.NTObject["_harvesterFlameWall", %i];
		%b.setColliding(%collide);
	}
}

function setSpectatorPlatformColliding(%collide)
{
	%bg = BrickGroup_888888;
	for(%i = 0; %i < %bg.NTObjectCount["_harvesterSpectatorPlatform"]; %i++)
	{
		%b = %bg.NTObject["_harvesterSpectatorPlatform", %i];
		%b.setColliding(%collide);
	}
}

function enableBossWalls()
{
	setRingColliding(1);
	setSpectatorPlatformColliding(1);
	setRingEmitter("BurnEmitterA");
}

function disableBossWalls()
{
	setRingColliding(0);
	setSpectatorPlatformColliding(0);
	setRingEmitter(0);
}

//checks status of bossfight players, clears and resets boss if nobody left
function bossfightStateLoopCheck()
{
	cancel($BossfightStateCheckSchedule);

	if (HarvesterFightSet.getCount() == 0 && HarvesterDeathSet.getCount() == 0)
	{
		onBossfightComplete("");
		return;
	}

	if ($bossDead && HarvesterFightSet.getCount() > 0)
	{
		//winning isnt handled here - check HarvesterOutroCutscene packaged function
	}
	else if (HarvesterDeathSet.getCount() > 0 && HarvesterFightSet.getCount() == 0)
	{
		if ($lastLoopCheckStatus !$= "Lose")
		{
			$loopCheckTicks = 0;
		}
		$lastLoopCheckStatus = "Lose";
		$loopCheckTicks++;
		if ($loopCheckTicks >= 5)
		{
			onBossfightComplete("Lose");
			return;
		}
	}

	$BossfightStateCheckSchedule = schedule(1000, 0, bossfightStateLoopCheck);
}

function onBossfightComplete(%status)
{
	cancel($BossfightStateCheckSchedule);

	switch$ (%status)
	{
		case "Win":
			for (%i = 0; %i < HarvesterFightSet.getCount(); %i++)
			{
				%cl = HarvesterFightSet.getObject(%i).client;
				%cl.checkpointBrick = "";
				%cl.checkPointBrickPos = "";
			}

			for (%i = 0; %i < HarvesterDeathSet.getCount(); %i++)
			{
				%cl = HarvesterDeathSet.getObject(%i);
				%cl.checkpointBrick = "";
				%cl.checkPointBrickPos = "";
			}
			disableSecretRamp();
		case "Lose":
			for (%i = 0; %i < HarvesterDeathSet.getCount(); %i++)
			{
				%cl = HarvesterDeathSet.getObject(%i);
				%cl.instantRespawn();
				%cl.setDamageFlash(1);
			}
			deleteHarvester();
	}

	disableBossWalls();

	HarvesterDeathSet.clear();
	HarvesterFightSet.clear();
}

function handleBossfightPlayerDeath(%cl)
{
	if (!HarvesterDeathSet.isMember(%cl))
	{
		HarvesterDeathSet.add(%cl);
	}
	schedule(10, %cl, spawnOutsideArena, %cl);
}

function spawnOutsideArena(%cl)
{
	%cl.instantRespawn();
	%cl.player.position = _harvesterDeathZone.position;
	%cl.player.setDamageFlash(1);
	commandToClient(%cl, 'MessageBoxOK', "Harvested", "<font:Palatino Linotype:30>- You have been harvested -\n<font:Palatino Linotype:18>If the boss is defeated, you will get rewards");
}

function onBossFightWin()
{
	for (%i = 0; %i < HarvesterDeathSet.getCount(); %i++)
	{
		%cl = HarvesterDeathSet.getObject(%i);
		if (!isObject(%cl.player))
		{
			%cl.instantRespawn();
		}
		%pl = %cl.player;
		%pl.setTransform(_harvesterPlayerSpawn.getTransform());
	}
}

function removeBossCombatItems(%pl)
{
	for (%i = 0; %i < %pl.dataBlock.maxTools; %i++)
	{
		if (isObject(%pl.tool[%i]) && (%pl.tool[%i].getName() $= "MasterKeyItem"
				|| %pl.tool[%i].getName() $= "lockGunItem")) //TODO: add healing item as well
		{
			%pl.farmingRemoveItem(%i);
		}
	}
}

package HarvesterEncounter
{
	function GameConnection::onDeath(%cl, %source, %killer, %type, %location)
	{
		if(isObject(%cl.player) && (HarvesterFightSet.isMember(%cl.player) || HarvesterDeathSet.isMember(%cl)))
		{
			handleBossfightPlayerDeath(%cl);
		}
		return Parent::onDeath(%cl, %source, %killer, %type, %location);
	}

	function harvesterOutroCutscene(%stage)
	{
		parent::harvesterOutroCutscene(%stage);
		if (%stage == 6)
		{
			onBossfightComplete("Win");
		}
	}

	function serverCmdHome(%cl)
	{
		%pl = %cl.player;

		if (HarvesterFightSet.isMember(%pl) || HarvesterDeathSet.isMember(%cl))
		{
			commandToClient(%cl, 'MessageBoxOK', "Restricted", "You are currently in the Harvester boss fight. Please wait until the fight is complete.");
			return;
		}

		removeBossCombatItems(%pl);
		parent::serverCmdHome(%cl);
	}
};
activatePackage(HarvesterEncounter);