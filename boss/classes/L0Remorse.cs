//----------------------//
// L0 - "Remorse" Item: //
//----------------------//

datablock ItemData(L0RemorseItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/remorse.dts";
	emap = false;
	
	doColorShift = true;
	colorShiftColor = "0.3 0.3 0.3 1.0";
	
	//----------//
	// Physics: //
	//----------//

	mass = 1.0;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	//-------------//
	// Properties: //
	//-------------//
	
	image = L0RemorseImage;
	
	canDrop = true;
	
	uiName = "L0 - \"Remorse\"";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_remorse";

	category = "Weapon";
	className = "Weapon";
	
	//----------//
	// Farming: //
	//----------//
	
	durability = 100000;
	
	canPickupMultiple = 1;

	hasDataID = 1;
	isDataIDTool = 1;
};

//-----------------------//
// L0 - "Remorse" Image: //
//-----------------------//

datablock ShapeBaseImageData(L0RemorseImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/blade.dts";

	emap = false;
	
	doColorShift = L0RemorseItem.doColorShift;
	colorShiftColor = L0RemorseItem.colorShiftColor;
	
	//-----------//
	// Mounting: //
	//-----------//
	
	offset = "0.0 0.0 0.0";
	eyeOffset = "0.0 0.0 0.0";

	rotation = "0.0 0.0 0.0 0.0";
	eyeRotation = "0.0 0.0 0.0 0.0";
	
	mountPoint = $RightHandSlot;
	
	//-------------//
	// Properties: //
	//-------------//
	
	correctMuzzleVector = true;
	melee = false;

	item = L0RemorseItem;
	
	ammo = "";
	projectile = "";
	projectileType = Projectile;

	armReady = true;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//----------//
	// Farming: //
	//----------//
	
	areaHarvest = 4;
	
	toolTip = "+3 area harvest above ground crops";
	
	//---------//
	// States: //
	//---------//
	
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.25;
	stateTransitionOnTimeout[0] = "PreReady";
	stateWaitForTimeout[0] = true;
	stateAllowImageChange[0] = true;
	stateSound[0] = weaponSwitchSound;
	stateSequence[0] = "equip";
	stateScript[0] = "onActivate";
	
	stateName[1] = "PreReady";
	stateTimeoutValue[1] = 0.15;
	stateTransitionOnTimeout[1] = "Ready";
	stateWaitForTimeout[1] = true;
	stateAllowImageChange[1] = true;
	stateScript[1] = "onPreReady";
	
	stateName[2] = "Ready";
	stateTransitionOnTriggerDown[2] = "PreFire";
	stateAllowImageChange[2] = true;
	stateScript[2] = "onReady";
	
	stateName[3] = "PreFire";
	stateTimeoutValue[3] = 0.25;
	stateTransitionOnTimeout[3] = "Fire";
	stateWaitForTimeout[3] = true;
	stateAllowImageChange[3] = false;
	stateScript[3] = "onPreFire";
	
	stateName[4] = "Fire";
	stateTimeoutValue[4] = 0.45;
	stateTransitionOnTimeout[4] = "Done";
	stateWaitForTimeout[4] = true;
	stateAllowImageChange[4] = false;
	stateEmitter[4] = HarvesterBladeEnergyEmitter;
	stateEmitterTime[4] = 0.15;
	stateEmitterNode[4] = "muzzlePoint";
	stateScript[4] = "onFire";
	stateFire[4] = true;
	
	stateName[5] = "Done";
	stateTimeoutValue[5] = 0.25;
	stateTransitionOnTimeout[5] = "Ready";
	stateWaitForTimeout[5] = true;
	stateAllowImageChange[5] = false;
	stateScript[5] = "onDone";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L0RemorseImage::onPreReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%effect = new Projectile()
		{
			dataBlock = HarvesterBladeEquipProjectile;
			initialVelocity = %player.getMuzzleVector(%slot);
			initialPosition = %player.getMuzzlePoint(%slot);
			sourceObject = %player;
			sourceSlot = %slot;
			client = %player.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L0RemorseImage::onPreFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "jump");
		%player.playThread(2, "activate");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L0RemorseImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%shape = new StaticShape()
		{
			dataBlock = HarvesterBladeTrailShape;
			scale = "9.0 9.0 1.0";
		};

		if(isObject(%shape))
		{
			MissionCleanup.add(%shape);

			%rotation = relativeVectorToRotation(%player.getForwardVector(), %player.getUpVector());
			
			%local = %player.getHackPosition() SPC %rotation;
			%offset = "0.0 0.0 0.0" SPC eulerToQuat("70.0 0.0 270.0");
			%actual = matrixMultiply(%local, %offset);

			%shape.setTransform(%actual);
			%shape.playThread(0, "rotate");
			%shape.schedule(1000, delete);
		}
		
		%player.playThread(0, "plant");
		%player.playThread(2, "shiftTo");
		%player.playThread(3, "shiftTo");

		serverPlay3d("HarvesterBladeAttackSound" @ getRandom(1, 2), %player.getHackPosition());
	}
}