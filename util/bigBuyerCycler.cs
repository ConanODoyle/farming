
if (!isObject(BigBuyerMover))
{
	new SimSet(BigBuyerMover);
}

package BigBuyerMoving
{
	function AIPlayer::randomBuyerLoop(%bot, %selectionCount, %speak, %timeRange, %shopObjects)
	{
		if (%bot.nextDealTime < $Sim::Time)
		{
			%bot.dealChange++;
			for (%i = 0; %i < %bot.dealChange - 5; %i++)
			{
				if (getRandom() > 0.1)
				{
					continue;
				}

				if (moveBuyer(%bot))
				{
					return;
				}
			}
		}
		return parent::randomBuyerLoop(%bot, %selectionCount, %speak, %timeRange, %shopObjects);
	}
};
activatePackage(BigBuyerMoving);

function moveBuyer(%bot)
{
	%currSpawn = %bot.spawnBrick;
	if (!isObject(%currSpawn))
	{
		messageAll('', "No spawn brick found for " @ %bot @ "! Report this issue to Conan");
		return 0;
	}

	while (%safety++ < 10)
	{
		%pick = BigBuyerMover.getObject(getRandom(BigBuyerMover.getCount() - 1));
		if (!isObject(%pick.hBot))
		{
			%pick.setHSpawnClose(0, 10000);
			%currSpawn.setHSpawnClose(1, 0);
			return 1;
		}
	}
	return 0;
}

function findAllBuyerSpawns()
{
    for (%i = 0; %i < Brickgroup_888888.getCount(); %i++)
    {
        %b = Brickgroup_888888.getObject(%i);
        %name = %b.getName();
        if (striPos(%name, "_bigbuyer") == 0 && !BigBuyerMover.isMember(%b))
        {
            BigBuyerMover.add(%b);
			messageAll('', "    Found big buyer spawn " @ %b);
            %b.setHSpawnClose(1, 0);
        }
    }

    //spawn 2
    if (BigBuyerMover.getCount() < 2)
    {
    	return;
    }

    for (%i = 0; %i < 2; %i++)
    {
	    %pick = BigBuyerMover.getObject(getRandom(BigBuyerMover.getCount() - 1));
		if (isObject(%pick.hBot))
		{
			%i--;
			if (%safety++ > 10)
			{
				break;
			}
			continue;
		}
		messageAll('', "Spawning big buyer on " @ %pick);
	    %pick.setHSpawnClose(0, 10000);
    }
}