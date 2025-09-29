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
	eval("function updater::checkdaytick(%this, %date){ return; }");
	$GamemodeDisplayName = "Framing";
	infoLoop();
	loopSaveLots(0);
	findAllBuyerSpawns();
	loopApplyDateTime();
	dailyRefreshSchedule();
	announce("Started loops");
}

function stopLoops()
{
	cancel($masterGrowSchedule);
	announce("Stopped loops");
}


function createHostBrickgroup()
{
	%brickGroup = new SimGroup (("BrickGroup_" @ getMyBLID()));
	mainBrickGroup.add(%brickGroup);
	%brickGroup.name = "\c1BLID: " @ getMyBLID();
	%brickGroup.bl_id = getMyBLID();
}

function startup()
{
	createHostBrickgroup();
	serverCmdLoadAutosave(AIConsole, "last");
	$waitingForLoadFinish = 1;
}

function startup_postLoad()
{
	startLoops();
	resetAllBots(AIConsole);

	//load environment
	%file = new FileObject();
	%file.openForRead("Add-ons/Server_Farming/environment.txt");
	while (!%file.isEOF())
	{
		%line = %file.readLine();
		%var = getWord(%line, 0);
		%value = getWords(%line, 1, 10);
		serverCmdEnvGui_SetVar(AIConsole, %var, %value);
	}
	// commandToServer ('EnvGui_SetVar', %varName, %value);
	%file.close();
	%file.delete();

	//load environment zones
	serverCmdLoadEnvZones(AIConsole, "Farming");
	serverCmdHideEnvZones(AIConsole);


	$Pref::Server::Password = "";
	webcom_postServer();
}

package Farming_Startup
{
	function ServerLoadSaveFile_End()
	{
		%ret = parent::ServerLoadSaveFile_End();
		if ($waitingForLoadFinish)
		{
			$waitingForLoadFinish = 0;
			schedule(500, 0, startup_postLoad);
		}
		return %ret;
	}
};
activatePackage(Farming_Startup);
resetAllOpCallFunc();


schedule(15000, 0, startup);
