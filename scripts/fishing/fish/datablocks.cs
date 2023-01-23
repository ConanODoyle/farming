
function DataString_FishItem(%uiname,%safeName,%itemName,%imageName,%shape)
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
		@"doColorShift = false;"
		@"colorShiftColor = \"1 1 1 1\";"
		@"canDrop = 1;"
		@"isStackable = 1;"
		@"stackType = \"" @ %safeName @ "\";"
	@"};";
}

function DataString_FishImage(%uiname,%imageName,%itemName,%shape)
{
	return "datablock ShapeBaseImageData(" @ %imageName @ ")"
	@"{"
		@"className = \"FishImage\";"
		@"shapeFile = \"" @ %shape @ "\";"
		@"emap = true;"
		@"offset = \"-0.53 0.2 0\";"
		@"doColorShift = false;"
		@"colorShiftColor = \"1 1 1 1\";"
		@"item = \"" @ %itemName @ "\";"
		@"armReady = 1;"
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

function FishDatablocks(%name,%shapeName,%stackMax)
{
	%displayName = %name SPC "Fish";
	%safeName = getSafeVariableName(%name @ "Fish");
	%itemName = %safeName @ "Item";
	%imageName = %safeName @ "Image";
	%shape = expandFileName("./resources/" @ %shapeName @ "/" @ %shapeName @ ".dts");
	eval(DataString_FishItem(%displayName,%safeName,%itemName,%imageName,%shape));
	eval(DataString_FishImage(%uiname,%imageName,%itemName,%shape));

	%count = %stackMax;
	$Stackable_[%safeName @ "_StackedItemTotal"] = %count;
	for(%i = 0; %i < %count; %i++)
	{
		%uiName = %displayName @ %i;
		%itemName = %safeName @ %i @ "Item";
		%imageName = %safeName @ %i @ "Image";
		%shape = expandFileName("./resources/" @ %shapeName @ "/" @ %shapeName @ (%i + 1) @ ".dts");

		$Stackable_[%safeName @ "_StackedItem" @ %i] = %itemName SPC (%i + 1);
		eval(DataString_FishItem(%uiname,%safeName,%itemName,%imageName,%shape));
		eval(DataString_FishImage(%uiname,%imageName,%itemName,%shape));
	}
}

function FishImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyBoth);
}

FishDatablocks("Sardine","Sardine",4);
FishDatablocks("Bass","Bass",3);
FishDatablocks("Catfish","Catfish",3);
FishDatablocks("Minnow","Minnow",3);
FishDatablocks("Anchovy","Anchovy",4);
FishDatablocks("Arowana","Arowana",3);