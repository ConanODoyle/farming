//////////////////////////////////////////////////////////////
//              Support_AutoSaver - Packaging               //
//////////////////////////////////////////////////////////////

//If we make any kind of modification (not events), then we will make it autosave. There is no easy way to tell if an event is changed, so I am not going to bother.
package Server_Autosaver
{
	//If you re-exec this add-on this function can possibly break (sometimes changing the gamemode causes the problem)
	//I have no idea why this happens, not on my side
	function ServerLoadSaveFile_End()
	{
		Parent::ServerLoadSaveFile_End();
		Autosaver_BootUp();
	}

	//Restarting the server could break the init prefs - This should prevent that
	function onServerDestroyed()
	{
		Parent::onServerDestroyed();
		$Server::AS::Init = 0;
		$Server::AS::HasAutoLoaded = 0;
	}

	function fxDtsBrick::setColor(%this, %id)
	{
		$Server::AS["BrickChanged"] = 1;
		Parent::setColor(%this, %id);
	}

	function fxDtsBrick::setColorFX(%this, %id)
	{
		$Server::AS["BrickChanged"] = 1;
		Parent::setColorFX(%this, %id);
	}

	function fxDtsBrick::setItem(%this, %id, %c)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setItem(%this, %id, %c);
	}

	function fxDtsBrick::setLight(%this, %id, %c)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setLight(%this, %id, %c);
	}

	function fxDtsBrick::setEmitter(%this, %id, %c)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setEmitter(%this, %id, %c);
	}

	function fxDtsBrick::setItemDirection(%this, %id, %c)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setItemDirection(%this, %id, %c);
	}

	function fxDtsBrick::setItemPosition(%this, %id, %c)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setItemPosition(%this, %id, %c);
	}

	function fxDtsBrick::setItemRespawnTime(%this, %time, %c)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setItemRespawnTime(%this, %time, %c);
	}

	function fxDtsBrick::setNTObjectName(%this, %name)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setNTObjectName(%this, %name);
	}

	function fxDtsBrick::setVehicle(%this, %vehicle, %c)
	{
		$Server::AS["BrickChanged"] = 1;
		return Parent::setVehicle(%this, %vehicle, %c);
	}
};
activatePackage("Server_Autosaver");