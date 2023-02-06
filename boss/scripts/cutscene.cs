function harvesterIntroCutscene(%stage)
{
	talk("harvesterIntroCutscene(" @ %stage @ ")");
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
			
		case 2:
			%nextStageTime = 3000;
			
			// Land sound (synced with landing from jump).
			serverPlay3d(HarvesterLandSound, CutsceneHarvester.position);
			
			setHarvesterFightCamera(_harvesterCamera1.position, eulerToQuat("-60 0 180"));
			
		case 3:
			%nextStageTime = 3000;
			
			CutsceneHarvester.mountImage(HarvesterBladeImage, 0);
			CutsceneHarvester.setHeadAngle($piOver2);
			CutsceneHarvester.playThread(0, "slamDone");
			
			setHarvesterFightCamera(_harvesterCamera2.position, eulerToQuat("15 0 115"));
			
		case 4:
			%nextStageTime = 3000;
			
			CutsceneHarvester.setTransform($harvesterDummy.position SPC "-1 0 0 3.14159");
			CutsceneHarvester.setHeadAngle(0);
			CutsceneHarvester.playThread(0, "sweepReady");
			
			setHarvesterFightCamera(_harvesterCamera3.position, eulerToQuat("30 0 -37.5"));
			
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
			
		default:
			%nextStageTime = 3000;
			clearHarvesterFightCamera();
	}
	
	if(%stage < 8)
	{
		schedule(%nextStageTime, 0, harvesterIntroCutscene, %stage + 1);
	}
}