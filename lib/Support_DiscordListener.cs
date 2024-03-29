function createDiscordMessageListener()
{
	if(isObject($DiscordMessageListener))
	{
		talk("Listener already exists!");
		return;
	}
	
	$DiscordMessageListener = new TCPObject(DiscordMessageListenerClass){};
	$DiscordMessageListener.schedule(8000, listen, 28008);
	
	echo("Discord listener running on port" SPC 28008);
}

function DiscordMessageListenerClass::onConnectRequest(%this, %ip, %id)
{
	if (getSubStr(%ip, 0, strPos(%ip, ":")) !$= "127.0.0.1") //ignoring connection attempts from other servers
	{
		return;
	}
	if(isObject(%this.connection[%ip]))
	{
		// echo(%this.getName() @ ": Got duplicate connection from" SPC %ip);
		%this.connection[%ip].disconnect();
		%this.connection[%ip].delete();
	}
	// echo(%this.getName() @ ": Creating connection to" SPC %ip);
	%this.connection[%ip] = new TCPobject("discordListenClient", %id){class = DiscordMessageListenerClass; parent = %this; client = %ip;};
	%this.connection[%ip].schedule(180000, delete);
}

function discordListenClient::onLine(%this, %line)
{
	if ($debugDiscordListener)
	{
		talk("[" @ strReplace(%line, "\t", "|") @ "]");
	}
	else if (getFieldCount(%line) > 2)
	{
		%key = getField(%line, 0);
		if (strPos(%key, $Pref::Server::server2blkey) != 0)
		{
			error("ERROR: discordListenClient::onLine - key mismatch!");
			return;
		}
		messageAll('', "<color:7289DA>@" @ getField(%line, 1) @ "\c6: " @ getFields(%line, 2, 200));
		echo("[DISCORD] @" @ getField(%line, 1) @ "\c6: " @ getFields(%line, 2, 200));
	}
	else
	{
		%key = getField(%line, 0);
		if (strPos(%key, $Pref::Server::server2blkey) != 0)
		{
			error("ERROR: discordListenClient::onLine - key mismatch!");
			return;
		}

		%word = getField(%line, 1);
		switch$ (%word)
		{
			case "playerlist": sendPlayerList();
		}
		return;
	}
}

function discordListenClient::onConnected(%this)
{
}
 
function discordListenClient::onConnectFailed(%this)
{
}

function discordListenClient::onDisconnect(%this)
{
	%this.schedule(0, delete);
}

function discordMessager::buildRequest(%this)
{
	%len = strLen(%this.query);
	%path = %this.path;

	if(%len)
	{
		%type		= "Content-Type: text/html; charset=windows-1252\r\n";
		if(%this.method $= "GET" || %this.method $= "HEAD")
		{
			%path	= %path @ "?" @ %this.query;
		}
		else
		{
			%length	= "Content-Length:" SPC %len @ "\r\n";
			%body	= %this.query;
		}
	}
	%requestLine	= %this.method SPC %path SPC %this.protocol @ "\r\n";
	%host			= "Host:" SPC %this.server @ "\r\n";
	%ua				= "User-Agent: Torque/1.3\r\n";
	%request = %requestLine @ %host @ %ua @ %length @ %type @ "\r\n" @ %body;
	return %request;
}

schedule(100, 0, createDiscordMessageListener);




function serverCmdToggleDiscordChat(%cl)
{
	if (!%cl.isAdmin)
	{
		return;
	}

	$DiscordChatDisabled = !$DiscordChatDisabled;
	%str = $DiscordChatDisabled ? "\c0OFF" : "\c2ON";
	messageAll('', "\c4Discord chat has been turned " @ %str);
}




package DiscordListener
{
	function serverCmdMessageSent(%cl, %msg)
	{
		parent::serverCmdMessageSent(%cl, %msg);

		// This is now handled directly in scripts/messageSent.cs
		//if($autoModeratorMute[%cl.BL_ID] < $sim::time && !%cl.isSpamming && !$DiscordChatDisabled)
		//{
		//	sendMessage(%cl, %msg);
		//}
	}

	function GameConnection::AutoAdminCheck(%cl)
	{
		sendMessage(%cl, "joined the server", "connection");
		%cl.hasCheckedAdmin = 1;
		return parent::AutoAdminCheck(%cl);
	}

	function GameConnection::onDrop(%cl)
	{
		if (%cl.hasCheckedAdmin)
		{
			sendMessage(%cl, "left the server", "connection");
		}
		return parent::onDrop(%cl);	
	}
};
activatePackage(DiscordListener);

if (isPackage(ChatEval))
{
	deactivatePackage(ChatEval);
	activatePackage(ChatEval);
}

function sendMessage(%cl, %msg, %type)
{
	%msg = urlEnc(%msg);
	%author = urlEnc(%cl.name);
	%bl_id = urlEnc(%cl.bl_id);
	%key = urlEnc($Pref::Server::bl2serverkey);
	%type = urlEnc(%type);

	%query = "author=" @ %author @ "&message=" @ %msg @ "&bl_id=" @ %bl_id @ "&verifykey=" @ %key @ "&type=" @ %type;
	%tcp = TCPClient("POST", "127.0.0.1", "28010", "/rcvmsg", %query, "", "discordMessager");
}

function purgeDiscordMessages(%cl)
{
	if (%cl.bl_id != 4928)
	{
		return;
	}
	%key = urlEnc($Pref::Server::bl2serverkey);

	%query = "verifykey=" @ %key;
	%tcp = TCPClient("POST", "127.0.0.1", "28010", "/purge", %query);
}

function serverCmdToggleDiscordChannel(%cl)
{
	if (!%cl.isSuperAdmin)
	{
		return;
	}
	%key = urlEnc($Pref::Server::bl2serverkey);

	%query = "verifykey=" @ %key;
	%tcp = TCPClient("POST", "127.0.0.1", "28010", "/toggleChannel", %query);
	sendMessage(AIConsole, "Toggled Discord Chat Bridge\x99 channel!");
	messageAll('', "\c3" @ %cl.name @ " \c5toggled the Discord Chat Bridge\x99 channel!");
}

function sendPlayerList()
{
	if ($nextPlayerList > $Sim::Time)
	{
		return;
	}

	%query = "verifykey=" @ urlEnc($Pref::Server::bl2serverkey);

	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%adminStr = %cl.isSuperAdmin ? "SA" : (%cl.isAdmin ? "A" : "-");
		%query = %query @ "&" @ urlEnc(%cl.bl_id) @ "=" @ urlEnc(%adminStr TAB %cl.name TAB %cl.getMoney());
	}

	%tcp = TCPClient("POST", "127.0.0.1", "28010", "/sendplayerlist", %query);
	$nextPlayerList = $Sim::Time + 2;
}