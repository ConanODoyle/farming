///////////////
//Compost Bin//
///////////////

datablock fxDTSBrickData(brickFlowerPotData : brick1x1Data) 
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Flower Pot";

	brickFile = "./resources/flowerPot.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/flower_pot";

	cost = 0;
	isDirt = 1;
	maxWater = 30;
	isPot = 1;
	customRadius = -1.05;
	placerItem = "FlowerPotItem";
	isProcessor = 1;
};


////////
//Item//
////////

datablock ItemData(FlowerPotItem : HammerItem)
{
	shapeFile = "./resources/FlowerpotItem.dts";
	uiName = "Flower Pot";
	image = "FlowerpotBrickImage";
	colorShiftColor = "0.5 0.5 0.5 1";

	iconName = "";

	isSellable = 1;
	cost = 100;
};

datablock ShapeBaseImageData(FlowerpotBrickImage)
{
	shapeFile = "./resources/FlowerpotItem.dts";
	emap = true;

	offset = "-0.56 0 -0.25";
	eyeOffset = "0 0 0";

	item = FlowerpotItem;
	armReady = 1;

	doColorshift = true;
	colorShiftColor = FlowerpotItem.colorShiftColor;

	toolTip = "Brick - grinds produce into product";
	mountPoint = 0;

	placeBrick = "brickFlowerpotData";

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

function FlowerpotBrickImage::onMount(%this, %obj, %slot)
{
	%obj.itemBrickSelection = %this.placeBrick.getID();
	%obj.client.itemBrickSelection = %this.placeBrick.getID();
	%obj.playThread(1, armReadyBoth);
}

function FlowerpotBrickImage::onUnmount(%this, %obj, %slot)
{
	%obj.itemBrickSelection = 0;
	%obj.client.itemBrickSelection = 0;
	if (isObject(%obj.tempbrick))
	{
		%obj.tempbrick.delete();
	}
}

function FlowerpotBrickImage::onLoop(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("<color:ffff00>-Flowerpot-<br><color:ffffff>Click to place ghost brick", 1);
	}
}

function FlowerpotBrickImage::onFire(%this, %obj, %slot)
{
	// talk("Firing");
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
		// brickDeployProjectile::onCollision(%p, %hit, "0 0 0", getWords(%ray, 1, 3), getWords(%ray, 4, 6));
		// if (isObject(%p))
		// {
		// 	%p.delete();
		// }
		// talk(%p.getDatablock().getName());
		// brickImage::onFire(brickImage, %obj, %slot);

		// %obj.client.currInv = %currInv;
		// %obj.client.instantUseData = %currData;
	}
}