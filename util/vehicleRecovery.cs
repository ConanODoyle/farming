package vehicleRecovery
{
	function Player::activateStuff(%obj)
	{
		if(isObject(%client = %obj.client) && %obj.nextRecovery < $sim::time)
		{
			if(%client.getClassName() $= "GameConnection")
			{
				%start = %obj.getEyeTransform();
				%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
				%hit = firstWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType));

				if(isObject(%hit))
				{
					if(%hit.getDatablock().specialBrickType $= "VehicleSpawn" && getTrustLevel(%client, %hit))
					{
						commandToClient(%client, 'messageBoxYesNo', "Recover Vehicle", "Recover this vehicle?\n\nThis is free.", 'recoverMyVehiclePlease');
						%obj.recoveringVehicle = %hit;
					}
				}
			}
		}

		return parent::activateStuff(%obj);
	}
};
activatePackage(vehicleRecovery);

function servercmdRecoverMyVehiclePlease(%client)
{
	if(!isObject(%client.player))
		return;

	%player = %client.player;

	if(!isObject(%player.recoveringVehicle))
		return;

	if(vectorDist(%player.getPosition(), %player.recoveringVehicle.getPosition()) > 6)
	{
		%client.chatMessage("You have moved too far away.");
		return;
	}

	%player.recoveringVehicle.recoverVehicle();
	%player.recoveringVehicle = "";
	%player.nextRecovery = $sim::time + 3;

	%client.chatMessage("Vehicle recovered.");
}