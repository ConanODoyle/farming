package IPLogger
{
	function GameConnection::autoAdminCheck(%cl)
	{
		%ip = %cl.getRawIP();
		%blid = %cl.bl_id;

		if (strPos(" " @ $Pref::IPLogger @ " ", " " @ %ip @ " ") < 0)
		{
			$Pref::IPLogger::BLID_[%blid] = trim($Pref::IPLogger::BLID_[%blid] SPC %ip);
			export("$Pref*", "config/server/prefs.cs");
		}
		
		return parent::autoAdminCheck(%cl);
	}
};
activatePackage(IPLogger);

function collectAllClientsIP()
{
	deleteVariables("$IP_*");
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%ip = %cl.getRawIP();
		%port = getSubStr(%cl.getAddress(), strLen(%ip) + 1, 20);

		$Pref::IPLogger::BLID_[%cl.bl_id] = %ip TAB %port;
		$IP_[%ip] = trim($IP_[%ip] TAB %cl.name);
		// echo($Pref::IPLogger::BLID_[%cl.bl_id]);
	}
}

function listMultipleIPs()
{
	collectAllClientsIP();
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%ip = %cl.getRawIP();
		%port = getSubStr(%cl.getAddress(), strLen(%ip) + 1, 20);

		if (getFieldCount($IP_[%ip]) > 1 && !%ip_checked[%ip])
		{
			%ret = %ret TAB %ip @ ": " @ strReplace($IP_[%ip], "\t", ", ") @ " on same ip";
			// echo(%ip @ ": " @ strReplace($IP_[%ip], "\t", ", ") @ " on same ip");
			%ip_checked[%ip] = 1;
		}
	}
	return %ret;
}

function messageIPLog(%cl) 
{
	%fields = listMultipleIPs();
	%fields = removeField(%fields, 0);

	echo(getSafeVariableName(%cl.getPlayerName()) @ " checked for alts on same IP...");
	echo("----------------------------------------");
	for (%j = 0; %j < getFieldCount(%fields); %j++)
	{
		echo(getField(%fields, %j));
		messageClient(%cl, '', "\c6" @ getField(%fields, %j));
	}
	echo("----------------------------------------");
}

function serverCmdListDualIPs(%cl)
{
	if (%cl.isAdmin)
	{
		messageIPLog(%cl);
	}
}