datablock ParticleData(goldenParticleA)
{
	textureName			 = "base/lighting/flare";
	dragCoefficient		= 0.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0; 
	inheritedVelFactor	= 1;
	lifetimeMS			  = 300;
	lifetimeVarianceMS	= 100;
	useInvAlpha = false;
	spinRandomMin = 280.0;
	spinRandomMax = 281.0;

	colors[0]	  = "1 1 0 0";
	colors[1]	  = "1 1 0 1";
	colors[2]	  = "1 1 0 0";

	sizes[0]		= 1.5;
	sizes[1]		= 3.3;
	sizes[2]		= 1.8;

	times[0]		= 0.0;
	times[1]		= 0.3;
	times[2]		= 1.0;
};

datablock ParticleData(goldenParticleB)
{
	textureName			 = "base/lighting/flare";
	dragCoefficient		= 0.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0; 
	inheritedVelFactor	= 1;
	lifetimeMS			  = 300;
	lifetimeVarianceMS	= 100;
	useInvAlpha = false;
	spinRandomMin = 280.0;
	spinRandomMax = 281.0;

	colors[0]	  = "1 1 0 0";
	colors[1]	  = "1 1 0 1";
	colors[2]	  = "1 1 0 0";

	sizes[0]		= 0;
	sizes[1]		= 1;
	sizes[2]		= 0;

	times[0]		= 0.0;
	times[1]		= 0.5;
	times[2]		= 1.0;
};

datablock ParticleEmitterData(goldenEmitter)
{
	ejectionPeriodMS = 280;
	periodVarianceMS = 110;

	ejectionOffset = 0.2;
	ejectionOffsetVariance = 0.2;
	
	ejectionVelocity = 0;
	velocityVariance = 0;

	thetaMin			= 0.0;
	thetaMax			= 180.0;  

	phiReferenceVel  = 0;
	phiVariance		= 360;

	particles = "goldenParticleA goldenParticleB";	

	useEmitterColors = false;

	uiName = "Golden Shine";
	bonusYield = "22 32";
};

////

datablock ParticleData(silverParticleA : goldenParticleA)
{
	colors[0]	  = "0.8 0.8 0.8 0";
	colors[1]	  = "1 1 1 1";
	colors[2]	  = "0.8 0.8 0.8 0";
};

datablock ParticleData(silverParticleB : goldenParticleB)
{
	colors[0]	  = "0.8 0.8 0.8 0";
	colors[1]	  = "1 1 1 1";
	colors[2]	  = "0.8 0.8 0.8 0";
};

datablock ParticleEmitterData(silverEmitter : goldenEmitter)
{
	particles = "silverParticleA silverParticleB";

	uiName = "Silver Shine";
	bonusYield = "8 16";
};

////

datablock ParticleData(BronzeParticleA : goldenParticleA)
{
	colors[0]	  = "0.4 0.1 0.0 0";
	colors[1]	  = "0.8 0.4 0 1";
	colors[2]	  = "0.4 0.1 0.0 0";
};

datablock ParticleData(BronzeParticleB : goldenParticleB)
{
	colors[0]	  = "0.4 0.1 0.0 0";
	colors[1]	  = "0.8 0.4 0 1";
	colors[2]	  = "0.4 0.1 0.0 0";
};

datablock ParticleEmitterData(BronzeEmitter : goldenEmitter)
{
	particles = "BronzeParticleA BronzeParticleB";

	uiName = "Bronze Shine";
	bonusYield = "4 8";
};

////

package ShinyPlants
{
	function plantCrop(%image, %obj, %slot, %pos)
	{
		%ret = parent::plantCrop(%image, %obj, %slot, %pos);

		if (isObject(%ret) && %ret.getClassName() $= "fxDTSBrick" && (getRandom() < 0.002 || %obj.guaranteedShiny)
			&& %obj.lastShiny + 30 < $Sim::Time)
		{
			if (getRandom() < 0.15)
			{
				//gold plant
				%ret.setEmitter(goldenEmitter.getID());
				%type = "<color:faef00>Golden";
			}
			else
			{
				//silver plant
				%ret.setEmitter(silverEmitter.getID());
				%type = "<color:fafafa>Silver";
			}

			if (isObject(%cl = %obj.client))
			{
				messageAll('MsgUploadStart', "<bitmap:base/client/ui/ci/star> \c3" @ 
					%cl.name @ "\c6 planted a " @ %type SPC %ret.getDatablock().cropType @ "\c6!");
			}

			%obj.lastShiny = $Sim::Time;
		}

		return %ret;
	}
};
activatePackage(ShinyPlants);