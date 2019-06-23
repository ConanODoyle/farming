package WeedLmao
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if ((%pos = strPos(strlwr(%msg), "weed")) >= 0)
		{
			if (strPos(strlwr(%msg), "dude") >= 0)
			{
				%chance = 0.2;
			}
			else
			{
				%chance = 0.05;
			}

			if (getRandom() < %chance)
			{
				%msg = getSubStr(%msg, 0, (%pos + 4)) SPC "lmao";
			}
		}

		parent::serverCmdMessageSent(%cl, %msg);
	}
};
schedule(20000, 0, activatePackage, WeedLmao);
