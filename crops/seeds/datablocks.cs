function SeedImage::onFire(%this, %obj, %slot)
{
	plantCrop(%this, %obj, %slot);
}

function SeedImage::onLoop(%this, %obj, %slot)
{
	seedLoop(%this, %obj);
}

function DataString_SeedItem(%uiname,%safeName,%itemName,%imageName,%shape,%color)
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

function DataString_SeedImage(%uiname,%imageName,%itemName,%shape,%color,%cropBrick,%cropType)
{
	return "datablock ShapeBaseImageData(" @ %imageName @ ")"
	@"{"
		@"className = \"SeedImage\";"
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
		@"stateName[2] = \"Fire\";"
		@"stateScript[2] = \"onFire\";"
		@"stateTransitionOnTriggerUp[2] = \"Loop\";"
		@"stateTimeoutValue[2] = 0.1;"
	@"};";
}

function SeedDatablocks(%name,%singleColor,%singleShape,%packColor,%tree,%stackMul)
{
	%displayName = %name SPC "Seed";
	%safeName = getSafeVariableName(%name @ "Seed");
	%itemName = %safeName @ "Item";
	%imageName = %safeName @ "0Image";
	%singleShape = expandFileName("./" @ %singleShape @ ".dts");
	eval(DataString_SeedItem(%displayName,%safeName,%itemName,%imageName,%singleShape,%singleColor));

	%cropBrick = "brick" @ %name @ "0CropData";
	if(%tree)
	{
		%cropBrick = "brick" @ %name @ "Tree0CropData";
	}
	
	%count = 4;
	$Stackable_[%safeName @ "_StackedItemTotal"] = %count;
	for(%i = 0; %i < %count; %i++)
	{
		%uiName = %displayName @ %i;
		%itemName = %safeName @ %i @ "Item";
		%imageName = %safeName @ %i @ "Image";
		%shape = expandFileName("./seed" @ (%i + 1) @ ".dts");

		$Stackable_[%safeName @ "_StackedItem" @ %i] = %itemName SPC (%stackMul * (%i + 1));
		eval(DataString_SeedItem(%uiname,%safeName,%itemName,%imageName,%shape,%packColor));
		eval(DataString_SeedImage(%uiname,%imageName,%itemName,%shape,%packColor,%cropBrick,%name));
	}
}

//tree
SeedDatablocks("Apple","0.09 0.04 0 1","seedsShort","1 0 0 1",true,1);
SeedDatablocks("Date","0.392 0.192 0 1","seeds","0.412 0.192 0 1",true,1);
SeedDatablocks("Mango","0.09 0.04 0 1","seedsShort","1 0.83 0 1",true,1);
SeedDatablocks("Peach","0.412 0.192 0 1","seedsLarge","1 0.603 0.419 1",true,1);

//underground
SeedDatablocks("Carrot","0.39 0.34 0.30 1","seeds","1 0.502 0 1",false,3);
SeedDatablocks("Onion","0.08 0.08 0.08 1","seedsFlat","1 0.96 0.74 1",false,6);
SeedDatablocks("Potato","0.9 0.68 0.12 1","seeds","0.568 0.329 0 1",false,3);
SeedDatablocks("Turnip","0.08 0.08 0.08 1","seedsRound","0.9 0.9 0.9 1",false,6);

//aboveground
SeedDatablocks("Blueberry","0.71 0.4 0 1","seedsround","0.24 0.12 0.487 1",false,3);
SeedDatablocks("Cabbage","0.2 0.1 0 1","seedsShort","0 0.4 0.2 1",false,3);
SeedDatablocks("Cactus","0.08 0.08 0.08 1","seedsFlat","0.329 0.419 0.184 1",false,2);
SeedDatablocks("Chili","0.95 0.85 0 1","seedsShort","0.9 0 0 1",false,4);
SeedDatablocks("Corn","0.95 0.85 0 1","seedsShort","0.95 0.85 0 1",false,3);
SeedDatablocks("Daisy","0.9 0.68 0.12 1","seeds","0.568 0.329 0 1",false,2);
SeedDatablocks("Lily","0.9 0.68 0.12 1","seeds","0.568 0.329 0 1",false,2);
SeedDatablocks("Portobello","0.08 0.08 0.08 1","seedsRound","0.08 0.08 0.08 1",false,4);
SeedDatablocks("Rose","0.474 0.219 0 1","seedsShort","0.568 0.06 0 1",false,2);
SeedDatablocks("Strawberry","0.71 0.8 0 1","seedsShort","1 0 0 1",false,3);
SeedDatablocks("Tomato","0.8 0.66 0.48 1","seedsFlat","1 0 0 1",false,3);
SeedDatablocks("Watermelon","0.08 0.08 0.08 1","seedsShort","0.094 0.2 0.149 1",false,2);
SeedDatablocks("Wheat","0.854 0.647 0.122 1","seedsShort","1 0.96 0.74 1",false,6);
SeedDatablocks("AncientFlower","1 0.69 0.90 1","seedsRound","1 0.69 0.90 1",false,1);
