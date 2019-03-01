///////////////
//Compost Bin//
///////////////

datablock fxDTSBrickData(brickGrindstoneData)
{
	category = "";
	subCategory = "";
	uiName = "Grindstone";

	brickFile = "./resources/grindstone.blb";

	// cost = 800;
	isProcessor = 1;
	processorFunction = "grindIntoProduct";
	activateFunction = "grindstoneInfo";
};


////////
//Item//
////////

datablock ItemData(GrindstoneItem : HammerItem)
{
	shapeFile = "./resources/grindstoneItem.dts";
	uiName = "Grindstone";
	image = "GrindstoneImage";
	colorShiftColor = "0.5 0.5 0.5 1";

	iconName = "";

	isSellable = 1;
	cost = 1500;
};

datablock ShapeBaseImageData(GrindstoneImage)
{
	shapeFile = "./resources/grindstone.dts";
	emap = true;

	item = GrindstoneItem;
	armReady = 1;

	doColorshift = true;
	colorShiftColor = GrindstoneItem.colorShiftColor;

	toolTip = "Brick - grinds produce into product";
	mountPoint = 0;

	placeBrick = "brickGrindstoneData";

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
	stateTransitionOnTimeout[3]	= "LoopA";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = true;
};

function GrindstoneImage::onMount(%this, %obj, %slot)
{
	%obj.canPlaceGrindstone = 1;
	%obj.client.canPlaceGrindstone = 1;
	%obj.playThread(1, armReadyBoth);
}

function GrindstoneImage::onUnmount(%this, %obj, %slot)
{
	%obj.canPlaceGrindstone = 0;
	%obj.client.canPlaceGrindstone = 0;
	if (isObject(%obj.tempbrick))
	{
		%obj.tempbrick.delete();
	}
}

function GrindstoneImage::onLoop(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("<color:ffff00>-Grindstone-<br><color:ffffff>Click to place ghost brick", 1);
	}
}

function GrindstoneImage::onFire(%this, %obj, %slot)
{
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 4));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::fxPlaneType);
	if (isObject(%hit = getWord(%ray, 0)))
	{
		%currInv = %obj.client.currInv;
		%currData = %obj.client.instantUseData;
		%obj.client.currInv = 0;
		%obj.client.instantUseData = %this.placeBrick;

		%p = new Projectile() {
			initialPosition = getWords(%ray, 1, 3);
			client = %obj.client;
		};
		brickDeployProjectile::onCollision(%p, %hit, "0 0 0", getWords(%ray, 1, 3), getWords(%ray, 4, 6));
		if (isObject(%p))
		{
			%p.delete();
		}

		%obj.client.currInv = %currInv;
		%obj.client.instantUseData = %currData;
	}
}