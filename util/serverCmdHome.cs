function serverCmdHome(%cl) {
	if($Sim::Time - %cl.lastHomeTime < 60) {
		%cl.centerPrint("You're trying to go home too often. Try again in" SPC convTime(90 - ($Sim::Time - %cl.lastHomeTime), 0) @ ".", 3);
		return;
	}

	if(isObject(%pl = %cl.player)) {
		
		%lotList = %cl.brickGroup.lotList;
		
		if (isObject(%veh = %pl.getObjectMount())) {
			%pl.dismount();
			%veh.spawnBrick.recoverVehicle();
		}
		
		if(getWordCount(%lotList) < 1) {
			%cl.centerPrint("You don't have a home - sending you back to spawn! <br><font:palatino linotype:36>If your lot is missing, talk to the lot manager!", 8);
			poofTeleport(%pl, _globalSpawn.getTransform());
			%cl.lastHomeTime = $Sim::Time;
			return;
		}

		%cl.centerPrint("Sending you to town...", 3);
		poofTeleport(%pl, _globalSpawn.getTransform());
		%cl.lastHomeTime = $Sim::Time;
		return;

		if(!%cl.checkMoney(5))
		{
			%cl.centerPrint("You don't have enough money. You need at least $5. Sending you to town...", 3);
			poofTeleport(%pl, _globalSpawn.getTransform());
			%cl.lastHomeTime = $Sim::Time;
			return;
		}
		
		for(%i = 0; %i < getWordCount(%lotList); %i++) {
			
			%lot = getWord(%lotList, %i);
			
			if(%lot.dataBlock.isSingle) {
				%found = %lot;
				break;
			}
		}
		if (!isObject(%found))
		{
			talk("ERROR: No single lot list found when calling /home! Lot list: " @ %lotList);
			return;
		}
				
		%cl.addMoney(-5);
		%cl.centerPrint("\c4You paid $5 to teleport home!", 6);
		%cl.lastHomeTime = $Sim::Time;
		poofTeleport(%pl, %lot.getTransform());
	}
}

function poofTeleport(%pl, %targetPos)
{
	%pb = new Projectile() {
		dataBlock = "deathProjectile";
		initialVelocity = "0 0 0";
		initialPosition = %pl.getTransform();
		sourceObject = %pl;
		sourceSlot = 0;
		client = %pl.client;
	};
	
	%pb.setScale(%pl.getScale());
	MissionCleanup.add(%pb);
	
	%pl.setTransform(%targetPos);
	
	%pa = new Projectile() {
		dataBlock = "deathProjectile";
		initialVelocity = "0 0 0";
		initialPosition = %pl.getTransform();
		sourceObject = %pl;
		sourceSlot = 0;
		client = %pl.client;
	};
	%pa.setScale(%pl.getScale());
	MissionCleanup.add(%pa);
}