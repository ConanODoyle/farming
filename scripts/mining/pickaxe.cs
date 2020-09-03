
datablock ItemData(PickaxeItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/Pickaxe";
	shapeFile = "./resources/Pickaxe.dts";
	uiName = "Pickaxe";

	image = "PickaxeImage";
	colorShiftColor = "0.4 0 0 1";

	cost = 1000;
};

datablock ShapeBaseImageData(PickaxeImage)
{
	shapeFile = "./resources/Pickaxe.dts";
	emap = true;

	item = PickaxeItem;
	doColorShift = true;
	colorShiftColor = "0.4 0 0 1";

	armReady = true;

	toolTip = "Mine coal and other minerals";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
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
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));

	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);
	if (isObject(%hit))
	{
		%hit.onPickaxeHit(%obj);
		%p = new Projectile() {
			dataBlock = hammerProjectile;
			initialPosition(getWords(%ray, 1, 3));
		};
		%p.explode();
	}
}

function fxDTSBrick::onPickaxeHit(%this, %pl)
{
	%pl = %cl.player;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onPickaxeHit", %client);
}

registerInputEvent("fxDTSBrick", "onPickaxeHit", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");