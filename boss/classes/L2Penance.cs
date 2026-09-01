//-------//
// Bomb: //
//-------//

datablock ExplosionData(L2PenanceBombExplosion : HarvesterBombExplosion)
{
	//-------------//
	// Properties: //
	//-------------//

	damageRadius = 0.0;
	radiusDamage = 0.0;

	impulseRadius = 0.0;
	impulseForce = 0.0;
};
datablock ProjectileData(L2PenanceBombProjectile : HarvesterBombProjectile)
{
	//------------//
	// Explosion: //
	//------------//

	explosion = L2PenanceBombExplosion;

	//---------------//
	// Miscellanous: //
	//---------------//

	uiName = "";
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function L2PenanceBombProjectile::onExplode(%this, %projectile, %position, %fade)
{
	HarvesterBombProjectile::onExplode(%this, %projectile, %position, %fade);
}

//---------------//
// Cluster Bomb: //
//---------------//

datablock ProjectileData(L2PenanceClusterBombProjectile : HarvesterClusterBombProjectile)
{
	//------------//
	// Explosion: //
	//------------//

	explosion = L2PenanceBombExplosion;

	//---------------//
	// Miscellanous: //
	//---------------//

	uiName = "";

	baseProjectile = L2PenanceBombProjectile;
	subProjectile = L2PenanceBombProjectile;
};

/// @param	this		projectile datablock
/// @param	projectile	projectile
/// @param	position	3-element position
/// @param	fade		number
function L2PenanceClusterBombProjectile::onExplode(%this, %projectile, %position, %fade)
{
	HarvesterClusterBombProjectile::onExplode(%this, %projectile, %position, %fade);
}

/// @param	this		projectile datablock
/// @param	projectile	projectile
function L2PenanceClusterBombProjectile::split(%this, %projectile)
{
	HarvesterClusterBombProjectile::split(%this, %projectile);
}

//----------------------//
// L2 - "Penance" Item: //
//----------------------//

datablock ItemData(L2PenanceItem)
{
	//------------//
	// Rendering: //
	//------------//

	shapeFile = $Harvester::Root @ "/resources/shapes/bomb3Image.dts";
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

	image = L2PenanceImage;

	canDrop = true;

	uiName = "L2 - \"Penance\"";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_penance";

	category = "Weapon";
	className = "Weapon";

	//----------//
	// Farming: //
	//----------//

	durability = 320;

	canPickupMultiple = 0;
	isBossReward = 1;

	hasDataID = 1;
	isDataIDTool = 1;
};

//-----------------------//
// L2 - "Penance" Image: //
//-----------------------//

datablock ShapeBaseImageData(L2PenanceImage)
{
	//------------//
	// Rendering: //
	//------------//

	shapeFile = $Harvester::Root @ "/resources/shapes/bomb3Image.dts";

	emap = false;

	doColorShift = L2PenanceItem.doColorShift;
	colorShiftColor = L2PenanceItem.colorShiftColor;

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

	item = L2PenanceItem;

	ammo = "";
	projectile = L2PenanceClusterBombProjectile;
	projectileType = Projectile;

	armReady = true;

	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";

	//----------//
	// Farming: //
	//----------//

	min = 32;

	tooltip = "Plants up to 32 seeds in a row";

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
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateWaitForTimeout[1] = false;
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "Ready2";
	stateAllowImageChange[1] = true;
	stateScript[1] = "onLoop";

	stateName[2] = "Ready2";
	stateTransitionOnTriggerDown[2] = "PreFire";
	stateWaitForTimeout[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Ready";
	stateAllowImageChange[2] = true;
	stateScript[2] = "onLoop";

	stateName[3] = "PreFire";
	stateTimeoutValue[3] = 0.12;
	stateTransitionOnTimeout[3] = "Fire";
	stateWaitForTimeout[3] = true;
	stateAllowImageChange[3] = false;
	stateScript[3] = "onPreFire";

	stateName[4] = "Fire";
	stateTimeoutValue[4] = 0.45;
	stateTransitionOnTimeout[4] = "Done";
	stateWaitForTimeout[4] = true;
	stateAllowImageChange[4] = false;
	stateScript[4] = "onFire";
	stateFire[4] = true;

	stateName[5] = "Done";
	stateTimeoutValue[5] = 0.25;
	stateTransitionOnTimeout[5] = "Ready";
	stateWaitForTimeout[5] = true;
	stateAllowImageChange[5] = false;
	stateScript[5] = "onDone";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L2PenanceImage::onLoop(%this, %player, %slot)
{
	PlanterImage::onLoop(%this, %player, %slot);
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L2PenanceImage::onPreFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "plant");
		%player.playThread(2, "shiftTo");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function L2PenanceImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		if(getDurability(%this, %player, %slot) > 0)
			Parent::onFire(%this, %player, %slot);

		PlanterImage::onFire(%this, %player, %slot);

		%player.playThread(0, "jump");
		%player.playThread(2, "shiftUp");
		%player.playThread(3, "rotCW");
	}
}