datablock ItemData(PlanterItem : HammerItem)
{
	shapeFile = "./redtools/planter.dts";
	iconName = "Add-Ons/Server_Farming/icons/Planter_Icon";
	uiName = "Seed Planter";

	hasDataID = 1;
	isDataIDTool = 1;

	durability = 1000;

	doColorshift = false;
	colorShiftColor = "0.4 0 0 1";
	image = PlanterImage;
};

datablock ShapeBaseImageData(PlanterImage)
{
	shapeFile = "./redtools/planter.dts";
	emap = true;

	doColorshift = false;
	colorShiftColor = PlanterItem.colorShiftColor;

	item = PlanterItem;

	armReady = true;

	tooltip = "Plants up to 6 seeds in a row";

	min = 6;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";
	stateWaitForTimeout[1] = false;

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "LoopA";
	stateTimeoutValue[3] = 0.1;
	stateWaitForTimeout[3] = true;
	stateWaitForTimeout[3] = false;
};

datablock ItemData(PlanterV2Item : HammerItem)
{
	shapeFile = "./bluetools/planter.dts";
	iconName = "Add-Ons/Server_Farming/icons/Planter_V2_Icon";
	uiName = "Seed Planter V2";

	hasDataID = 1;
	isDataIDTool = 1;

	durability = 1000;

	doColorshift = false;
	colorShiftColor = "0 0 0.4 1";
	image = PlanterV2Image;
};

datablock ShapeBaseImageData(PlanterV2Image)
{
	shapeFile = "./bluetools/planter.dts";
	emap = true;

	doColorshift = false;
	colorShiftColor = PlanterV2Item.colorShiftColor;

	item = PlanterV2Item;

	armReady = true;

	tooltip = "Plants up to 18 seeds in a row";

	min = 18;

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
	stateTimeoutValue[3] = 0.1;
	stateWaitForTimeout[3] = true;
};

function PlanterImage::onLoop(%this, %obj, %slot)
{
	%cl = %obj.client;
	%objDB = %obj.getDatablock();
	%count = 0;
	for (%i = 0; %i < %objDB.maxTools; %i++)
	{
		%item = %obj.tool[%i];
		%stackCount = %obj.toolStackCount[%i];
		%stackType = %item.stackType;

		if (%stackType !$= "" && getSubStr(%stackType, strLen(%stackType) - 4, 4) $= "Seed")
		{
			if (%count > 4)
			{
				%line[%count++ - 1] = "and more...";
				break;
			}
			%seedName = strReplace(getSubStr(%stackType, 0, strLen(%stackType) - 4), "_", " ");
			%idx = %i;
			%line[%count++ - 1] = %seedName @ ": " @ %stackCount @ " <br>";
		}
	}

	for (%i = 0; %i < %count; %i++)
	{
		%append = %append @ %line[%i];
	}

	%durability = getDurability(%this, %obj, %slot);
	%durabilityString = "<color:cccccc>Durability: " @ %durability;

	if (%idx !$= "")
	{
		%cl.centerprint("<just:right>\c3-Row " @ %this.item.uiname @"- \n" @ %durabilityString @ " \n\c6" @ %append, 1);
	}
	else
	{
		%cl.centerprint("<just:right>\c3-Row " @ %this.item.uiname @"- \n" @ %durabilityString @ " \n\c0No seeds in inventory! ", 1);
	}
}

function PlanterImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);
	if (getDurability(%this, %obj, %slot) == 0)
	{
		if (isObject(%cl = %obj.client))
		{
			%durability = getDurability(%this, %obj, %slot);
			%durabilityString = "<color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!";
			%cl.centerprint("<just:right>\c3-Row " @ %this.item.uiname @"- \n" @ %durabilityString, 1);
		}
		return;
	}

	%cl = %obj.client;
	%objDB = %obj.getDatablock();
	%count = 0;
	for (%i = 0; %i < %objDB.maxTools; %i++)
	{
		%item = %obj.tool[%i];
		%stackCount = %obj.toolStackCount[%i];
		%stackType = %item.stackType;

		if (%stackType !$= "" && getSubStr(%stackType, strLen(%stackType) - 4, 4) $= "Seed")
		{
			// %idx = %i;
			// break;
			%seed[%count++ - 1] = %i TAB %stackCount TAB %stackType TAB %item;
		}
	}

	if (%count == 0)//(%idx $= "" || %stackCount <= 0)
	{
		return;
	}

	%objDir = %obj.getForwardVector();
	if (mAbs(getWord(%objDir, 0)) > mAbs(getWord(%objDir, 1)))
	{
		%neg = getWord(%objDir, 0) < 0 ? -1 : 1;
		%plantingDir = %neg SPC "0 0";
	}
	else
	{
		%neg = getWord(%objDir, 1) < 0 ? -1 : 1;
		%plantingDir = "0" SPC %neg SPC "0";
	}
	//actual fire check starts here
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);


	if (isObject(%ray))
	{
		%hitloc = getWords(%ray, 1, 3);
		%plantCount = 0;
		%plantMax = %this.min;
		%originalCurrTool = %obj.currTool;
		%greenhouseCheck = getWord(containerRaycast(%hitloc, vectorAdd(%hitloc, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType), 0);
		%inGreenhouse = (isObject(%greenhouseCheck) && %greenhouseCheck.getDatablock().isGreenhouse);

		// talk("planter found " @ %count);
		for (%i = 0; %i < %count; %i++)
		{
			%currSlot = getField(%seed[%i], 0);
			%currStackCount = getField(%seed[%i], 1);
			%currStackType = getField(%seed[%i], 2);
			%currItem = getField(%seed[%i], 3);

			%brickDB = %currItem.image.cropBrick;
			%plantRadius = getPlantData(%brickDB.cropType, "plantSpace") * 0.5 + 0.5 - (%inGreenhouse * 0.5);
			%obj.currTool = %currSlot;

			%plantingSpace = vectorScale(%plantingDir, %plantRadius);
			// talk("Planting items idx: " @ %i @ " currPlantcount: " @ %plantCount);
			// talk("type: " @ %currStackType @ " count: " @ %currStackCount);
			if (%plantCount < %plantMax)
			{
				%total = getMin(%currStackCount, %plantMax - %plantCount);
				for (%j = 0; %j < %total; %j++)
				{
					if (%plantCount > 0)
					{
						%hitloc = vectorAdd(%hitloc, %plantingSpace);
						%greenhouseCheck = getWord(containerRaycast(%hitloc, vectorAdd(%hitloc, "0 0 300"), $TypeMasks::fxBrickAlwaysObjectType), 0);
						if (%inGreenhouse != (isObject(%greenhouseCheck) && %greenhouseCheck.getDatablock().isGreenhouse))
						{
							%obj.currTool = %originalCurrTool;
							return;
						}
					}
					%valid = plantCrop(%curritem.image, %obj, "", %hitloc);
					if (!%valid) //could not plant, immediately exit
					{
						%obj.currTool = %originalCurrTool;
						return;
					}
					%obj.currTool = %originalCurrTool;
					useDurability(%this, %obj, %slot);
					%plantCount++;
					if (getDurability(%this, %obj, %slot) == 0)
					{
						if (isObject(%cl = %obj.client))
						{
							%durability = getDurability(%this, %obj, %slot);
							%durabilityString = "<color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!";
							%cl.centerprint("<just:right>\c3-Row " @ %this.item.uiname @"- \n" @ %durabilityString, 1);
						}
						return;
					}
					%obj.currTool = %currSlot;
					// %shape = createBoxAt(%hitloc, "1 0 0 1", 0.1);
					// %shape.schedule(2000, delete);
				}
				// talk("currPlantcount: " @ %plantCount);
			}
			else //plantcount >= plantMax,  we maxed out
			{
				%obj.currTool = %originalCurrTool;
				return;
			}
		}
		%obj.currTool = %originalCurrTool; //just in case
		// %brickDB = %item.image.cropBrick;

		// %min = %this.min;
		// %currTool = %obj.currTool; //dangerous... but we will be careful to reset it back to its original value!
		// %obj.currTool = %idx;
		// for (%i = 0; %i < getMin(%min, %stackCount); %i++)
		// {
		//  %p = vectorAdd(%hitloc, vectorScale(%plantingSpace, %i));
		//  %valid = plantCrop(%item.image, %obj, "", %p);
		//  if (!%valid)
		//  {
		//      %obj.currTool = %currTool;
		//      return;
		//  }
		//  // %shape = createBoxAt(%p, "1 0 0 1", 0.1);
		//  // %shape.schedule(2000, delete);
		// }
		// %obj.currTool = %currTool;
	}
}

function PlanterV2Image::onLoop(%this, %obj, %slot)
{
	PlanterImage::onLoop(%this, %obj, %slot);
}

function PlanterV2Image::onFire(%this, %obj, %slot)
{
	PlanterImage::onFire(%this, %obj, %slot);
}
