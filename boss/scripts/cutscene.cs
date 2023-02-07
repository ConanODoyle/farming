function harvesterIntroCutscene(%stage)
{
	switch(%stage)
	{
		case 0:
			%nextStageTime = 150;
			
			new AIPlayer(CutsceneHarvester)
			{
				dataBlock = HarvesterArmor;
				position = _harvesterIntro.position;
				name = "The Harvester";
				invulnerable = true;
				isBot = true;
			};
				
			if(isObject(CutsceneHarvester))
			{
				MissionCleanup.add(CutsceneHarvester);
				
				HarvesterArmor.applyAvatar(CutsceneHarvester);
				CutsceneHarvester.mountImage(HarvesterFoldedBladeImage, 1);
				CutsceneHarvester.mountImage(HarvesterBeamRifleBackImage, 2);
				
				%effect = new Projectile()
				{
					dataBlock = HarvesterTeleportProjectile;
					initialVelocity = CutsceneHarvester.getForwardVector();
					initialPosition = CutsceneHarvester.getHackPosition();
					sourceObject = CutsceneHarvester;
					client = CutsceneHarvester.client;
				};

				if(isObject(%effect))
				{
					MissionCleanup.add(%effect);
					%effect.explode();
				}
			}
			
			setHarvesterFightCamera(_harvesterCamera0.position, eulerToQuat("40.0 0.0 -150.0"));
			
			setHarvesterFightMusic(HarvesterIntroMusic);
			schedule(20210, 0, setHarvesterFightMusic, HarvesterPhase1Music);
			
		case 1:
			%nextStageTime = 1900;
			
			CutsceneHarvester.setVelocity("0.0 6.0 16.0");
			CutsceneHarvester.playThread(0, "slamAttack");
			
			serverPlay3d(HarvesterLeapSound, CutsceneHarvester.position);
			
			CutsceneHarvester.harvesterChat("???", "Who dares trespass? Why have you come?");
			
			CutsceneHarvester.playAudio(2, HarvesterIntroVoiceSound1);
			CutsceneHarvester.schedule(2200, playAudio, 2, HarvesterIntroVoiceSound2);
			
		case 2:
			%nextStageTime = 2400;
			
			// Land sound (synced with landing from jump).
			serverPlay3d(HarvesterLandSound, CutsceneHarvester.position);
			
			setHarvesterFightCamera(_harvesterCamera1.position, eulerToQuat("-60.0 0.0 180.0"));
			
		case 3:
			%nextStageTime = 2200;
			
			CutsceneHarvester.mountImage(HarvesterBladeImage, 0);
			CutsceneHarvester.setHeadAngle($piOver2);
			CutsceneHarvester.playThread(0, "slamDone");
			
			setHarvesterFightCamera(_harvesterCamera2.position, eulerToQuat("15.0 0.0 115.0"));
			
			CutsceneHarvester.harvesterChat("???", "No matter...");
			CutsceneHarvester.playAudio(2, HarvesterIntroVoiceSound3);
			
		case 4:
			%nextStageTime = 3300;
			
			CutsceneHarvester.setTransform(CutsceneHarvester.position SPC "-1.0 0.0 0.0 3.14159");
			CutsceneHarvester.setHeadAngle(0.0);
			CutsceneHarvester.playThread(0, "sweepReady");
			
			setHarvesterFightCamera(_harvesterCamera3.position, eulerToQuat("30.0 0.0 -37.5"));
			
			CutsceneHarvester.harvesterChat("???", "The tallest stalk is cut down first, farmer.");
			CutsceneHarvester.playAudio(2, HarvesterIntroVoiceSound4);
			
		case 5:
			%nextStageTime = 250;
			
			CutsceneHarvester.playThread(0, "stunDone");
			
			setHarvesterFightCamera(_harvesterCamera4.position, eulerToQuat("0.0 0.0 0.0"));
			
		case 6:
			%nextStageTime = 3000;
			
			%effect = new Projectile()
			{
				dataBlock = HarvesterTeleportProjectile;
				initialVelocity = CutsceneHarvester.getForwardVector();
				initialPosition = CutsceneHarvester.getHackPosition();
				sourceObject = CutsceneHarvester;
				client = CutsceneHarvester.client;
			};

			if(isObject(%effect))
			{
				MissionCleanup.add(%effect);
				%effect.explode();
			}
			
			CutsceneHarvester.mountImage(HarvesterVisorLightImage, 3);
			
			serverPlay3d(HarvesterBeamRifleChargeSound, CutsceneHarvester.position);
			
			CutsceneHarvester.harvesterChat("The Harvester", "for I am the Harvester.");
			CutsceneHarvester.playAudio(2, HarvesterIntroVoiceSound5);
		
		case 7:
			%nextStageTime = 1250;
			
			%effect = new Projectile()
			{
				dataBlock = HarvesterTeleportProjectile;
				initialVelocity = CutsceneHarvester.getForwardVector();
				initialPosition = CutsceneHarvester.getHackPosition();
				sourceObject = CutsceneHarvester;
				client = CutsceneHarvester.client;
			};

			if(isObject(%effect))
			{
				MissionCleanup.add(%effect);
				%effect.explode();
			}
			
			CutsceneHarvester.schedule(0, delete);
		
		case 8:
			clearHarvesterFightCamera();
			schedule(1000, 0, spawnHarvester);
			
		default:
			%nextStageTime = 3000;
			clearHarvesterFightCamera();
	}
	
	if(%stage < 8)
	{
		schedule(%nextStageTime, 0, harvesterIntroCutscene, %stage + 1);
	}
}

function AIPlayer::spawnHarvesterOutroExplosion(%this)
{
	%dataBlock = HarvesterBombProjectile;
	%scale = getRandom() / 2 + 0.5;
	%scale = %scale SPC %scale SPC %scale;
	
	if(getRandom() > 0.5)
	{
		%dataBlock = HarvesterAppearProjectile;
	}
	else if(getRandom() > 0.5)
	{
		%dataBlock = HarvesterBeamRifleProjectile;
	}
	else if(getRandom() > 0.75)
	{
		%dataBlock = BloodLilyProjectile;
		%scale = "0.7 0.7 0.7";
	}
	
	%effect = new Projectile()
	{
		dataBlock = %dataBlock;
		initialVelocity = %this.getForwardVector();
		initialPosition = vectorAdd(vectorAdd(%this.position, "0.0 0.0 5.0"), getRandom(-2.0, 2.0) SPC getRandom(-2.0, 2.0) SPC getRandom(-2.0, 2.0));
		scale = %scale;
	};

	if(isObject(%effect))
	{
		MissionCleanup.add(%effect);
		%effect.explode();
	}
}

function harvesterOutroCutscene(%stage)
{
	switch(%stage)
	{
		case 0:
			%nextStageTime = 500;

			new AIPlayer(CutsceneHarvester)
			{
				dataBlock = HarvesterArmor;
				position = _harvesterDeath.position;
				name = "The Harvester";
				invulnerable = true;
				isBot = true;
			};

			if(isObject(CutsceneHarvester))
			{
				MissionCleanup.add(CutsceneHarvester);

				HarvesterArmor.applyAvatar(CutsceneHarvester);
				CutsceneHarvester.mountImage(HarvesterFoldedBladeImage, 0);
				CutsceneHarvester.mountImage(HarvesterBeamRifleBackImage, 1);
				CutsceneHarvester.mountImage(HarvesterVisorLightImage, 2);
				CutsceneHarvester.mountImage(HarvesterStaggerImage, 3);
				
				CutsceneHarvester.playThread(0, "stun");

				%effect = new Projectile()
				{
					dataBlock = BloodLilyProjectile;
					initialVelocity = CutsceneHarvester.getForwardVector();
					initialPosition = CutsceneHarvester.getHackPosition();
					sourceObject = CutsceneHarvester;
					client = CutsceneHarvester.client;
				};

				if(isObject(%effect))
				{
					MissionCleanup.add(%effect);
					%effect.explode();
				}
			}
			
			new StaticShape(HarvesterVortex)
			{
				dataBlock = HarvesterVortexShape;
				position = _harvesterDeath.position;
			};

			if(isObject(HarvesterVortex))
			{
				MissionCleanup.add(HarvesterVortex);
				HarvesterVortex.playThread(0, "rotate");
			}
			
			new StaticShape(HarvesterRiser)
			{
				dataBlock = HarvesterRiserShape;
				position = _harvesterDeath.position;
			};

			if(isObject(HarvesterRiser))
			{
				MissionCleanup.add(HarvesterRiser);
				HarvesterRiser.hideNode("ALL");
				HarvesterRiser.mountObject(CutsceneHarvester, 0);
			}

			setHarvesterFightCamera(_harvesterCamera0.position, eulerToQuat("40.0 0.0 -150.0"));

			setHarvesterFightMusic(HarvesterSiriusBMusic);
			
			setHarvesterFightCamera(_harvesterCamera5.position, eulerToQuat("15.0 0.0 165.0"));
		
		case 1:
			%nextStageTime = 6200;
			
			for(%i = 0; %i < 80; %i++)
			{
				CutsceneHarvester.schedule(300 * %i, spawnHarvesterOutroExplosion);
			}
			
			HarvesterRiser.playThread(0, "rise");
						
			CutsceneHarvester.harvesterChat("The Harvested", "I have not seen... such crimson flowers before...");
			CutsceneHarvester.playAudio(2, HarvesterOutroVoiceSound1);
			
		case 2:
			%nextStageTime = 6200;
			
			setHarvesterFightCamera(_harvesterCamera6.position, eulerToQuat("45.0 0.0 205.0"));
				
		case 3:
			%nextStageTime = 6200;
			
			setHarvesterFightCamera(_harvesterCamera7.position, eulerToQuat("-70.0 0.0 180.0"));
				
		case 4:
			%nextStageTime = 6200;
			
			setHarvesterFightCamera(_harvesterCamera8.position, eulerToQuat("0.0 0.0 180.0"));
			
			CutsceneHarvester.harvesterChat("The Harvested", "What flowers... shall bloom upon my... grave..?");
			CutsceneHarvester.playAudio(2, HarvesterOutroVoiceSound2);
		
		case 5:
			%nextStageTime = 9000;
			
			%offset = vectorAdd(CutsceneHarvester.getHackPosition(), "0.0 0.0 10.0");
			
			%effect = new Projectile()
			{
				dataBlock = BloodLilyProjectile;
				initialVelocity = CutsceneHarvester.getForwardVector();
				initialPosition = %offset;
				scale = "1.5 1.5 1.5";
				sourceObject = CutsceneHarvester;
				client = CutsceneHarvester.client;
			};

			if(isObject(%effect))
			{
				MissionCleanup.add(%effect);
				%effect.explode();
			}
			
			serverPlay3d(HarvesterDeathSound1, %offset);
			
			CutsceneHarvester.schedule(0, delete);
			HarvesterVortex.delete();
			HarvesterRiser.delete();
			
		case 6:
			clearHarvesterFightCamera();
			clearHarvesterFightMusic();
			
		default:
			%nextStageTime = 3000;
			clearHarvesterFightCamera();
	}
	
	if(%stage < 6)
	{
		schedule(%nextStageTime, 0, harvesterOutroCutscene, %stage + 1);
	}
}