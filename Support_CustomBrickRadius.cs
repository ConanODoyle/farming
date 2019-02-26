package CustomBrickRadius
{
	function fxDTSBrick::onAdd(%this)
	{
		%ret = parent::onAdd(%this);
		if (%this.isPlanted && %this.getDatablock().customRadius !$= "")
		{
			%db = %this.getDatablock();
			%customRadius = %db.customRadius;
			%width = %db.brickSizeX * 0.5;
			%length = %db.brickSizeY * 0.5;
			%height = %db.brickSizeZ * 0.2;

			if (%this.angleID % 2 == 1)
			{
				%temp = %width;
				%width = %height;
				%height = %temp;
			}

			%box = %width + %customRadius SPC %length + %customRadius SPC %height;

			initContainerBoxSearch(%this.getPosition(), %box, $Typemasks::fxBrickAlwaysObjectType);
			while (isObject(%next = containerSearchNext()))
			{
				if (%next.getDatablock() == %db && %next != %this)
				{
					schedule(0, 0, fixRemoveBrick, %this);
					%this.schedule(0, delete);
					return %ret;
				}
			}
		}
		return %ret;
	}
};
activatePackage(CustomBrickRadius);

function fixRemoveBrick(%b)
{
	%cl = getBrickgroupFromObject(%b).client;

	%stackNext = %cl.undoStack.pop();
	if (getWord(%stackNext, 0) != %b)
	{
		%cl.undoStack.push(%stackNext);
	}
	else
	{
		messageClient(%cl, 'MsgPlantError_Overlap');
	}
}