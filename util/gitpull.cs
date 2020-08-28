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
	$gitPullTCP = connectToURL("155.138.204.83/pullfarming", "GET", "", "TCPGitPullObj", "");
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