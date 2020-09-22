
//rain

if (!isObject(RainFillSimSet))
{
	$RainFillSimSet = new SimSet(RainFillSimSet){ };
}

$rainChance = 0.002;
$noRainModifier = 0.0001;
$heatWaveChance = 0.0045;
$heatWaveRainReduction = 0.0005;

function rainCheckLoop()
{
	cancel($masterRainCheckLoop);

	if (!isObject(MissionCleanup))
	{
		return;
	}
	else if ($disableWeather)
	{
		$masterRainCheckLoop = schedule(15000, 0, rainCheckLoop);
		return;
	}

	if ($lastRained $= "" || $lastRained > $Sim::Time)
	{
		$lastRained = $Sim::Time;
		$isRaining = 0;
	}

	%rainChance = $rainChance + $noRainModifier * mFloor(($Sim::Time - $lastRained + 0.1) / 15);
	%heatWaveChance = $heatWaveChance - (6 - mFloor(($Sim::Time - $lastRained) / 15) / 20) * $heatWaveRainReduction;
	if (!$isRaining && !$isHeatWave)
	{
		if (getRandom() < %rainChance)
		{
			// talk("Rainchance: " @ %rainChance);
			// talk("Last Rained: " @ $Sim::Time - $lastRained);
			echo("[" @ getDateTime() @ "] It has started raining");
			messageAll('MsgUploadStart',"\c6[\c3It has started to rain...\c6]");
			startRain();
		}
		else if (getRandom() < %heatWaveChance)
		{
			echo("[" @ getDateTime() @ "] A heat wave has started");
			messageAll('MsgUploadStart',"\c6[\c3A heat wave has started...\c6]");
			startHeatWave();
		}
	}
	else if ($isRaining ||$isHeatWave)
	{
		if ($isRaining)
		{
			$RainTicksLeft--;
			if ($RainTicksLeft <= 0)
			{
				echo("[" @ getDateTime() @ "] It has stopped raining");
				messageAll('MsgUploadStart',"\c6[\c3The rain stopped...\c6]");
				stopRain();
			}
		}
		else if ($isHeatWave)
		{
			$HeatWaveTicksLeft--;
			if ($HeatWaveTicksLeft <= 0)
			{
				echo("[" @ getDateTime() @ "] The heat wave has stopped");
				messageAll('MsgUploadStart',"\c6[\c3The heat wave ended...\c6]");
				stopHeatWave();
			}
		}
	}

	$masterRainCheckLoop = schedule(20000, 0, rainCheckLoop);
}

function startRain()
{
	Sky.materialList = "Add-ons/Sky_Skylands_Clouds/Skylands_Clouds.dml";
	$Sky::cloudHeight0 = 100;
	$Sky::cloudHeight1 = 0.5;
	$Sky::cloudHeight2 = 100;
	// Sky.cloudHeight[0] = $Sky::cloudHeight0;
	Sky.cloudHeight[1] = $Sky::cloudHeight1;
	// Sky.cloudHeight[2] = $sky::cloudHeight2;
	// Sky.sendUpdate();

	%rainTicks = 0;
	%rand = getRandom();
	%rand *= %rand;

	%rainTicks = mFloor(%rand * 40) + 8;

	// talk("Rainticks: " @ %rainTicks);
	// talk("currChance: " @ %currChance + $rainKeepModifier / %rainTicks);
	// talk("rand: " @ %rand);
	$RainTicksLeft = %rainTicks;

	$isRaining = 1;
	
	//create precipitation & adjust lighting
	$origAmbient = Sun.ambient;
	$origColor = Sun.color;
	$origShadow = Sun.shadowColor;

	gradualEnvColorshift(0.6, 0.6, 0.5, 10000);
	fadeInRain();
	startRainSound();
	schedule(5000, 0, rainLoop, 0);
}

function stopRain()
{
	$isRaining = 0;
	$RainTicksLeft = 0;
	$lastRained = $Sim::Time;

	cancel($masterRainLoop);
	//delete precipitation

	gradualEnvColorshift(1.666666, 1.666666, 2, 10000);
	clearRain();
	stopRainSound();
	cancel($masterRainLoop);
}

function startHeatWave()
{
	%targetVigColor = "0.439 0.341 0.215";
	gradualVignetteColorshift(getWord(%targetVigColor, 0), getWord(%targetVigColor, 1), getWord(%targetVigColor, 2), 10000);
	$isHeatWave = 1;

	%HeatWaveTicks = 0;
	%rand = getRandom();
	%rand *= %rand;

	%HeatWaveTicks = mFloor(%rand * 40) + 12;

	// talk("HeatWaveTicks: " @ %HeatWaveTicks);
	// talk("currChance: " @ %currChance + $rainKeepModifier / %HeatWaveTicks);
	// talk("rand: " @ %rand);
	$HeatWaveTicksLeft = %HeatWaveTicks;
	schedule(5000, 0, heatLoop, 0);
}

function stopHeatWave()
{
	%targetVigColor = "0 0 0";
	gradualVignetteColorshift(getWord(%targetVigColor, 0), getWord(%targetVigColor, 1), getWord(%targetVigColor, 2), 10000);

	cancel($masterHeatLoop);

	$isHeatWave = 0;
	$HeatWaveTicksLeft = 0;
	$lastHeatWave = $Sim::Time;
}

function roundToDecPoint(%num, %numDec)
{
	%factor = mPow(10, %numDec);
	return mFloor(%num * %factor + 0.5) / %factor;
}

function gradualVignetteColorshift(%x, %y, %z, %time)
{
	%ticks = mFloor(%time / 200);
	%vig = $EnvGuiServer::VignetteColor; %vigA = getWord(%vig, 3);
	%x = (%x - getWord(%vig, 0)) / %ticks;
	%y = (%y - getWord(%vig, 1)) / %ticks;
	%z = (%z - getWord(%vig, 2)) / %ticks;

	for (%i = 0; %i < %ticks; %i++)
	{
		%time = (%i + 1) * 200;
		%newVig = vectorAdd(%vig, 
			%x * (1.0 + %i) SPC %y * (1.0 + %i) SPC %z * (1.0 + %i)) SPC %vigA;
		schedule(%time, 0, setWeatherVignetteColor, %newVig, 1);
	}
}

function gradualEnvColorshift(%xF, %yF, %zF, %time)
{
	%xF = %xF - 1; %yF = %yF - 1; %zF = %zF - 1;

	%ticks = mFloor(%time / 200);
	%xF = %xF / %ticks;
	%yF = %yF / %ticks;
	%zF = %zF / %ticks;

	%amb = Sun.ambient; %ambA = getWord(%amb, 3);
	%col = Sun.color; %colA = getWord(%col, 3);
	%sha = Sun.shadowColor; %shaA = getWord(%sha, 3);
	%sky = Sky.skyColor; %skyA = getWord(%sky, 3);
	%fog = Sky.fogColor; %fogA = getWord(%fog, 3);

	for (%i = 0; %i < %ticks; %i++)
	{
		%time = (%i + 1) * 200;
		%newAmb = vectorAdd(%amb, 
			getWord(%amb, 0) * %xF * (1.0 + %i) SPC getWord(%amb, 1) * %yF * (1.0 + %i) SPC getWord(%amb, 2) * %zF * (1.0 + %i)) SPC %ambA;
		%newCol = vectorAdd(%col, 
			getWord(%col, 0) * %xF * (1.0 + %i) SPC getWord(%col, 1) * %yF * (1.0 + %i) SPC getWord(%col, 2) * %zF * (1.0 + %i)) SPC %colA;
		%newSha = vectorAdd(%sha, 
			getWord(%sha, 0) * %xF * (1.0 + %i) SPC getWord(%sha, 1) * %yF * (1.0 + %i) SPC getWord(%sha, 2) * %zF * (1.0 + %i)) SPC %shaA;
		%newFog = vectorAdd(%fog, 
			getWord(%fog, 0) * %xF * (1.0 + %i) SPC getWord(%fog, 1) * %yF * (1.0 + %i) SPC getWord(%fog, 2) * %zF * (1.0 + %i)) SPC %fogA;
		%newSky = vectorAdd(%sky, 
			roundToDecPoint(getWord(%sky, 0) * %xF * (1.0 + %i), 5) SPC 
			roundToDecPoint(getWord(%sky, 1) * %yF * (1.0 + %i), 5) SPC 
			roundToDecPoint(getWord(%sky, 2) * %zF * (1.0 + %i), 5)) SPC %skyA;

		schedule(%time, Sun, setEnvColors, %newAmb, %newCol, %newSha, %newFog, %newSky);
	}
	// talk("factor: " @ %xf * %ticks + 1 @ " sky: " @ %newSky);
}

function setEnvColors(%amb, %col, %sha, %fog, %sky)
{
	Sun.ambient = %amb;
	Sun.color = %col;
	Sun.shadowColor = %sha;
	Sky.fogColor = %fog;
	Sky.skyColor = %sky;

	Sun.sendUpdate();
	Sky.sendUpdate();
}

function setWeatherVignetteColor(%color, %multiply)
{
	$EnvGuiServer::VignetteColor = %color;
	$EnvGuiServer::VignetteMultiply = %multiply;
	EnvGuiServer::SendVignetteAll();
}

function createRain(%drops)
{	
	//values taken from default slate storm
	$Rain::dropTexture = "Add-Ons/Sky_Slate_Storm/rain.png";
	$Rain::splashTexture = "Add-Ons/Sky_Slate_Storm/water_splash.png";
	$Rain::dropSize = 0.75;
	$Rain::splashSize = 0.2;
	$Rain::splashMS = 250;
	$Rain::useTrueBillboards = 0;
	$Rain::minSpeed = 1.5;
	$Rain::maxSpeed = 2;
	$Rain::minMass = 0.75;
	$Rain::maxMass = 0.85;
	$Rain::maxTurbulence = 0.1;
	$Rain::turbulenceSpeed = 0.2;
	$Rain::rotateWithCamVel = 1;
	$Rain::useTurbulence = 0;
	$Rain::numDrops = 2500;//1800;
	$Rain::boxWidth = 100;//80;
	$Rain::boxHeight = 60;//50;
	$Rain::doCollision = 1;

	if (isObject(Rain))
	{
		Rain.schedule(100, delete);
	}
	if (isFile($Rain::DropTexture) && %drops > 0)
	{
		new Precipitation(Rain){
			dataBlock = DataBlockGroup.getObject(0);
			dropTexture = $Rain::DropTexture;
			splashTexture = $Rain::SplashTexture;
			dropSize = $Rain::DropSize;
			splashSize = $Rain::SplashSize;
			splashMS = $Rain::SplashMS;
			useTrueBillboards = $Rain::UseTrueBillboards;
			minSpeed = $Rain::minSpeed;
			maxSpeed = $Rain::maxSpeed;
			minMass = $Rain::minMass;
			maxMass = $Rain::maxMass;
			maxTurbulence = $Rain::maxTurbulence;
			turbulenceSpeed = $Rain::turbulenceSpeed;
			rotateWithCamVel = $Rain::rotateWithCamVel;
			useTurbulence = $Rain::useTurbulence;
			numDrops = %drops;//$Rain::numDrops;
			boxWidth = $Rain::boxWidth;
			boxHeight = $Rain::boxHeight;
			doCollision = $Rain::doCollision;
		};
		MissionGroup.add(Rain);
	}

}

function fadeInRain()
{
	$Rain::numDrops = 2500;
	for (%i = 0; %i < 10; %i++)
	{
		%drops = $Rain::numDrops / 10 * (%i + 1);
		schedule((%i + 1) * 800, 0, createRain, %drops);
	}

	for (%i = 0; %i < 100; %i++)
	{
		schedule((%i + 1) * 100, 0, eval,
			"Sky.cloudSpeed[1] = 0.003 / 10 * " @ ((%i + 1) / 10) @ ";" @
			// "Sky.cloudHeight[1] = 0.1 + 0.7 / 10 * " @ (10 - (%i + 1) / 30) @ ";" @
			"Sky.sendUpdate();");
	}
}

function clearRain()
{
	$Rain::numDrops = 2500;
	for (%i = 0; %i < 10; %i++)
	{
		%drops = Rain.numDrops / 10 * (10 - %i);
		schedule((%i + 1) * 800, 0, createRain, %drops);
	}
	schedule((%i + 1) * 1200, 0, createRain, 0);


	for (%i = 0; %i < 100; %i++)
	{
		schedule((%i + 1) * 100, 0, eval,
			"Sky.cloudSpeed[1] = 0.003 / 10 * " @ (10 - (%i + 1) / 10) @ ";" @
			// "Sky.cloudHeight[1] = 0.1 + 0.7 / 10 * " @ ((%i + 1) / 30) @ ";" @
			"Sky.sendUpdate();");
	}
}


function rainLoop(%index)
{
	cancel($masterRainLoop);

	if (%index < 0)
	{
		%index = RainFillSimSet.getCount() - 1;
	}
	else if (%index > RainFillSimSet.getCount() - 1)
	{
		%index = RainFillSimSet.getCount() - 1;
	}
	// echo("CurrIndex: " @ %index);

	%max = 16;
	for (%i = 0; %i < %max; %i++)
	{
		if (%i > %index) //we reached end of rain simset
		{
			break;
		}
		%brick = RainFillSimSet.getObject(%index - %i);

		if (%brick.nextRain $= "")
		{
			%brick.nextRain = $Sim::Time;
		}

		%db = %brick.getDatablock();
		%pos = %brick.getPosition();
		if (%brick.nextRain < $Sim::Time && !%brick.inGreenhouse && %brick.waterLevel < %db.maxWater)
		{
			%numTimes = mFloor(($Sim::Time - %brick.nextRain) / 2) + 1;

			%ray = containerRaycast(%pos, vectorAdd(%pos, "0 0 100"), $TypeMasks::fxBrickAlwaysObjectType, %brick);
			%hit = getWord(%ray, 0);
			while (%hit.getDatablock().isPlant)
			{
				%pos = getWords(%ray, 1, 3);
				%ray = containerRaycast(%pos, vectorAdd(%pos, "0 0 100"), $TypeMasks::fxBrickAlwaysObjectType, %hit);
				%hit = getWord(%ray, 0);
			}

			if (!isObject(%hit) || %hit.getGroup().bl_id == 888888)
			{
				if (%db.isDirt)
				{
					%brick.setWaterLevel(%brick.waterLevel + 40 * %numTimes);
				}
				else if (%db.isWaterTank)
				{
					%brick.setWaterLevel(%brick.waterLevel + 30 * %numTimes); //%db.maxWater * %numTimes / 500);
				}
			}
			%brick.nextRain = $Sim::Time + 2;
		}
		
		%totalBricksProcessed++;
	}

	$masterRainLoop = schedule(33, 0, rainLoop, %index - %totalBricksProcessed);
}

function heatLoop(%index)
{
	cancel($masterHeatLoop);

	if (%index < 0)
	{
		%index = RainFillSimSet.getCount() - 1;
	}
	else if (%index > RainFillSimSet.getCount() - 1)
	{
		%index = RainFillSimSet.getCount() - 1;
	}
	// echo("CurrIndex: " @ %index);

	%max = 16;
	for (%i = 0; %i < %max; %i++)
	{
		if (%i > %index) //we reached end of plant simset
		{
			break;
		}
		%brick = RainFillSimSet.getObject(%index - %i);

		if (%brick.nextHeat $= "")
		{
			%brick.nextHeat = $Sim::Time;
		}

		%db = %brick.getDatablock();
		%pos = %brick.getPosition();
		if (%brick.nextHeat < $Sim::Time && !%brick.inGreenhouse && %brick.waterLevel > 0)
		{
			%numTimes = mFloor(($Sim::Time - %brick.nextRain) / 2) + 1;

			%ray = containerRaycast(%pos, vectorAdd(%pos, "0 0 100"), $TypeMasks::fxBrickAlwaysObjectType, %brick);
			%hit = getWord(%ray, 0);
			while (%hit.getDatablock().isPlant)
			{
				%pos = getWords(%ray, 1, 3);
				%ray = containerRaycast(%pos, vectorAdd(%pos, "0 0 100"), $TypeMasks::fxBrickAlwaysObjectType, %hit);
				%hit = getWord(%ray, 0);
			}

			if (!isObject(%hit) || %hit.getGroup().bl_id == 888888)
			{
				if (%db.isDirt)
				{
					%brick.setWaterLevel(%brick.waterLevel - 5 * %numTimes);
				}
				else if (%db.isWaterTank)
				{
					%brick.setWaterLevel(%brick.waterLevel - 10 * %numTimes); //%db.maxWater * %numTimes / 500);
				}
			}
			%brick.nextHeat = $Sim::Time + 2;
		}
		
		%totalBricksProcessed++;
	}

	$masterHeatLoop = schedule(33, 0, heatLoop, %index - %totalBricksProcessed);
}

package rainPackage
{
	function fxDTSBrick::onAdd(%obj)
	{
		%db = %obj.getDatablock();
		if (%obj.isPlanted && (%db.isDirt || %db.isWaterTank))
		{
			RainFillSimSet.add(%obj);
		}

		return parent::onAdd(%obj);
	}
};
activatePackage(rainPackage);

function createRainSound(%vol, %db)
{
	if (isObject(ambientRain))
	{
		ambientRain.delete();
	}
	
	if (!isObject(%db))
	{
		%db = AmbientRainSound;
	}

	%m = new AudioEmitter(ambientRain)
	{
		position = "0 0 10000";
		profile = %db;
		useProfileDescription = false;
		description = "";
		type = "0";
		volume = %vol;
		outsideAmbient = "1";
		ReferenceDistance = "100000";
		maxDistance = 100000;
		isLooping = 1;
	};
	%m.setScopeAlways();
}

function startRainSound()
{
	schedule(2000, 0, createRainSound, 1, AmbientRainFadeInSound);
	schedule(6500, 0, createRainSound, 1, AmbientRainSound);
}

function stopRainSound()
{
	schedule(3000, ambientRain, createRainSound, 1, AmbientRainFadeOutSound);
	schedule(7800, 0, eval, "if (isObject(ambientRain)) ambientRain.delete();");
}

