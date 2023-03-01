if(!isObject(HarvesterFightSet))
{
	new SimSet(HarvesterFightSet);
}

/// @param	message	string
function harvesterMessage(%message)
{
	for(%i = 0; %i < HarvesterFightSet.getCount(); %i++)
	{
		%player = HarvesterFightSet.getObject(%i);
		%client = %player.client;
		
		if(!isObject(%client))
		{
			continue;
		}
		
		messageClient(%client, '', %message);
	}

	//outside arena additions
	for(%i = 0; %i < HarvesterDeathSet.getCount(); %i++)
	{
		%client = HarvesterDeathSet.getObject(%i);

		messageClient(%client, '', %message);
	}
}

/// @param	profile	audio profile
function setHarvesterFightMusic(%profile)
{
	if(isObject(HarvesterMusic))
	{
		HarvesterMusic.delete();
	}
	
	if(!isObject(%profile))
	{
		return; // Set non-existent profile to stop music.
	}
	
	new AudioEmitter(HarvesterMusic)
	{
		profile = %profile;
		position = "-99999999.0 -99999999.0 -99999999.0"; // Allows for per-client music by scoping audio emitter to specific clients.
		maxDistance = inf;
		volume = 0.9;
	};

	for(%i = 0; %i < HarvesterFightSet.getCount(); %i++)
	{
		%player = HarvesterFightSet.getObject(%i);
		%client = %player.client;
		
		if(!isObject(%client))
		{
			continue;
		}
		
		HarvesterMusic.scopeToClient(%client);
	}

	//outside arena additions
	for(%i = 0; %i < HarvesterDeathSet.getCount(); %i++)
	{
		%client = HarvesterDeathSet.getObject(%i);

		HarvesterMusic.scopeToClient(%client);
	}
}

function clearHarvesterFightMusic()
{
	setHarvesterFightMusic();
}

/// @param	position	3-element position
/// @param	rotation	4-element axis-angle rotation
function setHarvesterFightCamera(%position, %rotation)
{	
	for(%i = 0; %i < HarvesterFightSet.getCount(); %i++)
	{
		%player = HarvesterFightSet.getObject(%i);
		%client = %player.client;
		
		if(!isObject(%client))
		{
			continue;
		}

		%camera = %client.camera;

		if(!isObject(%camera))
		{
			continue;
		}
		
		// Release camera from player.
		%camera.setFlyMode();
		%camera.setMode("Observer");
		
		// Client controls camera.
		%client.setControlObject(%camera);
		
		// Camera controls dummy camera. Apparently a default thing?
		%camera.setControlObject(%client.dummyCamera);
		%client.dummyCamera.setTransform(%camera.getTransform());
		
		%camera.setTransform(%position SPC %rotation);
	}
	for(%i = 0; %i < HarvesterDeathSet.getCount(); %i++)
	{
		%client = HarvesterDeathSet.getObject(%i);
		
		if(!isObject(%client))
		{
			continue;
		}
		if(!isObject(%client.player))
		{
			%client.instantRespawn();
			%client.player.setTransform(_harvesterDeathZone.getTransform());
		}

		%camera = %client.camera;

		if(!isObject(%camera))
		{
			continue;
		}
		
		// Release camera from player.
		%camera.setFlyMode();
		%camera.setMode("Observer");
		
		// Client controls camera.
		%client.setControlObject(%camera);
		
		// Camera controls dummy camera. Apparently a default thing?
		%camera.setControlObject(%client.dummyCamera);
		%client.dummyCamera.setTransform(%camera.getTransform());
		
		%camera.setTransform(%position SPC %rotation);
	}
}

function clearHarvesterFightCamera()
{
	for(%i = 0; %i < HarvesterFightSet.getCount(); %i++)
	{
		%player = HarvesterFightSet.getObject(%i);
		%client = %player.client;
		
		if(!isObject(%client))
		{
			continue;
		}
		
		%camera = %client.camera;

		if(!isObject(%camera))
		{
			continue;
		}

		// Client controls player.
		%client.setControlObject(%player);

		// Camera controls nothing.
		%camera.setControlObject("");
	}
	for(%i = 0; %i < HarvesterDeathSet.getCount(); %i++)
	{
		%client = HarvesterDeathSet.getObject(%i);
		%player = %client.player;
		
		if(!isObject(%client))
		{
			continue;
		}

		if (!isObject(%player))
		{
			%transform = %client.camera.getTransform();
			%client.instantRespawn();
			%client.player.setTransform(%transform);
			continue;
		}
		
		%camera = %client.camera;

		if(!isObject(%camera))
		{
			continue;
		}

		// Client controls player.
		// if (!isObject(%client.player))
		// {
		// 	%client.instantRespawn();
		// 	%client.player.position = _harvesterDeathZone.position;
		// }
		%client.setControlObject(%player);

		// Camera controls nothing.
		%camera.setControlObject("");
	}
}

function AIPlayer::updateHPShapename(%pl)
{
	%percent = %pl.getDamagePercent();
	%percent = 1 - %percent;

	%r = getMin((1 - %percent) * 2, 1);//0 -> 1 for 1 to 0.5, then 1 -> 1 for 0.5 to 0
	%g = (1 - getMax(0, 0.5 - %percent) * 2); //1 -> 1 for 1 to 0.5, then 1 -> 0 for 0.5 to 0

	%pl.setShapeNameColor(%r SPC %g SPC 0);
	%pl.setShapeName(mFloatLength(%percent * 100, 1) @ "% HP", 8564862);
	%pl.setShapeNameDistance(50);
}

package HarvesterFight
{
	/// @param	client	client
	/// @param	victim	player
	function minigameCanDamage(%client, %victim)
	{
		// Harvester v. players:
		if(%client.isBot)
		{
			%dataBlock = %client.getDataBlock();

			if(%dataBlock == HarvesterArmor.getID() || %dataBlock == AncientWarriorArmor.getID())
			{
				%client.schedule(1, updateHPShapename);
				if(%client.getID() == %victim.getID())
				{
					return false;
				}
				
				if(!HarvesterFightSet.isMember(%victim))
				{
					return false; // Only damage participating players.
				}
				
				return true;
			}
		}
		
		// Players v. Harvester:
		%dataBlock = %victim.getDataBlock();
		
		if(%dataBlock == HarvesterArmor.getID() || %dataBlock == AncientWarriorArmor.getID())
		{
			%victim.schedule(1, updateHPShapename);
			%player = %client.player;
			
			if((%client.getType() & $TypeMasks::PlayerObjectType) || (%client.getType() & $TypeMasks::CorpseObjectType))
			{
				%player = %client;
			}
			
			if(HarvesterFightSet.isMember(%player))
			{
				return true;
			}
			
			return false;
		}
			
		return Parent::minigameCanDamage(%client, %victim);
	}
};
activatePackage(HarvesterFight);