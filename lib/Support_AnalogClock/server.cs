
if (!isObject($ClockSimSet))
{
	$ClockSimSet = new SimSet(ClockSimSet);
}

datablock ItemData(GreenClockItem : HammerItem) {
	shapeFile = "./greenclock.dts";
	uiName = "Green Clock";
	doColorShift = false;

	image = "";
};

function GreenClockItem::onAdd(%this, %obj)
{
	%obj.canPickup = 0;
	$ClockSimSet.add(%obj);

	%obj.hideNode("min_basic");
	%obj.hideNode("min_point");
	%obj.hideNode("min_gap");
	%obj.hideNode("hr_basic");
	%obj.hideNode("hr_point");
	%obj.hideNode("hr_gap");
}


//assumes time given as hh:mm:ss or hh:mm, in 24 hour time
function playClockThread(%clock, %currTime, %smooth)
{
	%currTime = trim(strReplace(%currTime, ":", " "));
	%hr = getWord(%currTime, 0);
	%min = getWord(%currTime, 1);

	%clock.playThread(0, "hour" @ (%hr % 12));

	%clampmin = %min % 15;
	%segment = mFloor(%min / 15);

	if (%smooth)
	{
		%minThread = "min" @ %clampmin @ "t" @ (%clampmin+1);
	}
	else
	{
		%minThread = "min" @ %clampmin;
	}
	%segmentThread = "segment" @ %segment;

	%clock.playThread(1, %minThread);
	%clock.playThread(2, %segmentThread);
}

$smoothClock = 1;
function loopApplyDateTime(%clock)
{
	cancel($loopApplyDateTimeSchedule);

	%currTime = getWord(getDateTime(), 1);
	%split = trim(strReplace(%currTime, ":", " "));
	%hr = getWord(%split, 0);
	%min = getWord(%split, 1);
	%sec = getWord(%split, 2);

	%time = 200;
	if (%sec $= "00")
	{
		for (%i = 0; %i < $ClockSimSet.getCount(); %i++)
		{
			playClockThread($ClockSimSet.getObject(%i), %currTime, $smoothClock);
		}
		%time = 50000;
	}
	
	$loopApplyDateTimeSchedule = schedule(%time, 0, loopApplyDateTime, %clock);
}