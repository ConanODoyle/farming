package wateringCat
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (isObject(%cl.player))
		{
			for (%i = 0; %i < %cl.player.getDatablock().maxTools; %i++)
			{
				if (%cl.player.tool[%i] == "WateringCatItem".getID())
				{
					%hasWateringCat = 1;
					break;
				}
			}
		}

		if (strPos(strLwr(%msg), "again") >= 0 && %cl.didCatMessage)
		{
			%chance = 1;
		}
		else
		{
			%chance = 0.15;
		}

		if (%hasWateringCat && getRandom() < %chance && strPos(%msg, ":0") < 0)
		{
			%msg = %msg SPC ":0";
			%cl.didCatMessage = 1;
		}
		else
		{
			%cl.didCatMessage = 0;
		}

		return parent::serverCmdMessageSent(%cl, %msg);
	}
};
activatePackage(wateringCat);

package wateringSnake
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (isObject(%cl.player))
		{
			for (%i = 0; %i < %cl.player.getDatablock().maxTools; %i++)
			{
				if (%cl.player.tool[%i] == "WateringSnakeItem".getID())
				{
					%hasWateringCat = 1;
					break;
				}
			}
		}

		if (strPos(strLwr(%msg), "again") >= 0 && %cl.didSnakeMessage)
		{
			%chance = 1;
		}
		else
		{
			%chance = 0.15;
		}

		if (%hasWateringCat && getRandom() < %chance && strPos(%msg, ":V") < 0)
		{
			%msg = %msg SPC ":V";
			%cl.didSnakeMessage = 1;
		}
		else
		{
			%cl.didSnakeMessage = 0;
		}

		return parent::serverCmdMessageSent(%cl, %msg);
	}
};
activatePackage(wateringSnake);
