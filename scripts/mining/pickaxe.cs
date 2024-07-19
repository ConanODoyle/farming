
datablock ItemData(PickaxeItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/Pickaxe";
	shapeFile = "./resources/pickaxe/Pickaxe.dts";
	uiName = "Pickaxe";

	image = "PickaxeImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;
};

datablock ShapeBaseImageData(PickaxeImage)
{
	shapeFile = "./resources/pickaxe/Pickaxe.dts";
	emap = true;

	item = PickaxeItem;
	doColorShift = false;
	colorShiftColor = "0.4 0 0 1";

	armReady = true;

	toolTip = "Mine coal and other minerals";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.12;
	stateWaitForTimeout[0] = true;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 0.2;
	stateTransitionOnTimeout[2] = "Repeat";
	stateWaitForTimeout[2] = 1;

	stateName[3] = "Repeat";
	stateTimeoutValue[3] = 0.12;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";
};

function PickaxeImage::onFire(%this, %obj, %slot)
{
	if (%obj.nextHit > getSimTime())
		return;
	
	%obj.nextHit = getSimTime() + 300 | 0;
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));

	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::PlayerObjectType, %obj);
	%hit = getWord(%ray, 0);
	if (isObject(%hit))
	{
		if (%hit.getClassName() $= "fxDTSBrick")
		{
			%hit.onPickaxeHit(%obj);
		}
		%p = new Projectile() {
			dataBlock = hammerProjectile;
			initialPosition = getWords(%ray, 1, 3);
			initialVelocity = vectorScale(getWords(%ray, 4, 6), -1);
		};
		%p.explode();
		serverPlay3D(hammerHitSound, getWords(%ray, 1, 3));
	}
	%obj.playThread(0, shiftDown);
}

function fxDTSBrick::onPickaxeHit(%this, %pl)
{
	%cl = %pl.client;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onPickaxeHit", %cl);

	if (%this.isMineable)
	{
		processMiningHit(%this, %pl);
	}
}
registerInputEvent("fxDTSBrick", "onPickaxeHit", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

function fxDTSBrick::setMineable(%this, %isMineable, %coalChance, %phosphateChance)
{
	%this.isMineable = %isMineable;
	%this.coalChance = %coalChance;
	%this.phosphateChance = %phosphateChance;
}
registerOutputEvent("fxDTSBrick", "setMineable", "bool 0" TAB "string 100 100" TAB "string 100 100");

$SapphireChance = 0.0012;
$EmeraldChance = 0.0008;
$RubyChance = 0.0004;
$DiamondChance = 0.0001;

function processMiningHit(%this, %pl)
{
	%this.hitcount++;

	switch (%this.hitcount)
	{
		case 0: return;
		case 1: return;
		case 2: %coalCheck = 1; %phosphateCheck = 1;
		case 3: %coalCheck = 1; %phosphateCheck = 1; %gemCheck = 1; //no fall through :pensive:
		default: %coalCheck = 1; %phosphateCheck = 1; %gemCheck = 1;
	}

	if (%phosphateCheck && getRandom() < %this.phosphateChance * getMax(1, %this.hitcount / 5))
	{
		for (%i = 0; %i < getRandom(1, 4); %i++)
		{
			%this.spawnItem("0 0 5", "PhosphateItem");
		}
		serverPlay3D("FarmingHarvestBelowGroundPlantSound", %this.position);
		%this.hitcount = 0;
		%this.disappear(60);
		return;
	}
	if (%coalCheck && getRandom() < %this.coalChance * getMax(1, %this.hitcount / 5))
	{
		for (%i = 0; %i < getRandom(1, 4); %i++)
		{
			%this.spawnItem("0 0 5", "CoalItem");
		}
		serverPlay3D("FarmingHarvestBelowGroundPlantSound", %this.position);
		%this.hitcount = 0;
		%this.disappear(90);
		return;
	}
	if (%gemCheck)
	{
		%rand = getRandom();
		if (%rand < $DiamondChance)
		{
			%this.spawnItem("0 0 5", "DiamondItem");
		}
		else if (%rand < $RubyChance)
		{
			%this.spawnItem("0 0 5", "RubyItem");
		}
		else if (%rand < $EmeraldChance)
		{
			%this.spawnItem("0 0 5", "EmeraldItem");
		}
		else if (%rand < $SapphireChance)
		{
			%this.spawnItem("0 0 5", "SapphireItem");
		}
		else
		{
			return;
		}
		if (%pl.client.bl_id != 4382 || getRandom() < 0.3)
		{
			serverPlay3D("rewardSound", %this.position);
		}
		serverPlay3D("FarmingHarvestBelowGroundPlantSound", %this.position);
		%this.hitcount = 0;
		%this.disappear(90);
	}
}