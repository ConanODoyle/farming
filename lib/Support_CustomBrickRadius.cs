package CustomBrickRadius
{
	function fxDTSBrick::plant(%this)
	{
		%ret = parent::plant(%this);

		if (%ret == 0 && %this.getDatablock().customRadius !$= "")
		{
			%close = getCustomRadiusClosestBrick(%this);
			if (isObject(%close))
			{
				return 1;
			}
		}
		return %ret;
	}
};
activatePackage(CustomBrickRadius);

function getCustomRadiusClosestBrick(%brick)
{
	%db = %brick.getDatablock();
	%customRadius = %db.customRadius;
	%customRadiusHeightCheck = %db.customRadiusHeightCheck;
	%width = %db.brickSizeX * 0.5;
	%length = %db.brickSizeY * 0.5;
	%height = (%customRadiusHeightCheck <= 0 ? (%db.brickSizeZ * 0.2) : %customRadiusHeightCheck);

	if (%brick.angleID % 2 == 1)
	{
		%temp = %width;
		%width = %length;
		%length = %temp;
	}

	%box = (%width + %customRadius) SPC (%length + %customRadius) SPC %height;

	initContainerBoxSearch(%brick.getPosition(), %box, $Typemasks::fxBrickAlwaysObjectType);
	while (isObject(%next = containerSearchNext()))
	{
		if (%next.getDatablock() == %db && %next != %brick)
		{
			return %next;
		}
	}
	return 0;
}

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