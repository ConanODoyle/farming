function WandImage::onHitObject (%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
{
	%client = %player.client;
	if (!isObject (%client))
	{
		return;
	}
	if (%hitObj.getType () & $TypeMasks::FxBrickAlwaysObjectType)
	{
		if (getTrustLevel (%player, %hitObj) < $TrustLevel::Wand)
		{
			if (%hitObj.stackBL_ID $= "" || %hitObj.stackBL_ID != %client.getBLID ())
			{
				%client.sendTrustFailureMessage (%hitObj.getGroup ());
				return;
			}
		}
		%hitObj.onToolBreak (%client);
		$CurrBrickKiller = %client;
		%hitObj.killBrick ();
	}
	else if (%hitObj.getType () & $TypeMasks::PlayerObjectType)
	{
		if (miniGameCanDamage (%client, %hitObj) == 1)
		{
			
		}
		else if (miniGameCanDamage (%client, %hitObj) == 0)
		{
			commandToClient (%client, 'CenterPrint', %hitObj.client.getPlayerName () @ " is in a different minigame.", 1);
			return;
		}
		else if (getTrustLevel (%player, %hitObj) < $TrustLevel::Wand)
		{
			%client.sendTrustFailureMessage (%hitObj.client.brickGroup);
			return;
		}
		// %hitObj.setVelocity ("0 0 15");
	}
}