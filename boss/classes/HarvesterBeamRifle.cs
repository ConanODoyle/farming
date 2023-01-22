//--------------------//
// Beam Rifle Sounds: //
//--------------------//

datablock AudioProfile(HarvesterBeamRifleFireSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/beamFire.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(HarvesterBeamRifleChargeSound)
{
	fileName = $Harvester::Root @ "/resources/sounds/fx/spinCharge.wav";
	description = AudioDefault3d;
	preload = true;
};

//-------//
// Beam: //
//-------//

datablock StaticShapeData(HarvesterBeamShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/beam.dts";
};

//-------------------//
// Blade Equip Ring: //
//-------------------//

datablock ParticleData(HarvesterBeamRifleSpinChargeParticle)
{
	//------------//
	// Rendering: //
	//------------//
	
	textureName =  $Harvester::Root @ "/resources/particles/charge";
	
	useInvAlpha = false;
	
	colors[0]	= "1.0 0.1 0.1 1.0";
	colors[1]	= "1.0 0.1 0.0 0.0";
	
	sizes[0]	= 5.0;
	sizes[1]	= 2.0;

	times[0]	= 0.0;
	times[1]	= 1.0;

	//-------------//
	// Properties: //
	//-------------//
	
	dragCoefficient = 0.0;
	gravityCoefficient = 0.0;

	inheritedVelFactor = 0.0;
	constantAcceleration = 0.0;

	lifetimeMS = 400;
	lifetimeVarianceMS = 0;

	spinSpeed = 1500.0;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;
};
datablock ParticleEmitterData(HarvesterBeamRifleSpinChargeEmitter)
{
	//------------//
	// Rendering: //
	//------------//
	
	particles = "HarvesterBeamRifleSpinChargeParticle";
	
	overrideAdvance = false;
	useEmitterColors = false;

	//-------------//
	// Properties: //
	//-------------//
	
	ejectionPeriodMS = 150;
	periodVarianceMS = 0;
	
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	
	ejectionOffset = 0.0;
	
	thetaMin = 0.0;
	thetaMax = 180.0;
	
	phiReferenceVel = 0.0;
	phiVariance = 360.0;
};

//------------------//
// Beam Rifle Item: //
//------------------//

datablock ItemData(HarvesterBeamRifleItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";
	emap = false;
	
	doColorShift = true;
	colorShiftColor = "0.3 0.3 0.3 1.0";
	
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
	
	image = HarvesterBeamRifleImage;
	
	canDrop = true;
	
	uiName = "The Harvester's Last Word";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_lastWord";

	category = "Weapon";
	className = "Weapon";
};

//------------------------//
// Beam Rifle Back Image: //
//------------------------//

datablock ShapeBaseImageData(HarvesterBeamRifleBackImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";

	emap = false;
	
	doColorShift = HarvesterBeamRifleItem.doColorShift;
	colorShiftColor = HarvesterBeamRifleItem.colorShiftColor;
	
	//-----------//
	// Mounting: //
	//-----------//
	
	offset = "0.4 -0.62 0.1";
	eyeOffset = "0.0 0.0 0.0";

	rotation = eulerToQuat_degrees("-45.0 22.5 90.0");
	eyeRotation = "0.0 0.0 0.0 0.0";
	
	mountPoint = $BackSlot;
	
	//-------------//
	// Properties: //
	//-------------//
	
	item = HarvesterBeamRifleItem;

	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//---------//
	// States: //
	//---------//
	
	stateName[0] = "Activate";
	stateAllowImageChange[0] = true;
	stateScript[0] = "onActivate";
};

//-------------------//
// Beam Rifle Image: //
//-------------------//

datablock ShapeBaseImageData(HarvesterBeamRifleImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";

	emap = false;
	
	doColorShift = HarvesterBeamRifleItem.doColorShift;
	colorShiftColor = HarvesterBeamRifleItem.colorShiftColor;
	
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

	item = HarvesterBeamRifleItem;
	
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
	stateTimeoutValue[2] = 1.75;
	stateTransitionOnTimeout[2] = "Fire";
	stateWaitForTimeout[2] = true;
	stateEmitter[2] = HarvesterBeamRifleSpinChargeEmitter;
	stateEmitterTime[2] = 1.25;
	stateEmitterNode[2] = "muzzleNode";
	stateAllowImageChange[2] = true;
	stateSound[2] = HarvesterBeamRifleChargeSound;
	stateScript[2] = "onCharge";
	
	stateName[3] = "Fire";
	stateTimeoutValue[3] = 0.35;
	stateTransitionOnTimeout[3] = "Wait";
	stateWaitForTimeout[3] = true;
	stateEmitter[3] = HarvesterBladeEnergyEmitter;
	stateEmitterTime[3] = 0.15;
	stateEmitterNode[3] = "muzzleNode";
	stateAllowImageChange[3] = false;
	stateSound[3] = HarvesterBeamRifleFireSound;
	stateScript[3] = "onFire";
	stateFire[3] = true;
	
	stateName[4] = "Wait";
	stateTimeoutValue[4] = 0.75;
	stateTransitionOnTimeout[4] = "CheckFire";
	stateWaitForTimeout[4] = true;
	stateAllowImageChange[4] = false;
	stateScript[4] = "onWait";
	
	stateName[5] = "CheckFire";
	stateTransitionOnTriggerDown[5] = "Fire";
	stateTransitionOnTriggerUp[5] = "Done";
	stateAllowImageChange[5] = true;
	stateScript[5] = "onCheckFire";
	
	stateName[6] = "Done";
	stateTimeoutValue[6] = 0.35;
	stateTransitionOnTimeout[6] = "Ready";
	stateWaitForTimeout[6] = true;
	stateAllowImageChange[6] = false;
	stateScript[6] = "onDone";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onMount(%this, %player, %slot)
{
	%image = %player.getMountedImage(2);
	
	if(isObject(%image) && %image == HarvesterBeamRifleBackImage.getID())
	{
		%player.unMountImage(2);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onUnMount(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.mountImage(HarvesterBeamRifleBackImage, 2);
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "root");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "gunReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		Parent::onFire(%this, %player, %slot);
		
		%player.playThread(0, "gunRecoil");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onWait(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "gunRecoilDone");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function HarvesterBeamRifleImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "gunDone");
	}
}