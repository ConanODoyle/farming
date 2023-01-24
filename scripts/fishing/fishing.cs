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

datablock ItemData(BobberItem : HammerItem)
{
	shapeFile = "./fishingpole/bobber.dts";
	uiName = "";
	colorShiftColor = "1 1 1 1";
	doColorShift = 0;
	image = "";
	isProp = 1;
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

function createBobber(%transform, %velocity)
{
	%item = new Item(Bobber)
	{
		dataBlock = BobberItem;
		isBobber = 1;
	};
	%item.setTransform(%transform);
	%item.playThread(0, idle);
	%item.setVelocity(%velocity);
	return %item;
}

function cleanupBobber(%bobber)
{
	if (isObject(%bobber.bait)) { %bobber.bait.delete(); }
	if (isObject(%bobber.line)) { %bobber.line.delete(); }
	%bobber.delete();
}

function bobberCheck(%bobber)
{
	//string to bobber
	if (!isObject(%bobber.line))
	{
		%bobber.line = new StaticShape(FishingLine){ dataBlock = FishingLineShape; };
	}

	%pos = %bobber.getSlotTransform(0);
	%color = setWord(getColorIDTable(%bobber.player.client.currentColor), 3, 1);
	%bobber.line.drawLine(%pos, %bobber.player.getMuzzlePoint(0), "1 1 1 1", 0.8);
	%bobber.setNodeColor("bobberTop", %color);

	%start = %bobber.player.getMuzzlePoint(0);
	%end = %pos;
	%dir = vectorNormalize(vectorSub(%end, %start));
	%right = vectorNormalize(vectorCross(%dir, "0 0 -1"));
	%up = vectorCross(%dir, %right);
	%rot = relativeVectorToRotation(%right, %up);
	%bobber.line.setTransform(%bobber.line.position SPC %rot);

	//velocity to force network updates on the bobber's position
	%bobber.locUpdate = ((%bobber.locUpdate + 1) % 10);
	if (%bobber.locUpdate == 0)
	{
		%bobber.setVelocity(vectorAdd(%bobber.getVelocity(), "0 0 0.001"));
	}

	//line of sight/distance checks
	%dist = vectorDist(%bobber.position, %bobber.player.position);
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

	if (%bobber.LOSBlockedCount > 80)
	{
		messageClient(%bobber.player.client, '', "Fishing line broken - make sure your fishing line isn't blocked by bricks!");
		cleanupBobber(%bobber);
		return;
	}
	if (%dist > %bobber.maxDistance)
	{
		messageClient(%bobber.player.client, '', "Fishing line broken - bobber too far away!");
		cleanupBobber(%bobber);
		return;
	}

	//fish hook checks
	bobberFishCheck(%bobber);
}

function bobberFishCheck(%bobber)
{
	if (!%bobber.fishPending)
	{
		//reset fish check timing
		if (%bobber.nextFishCheck > $Sim::Time)
		{
			return;
		}
		%bobber.nextFishCheck = $Sim::Time + 3 | 0;
		// messageClient(fcn(Conan), '', "\c3[" @ $Sim::Time @ "]\c6 Next fish check " @ %bobber.nextFishCheck);

		//ensure we're on water
		initContainerRadiusSearch(vectorAdd(%bobber.position, "0 0 -0.2"), 0.15, $Typemasks::PhysicalZoneObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next.isWater)
			{
				%found = 1;
				break;
			}
		}

		if (!%found)
		{
			return;
		}

		%bobber.fishChance = 0.8 + %bobber.nearestFishingSpot.fish * 0.5;
		// messageClient(fcn(Conan), '', "\c3[" @ $Sim::Time @ "]\c6 Fish check - found water, chance: " @ %bobber.fishChance);

		if (getRandom() < %bobber.fishChance)
		{
			%bobber.fishPending = 1;
			%bobber.nextDunkTime = $Sim::Time + getRandom(10) + 8;
			// messageClient(fcn(Conan), '', "\c3[" @ $Sim::Time @ "]\c6 Fish now pending, dunk time " @ %bobber.nextDunkTime);
		}
	}
	else
	{	
		//times up, dunk!
		if (%bobber.nextDunkTime < $Sim::Time)
		{
			%bobber.dunkTime = getSimTime();
			%bobber.setVelocity("0 0 -" @ (getRandom(80, 150)));
			%bobber.fishPending = 0;
			%bobber.nextFishCheck = $Sim::Time + 8 | 0;
			// messageClient(fcn(Conan), '', "\c3[" @ $Sim::Time @ "]\c6 Fish dunking!!!!");
			return;
		}

		//do a bob?
		if (%bobber.nextBobTime < $Sim::Time && getRandom() < 0.1)
		{
			%bobber.nextBobTime = $Sim::Time + 2;
			%bobber.setVelocity("0 0 -" @ getRandom(30, 50));
			// messageClient(fcn(Conan), '', "\c3[" @ $Sim::Time @ "]\c6 Fish bobbing");
		}
	}
}

function reelBobber(%bobber)
{
	%delta = getSimTime() - %bobber.dunkTime;
	if (%delta <= 4000)
	{
		messageClient(%bobber.player.client, '', "\c2You got a fish!");
		messageClient(%bobber.player.client, '', "\c3Reel Delay: \c6" @ %delta @ "ms");
	}
	else
	{
		messageClient(%bobber.player.client, '', "\c0You didn't catch anything...");
	}
	cleanupBobber(%bobber);
	return;
}

function startFish(%player, %lineDist, %lineVel)
{
	if (isObject(%player.bobber))
	{
		if (isObject(%player.bobber))
		{
			cleanupBobber(%player.bobber);
		}
		return 0;
	}

	%bobberPos = setWord(%hitPos, 2, getWord(%brick.position, 2) - %brick.dataBlock.brickSizeZ * 0.1);
	%bobberPos = %player.getMuzzlePoint(0) SPC getWords(%player.getTransform(), 3, 6);
	%velocity = vectorScale(%player.getMuzzleVector(0), %lineVel);
	%velocity = vectorAdd(%velocity, vectorScale(%player.getVelocity(), 0.5));

	%player.bobber = %player.boober = %bobber = createBobber(%bobberPos, %velocity);
	%bobber.player = %player;
	%bobber.fishingSpot = %brick;
	%bobber.setNodeColor("bobberTop", setWord(getColorIDTable(%player.client.currentColor), 3, 1));
	%bobber.sourcePlayer = %player;
	%bobber.maxDistance = %lineDist;
	$FishingSimSet.add(%player.bobber);
}