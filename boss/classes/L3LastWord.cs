//------------------------//
// L3 - "Last Word" Item: //
//------------------------//

datablock ItemData(L3LastWordItem)
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
	
	image = L3LastWordImage;
	
	canDrop = true;
	
	uiName = "L3 - \"Last Word\"";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_lastWord";

	category = "Weapon";
	className = "Weapon";
	
	//----------//
	// Farming: //
	//----------//
	
	durability = 100000;

	hasDataID = 1;
	isDataIDTool = 1;
};

//-------------------------//
// L3 - "Last Word" Image: //
//-------------------------//

datablock ShapeBaseImageData(L3LastWordImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/beamRifle.dts";

	emap = false;
	
	doColorShift = L3LastWordItem.doColorShift;
	colorShiftColor = L3LastWordItem.colorShiftColor;
	
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

	item = L3LastWordItem;
	
	ammo = "";
	projectile = "";
	projectileType = Projectile;

	armReady = true;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//----------//
	// Hitscan: //
	//----------//
	
	hitscanRange = $Harvester::BeamRifle::Range;
	hitscanTypes = $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FXBrickObjectType;
	
	hitscanDamage = 0;
	hitscanDamageType = $DamageType::Direct;
	hitscanDamageFalloff = 0;
	
	hitscanProjectile = L3LastWordWaterProjectile;
	
	hitscanTracerStaticShape = HarvesterBeamShape;
	hitscanTracerStaticLifetime = 100;
	
	hitscanSpread = 0.0;
	hitscanShotCount = 1;
	
	hitscanPenetrate = false;
	
	hitscanFromMuzzle = true;
	
	hitscanImpactImpulse = 0.0;
	hitscanVerticalImpulse = 0.0;
	
	hitscanExplodeOnMiss = true;
	
	//----------//
	// Farming: //
	//----------//
	
	waterRange = $Harvester::BeamRifle::Range;
	tankAmount = 1000;
	waterAmount = 1000;
	
	toolTip = "Waters Dirt: +1000 | Tanks: +1000";
	
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
function L3LastWordImage::onReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		wateringCanReady(%this, %player, %slot);
	}
}

/// @param	this		projectile datablock
/// @param	player		player
/// @param	slot		number
/// @param	collision	object
/// @param	position	3-element position
/// @param	normal		3-element vector
/// @param	vector		3-element vector
/// @param	crit		boolean
// function L3LastWordImage::onHitscanExplode(%this, %player, %slot, %collision, %position, %normal, %vector, %crit)
// {
	// Parent::onHitscanExplode(%this, %player, %slot, %collision, %position, %normal, %vector, %crit);
	
	// if(!isObject(%collision))
	// {
		// return;
	// }
	
	// if(!(%collision.getType() & $TypeMasks::fxBrickObjectType))
	// {
		// return;
	// }
// }

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L3LastWordImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		Parent::onFire(%this, %player, %slot);
		waterCanFire(%this, %player, %slot);
		
		%player.playThread(0, "jump");
		%player.playThread(2, "activate");
	}
}