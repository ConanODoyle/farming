function createDiscordMessageListener()
{
	if(isObject($DiscordMessageListener))
		$DiscordMessageListener.delete();
	
	$DiscordMessageListener = new TCPObject(DiscordMessageListenerClass){};
	$DiscordMessageListener.listen(28008);
	
	talk("Discord listener running on port" SPC 28008);
}

function DiscordMessageListenerClass::onConnectRequest(%this, %ip, %id)
{
	if (getSubStr(%ip, 0, strPos(%ip, ":")) !$= "155.138.204.83") //ignoring connection attempts from other servers
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
		talk("[" @ %line @ "]");
	else
		%key = getField(%line, 0);
		if (strPos(%key, $Pref::Server::server2blkey) != 0)
		{
			error("ERROR: discordListenClient::onLine - key mismatch!");
			return;
		}
		messageAll('', "\c5@" @ getField(%line, 1) @ "\c6: " @ getFields(%line, 2, 200));
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

schedule(0, 0, createDiscordMessageListener);




package DiscordListener
{
	function serverCmdMessageSent(%cl, %msg)
	{
		parent::serverCmdMessageSent(%cl, %msg);

		if($autoModeratorMute[%cl.BL_ID] < $sim::time && !%cl.isSpamming && !$DiscordChatDisabled)
		{
			sendMessage(%cl, %msg);
		}
	}
};
activatePackage(DiscordListener);

if (isPackage(ChatEval))
{
	deactivatePackage(ChatEval);
	activatePackage(ChatEval);
}


function sendMessage(%cl, %msg)
{
	%msg = urlEnc(%msg);
	%author = urlEnc(%cl.name);
	%bl_id = urlEnc(%cl.bl_id);
	%key = urlEnc($Pref::Server::bl2serverkey);

	%query = "author=" @ %author @ "&message=" @ %msg @ "&bl_id=" @ %bl_id @ "&verifykey=" @ %key;
	%tcp = TCPClient("POST", "155.138.204.83", "28010", "/rcvmsg", %query);
}

function purgeDiscordMessages(%cl)
{
	if (%cl.bl_id != 4928)
	{
		return;
	}
	%key = urlEnc($Pref::Server::bl2serverkey);

	%query = "verifykey=" @ %key;
	%tcp = TCPClient("POST", "155.138.204.83", "28010", "/purge", %query);
}
