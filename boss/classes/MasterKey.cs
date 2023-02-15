//--------------------//
// Master Key Sounds: //
//--------------------//

datablock AudioProfile(MasterKeyFireSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/masterKeyFire.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(MasterKeyReloadSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/masterKeyReload.wav";
	description = AudioDefault3d;
	preload = true;
};

//--------------//
// Melee Trail: //
//--------------//

datablock StaticShapeData(MasterKeyMeleeTrailShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/smallSlice.dts";
};

//--------//
// Trail: //
//--------//

datablock ParticleData(MasterKeyTrailParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/cloud";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 1.0 0.0 0.1";
	colors[1]	= "1.0 1.0 0.4 0.08";
	colors[2]	= "0.7 0.7 0.7 0.0";
	
	sizes[0]	= 0.4;
	sizes[1]	= 0.25;
	sizes[2]	= 0.0;
	
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 100;
	lifetimeVarianceMS = 0;

	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
};
datablock ParticleEmitterData(MasterKeyTrailEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "MasterKeyTrailParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-------//
// Conc: //
//-------//

datablock ProjectileData(MasterKeyConcProjectile : gunProjectile)
{
	//------------//
	// Rendering: //
	//------------//

	projectileShapeName = "Add-Ons/Vehicle_Tank/tankbullet.dts";
	
	particleEmitter = MasterKeyTrailEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	directDamage = $Harvester::MasterKey::ConcDamage;
	
	impactImpulse = 100.0;
	verticalImpulse = 100.0;
	
	//----------//
	// Physics: //
	//----------//
	
	armingDelay = 0;
	lifetime = 90;
	fadeDelay = 0;
	
	//------------//
	// Explosion: //
	//------------//
	
	brickExplosionRadius = 0.0;
	brickExplosionImpact = false;
	brickExplosionForce = 0.0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	collision	object
/// @param	fade		number
/// @param	position	3-element position
/// @param	normal		3-element vector
function MasterKeyConcProjectile::damage(%this, %projectile, %collision, %fade, %position, %normal)
{
	// Prevent senseless violence against innocent townsfolk.	
	%dataBlock = %collision.getDataBlock();

	if(%dataBlock == HarvesterArmor.getID() || %dataBlock == AncientWarriorArmor.getID())
	{
		return Parent::damage(%this, %projectile, %collision, %fade, %position, %normal);
	}
	
	return;
}

//---------//
// Bullet: //
//---------//

datablock ProjectileData(MasterKeyProjectile : gunProjectile)
{
	//------------//
	// Rendering: //
	//------------//

	particleEmitter = MasterKeyTrailEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	directDamage = $Harvester::MasterKey::ProjectileDamage;
	
	impactImpulse = 0.0;
	verticalImpulse = 0.0;
	
	//----------//
	// Physics: //
	//----------//
	
	armingDelay = 0;
	lifetime = 2000;
	fadeDelay = 0;
	
	//------------//
	// Explosion: //
	//------------//
	
	brickExplosionRadius = 0.0;
	brickExplosionImpact = false;
	brickExplosionForce = 0.0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	collision	object
/// @param	fade		number
/// @param	position	3-element position
/// @param	normal		3-element vector
function MasterKeyProjectile::damage(%this, %projectile, %collision, %fade, %position, %normal)
{
	// Prevent senseless violence against innocent townsfolk.	
	%dataBlock = %collision.getDataBlock();

	if(%dataBlock == HarvesterArmor.getID() || %dataBlock == AncientWarriorArmor.getID())
	{
		return Parent::damage(%this, %projectile, %collision, %fade, %position, %normal);
	}
	
	return;
}

//------------------//
// Master Key Item: //
//------------------//

datablock ItemData(MasterKeyItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/masterKey.dts";
	emap = false;
	
	rotate = true;
	
	doColorShift = true;
	colorShiftColor = "0.75 0.75 0.75 1.0";
	
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
	
	image = MasterKeyImage;
	
	canDrop = true;
	
	uiName = "Master Key";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_masterKey";

	category = "Weapon";
	className = "Weapon";
};

/// @param	this	item datablock
/// @param	item	item
function MasterKeyItem::onAdd(%this, %item)
{
	Parent::onAdd(%this, %item);
	%item.rotate = %this.rotate;
}

//--------------//
// Blade Image: //
//--------------//

datablock ShapeBaseImageData(MasterKeyImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/masterKey.dts";

	emap = false;
	
	doColorShift = MasterKeyItem.doColorShift;
	colorShiftColor = MasterKeyItem.colorShiftColor;
	
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

	item = MasterKeyItem;
	
	ammo = "";
	projectile = MasterKeyConcProjectile;
	projectileType = Projectile;
	subProjectile = MasterKeyProjectile;
	subProjectileType = Projectile;
	subProjectileCount = 3;
	subProjectileSpread = 0.01;
	
	casing = gunShellDebris;
	shellExitDir = "1.0 -1.3 1.0";
	shellExitOffset = "0 0 0";
	shellExitVariance = 15.0;	
	shellVelocity = 7.0;

	armReady = true;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//-----------//
	// Hitboxes: //
	//-----------//
	
	hitboxCount[0] = 1;
	hitboxMultiLockout[0] = true;
	
	hitboxOffset[0, 0] = "0.0 0.25 0.0";
	hitboxRadius[0, 0] = 2.0;
	hitboxDamage[0, 0] = $Harvester::MasterKey::Melee::Damage;
	hitboxDamageType[0, 0] = $DamageType::Sword;
	hitboxProjectile[0, 0] = swordProjectile;
	hitboxProjectileOnTarget[0, 0] = true;	
	hitboxVelocity[0, 0] = "0.0 10.0 5.0";
	hitboxUseMass[0, 0] = true;
	hitboxCancelVelocity[0, 0] = false;
	hitboxColor[0, 0] = "0.90 0.10 0.29";
	hitboxSpawnTime[0, 0] = 0;
	
	//---------//
	// States: //
	//---------//
	
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.15;
	stateTransitionOnTimeout[0] = "Eject";
	stateWaitForTimeout[0] = true;
	stateAllowImageChange[0] = true;
	stateSound[0] = weaponSwitchSound;
	stateScript[0] = "onActivate";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;
	stateScript[1] = "onReady";
	
	stateName[2] = "Fire";
	stateTimeoutValue[2] = 0.15;
	stateTransitionOnTimeout[2] = "Recoil";
	stateWaitForTimeout[2] = true;
	stateAllowImageChange[2] = false;
	stateSound[2] = MasterKeyFireSound;
	stateSequence[2] = "recoil";
	stateScript[2] = "onFire";
	stateFire[2] = true;
	
	stateName[3] = "Recoil";
	stateTimeoutValue[3] = 0.35;
	stateTransitionOnTimeout[3] = "Eject";
	stateWaitForTimeout[3] = true;
	stateAllowImageChange[3] = false;
	stateSequence[3] = "recoilDone";
	stateScript[3] = "onRecoil";
	
	stateName[4] = "Eject";
	stateTimeoutValue[4] = 0.35;
	stateTransitionOnTimeout[4] = "Ready";
	stateWaitForTimeout[4] = true;
	stateAllowImageChange[4] = false;
	stateEjectShell[4] = true;
	stateSound[4] = MasterKeyReloadSound;
	stateSequence[4] = "eject";
	stateScript[4] = "onEject";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function MasterKeyImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		Parent::onFire(%this, %player, %slot);
		
		%adjustment[0] = unitVectorFromAngles(1.40625, -1.40625);
		%adjustment[1] = unitVectorFromAngles(0, 1.40625);
		%adjustment[2] = unitVectorFromAngles(-1.40625, -1.40625);
		
		for(%i = 0; %i < %this.subProjectileCount; %i++)
		{
			%rotation = relativeVectorToRotation(%player.getMuzzleVector(%slot), %player.getUpVector());
			%clamped = mClampF(firstWord(%rotation), -89.9, 89.9) SPC restWords(%rotation);
			%local = %player.getHackPosition() SPC %clamped;
			%actual = matrixMulVector(%local, %adjustment[%i]);
			%velocity = vectorScale(%actual, %this.subProjectile.muzzleVelocity);
			%velocity = randomVectorNudge(%velocity, %this.subProjectileSpread);
			
			%projectile = new (%this.subProjectileType)()
			{
				dataBlock = %this.subProjectile;
				initialVelocity = %velocity;
				initialPosition = %player.getMuzzlePoint(%slot);
				sourceObject = %player;
				sourceSlot = %slot;
				client = %player.client;
			};

			if(isObject(%projectile))
			{
				MissionCleanup.add(%projectile);
			}
		}
		
		if(getWord(%player.getLookVector(), 2) < -0.261799)
		{
			%player.addVelocity(vectorScale(%player.getLookVector(), -4.0));
		}
		
		%player.playThread(0, "activate");
		%player.playThread(2, "jump");
	}
}

/// @param	this		projectile datablock
/// @param	player		player
/// @param	slot		number
/// @param	collision	object
/// @param	position	3-element position
/// @param	damage		number
/// @param	damageType	number
/// @param	velocity	3-element vector
/// @param	group		number
/// @param	index		number
function MasterKeyImage::onHitboxDamage(%this, %player, %slot, %collision, %position, %damage, %damageType, %velocity, %group, %index)
{
	// Prevent senseless violence against innocent townsfolk.	
	%dataBlock = %collision.getDataBlock();

	if(%dataBlock == HarvesterArmor.getID() || %dataBlock == AncientWarriorArmor.getID())
	{
		return Parent::onHitboxDamage(%this, %player, %slot, %collision, %position, %damage, %damageType, %velocity, %group, %index);
	}
	
	return;
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function MasterKeyImage::onEject(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "shiftLeft");
		%player.playThread(2, "plant");
	}
}

package MasterKeyAltFire
{
	/// @param	this	playertype
	/// @param	player	player
	/// @param	trigger	number
	/// @param	val		bool
	function Armor::onTrigger(%this, %player, %trigger, %val)
	{
		Parent::onTrigger(%this, %player, %trigger, %val);
		
		if(!%val || %trigger != 4)
		{
			return;
		}
		
		%image = %player.getMountedImage(0);
		
		if(!isObject(%image))
		{
			return;
		}
		
		if(%image != MasterKeyImage.getID())
		{
			return;
		}
		
		if(getSimTime() - %player.lastMasterKeyMelee < $Harvester::MasterKey::Melee::CooldownMS)
		{
			return;
		}
		
		%player.lastMasterKeyMelee = getSimTime();
		
		%player.cancelPendingHitboxes();
		MasterKeyImage.spawnHitboxGroup(%player, 0, 0);
		
		%shape = new StaticShape()
		{
			dataBlock = MasterKeyMeleeTrailShape;
			scale = "4.0 4.0 2.0";
		};

		if(isObject(%shape))
		{
			MissionCleanup.add(%shape);

			%rotation = relativeVectorToRotation(%player.getLookVector(), %player.getUpVector());
			%clamped = mClampF(firstWord(%rotation), -89.9, 89.9) SPC restWords(%rotation);
			
			%local = %player.getHackPosition() SPC %clamped;
			%offset = "0.3 1.4 0.7" SPC eulerToQuat("0.0 67.0 0.0");
			%actual = matrixMultiply(%local, %offset);

			%shape.setTransform(%actual);
			%shape.playThread(0, "rotate");
			%shape.schedule(1000, delete);
		}
		
		%player.playThread(0, "activate");
		%player.playThread(2, "jump");
	}
};
activatePackage(MasterKeyAltFire);