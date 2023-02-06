//--------------------//
// Ghost Lily Sounds: //
//--------------------//

datablock AudioProfile(GhostLilyExplosionSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/lilyExplode.wav";
	description = AudioDefault3d;
	preload = true;
};

//-------------------//
// Ghost Lily Bloom: //
//-------------------//

datablock ParticleData(GhostLilyBloomParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/lilyPetal";
	
	useInvAlpha = false;
	
	colors[0]	= "0.4 0.6 1.0 0.0";
	colors[1]	= "0.5 0.7 1.0 1.0";
	colors[2]	= "0.4 0.6 1.0 0.5";
	colors[3]	= "0.3 0.3 1.0 0.0";

	sizes[0]	= 1.0;
	sizes[1]	= 4.0;
	sizes[2]	= 3.5;
	sizes[3]	= 1.0;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.4;
	times[3]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = -0.01;

	inheritedVelFactor = 0.0;
	constantAcceleration = 4.0;

	lifetimeMS = 1500;
	lifetimeVarianceMS = 200;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(GhostLilyBloomEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "GhostLilyBloomParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;
	
	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 100;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.2;
	velocityVariance = 0.1;
	
	ejectionOffset = 1.6;
	
	thetaMin = 20.0;
	thetaMax = 130.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//--------------------//
// Ghost Lily Petals: //
//--------------------//

datablock ParticleData(GhostLilyPetalParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/smallPetal";
	
	useInvAlpha = false;
	
	colors[0]	= "0.5 0.7 1.0 0.0";
	colors[1]	= "0.5 0.7 1.0 1.0";
	colors[2]	= "0.5 0.7 1.0 1.0";
	colors[3]	= "0.5 0.7 1.0 0.0";
	
	sizes[0]	= 1.0;
	sizes[1]	= 1.0;
	sizes[2]	= 1.0;
	sizes[3]	= 1.0;
	
	times[0]	= 0.05;
	times[1]	= 0.1;
	times[2]	= 0.95;
	times[3]	= 1.0;
	
	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.5;
	windCoefficient = 5.0;
	gravityCoefficient = -0.01;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 3072;
	lifetimeVarianceMS = 0;

	spinSpeed = 7.5;
	spinRandomMin = -180.0;
	spinRandomMax = 270.0;
};
datablock ParticleEmitterData(GhostLilyPetalEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "GhostLilyPetalParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 6;
	periodVarianceMS = 0;
	
	ejectionVelocity = 15.0;
	velocityVariance = 5.5;
	
	ejectionOffset = 0.2;
	
	thetaMin = 20.0;
	thetaMax = 130.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-------------//
// Ghost Lily: //
//-------------//

datablock ExplosionData(GhostLilyExplosion)
{
	//------------//
	// Rendering: //
	//------------//

	particleEmitter = "GhostLilyBloomEmitter";
	particleDensity = 15;
	particleRadius = 0.1;

	emitter[0] = "GhostLilyPetalEmitter";
	
	//-------------//
	// Properties: //
	//-------------//
	
	soundProfile = "GhostLilyExplosionSound";
	
	lifeTimeMS = 100;
};

datablock ProjectileData(GhostLilyProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = "GhostLilyExplosion";
	
	explodeOnDeath = true;
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