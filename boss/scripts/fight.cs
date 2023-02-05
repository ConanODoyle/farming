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
		volume = 1.0;
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
			if(%client.getDataBlock() == HarvesterArmor.getID())
			{
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
		if(%victim.getDataBlock() == HarvesterArmor.getID())
		{
			return true;
		}
			
		return Parent::minigameCanDamage(%client, %victim);
	}
};
activatePackage(HarvesterFight);