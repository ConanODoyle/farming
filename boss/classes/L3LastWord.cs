//-------------//
// Water Beam: //
//-------------//

datablock StaticShapeData(L3LastWordWaterBeamShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/waterBeam.dts";
};

//-------------//
// Water Ring: //
//-------------//

datablock ParticleData(L3LastWordWaterRingParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/bubble";

	useInvAlpha = false;

	colors[0]	= "0.5 0.7 1.0 0.3";
	colors[1]	= "0.3 0.5 0.8 0.0";

	sizes[0]	= 3.0;
	sizes[1]	= 14.0;

	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 500;
	lifetimeVarianceMS = 200;

	spinSpeed = 1700.0;
	spinRandomMin = -300.0;
	spinRandomMax = 300.0;
};

datablock ParticleEmitterData(L3LastWordWaterRingEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "L3LastWordWaterRingParticle";
	
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
	
	thetaMin = 89.0;
	thetaMax = 90.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 0.0;
};

//---------------------//
// Water Debris Trail: //
//---------------------//

datablock ParticleData(L3LastWordWaterDebrisTrailParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = "base/data/particles/bubble";

	useInvAlpha = false;

	colors[0]	= "0.666667 0.8 1.0 0.8";
	colors[1]	= "0.666667 0.8 0.8 0.0";

	sizes[0]	= 0.5;
	sizes[1]	= 0.5;

	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = 0.998779;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1200;
	lifetimeVarianceMS = 400;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};

datablock ParticleEmitterData(L3LastWordWaterDebrisTrailEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "L3LastWordWaterDebrisTrailParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 33;
	periodVarianceMS = 11;
	
	ejectionVelocity = 0.5;
	velocityVariance = 0.5;
	
	ejectionOffset = 0.0;
	
	thetaMin = 89.0;
	thetaMax = 90.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 0.0;
};

//-------------------------//
// Water Explosion Debris: //
//-------------------------//

datablock DebrisData(L3LastWordWaterDebris)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = "base/data/shapes/empty.dts";
	
	emitters = "L3LastWordWaterDebrisTrailEmitter";
	
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

//------------------//
// Water Explosion: //
//------------------//

datablock ExplosionData(L3LastWordWaterExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionSphere1.dts";
	
	particleEmitter = L3LastWordWaterRingEmitter;
	particleDensity = 12;
	particleRadius = 0;

	emitter[0] = PlayerBubbleEmitter;
	
	//---------//
	// Debris: //
	//---------//
	
	debris = L3LastWordWaterDebris;
	
	debrisNum = 4;
	debrisNumVariance = 2;
	
	debrisVelocity = 12.0;
	debrisVelocityVariance = 6.0;
	
	debrisThetaMin = 70.0;
	debrisThetaMax = 180.0;
	
	debrisPhiMin = 0.0;
	debrisPhiMax = 360.0;
	
	//-------------//
	// Properties: //
	//-------------//
	
	soundProfile = Splash1Sound;
	
	lifeTimeMS = 150;
};

datablock ProjectileData(L3LastWordWaterProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = L3LastWordWaterExplosion;
};

//---------------//
// Water Splash: //
//---------------//

datablock SplashData(L3LastWordWaterSplash : PlayerSplash)
{
	ejectionAngle = 45.0;
	ringLifetime = 0.85;
	lifetimeMS = 400;
	velocity = 7.2;
	startRadius = 0.3;
};

//------------------------//
// L3 - "Last Word" Item: //
//------------------------//

datablock ItemData(L3LastWordItem)
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
	
	image = L3LastWordImage;
	
	canDrop = true;
	
	uiName = "L3 - \"Last Word\"";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_lastWord";

	category = "Weapon";
	className = "Weapon";
	
	//----------//
	// Farming: //
	//----------//
	
	durability = 10000;
	isBossReward = 1;

	hasDataID = 1;
	isDataIDTool = 1;
};

//-------------------------//
// L3 - "Last Word" Image: //
//-------------------------//

datablock ShapeBaseImageData(L3LastWordImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";

	emap = false;
	
	doColorShift = L3LastWordItem.doColorShift;
	colorShiftColor = L3LastWordItem.colorShiftColor;
	
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

	item = L3LastWordItem;
	
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
	
	hitscanRange = $Harvester::BeamRifle::Range;
	hitscanTypes = $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FXBrickObjectType;
	
	hitscanDamage = 0;
	hitscanDamageType = $DamageType::Direct;
	hitscanDamageFalloff = 0;
	
	hitscanProjectile = L3LastWordWaterProjectile;
	
	hitscanTracerStaticShape = L3LastWordWaterBeamShape;
	hitscanTracerStaticLifetime = 100;
	
	hitscanSpread = 0.0;
	hitscanShotCount = 1;
	
	hitscanPenetrate = false;
	
	hitscanFromMuzzle = true;
	
	hitscanImpactImpulse = 0.0;
	hitscanVerticalImpulse = 0.0;
	
	hitscanExplodeOnMiss = true;
	
	//----------//
	// Farming: //
	//----------//
	
	waterRange = $Harvester::BeamRifle::Range;
	tankAmount = 1000;
	waterAmount = 1000;
	
	toolTip = "Waters Dirt: +1000 | Tanks: +1000";
	
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
	stateWaitForTimeout[1] = false;
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "Ready2";
	stateAllowImageChange[1] = true;
	stateScript[1] = "onReady";

	stateName[7] = "Ready2";
	stateTransitionOnTriggerDown[7] = "Charge";
	stateWaitForTimeout[7] = false;
	stateTimeoutValue[7] = 0.1;
	stateTransitionOnTimeout[7] = "Ready";
	stateAllowImageChange[7] = true;
	stateScript[7] = "onReady";
	
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
function L3LastWordImage::onReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		wateringCanReady(%this, %player, %slot);
	}
}

/// @param	this		projectile datablock
/// @param	player		player
/// @param	slot		number
/// @param	collision	object
/// @param	position	3-element position
/// @param	normal		3-element vector
/// @param	vector		3-element vector
/// @param	crit		boolean
function L3LastWordImage::onHitscanExplode(%this, %player, %slot, %collision, %position, %normal, %vector, %crit)
{
	Parent::onHitscanExplode(%this, %player, %slot, %collision, %position, %normal, %vector, %crit);
	
	// %splash = new Splash()
	// {
		// dataBlock = L3LastWordWaterSplash;
		// position = %position;
		// rotation = eulerToQuat_degrees(relativeVectorToRotation(%normal, %player.getUpVector()));
	// };

	// if(isObject(%splash))
	// {
		// MissionCleanup.add(%splash);
		// %splash.setScopeAlways();
	// }
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L3LastWordImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		Parent::onFire(%this, %player, %slot);
		waterCanFire(%this, %player, %slot);
		
		%player.playThread(0, "jump");
		%player.playThread(2, "activate");
	}
}