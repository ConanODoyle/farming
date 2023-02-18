// Servercmdmessagesent overwrite - Enables discord-esq pings, custom name colors, titles, fake client chat, and more

function servercmdMessageSent(%client,%msg)
{
	serverCmdStopTalking(%client);

	%msg = stripMLControlChars(trim(%msg));
	%time = getSimTime();

	if(%msg $= "")
		return;

	%mini = getMinigameFromObject(%client);

	%name = %client.name;
	if(!$Pref::Server::DisableClanTags)
	{
		%pre = %client.clanPrefix;
		%suf = %client.clanSuffix;
	}

	// Custom titles
	if(%client.title !$= "")
		%pre = "<spush>" @ %client.title @ "<spop>" SPC %pre;

	// Donor ribbon if you want I guess
	//if(%client.isDonator)
	//	%pre = "<bitmap:base/client/ui/CI/blueRibbon.png>\c7 " @ %pre;

	if(%client.netname $= "CONSOLE")
		%name = "CONSOLE";

	if(!$forcedName[%client.BL_ID] $= "")
		%name = $forcedName[%client.BL_ID];

	//did they repeat the same message recently?
	if(stripChars(%msg, "!.?,") $= stripChars(%client.lastMsg, "!.?,") && %time-%client.lastMsgTime < $SPAM_PROTECTION_PERIOD)
	{
		if(!%client.isSpamming)
		{
			if(!%client.isAdmin)
			{
				messageClient(%client,'',"\c5Do not repeat yourself.");
				%client.isSpamming = 1;
				%client.spamProtectStart = %time;
				%client.schedule($SPAM_PENALTY_PERIOD,spamReset);
			}
		}
	}

	//are they sending messages too quickly?
	if(!%client.isAdmin && !%client.isSpamming)
	{
		if(%client.spamMessageCount >= $SPAM_MESSAGE_THRESHOLD)
		{
			%client.isSpamming = 1;
			%client.spamProtectStart = %time;
			%client.schedule($SPAM_PENALTY_PERIOD,spamReset);
		}
		else
		{
			%client.spamMessageCount ++;
			%client.schedule($SPAM_PROTECTION_PERIOD,spamMessageTimeout);
		}
	}

	//tell them they're spamming and block the message
	if(%client.isSpamming)
	{
		spamAlert(%client);
		return;
	}

	//eTard Filter
	if($Pref::Server::eTardFilter && !%client.isSuperAdmin)
	{
		%list = strReplace($Pref::Server::eTardList,",","\t");

		for(%i=0; %i < getFieldCount(%list); %i++)
		{
			%wrd = getField(%list,%i);
			if(striPos(" " @ %msg @ " ",%wrd) >= 0)
			{
				messageClient(%client,'',"\c5This is a civilized game. Please use full words.");
				if(!%client.isAdmin)
					return;
			}
		}
	}

	%player = %client.player;
	if(isObject(%player))
	{
		%player.playThread(3, "talk");
		%player.schedule(strlen(%msg) * 50, "playThread", 3, "root");
	}

	//URLs
	%msg = strReplace(%msg,"https://","http://");

	for(%i=0; %i < getWordCount(%msg); %i++)
	{
		%word = getWord(%msg,%i);
		%url  = getSubStr(%word,7,strLen(%word));

		if(getSubStr(%word,0,7) $= "http://" && strPos(%url,":") == -1)
		{
			%word = "<sPush><a:" @ %url @ ">" @ %url @ "</a><sPop>";
			%msg = setWord(%msg,%i,%word);
		}
	}

	// Custom name colors
	if(%client.isDonator)
		%color = "<color:ffaa00>";

	if($Pref::Farming::CustomNameColor[%client.BL_ID] !$= "")
		%color = $Pref::Farming::CustomNameColor[%client.BL_ID];
	else if(%client.nameColor !$= "")
		%color = %client.nameColor;

	if(%all $= "")
		%all  = '\c7%1\c3%5%2\c7%3%7: %4';

	// Discord ping stuff
	for (%i = 0; %i < getWordCount(%msg); %i++)
	{
		%pingWord = getWord(%msg, %i);
		if (%pingWord $= "@everyone")
		{
			%pingWord = "\c4@everyone\c6";

			if(%client.isAdmin)
				%pingAll = 1;
		}

		if (%pingWord $= "@here")
		{
			%pingWord = "\c4@here\c6";

			if(%client.isAdmin)
				%pingAll = 1;
		}

		%newMsg = %newMsg SPC %pingWord;
	}

	%newMsg = trim(%newMsg);
	%wordCount = getWordCount(%newMsg);
	%clientCount = ClientGroup.getCount();

	// Check for exact name matches
	for(%i = 0; %i < %wordCount; %i++)
	{
		%word = getWord(%newMsg, %i);

		for(%j = 0; %j < %clientCount; %j++)
		{
			%checkClient = ClientGroup.getObject(%j);
			%checkName = %checkClient.name;

			if("@" @ strLwr(%checkName) $= strLwr(%word))
			{
				%newMsg = setWord(%newMsg, %i, "\c4@" @ %checkName @ "\c6");
				%pingUser = %pingUser SPC %checkClient.getID();
			}
		}
	}

	// Check for partial name matches
	// I'M SORRY BUT THIS IS GONNA BE IMPOSSIBLE TO READ
	%msgLength = strLen(%newMsg);
	%cursor = 0;
	%error = 0;

	for(%i = 0; %i < %msgLength; %i += %ping + 1)
	{
		%error++;
		if(%error > 500)
		{
			announce("Ping chat exploded. This is a bug for Pecon to fix.");
			break;
		}

		%ping = strPos(getSubStr(%newMsg, %i, %msgLength), "@");

		if(%ping == -1)
			break; // No @s found

		if(%i > 0 || %ping > 0)
		{
			if(getSubStr(%newMsg, %i + %ping - 1, 1) !$= " ")
			{
				//warn("Preceeding space rule" SPC %i + %ping - 1);
				continue; // If the @ is preceeded by something other than a space, it is invalid.
			}
		}

		if(getSubStr(%newMsg, %ping + 1, 1) $= " ")
		{
			//warn("Following space rule");
			continue; // If the @ is followed by a space, it is invalid
		}

		//warn("Found @" SPC %ping SPC %i);

		%pingString = getSubStr(%newMsg, %i + %ping + 1, %msgLength);
		%pingLength = strLen(%pingString);
		%highestScore = 0;

		for(%j = 0; %j < %clientCount; %j++)
		{
			%pingTarget = ClientGroup.getObject(%j);

			if(%pingClient[%pingTarget])
				continue;

			%checkName = %pingTarget.name;
			%valid = false;
			%score = 0;
			//warn("Testing '" @ %checkName @ "' '" @ %pingString @ "'");

			for(%cursor = 1; %cursor <= %pingLength; %cursor++)
			{
				if(%cursor >= strLen(%checkName))
					break;

				if(strPos(strLwr(%pingString), strLwr(getSubStr(%checkName, 0, %cursor))) == 0)
				{
					//warn("Match" SPC strLwr(getSubStr(%checkName, 0, %cursor)));

					if(%cursor > %highestScore)
					{
						%score = %cursor;
						%scoreEndPos = %i + %ping + %cursor + 1;

						//warn("validity:" SPC getSubStr(%pingString, %cursor + 1, 1) SPC %pingString SPC %cursor + 1);
						if(getSubStr(%pingString, %cursor + 1, 1) $= " " || %cursor + 1 >= %pingLength)
						{
							%valid = true;

							if(%cursor >= strLen(%checkName))
								break;
						}
					}
				}
				else
				{
					break;
				}
			}

			if(%score > %highestScore && %valid)
			{
				%highestScore = %score;
				%highest = %pingTarget;
				%highestEndPos = %scoreEndPos;
			}
		}

		if(isObject(%highest))
		{
			//warn("Using" SPC %highestScore SPC %highestEndPos);
			%pingTarget[%highest] = true;
			%pingUser = %pingUser SPC %highest.getID();

			if (getSubStr(%newMsg, %highestEndPos - 1, 1) $= " ")
			{
				%extraSpace = " ";
			}
			%newMsg = getSubStr(%newMsg, 0, %i + %ping) @ "\c4@" @ %highest.name @ "\c6" @ %extraSpace @ getSubStr(%newMsg, %highestEndPos, %msgLength);
			%msgLength = strLen(%newMsg);
			%i += %ping + strLen(%highest.name) + 2;
		}
	}

	%pingUser = %pingUser @ " ";
	// End discord ping stuff


	// Process word replacers
	for(%i = 0; %i < $wordReplacerCount; %i++)
	{
		%newMsg = strireplace(%newMsg, $filterOldwords[%i], $filterNewWords[%i]);
	}


	%groupCount = clientGroup.getCount();
	for(%i = 0; %i < %groupCount; %i++)
	{
		%cl = clientGroup.getObject(%i);

		if(%pingAll)
		{
			if(isObject(Beep_Popup_Sound))
				%cl.play2D(nameToID("Beep_Popup_Sound"));

			commandToClient(%cl, 'chatMessage', %client, '', '', %all, "<div:1>" @ %pre, %name, %suf @ "\c6", %newMsg, %color, %team.name, "<color:ffffff>"); 
		}
		else if(trim(%pingUser) !$= "")
		{
			if (strPos(%pingUser, " " @ %cl.getID() @ " ") >= 0)
			{
				if(isObject(Beep_Popup_Sound))
					%cl.play2D(nameToID("Beep_Popup_Sound"));

				commandToClient(%cl, 'chatMessage', %client, '', '', %all, "<div:1>" @ %pre, %name, %suf @ "\c6", %newMsg, %color, %team.name, "<color:ffffff>"); 
			}
			else
			{
				commandToClient(%cl, 'chatMessage', %client, '', '', %all, %pre, %name, %suf @ "\c6", %newMsg, %color, %team.name, "<color:ffffff>"); 
			}
		}
		else
			commandToClient(%cl, 'chatMessage', %client, '', '', %all, %pre, %name, %suf @ "\c6", %msg, %color, %team.name, "<color:ffffff>"); 
	}

	echo(%name @ ":" SPC %newMsg);

	// Send to the discord bridge
	if(isFunction("sendMessage"))
		sendMessage(%client, stripMLControlChars(%newMsg));

	%client.lastMsg = %msg;
	%client.lastMsgTime = %time;
}

// Fake client stuff - Allows you to do servercmd commands from console more easily
if(!isObject(AIConsole))
	new scriptObject(AIConsole)
	{
		name = "Console";
		netname = "Console";
		BL_ID = ":robot:";
		isAdmin = true;
		isSuperAdmin = true;
		echoMessages = true;
		nameColor = "\c5";
	};

function AIConsole::chatMessage(%this, %msg)
{
	echo("[chatMessage]:" SPC %msg);
}

function AIConsole::centerPrint(%this, %msg, %time)
{
	echo("[centerPrint]:" SPC %msg);
}

function AIConsole::bottomPrint(%this, %msg, %time, %bar)
{
	echo("[bottomPrint]:" SPC %msg);
}

function talk(%message)
{
	servercmdMessageSent(AIConsole, %message);
}

function addWordReplacer(%oldword,%newword)
{
	$filterOldWords[$wordReplacerCount] = %oldWord;
	$filterNewWords[$wordReplacerCount] = %newWord;
	$wordReplacerCount++;
}

$wordReplacerCount = 0;
addWordReplacer("mlp", "cake");
addWordReplacer("pony", "cake");
addWordReplacer("furr", "cake");
addWordReplacer("rekt", "I am stupid.");
addWordReplacer("nigg", "cake");
addWordReplacer("negro", "cake");
addWordReplacer("fagg", "cake");
addWordReplacer("trann", "cake");
addWordReplacer("autis", "cake");
