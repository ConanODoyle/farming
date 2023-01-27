/// @param	this	player
function Player::cancelZoneCheck(%this)
{
	if(isEventPending(%this.zoneCheckSchedule))
	{
		cancel(%this.zoneCheckSchedule);
	}
}

/// @param	this	player
/// @param	time	number
function Player::checkInZoneAfter(%this, %time)
{
	%this.cancelZoneCheck();
	%this.zoneCheckSchedule = %this.schedule(%time, checkInZone);
}

/// @param	this	player
function Player::checkInZone(%this)
{
	%inZone = false;
	
	initContainerRadiusSearch(%this.position, 0.01, $TypeMasks::TriggerObjectType);
	while(isObject(%found = containerSearchNext()))
	{
		if(!%inZone)
		{
			%brick = %found.triggerBrick;
			%inZone = true;
		}
	}
	
	if(!%inZone)
	{
		return;
	}
	
	if(!isObject(%brick))
	{
		return;
	}
	
	%brick.onCheckInZoneTrue(%this.client);
}

/// @param	this	brick
function fxDTSBrick::onCheckInZoneTrue(%this, %client)
{
	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %client.player;
	$InputTarget_["Client"] = %client;
	
	%this.processInputEvent(onCheckInZoneTrue, %client);
}

registerOutputEvent(Player, checkInZone);
registerOutputEvent(Player, checkInZoneAfter, "int 0 30000 5000");
registerOutputEvent(Player, cancelZoneCheck);
registerInputEvent(fxDTSBrick, onCheckInZoneTrue, "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");