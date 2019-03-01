

datablock ParticleData(InfoBaseParticle)
{
	textureName			 = "base/data/shapes/sprayCanLabel";
	dragCoefficient		= 10.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0; 
	inheritedVelFactor	= 0;
	lifetimeMS			  = 8000;
	lifetimeVarianceMS	= 0;
	useInvAlpha = true;
	spinRandomMin = 0;
	spinRandomMax = 0;

	colors[0]	  = "1 1 1 1";
	colors[1]	  = "1 1 1 1";
	colors[2]	  = "1 1 1 1";
	colors[3]	  = "1 1 1 1";

	sizes[0]		= 0;
	sizes[1]		= 4;
	sizes[2]		= 3.8;
	sizes[3]		= 3.8;

	times[0]		= 0.0;
	times[1]		= 0.05;
	times[2]		= 0.1;
	times[3]		= 1;
};

datablock ParticleEmitterData(InfoBaseEmitter)
{
	ejectionPeriodMS = 1000;
	periodVarianceMS = 0;

	ejectionOffset = 0;
	ejectionOffsetVariance = 0;
	
	ejectionVelocity = 0;
	velocityVariance = 0;

	thetaMin			= 0.0;
	thetaMax			= 1.0;  

	phiReferenceVel  = 0;
	phiVariance		= 1;

	particles = "InfoBaseParticle";	

	useEmitterColors = false;
	orientParticles = false;

	uiName = "";
};

datablock ExplosionData(InfoBaseExplosion)
{
	cameraShakeFalloff = 1;
	camShakeFrequency = "0 0 0";
	camShakeRadius = 0;
	camShakeAmp = "0 0 0";
	camShakeDuration = 0;

	particleRadius = 0.1;
	particleEmitter = InfoBaseEmitter;
	particleDensity = 1;
	delayMS = 0;

	lifetimeMS = 50;
	sizes[0]		= "1 1 1";
	sizes[1]		= "1 1 1";

	times[0]		= 0;
	times[1]		= 1;
};

datablock ProjectileData(InfoBaseProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	explosion = InfoBaseExplosion;
	muzzleVelocity = 10;
	lifetime = 30;
	explodeOnDeath = true;
};

function registerAllInfoParticles()
{
	%loc = "Add-ons/Server_Farming/scripts/infoBubbles/*.png";
	for (%dir = findFirstFile(%loc); %dir !$= ""; %dir = findNextFile(%loc))
	{
		// echo("DIR:" SPC %dir);
		%pos1 = strLastPos(%dir, "/")+1;
		%pos2 = strPos(%dir, ".", %pos1 + 1);
		%name = getSubStr(%dir, %pos1, %pos2 - %pos1);
		%name = stripChars(%name, " (){},.<>?/\\!@#$%^&*:;'\"-+=[]|");
		// echo("NAME:" SPC %name);

		if (!isObject("Info" @ %name @ "Particle"))
		{
			%db = %db @ "datablock ParticleData(Info" @ %name @ "Particle : InfoBaseParticle)";
			%db = %db @ "{";
			%db = %db @ 	"textureName = \"./" @ %name @ "\";";
			%db = %db @ "};";

			%db = %db @ "datablock ParticleEmitterData(Info" @ %name @ "Emitter : InfoBaseEmitter)";
			%db = %db @ "{";
			%db = %db @ 	"particles = Info" @ %name @ "Particle;";
			%db = %db @ "};";

			%db = %db @ "datablock ExplosionData(Info" @ %name @ "Explosion : InfoBaseExplosion)";
			%db = %db @ "{";
			%db = %db @ 	"particleEmitter = Info" @ %name @ "Emitter;";
			%db = %db @ "};";

			%db = %db @ "datablock ProjectileData(Info" @ %name @ "Projectile : InfoBaseProjectile)";
			%db = %db @ "{";
			%db = %db @ 	"explosion = Info" @ %name @ "Explosion;";
			%db = %db @ "};";

			eval(%db);
			echo("    Registered \"" @ %name @ "\" info bubble");
			%count++;
		}
	}
	echo("Registered " @ %count @ " new info bubbles");
}

registerAllInfoParticles();