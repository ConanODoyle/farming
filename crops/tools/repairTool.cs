// datablock
datablock ItemData(RepairToolItem : HammerItem)
{
	shapeFile = "./redtools/blowtorch.dts";
	uiName = "Repair Tool";
	image = "RepairToolImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;

	hasDataID = 1;
	isDataIDTool = 1;

	isRepairTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	baseDurability = 10;
	chanceDurability = 0.8;
	bonusDurability = 1;

	iconName = "Add-ons/Server_Farming/icons/RepairTool";
};

datablock ShapeBaseImageData(RepairToolImage)
{
	shapeFile = "./redtools/blowtorch.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = RepairToolItem.colorShiftColor;

	item = "RepairToolItem";

	armReady = 1;

	repairPercent = 0.25;
	minRepairAmount = 20;

	toolTip = "Repairs other tools";

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
	stateTimeoutValue[2] = 1;
	stateWaitForTimeout[2] = true;

	stateName[3] = "Ready2";
	stateTransitionOnTimeout[3] = "Ready";
	stateTimeoutValue[3] = 0.2;
	stateScript[3] = "onReady";
	stateTransitionOnTriggerDown[3] = "Fire";
	stateWaitForTimeout[3] = false;
};

// functions
function RepairToolImage::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%this, %obj, %slot);
		%cl.centerprint("\n<just:right><color:cccccc>Repairs left: " @ %durability @ " ", 1);
	}
}

function RepairToolImage::onFire(%this, %obj, %slot)
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

	%raycastResult = containerRayCast(%start, %end, %mask, %exempt);
	%hit = getWord(%raycastResult, 0);
	if (!isObject(%hit) || %hit.getClassName() !$= "Item")
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("You must use the repair tool on a dropped tool!", 1);
		}
		return;
	}

	%otherID = %hit.dataID;

	if (!%hit.getDatablock().isDataIDTool || getDataIDMaxDurability(%otherID) == 0)
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("That item doesn't have durability!", 1);
		}
		return;
	}

	if (%hit.getDatablock().isRepairTool)
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("You can't repair a repair tool!", 1);
		}
		return;
	}

	if (getDataIDDurability(%otherID) < getDataIDMaxDurability(%otherID))
	{
		useDurability(%this, %obj, %slot);

		%otherMaxDurability = getDataIDMaxDurability(%otherID);
		%durabilityToAdd = mCeil(getMax(%this.minRepairAmount, %this.repairPercent * %otherMaxDurability));

		%repairAmount = incDurability(%otherID, %durabilityToAdd);
	}
	else
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("That tool doesn't need repairs!", 1);
		}
		return;
	}

	%obj.playThread(0, "plant");

	if (getDurability(%this, %obj, %slot) <= 0)
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("<color:ffffff>Repaired <color:00ff00>" @ %repairAmount @ " durability<color:ffffff>, <color:ff0000>but your repair tool broke!", 1);
		}
		%obj.farmingRemoveItem(%obj.currTool);
	}
	else
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("<color:ffffff>Repaired <color:00ff00>" @ %repairAmount @ " durability<color:ffffff>!", 1);
		}
	}
}