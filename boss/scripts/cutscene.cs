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
			
			setHarvesterFightCamera(_harvesterCamera0.position, eulerToQuat("40 0 -150"));
			
			setHarvesterFightMusic(HarvesterIntroMusic);
			schedule(20210, 0, setHarvesterFightMusic, HarvesterPhase1Music);
			
		case 1:
			%nextStageTime = 1900;
			
			CutsceneHarvester.setVelocity("0 6 16");
			CutsceneHarvester.playThread(0, "slamAttack");
			
			serverPlay3d(HarvesterLeapSound, CutsceneHarvester.position);
			
			CutsceneHarvester.harvesterChat("???", "Who dares trespass? Why have you come?");
			
			CutsceneHarvester.playAudio(2, HarvesterIntroVoiceSound1);
			CutsceneHarvester.schedule(2200, playAudio, 2, HarvesterIntroVoiceSound2);
			
		case 2:
			%nextStageTime = 2400;
			
			// Land sound (synced with landing from jump).
			serverPlay3d(HarvesterLandSound, CutsceneHarvester.position);
			
			setHarvesterFightCamera(_harvesterCamera1.position, eulerToQuat("-60 0 180"));
			
		case 3:
			%nextStageTime = 2200;
			
			CutsceneHarvester.mountImage(HarvesterBladeImage, 0);
			CutsceneHarvester.setHeadAngle($piOver2);
			CutsceneHarvester.playThread(0, "slamDone");
			
			setHarvesterFightCamera(_harvesterCamera2.position, eulerToQuat("15 0 115"));
			
			CutsceneHarvester.harvesterChat("???", "No matter...");
			CutsceneHarvester.playAudio(2, HarvesterIntroVoiceSound3);
			
		case 4:
			%nextStageTime = 3300;
			
			CutsceneHarvester.setTransform(CutsceneHarvester.position SPC "-1 0 0 3.14159");
			CutsceneHarvester.setHeadAngle(0);
			CutsceneHarvester.playThread(0, "sweepReady");
			
			setHarvesterFightCamera(_harvesterCamera3.position, eulerToQuat("30 0 -37.5"));
			
			CutsceneHarvester.harvesterChat("???", "The tallest stalk is cut down first, farmer.");
			CutsceneHarvester.playAudio(2, HarvesterIntroVoiceSound4);
			
		case 5:
			%nextStageTime = 250;
			
			CutsceneHarvester.playThread(0, "stunDone");
			
			setHarvesterFightCamera(_harvesterCamera4.position, eulerToQuat("0 0 0"));
			
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