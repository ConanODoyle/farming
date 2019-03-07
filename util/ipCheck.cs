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

		$Pref::TimeLogger::BLID_[%blid] = getRealTime();
		$Pref::TimeLogger::IP_[%ip] = getRealTime();
		
		return parent::autoAdminCheck(%cl);
	}

	function GameConnection::onDrop(%cl)
	{
		%ip = %cl.getRawIP();
		%blid = %cl.bl_id;

		$Pref::TimeLogger::BLID_[%blid] = getRealTime();
		$Pref::TimeLogger::IP_[%ip] = getRealTime();

		return parent::onDrop(%cl);
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
	messageClient(%cl, '', "\c6Checking for same-ip users...");

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

function serverCmdLastPlayed(%cl, %blid)
{
	if (!%cl.isAdmin)
	{
		return;
	}

	if ($Pref::TimeLogger::BLID_[%blid] $= "")
	{
		messageClient(%cl, '', "\c6BLID \c3" @ %blid @ "\c6 is not logged!");
		return;
	}
	else if (isObject(%t = findClientByBL_ID(%blid)))
	{
		messageClient(%cl, '', "\c3" @ %t.name @ "\c6 is on the server!");
		return;
	}

	%lastPlayed = getRealTime() - $Pref::TimeLogger::BLID_[%blid];
	%time = %lastPlayed / 1000 / 60 / 60 | 0;
	%day = mFloor(%time / 24 | 0);
	%hour = %time % 24;
	%dP = %day > 1 ? "s" : "";
	%hP = %hour > 1 ? "s" : "";
	%msg = (%day > 0 ? %day @ " day" @ %dP : "") @ %hour @ " hour" @ %hP;
	messageClient(%cl, '', "\c6BLID \c3" @ %blid @ "\c6 was last on the server" @ %time @ " ago!");
	return;
}