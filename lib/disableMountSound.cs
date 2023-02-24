//overwrite default onMount code to include a check for skipMountSound
//everything except the if statement over the sound is exactly the same as default
function Armor::onMount(%this, %obj, %vehicle, %node)
{
	if (%node == 0.0)
	{
		if (%vehicle.isHoleBot)
		{
			if (%vehicle.controlOnMount)
			{
				%obj.setControlObject(%vehicle);
				%vehicle.lastDrivingClient = %obj.client;
			}
		}
		else
		{
			if (%vehicle.getControllingClient() == 0.0)
			{
				%obj.setControlObject(%vehicle);
				%vehicle.lastDrivingClient = %obj.client;
			}
		}
	}
	else
	{
		%obj.setControlObject(%obj);
	}
	%obj.setTransform("0 0 0 0 0 1 0");
	%obj.playThread(0, %vehicle.getDataBlock().mountThread[%node]);
	if (!%this.skipMountSound && !%obj.skipMountSound && !%vehicle.skipMountSound && !%vehicle.getDataBlock().skipMountSound)
	{
		ServerPlay3D(playerMountSound, %obj.getPosition());
	}
	if (%vehicle.getDataBlock().lookUpLimit !$= "")
	{
		%obj.setLookLimits(%vehicle.getDataBlock().lookUpLimit, %vehicle.getDataBlock().lookDownLimit);
	}
}