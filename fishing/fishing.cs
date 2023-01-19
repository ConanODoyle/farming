//gameplay:
//	cast line into a fishing zone over a pool of water
//	bobber spawns, plays animations
//	bobber will indicate a hook soon with bob, then dunk to indicate its time to pull
//	quality of catch will be determined by timing of click after pull is animated
//	bob will locate nearest "fishing zone" brick to determine catch chances and fish depletion

// datablock ParticleEmitterNodeData (GenericEmitterNode)
// datablock ParticleEmitterNodeData (HalfEmitterNode)
// datablock ParticleEmitterNodeData (FifthEmitterNode)
// datablock ParticleEmitterNodeData (TenthEmitterNode)
// datablock ParticleEmitterNodeData (TwentiethEmitterNode)
// datablock ParticleEmitterNodeData (FourtiethEmitterNode)

$Fishing::ZoneEmitter = "PlayerFoamDropletsEmitter";
$Fishing::ZoneRestTime = 30;
$Fishing::MaxZoneFishCount = 5;

if (!isObject(FishingSimSet))
{
	$FishingSimSet = new SimSet(FishingSimSet) { };
	$RemoveFishingSimSet = new SimSet(RemoveFishingSimSet) { };
}

datablock fxDTSBrickData(brickFishingSpotData : brick8x8fData)
{
	category = "Farming";
	subCategory = "Fishing";
	uiName = "Fishing Spot";
	isIllegal = 1;
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
			fishingCheck(FishingSimSet.getObject(%curr));
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
	
	$masterFishingSchedule = schedule(100, 0, fishingTick, %idx);
}

function fishingCheck(%brick)
{
	if (%brick.getGroup().bl_id != 888888)
	{
		$RemoveFishingSimSet.add(%brick);
		return;
	}

	//restock fish based on number of fish left and time since last fished
	if ($Sim::Time - %brick.lastFished < $Fishing::ZoneRestTime)
	{
		return;
	}

	if (%brick.fish < $Fishing::MaxZoneFishCount)
	{
		%brick.fish++;
		%brick.lastFished = $Sim::Time;
	}

	//adjust visual effect of zone based on fishcount
	if (%brick.fish <= 0)
	{
		if (isObject(%brick.emitter))
		{
			%brick.setEmitter(0);
		}
		return;
	}

	if (!isObject(%brick.emitter))
	{
		%brick.setEmitter($Fishing::ZoneEmitter);
	}

	%ratio = %brick.fish / $Fishing::MaxZoneFishCount;

	if (%ratio > 0.75) { %brick.emitter.setDatablock(HalfEmitterNode); }
	else if (%ratio > 0.5) { %brick.emitter.setDatablock(FifthEmitterNode); }
	else if (%ratio > 0.25) { %brick.emitter.setDatablock(TenthEmitterNode); }
	else { %brick.emitter.setDatablock(TwentiethEmitterNode); }
}