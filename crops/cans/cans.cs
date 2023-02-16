function SeedImage::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function SeedImage::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DataString_CanItem(%uiname,%safeName,%itemName,%imageName,%shape,%color)
{
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

function DataString_CanImage(%uiname,%imageName,%itemName,%shape,%color,%cropType)
{
	return "datablock ShapeBaseImageData(" @ %imageName @ ")"
	@"{"
		@"className = \"CanImage\";"
		@"shapeFile = \"" @ %shape @ "\";"
		@"emap = true;"
		@"doColorShift = true;"
		@"colorShiftColor = \"" @ %color @ "\";"
		@"item = \"" @ %itemName @ "\";"
		@"armReady = 1;"
		@"cropBrick = \"" @ %cropBrick @ "\";"
		@"cropType = \"" @ %cropType @ "\";"
		@"stateName[0] = \"Activate\";"
		@"stateTransitionOnTimeout[0] = \"Loop\";"
		@"stateTimeoutValue[0] = 0.1;"
		@"stateName[1] = \"Loop\";"
		@"stateScript[1] = \"onLoop\";"
		@"stateTransitionOnTriggerDown[1] = \"Fire\";"
		@"stateTimeoutValue[1] = 0.1;"
		@"stateTransitionOnTimeout[1] = \"Loop\";"
		@"stateWaitForTimeout[1] = false;"
		@"stateName[3] = \"Fire\";"
		@"stateScript[3] = \"onFire\";"
		@"stateTransitionOnTriggerUp[3] = \"Loop\";"
		@"stateTimeoutValue[2] = 0.1;"
	@"};";
}

function SeedDatablocks(%name,%color)
{
	%displayName = "Canned" SPC %name;
	%safeName = getSafeVariableName("Canned" @ %name);
	%itemName = %safeName @ "Item";
	%imageName = %safeName @ "0Image";
	%singleShape = expandFileName("./can1.dts");
	eval(DataString_SeedItem(%displayName,%safeName,%itemName,%imageName,%singleShape,%color));

	%cropBrick = "brick" @ %name @ "0CropData";

	%count = 3;
	$Stackable_[%safeName @ "_StackedItemTotal"] = %count;
	for(%i = 0; %i < %count; %i++)
	{
		%uiName = %displayName @ %i;
		%itemName = %safeName @ %i @ "Item";
		%imageName = %safeName @ %i @ "Image";
		%shape = expandFileName("./can" @ (%i + 1) @ ".dts");

		$Stackable_[%safeName @ "_StackedItem" @ %i] = %itemName SPC getMin(5 * (%i + 1) - 1, 10); // 4, 9, 10
		eval(DataString_CanItem(%uiname,%safeName,%itemName,%imageName,%shape,%packColor));
		eval(DataString_CanImage(%uiname,%imageName,%itemName,%shape,%packColor,%cropBrick,%name));
	}
}

//tree
SeedDatablocks("Apple","0.24 0.12 0.487 1");
SeedDatablocks("Date","0.412 0.192 0 1");
SeedDatablocks("Mango","1 0.83 0 1");
SeedDatablocks("Peach","1 0.603 0.419 1");

//underground
SeedDatablocks("Carrot","1 0.502 0 1");
SeedDatablocks("Onion","1 0.96 0.74 1");
SeedDatablocks("Potato","0.568 0.329 0 1");
SeedDatablocks("Turnip","0.9 0.9 0.9 1");

//aboveground
SeedDatablocks("Blueberry","0.24 0.12 0.487 1");
SeedDatablocks("Cabbage","0 0.4 0.2 1");
SeedDatablocks("Cactus","0.329 0.419 0.184 1");
SeedDatablocks("Chili","0.9 0 0 1");
SeedDatablocks("Corn","0.95 0.85 0 1");
SeedDatablocks("Daisy","0.568 0.329 0 1");
SeedDatablocks("Lily","0.568 0.329 0 1");
SeedDatablocks("Portobello","0.08 0.08 0.08 1");
SeedDatablocks("Rose","0.568 0.06 0 1");
SeedDatablocks("Strawberry","1 0 0 1");
SeedDatablocks("Tomato","1 0 0 1");
SeedDatablocks("Watermelon","0.094 0.2 0.149 1");
SeedDatablocks("Wheat","1 0.96 0.74 1");
