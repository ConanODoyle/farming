//--------//
// Spike: //
//--------//

datablock ExplosionData(L4SilenceSpikeExplosion : HarvesterSpikeExplosion)
{
	//-------------//
	// Properties: //
	//-------------//

	damageRadius = 0.0;
	radiusDamage = 0.0;

	impulseRadius = 0.0;
	impulseForce = 0.0;
};

datablock ProjectileData(L4SilenceSpikeProjectile : HarvesterSpikeProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = L4SilenceSpikeExplosion;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function L4SilenceSpikeProjectile::onExplode(%this, %projectile, %position, %fade)
{
	Parent::onExplode(%this, %projectile, %position, %fade);
	serverPlay3d("HarvesterSpikeExplosionSound" @ getRandom(1, 2), %position);
}

//----------------------//
// L4 - "Silence" Item: //
//----------------------//

datablock ItemData(L4SilenceItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/silence.dts";
	emap = false;
	
	doColorShift = true;
	colorShiftColor = "0.1 0.1 0.1 1.0";
	
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
	
	image = L4SilenceImage;
	
	canDrop = true;
	
	uiName = "L4 - \"Silence\"";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_silence";

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
// L4 - "Silence" Image: //
//-----------------------//

datablock ShapeBaseImageData(L4SilenceImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/silence.dts";

	emap = false;
	
	doColorShift = L4SilenceItem.doColorShift;
	colorShiftColor = L4SilenceItem.colorShiftColor;
	
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

	item = L4SilenceItem;
	
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
	
	toolTip = "+3 area harvest below ground crops";
	
	//---------//
	// States: //
	//---------//
	
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.15;
	stateTransitionOnTimeout[0] = "Ready";
	stateWaitForTimeout[0] = true;
	stateAllowImageChange[0] = true;
	stateSound[0] = weaponSwitchSound;
	stateScript[0] = "onActivate";
	
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Charge";
	stateAllowImageChange[1] = true;
	stateScript[1] = "onReady";
	
	stateName[2] = "Charge";
	stateTimeoutValue[2] = 0.65;
	stateTransitionOnTimeout[2] = "Fire";
	stateWaitForTimeout[2] = true;
	stateEmitter[2] = HarvesterBeamRifleSpinChargeEmitter;
	stateEmitterTime[2] = 0.5;
	stateEmitterNode[2] = "muzzleNode";
	stateAllowImageChange[2] = true;
	stateSound[2] = HarvesterBeamRifleChargeSound;
	stateScript[2] = "onCharge";
	
	stateName[3] = "Fire";
	stateTimeoutValue[3] = 0.5;
	stateTransitionOnTimeout[3] = "Wait";
	stateWaitForTimeout[3] = true;
	stateEmitter[3] = HarvesterBladeEnergyEmitter;
	stateEmitterTime[3] = 0.25;
	stateEmitterNode[3] = "muzzleNode";
	stateAllowImageChange[3] = false;
	stateSound[3] = HarvesterBeamRifleFireSound;
	stateScript[3] = "onFire";
	stateFire[3] = true;
	
	stateName[4] = "Wait";
	stateTimeoutValue[4] = 0.5;
	stateTransitionOnTimeout[4] = "CheckFire";
	stateWaitForTimeout[4] = true;
	stateAllowImageChange[4] = false;
	stateScript[4] = "onWait";
	
	stateName[5] = "CheckFire";
	stateTransitionOnTriggerDown[5] = "Fire";
	stateTransitionOnTriggerUp[5] = "Done";
	stateAllowImageChange[5] = true;
	stateScript[5] = "onCheckFire";
	
	stateName[6] = "Done";
	stateTimeoutValue[6] = 0.35;
	stateTransitionOnTimeout[6] = "Ready";
	stateWaitForTimeout[6] = true;
	stateAllowImageChange[6] = false;
	stateScript[6] = "onDone";
};

/// @param	this		weapon image
/// @param	player		player
/// @param	slot		number
/// @param	position	3-element position
/// @param	direction	3-element vector
function L4SilenceImage::spawnSpike(%this, %player, %slot, %position, %direction)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%typeMasks = $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FXBrickObjectType;
		
		%start = vectorAdd(vectorAdd(%position, "0.0 0.0 3.0"), %direction);
		%end = vectorSub(%start, "0.0 0.0 9.0");
		
		%groundRay = containerRaycast(%start, %end, %typeMasks);
		
		if(!isObject(firstWord(%groundRay)))
		{
			return;
		}
		
		%groundPosition = getWords(%groundRay, 1, 3);
		
		%scale = getRandom() + 1.0;
		
		%shape = new StaticShape()
		{
			dataBlock = HarvesterSpikeShape;
			position = %groundPosition;
			rotation = eulerToQuat_Degrees("0.0 22.5" SPC getRandom(0, 360));
			scale = %scale SPC %scale SPC %scale;
		};
		
		if(isObject(%shape))
		{
			MissionCleanup.add(%shape);
			
			%shape.setNodeColor("spike", "0.1 0.1 0.1 1.0");
			%shape.setNodeColor("iridescence", getRandom() SPC getRandom() SPC getRandom() SPC 0.0);

			%shape.playThread(0, "spike");
			%shape.schedule(1000, delete);
		}
		
		%effect = new Projectile()
		{
			dataBlock = L4SilenceSpikeProjectile;
			initialVelocity = "0.0 0.0 1.0";
			initialPosition = %groundPosition;
			scale = vectorScale(%scale SPC %scale SPC %scale, 0.5);
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
function L4SilenceImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "spearReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L4SilenceImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		for(%i = 0; %i < 8; %i++)
		{
			%this.schedule(67 + 67 * %i, spawnSpike, %player, %slot, %player.getHackPosition(), vectorScale(unitVectorFromAngles(45 * %i, 0), 4));
		}
				
		%effect = new Projectile()
		{
			dataBlock = HarvesterSpikeProjectile;
			initialVelocity = %player.getMuzzleVector(%slot);
			initialPosition = %player.getMuzzlePoint(%slot);
			scale = "0.67 0.67 0.67";
			sourceObject = %player;
			sourceSlot = %slot;
			client = %player.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
		
		%player.playThread(0, "plant");
		%player.playThread(2, "spearThrow");
	}
}