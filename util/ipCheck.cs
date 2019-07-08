package IPLogger
{
	function GameConnection::autoAdminCheck(%cl)
	{
		%ip = %cl.getRawIP();
		%port = getSubStr(%cl.getAddress(), strLen(%ip) + 1, 20);
		%blid = %cl.bl_id;

		if (strPos(" " @ $Pref::IPLogger @ "\t", " " @ %ip @ "\t") < 0)
		{
			$Pref::IPLogger::BLID_[%blid] = trim($Pref::IPLogger::BLID_[%blid] SPC %ip TAB %port);
			export("$Pref*", "config/server/prefs.cs");
		}

		$Pref::TimeLogger::BLID_[%blid] = getRealTime();
		%ip_ = strReplace(%ip, ".", "_");
		$Pref::TimeLogger::IP_[%ip_] = getRealTime();

		return parent::autoAdminCheck(%cl);
	}

	function GameConnection::onDrop(%cl)
	{
		%ip = %cl.getRawIP();
		%blid = %cl.bl_id;

		$Pref::TimeLogger::BLID_[%blid] = getRealTime();
		%ip_ = strReplace(%ip, ".", "_");
		$Pref::TimeLogger::IP_[%ip_] = getRealTime();

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
		%ip_ = strReplace(%ip, ".", "_");
		$IP_[%ip_] = trim($IP_[%ip_] TAB %cl.name);
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

		%ip_ = strReplace(%ip, ".", "_");
		if (getFieldCount($IP_[%ip_]) > 1 && !%ip_checked[%ip])
		{
			%ret = %ret TAB %ip @ ": " @ strReplace($IP_[%ip_], "\t", ", ") @ " on same ip";
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

function serverCmdCleanIPLogs(%cl)
{
	if (!%cl.isSuperAdmin) return;
	
	if (!%cl.cleanIPsRepeat)
	{
		%cl.cleanIPsRepeat = 1;
		messageClient(%cl, '', "Are you sure you want to clean up IP logger entries? This may cause serious server lag!");
		messageClient(%cl, '', "Additionally, it will delete ALL IP logs! Repeat /cleanIPLogs to proceed.");
		%cl.cleanIPsRepeatSched = schedule(5000, 0, unSetIPCleanRepeat, %cl);
		return;
	}
	cancel(%cl.cleanIPsRepeatSched);
	%cl.cleanIPsRepeat = 0;

	deleteVariables("$Pref::IPLogger::BLID_*");
	messageClient(%cl, 'MsgAdminForce', "IP logs cleaned up.");
}

function unSetIPCleanRepeat(%cl)
{
	%cl.cleanIPsRepeat = 0;
	messageClient(%cl, '', "IP cleanup timed out - no IP logs cleared.");
}
