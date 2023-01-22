//----------------------//
// Cluster Bomb Sounds: //
//----------------------//

datablock AudioProfile(HarvesterBombExplodeSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/clusterBombExplode1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterBombExplodeSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/clusterBombExplode2.wav";
	description = AudioDefault3d;
	preload = true;
};

//--------//
// Blast: //
//--------//

datablock ParticleData(HarvesterBombExplosionBlastFrame1Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast1";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame2Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast2";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame3Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast3";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame4Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast4";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame5Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast5";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame6Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast6";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame7Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast7";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame8Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast8";
};
datablock ParticleData(HarvesterBombExplosionBlastFrame9Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/blast/blast9";
};
datablock ParticleData(HarvesterBombExplosionBlastParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/blast/blast1";

	animTexName[0]	= $Harvester::Root @ "/resources/particles/blast/blast1";
	animTexName[1]	= $Harvester::Root @ "/resources/particles/blast/blast2";
	animTexName[2]	= $Harvester::Root @ "/resources/particles/blast/blast3";
	animTexName[3]	= $Harvester::Root @ "/resources/particles/blast/blast4";
	animTexName[4]	= $Harvester::Root @ "/resources/particles/blast/blast5";
	animTexName[5]	= $Harvester::Root @ "/resources/particles/blast/blast6";
	animTexName[6]	= $Harvester::Root @ "/resources/particles/blast/blast7";
	animTexName[7]	= $Harvester::Root @ "/resources/particles/blast/blast8";
	animTexName[8]	= $Harvester::Root @ "/resources/particles/blast/blast9";
	
	animateTexture = true;
	framesPerSec = 30;
	
	useInvAlpha = false;

	colors[0]	= "1.0 1.0 1.0 1";
	colors[1]	= "1.0 0.8 0.5 1";

	sizes[0]	= 2.0;
	sizes[1]	= 4.0;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1;
	gravityCoefficient = 0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 300;
	lifetimeVarianceMS = 0;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(HarvesterBombExplosionBlastEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBombExplosionBlastParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = HarvesterBombExplosionBlastParticle.lifetimeMS;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0;
	velocityVariance = 0;
	
	ejectionOffset = 0;
	
	thetaMin = 89;
	thetaMax = 90;
	
	phiReferenceVel = 0;
	phiVariance = 360;
};

//-------------//
// Bomb Flare: //
//-------------//

datablock ParticleData(HarvesterBombExplosionFlareParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName =  $Harvester::Root @ "/resources/particles/blastFlare";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.9 0.8 0.6";
	colors[1]	= "1.0 0.5 0.2 0.4";
	colors[2]	= "1.0 0.4 0.1 0.0";
	
	sizes[0]	= 15;
	sizes[1]	= 30;
	sizes[2]	= 10;

	times[0]	= 0.0;
	times[1]	= 0.02;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 300;
	lifetimeVarianceMS = 120;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBombExplosionFlareEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBombExplosionFlareParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 11;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.2;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-----------------//
// Bomb Explosion: //
//-----------------//

datablock ParticleData(HarvesterBombExplosionParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/cloud";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 1.0 1.0 0.3";
	colors[1]	= "0.9 0.5 0.0 0.6";
	colors[2]	= "0.1 0.05 0.025 0.1";
	colors[3]	= "0.1 0.05 0.025 0.0";

	sizes[0]	= 3.0;
	sizes[1]	= 5.3;
	sizes[2]	= 5.5;
	sizes[3]	= 3.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.0;
	gravityCoefficient = -1.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 800;
	lifetimeVarianceMS = 100;

	spinSpeed = 25.0;
	spinRandomMin = -25.0;
	spinRandomMax = 25.0;
};
datablock ParticleEmitterData(HarvesterBombExplosionEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBombExplosionParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 4;
	periodVarianceMS = 0;
	
	ejectionVelocity = 3.0;
	velocityVariance = 2.0;
	
	ejectionOffset = 2.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-------------//
// Bomb Smoke: //
//-------------//

datablock ParticleData(HarvesterBombExplosionSmokeParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/cloud";
	
	useInvAlpha = true;
	
	colors[0]	= "0.95 0.9 0.8 0.8";
	colors[1]	= "0.1 0.05 0.025 0.6";
	colors[2]	= "0.1 0.05 0.025 0.0";

	sizes[0]	= 6.0;
	sizes[1]	= 9.0;
	sizes[2]	= 9.5;

	times[0]	= 0.0;
	times[1]	= 0.02;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 5.0;
	gravityCoefficient = -0.5;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 400;
	lifetimeVarianceMS = 100;

	spinSpeed = 5.0;
	spinRandomMin = -5.0;
	spinRandomMax = 5.0;
};
datablock ParticleEmitterData(HarvesterBombExplosionSmokeEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBombExplosionSmokeParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 4;
	periodVarianceMS = 0;
	
	ejectionVelocity = 7.0;
	velocityVariance = 2.0;
	
	ejectionOffset = 1.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 30.0;
	phiVariance = 32.0;
};

//-------//
// Bomb: //
//-------//

datablock ExplosionData(HarvesterBombExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionSphere1.dts";
	
	particleEmitter = HarvesterBombExplosionBlastEmitter;
	particleDensity = 4;
	particleRadius = 3.5;
	
	emitter[0] = HarvesterBombExplosionFlareEmitter;
	emitter[1] = HarvesterBombExplosionEmitter;
	emitter[2] = HarvesterBombExplosionSmokeEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	damageRadius = 7.0;
	radiusDamage = 50.0;

	impulseRadius = 7.0;
	impulseForce = 2000.0;
	
	shakeCamera = true;
	camShakeFreq = "2.0 4.0 2.0";
	camShakeAmp = "2.5 6.5 2.5";
	camShakeDuration = 1.0;
	camShakeRadius = 15.0;
};
datablock ProjectileData(HarvesterBombProjectile)
{
	//------------//
	// Rendering: //
	//------------//
	
	projectileShapeName = $Harvester::Root @ "/resources/shapes/bombProjectile.dts";
	
	hasLight = false;
	
	//-------------//
	// Properties: //
	//-------------//
	
	directDamage = 0.0;
	directDamageType = $DamageType::Direct;
	radiusDamageType = $DamageType::Direct;	
	
	impactImpulse = 0.0;
	verticalImpulse = 0.0;
	
	muzzleVelocity = 12.0;
	
	//----------//
	// Physics: //
	//----------//
	
	armingDelay = 1500;
	lifetime = 1500;
	fadeDelay = 0;
	
	isBallistic = true;
	gravityMod = 1;
	bounceElasticity = 0.9;
	bounceFriction = 0.1;
	
	ballRadius = 0.2;
	
	//------------//
	// Explosion: //
	//------------//

	explosion = HarvesterBombExplosion;
	
	explodeOnPlayerImpact = false;
	explodeOnDeath = true;
	
	brickExplosionRadius = 0.0;
	brickExplosionImpact = false;
	brickExplosionForce = 0.0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;
	
	//---------------//
	// Miscellanous: //
	//---------------//
	
	uiName = "The Harvester's Bomb";
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function HarvesterBombProjectile::onExplode(%this, %projectile, %position, %fade)
{
	Parent::onExplode(%this, %projectile, %position, %fade);
	serverPlay3d("HarvesterBombExplodeSound" @ getRandom(1, 2), %position);
}

//---------------//
// Cluster Bomb: //
//---------------//

datablock ProjectileData(HarvesterClusterBombProjectile : HarvesterBombProjectile)
{
	//------------//
	// Rendering: //
	//------------//
	
	projectileShapeName = $Harvester::Root @ "/resources/shapes/bomb3Projectile.dts";
	
	//----------//
	// Physics: //
	//----------//
	
	ballRadius = 0.4;
	
	//---------------//
	// Miscellanous: //
	//---------------//
	
	uiName = "The Harvester's Cluster Bomb";
	
	subProjectile = HarvesterBombProjectile;
	subProjectileCount = 3;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function HarvesterClusterBombProjectile::onExplode(%this, %projectile, %position, %fade)
{
	Parent::onExplode(%this, %projectile, %position, %fade);
	serverPlay3d("HarvesterBombExplodeSound" @ getRandom(1, 2), %position);
}

/// @param	alpha	number (in degrees)
/// @param	beta	number (in degrees)
/// @return	normalized 3-element vector
function unitVectorFromAngles(%alpha, %beta)
{
	%alpha = mDegToRad(%alpha);
	%beta = mDegToRad(%beta);
	
	%x = mSin(%alpha) * mCos(%beta);
	%y = mCos(%alpha) * mCos(%beta);
	%z = mSin(%beta);
	
	return %x SPC %y SPC %z;
}

/// @param	this		projectile datablock
/// @param	projectile	projectile
function HarvesterClusterBombProjectile::split(%this, %projectile)
{
	if(isObject(%projectile))
	{
		%adjustment[0] = unitVectorFromAngles(22.5, 0);
		%adjustment[1] = unitVectorFromAngles(0, 22.5);
		%adjustment[2] = unitVectorFromAngles(-22.5, 0);
			
		for(%i = 0; %i < %this.subProjectileCount; %i++)
		{	
			%rotation = relativeVectorToRotation(%projectile.initialVelocity, %projectile.getUpVector());
			%local = %projectile.position SPC %rotation;
			%actual = matrixMulVector(%local, %adjustment[%i]);
			%velocity = vectorScale(%actual, %this.subProjectile.muzzleVelocity);
			%velocity = randomVectorNudge(%velocity, 0.05);
				
			%split = new Projectile()
			{
				dataBlock = %this.subProjectile;
				initialVelocity = %velocity;
				initialPosition = %projectile.position;
				sourceObject = %projectile.sourceObject;
				sourceSlot = %projectile.sourceSlot;
				client = %projectile.client;
			};

			if(isObject(%split))
			{
				MissionCleanup.add(%split);
			}
		}
		
		// Spawn a projectile to compensate for Torque's fuckery (.explode() does not spawn explosion at correct position after a bounce).
		%effect = new Projectile()
		{
			dataBlock = HarvesterBombProjectile;
			initialVelocity = "0 0 1";
			initialPosition = %projectile.position;
			sourceObject = %projectile.sourceObject;
			sourceSlot = %projectile.sourceSlot;
			client = %projectile.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
		
		%projectile.delete();
	}
}

//-------------------//
// Big Cluster Bomb: //
//-------------------//

datablock ProjectileData(HarvesterBigClusterBombProjectile : HarvesterClusterBombProjectile)
{
	//------------//
	// Rendering: //
	//------------//
	
	projectileShapeName = $Harvester::Root @ "/resources/shapes/bomb9Projectile.dts";
	
	//----------//
	// Physics: //
	//----------//
	
	ballRadius = 0.6;
	
	//---------------//
	// Miscellanous: //
	//---------------//
	
	uiName = "The Harvester's Big Cluster Bomb";
	
	subProjectile = HarvesterClusterBombProjectile;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function HarvesterBigClusterBombProjectile::onExplode(%this, %projectile, %position, %fade)
{
	Parent::onExplode(%this, %projectile, %position, %fade);
	serverPlay3d("HarvesterBombExplodeSound" @ getRandom(1, 2), %position);
}

/// @param	this		projectile datablock
/// @param	projectile	projectile
function HarvesterBigClusterBombProjectile::split(%this, %projectile)
{
	if(isObject(%projectile))
	{
		%adjustment[0] = unitVectorFromAngles(22.5, 0);
		%adjustment[1] = unitVectorFromAngles(0, 22.5);
		%adjustment[2] = unitVectorFromAngles(-22.5, 0);
			
		for(%i = 0; %i < %this.subProjectileCount; %i++)
		{	
			%rotation = relativeVectorToRotation(%projectile.initialVelocity, %projectile.getUpVector());
			%local = %projectile.position SPC %rotation;
			%actual = matrixMulVector(%local, %adjustment[%i]);
			%velocity = vectorScale(%actual, %this.subProjectile.muzzleVelocity);
			%velocity = randomVectorNudge(%velocity, 0.05);
				
			%split = new Projectile()
			{
				dataBlock = %this.subProjectile;
				initialVelocity = %velocity;
				initialPosition = %projectile.position;
				sourceObject = %projectile.sourceObject;
				sourceSlot = %projectile.sourceSlot;
				client = %projectile.client;
			};

			if(isObject(%split))
			{
				MissionCleanup.add(%split);
			}
		}
		
		// Spawn a projectile to compensate for Torque's fuckery (.explode() does not spawn explosion at correct position after a bounce).
		%effect = new Projectile()
		{
			dataBlock = HarvesterBombProjectile;
			initialVelocity = "0 0 1";
			initialPosition = %projectile.position;
			sourceObject = %projectile.sourceObject;
			sourceSlot = %projectile.sourceSlot;
			client = %projectile.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
		
		%projectile.delete();
	}
}

//--------------------//
// Cluster Bomb Item: //
//--------------------//

datablock ItemData(HarvesterClusterBombItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/bomb3Image.dts";
	emap = false;
	
	doColorShift = false;
	
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
	
	image = HarvesterClusterBombImage;
	
	canDrop = true;
	
	uiName = "The Harvester's Penance (x3)";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_penance";

	category = "Weapon";
	className = "Weapon";
};

//---------------------//
// Cluster Bomb Image: //
//---------------------//

datablock ShapeBaseImageData(HarvesterClusterBombImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/bomb3Image.dts";

	emap = false;
	
	doColorShift = HarvesterClusterBombItem.doColorShift;
	colorShiftColor = HarvesterClusterBombItem.colorShiftColor;
	
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

	item = HarvesterClusterBombItem;
	
	ammo = "";
	projectile = HarvesterClusterBombProjectile;
	projectileType = Projectile;

	armReady = true;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
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
	stateTransitionOnTriggerUp[2] = "Fire";
	stateAllowImageChange[2] = false;
	stateScript[2] = "onCharge";
	
	stateName[3] = "Fire";
	stateTimeoutValue[3] = 0.45;
	stateTransitionOnTimeout[3] = "Done";
	stateWaitForTimeout[3] = true;
	stateAllowImageChange[3] = false;
	stateSound[3] = HarvesterClusterBombFireSound;
	stateScript[3] = "onFire";
	stateFire[3] = true;
	
	stateName[4] = "Done";
	stateTimeoutValue[4] = 0.25;
	stateTransitionOnTimeout[4] = "Ready";
	stateWaitForTimeout[4] = true;
	stateAllowImageChange[4] = false;
	stateScript[4] = "onDone";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterClusterBombImage::onReady(%this, %player, %slot)
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
function HarvesterClusterBombImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterClusterBombImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		Parent::onFire(%this, %player, %slot);
		
		%player.playThread(0, "sweepAttack");
		%player.playAudio(1, "HarvesterSmallYellSound" @ getRandom(1, 2));
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterClusterBombImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepDone");
	}
}

//------------------------//
// Big Cluster Bomb Item: //
//------------------------//

datablock ItemData(HarvesterBigClusterBombItem : HarvesterClusterBombItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/bomb9Image.dts";
	
	//-------------//
	// Properties: //
	//-------------//
	
	image = HarvesterBigClusterBombImage;
	
	uiName = "The Harvester's Penance (x9)";
};

//-------------------------//
// Big Cluster Bomb Image: //
//-------------------------//

datablock ShapeBaseImageData(HarvesterBigClusterBombImage : HarvesterClusterBombImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/bomb9Image.dts";
	
	doColorShift = HarvesterBigClusterBombItem.doColorShift;
	colorShiftColor = HarvesterBigClusterBombItem.colorShiftColor;
	
	//-------------//
	// Properties: //
	//-------------//

	item = HarvesterBigClusterBombItem;

	projectile = HarvesterBigClusterBombProjectile;
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBigClusterBombImage::onReady(%this, %player, %slot)
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
function HarvesterBigClusterBombImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBigClusterBombImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		Parent::onFire(%this, %player, %slot);
		
		%player.playThread(0, "sweepAttack");
		%player.playAudio(1, "HarvesterSmallYellSound" @ getRandom(1, 2));
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBigClusterBombImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepDone");
	}
}

package HarvesterClusterBombSplitting
{
	/// @param	this	projectile datablock
	function Projectile::onAdd(%this)
	{
		Parent::onAdd(%this);
		
		%datablock = %this.getDatablock();
		
		if(%datablock == HarvesterClusterBombProjectile.getID() || %datablock == HarvesterBigClusterBombProjectile.getID())
		{
			%datablock.scheduleNoQuota(getRandom(750, 1250), split, %this);
		}
	}
};
activatePackage(HarvesterClusterBombSplitting);