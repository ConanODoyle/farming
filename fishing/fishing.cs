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
	
	$masterFishingSchedule = schedule(80, 0, fishingTick, %idx);
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
			if (isObject(%obj.bobber.bait))
			{
				%obj.bobber.bait.delete();
			}
			%obj.bobber.delete();
		}
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

function startFish(%player, %brick, %hitPos)
{
	if (isObject(%player.bobber) || !%brick.dataBlock.isFishingSpot)
	{
		return 0;
	}

	%bobberPos = setWord(%hitPos, 2, getWord(%brick.position, 2) - %brick.dataBlock.brickSizeZ * 0.1);

	%player.bobber = createBobber(%bobberPos, getWords(%player.getTransform(), 3, 6));
	$FishingSimSet.add(%player.bobber);
}