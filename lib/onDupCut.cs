package onDupCut
{
	function ND_Selection::tickCutting(%this)
	{
		cancel(%this.cutSchedule);

		//Get bounds for this tick
		%start = %this.cutIndex;
		%end = %start + $Pref::Server::ND::ProcessPerTick;

		if(%end > %this.brickCount)
			%end = %this.brickCount;

		%cutSuccessCount = %this.cutSuccessCount;
		%cutFailCount = %this.cutFailCount;

		%admin = %this.client.isAdmin;
		%group = %this.client.brickGroup.getId();
		%bl_id = %this.client.bl_id;

		//Cut bricks
		for(%i = %start; %i < %end; %i++)
		{
			%brick = $NS[%this, "B", %i];

			if(!isObject(%brick))
				continue;

			if(!ndTrustCheckModify(%brick, %group, %bl_id, %admin))
			{
				%cutFailCount++;
				continue;
			}

			// Support for hole bots
			if(isObject(%brick.hBot))
			{
				%brick.hBot.spawnProjectile("audio2d", "deathProjectile", "0 0 0", 1);
				%brick.hBot.delete();
			}

			//START OF CHANGES
			fxDTSBrick::onDupCut(%brick);
			//END OF CHANGES
			%brick.delete();
			%cutSuccessCount++;
		}

		//Save how far we got
		%this.cutIndex = %i;

		%this.cutSuccessCount = %cutSuccessCount;
		%this.cutFailCount = %cutFailCount;

		//Tell the client how much we cut this tick
		if(%this.client.ndLastMessageTime + 0.1 < $Sim::Time)
		{
			%this.client.ndUpdateBottomPrint();
			%this.client.ndLastMessageTime = $Sim::Time;
		}

		if(%i >= %this.brickCount)
			%this.finishCutting();
		else
			%this.cutSchedule = %this.schedule(30, tickCutting);
	}
};
activatePackage(onDupCut);

function fxDTSBrick::onDupCut(%this)
{
	return;
}