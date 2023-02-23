
%error = forceRequiredAddon("Support_Autosaver");

if (%error == $Error::Addon_NotFound)
{
	error("ERROR: Server_Farming/lib/Support_FarmingAutosaver.cs - required add-on Support_Autosaver not found!");
	error("Will not continue, please install Support_Autosaver");
	crash();
}

function farmingLoadAutosave(%path, %type, %dataObj, %rotation, %bl_id)
{
	if (!isFile(%path))
	{
		if (isFile(%oldPath = $Pref::Server::AS_["Directory"] @ %path @ ".bls"))
		{
			%path = %oldPath;
		}
		else
		{
			error("ERROR: farmingLoadAutosave - %path " @ %path @ " is not a valid file");
			talk("ERROR: farmingLoadAutosave - %path " @ %path @ " is not a valid file");
			return -1;
		}
	}

	%file = new FileObject();
	%file.openForRead(%path);
	%file.readLine();
	%file.readLine();
	%desc = %file.readLine();
	%file.close();
	%file.delete();
	%offset = getField(%desc, 1);
	%position = %dataObj.pos[0];

	if (%position !$= "")
	{
		%offset = vectorSub(%position, %offset);
	}
	else
	{
		%offset = "0 0 0";
	}
	// talk("Set loadoffset to " @ %offset @ " - description: " @ %desc);
	// return;
	echo("farmingLoadAutosave: " @ %dataObj);
	farmingDirectLoad(findClientByBL_ID(%bl_id), %path, %type, %dataObj, %offset, %position, %rotation, 3, 1);
}

function farmingLoadLastAutosave(%bl_id, %type, %dataObj, %rotation)
{
	if (isFile($Pref::Farming::Last[%type @ "Autosave" @ %bl_id]))
	{
		farmingLoadAutosave($Pref::Farming::Last[%type @ "Autosave" @ %bl_id], %type, %dataObj, %rotation, %bl_id);
		// talk("Loading lot " @ %bl_id @ " at " @ %dataObj.pos[0] @ "...");
		return;
	}
	echo("[" @ getDateTime() @ "] farmingLoadLastAutosave: No last " @ strLwr(%type) @ " found for BLID " @ %bl_id @ "!");
}

package FarmingAutosaverLoader
{
	function Autosaver_Begin(%name, %bl_id)
	{
		if ($Server::AS["InUse"])
		{
			return -100;
		}
		return parent::Autosaver_Begin(%name, %bl_id);
	}
};
activatePackage(FarmingAutosaverLoader);


//Assumes Visolator's Support_Autosaver mod is in use
//Overrides specific functions to enable saving of individual lots
function Autosaver_InitGroups()
{
	if(!$Server::AS["InUse"])
		return;

	//Autosaver_SetState("Gathering groups (tick " @ $Server::ASGroup_Current @ ")");
	if(!isObject($Server::ASGroup[$Server::ASGroup_Current]))
	{
		if($Pref::Server::AS_["ShowProgress"])
		{
			if($Pref::Server::AS_["TimeElapsed"])
			{
				%time = mCeil((getRealTime() - $Server::AS["Init"]) / 1000);
				%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
			}

			CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c2Complete " @ %timeStr, 2);
		}

		//Autosaver_SetState("Gathering groups DONE");

		if($Server::TempAS["NeatSaving"])
			$Server::AS["List"].sortNumerical(0, 1);
		
		//START OF CHANGES
		if ($LotCenterBrick $= " " || $Server::ASGroupCount > 1)
		{
			// if ($Server::ASGroupCount > 1)
			// 	talk("Global saving");
			// else
			// 	talk("Multiple or no lot center bricks found!");
			$LotCenterBrick = "";
		}
		else
		{
			// talk("One lot center brick found (" @ getField($LotCenterBrick, 0) @ ") [" @ getField($LotCenterBrick, 1) @ "]");
		}
		Autosaver_SaveInit($LotCenterBrick);
		//END OF CHANGES

		return;
	}
	else //START OF CHANGES
	{
		//during the first call of this function, meaning groups haven't been parsed for bricks yet
		//reset the lot center brick
		$LotCenterBrick = "";
	} //END OF CHANGES

	Autosave_GroupTick($Server::ASGroup[$Server::ASGroup_Current], 0);
}

function Autosave_GroupTick(%group, %count)
{
	if(!$Server::AS["InUse"] || !isObject(%list = $Server::AS["List"]) || !isObject(%group))
	{
		if($Pref::Server::AS_["Announce"])
		{
			%saveErrorTag = '';
			if($Pref::Server::AS_["Sounds"])
				%saveErrorTag = 'MsgClearBricks';

			%date = getDateTime();
			%diff = (getRealTime() - $Server::AS["Init"]);
			%time = %diff / 1000;

			if(%time > 60)
			{
				%timeString = getTimeString(mFloor(%time));
				%TimeElapsed = ($Pref::Server::AS_["TimeElapsed"] ? "Time elapsed: \c3" @ %time @ " minute" @ (%time != 1 ? "s" : "") : "");
				%TimeElapsedEcho = ($Pref::Server::AS_["TimeElapsed"] ? "Time elapsed: " @ %time @ " minute" @ (%time != 1 ? "s" : ""): "");
			}
			else
			{
				%time = mFloatLength(%time, 2);
				if(%time < 1)
					%time = 0;

				%TimeElapsed = ($Pref::Server::AS_["TimeElapsed"] ? "Time elapsed: \c3" @ %time @ " second" @ (%time != 1 ? "s" : "") : "");
				%TimeElapsedEcho = ($Pref::Server::AS_["TimeElapsed"] ? "Time elapsed: " @ %time @ " second" @ (%time != 1 ? "s" : ""): "");
			}

			messageAll(%saveErrorTag, ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6" @ ($Pref::Server::AS_["Enabled"] ? "Autosave" : "Save") @ " failed. Missing resources. " @ %TimeElapsed);
			error("Autosave_GroupTick() - Missing resources");
		}

		if($Pref::Server::AS_["ShowProgress"])
		{
			if($Pref::Server::AS_["TimeElapsed"])
			{
				%time = mCeil((getRealTime() - $Server::AS["Init"]) / 1000);
				%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
			}

			CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c0Failed " @ %timeStr, 1);
		}

		return;
	}

	%gc = %group.getCount();
	if(%count >= %gc)
	{
		if($Pref::Server::AS_["ShowProgress"])
		{
			if($Pref::Server::AS_["TimeElapsed"])
			{
				%time = mCeil((getRealTime() - $Server::AS["Init"]) / 1000);
				%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
			}

			CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c2Complete " @ %timeStr, 1);
		}

		//Autosaver_SetState("Gathering group " @ %group.bl_id @ " DONE - Overloaded");
		$Server::ASGroup_Current++;
		Autosaver_InitGroups(); 
		return;
	}

	if($Server::TempAS["NeatSaving"] && !$Server::AS["SaveWarn"] && $Server::AS["Brickcount"] > 75000)
	{
		$Server::AS["SaveWarn"] = 1;
		messageAll('', ($Pref::Server::AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c0Warning\c6: Many bricks detected, there may be lag.");
	}

	for(%i = %count; %i <= %count + $Pref::Server::AS_["ChunkCount"]; %i++)
	{
		if(%i >= %gc)
		{
			if($Pref::Server::AS_["ShowProgress"])
			{
				if($Pref::Server::AS_["TimeElapsed"])
				{
					%time = mCeil((getRealTime() - $Server::AS["Init"]) / 1000);
					%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
				}

				CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c2Complete " @ %timeStr, 1);
			}

			//Autosaver_SetState("Gathering group " @ %group.bl_id @ " DONE");
			$Server::ASGroup_Current++;
			Autosaver_InitGroups(); 
			return;
		}

		%brick = %group.getObject(%i);
		//START OF CHANGES
		if (%brick.getDatablock().isSingle) //this is a single lot, and it is the center of the lot complex
		{
			$LotCenterBrick = $LotCenterBrick $= "" ? %brick TAB %brick.getPosition() : " "; //space string in case multiple center bricks are found
		}
		//END OF CHANGES
		if(%brick.isPlanted)
		{
			if($Server::TempAS["NeatSaving"] && %list.getRowNumByID(%brick) == -1)
				%list.addRow(%brick, %brick.getDistanceFromGround());

			$Server::ASBrickIdx[$Server::AS["Brickcount"]] = %brick;
			$Server::AS["Brickcount"]++;
		}
		else if($Pref::Server::AS_["RemoveUnwantedTempbricks"] && isObject(%brick))
		{
			%del = 1;
			for(%i = 0; %i < ClientGroup.getCount(); %i++)
			{
				if(ClientGroup.getObject(%i).player.tempBrick == %brick)
					%del = 0;
			}
		
			if(%del) //This helps delete unwanted temp bricks that don't belong to anyone
				%brick.schedule(0, "delete");
		}
	}

	if($Pref::Server::AS_["ShowProgress"])
	{
		if($Pref::Server::AS_["TimeElapsed"])
		{
			%time = mCeil((getRealTime() - $Server::AS["Init"]) / 1000);
			%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
		}

		%progress = mFloatLength((($Server::ASGroup_Current / $Server::ASGroupCount)) * 100, 1);
		CenterPrintAll("<just:right>\c6Gathering groups... \n<font:arial bold:20>\c3" @ %progress @ "\c6% " @ %timeStr, 1);
	}

	schedule(33, 0, "Autosave_GroupTick", %group, %count + $Pref::Server::AS_["ChunkCount"] + 1);
}

//FUNCTION SIGNATURE CHANGED
function Autosaver_SaveInit(%desc, %extraSet)
{
	//Autosaver_SetState("Save init");
	%dir = $Pref::Server::AS_["Directory"];
	if(isObject($Server::AS["TempB"]))
	{
		$Server::AS["TempB"].close();
		$Server::AS["TempB"].delete();
	}

	if(!$Server::AS["InUse"])
		return;

	$Server::AS["TempB"] = new FileObject(){path = %dir @ "SAVETEMP.bls";};
	$Server::AS["TempB"].openForWrite($Server::AS["TempB"].path);

	$Server::AS["TempB"].writeLine("This is a Blockland save file.  You probably shouldn't modify it cause you'll mess it up.");
	$Server::AS["TempB"].writeLine("1");
	$Server::AS["TempB"].writeLine(%desc); //ADDED description export

	for(%i = 0; %i < 64; %i++)
		$Server::AS["TempB"].writeLine(getColorIDTable(%i));

	$Server::AS["TempB"].writeLine("Linecount " @ $Server::AS["Brickcount"]);

	$Server::AS["BricksSaved"] = 0;
	$Server::AS["Eventcount"] = 0;
	Autosaver_SaveTick($Server::AS["TempB"], 0);
}