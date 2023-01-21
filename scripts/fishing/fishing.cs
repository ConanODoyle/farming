//gameplay:
//	cast line into a fishing spot over a pool of water
//	bobber spawns, plays animations
//	bobber will indicate a hook soon with bob, then dunk to indicate its time to pull
//	quality of catch will be determined by timing of click after pull is animated
//	fishing spot determines catch chances and fish depletion

//rods have the following qualities:
//	quality of hook
//	avg time to bite
//	cast distance
//	reel timing forgiveness
//	durability

//bait?
//	quality, class
//	affects the quality, class of fish fished

// datablock ParticleEmitterNodeData (GenericEmitterNode)
// datablock ParticleEmitterNodeData (HalfEmitterNode)
// datablock ParticleEmitterNodeData (FifthEmitterNode)
// datablock ParticleEmitterNodeData (TenthEmitterNode)
// datablock ParticleEmitterNodeData (TwentiethEmitterNode)
// datablock ParticleEmitterNodeData (FourtiethEmitterNode)

$Fishing::SpotEmitter = "PlayerFoamDropletsEmitter";
$Fishing::SpotResetTime = "180 360";

if (!isObject(FishingSimSet))
{
	$FishingSimSet = new SimSet(FishingSimSet) { };
	$RemoveFishingSimSet = new SimSet(RemoveFishingSimSet) { };
}

datablock StaticShapeData(BobberShape)
{
	shapeFile = "./fishingpole/bobber.dts";
};

datablock StaticShapeData(FishingLineShape)
{
	shapeFile = "./fishingpole/line.dts";
};

datablock fxDTSBrickData(brickFishingSpotData : brick8x8fData)
{
	category = "Farming";
	subCategory = "Fishing";
	uiName = "Fishing Spot";
	isIllegal = 1;
	isFishingSpot = 1;
};

function brickFishingSpotData::onAdd(%this, %obj)
{
	$FishingSimSet.add(%obj);
}

function fishingTick(%idx)
{
	cancel($masterFishingSchedule);
	
	if (!isObject(MissionCleanup))
	{
		return;
	}
	
	for (%i = 0; %i < 16; %i++)
	{
		%curr = %idx + %i;
		if (%curr >= FishingSimSet.getCount())
		{
			break;
		}
		else
		{
			%obj = FishingSimSet.getObject(%curr);
			if (%obj.isBobber)
			{
				bobberCheck(%obj);
			}
			else
			{
				fishingSpotCheck(%obj);
			}
		}
	}
	
	for (%j = 0; %j < RemoveFishingSimSet.getCount(); %j++)
	{
		FishingSimSet.remove(RemoveFishingSimSet.getObject(%j));
	}
	RemoveFishingSimSet.clear();
	
	%idx = %idx + %i + 1;
	if (%idx >= FishingSimSet.getCount())
	{
		%idx = 0;
	}
	
	$masterFishingSchedule = schedule(33, 0, fishingTick, %idx);
}

function fishingSpotCheck(%brick)
{
	if (%brick.getGroup().bl_id != 888888)
	{
		$RemoveFishingSimSet.add(%brick);
		return;
	}

	//restock fish based on number of fish left and time since last fished
	if (%brick.nextRestockTime > $Sim::Time)
	{
		return;
	}

	%brick.fish = getRandom();
	%brick.nextRestockTime = $Sim::Time + getRandom(getWord($Fishing::SpotResetTime, 0), getWord($Fishing::SpotResetTime, 1));

	//adjust visual effect of Spot based on fishcount
	if (!isObject(%brick.emitter))
	{
		%brick.setEmitter($Fishing::SpotEmitter);
	}

	%ratio = %brick.fish;

	if (%ratio > 0.75) { %brick.emitter.setDatablock(GenericEmitterNode); }
	else if (%ratio > 0.5) { %brick.emitter.setDatablock(HalfEmitterNode); }
	else if (%ratio > 0.25) { %brick.emitter.setDatablock(FifthEmitterNode); }
	else { %brick.emitter.setDatablock(TenthEmitterNode); }
}






package FishingPackage
{
	function Armor::onRemove(%this, %obj)
	{
		if (isObject(%obj.bobber))
		{
			cleanupBobber(%obj.bobber);
		}
		parent::onRemove(%this, %obj);
	}
};
activatePackage(FishingPackage);

function createBobber(%pos, %rotation)
{
	%shape = new StaticShape(Bobber)
	{
		dataBlock = BobberShape;
		isBobber = 1;
	};
	%shape.setTransform(%pos SPC %rotation);
	%shape.playThread(0, idle);
	return %shape;
}

function cleanupBobber(%bobber)
{
	if (isObject(%bobber.bait)) { %bobber.bait.delete(); }
	if (isObject(%bobber.line)) { %bobber.line.delete(); }
	%bobber.delete();
}

function bobberCheck(%bobber)
{
	if (!isObject(%bobber.line))
	{
		%bobber.line = new StaticShape(FishingLine){ dataBlock = FishingLineShape; };
	}
	if (isObject(%bobber.getMountedObject(0)))
	{
		%pos = %bobber.getMountedObject(0).getTransform();
	}
	else
	{
		%pos = %bobber.getSlotTransform(2);
	}
	%bobber.line.drawLine(%pos, %bobber.player.getMuzzlePoint(0), "0 0 0 1", 0.8);
	%dist = vectorDist(%bobber.position, %bobber.player.position);
	// %bobber.line.setScale(setWord(%bobber.line.scale, 2, %dist / 200));

	%start = %bobber.position;
	%end = %bobber.player.getMuzzlePoint(0);
	%hit = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	if (isObject(%hit))
	{
		%bobber.LOSBlockedCount++;
	}
	else
	{
		%bobber.LOSBlockedCount = 0;
	}

	if (%bobber.LOSBlockedCount > 100)
	{
		messageClient(%bobber.player.client, '', "Fishing line broken - make sure your fishing line isn't blocked by bricks!");
		cleanupBobber(%bobber);
	}
	if (%dist > %bobber.maxDistance)
	{
		messageClient(%bobber.player.client, '', "Fishing line broken - you walked too far away!");
		cleanupBobber(%bobber);
	}
}

function reelBobber(%bobber)
{
	cleanupBobber(%bobber);
	return;
}

function startFish(%player, %brick, %hitPos, %lineDist)
{
	if (isObject(%player.bobber) || !%brick.dataBlock.isFishingSpot)
	{
		if (isObject(%player.bobber))
		{
			cleanupBobber(%player.bobber);
		}
		return 0;
	}

	%bobberPos = setWord(%hitPos, 2, getWord(%brick.position, 2) - %brick.dataBlock.brickSizeZ * 0.1);

	%player.bobber = %player.boober = %bobber = createBobber(%bobberPos, getWords(%player.getTransform(), 3, 6));
	%bobber.player = %player;
	%bobber.fishingSpot = %brick;
	%bobber.setNodeColor("bobberTop", setWord(getColorIDTable(%player.client.currentColor), 3, 1));
	%bobber.sourcePlayer = %player;
	%bobber.maxDistance = %lineDist + 20;
	$FishingSimSet.add(%player.bobber);
}