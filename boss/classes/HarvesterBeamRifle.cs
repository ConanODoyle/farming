//--------------------//
// Beam Rifle Sounds: //
//--------------------//

datablock AudioProfile(HarvesterBeamRifleFireSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/beamFire.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterBeamRifleChargeSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/spinCharge.wav";
	description = AudioDefault3d;
	preload = true;
};

//-------//
// Beam: //
//-------//

datablock StaticShapeData(HarvesterBeamShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/beam.dts";
};

//-------------------//
// Blade Equip Ring: //
//-------------------//

datablock ParticleData(HarvesterBeamRifleSpinChargeParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName =  $Harvester::Root @ "/resources/particles/charge";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.1 0.1 1.0";
	colors[1]	= "1.0 0.1 0.0 0.0";
	
	sizes[0]	= 5.0;
	sizes[1]	= 2.0;

	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 400;
	lifetimeVarianceMS = 0;

	spinSpeed = 1500.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBeamRifleSpinChargeEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBeamRifleSpinChargeParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 150;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-----------------------------//
// Beam Rifle Explosion Flare: //
//-----------------------------//

datablock ParticleData(HarvesterBeamRifleExplosionFlareParticle : HarvesterBombExplosionFlareParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName =  $Harvester::Root @ "/resources/particles/sparkle";
	
	colors[0]	= "1.0 0.9 0.9 0.6";
	colors[1]	= "1.0 0.2 0.2 0.4";
	colors[2]	= "1.0 0.0 0.0 0.0";
	
	sizes[0]	= 18;
	sizes[1]	= 33;
};
datablock ParticleEmitterData(HarvesterBeamRifleExplosionFlareEmitter : HarvesterBombExplosionFlareEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBeamRifleExplosionFlareParticle";
};

//-----------------------//
// Beam Rifle Explosion: //
//-----------------------//

datablock ParticleData(HarvesterBeamRifleExplosionParticle : HarvesterBombExplosionParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	colors[0]	= "1.0 1.0 1.0 0.3";
	colors[1]	= "1.0 0.2 0.2 0.6";
	colors[2]	= "0.1 0.05 0.025 0.1";
	colors[3]	= "0.1 0.05 0.025 0.0";
};
datablock ParticleEmitterData(HarvesterBeamRifleExplosionEmitter : HarvesterBombExplosionEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBeamRifleExplosionParticle";
};

//-----------------------------//
// Beam Rifle Explosion Smoke: //
//-----------------------------//

datablock ParticleData(HarvesterBeamRifleExplosionSmokeParticle : HarvesterBombExplosionSmokeParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	colors[0]	= "0.98 0.9 0.8 0.8";
	colors[1]	= "0.1 0.045 0.02 0.6";
	colors[2]	= "0.1 0.05 0.025 0.0";
};
datablock ParticleEmitterData(HarvesterBeamRifleExplosionSmokeEmitter : HarvesterBombExplosionSmokeEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBeamRifleExplosionSmokeParticle";
};

//------------------------------------//
// Beam Rifle Explosion Debris Trail: //
//------------------------------------//

datablock ParticleData(HarvesterBeamRifleExplosionDebrisTrailParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/chunk";
	
	useInvAlpha = false;

	colors[0]	= "1.0 0.4 0.4 0.07";
	colors[1]	= "1.0 0.2 0.2 0.07";
	colors[2]	= "0.3 0.3 0.3 0.0";

	sizes[0]	= 1.15;
	sizes[1]	= 0.7;
	sizes[2]	= 0.5;
	
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.0;
	gravityCoefficient = -0.2;

	inheritedVelFactor = 1.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;

	spinSpeed = 0.0;
	spinRandomMin = -2000.0;
	spinRandomMax = 2000.0;
};
datablock ParticleEmitterData(HarvesterBeamRifleExplosionDebrisTrailEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBeamRifleExplosionDebrisTrailParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 90.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//------------------------------//
// Beam Rifle Explosion Debris: //
//------------------------------//

datablock DebrisData(HarvesterBeamRifleExplosionDebris)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = "base/data/shapes/empty.dts";
	
	emitters = "HarvesterBeamRifleExplosionDebrisTrailEmitter";
	
	staticOnMaxBounce = false;
	snapOnMaxBounce = false;
	explodeOnMaxBounce = true;
	
	fade = true;

	//-------------//
	// Properties: //
	//-------------//
	
	lifetime = 2.0;

	minSpinSpeed = 0.0;
	maxSpinSpeed = 0.0;
	
	//----------//
	// Physics: //
	//----------//
	
	gravModifier = 2.0;
	numBounces = 2;
	elasticity = 0.999;
	friction = 0.0;
};

//-----------------------//
// Beam Rifle Explosion: //
//-----------------------//

datablock ExplosionData(HarvesterBeamRifleExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionSphere1.dts";
	
	particleEmitter = HarvesterBombExplosionBlastEmitter;
	particleDensity = 4;
	particleRadius = 3.5;
	
	emitter[0] = HarvesterBeamRifleExplosionFlareEmitter;
	emitter[1] = HarvesterBeamRifleExplosionEmitter;
	emitter[2] = HarvesterBeamRifleExplosionSmokeEmitter;
	
	//---------//
	// Debris: //
	//---------//
	
	debris = HarvesterBeamRifleExplosionDebris;
	
	debrisNum = 4;
	debrisNumVariance = 2;
	
	debrisVelocity = 12;
	debrisVelocityVariance = 6;
	
	debrisThetaMin = 70;
	debrisThetaMax = 180;
	
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	shakeCamera = true;
	camShakeFreq = "2.0 4.0 2.0";
	camShakeAmp = "2.5 6.5 2.5";
	camShakeDuration = 1.0;
	camShakeRadius = 15.0;
};

datablock ProjectileData(HarvesterBeamRifleProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterBeamRifleExplosion;

	explodeOnDeath = true;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function HarvesterBeamRifleProjectile::onExplode(%this, %projectile, %position, %fade)
{
	Parent::onExplode(%this, %projectile, %position, %fade);
	serverPlay3d("HarvesterBombExplosionSound" @ getRandom(1, 2), %position);
}

//------------------//
// Beam Rifle Item: //
//------------------//

datablock ItemData(HarvesterBeamRifleItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";
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
	
	image = HarvesterBeamRifleImage;
	
	canDrop = true;
	
	uiName = "The Harvester's Last Word";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_lastWord";

	category = "Weapon";
	className = "Weapon";
};

//------------------------//
// Beam Rifle Back Image: //
//------------------------//

datablock ShapeBaseImageData(HarvesterBeamRifleBackImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";

	emap = false;
	
	doColorShift = HarvesterBeamRifleItem.doColorShift;
	colorShiftColor = HarvesterBeamRifleItem.colorShiftColor;
	
	//-----------//
	// Mounting: //
	//-----------//
	
	offset = "0.4 -0.62 0.1";
	eyeOffset = "0.0 0.0 0.0";

	rotation = eulerToQuat_degrees("-45.0 22.5 90.0");
	eyeRotation = "0.0 0.0 0.0 0.0";
	
	mountPoint = $BackSlot;
	
	//-------------//
	// Properties: //
	//-------------//
	
	item = HarvesterBeamRifleItem;

	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//---------//
	// States: //
	//---------//
	
	stateName[0] = "Activate";
	stateAllowImageChange[0] = true;
	stateScript[0] = "onActivate";
};

//-------------------//
// Beam Rifle Image: //
//-------------------//

datablock ShapeBaseImageData(HarvesterBeamRifleImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";

	emap = false;
	
	doColorShift = HarvesterBeamRifleItem.doColorShift;
	colorShiftColor = HarvesterBeamRifleItem.colorShiftColor;
	
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

	item = HarvesterBeamRifleItem;
	
	ammo = "";
	projectile = "";
	projectileType = Projectile;

	armReady = true;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//----------//
	// Hitscan: //
	//----------//
	
	hitscanRange = 64;
	hitscanTypes = $TypeMasks::PlayerObjectType | $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FXBrickObjectType;
	
	hitscanDamage = 70;
	hitscanDamageType = $DamageType::Direct;
	hitscanDamageFalloff = 0.9;
	
	hitscanProjectile = HarvesterBeamRifleProjectile;
	
	hitscanTracerStaticShape = HarvesterBeamShape;
	hitscanTracerStaticLifetime = 100;
	
	hitscanSpread = 0.0;
	hitscanShotCount = 1;
	
	hitscanPenetrate = false;
	
	hitscanFromMuzzle = true;
	
	hitscanImpactImpulse = 24;
	hitscanVerticalImpulse = 11;
	
	hitscanExplodeOnMiss = true;
	
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
	stateTimeoutValue[2] = 1.75;
	stateTransitionOnTimeout[2] = "Fire";
	stateWaitForTimeout[2] = true;
	stateEmitter[2] = HarvesterBeamRifleSpinChargeEmitter;
	stateEmitterTime[2] = 1.25;
	stateEmitterNode[2] = "muzzleNode";
	stateAllowImageChange[2] = true;
	stateSound[2] = HarvesterBeamRifleChargeSound;
	stateScript[2] = "onCharge";
	
	stateName[3] = "Fire";
	stateTimeoutValue[3] = 0.35;
	stateTransitionOnTimeout[3] = "Wait";
	stateWaitForTimeout[3] = true;
	stateEmitter[3] = HarvesterBladeEnergyEmitter;
	stateEmitterTime[3] = 0.15;
	stateEmitterNode[3] = "muzzleNode";
	stateAllowImageChange[3] = false;
	stateSound[3] = HarvesterBeamRifleFireSound;
	stateScript[3] = "onFire";
	stateFire[3] = true;
	
	stateName[4] = "Wait";
	stateTimeoutValue[4] = 0.75;
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

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onMount(%this, %player, %slot)
{
	%image = %player.getMountedImage(2);
	
	if(isObject(%image) && %image == HarvesterBeamRifleBackImage.getID())
	{
		%player.unMountImage(2);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onUnMount(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.mountImage(HarvesterBeamRifleBackImage, 2);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "root");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "gunReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		Parent::onFire(%this, %player, %slot);
		
		%player.playThread(0, "gunRecoil");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onWait(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "gunRecoilDone");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "gunDone");
	}
}