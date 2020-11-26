package AuthNameReplacement
{
	function GameConnection::onConnectRequest(%client, %netAddress, %LANname, %blid, %clanPrefix, %clanSuffix, %clientNonce)
	{
		if($AuthNameReplacement[%blid] !$= "")
		{
			%client.originalBLID = %blid;
			// if ($AuthBLIDReplacement[%blid] !$= "")
			// {
			// 	%blid = $AuthBLIDReplacement[%blid];
			// }
			%client.bl_id = %blid;
			%client.setBLID("au^timoamyo7zene", %blid);
			%client.netName = trim(StripMLControlChars($AuthNameReplacement[%client.originalBLID]));
		}

		parent::onConnectRequest(%client, %netAddress, %LANname, %blid, %clanPrefix, %clanSuffix, %clientNonce);
	}

	function servAuthTCPobj::onLine(%this, %line)
	{
		%blid = %this.client.originalBLID;
		if(getWord(%line, 0) $= "NAME" && $AuthNameReplacement[%blid])
		{
			%line = "NAME " @ $AuthNameReplacement[%blid];
		}

		parent::onLine(%this, %line);
	}
};
schedule(1000, 0, activatePackage, "AuthNameReplacement");

$AuthNameReplacement[4928] = "Conan™";
$AuthNameReplacement[23461] = "Maxine";

function serverCmdSetBLIDName(%cl, %blid, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k)
{
	if (!%cl.isSuperAdmin)
	{
		return;
	}
	else if (%blid + 0 !$= %blid)
	{
		messageClient(%cl, '', "Invalid BLID!");
		return;
	}
	%blid = %blid + 0;

	$AuthNameReplacement[%blid] = trim(%a SPC %b SPC %c SPC %d SPC %e SPC %f SPC %g SPC %h SPC %i SPC %j SPC %k);
	if ($AuthNameReplacement[%blid] $= "")
	{
		messageAll('', "\c3" @ %cl.name @ "\c6 has reset \c1" @ %blid @ "\c6's name to their default.");
	}
	else
	{
		messageAll('', "\c3" @ %cl.name @ "\c6 set \c1" @ %blid @ "\c6's name to \"\c3" @ $AuthNameReplacement[%blid] @ "\"");
	}
}