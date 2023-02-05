//----------------------//
// Ancient Blade Trail: //
//----------------------//

datablock StaticShapeData(AncientBladeTrailShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/oldBladeTrail.dts";
};

//---------------------//
// Ancient Blade Item: //
//---------------------//

datablock ItemData(AncientBladeItem : HarvesterBladeItem)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/oldBlade.dts";

	colorShiftColor = "0.3 0.3 0.3 1.0";

	//-------------//
	// Properties: //
	//-------------//
	
	image = AncientBladeImage;
	
	uiName = "Ancient Warrior's Blade";
	iconName = $Harvester::Root @ "/resources/ui/icons/icon_ancientBlade";
};

//----------------------//
// Ancient Blade Image: //
//----------------------//

datablock ShapeBaseImageData(AncientBladeImage : HarvesterBladeImage)
{
	//------------//
	// Rendering: //
	//------------//
	
	shapeFile = $Harvester::Root @ "/resources/shapes/oldBlade.dts";

	doColorShift = AncientBladeItem.doColorShift;
	colorShiftColor = AncientBladeItem.colorShiftColor;

	//-------------//
	// Properties: //
	//-------------//
	
	item = AncientBladeItem;
	
	//---------------//
	// Miscellanous: //
	//---------------//

	className = "WeaponImage";
	
	//-----------//
	// Hitboxes: //
	//-----------//
	
	hitboxProjectile[0, 0] = swordProjectile;
	hitboxProjectile[0, 1] = swordProjectile;
	hitboxProjectile[0, 2] = swordProjectile;
	hitboxProjectile[0, 3] = swordProjectile;
	
	//---------//
	// States: //
	//---------//
	
	stateEmitter[4] = "";
};

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function AncientBladeImage::onReady(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "root");
	}
	
	%player.stopAudio(1);
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function AncientBladeImage::onCharge(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepReady");
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function AncientBladeImage::onFire(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%this.spawnHitboxGroup(%player, %slot, 0);
		
		%shape = new StaticShape()
		{
			dataBlock = AncientBladeTrailShape;
			scale = "12.0 12.0 1.0";
		};

		if(isObject(%shape))
		{
			MissionCleanup.add(%shape);

			%rotation = relativeVectorToRotation(%player.getForwardVector(), %player.getUpVector());
			
			%local = %player.getHackPosition() SPC %rotation;
			%offset = "0.0 1.0 0.4" SPC eulerToQuat("-4.0 180.0 0.0");
			%actual = matrixMultiply(%local, %offset);

			%shape.setTransform(%actual);
			%shape.playThread(0, "rotate");
			%shape.schedule(1000, delete);
		}
	
		%effect = new Projectile()
		{
			dataBlock = HarvesterBladeRecoilProjectile;
			initialVelocity = %player.getMuzzleVector(%slot);
			initialPosition = %player.getMuzzlePoint(%slot);
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
		serverPlay3d("HarvesterBladeAttackSound" @ getRandom(1, 2), %player.getMuzzlePoint(%slot));
	}
}

/// @param	this	weapon image
/// @param	player	player
/// @param	slot	number
function AncientBladeImage::onDone(%this, %player, %slot)
{
	if(%player.getDamagePercent() < 1.0)
	{
		%player.playThread(0, "sweepDone");
	}
}