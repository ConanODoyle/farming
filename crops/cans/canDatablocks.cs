function CanImage::onMount(%this, %obj, %slot)
{
	if (%obj.toolStackCount[%obj.currTool] > 1)
	{
		%obj.playThread(1, armReadyBoth);
	}
}

function CanImage::onUnmount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyRight);
}

function CanImage::onFire(%this, %obj, %slot)
{
	%count = %obj.toolStackCount[%obj.currTool];
	if (%count > 1)
	{
		%obj.toolStackCount[%obj.currTool]--;

		%stackType = %this.item.stackType;
		%bestItem = getStackTypeDatablock(%stackType, %obj.toolStackCount[%obj.currTool]);

		%obj.tool[%obj.currTool] = %bestItem.getID();
		messageClient(%obj.client, 'MsgItemPickup', '', %obj.currTool, %bestItem.getID());
		%obj.mountImage(%bestItem.image, 0);
	}
	else
	{
		%obj.farmingRemoveItem(%obj.currTool);
	}

	%obj.farmingAddStackableItem(%this.cannedCropType, getMaxStack(%this.cannedCropType));
}

function CanImage::onLoop(%this, %obj, %slot)
{
	%type = "Canned " @ %this.cropType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right>\c3-Can " @ %obj.currTool + 1 @ "- \n\c3" @ %type @ "\c6: " @ %count @ " \n\c5Click to uncan ", 1);
	}
}

function DataString_CanItem(%cropType,%shape,%color,%index)
{
	%displayName = "Canned" SPC %cropType;
	%uiName = %displayName @ %index;
	%safeName = getSafeVariableName("Canned" @ %cropType);
	%itemName = %safeName @ %index @ "Item";
	%imageName = %safeName @ (%index + 0) @ "Image";

	return ""
	@"datablock ItemData(" @ %itemName @ ")"
	@"{"
		@"category = \"Tools\";"
		@"className = \"Weapon\";"
		@"shapeFile = \"" @ %shape @ "\";"
		@"mass = 1;density = 0.2;"
		@"elasticity = 0.2;"
		@"friction = 0.6;"
		@"emap = 1;"
		@"uiName = \"" @ %uiName @ "\";"
		@"image = \"" @ %imageName @ "\";"
		@"doColorShift = true;"
		@"colorShiftColor = \"" @ %color @ "\";"
		@"canDrop = 1;"
		@"isStackable = 1;"
		@"stackType = \"" @ %safeName @ "\";"
	@"};";
}

function DataString_CanImage(%cropType,%shape,%color,%index)
{
	%safeName = getSafeVariableName("Canned" @ %cropType);
	%itemName = %safeName @ %index @ "Item";
	%imageName = %safeName @ (%index + 0) @ "Image";
	return "datablock ShapeBaseImageData(" @ %imageName @ ")"
	@"{"
		@"className = \"CanImage\";"
		@"shapeFile = \"" @ %shape @ "\";"
		@"emap = true;"
		@"doColorShift = true;"
		@"colorShiftColor = \"" @ %color @ "\";"
		@"item = \"" @ %itemName @ "\";"
		@"armReady = 1;"
		@"cannedCropType = \"" @ %cropType @ "\";"
		@"stateName[0] = \"Activate\";"
		@"stateTransitionOnTimeout[0] = \"Loop\";"
		@"stateTimeoutValue[0] = 0.3;"

		@"stateName[1] = \"Loop\";"
		@"stateScript[1] = \"onLoop\";"
		@"stateTransitionOnTriggerDown[1] = \"Fire\";"
		@"stateTimeoutValue[1] = 0.1;"
		@"stateTransitionOnTimeout[1] = \"Loop\";"
		@"stateWaitForTimeout[1] = false;"

		@"stateName[2] = \"Fire\";"
		@"stateScript[2] = \"onFire\";"
		@"stateTransitionOnTriggerUp[2] = \"Loop\";"
		@"stateTimeoutValue[2] = 0.1;"
	@"};";
}

function CanDatablocks(%cropType,%color)
{
	%safeName = getSafeVariableName("Canned" @ %cropType);

	%shape = expandFileName("./can1.dts");
	eval(DataString_CanItem(%cropType,%shape,%color));

	%count = 3;
	$Stackable_[%safeName @ "_StackedItemTotal"] = %count;
	for(%i = 0; %i < %count; %i++)
	{
		%uiName = %displayName @ %i;
		%itemName = %safeName @ %i @ "Item";
		%imageName = %safeName @ %i @ "Image";
		%shape = expandFileName("./can" @ (%i + 1) @ ".dts");

		$Stackable_[%safeName @ "_StackedItem" @ %i] = %itemName SPC getMax(%i * 5, 1); // 1, 5, 10
		eval(DataString_CanItem(%cropType,%shape,%color,%i));
		eval(DataString_CanImage(%cropType,%shape,%color,%i));
	}
}

//tree
CanDatablocks("Apple","1 0 0 1");
CanDatablocks("Date","0.412 0.192 0 1");
CanDatablocks("Mango","1 0.83 0 1");
CanDatablocks("Peach","1 0.603 0.419 1");

//underground
CanDatablocks("Carrot","1 0.502 0 1");
CanDatablocks("Onion","1 0.96 0.74 1");
CanDatablocks("Potato","0.568 0.329 0 1");
CanDatablocks("Turnip","0.9 0.9 0.9 1");

//aboveground
CanDatablocks("Blueberry","0.24 0.12 0.487 1");
CanDatablocks("Cabbage","0 0.4 0.2 1");
CanDatablocks("Cactus","0.329 0.419 0.184 1");
CanDatablocks("Chili","0.9 0 0 1");
CanDatablocks("Corn","0.95 0.85 0 1");
CanDatablocks("Portobello","0.08 0.08 0.08 1");
CanDatablocks("Strawberry","1 0 0 1");
CanDatablocks("Tomato","1 0 0 1");
CanDatablocks("Watermelon","0.094 0.2 0.149 1");
CanDatablocks("Wheat","1 0.96 0.74 1");
