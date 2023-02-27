//--------------//
// Spin Charge: //
//--------------//

datablock ParticleData(HarvesterSpiritSummonSpinChargeParticle : HarvesterBeamRifleSpinChargeParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	colors[0]	= "0.5 0.7 1.0 1.0";
	colors[1]	= "0.5 0.7 1.0 0.0";
};
datablock ParticleEmitterData(HarvesterSpiritSummonSpinChargeEmitter : HarvesterBeamRifleSpinChargeEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterSpiritSummonSpinChargeParticle";
};

//-----------------------//
// Spirit Summon Energy: //
//-----------------------//

datablock ParticleData(HarvesterSpiritSummonEnergyParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName = $Harvester::Root @ "/resources/particles/sparkle";
	
	useInvAlpha = false;

	colors[0]	= "0.5 0.7 1.0 0.1";
	colors[1]	= "0.5 0.7 1.0 0.1";
	colors[2]	= "0.5 0.7 1.0 0.0";

	sizes[0]	= 5.0;
	sizes[1]	= 2.0;
	sizes[2]	= 0.0;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.5;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.1;
	constantAcceleration = 0.0;

	lifetimeMS = 1000;
	lifetimeVarianceMS = 0;

	spinSpeed = 750.0;
	spinRandomMin = -25.0;
	spinRandomMax = 25.0;
};
datablock ParticleEmitterData(HarvesterSpiritSummonEnergyEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterSpiritSummonEnergyParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 89.0;
	thetaMax = 90.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 0.0;
};

//---------------------//
// Spirit Summon Item: //
//---------------------//

datablock ItemData(HarvesterSpiritSummonItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;
	
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
	
	image = HarvesterSpiritSummonImage;
	
	canDrop = true;
	
	uiName = "The Harvester's Redemption";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_redemption";

	category = "Weapon";
	className = "Weapon";
};

//----------------------//
// Spirit Summon Image: //
//----------------------//

datablock ShapeBaseImageData(HarvesterSpiritSummonImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = "base/data/shapes/empty.dts";

	emap = false;
	
	doColorShift = HarvesterSpiritSummonItem.doColorShift;
	colorShiftColor = HarvesterSpiritSummonItem.colorShiftColor;
	
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

	item = HarvesterSpiritSummonItem;
	
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
	
	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.15;
	stateTransitionOnTimeout[0] = "Ready";
	stateWaitForTimeout[0] = true;
	stateAllowImageChange[0] = true;
	stateSound[0] = weaponSwitchSound;
	stateScript[0] = "onActivate";
	
	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Charge";
	stateAllowImageChange[1] = true;
	stateScript[1] = "onReady";
	
	stateName[2] = "Charge";
	stateTimeoutValue[2] = 0.75;
	stateTransitionOnTimeout[2] = "Fire";
	stateWaitForTimeout[2] = true;
	stateEmitter[2] = HarvesterSpiritSummonSpinChargeEmitter;
	stateEmitterTime[2] = 0.5;
	stateEmitterNode[2] = "muzzleNode";
	stateAllowImageChange[2] = true;
	stateSound[2] = HarvesterBeamRifleChargeSound;
	stateScript[2] = "onCharge";
	
	stateName[3] = "Fire";
	stateTimeoutValue[3] = 0.45;
	stateTransitionOnTimeout[3] = "Done";
	stateWaitForTimeout[3] = true;
	stateEmitter[3] = HarvesterSpiritSummonEnergyEmitter;
	stateEmitterTime[3] = 0.15;
	stateEmitterNode[3] = "muzzleNode";
	stateAllowImageChange[3] = false;
	stateScript[3] = "onFire";
	stateFire[3] = true;
	
	stateName[4] = "Done";
	stateTimeoutValue[4] = 0.25;
	stateTransitionOnTimeout[4] = "Ready";
	stateWaitForTimeout[4] = true;
	stateAllowImageChange[4] = false;
	stateScript[4] = "onDone";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterSpiritSummonImage::onReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "root");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterSpiritSummonImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterSpiritSummonImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		clearAncientWarriors();
		%player.harvesterSummonAncientWarriors();
		
		%effect = new Projectile()
		{
			dataBlock = GhostLilyProjectile;
			initialVelocity = %player.getMuzzleVector(%slot);
			initialPosition = %player.getMuzzlePoint(%slot);
			scale = "0.7 0.7 0.7";
			sourceObject = %player;
			sourceSlot = %slot;
			client = %player.client;
		};

		if(isObject(%effect))
		{
			MissionCleanup.add(%effect);
			%effect.explode();
		}
		
		%player.playThread(0, "sweepAttack");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterSpiritSummonImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepDone");
	}
}