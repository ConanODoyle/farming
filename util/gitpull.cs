function serverCmdGitPull(%cl)
{
	if (!%cl.isSuperAdmin)
	{
		return;
	}
	messageAll('', "Starting git pull...");
	if (isObject($gitPullTCP))
	{
		$gitPullTCP.delete();
	}
	$gitPullTCP = TCPClient("GET", "155.138.204.83", "80", "pullfarming", "", "TCPGitPullObj", "");
}

function TCPGitPullObj::onLine(%this, %line)
{
	talk("line " @ %this.linecount + 0 @ ": " @ %line);
	%this.linecount++;
}