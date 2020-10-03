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

		if (%hasWateringCat && getRandom() < 0.15 && strPos(%msg, ":0") < 0)
		{
			%msg = %msg SPC ":0";
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

		if (%hasWateringCat && getRandom() < 0.15 && strPos(%msg, ":V") < 0)
		{
			%msg = %msg SPC ":V";
		}

		return parent::serverCmdMessageSent(%cl, %msg);
	}
};
activatePackage(wateringSnake);
