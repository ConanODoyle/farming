//==========================================//
// Harvest Above-Ground Particle & Emitter: //
//==========================================//

datablock ParticleData(FarmingHarvestAboveGroundPlantParticle)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	textureName = "./particles/hitspark";
	
	useInvAlpha = false;

	colors[0]	= "1.0 1.0 1.0 1.0";
	colors[1]	= "1.0 1.0 1.0 1.0";

	sizes[0]	= 0.25;
	sizes[1]	= 0.75;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//------------------//
	// Particle Fields: //
	//------------------//
	
	dragCoefficient = 1;
	gravityCoefficient = 0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 100;
	lifetimeVarianceMS = 30;

	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
};
datablock ParticleEmitterData(FarmingHarvestAboveGroundPlantEmitter)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	particles = "FarmingHarvestAboveGroundPlantParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-----------------//
	// Emitter Fields: //
	//-----------------//
	
	ejectionPeriodMS = 5;
	periodVarianceMS = 2;
	
	ejectionVelocity = 12;
	velocityVariance = 0;
	
	ejectionOffset = 0;
	
	thetaMin = 0;
	thetaMax = 90;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Harvest Above-Ground Spark";
};

//==================================//
// Harvest Leaf Particle & Emitter: //
//==================================//

datablock ParticleData(FarmingHarvestPlantLeafParticle)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	textureName = "./particles/singleLeaf";
	
	useInvAlpha = true;

	colors[0]	= "0.22 0.43 0.21 1.0";
	colors[1]	= "0.22 0.43 0.21 0.0";

	sizes[0]	= 1.15;
	sizes[1]	= 0.65;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//------------------//
	// Particle Fields: //
	//------------------//
	
	dragCoefficient = 0;
	gravityCoefficient = 1;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;

	spinSpeed = 50;
	spinRandomMin = -100;
	spinRandomMax = 100;
};
datablock ParticleEmitterData(FarmingHarvestPlantLeafEmitter)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	particles = "FarmingHarvestPlantLeafParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;
	
	orientParticles = true;
	orientOnVelocity = true;

	//-----------------//
	// Emitter Fields: //
	//-----------------//
	
	ejectionPeriodMS = 12;
	periodVarianceMS = 2;
	
	ejectionVelocity = 4;
	velocityVariance = 1;
	
	ejectionOffset = 0;
	
	thetaMin = 30;
	thetaMax = 60;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Harvest Plant Leaf";
};

datablock ParticleData(FarmingHarvestTriplePlantLeafParticle : FarmingHarvestPlantLeafParticle)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	textureName = "./particles/tripleLeaf";
	
	colors[0]	= "0.17 0.34 0.17 1.0";
	colors[1]	= "0.17 0.34 0.17 1.0";

};
datablock ParticleEmitterData(FarmingHarvestTriplePlantLeafEmitter : FarmingHarvestPlantLeafEmitter)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	particles = "FarmingHarvestTriplePlantLeafParticle";

	//-----------------//
	// Emitter Fields: //
	//-----------------//
	
	uiName = "Harvest Plant Triple Leaf";
};

//================================================//
// Harvest Below-Ground Plant Particle & Emitter: //
//================================================//

datablock ParticleEmitterData(FarmingHarvestBelowGroundPlantEmitter : FarmingHarvestAboveGroundPlantEmitter)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	particles = "FarmingHarvestAboveGroundPlantParticle";

	//-----------------//
	// Emitter Fields: //
	//-----------------//
	
	thetaMin = 0;
	thetaMax = 30;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Harvest Below-Ground Spark";
};

//==================================//
// Harvest Dirt Particle & Emitter: //
//==================================//

datablock ParticleData(FarmingHarvestDirtParticle)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	textureName = "base/data/particles/chunk";
	
	useInvAlpha = true;

	colors[0]	= "0.44 0.29 0.16 1.0";
	colors[1]	= "0.44 0.29 0.16 0.0";

	sizes[0]	= 1.0;
	sizes[1]	= 0.5;
	
	times[0]	= 0.0;
	times[1]	= 1.0;

	//------------------//
	// Particle Fields: //
	//------------------//
	
	dragCoefficient = 0.1;
	gravityCoefficient = 2.0;

	inheritedVelFactor = 0;
	constantAcceleration = 0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 500;

	spinSpeed = 1000;
	spinRandomMin = -125;
	spinRandomMax = 125;
};
datablock ParticleEmitterData(FarmingHarvestDirtEmitter)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	particles = "FarmingHarvestDirtParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-----------------//
	// Emitter Fields: //
	//-----------------//
	
	ejectionPeriodMS = 8;
	periodVarianceMS = 2;
	
	ejectionVelocity = 6;
	velocityVariance = 2;
	
	ejectionOffset = 0;
	
	thetaMin = 0;
	thetaMax = 45;
	
	phiReferenceVel = 0;
	phiVariance = 360;
	
	uiName = "Harvest Dirt";
};

//========================================//
// Harvest Small Dirt Particle & Emitter: //
//========================================//

datablock ParticleData(FarmingHarvestSmallDirtParticle : FarmingHarvestDirtParticle)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//

	colors[0]	= "0.33 0.24 0.15 1.0";
	colors[1]	= "0.33 0.24 0.15 0.0";

	sizes[0]	= 0.75;
	sizes[1]	= 0.25;
};
datablock ParticleEmitterData(FarmingHarvestSmallDirtEmitter : FarmingHarvestDirtEmitter)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//
	
	particles = "FarmingHarvestSmallDirtParticle";

	//-----------------//
	// Emitter Fields: //
	//-----------------//
	
	ejectionPeriodMS = 10;
	
	ejectionVelocity = 7;
	velocityVariance = 2;
	
	thetaMax = 30;
	
	uiName = "Harvest Small Dirt";
};

//==============================================//
// Harvest Above-Ground Explosion & Projectile: //
//==============================================//

datablock ExplosionData(FarmingHarvestAboveGroundPlantExplosion)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//

	explosionShape = ""; 
	explosionScale = "1 1 1";

	faceViewer = true;
	
	emitter[0] = "FarmingHarvestAboveGroundPlantEmitter";
	emitter[1] = "FarmingHarvestPlantLeafEmitter";
	emitter[2] = "FarmingHarvestTriplePlantLeafEmitter";
	
	//-------------------//
	// Explosion Fields: //
	//-------------------//

	lifeTimeMS = 33;
	
	soundProfile = "FarmingHarvestAboveGroundPlantSound";

	shakeCamera = false;
};

datablock ProjectileData(FarmingHarvestAboveGroundPlantProjectile)
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

	explosion = "FarmingHarvestAboveGroundPlantExplosion";

	explodeOnDeath = true;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce = 0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;

	//----------------------//
	// Miscellanous Fields: //
	//----------------------//

	uiName = "Harvest Above-Ground Plant";
};

//==============================================//
// Harvest Below-Ground Explosion & Projectile: //
//==============================================//

datablock ExplosionData(FarmingHarvestBelowGroundPlantExplosion)
{
	//-------------------//
	// Rendering Fields: //
	//-------------------//

	explosionShape = ""; 
	explosionScale = "1 1 1";

	faceViewer = true;
	
	emitter[0] = "FarmingHarvestBelowGroundPlantEmitter";
	emitter[1] = "FarmingHarvestDirtEmitter";
	emitter[2] = "FarmingHarvestSmallDirtEmitter";
	
	//-------------------//
	// Explosion Fields: //
	//-------------------//

	lifeTimeMS = 33;
	
	soundProfile = "FarmingHarvestBelowGroundPlantSound";

	shakeCamera = false;
};

datablock ProjectileData(FarmingHarvestBelowGroundPlantProjectile : FarmingHarvestAboveGroundPlantProjectile)
{
	//------------------------------//
	// Projectile Explosion Fields: //
	//------------------------------//

	explosion = "FarmingHarvestBelowGroundPlantExplosion";

	//----------------------//
	// Miscellanous Fields: //
	//----------------------//

	uiName = "Harvest Below-Ground Plant";
};