$LilyPlanter = "-504 -838.5 112.5";
$RosePlanter = "-1030.5 -1021 17.9";
$DaisyPlanter = "-584.5 -442.5 47.7";

$VoidFlowerPlanter = "Planter Box";
$Wanderer = "_sequenceBot";
$BlackColor = 53;

// by Pah1023
$hex = "0123456789ABCDEF";
function byteToHex(%n)
{
    return getSubStr($hex, mFloor(%n / 16), 1) @ getSubStr($hex, %n & 15, 1);
}

function getRandomValidAsciiChar()
{
	// This block excludes value 127, the DEL character
	%rand = 0;
	if(getRandom() > 0.425339367) // Ratio of the first block being chosen over the 2nd block.
	{
		%rand = getRandom(128, 255);
	}
	else
	{
		%rand = getRandom(33, 126);
	}
	
	return collapseEscape("\\x" @ byteToHex(%rand));
}

function getVoidNoisedWord(%word, %noise)
{
	%result = "";
	
	for(%i = 0; %i < strLen(%word); %i++)
	{
		if(getRandom() > 1 - mAbs(%noise))
		{
			%char = getRandomValidAsciiChar();
		}
		else
		{
			%char = getSubStr(%word, %i, 1);
		}
		
		%result = %result @ %char;
	}
	
	return trim(%result);
}

package Void
{
	function plantCrop(%image, %obj, %imageSlot, %remotePlacement)
	{
		%cropType = %image.cropType;
		if (%cropType !$= "Lily" && %cropType !$= "Rose" && $cropType !$= "Daisy")
		{
			return parent::plantCrop(%image, %obj, %imageSlot, %remotePlacement);
		}

		if (%remotePlacement $= "")
		{
			%start = %obj.getEyePoint();
			%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
		}
		else
		{
			%start = vectorAdd(%remotePlacement, "0 0 0.1");
			%end = vectorAdd(%remotePlacement, "0 0 -0.1");
		}

		%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
		%hit = getWord(%ray, 0);
		//check that we did hit a brick and confirm its normal
		if (!isObject(%hit) || vectorDist(getWords(%ray, 4, 6), "0 0 1") > 0.01)
		{
			return 0;
		}

		if (%hit.dataBlock.uiName $= $VoidFlowerPlanter && %hit.getGroup().bl_id == 888888)
		{
			//reuse %ray since might as well
			return voidPlantCrop(%image, %obj, %imageSlot, %remotePlacement, %ray);
		}
		return parent::plantCrop(%image, %obj, %imageSlot, %remotePlacement);
	}

	function fxDTSBrick::grow(%brick, %growDB)
	{
		if (%growDB.cropType $= "Void")
		{
			if (%growDB.stage == 4)
			{
				%brick.setItem("VoidEncasedKeySeedlingItem");
				%brick.item.canPickup = 0;
			}
			if (%growDB.stage == 5)
			{
				%brick.setItem("VoidEncasedKeyItem");
				%brick.item.canPickup = 0;
			}
		}
		return parent::grow(%brick, %growDB);
	}
	
	function displayPlantStatus(%brick, %cl)
	{
		%db = %brick.dataBlock;
		%cropType = %db.cropType;
		
		if(%cropType !$= "Void")
		{
			return Parent::displayPlantStatus(%brick, %cl);
		}
		
		%string = "I MUST CONSUME ALL";
		
		// Schedules make the text update faster than the item loop for more glitchy effectiveness.
		%cl.schedule(0, centerprint, "<just:right><color:FF0000>" @ getVoidNoisedWord(%string, mSin($Sim::Time / 2)), 1);
		%cl.schedule(25, centerprint, "<just:right><color:FF0000>" @ getVoidNoisedWord(%string, mSin($Sim::Time / 2)), 1);
		%cl.schedule(50, centerprint, "<just:right><color:FF0000>" @ getVoidNoisedWord(%string, mSin($Sim::Time / 2)), 1);
		%cl.schedule(75, centerprint, "<just:right><color:FF0000>" @ getVoidNoisedWord(%string, mSin($Sim::Time / 2)), 1);
	}
};
activatePackage(Void);

function voidPlantCrop(%image, %obj, %imageSlot, %remotePlacement, %ray)
{
	%cropType = %image.cropType;
	%brickDB = %image.cropBrick;
	%zOffset = %brickDB.brickSizeZ * 0.1;
	%isTree = %brickDB.isTree;
	%type = %image.item.stackType;
	%slot = %obj.currTool;
	%toolStackCount = %obj.toolStackCount[%obj.currTool];

	%expRequirement = getPlantData(%cropType, "experienceRequired");
	%expCost = getPlantData(%cropType, "experienceCost");
	%plantingLayer = getPlantData(%cropType, "plantingLayer");
	%plantingBoxDisabled = getPlantData(%cropType, "plantingBoxDisabled");
	%radius = getPlantData(%brickDB.cropType, "plantSpace");

	if (%obj.client.farmingExperience < %expCost)
	{
		pushSeedError(%obj, "You don't have enough experience to plant this crop!", 3);
		return 0;
	}

	if (%toolStackCount <= 0)
	{
		return 0;
	}

	%hit = getWord(%ray, 0);
	//check that we did hit a brick and confirm its normal
	if (!isObject(%hit) || vectorDist(getWords(%ray, 4, 6), "0 0 1") > 0.01)
	{
		return 0;
	}
	%hitLoc = getWords(%ray, 1, 3);

	//run raycasts to check the actual planting location, rather than just rely on the eye vector hit location
	%error = checkPlantLocationError(%hitLoc, %brickDB, %obj);

	switch (%error)
	{
		case 1: %errMsg = "Unable to plant here.";
		case 2: %errMsg = "This is not dirt.";
		case 3: %errMsg = getField(%error, 1);
		case 4: //Trust error. We only get here if its a public brick, so its time to cook :)
			//...can just Not Do Anything, but can also play a sound :0

		default: %errMsg = "Unable to plant here.";
	}

	if (%errMsg !$= "")
	{
		pushSeedError(%obj, %errMsg, 1);
		failPlantSeed(getBrickPlantPosition(%hitLoc, %brickDB), %brickDB);
		return 0;
	}
	%planterFound = getWord(%error, 2);

	%pos = getBrickPlantPosition(%hitLoc, %brickDB);

	//plant successful, make plant brick
	%b = createPlantBrick(%pos, %brickDB, 1, "", %brickDB.defaultColor + 0);
	%b.plantedTime = $Sim::Time;
	%b.inGreenhouse = %inGreenhouse;

	%error = %b.plant();
	if (%error > 0 || %error $= "")
	{
		%b.delete();

		return failPlantSeed(%pos, %brickDB);
	}

	%b.setTrusted(1);
	%b.setColliding(0);
	%bg = "BrickGroup_888888";
	%bg.add(%b);

	//update inventory item
	updateSeedCount(%image, %obj, %imageslot);

	//call voidCheckPlants
	cancel($VoidCheckPlantSchedule);
	$VoidCheckPlantSchedule = schedule(2000, 0, voidCheckPlants);

	return %b;
}

function findVoidPlanters()
{
	initContainerRadiusSearch($LilyPlanter, 0.1, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (%next.dataBlock.uiName $= $VoidFlowerPlanter)
		{
			$LilyPlanter = %next;
			break;
		}
	}

	initContainerRadiusSearch($RosePlanter, 0.1, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (%next.dataBlock.uiName $= $VoidFlowerPlanter)
		{
			$RosePlanter = %next;
			break;
		}
	}

	initContainerRadiusSearch($DaisyPlanter, 0.1, $Typemasks::fxBrickObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (%next.dataBlock.uiName $= $VoidFlowerPlanter)
		{
			$DaisyPlanter = %next;
			break;
		}
	}
}

function voidCheckPlants()
{
	cancel($VoidCheckPlantSchedule);
	if (!isObject($LilyPlanter) || !isObject($RosePlanter) || !isObject($DaisyPlanter))
	{
		findVoidPlanters();
		if (!isObject($LilyPlanter) || !isObject($RosePlanter) || !isObject($DaisyPlanter))
		{
			talk("An error occured: please notify an admin");
			return;
		}
	}

	%lilyUp = $LilyPlanter.getUpBrick(0);
	if (%lilyUp.dataBlock.uiName $= "Lily2")
	{
		%lilyFound = 1;
	}

	%roseUp = $RosePlanter.getUpBrick(0);
	if (%roseUp.dataBlock.uiName $= "Rose2")
	{
		%roseFound = 1;
	}

	%daisyUp = $DaisyPlanter.getUpBrick(0);
	if (%daisyUp.dataBlock.uiName $= "Daisy2")
	{
		%daisyFound = 1;
	}

	if (isObject(%lilyUp) && isObject(%roseUp) && isObject(%daisyUp))
	{
		%lilyDB = %lilyUp.dataBlock;
		%roseDB = %roseUp.dataBlock;
		%daisyDB = %daisyUp.dataBlock;
		//delete plants if any are non flowers, or at least one is wilted
		if (%lilyDB.cropType !$= "Lily" || %roseDB.cropType !$= "Rose" || %daisyDB.cropType !$= "Daisy"
			|| %lilyDB.stage >= 3 || %roseDB.stage >= 3 || %daisyDB.stage >= 3)
		{
			voidDeletePlants(%lilyUp, %roseUp, %daisyUp);
		}
		//confirm plants if all flowers are at the correct stage
		else if (%lilyFound && %roseFound && %daisyFound)
		{
			voidConfirmPlants(%lilyUp, %roseUp, %daisyUp);
			return; //loop is restarted in voidConfirmPlants
		}
		//correct plants, but not grown up - do nothing
	}
	$Wanderer.setHSpawnClose(1, 0);

	$VoidCheckPlantSchedule = schedule(32000, 0, voidCheckPlants);
}





//needs to restart voidCheckPlants
function voidConfirmPlants(%b0, %b1, %b2)
{
	//blacken plants, spawn bot, then later delete plants and despawn bot
	$Wanderer.setHSpawnClose(1, 2000);
	for (%i = 0; %i < 3; %i++)
	{
		%b[%i].setColor($BlackColor);

		%p = new Projectile()
		{
			dataBlock = "FarmingPlantGrowthProjectile";
			initialVelocity = "0 0 1";
			initialPosition = %b[%i].position;
		};

		if (isObject(%p))
		{
			%p.explode();
		}
	}

	schedule(20 * 60000, 0, voidEndConfirmPlants, %b0, %b1, %b2);
}

function voidEndConfirmPlants(%b0, %b1, %b2)
{
	$Wanderer.setHSpawnClose(1, 0);
	schedule(1000, 0, voidCheckPlants);
}





//doesnt need to restart voidcheckplants
function voidDeletePlants(%b0, %b1, %b2)
{
	for (%i = 0; %i < 3; %i++)
	{
		%b[%i].setShapeFX(1);
		%b[%i].spawnExplosion("PlayerSootProjectile", 0.4);
		%b[%i].schedule(2000, killPlant);
	}
}




datablock ItemData(VoidEncasedKeySeedlingItem)
{
	shapeFile = "./crops/encasedKeySeedling.dts";
	emap = false;

	colorShiftColor = "0 0 0 1";
	doColorShift = false;

	mass = 1.0;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	image = "";
	
	canDrop = true;
	isProp = 1;
	
	uiName = "Void Seedling";
};

datablock ItemData(VoidEncasedKeyItem)
{
	shapeFile = "./crops/encasedKey.dts";
	emap = false;

	iconName = "Add-ons/Server_Farming/icons/voidcapsule";

	colorShiftColor = "0 0 0 1";
	doColorShift = false;

	mass = 1.0;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	image = VoidEncasedKeyImage;
	
	canDrop = true;
	
	uiName = "Void Casing";
};

datablock ShapeBaseImageData(VoidEncasedKeyImage)
{
	shapeFile = "./crops/encasedKey.dts";

	offset = "0 0.4 0";

	emap = false;
	
	doColorShift = VoidEncasedKeyItem.doColorShift;
	colorShiftColor = VoidEncasedKeyItem.colorShiftColor;

	mountPoint = $RightHandSlot;
	item = VoidEncasedKeyItem;
	
	armReady = true;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "Loop";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "Loop";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "Loop";
	stateWaitForTimeout[1] = false;

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "Loop";
	stateTimeoutValue[2] = 0.1;
};

datablock ItemData(VoidKeyItem)
{
	shapeFile = "./crops/key.dts";
	emap = false;

	iconName = "Add-ons/Server_Farming/icons/voidkey";

	colorShiftColor = "0 0 0 1";
	doColorShift = false;

	mass = 1.0;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	image = VoidKeyImage;
	
	canDrop = true;
	
	uiName = "Void Key";
};

datablock ShapeBaseImageData(VoidKeyImage)
{
	shapeFile = "./crops/key.dts";

	offset = "0 0.4 0";

	emap = false;
	
	doColorShift = VoidKeyItem.doColorShift;
	colorShiftColor = VoidKeyItem.colorShiftColor;

	mountPoint = $RightHandSlot;
	item = VoidKeyItem;
	
	armReady = true;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "Loop";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "Loop";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "Loop";
	stateWaitForTimeout[1] = false;

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "Loop";
	stateTimeoutValue[2] = 0.1;
};

function VoidEncasedKeySeedlingItem::onAdd(%this, %item)
{
	parent::onAdd(%this, %item);
	%item.rotate = 1;
}

function VoidEncasedKeyItem::onAdd(%this, %item)
{
	parent::onAdd(%this, %item);
	%item.rotate = 1;
}

function VoidKeyItem::onAdd(%this, %item)
{
	parent::onAdd(%this, %item);
	%item.rotate = 1;
}