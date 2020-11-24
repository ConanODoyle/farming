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
	if(%client.donorRibbon)
		%pre = "<bitmap:base/client/ui/CI/blueRibbon.png>\c7 " @ %pre;

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
	if(%client.nameColor !$= "")
		%color = %client.nameColor;

	if(%all $= "")
		%all  = '\c7%1\c3%5%2\c7%3%7: %4';

	// Conan discord ping stuff
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

	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%pingTarget = ClientGroup.getObject(%i);
		%checkName = %pingTarget.name;

		if ((%ping = strPos(strLwr(%newMsg), "@" @ strLwr(%checkName))) >= 0)
		{
			%pingPre = getSubStr(%newMsg, 0, %ping);
			%pingMid = "\c4@" @ %checkName @ "\c6";
			%pingPost = getSubStr(%newMsg, %ping + strLen(%pingMid) - 1, 10000);
			// echo("pre: [" @ %pre @ "]");
			// echo("mid: [" @ %pingMid @ "]");
			// echo("post: [" @ %pingPost @ "]");
			%newMsg = %pingPre @ %pingMid SPC %pingPost;

			%pingUser = %pingUser SPC %pingTarget.getID();
		}
	}
	%pingUser = %pingUser @ " ";
	// End discord ping stuff


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

	echo(%name @ ":" SPC %msg);

	%client.lastMsg = %msg;
	%client.lastMsgTime = %time;
}

// Fake client stuff - Allows you to do servercmd commands from console more easily
if(!isObject(AIConsole))
	new scriptObject(AIConsole)
	{
		name = "Console";
		netname = "Console";
		BL_ID = 42;
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