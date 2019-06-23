datablock ParticleData(FarmingPlantGrowthSparkleFrame1Particle)
{
	textureName = "./particles/Biggufurea";
};
datablock ParticleData(FarmingPlantGrowthSparkleFrame2Particle)
{
	textureName = "./particles/furea";
};
datablock ParticleData(FarmingPlantGrowthSparkleFrame3Particle)
{
	textureName = "./particles/Sukoshifurea";
};

//==================================//
// Plant Growth Particle & Emitter: //
//==================================//

datablock ParticleData(FarmingPlantGrowthSparkleParticle)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//

	textureName = "./particles/animated/block/block1"; // This automatically gets replaced with animTexName[0].
	
	// Animated texture.
	animTexName[0]	= "./particles/Biggufurea";
	animTexName[1]	= "./particles/furea";
	animTexName[2]	= "./particles/Sukoshifurea";
	
	animateTexture = true;
	framesPerSec = 10;
	
	useInvAlpha = false;

	colors[0]	= "1 1 1 1.0";
	colors[1]	= "0.4 1 0.5 1.0";
	colors[2]	= "0.4 1 0.5 1.0";

	sizes[0]	= 4.0;
	sizes[1]	= 1.0;
	sizes[2]	= 0.0;
	
	times[0]	= 0.0;
	times[1]	= 0.2;
	times[2]	= 1.0;

	//------------------//
	// Particle Fields: //
	//------------------//
	
	dragCoefficient = 0;
	gravityCoefficient = -0.1;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 50;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(FarmingPlantGrowthSparkleEmitter)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	particles = "FarmingPlantGrowthSparkleParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = false;
	orientOnVelocity = false;

	//-----------------//
	// Emitter Fields: //
	//-----------------//
	
	ejectionPeriodMS = 66;
	periodVarianceMS = 22;
	
	ejectionVelocity = 0;
	velocityVariance = 0;
	
	ejectionOffset = 1;
	
	thetaMin = 0;
	thetaMax = 90;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Plant Growth Sparkle";
};

//==============================================//
// Harvest Above-Ground Explosion & Projectile: //
//==============================================//

datablock ExplosionData(FarmingPlantGrowthExplosion)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//

	explosionShape = ""; 
	explosionScale = "1 1 1";

	faceViewer = true;
	
	emitter[0] = "FarmingPlantGrowthSparkleEmitter";
	
	//-------------------//
	// Explosion Fields: //
	//-------------------//

	lifeTimeMS = 300;
	
	soundProfile = "FarmingPlantGrowthSound";

	shakeCamera = false;
};

datablock ProjectileData(FarmingPlantGrowthProjectile)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//

	projectileShapeName = "base/data/shapes/empty.dts";

	hasLight = false;
	
	//--------------------//
	// Projectile Fields: //
	//--------------------//

	directDamage = 0;
	directDamageType = $DamageType::Direct;
	radiusDamageType = $DamageType::Direct;
	
	lifetime = 0;

	//-----------------//
	// Physics Fields: //
	//-----------------//

	isBallistic = false;
	
	//------------------------------//
	// Projectile Explosion Fields: //
	//------------------------------//

	explosion = "FarmingPlantGrowthExplosion";

	explodeOnDeath = true;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce = 0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;

	//----------------------//
	// Miscellanous Fields: //
	//----------------------//

	uiName = "Plant Growth";
};