function startLoops()
{
	growTick(0);
	sprinklerTick(0);
	compostTick(0);
	weedTick(0);
	fishingTick(0);
	$disableWeather = 0;
	//fishingTick(0);
	_sequenceBot.setHSpawnClose(1, 0);
	randomBappsMatLoop();
	randomPostOfficeLoop();
}

function stopLoops()
{
	cancel($masterGrowSchedule);
}