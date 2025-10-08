package VehicleHonking
{
	function Armor::onTrigger(%this, %obj, %trig, %val)
	{
		if (%trig == 0 && isObject(%veh = %obj.getObjectMount()) && %veh.getControllingObject() == %obj)
		{
			if (%val && isObject(%veh.dataBlock.honkSound))
			{
				%veh.playAudio(0, %veh.dataBlock.honkSound);
			}
		}
		return parent::onTrigger(%this, %obj, %trig, %val);
	}
};
activatePackage(VehicleHonking);


function assignHonks()
{
	duoStandardJeepVehicle.honkSound = "HonkSound";
	duoCargoJeepVehicle.honkSound = "HonkSound";
	JeepVehicle.honkSound = "HonkSound";
}

schedule(1, 0, assignHonks);