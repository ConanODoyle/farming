package Support_StagedDoors
{
	function fxDTSBrick::doorOpen( %obj, %ccw, %client )
	{
		%data = %obj.getDataBlock();
		
		if( !%data.isDoor )
			return;
		
		if( %obj.lastDoorDataBlockSwitch + 95 > getSimTime() )
			return;
			
		// do rotation check, clockwise or not
		StagedDoors_nextStage(%obj, 1, %ccw);

		%obj.isCCW = %ccw;
		
		// if we're trying to set as the same datablock don't do anything
		if( %data.getName() $= %newDataBlock )
			return;
		
		// onDoorOpen event
		%obj.onDoorOpen( %client );
		
		// if( !%ccw )
			// %obj.setDoorDataBlock( %data.openCW );
		// else
		
		// set the datablock
		%obj.setDoorDataBlock( %newDataBlock );
	}

	function fxDTSBrick::doorClose( %obj, %client )
	{
		%data = %obj.getDataBlock();
		
		if( !%data.isDoor )
			return;
		
		if( %obj.lastDoorDataBlockSwitch + 95 > getSimTime() )
			return;
			
		// onDoorClose event
		%obj.onDoorClose( %client );
		
		StagedDoors_nextStage(%obj, -1, %obj.isCCW);
	}
};
activatePackage(Support_StagedDoors);

function StagedDoors_nextStage(%obj, %direction, %ccw)
{
	cancel(%obj.nextStageSchedule);

	%db = %obj.dataBlock;
	%ccwString = %ccw ? "CCW" : "CW";
	%index = %obj.stageIndex + %direction;
	%nextDB = %db.stage[%index, %ccw];
	%time = %db.stage[%index, %ccw, "Time"] > 95 ? %db.stage[%index, %ccw, "Time"] : 95;

	if (!isObject(%nextDB))
	{
		switch (%direction)
		{
			case -1:	%nextDB = %db.closed[%ccwString]; %index = 0;
			case 1:		%nextDB = %db.open[%ccwString];
		}
		%break = 1;
	}

	%obj.setDoorDataBlock(%nextDB);
	%obj.stageIndex = %index;

	if (!%break)
	{
		%obj.nextStageSchedule = schedule(%time, %obj, "StagedDoors_nextStage", %direction, %ccw);
	}
}