//---------------//
// Blade Sounds: //
//---------------//

datablock AudioProfile(HarvesterBladeAttackSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/bladeAttack1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterBladeAttackSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/bladeAttack2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterBladeHitSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/bladeHit1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterBladeHitSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/bladeHit2.wav";
	description = AudioDefault3d;
	preload = true;
};

//---------------//
// Blade Trail: //
//---------------//

datablock StaticShapeData(HarvesterBladeTrailShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/bladeTrail.dts";
};

//-------------------//
// Blade Equip Ring: //
//-------------------//

datablock ParticleData(HarvesterBladeEquipRingParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/fadeRing";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.1 0.1 1.0";
	colors[1]	= "1.0 0.1 0.0 0.0";
	
	sizes[0]	= 0.5;
	sizes[1]	= 5.0;

	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 300;
	lifetimeVarianceMS = 0;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBladeEquipRingEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBladeEquipRingParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = HarvesterBladeEquipRingParticle.lifetimeMS;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//----------------------//
// Blade Equip Sparkle: //
//----------------------//

datablock ParticleData(HarvesterBladeEquipSparkleFrame1Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/dot/bigDot";
};
datablock ParticleData(HarvesterBladeEquipSparkleFrame2Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/dot/smallDot";
};
datablock ParticleData(HarvesterBladeEquipSparkleParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/dot/bigDot";

	animTexName[0]	= $Harvester::Root @ "/resources/particles/dot/bigDot";
	animTexName[1]	= $Harvester::Root @ "/resources/particles/dot/smallDot";
	
	animateTexture = true;
	framesPerSec = 15;
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.4 0.3 1.0";
	colors[1]	= "1.0 0.1 0.1 1.0";
	colors[2]	= "1.0 0.1 0.0 1.0";

	sizes[0]	= 0.4;
	sizes[1]	= 0.1;
	sizes[2]	= 0.0;
	
	times[0]	= 0.0;
	times[1]	= 0.8;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.2;
	gravityCoefficient = 0.1;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 400;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBladeEquipSparkleEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBladeEquipSparkleParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 11;
	periodVarianceMS = 0;
	
	ejectionVelocity = 4.0;
	velocityVariance = 1.0;
	
	ejectionOffset = 0.5;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//---------------//
// Blade Energy: //
//---------------//

datablock ParticleData(HarvesterBladeEnergyParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/shock";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.4 0.3 1.0";
	colors[1]	= "1.0 0.1 0.1 0.0";
	colors[2]	= "1.0 0.1 0.0 0.0";
	
	sizes[0]	= 5.0;
	sizes[1]	= 4.0;
	sizes[2]	= 1.0;

	times[0]	= 0.0;
	times[1]	= 0.25;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 500;
	lifetimeVarianceMS = 250;

	spinSpeed = 33.0;
	spinRandomMin = -100.0;
	spinRandomMax = 100.0;
};
datablock ParticleEmitterData(HarvesterBladeEnergyEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBladeEnergyParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 8;
	periodVarianceMS = 2;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 1.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//------------------//
// Blade Hit Spark: //
//------------------//

datablock ParticleData(HarvesterBladeHitSparkParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/hitSpark";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 1.0 1.0 1.0";
	colors[1]	= "1.0 1.0 1.0 1.0";

	sizes[0]	= 1.0;
	sizes[1]	= 1.75;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 150;
	lifetimeVarianceMS = 25;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBladeHitSparkEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBladeHitSparkParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 14;
	periodVarianceMS = 0;
	
	ejectionVelocity = 30.0;
	velocityVariance = 4.0;
	
	ejectionOffset = 0.2;
	
	thetaMin = 0.0;
	thetaMax = 45.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-------------------//
// Blade Hit Sparks: //
//-------------------//

datablock ParticleData(HarvesterBladeHitSparksParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/pSpark";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.9 0.8 1.0";
	colors[1]	= "1.0 0.4 0.3 1.0";
	colors[2]	= "1.0 0.1 0.1 1.0";
	colors[3]	= "1.0 0.1 0.0 1.0";

	sizes[0]	= 0.0;
	sizes[1]	= 1.3;
	sizes[2]	= 0.8;
	sizes[3]	= 0.0;
	
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.0;
	gravityCoefficient = 2.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 450;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBladeHitSparksEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBladeHitSparksParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 9;
	periodVarianceMS = 3;
	
	ejectionVelocity = 22.0;
	velocityVariance = 7.0;
	
	ejectionOffset = 0.7;
	
	thetaMin = 0.0;
	thetaMax = 35.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//------------------//
// Blade Hit Spark: //
//------------------//

datablock ParticleData(HarvesterBladeHitShockParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/shock";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.4 0.3 1.0";
	colors[1]	= "1.0 0.1 0.0 0.0";

	sizes[0]	= 5.0;
	sizes[1]	= 4.0;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 500;
	lifetimeVarianceMS = 250;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBladeHitShockEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBladeHitShockParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 14;
	periodVarianceMS = 0;
	
	ejectionVelocity = 16.0;
	velocityVariance = 6.0;
	
	ejectionOffset = 0.3;
	
	thetaMin = 0.0;
	thetaMax = 90.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//--------------//
// Blade Equip: //
//--------------//

datablock ExplosionData(HarvesterBladeEquipExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	particleEmitter = HarvesterBladeEquipRingEmitter;
	particleDensity = 1;
	particleRadius = 0.0;
	
	emitter[0] = HarvesterBladeEquipSparkleEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
};

datablock ProjectileData(HarvesterBladeEquipProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterBladeEquipExplosion;

	explodeOnDeath = true;
};

//------------//
// Blade Hit: //
//------------//

datablock ExplosionData(HarvesterBladeHitExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	emitter[0] = HarvesterBladeHitSparkEmitter;
	emitter[1] = HarvesterBladeHitSparksEmitter;
	emitter[2] = HarvesterBladeHitShockEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	shakeCamera = true;
	camShakeFreq = "6.0 6.5 6.0";
	camShakeAmp = "2.0 4.0 2.0";
	camShakeDuration = 0.25;
	camShakeRadius = 6;
};

datablock ProjectileData(HarvesterBladeHitProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterBladeHitExplosion;

	explodeOnDeath = true;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function HarvesterBladeHitProjectile::onExplode(%this, %projectile, %position, %fade)
{
	Parent::onExplode(%this, %projectile, %position, %fade);
	serverPlay3d("HarvesterBladeHitSound" @ getRandom(1, 2), %position);
}

//---------------//
// Blade Recoil: //
//---------------//

datablock ExplosionData(HarvesterBladeRecoilExplosion)
{	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	shakeCamera = true;
	camShakeFreq = "1.0 1.0 1.0";
	camShakeAmp = "3.0 5.2 3.0";
	camShakeDuration = 0.7;
	camShakeRadius = 22.0;
};

datablock ProjectileData(HarvesterBladeRecoilProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterBladeRecoilExplosion;

	explodeOnDeath = true;
};

//-------------//
// Blade Item: //
//-------------//

datablock ItemData(HarvesterBladeItem)
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
	
	image = HarvesterBladeImage;
	
	canDrop = true;
	
	uiName = "The Harvester's Remorse";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_remorse";

	category = "Weapon";
	className = "Weapon";
};

//---------------------//
// Folded Blade Image: //
//---------------------//

datablock ShapeBaseImageData(HarvesterFoldedBladeImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/blade.dts";

	emap = false;
	
	doColorShift = HarvesterBladeItem.doColorShift;
	colorShiftColor = HarvesterBladeItem.colorShiftColor;
	
	//-----------//
	// Mounting: //
	//-----------//
	
	offset = "0.0 0.0 0.0";
	eyeOffset = "0.0 0.0 0.0";

	rotation = "0.0 0.0 0.0 0.0";
	eyeRotation = "0.0 0.0 0.0 0.0";
	
	mountPoint = $LeftHandSlot;
	
	//-------------//
	// Properties: //
	//-------------//
	
	item = HarvesterBladeItem;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//---------//
	// States: //
	//---------//
	
	stateName[0] = "Activate";
	stateAllowImageChange[0] = true;
	// stateSound[0] = weaponSwitchSound;
	stateSequence[0] = "unequip";
	stateScript[0] = "onActivate";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterFoldedBladeImage::onActivate(%this, %player, %slot)
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

//--------------//
// Blade Image: //
//--------------//

datablock ShapeBaseImageData(HarvesterBladeImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/blade.dts";

	emap = false;
	
	doColorShift = HarvesterBladeItem.doColorShift;
	colorShiftColor = HarvesterBladeItem.colorShiftColor;
	
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

	item = HarvesterBladeItem;
	
	ammo = "";
	projectile = "";
	projectileType = Projectile;

	armReady = true;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//-----------//
	// Hitboxes: //
	//-----------//
	
	hitboxCount[0] = 4;
	hitboxMultiLockout[0] = true;
	hitboxIgnorePitch[0] = true;
	hitboxIgnoreMuzzlePoint[0] = true;
	hitboxLimitHits[0] = false;
	
	hitboxOffset[0, 0] = "0.0 2.0 1.33";
	hitboxRadius[0, 0] = 2.0;
	hitboxIgnoreOcclusion[0, 0] = true;
	hitboxDamage[0, 0] = $Harvester::Blade::Damage;
	hitboxDamageType[0, 0] = $DamageType::Direct;
	hitboxProjectile[0, 0] = HarvesterBladeHitProjectile;
	hitboxProjectileOnTarget[0, 0] = true;	
	hitboxVelocity[0, 0] = "0.0 18.0 8.5";
	hitboxUseMass[0, 0] = true;
	hitboxCancelVelocity[0, 0] = false;
	hitboxColor[0, 0] = "0.90 0.10 0.29";
	hitboxSpawnTime[0, 0] = 0;
	
	hitboxOffset[0, 1] = "0.0 4.75 1.22";
	hitboxRadius[0, 1] = 1.25;
	hitboxIgnoreOcclusion[0, 1] = true;
	hitboxDamage[0, 1] = $Harvester::Blade::Damage * 0.75;
	hitboxDamageType[0, 1] = $DamageType::Direct;
	hitboxProjectile[0, 1] = HarvesterBladeHitProjectile;
	hitboxProjectileScale[0, 1] = "0.8 0.8 0.8";
	hitboxProjectileOnTarget[0, 1] = true;	
	hitboxVelocity[0, 1] = "0.0 10.0 10.0";
	hitboxUseMass[0, 1] = true;
	hitboxCancelVelocity[0, 1] = false;
	hitboxColor[0, 1] = "0.90 0.10 0.29";
	hitboxSpawnTime[0, 1] = 0;
	
	hitboxOffset[0, 2] = "-2.25 0.5 1.33";
	hitboxRadius[0, 2] = 1.5;
	hitboxIgnoreOcclusion[0, 2] = true;
	hitboxDamage[0, 2] = $Harvester::Blade::Damage * 0.65;
	hitboxDamageType[0, 2] = $DamageType::Direct;
	hitboxProjectile[0, 2] = HarvesterBladeHitProjectile;
	hitboxProjectileScale[0, 2] = "0.7 0.7 0.7";
	hitboxProjectileOnTarget[0, 2] = true;	
	hitboxVelocity[0, 2] = "12.0 6.0 10.0";
	hitboxUseMass[0, 2] = true;
	hitboxCancelVelocity[0, 2] = false;
	hitboxColor[0, 2] = "0.90 0.10 0.29";
	hitboxSpawnTime[0, 2] = 0;
	
	hitboxOffset[0, 3] = "2.25 0.5 1.33";
	hitboxRadius[0, 3] = 1.5;
	hitboxIgnoreOcclusion[0, 3] = true;
	hitboxDamage[0, 3] = $Harvester::Blade::Damage * 0.65;
	hitboxDamageType[0, 3] = $DamageType::Direct;
	hitboxProjectile[0, 3] = HarvesterBladeHitProjectile;
	hitboxProjectileScale[0, 3] = "0.7 0.7 0.7";
	hitboxProjectileOnTarget[0, 3] = true;	
	hitboxVelocity[0, 3] = "6.0 -8.0 11.0";
	hitboxUseMass[0, 3] = true;
	hitboxCancelVelocity[0, 3] = false;
	hitboxColor[0, 3] = "0.90 0.10 0.29";
	hitboxSpawnTime[0, 3] = 0;
	
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
	stateTransitionOnTriggerDown[2] = "Charge";
	stateAllowImageChange[2] = true;
	stateScript[2] = "onReady";
	
	stateName[3] = "Charge";
	stateTransitionOnTriggerUp[3] = "Fire";
	stateAllowImageChange[3] = false;
	stateScript[3] = "onCharge";
	
	stateName[4] = "Fire";
	stateTimeoutValue[4] = 0.45;
	stateTransitionOnTimeout[4] = "Done";
	stateWaitForTimeout[4] = true;
	stateEmitter[4] = HarvesterBladeEnergyEmitter;
	stateEmitterTime[4] = 0.15;
	stateEmitterNode[4] = "muzzleNode";
	stateAllowImageChange[4] = false;
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
function HarvesterBladeImage::onMount(%this, %player, %slot)
{
	%image = %player.getMountedImage(1);
	
	if(isObject(%image) && %image == HarvesterFoldedBladeImage.getID())
	{
		%player.unMountImage(1);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBladeImage::onUnMount(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.mountImage(HarvesterFoldedBladeImage, 1);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBladeImage::onPreReady(%this, %player, %slot)
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
function HarvesterBladeImage::onReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "root");
	}
	
	%player.stopAudio(1);
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBladeImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBladeImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%this.spawnHitboxGroup(%player, %slot, 0);
		
		%shape = new StaticShape()
		{
			dataBlock = HarvesterBladeTrailShape;
			scale = "12.0 12.0 1.0";
		};

		if(isObject(%shape))
		{
			MissionCleanup.add(%shape);

			%rotation = relativeVectorToRotation(%player.getForwardVector(), %player.getUpVector());
			
			%local = %player.getHackPosition() SPC %rotation;
			%offset = "0.0 1.0 0.4" SPC eulerToQuat("-4.0 180.0 0.0");
			%actual = matrixMultiply(%local, %offset);

			%shape.setTransform(%actual);
			%shape.playThread(0, "rotate");
			%shape.schedule(1000, delete);
		}
	
		%effect = new Projectile()
		{
			dataBlock = HarvesterBladeRecoilProjectile;
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
		
		%player.playThread(0, "sweepAttack");
		%player.playAudio(1, "HarvesterYellSound" @ getRandom(1, 2));
		serverPlay3d("HarvesterBladeAttackSound" @ getRandom(1, 2), %player.getMuzzlePoint(%slot));
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBladeImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepDone");
	}
}