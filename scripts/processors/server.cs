//Exec the processors using this support module.
//Define core datablocks first
datablock ItemData(BrickPlacerItem : HammerItem)
{
	// shapeFile = "./resources/FlowerpotItem.dts";
	uiName = "";
	image = "BrickPlacerImage";
	// colorShiftColor = "0.5 0.5 0.5 1";

	// iconName = "";

	isSellable = 1;
	// cost = 100;
	canPickupMultiple = 1;
};


datablock ShapeBaseImageData(BrickPlacerImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	// offset = "-0.56 0 -0.25";
	// eyeOffset = "0 0 0";

	item = "BrickPlacerItem";
	armReady = 1;

	doColorshift = true;
	colorShiftColor = "1 1 1 1";

	toolTip = "Brick-Placement Item";
	mountPoint = 0;

	// placeBrick = "brickFlowerpotData";

	projectile = "brickDeployProjectile";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "LoopA";
	// stateTransitionOnTimeout[3]	= "LoopA";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = true;
};


exec("./fertilizer.cs");
exec("./flowerpot.cs");

function brickPlacerItem_onMount(%this, %obj, %slot)
{
	%obj.itemBrickSelection = %this.placeBrick.getID();
	%obj.client.itemBrickSelection = %this.placeBrick.getID();
	%obj.playThread(1, armReadyBoth);
}

function brickPlacerItem_onUnmount(%this, %obj, %slot)
{
	%obj.itemBrickSelection = 0;
	%obj.client.itemBrickSelection = 0;
	if (isObject(%obj.tempbrick))
	{
		%obj.tempbrick.delete();
	}
}

function brickPlacerItemLoop(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("<color:ffff00>-" @ %this.item.uiname @ "-<br><color:ffffff>Click to place ghost brick <br><color:ffffff>" @ %this.loopTip @ " ", 1);
	}
}

function brickPlacerItemFire(%this, %obj, %slot)
{
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::StaticObjectType | $Typemasks::StaticShapeObjectType);
	if (isObject(%hit = getWord(%ray, 0)))
	{
		// %obj.tempbrick.delete();
		%currInv = %obj.client.currInv;
		%currData = %obj.client.instantUseData.getID();
		%obj.client.currInv = "";
		%obj.client.instantUseData = %this.placeBrick;

		%p = new Projectile() {
			dataBlock = %this.projectile;
			initialPosition = vectorSub(getWords(%ray, 1, 3), %obj.getEyeVector());
			initialVelocity = vectorScale(%obj.getEyeVector(), 100);
			client = %obj.client;
			sourceObject = %obj;
			sourceClient = %obj.client;
		};
	}
}