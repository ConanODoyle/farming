// datablock
datablock ItemData(CropTrakKitItem : HammerItem)
{
	shapeFile = "./cropTrak/croptrakClosed.dts";
	uiName = "CropTrak\x99 Upgrade Kit";
	image = "CropTrakKitImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;

	isCropTrakKit = 1;

	iconName = "Add-ons/Server_Farming/icons/CropTrakKit";
};

datablock ShapeBaseImageData(CropTrakKitImage)
{
	shapeFile = "./cropTrak/cropTrak.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = CropTrakKitItem.colorShiftColor;

	offset = "-0.54 0 -0.1";
	eyeoffset = "0.58 1.5 -0.6";
	eyeRotation = eulerToMatrix("20 0 -30");

	item = "CropTrakKitItem";

	armReady = 1;

	upgradePercent = 0.25;
	minUpgradeAmount = 20;

	toolTip = "Gives CropTrak\x99 to a tool";

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "Ready";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.2;
	stateWaitForTimeout[1] = 0;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateWaitForTimeout[1] = false;

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 1;
	stateWaitForTimeout[2] = true;

	stateName[3] = "Ready2";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = 0;
	stateScript[3] = "onReady";
	stateTransitionOnTriggerDown[3] = "Fire";
	stateWaitForTimeout[3] = false;
};

function CropTrakKitImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyBoth");
}

function CropTrakKitImage::onUnmount(%this, %obj, %slot)
{
	%obj.playThread(1, "armReadyRight");
}

// functions
function CropTrakKitImage::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("\c6Apply to a dropped tool to give it a new \c4CropTrak\x99\c6 counter!", 1);
	}
}

function CropTrakKitImage::onFire(%this, %obj, %slot)
{
	%start = %obj.getEyePoint();
	%vector = vectorScale(%obj.getEyeVector(), 10 * getWord(%obj.getScale(), 2));
	%end = vectorAdd(%start, %vector);
	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::ItemObjectType;
	if (%obj.isMounted())
	{
		%exempt = %obj.getObjectMount();
	}
	else
	{
		%exempt = %obj;
	}

	%cl = %obj.client;
	%raycastResult = containerRayCast(%start, %end, %mask, %exempt);
	%hit = getWord(%raycastResult, 0);
	if (!isObject(%hit) || %hit.getClassName() !$= "Item")
	{
		if (isObject(%cl))
		{
			%cl.centerprint("You must use this on a dropped harvesting tool!", 8);
		}
		return;
	}

	%otherID = %hit.dataID;

	%success = addStatTrak(%hit.dataBlock, %otherID);
	if (!%success)
	{
		%cl.centerprint("That item does not support \c4CropTrak\x99\c0!", 8);
		return;
	}

	if (isObject(%cl = %obj.client))
	{
		%cl.centerprint("\c6Added \c4CropTrak\x99\c6! " @ getField(%success, 1), 8);
	}
	%obj.farmingRemoveItem(%obj.currTool);
}