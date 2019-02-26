function pickRandomEvent(%bot, %rand)
{
	switch$ (%rand)
	{
		case "buyProduce": //Produce Buy Bonus
			%crop = getRandomProduceType();
			%price = $Produce::BuyCost_[%crop] * 2;
			return "BUY 0" TAB %crop TAB %price TAB "I'm buying each " @ %crop @ " for double the price!";
		case "sellSeeds": //Seed sell discount
			%seed = getRandomSeedType();
			%price = $Produce::BuyCost_[%seed.stackType] / 2;
			%fixedName = strReplace(getSubStr(%seed, 0, strLen(%seed) - 8), "_", " ") SPC "seeds";
			return "SELL 1" TAB %seed TAB %price TAB "I'm selling " @ %fixedName @ " for half the price!";
		case "sellItem": //Special item
			%item = getRandomSpecialItem();
			%price = %item.cost * (10 - getRandom(0, 4)) / 8;
			return "SELL 1" TAB %item TAB %price TAB "I'm selling " @ %item.uiName @ "s!";
		case "sellInstrument": //Sell Instrument
			%item = getRandomInstrument();
			if (%item.uiName !$= "Keytar")
			{
				%price = 1000 * (5 - getRandom(0, 2)) / 4;
			}
			else
			{
				%price = 1000 * (5 - getRandom(0, 2)) / 4;
			}
			return "SELL 1" TAB %item TAB %price TAB "I'm selling " @ %item.uiName @ "s!";
		default: //nothing
			return "";
	}

	//shouldn't ever get here
	return "";
}

function AIPlayer::doRandomEventLoop(%bot, %time, %randOptions, %speak)
{
	cancel(%bot.randomEventSched);

	if (%bot.nextEventTime < $Sim::Time)
	{
		%bot.canRefresh = 0;
		%bot.refreshType = "";
		%rand = getRandom(getWordCount(%randOptions) - 1);
		%option = getWord(%randOptions, %rand);
		%event = pickRandomEvent(%bot, %option);
		%info = getField(%event, 3);

		if (%event !$= "")
		{
			%bot.canRefresh = 1;
			%bot.refreshType = %option;
			%bot.speak = %speak;
			%bot.time = %time;
			%bot.refreshTime = $Sim::Time + mCeil(%time * 2 / 3);
			echo("[" @ getDateTime() @ "] " @ %bot.name @ " has offered " @ getWord(%event, 0) SPC getField(%event, 1) @ " for " @ getField(%event, 2));
			%bot.setProduceData(getWord(%event, 1), getField(%event, 1) SPC getField(%event, 2), %bot.name, 500);
			// %bot.spawnBrick.setEventEnabled(1, 0);
			// %bot.spawnBrick.setEventEnabled(2, 1);
			%prefix = "<bitmap:base/data/particles/exclamation><bitmap:base/client/ui/ci/star>";
			if (%speak)
			{
				announce(%prefix @ "\c3" SPC %bot.name @ "\c6: " @ %info);
				%timeString = mFloor(%time / 60 + 0.5);
				%plural = %timeString > 1 ? "s" : "";
				announce(%prefix @ "\c3" SPC %bot.name @ "\c6: This deal lasts for " @ %timeString @ " minute" @ %plural @ "!");
			}
			else
			{
				announce(%prefix @ "\c3" SPC %bot.name @ "\c6: I got some new deals at the market!");
			}
		}
		else
		{
			echo("[" @ getDateTime() @ "] " @ %bot.name @ " has no offer");
			%bot.setProduceData(2);
			%bot.setShapeName(%bot.name);
			%bot.setShapeNameDistance(500);
			// %bot.spawnBrick.setEventEnabled(1, 1);
			// %bot.spawnBrick.setEventEnabled(2, 0);
			%timeBonus = %time / 2;
		}

		%bot.nextEventTime = $Sim::Time + %time - %timeBonus;
	}

	%bot.randomEventSched = %bot.schedule(5000, doRandomEventLoop, %time, %randOptions, %speak);
}

registerOutputEvent("Bot", "doRandomEventLoop", "int 0 100000 1200" TAB "string 200 100" TAB "bool", 1);

function AIPlayer::refreshOption(%bot, %cl)
{
	if (!%bot.canRefresh)
	{
		%bot.lookAtObject(%cl.player);
		messageClient(%cl, '', "<bitmap:base/client/ui/ci/star> \c3" @ %bot.name @ "\c6: I've already refreshed my deal once, sorry! Wait till the next one!");
		return;
	}
	else if (%bot.refreshTime > $Sim::Time)
	{
		%bot.lookAtObject(%cl.player);
		%time = mCeil(%bot.refreshTime - $Sim::Time);
		%plural = %time > 1 ? "s" : "";
		messageClient(%cl, '', "<bitmap:base/client/ui/ci/star> \c3" @ %bot.name @ "\c6: I just got this deal! You can refresh it in " @ %time @ " second" @ %plural @ ".");
		return;
	}
	else
	{
		messageAll('MsgUploadStart', "<bitmap:base/client/ui/ci/star> \c3" @ %bot.name @ "\c6: " @ %cl.name @ " refreshed my deal!");
	}

	%bot.canRefresh = 0;
	%option = %bot.refreshType;
	%event = pickRandomEvent(%bot, %option);
	%info = getField(%event, 3);
	%speak = %bot.speak;
	%time = %bot.time;

	echo("[" @ getDateTime() @ "] (REFRESH) " @ %bot.name @ " has offered " @ getWord(%event, 0) SPC getField(%event, 1) @ " for " @ getField(%event, 2));
	%bot.setProduceData(getWord(%event, 1), getField(%event, 1) SPC getField(%event, 2), %bot.name, 500);
	// %bot.spawnBrick.setEventEnabled(1, 0);
	// %bot.spawnBrick.setEventEnabled(2, 1);
	%prefix = "<bitmap:base/data/particles/exclamation><bitmap:base/client/ui/ci/star>";
	if (%speak)
	{
		announce(%prefix @ "\c3" SPC %bot.name @ "\c6: " @ %info);
		%timeString = mFloor(%time / 60 + 0.5);
		%plural = %timeString > 1 ? "s" : "";
		announce(%prefix @ "\c3" SPC %bot.name @ "\c6: This deal lasts for " @ %timeString @ " minute" @ %plural @ "!");
	}
	else
	{
		announce(%prefix @ "\c3" SPC %bot.name @ "\c6: I got some new deals at the market!");
	}
	%bot.nextEventTime = $Sim::Time + %time;
}

function serverCmdRefreshDeal(%cl)
{
	if (!%cl.isDonator)
	{
		messageClient(%cl, '', "You must be a donator to do this! <a:https://forum.blockland.us/index.php?topic=322462.0>Check the topic for the donation link!</a>");
		return;
	}
	else
	{
		if (isObject(%cl.player))
		{
			%pl = %cl.player;
			%start = %pl.getEyeTransform();
			%end = vectorAdd(%start, vectorScale(%pl.getEyeVector(), 5));
			%ray = containerRaycast(%start, %end, $TypeMasks::PlayerObjectType, %pl);

			if (isObject(%hit = getWord(%ray, 0)) && %hit.getClassName() $= "AIPlayer" && %hit.canRefresh !$= "")
			{
				%hit.refreshOption(%cl);
			}
		}
	}
}

function AIPlayer::chatRandomMessage(%bot, %cl)
{
	if (%bot.chatTimeout[%cl] > $Sim::Time)
	{
		return;
	}
	%bot.chatTimeout[%cl] = $Sim::Time + 1;

	%star = "<bitmap:base/client/ui/ci/star>";
	%name = %bot.name $= "" ? "Farmer" : %bot.name;

	if (strPos(strLwr(%name), "buy") >= 0)
	{
		%buyer = 1;
	}

	%msg = getRandom(10);
	switch (%msg)
	{
		case 0: %msg = "Nice weather outside, eh?";
		case 1: %msg = "It's a terrific day for rain... too bad Hughes isn't here...";
		case 2: %msg = "What's best in life? To plant your crops, see them grow before you, and reap the rewards of fertile soil!";
		case 3: %msg = "Later I'll have a deal you can't refuse!";
		case 4:
			if (%buyer) %msg = "Yes, I buy stuff, but I'm fully stocked!";
			else 		%msg = "Yes, I sell stuff, but I'm out of stock!";
		case 5: 
			if (%buyer) %msg = "I used to be a farmer, but then I realized I could make more money selling other people's crops";
			else		%msg = "I used to be a farmer, but then I took an opportunity to become a salesman.";
		case 6: 
			if (%buyer) %msg = "My unit limits are capped! I need some time to build an expansion.";
			else		%msg = "I ran out of supply! I need some time to construct additional storage units.";
		case 7: %msg = "If you need help, /tutorial gives you a basic rundown on everything!";
		case 8: %msg = "I have new deals every now and then, check back regularly to see what I offer!";
		case 9: %msg = "I've eaten a cabbage before. Yuck!";
		case 10: %msg = "You look new, have you read the tutorial?";
		default: 
			if (%buyer) %msg = "Hi! I sometimes buy stuff! Check back later to see what I'm buying.";
			else		%msg = "Hi! I sometimes sell stuff! Check back later to see what I have available.";
	}

	messageClient(%cl, '', %star SPC "\c3" @ %name @ "\c6: " @ %msg);
	%bot.setAimObject(%cl.getControlObject());
}

registerOutputEvent("Bot", "chatRandomMessage", "", 1);











//rain

if (!isObject(RainFillSimSet))
{
	$RainFillSimSet = new SimSet(RainFillSimSet){ };
}

$rainChance = 0.003;
$noRainModifier = 0.0001;

function rainCheckLoop()
{
	cancel($masterRainCheckLoop);

	if (!isObject(MissionCleanup))
	{
		return;
	}

	if ($lastRained $= "" || $lastRained > $Sim::Time)
	{
		$lastRained = $Sim::Time;
		$isRaining = 0;
	}

	%rainChance = $rainChance + $noRainModifier * mFloor(($Sim::Time - $lastRained + 0.1) / 15);
	%heatWaveChance = 0.004;
	if (!$isRaining && !$isHeatWave)
	{
		if (getRandom() < %rainChance)
		{
			// talk("Rainchance: " @ %rainChance);
			// talk("Last Rained: " @ $Sim::Time - $lastRained);
			echo("[" @ getDateTime() @ "] It has started raining");
			startRain();
		}
		// else if (getRandom() < %heatWaveChance)
		// {
		// 	echo("[" @ getDateTime() @ "] A heat wave has started");
		// 	startHeatWave();
		// }
	}
	else if ($isRaining ||$isHeatWave)
	{
		if ($isRaining)
		{
			$RainTicksLeft--;
			if ($RainTicksLeft <= 0)
			{
				echo("[" @ getDateTime() @ "] It has stopped raining");
				stopRain();
			}
		}
		// else if ($isHeatWave)
		// {
		// 	$HeatWaveTicksLeft--;
		// 	if ($HeatWaveTicksLeft <= 0)
		// 	{
		// 		echo("[" @ getDateTime() @ "] The heat wave has stopped");
		// 		stopHeatWave();
		// 	}
		// }
	}

	$masterRainCheckLoop = schedule(15000, 0, rainCheckLoop);
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

function roundToDecPoint(%num, %numDec)
{
	%factor = mPow(10, %numDec);
	return mFloor(%num * %factor + 0.5) / %factor;
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
		schedule((%i + 1) * 1200, 0, createRain, %drops);
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
		schedule((%i + 1) * 1200, 0, createRain, %drops);
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
		if (%i > %index) //we reached end of plant simset
		{
			break;
		}
		%brick = RainFillSimSet.getObject(%index - %i);

		if (%brick.nextRain $= "")
		{
			%brick.nextRain = $Sim::Time;
		}

		%db = %brick.getDatablock();
		%ray = %brick.getPosition();
		%ray = containerRaycast(%ray, vectorAdd(%ray, "0 0 100"), $TypeMasks::fxBrickAlwaysObjectType, %brick);
		if (%brick.nextRain < $Sim::Time)
		{
			%numTimes = mFloor(($Sim::Time - %brick.nextRain) / 2) + 1;
			if (!%brick.inGreenhouse && %brick.waterLevel < %db.maxWater)
			{
				if (%db.isDirt)
				{
					%brick.setWaterLevel(%brick.waterLevel + 30 * %numTimes);
				}
				else if (%db.isWaterTank && !isObject(%ray))
				{
					%brick.setWaterLevel(%brick.waterLevel + 50 * %numTimes); //%db.maxWater * %numTimes / 500);
				}
			}
			%brick.nextRain = $Sim::Time + 2;
		}
		
		%totalBricksProcessed++;
	}

	$masterRainLoop = schedule(33, 0, rainLoop, %index - %totalBricksProcessed);
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

function createRainSound(%vol)
{
	if (isObject(ambientRain))
	{
		ambientRain.delete();
	}
	%m = new AudioEmitter(ambientRain)
	{
		position = "0 0 10000";
		profile = AmbientRainSound;
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
	for (%i = 0; %i < 8; %i++)
	{
		schedule(1000 * (%i + 1) + getRandom(0, 6) * 500, 0, createRainSound, 0.4 / 8 * (%i + 1));
	}
}

function stopRainSound()
{
	for (%i = 0; %i < 8; %i++)
	{
		schedule(1000 * (%i + 1) + getRandom(0, 6) * 500, 0, createRainSound, 0.4 / 8 * (8 - %i));
	}
	schedule(%i * 1500, 0, eval, "AmbientRain.delete();");
}