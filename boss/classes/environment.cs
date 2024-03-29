//-----------------------//
// Environmental Sounds: //
//-----------------------//

datablock AudioProfile(SecretKeyActivateSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/keyActivate.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(SecretPlatformBreakSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/platformBreak.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(SecretPlatformFlameSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/platformFlame.wav";
	description = AudioDefault3d;
	preload = true;
};

//---------------//
// Music Sounds: //
//---------------//

datablock AudioProfile(HarvesterIntroMusic)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindIntro.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterPhase1Music)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindPhase1.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterChange1Music)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindChange1.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterPhase2Music)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindPhase2.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterChange2Music)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindChange2.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterPhase3Music)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindPhase3.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterChange3Music)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindChange3.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterPhase4Music)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/cybergrindPhase4.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

datablock AudioProfile(HarvesterSiriusBMusic)
{
	fileName = $Harvester::Root @ "/resources/sounds/music/siriusB.ogg";
	description = AudioMusicLooping3d;
	preload = true;
};

//---------//
// Vortex: //
//---------//

datablock StaticShapeData(HarvesterVortexShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/vortex.dts";
};

//-------//
// Riser: //
//-------//

datablock StaticShapeData(HarvesterRiserShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/riser.dts";
};

//-----------------------//
// Environmental Embers: //
//-----------------------//

datablock ParticleData(EmberParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/pSpark";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.8 0.5 0.0";
	colors[1]	= "1.0 0.5 0.2 1.0";
	colors[2]	= "1.0 0.2 0.0 0.0";

	sizes[0]	= 1.0;
	sizes[1]	= 1.0;
	sizes[2]	= 0.75;
	
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.5;
	gravityCoefficient = -0.3;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 5000;
	lifetimeVarianceMS = 2000;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(EmberEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "EmberParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 66;
	periodVarianceMS = 33;
	
	ejectionVelocity = 6.0;
	velocityVariance = 2.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 70.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
	
	uiName = "Environmental Embers";
};