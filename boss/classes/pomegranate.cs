datablock ItemData(PomegranateHealingItem)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/pomegranate.dts";
	emap = false;
	
	doColorShift = true;
	colorShiftColor = "0.86 0 0.21 1";
	
	mass = 1.0;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	
	image = PomegranateHealingImage;

	canDrop = true;
	
	uiName = "Pomegranate";
	iconName = $Harvester::Root @ "/resources/ui/icons/pomegranate";

	isBossReward = 1;
	category = "Weapon";
	className = "Weapon";
};

datablock ShapeBaseImageData(PomegranateHealingImage)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/pomegranate.dts";

	emap = false;
	
	doColorShift = PomegranateHealingItem.doColorShift;
	colorShiftColor = PomegranateHealingItem.colorShiftColor;

	offset = "-0.07 0.05 0";
	rotation = eulerToMatrix("0 11 0");
	mountPoint = 0;
	armReady = true;

	className = "WeaponImage";
	toolTip = "Heals +35 HP";
	minShotTime = 5000;
	healAmount = 35;

	stateName[0]						= "Activate";
	stateTimeoutValue[0]				= 0.15;
	stateTransitionOnTimeout[0]		 	= "Ready";
	stateSound[0]						= weaponSwitchSound;

	stateName[1]						= "Ready";
	stateTransitionOnTriggerDown[1]  	= "Fire";
	stateAllowImageChange[1]			= true;
	stateSequence[1]					= "Ready";

	stateName[2]						= "Fire";
	stateTransitionOnTimeout[2]	  		= "Reload";
	stateTimeoutValue[2]				= 0.14;
	stateAllowImageChange[2]			= false;
	stateScript[2]						= "onFire";
	stateWaitForTimeout[2]				= true;

	stateName[3]						= "Reload";
	stateTransitionOnTriggerUp[3]		= "Ready";
};

function PomegranateHealingImage::onFire(%this,%obj,%slot)
{
	%obj.playThread(2, plant);

	if (%obj.stimRecharging)
	{
		centerprint(%obj.client, "Can't take another bite yet!\n\nGive it a while to regrow..." , 5);
		return;
	}
	else if (%obj.getDamageLevel() <= 0)
	{
		centerprint(%obj.client, "You are already full!", 5);
		return;
	}

	%obj.playThread(2, shiftDown);
	%obj.lastStimTime = getSimTime();
	serverPlay3D(brickPlantSound,%obj.getPosition());

	%obj.stimRecharging = 1;
	schedule(%this.minShotTime, 0, "canEatPomegranate", %obj);	

	%obj.setWhiteout(0.3);
	%obj.setDamageLevel(getMax(0, %obj.getDamageLevel() - %this.healAmount));
	centerprint(%obj.client, "\c2" @ mCeil(%obj.dataBlock.maxDamage - %obj.getDamageLevel()) @ "HP (+" @ %this.healAmount@ ")", 3);
	// %obj.emote(medigunhealImage);
	%obj.spawnExplosion(healCrossProjectile,"1 1 1");
}

function canEatPomegranate(%obj)
{
	centerprint(%obj.client, "\c4Pomegranate regrown!\n\c6You can use it again.", 5);
	serverPlay3D(brickChangeSound, %obj.getPosition());
	%obj.stimRecharging = 0;
}