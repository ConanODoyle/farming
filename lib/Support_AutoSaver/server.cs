//Some stuff may be familiar from the older autosaver because there's no better way to make them.

/////////////////////////////////////////////////////////////
//              Support_AutoSaver - Debugging              //
//                                                         //
//              Set $Server::AS::State to 1                //
//           if you want to see how stuff works            //
/////////////////////////////////////////////////////////////

//All debug functions are commented, uncoment them if you want this to work
//You must have $Server::AS::Debug to 1 as well, this is if you want it there but you want to also toggle it and not keep commenting/uncommenting the code
//$Server::AS["Debug"] = 1;
function Autosaver_SetState(%state)
{
	if(%state $= "")
		return;

	if($Server::AS["State"] $= %state)
		return;

	$Server::AS["State"] = %state;
	if($Server::AS["Debug"])
	{
		messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c3Autosaver_SetState() - \c2" @ %state);
		echo("Autosaver_SetState() - " @ %state);
	}
}

//////////////////////////////////////////////////////////////////
//                  Support_AutoSaver - Prefs                   //
//                                                              //
//  A silly way to do prefs, but could be useful in the future  //
//////////////////////////////////////////////////////////////////

function Autosaver_PrefInit()
{
	if(!$Server::AS::Init)
	{
		//Autosaver_registerPref(%name, %varName, %default, %type)

		//Enables the autosaver
		Autosaver_registerPref("Enable", "Enabled", 1, "bool");

		//Announces any autosaver stuff into the chat. Console will always echo and log the time of the autosaver messages.
		Autosaver_registerPref("Announce", "Announce", 1, "bool");

		//Plays sounds with some messages, must have "Announce" enabled
		Autosaver_registerPref("Announce sounds", "Sounds", 0, "bool");

		//Announces what the save is being named to, must have "Announce" enabled
		Autosaver_registerPref("Announce save name", "AnnounceSaveName", 1, "bool");

		//Centerprints progress of saving
		Autosaver_registerPref("Centerprint progress", "ShowProgress", 0, "bool");

		//Reports how many events and brickgroups were saved, this is kind of pointless unless you want to know what is going on.
		Autosaver_registerPref("Report", "Report", 0, "bool");

		//Show time stamps on every autosaver message (console always shows time stamps)
		//This could create more clutter so this is off by default
		Autosaver_registerPref("Show time stamps", "TimeStamp", 0, "bool");

		//Show time elapsed on messages that do now show time (such as related saves not showing, save errors, etc)
		//Time elapsed is also shown in centerprint (if enabled)
		Autosaver_registerPref("Show time elapsed", "TimeElapsed", 1, "bool");

		//Interval to save (minutes)
		Autosaver_registerPref("Interval", "Interval", 5, "int 1 1440");

		//Location to save the files. Make sure to always have a "/" at the end.
		Autosaver_registerPref("Directory", "Directory", "saves/Autosaver/", "string 50 50");

		//Save ownership?
		Autosaver_registerPref("Ownership", "SaveOwnership", 1, "bool");

		//Save events? (Brick name is included on this)
		Autosaver_registerPref("Events", "SaveEvents", 1, "bool");

		//Painting a brick (color and fx), changing lights/emitters/items/name/etc will trigger the autosaver to actually save the build.
		Autosaver_registerPref("Overwrite on change", "OverwriteOnChange", 1, "bool");

		//Always save even though nothing has changed? I don't know why anyone would enable this but it's there in case someone wants to have multiple saves.
		//If you are on a budget of saving storage I do not recommend enabling this.
		Autosaver_registerPref("Save related brickcount", "SaveRelatedBrickcount", 0, "bool");

		//Save the bricks right after they are loaded, if this is disabled it will just schedule saving instead.
		//Note: This may not work if you have changed the gamemode without restarting the server completely
		Autosaver_registerPref("Save after bootloading", "ScheduleSaveOnBoot", 0, "bool");

		//Loads the last autosaved build on server start
		Autosaver_registerPref("Load save on start", "BootLoad", 0, "bool");

		//Neat saving is basically using a list and sort it by getting the distance from the ground. This makes it so when you load the autosave bricks load from bottom to top.
		//If this is disabled it is possible have bricks loading everywhere
		//If you are autosaving with tons of bricks (+80,000), I suggest disabling this to reduce lag when it sorts the distance
		Autosaver_registerPref("Enable neat saving", "NeatSaving", 0, "bool");

		//Turn off neat saving if there are too many bricks to handle (detected before autosaving bricks)
		Autosaver_registerPref("Neat saving auto-off", "NeatSaveProtect", 1, "bool");

		//Autosaver introduces a chunk saver which basically saves a bunch of bricks at once at every save tick.
		//Going over a certain amount such as 200 can possibly lag the server, this also varies on the tick preference
		//If you do not like chunk saving I recommend to have this preference around 1-25
		Autosaver_registerPref("Chunk saving count", "ChunkCount", 4000, "int 1 20000");

		//Attempt to remove any tempbricks that do not have an existing player
		Autosaver_registerPref("Remove unwanted tempbricks", "RemoveUnwantedTempbricks", 0, "bool");

		//This is the amount of time in MS to run another save loop
		Autosaver_registerPref("Save tick MS", "Tick", 1, "int 1 1000");

		//Amout of autosave files to keep - I have not tested this on a Mac or Linux, so this will be off by default
		Autosaver_registerPref("Max autosaves", "MaxSaves", -1, "int -1 1000");
	}

	$Server::AS::Init = 1;
}
schedule(0, 0, "Autosaver_PrefInit");
//Autosaver_registerPref(%name, %varName, %default, %type)

//This will register prefs for everything such as RTB, oRBS, or just vanilla - This will make it easier for me to add more prefs later
function Autosaver_registerPref(%name, %varName, %default, %type)
{
	//%type - Used for RTB/oRBS/etc. prefs - WILL NOT REGISTER IF THIS IS NOT A THING
	//%varName - Used for delaring the variable name, do not use spaces and such, or you will break someone's prefs
	//%default - Required to set default pref

	if(%default $= "")
	{
		error("Autosaver_registerPref(\"" @ %name @ "\") - Requires a default value");
		return;
	}

	$Server::AS["PrefCount"]++;

	//Syntax - I will not make a Server_Autosaver_getPref() command because that will just make the code much slower and we don't want that, so use this:
	//Make sure you don't use spaces and such or you will corrupt someone's prefs
	//$Pref::Server::AS_[blah]

	if(%varName $= "")
	{
		warn("Autosaver_registerPref(\"" @ %name @ "\") - Does not have a variable name, continuing");
		%varName = %name;
	}

	%varName = getSafeVariableName(%varName); //Putting protection on this anyways
	if(%type !$= "" && $AddOn__System_ReturnToBlockland == 1 && isFunction(RTB_registerPref))
		RTB_registerPref(%name, "Autosaver", "$Pref::Server::AS_" @ %varName, %type, "Support_AutoSaver", %default, 0, 0);
	else if(%type !$= "" && isFunction(ORBS_registerPref))
		ORBS_registerPref(%name, "Autosaver", "$Pref::Server::AS_" @ %varName, %type, "Support_AutoSaver", %default, 0, 0);
	else if($Pref::Server::AS_[%varName] $= "")
		$Pref::Server::AS_[%varName] = %default;
}

/////////////////////////////////////////////////////////////
//            Support_AutoSaver - AutoLoad Save            //
/////////////////////////////////////////////////////////////

function Autosaver_BootUp()
{
	cancel($AutosaverSch);
	//Autosaver_SetState("Autosaver_BootUp() - Starting loop");

	if(!$Pref::Server::AS_["ScheduleSaveOnBoot"])
		Autosaver_Schedule(1);
	else
		Autosaver_Begin(); //Save right away.
}

function Server_AutoloadSave(%bypass)
{
	if(!$Pref::Server::AS_["BootLoad"] && !%bypass)
		return;

	if($Autosaver::Pref["LastAutoSave"] $= "")
	{
		echo("[" @ getWord(getDateTime(), 1) @ "] [!] Failed to load last autosaved build. (Invalid file)");
		messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] Failed to load last autosaved build. (Invalid file)");
		return;
	}

	echo("[" @ getWord(getDateTime(), 1) @ "] [!] Autoloading last autosaved build, \"" @ $Autosaver::Pref["LastAutoSave"] @ "\"");
	messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] Autoloading last autosaved build, \"" @ $Autosaver::Pref["LastAutoSave"] @ "\"");
	loadAutoSave($Autosaver::Pref["LastAutoSave"]);
}

if(!$Server::AS::HasAutoLoaded)
{
	$Server::AS::HasAutoLoaded = 1;
	schedule(5000, 0, "Server_AutoloadSave");
}

/////////////////////////////////////////////////////////////
//              Support_AutoSaver - Commands               //
/////////////////////////////////////////////////////////////

function serverCmdToggleSaving(%this){serverCmdToggleAutosaver(%this);}
function serverCmdToggleSaver(%this){serverCmdToggleAutosaver(%this);}
function serverCmdToggleAS(%this){serverCmdToggleAutosaver(%this);}
function serverCmdTogAS(%this){serverCmdToggleAutosaver(%this);}
function serverCmdToggleAutosaver(%this)
{
	if(!%this.isSuperAdmin)
		return;

	$Pref::Server::AS_["Enabled"] = !$Pref::Server::AS_["Enabled"];
	%timestr = ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "");
	if($Pref::Server::AS_["Enabled"])
	{
		Autosaver_Schedule(1);
		messageAll('', '%1\c6[\c3!\c6] \c3%2 \c6has \c3enabled \c6the autosaver.', %timestr, %this.getPlayerName());
		echo("[" @ getWord(getDateTime(), 1) @ "] " @ %this.getPlayerName() @ " has enabled the autosaver.");
	}
	else
	{
		messageAll('', '%1\c6[\c3!\c6] \c3%2 \c6has \c3disabled \c6the autosaver.', %timestr, %this.getPlayerName());
		echo("[" @ getWord(getDateTime(), 1) @ "] " @ %this.getPlayerName() @ " has disabled the autosaver.");
	}
}

function serverCmdASB(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9){serverCmdAutoSaveBricks(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9);}
function serverCmdAutoSaveBricks(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9)
{
	if(!%this.isSuperAdmin)
		return;

	if($Server::AS["InUse"])
	{
		%this.chatMessage("\c6[\c3!\c6] Server is currently autosaving.");
		return;
	}

	if($Server::AS["Cooling"])
	{
		%this.chatMessage("\c6[\c3!\c6] Autosaver is currently optimizing the brick list.");
		return;
	}

	for(%i = 0; %i < 9; %i++)
	{
		if(%msg[%i] !$= "")
		{
			if(%msg $= "")
				%msg = %msg[%i];
			else
				%msg = %msg SPC %msg[%i];
		}
	}

	%msg = stripMLControLChars(%msg);
	%name = strReplace(%msg, "\"", "");
	%name = strReplace(%msg, "/", "");

	echo(%this.getPlayerName() @ " is attempting to " @ ($Pref::Server::AS_["Enabled"] ? "autosave" : "save") @ " bricks." @ (%name !$= "" ? " Custom name: \"" @ %name @ "\"" : ""));
	messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c3" @ %this.getPlayerName() @ " \c6is attempting to " @ ($Pref::Server::AS_["Enabled"] ? "autosave" : "save") @ " bricks." @ (%name !$= "" ? " Custom name: \"\c3" @ %name @ "\c6\"" : ""));

	$Server::AS["BrickChanged"] = 1;
	Autosaver_Begin(%name);
}

function serverCmdABID(%this, %bl_id, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9){serverCmdLoadAutoSaveID(%this, %bl_id, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9);}
function serverCmdAutoloadBricksID(%this, %bl_id, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9){serverCmdLoadAutoSaveID(%this, %bl_id, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9);}
function serverCmdLoadAutoSaveID(%this, %bl_id, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9)
{
	if(!%this.isSuperAdmin)
		return;

	for(%i = 0; %i < 9; %i++)
	{
		if(%msg[%i] !$= "")
		{
			if(%msg $= "")
				%msg = %msg[%i];
			else
				%msg = %msg SPC %msg[%i];
		}
	}

	%msg = stripMLControLChars(%msg);
	%msg = strReplace(%msg, "\"", "");
	%name = strReplace(%msg, "/", "");

	if(%msg $= "last")
	{
		echo(%this.getPlayerName() @ " is attempting to load bricks from the last autosave file for BL_ID: " @ %bl_id @ ".");
		messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c3" @ %this.getPlayerName() @ " \c6is attempting to load bricks from the last autosave file for BL_ID: " @ %bl_id @ ".");
	}
	else
	{
		echo(%this.getPlayerName() @ " is attempting to load bricks from an autosave file for BL_ID: " @ %bl_id @ ". - " @ %msg);
		messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c3" @ %this.getPlayerName() @ " \c6is attempting to load bricks from an autosave file for BL_ID: " @ %bl_id @ ". \c7- \c4" @ %msg);
	}

	loadAutoSave(%msg, %bl_id);
}

function serverCmdALB(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9){serverCmdLoadAutoSave(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9);}
function serverCmdLAS(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9){serverCmdLoadAutoSave(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9);}
function serverCmdAutoloadBricks(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9){serverCmdLoadAutoSave(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9);}
function serverCmdAutoloadSave(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9){serverCmdLoadAutoSave(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9);}
function serverCmdLoadAutoSave(%this, %msg0, %msg1, %msg2, %msg3, %msg4, %msg5, %msg6, %msg7, %msg8, %msg9)
{
	if(!%this.isSuperAdmin)
		return;

	for(%i = 0; %i < 9; %i++)
	{
		if(%msg[%i] !$= "")
		{
			if(%msg $= "")
				%msg = %msg[%i];
			else
				%msg = %msg SPC %msg[%i];
		}
	}

	%msg = trim(stripMLControLChars(%msg));
	%msg = strReplace(%msg, "\"", "");
	if(%msg $= "last")
	{
		echo(%this.getPlayerName() @ " is attempting to load bricks from the last autosave file.");
		messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c3" @ %this.getPlayerName() @ " \c6is attempting to load bricks from the last autosave file.");
		Server_AutoloadSave(1);
		return;
	}
	else
	{
		echo(%this.getPlayerName() @ " is attempting to load bricks from an autosave file. - " @ %msg);
		messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c3" @ %this.getPlayerName() @ " \c6is attempting to load bricks from an autosave file. \c7- \c4" @ %msg);
	}

	loadAutoSave(%msg);
}

//////////////////////////////////////////////////////////////
//             Support_AutoSaver - Brick parse              //
//                                                          //
//////////////////////////////////////////////////////////////

function fxDtsBrick::saveToFile(%brick, %events, %ownership, %f)
{
	if(!isObject(%brick) || !isObject(%f))
	{
		//error("fxDtsBrick::saveStrToFile() - File object does not exist!");
		return -1;
	}

	%data = %brick.getDataBlock();

	if(%data.hasPrint)
	{
		%texture = getPrintTexture(%brick.getPrintID());
		%path = filePath(%texture);
		%underscorePos = strPos(%path, "_");
		%name = getSubStr(%path, %underscorePos + 1, strPos(%path, "_", 14) - 14) @ "/" @ fileBase(%texture);
		if($printNameTable[%name] !$= "")
			%print = %name;
	}

	%f.writeLine(%data.uiName @ "\" " @ %brick.getPosition() SPC %brick.getAngleID() SPC %brick.isBasePlate() SPC %brick.getColorID() 
		SPC %print SPC %brick.getColorFXID() SPC %brick.getShapeFXID() SPC %brick.isRayCasting() SPC %brick.isColliding() SPC %brick.isRendering());

	if(%ownership && !$Server::LAN)
	{
		//%tempID = %brick.stackBL_ID;
		if(%tempID $= "")
			%tempID = getBrickGroupFromObject(%brick).bl_id;

		if(%tempID == -1)
			%tempID = 888888;

		if(%tempID !$= "")
			%f.writeLine("+-OWNER " @ %tempID);
		else
			%f.writeLine("+-OWNER 888888");
	}

	if(%events)
	{
		if(%brick.getName() !$= "")
			%f.writeLine("+-NTOBJECTNAME " @ %brick.getName());

		for(%b = 0; %b < %brick.numEvents; %b++)
		{
			%eventsFound++;
			%params = getFields(%brick.serializeEventToString(%b), 7, 10);
			%f.writeLine("+-EVENT" TAB %b TAB %brick.eventEnabled[%b] TAB %brick.eventInput[%b] TAB %brick.eventDelay[%b] TAB %brick.eventTarget[%b] 
				TAB %brick.eventNT[%b] TAB %brick.eventOutput[%b] TAB %params);
		}
	}
			
	if(isObject(%emitter = %brick.emitter) && isObject(%emitterData = %emitter.getEmitterDatablock()) && (%emitterName = %emitterData.uiName) !$= "")
		%f.writeLine("+-EMITTER " @ %emitterName @ "\" " @ %brick.emitterDirection);

	if(isObject(%light = %brick.getLightID()) && isObject(%lightData = %light.getDataBlock()) && (%lightName = %lightData.uiName) !$= "")
		%f.writeLine("+-LIGHT " @ %lightName @ "\" "); // Not sure if something else comes after the name

	if(isObject(%item = %brick.item) && isObject(%itemData = %item.getDataBlock()) && (%itemName = %itemData.uiName) !$= "")
		%f.writeLine("+-ITEM " @ %itemName @ "\" " @ %brick.itemPosition SPC %brick.itemDirection SPC %brick.itemRespawnTime);

	if(isObject(%audioEmitter = %brick.audioEmitter) && isObject(%audioData = %audioEmitter.getProfileID()) && (%audioName = %audioData.uiName) !$= "")
		%f.writeLine("+-AUDIOEMITTER " @ %audioName @ "\" "); // Not sure if something else comes after the name

	if(isObject(%spawnMarker = %brick.vehicleSpawnMarker) && (%spawnMarkerName = %spawnMarker.uiName) !$= "")
		%f.writeLine("+-VEHICLE " @ %spawnMarkerName @ "\" " @ %brick.reColorVehicle);

	return %eventsFound;
}

function fxDtsBrick::getSaveStr(%brick, %events, %ownership)
{
	%data = %brick.getDataBlock();
	if(%data.hasPrint)
	{
		%texture = getPrintTexture(%brick.getPrintId());
		%path = filePath(%texture);
		%underscorePos = strPos(%path, "_");
		%name = getSubStr(%path, %underscorePos + 1, strPos(%path, "_", 14) - 14) @ "/" @ fileBase(%texture);
		if($printNameTable[%name] !$= "")
			%print = %name;
	}

	%str = %data.uiName @ "\" " @ %brick.getPosition() SPC %brick.getAngleID() SPC %brick.isBasePlate() SPC %brick.getColorID() 
		SPC %print SPC %brick.getColorFXID() SPC %brick.getShapeFXID() SPC %brick.isRayCasting() SPC %brick.isColliding() SPC %brick.isRendering();

	if(%ownership && !$Server::LAN)
	{
		%tempID = getBrickGroupFromObject(%brick).bl_id;
		if(%tempID !$= "")
			%str = %str NL "+-OWNER " @ %tempID;
		else
			%str = %str NL "+-OWNER 888888";
	}

	if(%events)
	{
		if(%brick.getName() !$= "")
			%str = %str NL "+-NTOBJECTNAME " @ %brick.getName();

		for(%b = 0; %b < %brick.numEvents; %b++)
		{
			%params = getFields(%brick.serializeEventToString(%b), 7, 10);
			%str = %str NL "+-EVENT" TAB %b TAB %brick.eventEnabled[%b] TAB %brick.eventInput[%b] TAB %brick.eventDelay[%b] TAB %brick.eventTarget[%b] 
				TAB %brick.eventNT[%b] TAB %brick.eventOutput[%b] TAB %params;
		}
	}
			
	if(isObject(%emitter = %brick.emitter) && isObject(%emitterData = %emitter.getEmitterDatablock()) && (%emitterName = %emitterData.uiName) !$= "")
		%str = %str NL "+-EMITTER " @ %emitterName @ "\" " @ %brick.emitterDirection;

	if(isObject(%light = %brick.getLightID()) && isObject(%lightData = %light.getDataBlock()) && (%lightName = %lightData.uiName) !$= "")
		%str = %str NL "+-LIGHT " @ %lightName @ "\" "; // Not sure if something else comes after the name

	if(isObject(%item = %brick.item) && isObject(%itemData = %item.getDataBlock()) && (%itemName = %itemData.uiName) !$= "")
		%str = %str NL "+-ITEM " @ %itemName @ "\" " @ %brick.itemPosition SPC %brick.itemDirection SPC %brick.itemRespawnTime;

	if(isObject(%audioEmitter = %brick.audioEmitter) && isObject(%audioData = %audioEmitter.getProfileID()) && (%audioName = %audioData.uiName) !$= "")
		%str = %str NL "+-AUDIOEMITTER " @ %audioName @ "\" "; // Not sure if something else comes after the name

	if(isObject(%spawnMarker = %brick.vehicleSpawnMarker) && (%spawnMarkerName = %spawnMarker.uiName) !$= "")
		%str = %str NL "+-VEHICLE " @ %spawnMarkerName @ "\" " @ %brick.reColorVehicle;

	return %str;
}

/////////////////////////////////////////////////////////////
//        Support_AutoSaver - Core [Saving] [File]         //
/////////////////////////////////////////////////////////////
exec("./saving.cs");

/////////////////////////////////////////////////////////////
//       Support_AutoSaver - Core [Loading] [File]         //
/////////////////////////////////////////////////////////////
exec("./loading.cs");

/////////////////////////////////////////////////////////////
//        Support_AutoSaver - Core [Package] [File]        //
/////////////////////////////////////////////////////////////
exec("./package.cs");