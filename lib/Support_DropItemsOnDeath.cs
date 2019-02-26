package Server_FarmingDropInventoryOnDeath
{
	function GameConnection::onDeath(%client, %source, %killer, %type, %location)
	{
		if(isObject(%client.player))
		{
			%pos = %client.player.getPosition();

			for(%i = 0; %i < %client.player.getDatablock().maxTools; %i++)
			{
				%item = %client.player.tool[%i];

				if (isObject(%item) && !%item.skipDrop)
				{
					serverCmdDropTool(%client, %i);
					%drop = $LastDroppedItem;
					%vel = vectorNormalize(getRandom() - 0.5 SPC getRandom() - 0.5);
					%drop.setVelocity(vectorAdd(vectorScale(%vel, 3), "0 0 5"));
				}
			}
		}
		return Parent::onDeath(%client, %source, %killer, %type, %location);
	}

	function ItemData::onAdd(%this, %obj)
	{
		$LastDroppedItem = %obj;
		return Parent::onAdd(%this, %obj);
	}
};
activatePackage(Server_FarmingDropInventoryOnDeath);

function setSkipDropValues()
{
	hammerItem.skipDrop = 1;
	wrenchItem.skipDrop = 1;
	printGun.skipDrop = 1;
}

schedule(100, 0, setSkipDropValues);