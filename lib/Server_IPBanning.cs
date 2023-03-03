// Pecon7
// 12/30/12 - 10/22/14
// IP ban script. Ban players by their IP so they cannot rejoin from a different ID.

if(!$pref::server::IPBanningMode $= "")
	$pref::server::IPBanningMode = "ban";

if(!isfile("config/server/IPBanList.txt"))
{
	%file = new fileobject();
	%file.openforwrite("config/server/IPBanList.txt");
	%file.close();
	%file.delete();
}

package IPBanning
{
	function GameConnection::AutoAdminCheck(%this)
	{
		if(checkIfIPIsBanned(%this.getRawIP()))
			BanBLID(%this.BL_ID, -1, "Your IP address" SPC %this.getRawIP() SPC "is banned from this server.");
		else
			parent::AutoAdminCheck(%this);
	}
};

package IPBlocking
{
	function GameConnection::onConnectRequest(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i)
	{
		%ip = %this.getRawIP();
		
		if(checkIfIPIsBanned(%ip))
		{
			// At this point the connected client simply gets a connection error rather than this message. I don't know why but it doesn't look fixable.
			%this.schedule(0, delete, "You are banned from this server.");
		}
		else
			parent::onConnectRequest(%this,%a,%b,%c,%d,%e,%f,%g,%h,%i);
	}
};

if($pref::server::IPBanningMode $= "ban")
	activatePackage(IPBanning);
else if($pref::server::IPBanningMode $= "block")
	activatePackage(IPBlocking);
else
	activatePackage(IPBanning);

function checkIfIPIsBanned(%IP)
{
	%file = new fileobject();
	%file.openforread("config/server/IPBanList.txt");
	
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		
		if(getField(%line, 0) $= %IP)
		{
			%file.close();
			%file.delete();
			return true;
		}
	}
	
	%file.close();
	%file.delete();
	
	return false;
}

function servercmdchangeIPBanMode(%client)
{
	if(!%client.isSuperAdmin)
		return;
		
	switch$($pref::server::IPBanningMode)
	{
		case "ban":
			$pref::server::IPBanningMode = "block";
			deactivatePackage(IPBanning);
			activatePackage(IPBlocking);
			%client.chatMessage("IP Banning mode switched to block.");
		case "block":
			$pref::server::IPBanningMode = "ban";
			deactivatePackage(IPBlocking);
			activatePackage(IPBanning);
			%client.chatMessage("IP Banning mode switched to ban.");
		default:
			$pref::server::IPBanningMode = "ban";
			deactivatePackage(IPBlocking);
			activatePackage(IPBanning);
			%client.chatMessage("IP Banning mode switched to ban.");	
	}
}

function servercmdUnBanIP(%client, %IP)
{
	servercmdUnIPBan(%client, %IP);
}

function servercmdUnIPBan(%client, %IP)
{
	if(!%client.isSuperAdmin)
		return;
		
	%IP = strLwr(%IP); //IPv6 is a thing, just not quite yet
		
	%file = new FileObject(){};
	if(!%file.openForRead("config/server/IPBanList.txt"))
	{
		%file.close();
		%file.delete();
		%client.chatMessage("ERROR: Ban list does not exist!");
		return;
	}
	
	%skipLine = -1;
	%i = 0;
	while(!%file.isEOF())
	{
		%line[%i] = %file.readLine();
		%lineIP = strLwr(getField(%line[%i], 0));
		if(%IP $= %lineIP)
		{
			%skipLine = %i;
		}
		%i++;
	}
	
	%file.close();
	
	if(%skipLine == -1)
	{
		%client.chatMessage("That IP address was not found in the ban list.");
		return;
	}
	
	%file.openForWrite("config/server/IPBanList.txt");
	
	for(%k = 0; %k < %i; %k++)
	{
		if(%k == %skipLine)
			continue;
		
		%file.writeLine(%line[%k]);
	}
	
	%file.close();
	%file.delete();
	
	%client.chatMessage("Successfully removed" SPC %IP SPC "from the IP ban list.");
}

function serverCMDListIPBans(%client)
{
	if(!%client.isAdmin)
		return;
		
	%file = new FileObject(){};
	
	if(!%file.openForRead("config/server/IPBanList.txt"))
	{
		%file.close();
		%file.delete();
		%client.chatMessage("ERROR: Ban list does not exist!");
		return;
	}
	
	while(!%file.isEOF())
	{
		%line = %file.readLine();
		%IP = getField(%line, 0);
		%date = getField(%line, 1);
		%name = getField(%line, 2);
		%ID = getField(%line, 3);
		
		if(%date $= "")
			%client.chatMessage(%IP SPC "is banned.");
		else
			%client.chatMessage("On" SPC %date @ "," SPC %name SPC "(" @ %ID @ ")" SPC "banned the IP" SPC %IP);
	}
}

function serverCMDBanIP(%client, %IP)
{
	if(!%client.issuperadmin)
	{
		%client.chatmessage("Only super admins can do this.");
		return;
	}
	
	%ip = stripChars(%ip, "*;!@#$%^&*()-_+='/?><,`~\|");
	%ip = trim(%ip);
	
	// I'm guessing that when IPv6 finally hits Blockland's networking will be screwed, but just in case it isn't I'm leaving support for it here.
	if(strlen(%ip) < 6 || strLen(%ip) > 39)
	{
		%client.chatmessage("Invalid IP address.");
		return;
	}	
	
	%client.chatmessage("Banned IP" SPC %IP);
	
	%file = new fileobject();
	%file.openforappend("config/server/IPBanList.txt");
	%file.writeline(%IP TAB getDateTime() TAB %client.name TAB %client.BL_ID);
	%file.close();
	%file.delete();
}

function serverCmdIPBan(%client, %name)
{
	if(!%client.issuperadmin)
	{
		%client.chatmessage("Only super admins can do this.");
		return;
	}

	if(!isobject(%target = findclientbyname(%name)) || %name $= "")
	{
		if(!isobject(%target = findclientbyBL_ID(%name)) || %name $= "")
		{
			%client.chatmessage("Invalid name or BL_ID. Use /banIP to manually enter an IP address to ban.");
			return;
		}
	}
	
	%ip = %target.getRawIP();
		
	%client.chatmessage("IP banned" SPC %target.name SPC "(" @ %ip @ ")");
	
	
	%file = new fileobject();
	%file.openforappend("config/server/IPBanList.txt");
	%file.writeline(%IP TAB getDateTime() TAB %client.name TAB %client.BL_ID);
	%file.close();
	%file.delete();
	
	
	// BanBLID(%target.BL_ID, -1, "Your IP address" SPC %ip SPC "is banned from this server.");
	%target.ipBan();
}

function gameConnection::ipBan(%this)
{
	switch$($pref::server::IPBanningMode)
	{
		case "ban":
			banBLID(%this.BL_ID, -1, "Your IP address" SPC %this.getRawIP() SPC "is banned from this server.");
		case "block":
			%this.delete("You are banned from this server.");
		default:
			error("IP banning mode undefined!");
	}
}

function servercmdCheckIP(%client, %name)
{
	if(!%client.issuperadmin)
		return;
		
	if(!isobject(%target = findclientbyname(%name)))
		if(!isobject(%target = findclientbyBL_ID(%name)))
			return;
		
	messageClient(%client, '', %target.name @ "'s IP is" SPC %target.getrawIP());
}