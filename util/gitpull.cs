function serverCmdGitPull(%cl)
{
	if (!%cl.isSuperAdmin)
	{
		return;
	}
	talk("\c3" @ %cl.name @ " - Starting git pull...");
	if (isObject($gitPullTCP))
	{
		$gitPullTCP.delete();
	}
	$gitPullTCP = TCPClient("GET", "127.0.0.1", "28011", "/pullfarming", %query);
}

function TCPGitPullObj::onLine(%this, %line)
{
	if ($debugGitPullTCP)
	{
		talk("line " @ %this.linecount + 0 @ ": " @ %line);
	}
	%this.linecount++;
}

function TCPGitPullObj::onDone(%this, %error)
{
	if (%error)
	{
		talk("gitpull: Error in connecting to server!");
		return;
	}
	else
	{
		setModPaths(getModPaths());
		schedule(1000, 0, talk, "\c3Git pull complete!");
	}
}