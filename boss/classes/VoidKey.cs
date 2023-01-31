//-------------------//
// Void Seed Sounds: //
//-------------------//

datablock AudioProfile(VoidCallSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/voidCall.wav";
	description = AudioDefaultLooping3d;
	preload = true;
};

//-------------//
// Void Light: //
//-------------//

datablock fxLightData(VoidLight)
{
	//------------//
	// Rendering: //
	//------------//
	
	lightOn = true;
	
	radius = 10.0;
	brightness = -5.0;
	color = "1.0 1.0 1.0";
	
	flareOn = true;
	
	flareBitmap = $Harvester::Root @ "/resources/shapes/darkCorona";
	nearSize = 1.5;
	farSize = 0.75;
	linkFlare = true;
	linkFlareSize = false;
	blendMode = 1;
	
	//-------------//
	// Properties: //
	//-------------//
	
	animRadius = true;
	lerpRadius = true;
	minRadius = 5.0;
	maxRadius = 10.0;
	radiusTime = 4.0;
	radiusKeys = "AZA";
};

//--------------//
// Void Sphere: //
//--------------//

datablock ParticleData(VoidRingParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/fadeRing";
	
	useInvAlpha = true;

	colors[0]	= "0.0 0.0 0.0 1.0";
	colors[1]	= "0.0 0.0 0.0 0.0";

	sizes[0]	= 0.0;
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

	lifetimeMS = 3000;
	lifetimeVarianceMS = 0;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(VoidRingEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "VoidRingParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 1000;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 90.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//----------------//
// Void Droplets: //
//----------------//

datablock ParticleData(VoidDropletsParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/dot/bigDot";
	
	useInvAlpha = true;

	colors[0]	= "0.0 0.0 0.0 1.0";
	colors[1]	= "0.0 0.0 0.0 1.0";
	colors[2]	= "0.0 0.0 0.0 1.0";

	sizes[0]	= 0.3;
	sizes[1]	= 0.1;
	sizes[2]	= 0.0;
	
	times[0]	= 0.0;
	times[1]	= 0.8;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 1.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 3000;
	lifetimeVarianceMS = 1000;

	spinSpeed = 0.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(VoidDropletsEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "VoidDropletsParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 300;
	periodVarianceMS = 100;
	
	ejectionVelocity = 3.0;
	velocityVariance = 1.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//-----------------//
// Void Seed Item: //
//-----------------//

datablock ItemData(VoidSeedItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/voidSeed.dts";
	emap = false;
	
	// rotate = true;
	
	doColorShift = false;
	
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
	
	image = VoidSeedImage;
	
	canDrop = true;
	
	uiName = "Void Seed";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_voidSeed";

	category = "Weapon";
	className = "Weapon";
};

/// @param	this	item datablock
/// @param	item	item
function VoidSeedItem::onAdd(%this, %item)
{
	Parent::onAdd(%this, %item);
	%item.playThread(0, "spin");
	%item.playAudio(0, VoidCallSound);
	%item.voidSchedule = %this.schedule(0, voidLoop, %item);
}

/// @param	this	item datablock
/// @param	item	item
function VoidSeedItem::voidLoop(%this, %item)
{
	if(isEventPending(%item.voidSchedule))
	{
		cancel(%item.voidSchedule);
	}
	
	if(!isObject(%item))
	{
		return; // Prevent effects spawning when item is spawned and picked up on the same tick.
	}
	
	if(vectorLen(%item.getVelocity()) == 0)
	{
		%this.spawnVoid(%item);
	}
	else
	{
		%item.voidSchedule = %this.schedule(100, voidLoop, %item);
	}
}

/// @param	this	item datablock
/// @param	item	item
function VoidSeedItem::spawnVoid(%this, %item)
{
	if(!isObject(%item))
	{
		return; // Prevent effects spawning when item is spawned and picked up on the same tick.
	}
	
	%item.voidRing = new particleEmitterNode()
	{
		dataBlock = GenericEmitterNode;
		emitter = VoidRingEmitter;
		position = %item.position;
		scale = "0.05 0.05 0.05";
	};
	
	if(isObject(%item.voidRing))
	{
		MissionCleanup.add(%item.voidRing);
	}
	
	%item.voidDroplets = new particleEmitterNode()
	{
		dataBlock = GenericEmitterNode;
		emitter = VoidDropletsEmitter;
		position = %item.position;
		scale = "0.05 0.05 0.05";
	};
	
	if(isObject(%item.voidDroplets))
	{
		MissionCleanup.add(%item.voidDroplets);
	}
	
	%item.voidLight = new fxLight()
	{
		dataBlock = VoidLight;
		position = %item.position;
	};
	
	if(isObject(%item.voidLight))
	{
		MissionCleanup.add(%item.voidLight);
	}
}

/// @param	this	item datablock
/// @param	item	item
function VoidSeedItem::onRemove(%this, %item)
{
	if(isObject(%item.voidRing))
	{
		%item.voidRing.delete();
	}
		
	if(isObject(%item.voidDroplets))
	{
		%item.voidDroplets.delete();
	}
		
	if(isObject(%item.voidLight))
	{
		%item.voidLight.delete();
	}
}

//------------------//
// Void Seed Image: //
//------------------//

datablock ShapeBaseImageData(VoidSeedImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/voidSeed.dts";

	emap = false;
	
	doColorShift = VoidSeedItem.doColorShift;
	colorShiftColor = VoidSeedItem.colorShiftColor;
	
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

	item = VoidSeedItem;
	
	ammo = "";
	projectile = "";
	projectileType = Projectile;

	armReady = true;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//---------//
	// States: //
	//---------//
};