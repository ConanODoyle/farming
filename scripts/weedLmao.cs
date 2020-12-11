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

		if(strPos(%msg, "[INFO]") > -1)
		{
			if(isObject(%cl.player))
			{
				%player = %cl.player;
				%scale = %player.getScale();

				%z = getWord(%scale, 2) * 0.99;

				%player.setScale(getWords(%scale, 0, 1) SPC %z);
			}
		}

		parent::serverCmdMessageSent(%cl, %msg);
	}
};
schedule(20000, 0, activatePackage, WeedLmao);