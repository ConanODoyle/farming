datablock ItemData(ShovelItem : HammerItem)
{
	shapeFile = "./redtools/shovel.dts";
	uiName = "Shovel";
	image = "ShovelImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;

	hasDataID = 1;
	isDataIDTool = 1;
	
	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 200;
	chanceDurability = 0.8;
	bonusDurability = 20;

	iconName = "Add-ons/Server_Farming/icons/Shovel";
};

datablock ShapeBaseImageData(ShovelImage)
{
	shapeFile = "./redtools/shovel.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = ShovelItem.colorShiftColor;

	item = "ShovelItem";

	armReady = 1;

	toolTip = "Reclaims nutrients from soil";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "Ready";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.2;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateWaitForTimeout[1] = false;

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";

	stateName[3] = "Ready2";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.2;
	stateScript[3] = "onReady";
	stateTransitionOnTriggerDown[3] = "Fire";
	stateWaitForTimeout[3] = false;
};

function ShovelImage::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%this, %obj, %slot);
		%cl.centerprint("\n<just:right><color:cccccc>Durability: " @ %durability @ " ", 1);
	}
}

function ShovelImage::onFire(%this, %obj, %slot)
{
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));

	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::PlayerObjectType, %obj);
	%hit = getWord(%ray, 0);
	if (isObject(%hit))
	{
		if (%hit.getClassName() $= "fxDTSBrick")
		{
			%hit.onShovelHit(%obj);
		}
		%p = new Projectile() {
			dataBlock = swordProjectile;
			initialPosition = getWords(%ray, 1, 3);
			initialVelocity = vectorScale(getWords(%ray, 4, 6), -1);
		};
		%p.explode();

		if (getDurability(%this, %obj, %slot) == 0)
		{
			%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!", 1);
			return;
		}

		if (%hit.getDatablock().isDirt)
		{
			%nutrients = %hit.getNutrients();
			%nit = getWord(%nutrients, 0);
			%pho = getWord(%nutrients, 1);
			if (%nit >= CompostBag0Image.fertilizerNitrogen + 1 
				|| %pho >= PhosphateBag0Image.fertilizerPhosphate + 1)
			{
				useDurability(%this, %obj, %slot);
				if (%nit >= CompostBag0Image.fertilizerNitrogen + 1)
				{
					%vel = (getRandom(12) - 6) / 4 SPC  (getRandom(12) - 6) / 4 SPC 6;
					%nit -= CompostBag0Image.fertilizerNitrogen + 1;
					%item = new Item()
					{
						dataBlock = CompostBag0Item;
						count = 1;
						client = %obj.client;
					};
					%item.setTransform(getWords(%ray, 1, 3) SPC getRandomRotation());
					%item.setVelocity(%vel);
					%item.schedulePop();
				}
				if (%pho >= PhosphateBag0Image.fertilizerPhosphate + 1)
				{
					%vel = (getRandom(12) - 6) / 4 SPC  (getRandom(12) - 6) / 4 SPC 6;
					%pho -= PhosphateBag0Image.fertilizerPhosphate + 1;
					%item = new Item()
					{
						dataBlock = PhosphateBag0Item;
						count = 1;
						client = %obj.client;
					};
					%item.setTransform(getWords(%ray, 1, 3) SPC getRandomRotation());
					%item.setVelocity(%vel);
					%item.schedulePop();
				}
			}
			%hit.setNutrients(%nit, %pho);
		}
	}
	%obj.playThread(0, shiftDown);

}

function fxDTSBrick::onShovelHit(%this, %pl)
{
	%cl = %pl.client;

	$InputTarget_["Self"] = %this;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);

	%this.processInputEvent("onShovelHit", %client);
}
registerInputEvent("fxDTSBrick", "onShovelHit", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");