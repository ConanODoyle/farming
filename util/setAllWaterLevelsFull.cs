function setAllWaterLevelsFull()
{
	for (%i = 0; %i < MainBrickgroup.getCount(); %i++)
	{
		%group = MainBrickgroup.getObject(%i);

		for (%j = 0; %j < %group.getCount(); %j++)
		{
			%brick = %group.getObject(%j);
			if (%brick.getDatablock().isWaterTank || %brick.getDatablock().isDirt)
			{
				%brick.setWaterLevel(100000);
			}
		}
	}
}