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

		cancel(%obj.schedulePopSchedule);
		if (%obj.getDataBlock().isStackable)
		{
			%obj.schedulePopSchedule = %obj.schedule($Game::Item::PopTime - 1000 + $stackableItemExtraPopTime, schedulePopDelete);
		}
		else
		{
			%obj.schedulePopSchedule = %obj.schedule($Game::Item::PopTime - 1000, schedulePopDelete);
		}

		if (isObject(%oldQuotaObject))
		{
			setCurrentQuotaObject (%oldQuotaObject);
		}
	}
};
activatePackage(fixSchedulePop);

function Item::schedulePopDelete(%obj)
{
	%obj.startFade(1000, 0, 1);

	if (%obj.getDataBlock().doColorShift)
	{
		%color = getWords (%obj.getDataBlock().colorShiftColor, 0, 2);

		%obj.schedule(0, "setNodeColor", "ALL", %color  SPC 0.5);
		%obj.schedule(200,  "setNodeColor", "ALL", %color  SPC 0.4);
		%obj.schedule(400,  "setNodeColor", "ALL", %color  SPC 0.3);
		%obj.schedule(600,  "setNodeColor", "ALL", %color  SPC 0.2);
		%obj.schedule(800,  "setNodeColor", "ALL", %color  SPC 0.1);
	}
	else
	{
		%obj.schedule(0, "setNodeColor", "ALL", "1 1 1 0.5");
		%obj.schedule(200,  "setNodeColor", "ALL", "1 1 1 0.4");
		%obj.schedule(400,  "setNodeColor", "ALL", "1 1 1 0.3");
		%obj.schedule(600,  "setNodeColor", "ALL", "1 1 1 0.2");
		%obj.schedule(800,  "setNodeColor", "ALL", "1 1 1 0.1");
	}

	%obj.schedule(1000, "delete");
}