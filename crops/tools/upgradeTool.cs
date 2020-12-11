// datablock
datablock ItemData(UpgradeToolItem : HammerItem)
{
	shapeFile = "./bluetools/blowtorch.dts";
	uiName = "Upgrade Tool";
	image = "UpgradeToolImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;

	isRepairTool = 1;

	iconName = "Add-ons/Server_Farming/icons/RepairTool";
};

datablock ShapeBaseImageData(UpgradeToolImage)
{
	shapeFile = "./bluetools/blowtorch.dts";
	emap = true;

	doColorShift = false;
	colorShiftColor = RepairToolItem.colorShiftColor;

	item = "UpgradeToolItem";

	armReady = 1;

	upgradePercent = 0.25;
	minUpgradeAmount = 20;

	toolTip = "Upgrades other tools' durability";

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
function UpgradeToolImage::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%this, %obj, %slot);
		%cl.centerprint("\n<just:right><color:cccccc>Upgrades left: 1", 1);
	}
}

function UpgradeToolImage::onFire(%this, %obj, %slot)
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
	if (!isObject(%hit))
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("You must use the upgrade tool on a dropped tool!", 1);
		}
		return;
	}

	%otherID = %hit.dataID;

	if (!%hit.getDatablock().isDataIDTool || getDataIDMaxDurability(%otherID) == 0)
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("That tool doesn't have durability!", 1);
		}
		return;
	}

	if (%hit.isRepairTool)
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("You can't upgrade a repair tool!", 1);
		}
		return;
	}

	if (getDataIDArrayTagValue(%otherID, "hasDurabilityUpgrade"))
	{
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("That tool is already upgraded!", 1);
		}
		return;
	}
	else
	{
		useDurability(%this, %obj, %slot);

		%otherMaxDurability = getDataIDMaxDurability(%otherID);
		%durabilityToAdd = mCeil(getMax(%this.minUpgradeAmount, %this.upgradePercent * %otherMaxDurability));

		%upgradeAmount = incMaxDurability(%otherID, %durabilityToAdd);
		incDurability(%otherID, %upgradeAmount);
		setDataIDArrayTagValue(%otherID, "hasDurabilityUpgrade", 1);
		if (isObject(%cl = %obj.client))
		{
			%cl.centerprint("<color:ffffff>Added <color:00ffff>" @ %upgradeAmount @ " max durability<color:ffffff>!", 1);
		}
		%obj.farmingRemoveItem(%obj.currTool);
	}
}