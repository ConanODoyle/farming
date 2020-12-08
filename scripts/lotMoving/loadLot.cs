%error = forceRequiredAddon("Server_Floating_Bricks");

if (%error == $Error::Addon_NotFound)
{
	error("ERROR: Server_Farming/scripts/lotMoving/loadlot.cs - required add-on Server_Floating_Bricks not found!");
	error("Will not continue, please install Server_Floating_Bricks");
	crash();
}


function farmingProcessColorData(%loadFile, %colorMethod)
{
	%colorCount = -1;
	%i = 0;
	while (%i < 64)
	{
		if (getWord(getColorIDTable(%i), 3) > 0.001)
		{
			%colorCount += 1;
		}
		%i += 1;
	}
	if (%colorMethod == 0)
	{
		
	}	
	else if (%colorMethod == 1)
	{
		%divCount = 0;
		%i = 0;
		while (%i < 16)
		{
			if (getSprayCanDivisionSlot(%i) != 0)
			{
				%divCount += 1;
			}
			else 
			{
				break;
			}
			%i += 1;
		}
	}
	else if (%colorMethod == 2)
	{
		%colorCount = -1;
		%divCount = -1;
	}
	else if (%colorMethod == 3)
	{
		
	}
	%i = 0;
	while (%i < 64)
	{
		%color = %loadFile.readLine();
		%red = getWord(%color, 0);
		%green = getWord(%color, 1);
		%blue = getWord(%color, 2);
		%alpha = getWord(%color, 3);
		if (%colorMethod == 0)
		{
			if (%alpha >= 0.0001)
			{
				%match = 0;
				%j = 0;
				while (%j < 64)
				{
					if (colorMatch(getColorIDTable(%j), %color))
					{
						%loadFile.colorTranslation[%i] = %j;
						%match = 1;
						break;
					}
					%j += 1;
				}
				if (%match == 0)
				{
					error("ERROR: farmingProcessColorData() - color method 0 specified but match not found for color " @ %color);
				}
			}
		}
		else if (%colorMethod == 1)
		{
			if (%alpha >= 0.0001)
			{
				%match = 0;
				%j = 0;
				while (%j < 64)
				{
					if (colorMatch(getColorIDTable(%j), %color))
					{
						%loadFile.colorTranslation[%i] = %j;
						%match = 1;
						break;
					}
					%j += 1;
				}
				if (%match == 0)
				{
					setSprayCanColor(%colorCount += 1, %color);
					%loadFile.colorTranslation[%i] = %colorCount;
				}
			}
		}
		else if (%colorMethod == 2)
		{
			setSprayCanColor(%colorCount += 1, %color);
			%loadFile.colorTranslation[%i] = %i;
		}
		else if (%colorMethod == 3)
		{
			if (%alpha < 0.0001)
			{
				
			}
			else 
			{
				%minDiff = 99999;
				%matchIdx = -1;
				%j = 0;
				while (%j < 64)
				{
					%checkColor = getColorIDTable(%j);
					%checkRed = getWord(%checkColor, 0);
					%checkGreen = getWord(%checkColor, 1);
					%checkBlue = getWord(%checkColor, 2);
					%checkAlpha = getWord(%checkColor, 3);
					%diff = 0;
					%diff += mAbs(mAbs(%checkRed) - mAbs(%red));
					%diff += mAbs(mAbs(%checkGreen) - mAbs(%green));
					%diff += mAbs(mAbs(%checkBlue) - mAbs(%blue));
					if ((%checkAlpha > 0.99 && %alpha < 0.99) || (%checkAlpha < 0.99 && %alpha > 0.99))
					{
						%diff += 1000;
					}
					else 
					{
						%diff += mAbs(mAbs(%checkAlpha) - mAbs(%alpha)) * 0.5;
					}
					if (%diff < %minDiff)
					{
						%minDiff = %diff;
						%matchIdx = %j;
					}
					%j += 1;
				}
				if (%matchIdx == -1)
				{
					error("ERROR - LoadBricks() - Nearest match failed - wtf.");
				}
				else 
				{
					%loadFile.colorTranslation[%i] = %matchIdx;
				}
			}
		}
		%i += 1;
	}
	if (%colorMethod == 1)
	{
		echo("  setting spraycan division at ", %divCount, " ", %colorCount);
		setSprayCanDivision(%divCount, %colorCount, "File");
	}
	if (%colorMethod != 0 && %colorMethod != 3)
	{
		$maxSprayColors = %colorCount;
		%clientIndex = 0;
		while (%clientIndex < ClientGroup.getCount())
		{
			%cl = ClientGroup.getObject(%clientIndex);
			%cl.transmitStaticBrickData();
			%cl.transmitDataBlocks(1);
			commandToClient(%cl, 'PlayGui_LoadPaint');
			%clientIndex += 1;
		}
	}
}

function trueModulo(%num, %denominator)
{
	%denominator = mAbs(%denominator);
	return %num - %denominator * mFloor(%num / %denominator);
}

function rotatePoint(%center, %point, %deg)
{
	%deg = trueModulo(%deg, 360);
	%relativePoint = vectorSub(%point, %center);
	%x = getWord(%relativePoint, 0);
	%y = getWord(%relativePoint, 1);
	%z = getWord(%relativePoint, 2);
	%rad = %deg * $pi / 180;
	switch (%deg)
	{
		case 0:
			%rotatedPoint = %relativePoint;
		case 90:
			%rotatedPoint = -%y SPC %x SPC %z;
		case 180:
			%rotatedPoint = -%x SPC -%y SPC %z;
		case 270:
			%rotatedPoint = %y SPC -%x SPC %z;
		default:
			%rotatedPoint = mCos(%rad) * %x - mSin(%rad) * %y SPC mCos(%rad) * %y + mSin(%rad) * %x SPC %z;
	}
	return vectorAdd(%rotatedPoint, %center);
}

function farmingLoadEnd(%loadFile, %type, %dataObj, %brickGroup)
{
	// loop through all of the bricks
	%brickSet = %dataObj.brickSet;
	if (isObject(%brickSet))
	{
		%numBricks = %brickSet.getCount();
		for(%i = 0; %i < %numBricks; %i++)
		{
			%brick = %brickSet.getObject(%i);

			if (%brick.getDataBlock().isShopLot)
			{
				%brick.getGroup().shopLot = %brick;
			}

			// check if brick is floating
			if((!%brick.hasPathToGround() && %brick.getNumDownBricks() == 0) || %brick.getDataBlock().isLot || %brick.getDataBlock().isShopLot)
			{
				%brick.isBaseplate = true;
				%brick.onToolBreak(); // fix strange bug
				%brick.willCauseChainKill(); // recompute - thanks new duplicator
			}
		}
		%dataObj.brickSet.delete();
	}

	%time = getSimTime() - %startTime;
	%loadFile.delete();
	if (%type $= "Lot")
	{
		restoreLotBricks(%dataObj);
	}
	schedule(1000, %brickGroup, eval, %brickGroup @ ".isLoadingLot = 0;");
}

function farmingLoadTick(%loadFile, %type, %dataObj, %offset, %center, %rotation, %client, %brickGroup, %ownership, %lastLoadedBrick, %brickCount, %failCount) // needs replacing ServerLoadSaveFile_Tick
{
	if (isObject(ServerConnection))
	{
		if (!ServerConnection.isLocal())
		{
			return;
		}
	}
	%line = %loadFile.readLine();
	if (trim(%line) $= "")
	{
		return;
	}
	%firstWord = getWord(%line, 0);
	if (%firstWord $= "+-EVENT")
	{
		%prev = %brickGroup.client.isAdmin;
		%brickGroup.client.isAdmin = 1;
		if (isObject(%lastLoadedBrick))
		{
			%idx = getField(%line, 1);
			%enabled = getField(%line, 2);
			%inputName = getField(%line, 3);
			%delay = getField(%line, 4);
			%targetName = getField(%line, 5);
			%NT = getField(%line, 6);
			%outputName = getField(%line, 7);

			%par1 = getField(%line, 8);
			%par2 = getField(%line, 9);
			%par3 = getField(%line, 10);
			%par4 = getField(%line, 11);
			%inputEventIdx = inputEvent_GetInputEventIdx(%inputName);
			%targetIdx = inputEvent_GetTargetIndex("fxDTSBrick", %inputEventIdx, %targetName);
			if (%targetName == -1)
			{
				%targetClass = "fxDTSBrick";
			}
			else 
			{
				%field = getField($InputEvent_TargetList["fxDTSBrick", %inputEventIdx], %targetIdx);
				%targetClass = getWord(%field, 1);
			}
			%outputEventIdx = outputEvent_GetOutputEventIdx(%targetClass, %outputName);
			%NTNameIdx = -1;
			if (%client == %brickGroup)
			{
				%j = 0;
				while (%j < 4)
				{
					%field = getField($OutputEvent_parameterList[%targetClass, %outputEventIdx], %j);
					%dataType = getWord(%field, 0);
					if (%dataType $= "datablock")
					{
						if (%par[%j + 1] != -1 && !isObject(%par[%j + 1]))
						{
							warn("WARNING: could not find datablock for event " @ %outputName @ " -> " @ %par[%j + 1]);
						}
					}
					%j += 1;
				}
			}
			%client.wrenchBrick = %lastLoadedBrick;

			%globalLastLoaded = $LastLoadedBrick;
			$LastLoadedBrick = %lastLoadedBrick; // for serverCmdAddEvent

			serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
			$LastLoadedBrick = %globalLastLoaded; // restore global variable just in case

			%lastLoadedBrick.eventNT[%lastLoadedBrick.numEvents - 1] = %NT;
		}
		%brickGroup.client.isAdmin = %prev;
	}
	else if (%firstWord $= "+-NTOBJECTNAME")
	{
		if (isObject(%lastLoadedBrick))
		{
			%name = getWord(%line, 1);
			%lastLoadedBrick.setNTObjectName(%name);
		}
	}
	else if (%firstWord $= "+-LIGHT")
	{
		if (isObject(%lastLoadedBrick))
		{
			%line = getSubStr(%line, 8, strlen(%line) - 8);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Lights[%dbName];
			if (%client == %brickGroup)
			{
				if (!isObject(%db))
				{
					warn("WARNING: could not find light datablock for uiname \"" @ %dbName @ "\"");
				}
			}
			if (!isObject(%db))
			{
				%db = $uiNameTable_Lights["Player\'s Light"];
			}
			if ((strlen(%line) - %pos) - 2 >= 0)
			{
				%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
				%enabled = %line;
				if (%enabled $= "")
				{
					%enabled = 1;
				}
			}
			else 
			{
				%enabled = 1;
			}
			%quotaObject = getQuotaObjectFromBrick(%lastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			%lastLoadedBrick.setLight(%db);
			if (isObject(%lastLoadedBrick.light))
			{
				%lastLoadedBrick.light.setEnable(%enabled);
			}
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-EMITTER")
	{
		if (isObject(%lastLoadedBrick))
		{
			%line = getSubStr(%line, 10, strlen(%line) - 10);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			if (%dbName $= "NONE")
			{
				%db = 0;
			}
			else 
			{
				%db = $uiNameTable_Emitters[%dbName];
			}
			if (%client == %brickGroup)
			{
				if (%db $= "")
				{
					warn("WARNING: could not find emitter datablock for uiname \"" @ %dbName @ "\"");
				}
			}
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%dir = getWord(%line, 0);
			%quotaObject = getQuotaObjectFromBrick(%lastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			if (isObject(%db))
			{
				%lastLoadedBrick.setEmitter(%db);
			}
			%lastLoadedBrick.setEmitterDirection(%dir);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-ITEM")
	{
		if (isObject(%lastLoadedBrick))
		{
			%line = getSubStr(%line, 7, strlen(%line) - 7);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			if (%dbName $= "NONE")
			{
				%db = 0;
			}
			else 
			{
				%db = $uiNameTable_Items[%dbName];
			}
			if (%client == %brickGroup)
			{
				if (%dbName !$= "NONE" && !isObject(%db))
				{
					warn("WARNING: could not find item datablock for uiname \"" @ %dbName @ "\"");
				}
			}
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%pos = getWord(%line, 0);
			%dir = getWord(%line, 1);
			%respawnTime = getWord(%line, 2);
			%quotaObject = getQuotaObjectFromBrick(%lastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			if (isObject(%db))
			{
				%lastLoadedBrick.setItem(%db);
			}
			%lastLoadedBrick.setItemDirection(%dir);
			%lastLoadedBrick.setItemPosition(%pos);
			%lastLoadedBrick.setItemRespawntime(%respawnTime);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-AUDIOEMITTER")
	{
		if (isObject(%lastLoadedBrick))
		{
			%line = getSubStr(%line, 15, strlen(%line) - 15);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Music[%dbName];
			if (%client == %brickGroup)
			{
				if (!isObject(%db))
				{
					warn("WARNING: could not find music datablock for uiname \"" @ %dbName @ "\"");
				}
			}
			%quotaObject = getQuotaObjectFromBrick(%lastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			%lastLoadedBrick.setSound(%db);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "+-VEHICLE")
	{
		if (isObject(%lastLoadedBrick))
		{
			%line = getSubStr(%line, 10, strlen(%line) - 10);
			%pos = strpos(%line, "\"");
			%dbName = getSubStr(%line, 0, %pos);
			%db = $uiNameTable_Vehicle[%dbName];
			if (%client == %brickGroup)
			{
				if (!isObject(%db))
				{
					warn("WARNING: could not find vehicle datablock for uiname \"" @ %dbName @ "\"");
				}
			}
			%line = getSubStr(%line, %pos + 2, (strlen(%line) - %pos) - 2);
			%recolorVehicle = getWord(%line, 0);
			%quotaObject = getQuotaObjectFromBrick(%lastLoadedBrick);
			setCurrentQuotaObject(%quotaObject);
			%lastLoadedBrick.setVehicle(%db);
			%lastLoadedBrick.setReColorVehicle(%recolorVehicle);
			clearCurrentQuotaObject();
		}
	}
	else if (%firstWord $= "Linecount")
	{
		if (isObject(ProgressGui))
		{
			Progress_Bar.total = getWord(%line, 1);
			Progress_Bar.setValue(0);
			Progress_Bar.count = 0;
			Canvas.popDialog(ProgressGui);
			Progress_Window.setText("Loading Progress");
			Progress_Text.setText("Loading...");
		}
	}
	else if (%firstWord $= "+-OWNER")
	{
		if (isObject(%lastLoadedBrick))
		{
			if (%ownership == 1)
			{
				%ownerBLID = mAbs(mFloor(getWord(%line, 1)));
				%oldGroup = %lastLoadedBrick.getGroup();
				if ($Server::LAN)
				{
					%lastLoadedBrick.bl_id = %ownerBLID;
				}
				else if (%ownerBLID == 999999)
				{
					
				}
				else 
				{
					%ownerBrickGroup = "BrickGroup_" @ %ownerBLID;
					if (isObject(%ownerBrickGroup))
					{
						%ownerBrickGroup = %ownerBrickGroup.getId();
					}
					else 
					{
						%ownerBrickGroup = new SimGroup(("BrickGroup_" @ %ownerBLID));
						%ownerBrickGroup.client = 0;
						%ownerBrickGroup.name = "\c1BL_ID: " @ %ownerBLID @ "\c1\c0";
						%ownerBrickGroup.bl_id = %ownerBLID;
						mainBrickGroup.add(%ownerBrickGroup);
					}
					if (isObject(%ownerBrickGroup))
					{
						%ownerBrickGroup.add(%lastLoadedBrick);
						if (isObject(brickSpawnPointData))
						{
							if (%lastLoadedBrick.getDataBlock().getId() == brickSpawnPointData.getId())
							{
								if (%ownerBrickGroup != %oldGroup)
								{
									%oldGroup.removeSpawnBrick(%lastLoadedBrick);
									%ownerBrickGroup.addSpawnBrick(%lastLoadedBrick);
								}
							}
						}
					}
				}
			}
		}
	}
	else 
	{
		if (getBrickCount() >= getBrickLimit())
		{
			MessageAll('', 'Brick limit reached (%1)', getBrickLimit());
			farmingLoadEnd(%loadFile, %type, %dataObj, %brickGroup);
			return;
		}
		%quotePos = strstr(%line, "\"");
		if (%quotePos <= 0)
		{
			error("ERROR: farmingLoadTick() - Bad line \"" @ %line @ "\" - expected brick line but found no uiname");
			return;
		}
		%uiName = getSubStr(%line, 0, %quotePos);
		%db = $uiNameTable[%uiName];
		%line = getSubStr(%line, %quotePos + 2, 9999);
		%pos = getWords(%line, 0, 2);
		%angId = getWord(%line, 3);
		%isBaseplate = getWord(%line, 4) || %db.isLot || %db.isShopLot;
		%colorId = %loadFile.colorTranslation[mFloor(getWord(%line, 5))];
		%printName = getWord(%line, 6);
		if (strpos(%printName, "/") != -1)
		{
			%printName = fileBase(%printName);
			%aspectRatio = %db.printAspectRatio;
			%printIDName = %aspectRatio @ "/" @ %printName;
			%printId = $printNameTable[%printIDName];
			if (%printId $= "")
			{
				%printIDName = "Letters/" @ %printName;
				%printId = $printNameTable[%printIDName];
			}
			if (%printId $= "")
			{
				%printId = $printNameTable["Letters/-space"];
			}
		}
		else 
		{
			%printId = $printNameTable[%printName];
		}
		%colorFX = getWord(%line, 7);
		%shapeFX = getWord(%line, 8);
		%rayCasting = getWord(%line, 9);
		%collision = getWord(%line, 10);
		%rendering = getWord(%line, 11);
		%pos = VectorAdd(%pos, %offset);

		%pos = rotatePoint(%center, %pos, %rotation * 90);
		%angID = trueModulo(%angID - %rotation, 4);

		if (%db)
		{
			%trans = %pos;
			if (%angId == 0)
			{
				%trans = %trans SPC " 1 0 0 0";
			}
			else if (%angId == 1)
			{
				%trans = %trans SPC " 0 0 1" SPC $piOver2;
			}
			else if (%angId == 2)
			{
				%trans = %trans SPC " 0 0 1" SPC $pi;
			}
			else if (%angId == 3)
			{
				%trans = %trans SPC " 0 0 -1" SPC $piOver2;
			}
			%b = new fxDTSBrick("")
			{
				dataBlock = %db;
				angleID = %angId;
				isBasePlate = %isBaseplate;
				isFloatingBrick = %isBaseplate;
				forceBaseplate = %isBaseplate;
				colorID = %colorId;
				printID = %printId;
				colorFxID = %colorFX;
				shapeFxID = %shapeFX;
				isPlanted = 1;
				skipBuy = 1;
			};
			if (isObject(%brickGroup))
			{
				%brickGroup.add(%b);
			}
			else 
			{
				error("ERROR: farmingLoadTick() - %brickGroup does not exist!");
				MessageAll('', "ERROR: farmingLoadTick() - %brickGroup \"" @ %brickGroup @ "\" does not exist!");
				%b.delete();
				farmingLoadEnd(%loadFile, %type, %dataObj, %brickGroup);
				return;
			}
			%b.setTransform(%trans);
			%b.trustCheckFinished();
			%lastLoadedBrick = %b;
			%err = %b.plant();
			if (%err == 1 || %err == 3 || %err == 5)
			{
				%failCount += 1;
				%b.delete();
				%lastLoadedBrick = 0;
			}
			else 
			{
				if (%rayCasting !$= "")
				{
					%b.setRayCasting(%rayCasting);
				}
				if (%collision !$= "")
				{
					%b.setColliding(%collision);
				}
				if (%rendering !$= "")
				{
					%b.setRendering(%rendering);
				}
				if (%ownership && !$Server::LAN)
				{
					%oldGroup = %b.getGroup();
					%ownerGroup = "";
					if (%b.getNumDownBricks())
					{
						%ownerGroup = %b.getDownBrick(0).getGroup();
						%ownerGroup.add (%b);
					}
					else if (%b.getNumUpBricks())
					{
						%ownerGroup = %b.getUpBrick(0).getGroup();
						%ownerGroup.add(%b);
					}
					if (isObject(brickSpawnPointData))
					{
						if (%b.getDataBlock().getId() == brickSpawnPointData.getId())
						{
							if (%ownerGroup > 0 && %ownerGroup != %oldGroup)
							{
								%oldGroup.removeSpawnBrick(%b);
								%ownerGroup.addSpawnBrick(%b);
							}
						}
					}
				}
				else 
				{
					%lastLoadedBrick.client = %client;
				}
				if (!isObject(%dataObj.brickSet))
				{
					%dataObj.brickSet = new SimSet();
				}
				%dataObj.brickSet.add(%b);
			}
		}
		else 
		{
			if (!$Load_MissingBrickWarned[%uiName])
			{
				warn("WARNING: loadBricks() - DataBlock not found for brick named \"", %uiName, "\"");
				$Load_MissingBrickWarned[%uiName] = 1;
			}
			%lastLoadedBrick = 0;
			%failCount += 1;
		}
		%brickCount += 1;
		if (isObject(ProgressGui))
		{
			Progress_Bar.count += 1;
			Progress_Bar.setValue(Progress_Bar.count / Progress_Bar.total);
			if (Progress_Bar.count + 1 == Progress_Bar.total)
			{
				Canvas.popDialog(ProgressGui);
			}
		}
	}
	if (!%loadFile.isEOF())
	{
		if ($Server::ServerType $= "SinglePlayer")
		{
			$farmingLotLoadTickSchedule[%client.bl_id] = schedule(0, 0, farmingLoadTick, %loadFile, %type, %dataObj, %offset, %center, %rotation, %client, %brickGroup, %ownership, %lastLoadedBrick, %brickCount, %failCount);
		}
		else
		{
			$farmingLotLoadTickSchedule[%client.bl_id] = schedule(0, 0, farmingLoadTick, %loadFile, %type, %dataObj, %offset, %center, %rotation, %client, %brickGroup, %ownership, %lastLoadedBrick, %brickCount, %failCount);
		}
	}
	else
	{
		farmingLoadEnd(%loadFile, %type, %dataObj, %brickGroup);
	}
}

function farmingStartLoad(%filename, %type, %dataObj, %offset, %center, %rotation, %client, %brickGroup, %ownership, %colorMethod, %startTime)
{
	echo("LOADING BRICKS: " @ %filename @ " (ColorMethod " @ %colorMethod @ ")");
	if ($Game::MissionCleaningUp)
	{
		echo("LOADING CANCELED: Mission cleanup in progress");
		return;
	}
	%loadFile = new FileObject("");
	if (isFile(%filename))
	{
		%loadFile.openForRead(%filename);
	}
	else
	{
		%loadFile.openForRead("base/server/temp/temp.bls");
	}
	if ($UINameTableCreated == 0)
	{
		createUINameTable();
	}
	%lastLoadedBrick = 0;
	%failCount = 0;
	%brickCount = 0;
	%loadFile.readLine();
	%lineCount = %loadFile.readLine();
	%i = 0;
	while (%i < %lineCount)
	{
		%loadFile.readLine();
		%i += 1;
	}
	farmingProcessColorData(%loadFile, %colorMethod);
	%brickGroup.isLoadingLot = 1;
	farmingLoadTick(%loadFile, %type, %dataObj, %offset, %center, %rotation, %client, %brickGroup, %ownership, %lastLoadedBrick, %brickCount, %failCount);
	stopRaytracer();
}

function farmingDirectLoad(%client, %filename, %type, %dataObj, %offset, %center, %rotation, %colorMethod, %ownership)
{
	if (!isFile(%filename))
	{
		MessageAll('', "ERROR: File \"" @ %filename @ "\" not found.  If you are seeing this, you broke something.");
		return;
	}
	if (%ownership $= "")
	{
		%ownership = 1;
	}
	if (isObject(%client))
	{
		if (%ownership == 2)
		{
			%brickGroup = "BrickGroup_888888";
		}
		else
		{
			%brickGroup = %client.brickGroup;
		}
	}
	else
	{
		if ($Server::LAN)
		{
			if (%ownership == 2)
			{
				%brickGroup = "BrickGroup_888888";
			}
			else 
			{
				%brickGroup = "BrickGroup_999999";
			}
		}
		else if (%ownership == 2)
		{
			%brickGroup = "BrickGroup_888888";
		}
		else 
		{
			%brickGroup = "BrickGroup_" @ getMyBLID();
		}
		%brickGroup.isAdmin = 1;
		%brickGroup.brickGroup = %brickGroup;
		%client = %brickGroup;
	}
	%startTime = getSimTime();
	farmingStartLoad(%filename, %type, %dataObj, %offset, %center, %rotation, %client, %brickGroup, %ownership, %colorMethod, %startTime);
}