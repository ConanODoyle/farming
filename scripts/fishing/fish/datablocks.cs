
function DataString_FishItem(%uiname,%safeName,%itemName,%imageName,%shape,%notStackable)
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
		@"isStackable = " @ (!%notStackable) @ ";"
		@"stackType = \"" @ %safeName @ "\";"
		@"iconName = \"Add-ons/Server_Farming/icons/" @ (getSubStr(%itemName, 0, strLen(%itemName) - 4)) @ "\";"
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
		@"stateName[2] = \"Fire\";"
		@"stateScript[2] = \"onFire\";"
		@"stateTransitionOnTriggerUp[2] = \"Loop\";"
		@"stateTimeoutValue[2] = 0.1;"
	@"};";
}

function FishDatablocks(%name,%shapeName,%stackMax)
{
	%displayName = %name;
	%safeName = getSafeVariableName(%name);
	%itemName = %safeName @ "Item";
	%imageName = %safeName @ "Image";
	%shape = expandFileName("./resources/" @ %shapeName @ "/" @ %shapeName @ ".dts");
	eval(DataString_FishItem(%displayName,%safeName,%itemName,%imageName,%shape));
	eval(DataString_FishImage(%uiname,%imageName,%itemName,%shape));
	%imageName.rotation = eulerToMatrix("0 0 90");

	//storageMax and storage lists
	//list uses stacktype
	$StorageMax_[%safeName] = %stackMax * 3;
	$StorageTypeFishList = trim($StorageTypeFishList) TAB %safeName;

	$SellFishList = trim($SellFishList TAB strReplace(%name, " ", "_"));
	if (%stackMax <= 1) //cant be stacked
	{
		$Stackable_[%safeName @ "_StackedItemTotal"] = 1;
		$Stackable_[%safeName, "StackedItem0"] = %itemName SPC 1;
		return;
	}

	%count = %stackMax;
	$Stackable_[%safeName @ "_StackedItemTotal"] = %count;
	for(%i = 0; %i < %count; %i++)
	{
		%uiName = %displayName @ " (" @ (%i + 1) @ ")";
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
	if (%this.item.stackType $= "Bucket" && getRandom() < 0.1)
	{
		if (%obj.toolStackCount[%obj.currTool] > 1)
		{
			serverPlay3D(DearGodSound, %obj.position);
		}
		else
		{
			serverPlay3D(BucketSound, %obj.position);
		}
	}
	if (getRandom() < 0.001)
	{
		serverPlay3D(BucketSound, %obj.position);
	}
	%obj.playThread(1, armReadyBoth);
}

function FishImage::onUnmount(%this, %obj, %slot)
{
	%obj.playThread(1, armReadyRight);
}

FishDatablocks("Old Boot", "Old Boot", 3);
FishDatablocks("Glass Bottle", "Glass Bottle", 7);
FishDatablocks("Bucket", "Bucket", 3);

FishDatablocks("Sardine", "Sardine", 5);
FishDatablocks("Arowana", "Arowana", 3);
FishDatablocks("Minnow", "Minnow", 3);
FishDatablocks("Catfish", "Catfish", 3);
FishDatablocks("Anchovy", "Anchovy", 4);
FishDatablocks("Bass Fish", "Bass", 3);

FishDatablocks("Tuna", "Tuna", 1);
TunaImage.rotation = eulerToMatrix("0 -40 0");
FishDatablocks("Swordfish", "Swordfish", 1);
SwordfishImage.rotation = eulerToMatrix("0 -40 0");
