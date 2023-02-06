//----------------//
// Attack Sounds: //
//----------------//

datablock AudioProfile(HarvesterYellSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/yell1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterYellSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/yell2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterSmallYellSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/smallYell1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterSmallYellSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/smallYell2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterChargeSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/charge.wav";
	description = AudioDefault3d;
	preload = true;
};

//----------------//
// Damage Sounds: //
//----------------//

datablock AudioProfile(HarvesterWakeUpSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/wakeUp.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterDamageSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/damage.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterStaggerSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/bigDamage.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterDeathSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/death1.wav";
	description = AudioDefault3d;
	preload = true;
};

//-------------//
// Voicelines: //
//-------------//

datablock AudioProfile(HarvesterIntroVoiceSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/introVoice1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterIntroVoiceSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/introVoice2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterIntroVoiceSound3)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/introVoice3.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterIntroVoiceSound4)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/introVoice4.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterIntroVoiceSound5)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/introVoice5.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterPhaseChangeVoiceSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/phaseChangeVoice1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterPhaseChangeVoiceSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/phaseChangeVoice2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterPhaseChangeVoiceSound3)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/phaseChangeVoice3.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterDyingVoiceSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/dyingVoice.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterOutroVoiceSound1)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/outroVoice1.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterOutroVoiceSound2)
{
	fileName = $Harvester::Root @ "/resources/sounds/voice/outroVoice2.wav";
	description = AudioDefault3d;
	preload = true;
};

//------------------//
// Teleport Sounds: //
//------------------//

datablock AudioProfile(HarvesterTeleportOutSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/teleportOut.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterTeleportInSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/teleportIn.wav";
	description = AudioDefault3d;
	preload = true;
};

//-------------------//
// Teleport Sparkle: //
//-------------------//

datablock ParticleData(HarvesterTeleportFrame1Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/dot/bigDot";
};
datablock ParticleData(HarvesterTeleportFrame2Particle)
{
	textureName = $Harvester::Root @ "/resources/particles/dot/smallDot";
};
datablock ParticleData(HarvesterTeleportParticle)
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
	
	colors[0]	= "1.0 0.2 0.2 1.0";
	colors[1]	= "1.0 0.1 0.1 1.0";
	colors[2]	= "1.0 0.0 0.0 1.0";

	sizes[0]	= 0.6;
	sizes[1]	= 0.15;
	sizes[2]	= 0.0;
	
	times[0]	= 0.0;
	times[1]	= 0.8;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.3;
	gravityCoefficient = 0.1;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1300;
	lifetimeVarianceMS = 400;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterTeleportEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterTeleportParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 8;
	periodVarianceMS = 0;
	
	ejectionVelocity = 5.0;
	velocityVariance = 2.0;
	
	ejectionOffset = 2.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//----------------//
// Teleport Ring: //
//----------------//

datablock ParticleData(HarvesterTeleportRingParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/fadeRing";
	
	useInvAlpha = false;
	
	colors[0]	= "1 0.2 0.2 1.0";
	colors[1]	= "1.0 0.0 0.0 0.0";

	sizes[0]	= 22.0;
	sizes[1]	= 4.0;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 300;
	lifetimeVarianceMS = 0;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterTeleportRingEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterTeleportRingParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 300;
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
// Appear Ring: //
//--------------//

datablock ParticleData(HarvesterAppearRingParticle : HarvesterTeleportRingParticle)
{
	//------------//
	// Rendering: //
	//------------//

	sizes[0]	= 4.0;
	sizes[1]	= 16.0;
};
datablock ParticleEmitterData(HarvesterAppearRingEmitter : HarvesterTeleportRingEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterAppearRingParticle";
};

//-----------------//
// Teleport Flash: //
//-----------------//

datablock ParticleData(HarvesterTeleportFlashParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/bigFlare";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 1.0 1.0 1.0";
	colors[1]	= "1.0 0.8 0.8 0.0";

	sizes[0]	= 4.0;
	sizes[1]	= 30.0;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 100;
	lifetimeVarianceMS = 0;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterTeleportFlashEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterTeleportFlashParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 33;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-----------------//
// Teleport Trail: //
//-----------------//

datablock ParticleData(HarvesterTeleportTrailParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/blockhead";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.4 0.3 1.0";
	colors[1]	= "1.0 0.1 0.0 0.0";

	sizes[0]	= 3.15;
	sizes[1]	= 3.15;

	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 3.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 200;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterTeleportTrailEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterTeleportTrailParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 12;
	periodVarianceMS = 0;
	
	ejectionVelocity = -24.0;
	velocityVariance = 24.0;
	
	ejectionOffset = 6.0;
	
	thetaMin = 180.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
	
	uiName = "Teleport Trail";
};

//-----------//
// Teleport: //
//-----------//

datablock ExplosionData(HarvesterTeleportExplosion)
{
	//------------//
	// Rendering: //
	//------------//

	particleEmitter = HarvesterTeleportRingEmitter;
	particleDensity = 1;
	particleRadius = 0.0;

	emitter[0] = HarvesterTeleportFlashEmitter;
	emitter[1] = HarvesterTeleportEmitter;
	
	//-------------//
	// Properties: //
	//-------------//
	
	soundProfile = HarvesterTeleportOutSound;
	
	lifeTimeMS = 150;
};

datablock ProjectileData(HarvesterTeleportProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterTeleportExplosion;
};

//---------//
// Appear: //
//---------//

datablock ExplosionData(HarvesterAppearExplosion : HarvesterTeleportExplosion)
{
	//------------//
	// Rendering: //
	//------------//

	particleEmitter = HarvesterAppearRingEmitter;
	
	//-------------//
	// Properties: //
	//-------------//
	
	soundProfile = HarvesterTeleportInSound;
};

datablock ProjectileData(HarvesterAppearProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterAppearExplosion;
};

//-----------------//
// Teleport Trail: //
//-----------------//

datablock ExplosionData(HarvesterTeleportTrailExplosion)
{
	//------------//
	// Rendering: //
	//------------//
	
	emitter[0] = HarvesterTeleportTrailEmitter;
	
	//-------------//
	// Properties: //
	//-------------//

	lifeTimeMS = 150;
};

datablock ProjectileData(HarvesterTeleportTrailProjectile)
{
	//------------//
	// Explosion: //
	//------------//
	
	explosion = HarvesterTeleportTrailExplosion;

	explodeOnDeath = true;
};

//-------------//
// Playertype: //
//-------------//

datablock PlayerData(HarvesterArmor : PlayerStandardArmor)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/harvester.dts";
	
	//-----------//
	// Gameplay: //
	//-----------//

	maxDamage = 100.0;

	useCustomPainEffects = true;
	painThreshold = 0.0; // Plays pain sound from /any/ damage (fixes pain sound not playing due to health scaling).
	
	painSound = HarvesterWakeUpSound;
	deathSound = HarvesterDeathSound1;

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

	uiName = "The Harvester";
};

/// @param	this	playertype
/// @param	player	player
function HarvesterArmor::applyAvatar(%this, %player)
{
	%player.setNodeColor("ALL", $Harvester::Armor::Avatar::BodyColor);

	%player.setNodeColor("pants", $Harvester::Armor::Avatar::ClothesColor);
	%player.setNodeColor("RShoe", $Harvester::Armor::Avatar::ClothesColor);
	%player.setNodeColor("LShoe", $Harvester::Armor::Avatar::ClothesColor);
	%player.setNodeColor("ShoulderPads", $Harvester::Armor::Avatar::ClothesColor);

	%player.setNodeColor("RHand", $Harvester::Armor::Avatar::SkinColor);
	%player.setNodeColor("LHand", $Harvester::Armor::Avatar::SkinColor);
	%player.setNodeColor("HeadSkin", $Harvester::Armor::Avatar::SkinColor);

	%player.setNodeColor("ArmorMount", $Harvester::Armor::Avatar::ArmorColor);
	%player.setNodeColor("ShoulderBars", $Harvester::Armor::Avatar::ArmorColor);
	%player.setNodeColor("TorsoBars", $Harvester::Armor::Avatar::ArmorColor);

	%player.setNodeColor("Helmet", $Harvester::Armor::Avatar::PlateColor);
	%player.setNodeColor("ShoulderArmor", $Harvester::Armor::Avatar::PlateColor);
	%player.setNodeColor("TorsoArmor", $Harvester::Armor::Avatar::PlateColor);

	%player.setNodeColor("cloak", $Harvester::Armor::Avatar::CapeColor);
}