//--------------------//
// Ghost Lily Sounds: //
//--------------------//

datablock AudioProfile(GhostLilyExplosionSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/lilyExplode.wav";
	description = AudioDefault3d;
	preload = true;
};

//------------------------//
// Ancient Warrior Trail: //
//------------------------//

datablock ParticleData(AncientWarriorTrailParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/blockhead";
	
	useInvAlpha = false;
	
	colors[0]	= "0.5 0.7 1.0 0.0";
	colors[1]	= "0.5 0.7 1.0 0.15";
	colors[2]	= "0.3 0.3 1.0 0.0";

	sizes[0]	= 3.15;
	sizes[1]	= 3.15;
	sizes[2]	= 2.50;

	times[0]	= 0.0;
	times[1]	= 0.2;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 0;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(AncientWarriorTrailEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "AncientWarriorTrailParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 22;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.08;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 89.0;
	thetaMax = 90.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 0.0;
};

//-------------------//
// Ghost Lily Bloom: //
//-------------------//

datablock ParticleData(GhostLilyBloomParticle : BloodLilyBloomParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	colors[0]	= "0.4 0.6 1.0 0.0";
	colors[1]	= "0.5 0.7 1.0 1.0";
	colors[2]	= "0.4 0.6 1.0 0.5";
	colors[3]	= "0.3 0.3 1.0 0.0";

	sizes[0]	= 1.0;
	sizes[1]	= 4.0;
	sizes[2]	= 3.5;
	sizes[3]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = -0.01;

	constantAcceleration = 4.0;

	lifetimeMS = 1500;
	lifetimeVarianceMS = 200;
};
datablock ParticleEmitterData(GhostLilyBloomEmitter : BloodLilyBloomEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "GhostLilyBloomParticle";
	
	//-------------//
	// Properties: //
	//-------------//
	
	ejectionVelocity = 0.2;
	
	ejectionOffset = 1.6;
};

//--------------------//
// Ghost Lily Petals: //
//--------------------//

datablock ParticleData(GhostLilyPetalParticle : BloodLilyPetalParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	colors[0]	= "0.5 0.7 1.0 0.0";
	colors[1]	= "0.5 0.7 1.0 1.0";
	colors[2]	= "0.5 0.7 1.0 1.0";
	colors[3]	= "0.5 0.7 1.0 0.0";
	
	sizes[0]	= 1.0;
	sizes[1]	= 1.0;
	sizes[2]	= 1.0;
	sizes[3]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.5;
	gravityCoefficient = -0.01;

	lifetimeMS = 3072;
	lifetimeVarianceMS = 0;

	spinSpeed = 7.5;
	spinRandomMin = -180.0;
	spinRandomMax = 270.0;
};
datablock ParticleEmitterData(GhostLilyPetalEmitter : BloodLilyPetalEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "GhostLilyPetalParticle";
	
	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 6;
	
	ejectionVelocity = 15.0;
	
	ejectionOffset = 0.2;
};

//-------------//
// Ghost Lily: //
//-------------//

datablock ExplosionData(GhostLilyExplosion : BloodLilyExplosion)
{
	//------------//
	// Rendering: //
	//------------//

	particleEmitter = GhostLilyBloomEmitter;
	particleDensity = 15;
	particleRadius = 0.1;

	emitter[0] = GhostLilyPetalEmitter;
	
	//-------------//
	// Properties: //
	//-------------//
	
	soundProfile = GhostLilyExplosionSound;
	
	lifeTimeMS = 100;
};

datablock ProjectileData(GhostLilyProjectile : BloodLilyProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = GhostLilyExplosion;
};

//------------------------------//
// Ancient Warrior Trail Image: //
//------------------------------//

datablock ShapeBaseImageData(AncientWarriorTrailImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = "base/data/shapes/empty.dts";

	emap = false;
	
	doColorShift = false;

	//-----------//
	// Mounting: //
	//-----------//
	
	offset = "0.0 0.0 -0.5";
	eyeOffset = "0.0 0.0 0.0";

	rotation = "0.0 0.0 0.0 0.0";
	eyeRotation = "0.0 0.0 0.0 0.0";
	
	mountPoint = $BackSlot;

	//---------//
	// States: //
	//---------//
	
	stateName[0] = "Activate";
	stateTimeoutValue[0] = inf;
	stateTransitionOnTimeout[0] = "Activate";
	stateEmitter[0] = AncientWarriorTrailEmitter;
	stateEmitterTime[0] = inf;
	stateEmitterNode[0] = "muzzlePoint";
	stateWaitForTimeout[0] = true;
	stateAllowImageChange[0] = true;
};

//-------------//
// Playertype: //
//-------------//

datablock PlayerData(AncientWarriorArmor : PlayerStandardArmor)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/harvester.dts";
	
	//-----------//
	// Gameplay: //
	//-----------//

	maxDamage = $Harvester::AncientWarrior::Armor::BaseHealth;

	useCustomPainEffects = true;
	
	//----------//
	// Physics: //
	//----------//

	mass = 115.0;
	drag = 0.1;
	density = 0.85;

	//-----------//
	// Movement: //
	//-----------//

	canJet = false;

	//----------------//
	// Miscellaneous: //
	//----------------//

	uiName = "Ancient Warrior";
};

/// @param	this	playertype
/// @param	player	player
function AncientWarriorArmor::applyAvatar(%this, %player)
{
	%player.setNodeColor("ALL", $Harvester::AncientWarrior::Armor::Avatar::Color);
	%player.startFade(0, 0, true);
	
	%player.hideNode("Helmet");
	%player.hideNode("ArmorMount");
	%player.hideNode("ShoulderBars");
	%player.hideNode("ShoulderArmor");
	%player.hideNode("TorsoBars");
	%player.hideNode("TorsoArmor");
}

/// @param	this	playertype
/// @param	player	player
/// @param	state	string
function AncientWarriorArmor::onDisabled(%this, %player, %state)
{
	if(%player.isAncientWarrior)
	{
		%effect = new Projectile()
		{
			dataBlock = GhostLilyProjectile;
			initialVelocity = %player.getForwardVector();
			initialPosition = %player.getHackPosition();
			sourceObject = %player;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
	
		%player.schedule(0, delete);
	}
	
	return Parent::onDisabled(%this, %player, %state);
}