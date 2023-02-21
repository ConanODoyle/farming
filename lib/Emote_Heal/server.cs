//Emote_HealCross.cs

datablock AudioProfile(HealCrossSound)
{
	filename = "./Heal_sfx.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock ParticleData(HealCrossParticle)
{
   dragCoefficient      = 5.0;
   gravityCoefficient   = -1.0;
   inheritedVelFactor   = 0.0;
   windCoefficient      = 0;
   constantAcceleration = 0.0;
   lifetimeMS           = 2500;
   lifetimeVarianceMS   = 0;
   useInvAlpha          = false;
   textureName          = "./heal";
   colors[0]     = "1 1 1 1";
   colors[1]     = "1 1 1 1";
   colors[2]     = "1 1 1 0";
   sizes[0]      = 0.9;
   sizes[1]      = 0.9;
   sizes[2]      = 0.9;
   times[0]      = 0.0;
   times[1]      = 0.2;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(HealCrossEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 0;
   ejectionVelocity = 1.0;
   ejectionOffset   = 1.2;
   velocityVariance = 0.0;
   thetaMin         = 0;
   thetaMax         = 360;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   lifeTimeMS = 100;
   particles = "HealCrossParticle";

   doFalloff = false; //if we do fall off with this emitter it ends up flickering, for most emitters you want this TRUE

   emitterNode = GenericEmitterNode;        //used when placed on a brick
   pointEmitterNode = TenthEmitterNode; //used when placed on a 1x1 brick

   uiName = "Emote - HealCross";
};

datablock ExplosionData(HealCrossExplosion)
{
   lifeTimeMS = 1000;
   emitter[0] = HealCrossEmitter;
   soundProfile = HealCrossSound;
};

//we cant spawn explosions, so this is a workaround for now
datablock ProjectileData(HealCrossProjectile)
{
   explosion           = HealCrossExplosion;

   armingDelay         = 0;
   lifetime            = 10;
   explodeOnDeath		= true;
};