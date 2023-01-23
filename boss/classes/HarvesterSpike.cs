//---------------//
// Spike Sounds: //
//---------------//

datablock AudioProfile(HarvesterSpikeExplosionSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/rockExplode1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterSpikeExplosionSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/rockExplode2.wav";
	description = AudioDefault3d;
	preload = true;
};

//--------//
// Spike: //
//--------//

datablock StaticShapeData(HarvesterSpikeShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/spike.dts";
};

//-------------//
// Spike Ring: //
//-------------//

datablock ParticleData(HarvesterSpikeRingParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/fadeRing";
	
	useInvAlpha = true;
	
	colors[0]	= "0.0 0.0 0.0 1.0";
	colors[1]	= "0.0 0.0 0.0 0.0";
	
	sizes[0]	= 0.5;
	sizes[1]	= 7.0;

	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 200;
	lifetimeVarianceMS = 0;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterSpikeRingEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterSpikeRingParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = HarvesterSpikeRingParticle.lifetimeMS;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//--------------//
// Spike Spark: //
//--------------//

datablock ParticleData(HarvesterSpikeSparkParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/hitspark";
	
	useInvAlpha = false;

	colors[0]	= "1.0 1.0 1.0 0.7";
	colors[1]	= "1.0 1.0 1.0 0.7";

	sizes[0]	= 0.75;
	sizes[1]	= 1.5;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 130;
	lifetimeVarianceMS = 20;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterSpikeSparkEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterSpikeSparkParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 18;
	periodVarianceMS = 0;
	
	ejectionVelocity = 20.0;
	velocityVariance = 2.0;
	
	ejectionOffset = 1.0;
	
	thetaMin = 20.0;
	thetaMax = 55.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//--------------//
// Spike Chunk: //
//--------------//

datablock ParticleData(HarvesterSpikeChunkParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/chunk";
	
	useInvAlpha = true;

	colors[0]	= "0.2 0.2 0.2 1.0";
	colors[1]	= "0.1 0.1 0.1 1.0";

	sizes[0]	= 0.75;
	sizes[1]	= 0.0;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.2;
	gravityCoefficient = 1.7;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1500;
	lifetimeVarianceMS = 300;

	spinSpeed = 1000.0;
	spinRandomMin = -125.0;
	spinRandomMax = 125.0;
};
datablock ParticleEmitterData(HarvesterSpikeChunkEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterSpikeChunkParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 22;
	periodVarianceMS = 4;
	
	ejectionVelocity = 10.0;
	velocityVariance = 2.0;
	
	ejectionOffset = 0.5;
	
	thetaMin = 0.0;
	thetaMax = 45.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//--------//
// Spike: //
//--------//

datablock ExplosionData(HarvesterSpikeExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	particleEmitter = HarvesterSpikeRingEmitter;
	particleDensity = 1;
	particleRadius = 0.0;
	
	emitter[0] = HarvesterSpikeSparkEmitter;
	emitter[1] = HarvesterSpikeChunkEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
	
	damageRadius = $Harvester::Spike::Radius;
	radiusDamage = $Harvester::Spike::RadiusDamage;

	impulseRadius = $Harvester::Spike::Radius;
	impulseForce = 1000.0;
	
	shakeCamera = true;
	camShakeFreq = "5.0 6.0 5.0";
	camShakeAmp = "1.0 3.2 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10;
};

datablock ProjectileData(HarvesterSpikeProjectile)
{
	//-------------//
	// Properties: //
	//-------------//

	radiusDamageType = $DamageType::Direct;	
	
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterSpikeExplosion;

	explodeOnDeath = true;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function HarvesterSpikeProjectile::onExplode(%this, %projectile, %position, %fade)
{
	Parent::onExplode(%this, %projectile, %position, %fade);
	serverPlay3d("HarvesterSpikeExplosionSound" @ getRandom(1, 2), %position);
}

//-------------//
// Spike Item: //
//-------------//

datablock ItemData(HarvesterSpikeItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/silence.dts";
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
	
	image = HarvesterSpikeImage;
	
	canDrop = true;
	
	uiName = "The Harvester's Silence";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_silence";

	category = "Weapon";
	className = "Weapon";
};

//--------------//
// Spike Image: //
//--------------//

datablock ShapeBaseImageData(HarvesterSpikeImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/silence.dts";

	emap = false;
	
	doColorShift = HarvesterSpikeItem.doColorShift;
	colorShiftColor = HarvesterSpikeItem.colorShiftColor;
	
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

	item = HarvesterSpikeItem;
	
	ammo = "";
	projectile = "";
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
	stateTimeoutValue[2] = 0.75;
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
	stateTimeoutValue[6] = 0.15;
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
/// @param	iteration	number
/// @param	max			number
function HarvesterSpikeImage::iterate(%this, %player, %slot, %position, %direction, %iteration, %max)
{
	if(%player.getDamagePercent() < 1.0)
	{
		if(%iteration > %max)
		{
			return;
		}
		
		%typeMasks = $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FXBrickObjectType;
		
		%end = vectorSub(%position, "0.0 0.0 3.0");
		%groundRay = containerRaycast(%position, %end, %typeMasks);
		
		if(!isObject(firstWord(%groundRay)))
		{
			return;
		}
		
		%groundPosition = getWords(%groundRay, 1, 3);
		%middle = vectorAdd(%groundPosition, "0.0 0.0 0.4");
		%forward = vectorAdd(%middle, %direction);
		%wallRay = containerRaycast(%middle, %forward, %typeMasks);
		
		if(isObject(firstWord(%wallRay)))
		{
			return;
		}
		
		%scale = getRandom() + 1.0;
		
		if(%iteration == %max)
		{
			%scale = 3.0;
		}
		
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
			dataBlock = HarvesterSpikeProjectile;
			initialVelocity = "0.0 0.0 1.0";
			initialPosition = %groundPosition;
			scale = %scale SPC %scale SPC %scale;
			sourceObject = %player;
			sourceSlot = %slot;
			client = %player.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
		
		%this.schedule($Harvester::Spike::IterationTimeMS, iterate, %player, %slot, %forward, %direction, %iteration + 1, %max);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterSpikeImage::onReady(%this, %player, %slot)
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
function HarvesterSpikeImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "slamReady");
		%player.playAudio(1, HarvesterChargeSound);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterSpikeImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%direction = vectorScale(%player.getForwardVector(), $Harvester::Spike::IterationLength);
		%position = vectorAdd(vectorAdd(%player.position, "0.0 0.0 0.4"), %direction);
		
		%this.schedule($Harvester::Spike::IterationTimeMS, iterate, %player, %slot, %position, %direction, 0, $Harvester::Spike::MaxIterations);
		
		%effect = new Projectile()
		{
			dataBlock = HarvesterSpikeProjectile;
			initialVelocity = "0.0 0.0 1.0";
			initialPosition = %player.position;
			scale = "2.0 2.0 2.0";
			sourceObject = %player;
			sourceSlot = %slot;
			client = %player.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
		
		%player.playThread(0, "slamAttack");
		%player.playAudio(1, "HarvesterYellSound" @ getRandom(1, 2));
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterSpikeImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "slamDone");
	}
}