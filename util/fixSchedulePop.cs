package fixSchedulePop
{
	//default code edit to not create as many schedules on initial call
	function Item::schedulePop(%obj)
	{
		%oldQuotaObject = getCurrentQuotaObject();
		if (isObject(%oldQuotaObject))
		{
			clearCurrentQuotaObject();
		}
		%obj.schedule($Game::Item::PopTime - 1000.0, "startFade", 1000, 0, 1);
		if (%obj.getDataBlock().doColorShift)
		{
			%color = getWords(%obj.getDataBlock().colorShiftColor, 0, 2);
			%obj.schedule($Game::Item::PopTime - 1000.0, "schedulePopLoop", %color, 1);
		}
		else
		{
			%obj.schedule($Game::Item::PopTime - 1000.0, "schedulePopLoop", "1 1 1",  1);
		}
		if (isObject(%oldQuotaObject))
		{
			setCurrentQuotaObject(%oldQuotaObject);
		}
	}
};
activatePackage(fixSchedulePop);

function Item::schedulePop(%obj)
{
	%oldQuotaObject = getCurrentQuotaObject();
	if (isObject(%oldQuotaObject))
	{
		clearCurrentQuotaObject();
	}
	%obj.schedule($Game::Item::PopTime - 1000.0, "startFade", 1000, 0, 1);
	if (%obj.getDataBlock().doColorShift)
	{
		%color = getWords(%obj.getDataBlock().colorShiftColor, 0, 2);
		%obj.schedule($Game::Item::PopTime - 1000.0, "schedulePopLoop", %color, 0.5);
	}
	else
	{
		%obj.schedule($Game::Item::PopTime - 1000.0, "schedulePopLoop", "1 1 1",  0.5);
	}
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(%oldQuotaObject);
	}
}

function Item::schedulePopLoop(%obj, %color, %transparency)
{
	%obj.startFade(1000, 0, 1);
	if (%transparency < 0.05)
	{
		%obj.delete();
		return;
	}

	%obj.setNodeColor("ALL", %color SPC %transparency);
	%obj.schedule(40, schedulePopLoop, %color, %transparency - 0.02);
}