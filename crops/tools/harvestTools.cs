$statTrakBaseBonusModifier = 200;
$statTrakBonusReductionMultiplier = 1.8;
//harvest tools

datablock ItemData(TrowelItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/Trowel";
	shapeFile = "./redtools/trowel.dts";
	uiName = "Trowel";
	canPickupMultiple = 1;

	hasDataID = 1;
	isDataIDTool = 1;
	toolType = "Trowel";

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 200;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "TrowelImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;
};

datablock ShapeBaseImageData(TrowelImage)
{
	shapeFile = "./redtools/Trowel.dts";
	emap = true;

	className = "HarvestToolImage";

	item = TrowelItem;
	doColorShift = false;
	colorShiftColor = "0.4 0 0 1";

	armReady = true;

	toolTip = "+1 harvest to below ground crops";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateWaitForTimeout[1] = 0;
	stateTimeoutValue[1] = 0.2;
	stateScript[1] = "onReady";
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

	stateName[4] = "Ready2";
	stateTransitionOnTimeout[4] = "Ready";
	stateWaitForTimeout[4] = 0;
	stateTimeoutValue[4] = 0.2;
	stateScript[4] = "onReady";
	stateTransitionOnTriggerDown[4] = "Fire";
};

////

datablock ItemData(ClipperItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/Clipper";
	shapeFile = "./redtools/Clipper.dts";
	uiName = "Clipper";
	canPickupMultiple = 1;

	hasDataID = 1;
	isDataIDTool = 1;
	toolType = "Clipper";

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 200;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "ClipperImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;
};

datablock ShapeBaseImageData(ClipperImage : TrowelImage)
{
	shapeFile = "./redtools/Clipper.dts";

	item = ClipperItem;
	doColorShift = false;

	projectile = "swordProjectile";

	toolTip = "+1 harvest to above ground crops";
};

////

datablock ItemData(HoeItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/Hoe";
	shapeFile = "./redtools/Hoe.dts";
	uiName = "Hoe";
	canPickupMultiple = 1;

	hasDataID = 1;
	isDataIDTool = 1;
	toolType = "Hoe";

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "HoeImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;
};

datablock ShapeBaseImageData(HoeImage : TrowelImage)
{
	shapeFile = "./redtools/Hoe.dts";

	item = HoeItem;
	doColorShift = false;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest below ground crops";
};

////

datablock ItemData(SickleItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/Sickle";
	shapeFile = "./redtools/Sickle.dts";
	uiName = "Sickle";
	canPickupMultiple = 1;

	hasDataID = 1;
	isDataIDTool = 1;
	toolType = "Sickle";

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "SickleImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;
};

datablock ShapeBaseImageData(SickleImage : TrowelImage)
{
	shapeFile = "./redtools/Sickle.dts";

	item = SickleItem;
	doColorShift = false;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest above ground crops";
};

////

datablock ItemData(TreeClipperItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/icons/TreeClipper";
	shapeFile = "./redtools/TreeClipper.dts";
	uiName = "Tree Clipper";
	canPickupMultiple = 1;

	hasDataID = 1;
	isDataIDTool = 1;
	toolType = "Tree Clipper";

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "TreeClipperImage";
	colorShiftColor = "0.4 0 0 1";
	doColorShift = false;
};

datablock ShapeBaseImageData(TreeClipperImage : TrowelImage)
{
	shapeFile = "./redtools/TreeClipper.dts";

	item = TreeClipperItem;
	doColorShift = false;

	stateTimeoutValue[2] = 0.4;

	toolTip = "+1-5 harvest and prune trees";
};

////

function HarvestToolImage::onFire(%this, %obj, %slot, %hitLoc)
{
	%item = %this.item;

	if (%hitLoc $= "")
	{
		%obj.playThread(0, shiftDown);

		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
		%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::PlayerObjectType, %obj);
		%hitLoc = getWords(%ray, 1, 3);

		//harvest tool hit effect
		if (isObject(%hit = getWord(%ray, 0)) && !%hit.dataBlock.isPlant)
		{
			%db = isObject(%this.projectile) ? %this.projectile : swordProjectile;

			if (isObject(%db))
			{
				%p = new Projectile()
				{
					dataBlock = %db;
					initialPosition = %hitLoc;
					initialVelocity = "0 0 0";
				};
				%p.setScale("0.5 0.5 0.5");
				%p.explode();
			}
		}
		else if (!isObject(%hit = getWord(%ray, 0)))
		{
			 //dont do anything, we didnt hit any brick so we dont have a point to work off of
			return;
		}
	}

	//check durability
	//calculate stattrak bonus yield
	if (%item.hasDataID)
	{
		%durability = getDurability(%this, %obj, %slot);
		if (%durability == 0 && isObject(%cl = %obj.client))
		{
			%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!", 1);
			return;
		}
	}
	%dataID = %obj.toolDataID[%obj.currTool];

	//harvest
	if (%this.areaHarvest > 0)
	{
		//collect all targeted bricks first, otherwise explosion effects can re-run searches
		initContainerRadiusSearch(%hitLoc, %this.areaHarvest, $Typemasks::fxBrickObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next.dataBlock.isPlant && (%harvestCheck[%next.dataBlock] || canToolHarvest(%item, %next)))
			{
				%plant[%plantCount++ - 1] = %next;
				//memoize to reduce function calls
				%harvestCheck[%next.dataBlock] = 1;
			}
		}
	}
	else if (canToolHarvest(%item, %hit) || canPrune(%hit, %item))
	{
		%plant[0] = %hit;
		%plantCount = 1;
	}

	%cropTrakType = getDataIDArrayTagValue(%dataID, "statTrakType");
	%bonus = getStatTrakBonusYield(%dataID, %cropTrakType);
	//actually harvest the plants
	for (%i = 0; %i < %plantCount; %i++)
	{
		%plant = %plant[%i];
		%plantDB = %plant.dataBlock;
		%type = %plantDB.cropType;
		if (%type $= %cropTrakType || %cropTrakType $= "ALL")
		{
			%success = harvestBrick(%plant, %item, %obj, getRandom(0, %bonus));
		}
		else
		{
			%success = harvestBrick(%plant, %item, %obj);
		}

		if (%success)
		{
			if (%harvestCount[%plantDB] <= 0)
			{
				%harvestList[%harvestListCount++ - 1] = %plantDB;
			}
			%harvestCount[%plantDB]++;
			%harvestYieldCount[%plantDB] += getWord(%success, 1);
		}
	}

	//update harvest statistics on the tool
	for (%i = 0; %i < %harvestListCount; %i++)
	{
		%plantDB = %harvestList[%i];

		incrementHarvestCount(%dataID, %plantDB.cropType, %harvestCount[%plantDB]);
		incrementHarvestCount(%dataID, "ALL", %harvestCount[%plantDB]);
		incrementHarvestCount(%dataID, %plantDB.cropType @ "_yieldTotal", %harvestYieldCount[%plantDB]);
		incrementHarvestCount(%dataID, "ALL_yieldTotal", %harvestYieldCount[%plantDB]);
		setDataIDArrayTagValue(%dataID, "lastUserBLID", %obj.client.bl_id);
	}

	if (%item.hasDataID && %harvestListCount > 0)
	{
		useDurability(%this, %obj, %slot);
	}
	if (isObject(%cl = %obj.client))
	{
		centerprintHarvestToolInfo(%cl, %this, %obj, %slot);
	}
}

function HarvestToolImage::onReady(%this, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		centerprintHarvestToolInfo(%cl, %this, %obj, %slot);
	}
}

function centerprintHarvestToolInfo(%cl, %this, %obj, %slot)
{
	%durability = getDurability(%img, %obj, %slot);

	%statTrak = %obj.getToolStatTrak();
	%dataID = %obj.toolDataID[%obj.currTool];
	%cropTrakType = getDataIDArrayTagValue(%dataID, "statTrakType");
	if (%statTrak !$= "")
	{
		%string = "\c4" @ %statTrak @ " ";
		%string = %string NL "\c2Bonus yield: " @ (getStatTrakBonusYield(%dataID, %cropTrakType) + 0) @ " ";
	}

	if (%this.item.hasDataID && %dataID !$= "")
	{
		%title = %this.item.uiName @ " [" @ getSubStr(%dataID, strLen(%dataID) - 3, 3) @ "] \n";
	}

	if (getDataIDArrayTagValue(%dataID, "toolItemToggles") !$= "")
	{
		%string = %string NL "[Press Light to toggle type] ";
	}

	%cl.centerprint("<just:right>\c3" @ %title @ "<color:cccccc>Durability: " @ %durability @ " \n" @ %string, 1);
}

function getStatTrakBonusYield(%dataID, %type)
{
	if (%type $= "")
	{
		return 0;
	}
	
	if (%type $= "ALL")
	{
		%price = 4;
		%bonusCount = 1;
	}
	else
	{
		%price = getSellPrice(%type);
		%bonusCount = 1;
		if (getCropClass(%type) $= "Tree")
		{
			%price = %price / 2;
			%bonusCount = 2;
		}
	}
	
	//increase bonus threshold based on crop price and harvest count
	%totalHarvested = getDataIDArrayTagValue(%dataID, %type);
	%threshold = mFloor($statTrakBaseBonusModifier * mPow(%price, 0.75));
	while (%totalHarvested > 0 && %safety++ < 11)
	{
		%totalHarvested -= %threshold;
		if (%totalHarvested > 0)
		{
			%bonus += %bonusCount;
		}
		%threshold = mFloor(%threshold * $statTrakBonusReductionMultiplier);
	}
	return %bonus;
}

function incrementHarvestCount(%dataID, %type, %amount)
{
	%count = getDataIDArrayTagValue(%dataID, %type);
	%newAmount = IMath_add(%count, %amount);
	setDataIDArrayTagValue(%dataID, %type, %newAmount);
	return %newAmount;
}

function generateHarvestToolDurability(%item)
{
	%baseDurability = %item.baseDurability > 0 ? %item.baseDurability : 100;
	%bonusDurability = %item.bonusDurability;
	%chanceDurability = %item.chanceDurability;

	while (getRandom() < %chanceDurability)
	{
		%baseDurability += %bonusDurability;
	}

	return %baseDurability;
}

function generateHarvestToolModifiers(%item, %dataID)
{
	if (getRandom() < 0.07)
	{
		addStatTrak(%item, %dataID);
	}
}

function addStatTrak(%item, %dataID)
{
	switch$ (%item.uiName)
	{
		case "Trowel": %list = $UndergroundCropsList;
		case "Hoe": %list = $UndergroundCropsList;
		case "Clipper": %list = $AbovegroundCropsList;
		case "Sickle": %list = $AbovegroundCropsList;
		case "Tree Clipper": %list = $TreeCropsList;
		case "L4 - \"Silence\"": %list = $UndergroundCropsList;
		case "L0 - \"Remorse\"": %list = $AbovegroundCropsList;
		default: return 0;
	}
	%crop = getField(%list, getRandom(getFieldCount(%list) - 1));
	%displayAsKills = getRandom() < 0.1;

	setDataIDArrayTagValue(%dataID, "statTrakType", %crop);
	setDataIDArrayTagValue(%dataID, "displayAsKills", %displayAsKills);
	return 1 TAB "Crop: " @ %crop;
}



function Player::getToolStatTrak(%pl)
{
	if (%pl.tool[%pl.currTool].isDataIDTool)
	{
		switch (%pl.tool[%pl.currTool])
		{
			case (TrowelItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (ClipperItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (SickleItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (HoeItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (TreeClipperItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (L4SilenceItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (L0RemorseItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			default: return "";
		}
		%displayAsKills = getDataIDArrayTagValue(%dataID, "displayAsKills");
		%type = getDataIDArrayTagValue(%dataID, "statTrakType");
		%count = getDataIDArrayTagValue(%dataID, %type) + 0 | 0;

		if (%type $= "")
		{
			return "";
		}

		%word = %displayAsKills ? "kills:" : "harvests:";
		%color = %displayAsKills ? "\c0" : "\c5";
		return "[CropTrak\x99] \n" @ %color @ %type SPC %word SPC %count;
	}
	return "";
}

function getHighestToolHarvestCount(%dataID)
{
	if (%dataID $= "")
	{
		return "";
	}
	%tags = getDataIDArrayTags(%dataID);
	for (%i = 0; %i < getWordCount(%tags); %i++)
	{
		%tag = getWord(%tags, %i);
		talk(%tag);
		if (%tag $= "datablock" || %tag $= "durability" || %tag $= "maxDurability")
		{
			continue;
		}

		%count = getDataIDArrayTagValue(%dataID, %tag);
		if (%count > %maxCount)
		{
			%maxCount = %count;
			%maxType = %tag;
		}
	}

	return %maxType SPC %maxCount;
}

function toggleTool(%this, %pl, %slot)
{
	if (!%pl.tool[%pl.currTool].hasDataID || %pl.toolDataID[%pl.currTool] $= "")
	{
		return 0;
	}

	%dataID = %pl.toolDataID[%pl.currTool];

	%toggleOptions = getDataIDArrayTagValue(%dataID, "toolItemToggles");
	%count = getWordCount(%toggleOptions);

	for (%i = 0; %i < %count; %i++)
	{
		%tool = getWord(%toggleOptions, %i);
		if (%this.item.getName() $= %tool)
		{
			%found = 1;
			break;
		}
	}

	if (!%found)
	{
		return 0;
	}

	%next = (%i + 1) % %count;
	%nextTool = getWord(%toggleOptions, %next);

	if (!isObject(%nextTool))
	{
		return 0;
	}

	%nextTool = %nextTool.getID();
	%pl.tool[%pl.currTool] = %nextTool;
	messageClient(%pl.client, 'MsgItemPickup', "", %pl.currTool, %nextTool);
	%pl.unMountImage(%slot);
	%pl.mountImage(%nextTool.image, %slot);
	return 1;
}

package HarvestToolToggle
{
	function serverCmdLight(%cl)
	{
		if (isObject(%pl = %cl.player) && %pl.getMountedImage(0))
		{
			%success = toggleTool(%pl.getMountedImage(0), %pl, 0);

			if (%success)
			{
				return;
			}
		}
		return parent::serverCmdLight(%cl);
	}
};
activatePackage(HarvestToolToggle);