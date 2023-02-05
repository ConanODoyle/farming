function spawnHarvester()
{
	if(isObject(Harvester))
	{
		Harvester.delete();
	}
	
	%health = $Harvester::Armor::BaseHealth + $Harvester::Armor::ExtraHealthPerFighter * HarvesterFightSet.getCount();
	
	new AIPlayer(Harvester)
	{
		dataBlock = HarvesterArmor;
		position = "0 0 0";
		name = "The Harvester";
		isBot = true;
		
		// Max health is scaled by the number of players in the fight.
		maxHealth = %health;
		resistance = %health / HarvesterArmor.maxDamage;
		
		// Fight values.
		phase = 1;
		isBoss = true;
		
		// Dialogue flags.
		saidFinalMessage = false;
	};
	
	// Harvester.setMoveSpeed(0.7);
	Harvester.setMoveTolerance(3);
	
	HarvesterArmor.applyAvatar(Harvester);
	Harvester.mountImage(HarvesterFoldedBladeImage, 1);
	Harvester.mountImage(HarvesterBeamRifleBackImage, 2);
	
	Harvester.harvesterTeleport(0);
	Harvester.harvesterLoop();
}

/// @param	this	ai player
/// @param	level	number
function AIPlayer::harvesterTeleport(%this, %level)
{
	if(%this.getDamagePercent() < 1.0)
	{
		%searchAll = false;
		%name = "_teleportRing" @ %level;
		
		if(%level $= "")
		{
			// If the level argument was left blank, search all rings for the nearest teleport location.
			%searchAll = true;
		}

		%lastDistance = inf;
		
		for(%i = 0; %i < 5; %i++)
		{
			if(%searchAll)
			{
				// If searching all rings, clobber the name variable with each loop.
				%name = "_teleportRing" @ %i;
			}
			
			for(%j = 0; %j < BrickGroup_888888.NTObjectCount[%name]; %j++)
			{
				%current = BrickGroup_888888.NTObject[%name, %j];
				
				if(!isObject(%this.target))
				{
					%closest = %current;
					break;
				}
				
				%currentDistance = vectorDist(%current.position, %this.target.position);
				
				if(%currentDistance < %lastDistance)
				{
					%closest = %current;
					%lastDistance = %currentDistance;
				}
			}
			
			if(!%searchAll)
			{
				// If /not/ searching all rings, exit after only running once.
				break;
			}
		}
		
		if(!isObject(%closest))
		{
			return;
		}
		
		%preEffect = new Projectile()
		{
			// TODO: Bespoke teleport particles.
			dataBlock = HarvesterBladeEquipProjectile;
			initialVelocity = %this.getForwardVector();
			initialPosition = %this.getHackPosition();
			sourceObject = %this;
			sourceSlot = %slot;
			client = %this.client;
		};

		if(isObject(%preEffect))
		{
			MissionCleanup.add(%preEffect);
			%preEffect.explode();
		}
		
		%this.setTransform(%closest.position SPC getWords(%this.getTransform(), 3, 6));
		%this.setVelocity("0.0 0.0 0.0");
		
		%postEffect = new Projectile()
		{
			// TODO: Bespoke teleport particles.
			dataBlock = HarvesterBladeEquipProjectile;
			initialVelocity = %this.getForwardVector();
			initialPosition = %this.getHackPosition();
			sourceObject = %this;
			sourceSlot = %slot;
			client = %this.client;
		};

		if(isObject(%postEffect))
		{
			MissionCleanup.add(%postEffect);
			%postEffect.explode();
		}
	}
}

/// @param	this	ai player
function AIPlayer::harvesterFindTarget(%this)
{
	%highestWeight = -inf;
	
	echo("::harvesterFindTarget(" @ %this @ "):");
	
	for(%i = 0; %i < HarvesterFightSet.getCount(); %i++)
	{	
		%player = HarvesterFightSet.getObject(%i);
		
		if(%player.getID() == %this.getID() || %player.getState() $= "Dead")
		{
			continue;
		}
		
		%weight = 0.0;
		
		// Negatively scale target weighting by distance from target.
		%distance = vectorDist(%this.position, %player.position);
		%weight -= %distance;
		
		// Positively scale target weighting by damage received from target, scaled by distance.
		%weight += getMax(0, %this.damageReceived[%player] - (%this.damageReceived[%player] / $Harvester::AI::ArenaRadius) * %distance); // Lose up to 100% of your damage contribuion at edge.
		
		echo("  %weight for" SPC %player SPC "is" SPC %weight);

		if(%weight > %highestWeight)
		{
			%highestWeight = %weight;
			%highestTarget = %player;
		}
	}
	
	if(!isObject(%highestTarget))
	{
		return false;
	}
	
	echo("  %highestTarget is" SPC %highestTarget);
	
	%this.target = %highestTarget;
	return %this.target;
}

/// @param	this	ai player
/// @param	phase	string
/// @param	phase	string
function AIPlayer::harvesterChat(%this, %name, %message)
{
	harvesterMessage("<font:palatino linotype:32><spush><shadowcolor:000000AA><shadow:3:3>" @ %name @ "<spop>\c6:" SPC %message);
}

/// @param	this	ai player
function AIPlayer::harvesterBreakStagger(%this)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		switch(%this.phase)
		{			
			case 2:
				%message = "Your efforts will bear no fruit.";
				
			case 3:
				%message = "I will not wither away!";
				
			case 4:
				%message = "Your blood will nourish the soil!";
				
			default:
				%message = "...";
		}
		
		%this.harvesterChat("The Harvester", %message);
		
		%effect = new Projectile()
		{
			// TODO: Reuse teleport particles.
			dataBlock = HarvesterBladeEquipProjectile;
			initialVelocity = %this.getForwardVector();
			initialPosition = %this.getHackPosition();
			sourceObject = %this;
			sourceSlot = %slot;
			client = %this.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
		
		%this.playThread(0, "stunDone");
		%this.schedule(500, playThread, 0, "sweepDone");
		%this.schedule(1000, playThread, 0, "root");
		
		%this.playAudio(1, HarvesterYellSound1);

		%this.thinkLoop = %this.schedule(1000, harvesterLoop);
	}
}

/// @param	this	ai player
/// @param	time	number
function AIPlayer::harvesterStagger(%this, %time)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		%this.stop();
		%this.clearAim();
		%this.unMountImage(0);
		
		%this.playThread(0, "stun");
		
		%this.playAudio(1, HarvesterDamageSound);
		serverPlay3d(HarvesterStaggerSound, %this.position);
		
		%this.thinkLoop = %this.schedule(%time, harvesterBreakStagger);
	}
}

/// @param	this	ai player
/// @param	phase	number
function AIPlayer::harvesterSetPhase(%this, %phase)
{
	%this.phase = %phase;
	
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		%this.harvesterTeleport(0); // Ring 0 (center of the arena).
		
		switch(%this.phase)
		{
			case 2:
				%this.harvesterStagger(2200);
				
				setHarvesterFightMusic(HarvesterChange1Music);
				schedule(5052, 0, setHarvesterFightMusic, HarvesterPhase2Music);
				
			case 3:
				%this.harvesterStagger(2000);
				
				setHarvesterFightMusic(HarvesterChange2Music);
				schedule(10105, 0, setHarvesterFightMusic, HarvesterPhase3Music);
			
			case 4:
				%this.harvesterStagger(3700);
				
				setHarvesterFightMusic(HarvesterChange3Music);
				schedule(7579, 0, setHarvesterFightMusic, HarvesterPhase4Music);
				
			default:
				%this.harvesterStagger(2200);
		}
	}
}

/// @param	this	ai player
function AIPlayer::harvesterTimingTest(%this)
{
	announce("::harvesterTimingTest()");
	%this.timingTest = getSimTime();
}

/// @param	this	ai player
function AIPlayer::harvesterBladeAttack(%this)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		%this.stop();
		// %this.clearAim();
		
		%minimumTime = 700 + 96;
		%equipTime = 0;
		
		if(!%this.isImageMounted(HarvesterBladeImage))
		{
			%this.mountImage(HarvesterBladeImage, 0);
			%equipTime = 400; // +400 if not equipped.
		}
		
		switch(%this.phase)
		{
			case 1:
				%chargeTime = 1000;
				
			case 2:
				%chargeTime = 500;
				
				if(getRandom() > 0.67)
				{
					%this.harvesterTeleport();
				}
				
			case 3:
				%chargeTime = 250;
				
				if(getRandom() > 0.5)
				{
					%this.harvesterTeleport();
				}
				
			case 4:
				%chargeTime = 96;
				
				%this.harvesterTeleport();
				
			default:
				%chargeTime = 1000;
		}
		
		%this.schedule(0, setImageTrigger, 0, true);
		%this.schedule(%equipTime + %chargeTime, setImageTrigger, 0, false);
		
		%time = %equipTime + %chargeTime + %minimumTime;
		
		%this.thinkLoop = %this.schedule(%time, harvesterLoop);
		%this.lastBladeAttackTime = getSimTime() + %time;
	}
}

/// @param	this	ai player
function AIPlayer::harvesterBeamAttack(%this)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		%this.stop();
		
		%minimumTime = 3200 + 96;
		%equipTime = 0;
		
		%secondShot = false;
		%secondShotTime = 0;
		
		if(!%this.isImageMounted(HarvesterBeamRifleImage))
		{
			%this.mountImage(HarvesterBeamRifleImage, 0);
			%equipTime = 150; // +150 if not equipped.
		}
		
		switch(%this.phase)
		{
			case 1:
				%triggerReleaseTime = 2100;
				
				%this.harvesterTeleport(0);

			case 2:
				%triggerReleaseTime = 2100;
				
				%this.harvesterTeleport(0);

			case 3:
				%triggerReleaseTime = 3200;
				%secondShot = true;
				
				%this.harvesterTeleport(0);

			case 4:
				%triggerReleaseTime = 2100;
				
				%this.harvesterTeleport(getRandom(0, 4));
				
			default:
				%triggerReleaseTime = 2100;
				
				%this.harvesterTeleport(0);
		}
		
		if(%secondShot)
		{
			%secondShotTime = 1100; // +1100 if firing twice.
		}
		
		%this.schedule(0, setImageTrigger, 0, true);
		%this.schedule(%equipTime + %triggerReleaseTime, setImageTrigger, 0, false);
		
		%time = %equipTime + %minimumTime + %secondShotTime;
		
		%this.thinkLoop = %this.schedule(%time, harvesterLoop);
		%this.lastBeamAttackTime = getSimTime() + %time;
	}
}

/// @param	this	ai player
function AIPlayer::harvesterClusterBombAttack(%this)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		%this.stop();
		
		%minimumTime = 700 + 96;
		%equipTime = 0;
		
		if(!%this.isImageMounted(HarvesterClusterBombImage) || !%this.isImageMounted(HarvesterBigClusterBombImage))
		{
			switch(%this.phase)
			{
				case 1 or 2:
					%this.mountImage(HarvesterClusterBombImage, 0);
				
				case 3 or 4:
					%this.mountImage(HarvesterBigClusterBombImage, 0);
					
				default:
					%this.mountImage(HarvesterClusterBombImage, 0);
			}
			
			%equipTime = 150; // +150 if not equipped.
		}
		
		switch(%this.phase)
		{
			case 1:
				%chargeTime = 1000;
				
			case 2:
				%chargeTime = 500;
				
			case 3:
				%chargeTime = 250;
				
			case 4:
				%chargeTime = 96;
				
			default:
				%chargeTime = 1000;
		}
		
		%this.schedule(0, setImageTrigger, 0, true);
		%this.schedule(%equipTime + %chargeTime, setImageTrigger, 0, false);
		
		%time = %equipTime + %chargeTime + %minimumTime;
		
		%this.thinkLoop = %this.schedule(%time, harvesterLoop);
		%this.lastClusterBombAttackTime = getSimTime() + %time;
	}
}

/// @param	this	ai player
function AIPlayer::harvesterSpikeAttack(%this)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		%this.stop();
		
		%minimumTime = 2100 + 96;
		%equipTime = 0;
		
		%secondShot = false;
		%secondShotTime = 0;
		
		if(!%this.isImageMounted(HarvesterSpikeImage))
		{
			%this.mountImage(HarvesterSpikeImage, 0);
			%equipTime = 150; // +150 if not equipped.
		}
		
		switch(%this.phase)
		{
			case 1:
				%triggerReleaseTime = 1250;

			case 2:
				%triggerReleaseTime = 1250;

			case 3:
				%triggerReleaseTime = 2250;
				%secondShot = true;
				
				if(getRandom() > 0.5)
				{
					%this.harvesterTeleport(getRandom(1, 2));
				}

			case 4:
				%triggerReleaseTime = 1250;
				
				if(getRandom() > 0.5)
				{
					%this.harvesterTeleport();
				}
				
			default:
				%triggerReleaseTime = 1250;
		}
		
		if(%secondShot)
		{
			%secondShotTime = 1000; // +1100 if firing twice.
		}
		
		%this.schedule(0, setImageTrigger, 0, true);
		%this.schedule(%equipTime + %triggerReleaseTime, setImageTrigger, 0, false);
		
		%time = %equipTime + %minimumTime + %secondShotTime;
		
		%this.thinkLoop = %this.schedule(%time, harvesterLoop);
		%this.lastSpikeAttackTime = getSimTime() + %time;
	}
}

/// @param	this	ai player
function AIPlayer::harvesterSelectAttack(%this)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{	
		%distance = vectorDist(%this.position, %this.target.position);
		
		switch(%this.phase)
		{
			case 1:
				if(%distance < 7)
				{
					%this.harvesterBladeAttack();
				}
				else if(%this.lastClusterBombAttackTime + 3000 < getSimTime())
				{
					%this.harvesterClusterBombAttack();
				}
				else
				{
					// Reached the end of move select due to no moves being available, go back to start.
					%this.thinkLoop = %this.schedule(1000, harvesterLoop);
				}
			
			case 2:
				if(%distance < 7)
				{
					%this.harvesterBladeAttack();
				}
				else if(%distance < 14 && %this.lastSpikeAttackTime + 3000 < getSimTime())
				{
					%this.harvesterSpikeAttack();
				}
				else
				{
					if(getRandom() > 0.5 && %this.lastBeamAttackTime + 5000 < getSimTime())
					{
						%this.harvesterBeamAttack();
					}
					else if(%this.lastClusterBombAttackTime + 3000 < getSimTime())
					{
						%this.harvesterClusterBombAttack();
					}
					else
					{
						// Reached the end of move select due to no moves being available, go back to start.
						%this.thinkLoop = %this.schedule(1000, harvesterLoop);
					}
				}
			
			case 3:
				if(%distance < 7)
				{
					%this.harvesterBladeAttack();
				}
				else if(%distance < 14 && %this.lastSpikeAttackTime + 3000 < getSimTime())
				{
					%this.harvesterSpikeAttack();
				}
				else
				{
					if(getRandom() > 0.5 && %this.lastBeamAttackTime + 3000 < getSimTime())
					{
						%this.harvesterBeamAttack();
					}
					else if(%this.lastClusterBombAttackTime + 3000 < getSimTime())
					{
						%this.harvesterClusterBombAttack();
					}
					else
					{
						// Reached the end of move select due to no moves being available, go back to start.
						%this.thinkLoop = %this.schedule(1000, harvesterLoop);
					}
				}
				
			case 4:
				// Just go fuckin' /wild/.
				%move = getRandom(0, 3);
				switch(%move)
				{
					case 0:
						%this.harvesterBladeAttack();
						
					case 1:
						%this.harvesterBeamAttack();
						
					case 2:
						%this.harvesterClusterBombAttack();
						
					case 3:
						%this.harvesterSpikeAttack();
						
					default:
						%this.harvesterBladeAttack();
				}
		}
	}
}

/// @param	this	ai player
function AIPlayer::harvesterLoop(%this)
{
	if(isEventPending(%this.thinkLoop))
	{
		cancel(%this.thinkLoop);
	}
	
	if(%this.getDamagePercent() < 1.0)
	{
		if(%this.harvesterFindTarget())
		{
			%this.setMoveObject(%this.target);
			%this.setAimObject(%this.target);
		
			switch(%this.phase)
			{
				case 1:
					%time = 2000;
					
				case 2:
					%time = 1000;
				
				case 3:
					%time = 500;
					
				case 4:
					%time = 100;
					
				default:
					%time = 1000;
			}
			
			%this.thinkLoop = %this.schedule(%time, harvesterSelectAttack);
		}
		else
		{
			%this.stop();
			
			%this.thinkLoop = %this.schedule(3000, harvesterLoop);
		}
	}
}

/// @param	this		playertype
/// @param	player		player
/// @param	source		damage source
/// @param	position	3-element position
/// @param	damage		number
/// @param	type		number
function HarvesterArmor::damage(%this, %player, %source, %position, %damage, %type)
{
	%sourceObject = %source.sourceObject;

	if(isObject(%sourceObject))
	{
		%player.damageReceived[%sourceObject] += %damage;
	}
	
	if(%player.resistance !$= "" && %player.resistance > 0)
	{
		%damage /= %player.resistance;
	}
	
	talk("::damage - after resistance:" SPC %damage);
	talk("::damage - damageReceived[" @ %sourceObject @ "]:" SPC %player.damageReceived[%sourceObject]);
	
	if(%player.isBoss)
	{
		if(%player.getDamagePercent() > 0.95 && !%player.saidFinalMessage)
		{
			%player.saidFinalMessage = true;
			%player.harvesterChat("The Harvester", "Shorn one by one and cast to the winds, I gave to them my all...");
		}
		else if(%player.getDamagePercent() > 0.78 && %player.phase < 4)
		{
			%player.harvesterSetPhase(4);
		}
		else if(%player.getDamagePercent() > 0.35 && %player.phase < 3)
		{
			%player.harvesterSetPhase(3);
		}
		else if(%player.getDamagePercent() > 0.15 && %player.phase < 2)
		{
			%player.harvesterSetPhase(2);
		}		
	}

	return Parent::damage(%this, %player, %source, %position, %damage, %type);
}

/// @param	this	playertype
/// @param	player	player
/// @param	state	string
function HarvesterArmor::onDisabled(%this, %player, %state)
{
	if(%player.isBoss)
	{
	}
	
	return Parent::onDisabled(%this, %player, %state);
}