/////////////////////////////////////////////////////////////
//             Support_AutoSaver - Core [Loading]          //
/////////////////////////////////////////////////////////////

//Short way of loading a save - This is probably useless but I will leave it here
function LoadSaveFromPath(%path)
{	
	if(!isFile(%path))
	{
		messageAll('', "LoadSaveFromPath() - invalid save file (" @ %path @ ")");
		return;
	}

	if(fileExt(%path) !$= ".bls")
	{
		messageAll('', "LoadSaveFromPath() - invalid save (" @ %path @ ")");
		return;
	}

	cancel($Server::AS["Schedule"]);

	//Put it as a temp file in case they have to reload it
	fileCopy(%path, "base/server/temp/temp.bls");
	
	%quotaObj = getCurrentQuotaObject();
	clearCurrentQuotaObject();
	serverDirectSaveFileLoad(%path, 3, "", 1);
	if(isObject(%quotaObj))
		setCurrentQuotaObject(%quotaObj);
}

//Load an autosave, if bl_id exists it will load from the bl_id
//If you want to go outside of the autosave folder use this instead of loadAutoSaveFromPath()
//Merged this into 1 function
function loadAutoSave(%name, %bl_id, %desc)
{	
	if(%name $= "last" && $Autosaver::Pref["LastAutoSave"] !$= "")
	{
		%path = $Pref::Server::AS_["Directory"] @ $Autosaver::Pref["LastAutoSave"] @ ".bls";
		%writePath = $Pref::Server::AS_["Directory"] @ $Autosaver::Pref["LastAutoSave"] @ "TEMPBL_ID.bls";
	}
	else
	{
		%path = $Pref::Server::AS_["Directory"] @ %name @ ".bls";
		%writePath = $Pref::Server::AS_["Directory"] @ "TEMPBL_ID.bls";
	}

	if(!isFile(%path))
	{
		messageAll('', "loadAutoSave() - invalid save (" @ %path @ ")");
		return;
	}

	//Autosaver_SetState("Loading autosave with args (" @ %name @ ", " @ %bl_id @ ", " @ %desc @ ")");

	//If we have a bl_id let's try to only load the build based on who made it
	if(%bl_id !$= "" && mFloor(%bl_id) $= %bl_id && %bl_id >= 0)
	{
		%io2 = new FileObject();
		%io2.openForRead(%path);
		%start = false;
		while(!%io2.isEOF())
		{
			%line = %io2.readLine();
			%type = getWord(%line, 0);
			if(%type $= "Linecount" && !%start)
				%start = true;

			if(%start)
			{
				if(%type $= "+-OWNER" && getWord(%line, 1) == %bl_id)
					%bl_idCount++;
			}
		}
		%io2.close();
		%io2.delete();

		if(%bl_idCount <= 0)
		{
			messageAll('', "loadAutoSave() - no bl_id ownership for " @ %bl_id);
			return;
		}

		//Write a new thing
		%io = new FileObject();
		%io.openForWrite(%writePath);
		%io.writeLine("This is a Blockland save file.  You probably shouldn't modify it cause you'll screw it up.  This is an autosave file.");
		%io.writeLine("1");
		%io.writeLine(%desc);

		for(%i = 0; %i < 64; %i++)
			%io.writeLine(getColorIDTable(%i));

		%io.writeLine("Linecount " @ %bl_idCount);

		%io2 = new FileObject();
		%io2.openForRead(%path);
		%start = false;
		while(!%io2.isEOF())
		{
			%line = %io2.readLine();
			%type = getWord(%line, 0);
			if(%type $= "Linecount" && !%start)
				%start = true;

			if(%start)
			{
				if(%type !$= "+-OWNER" && %type !$= "+-ITEM" && %type !$= "+-NTOBJECTNAME" && %type !$= "+-EMITTER" && %type !$= "+-LIGHT" && %type !$= "+-AUDIOEMITTER" && %type !$= "+-VEHICLE" && strPos(%line, "\"") >= 0)
				{
					%writeMode = 0;
					%lastBrickLine = %line;
				}
				else if(%lastBrickLine !$= "" && %type $= "+-OWNER" && getWord(%line, 1) == %bl_id)
				{
					%writeMode = 1;
					%io.writeLine(%lastBrickLine);
					%io.writeLine("+-OWNER " @ %bl_id);
				}
				else if(%type !$= "+-OWNER" && %writeMode)
					%io.writeLine(%line);
			}
		}
		%io2.close();
		%io2.delete();

		%io.close();
		%io.delete();

		//Put it as a temp file in case they have to reload it
		fileCopy(%writePath, "base/server/temp/temp.bls");
	}
	else
		fileCopy(%path, "base/server/temp/temp.bls");


	cancel($Server::AS["Schedule"]);
	
	//Sometimes events don't load or something else breaks because of a quota object missing (?)
	%quotaObj = getCurrentQuotaObject();
	clearCurrentQuotaObject();
	serverDirectSaveFileLoad(%path, 3, "", 1);
	if(isObject(%quotaObj))
		setCurrentQuotaObject(%quotaObj);
}