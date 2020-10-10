function fxDTSBrick::setWaterLevel(%b, %amt)
{
	if (!%b.getDatablock().isDirt && !%b.getDatablock().isWaterTank)
	{
		return;
	}

	%max = %b.getDatablock().maxWater;
	if (%amt !$= "" && %amt >= 0)
	{
		%b.waterLevel = mCeil(getMin(getMax(%amt, 0), %max));
	}

	if (%b.getDatablock().isDirt)
	{
		%step = %max / $DirtWaterColorCount;
		// talk("Step: " @ %step);

		%dist = 10000;
		for (%i = 0; %i < $DirtWaterColorCount; %i++)
		{
			%curr = %i * %step;
			%currDist = mAbs(%b.waterLevel - %curr);
			if (%currDist < %dist)
			{
				%best = %i;
				%dist = %currDist;
			}
		}
		%b.setWaterColor = 1;
		%b.setColor($DirtWaterColor[%best + 0]);
		%b.setWaterColor = 0;
	}
}

package DirtWaterColor
{
	function ndTrustCheckSelect(%brick, %group, %bl_id, %admin)
	{
		if ((%brick.getDatablock().maxWater > 0 || %brick.getDatablock().isSprinkler) && !findClientByBL_ID(%bl_id).isBuilder)
		{
			return false;
		}
		return parent::ndTrustCheckSelect(%brick, %group, %bl_id, %admin);
	}
	function fxDTSBrick::onAdd(%obj)
	{
		if (%obj.getDatablock().isDirt)
		{
			%obj.schedule(33, setWaterLevel, "");
		}
		return parent::onAdd(%obj);
	}

	function fxDTSBrick::setColor(%obj, %color)
	{
		if (%obj.getDatablock().isDirt && !%obj.setWaterColor)
		{
			%obj.setWaterLevel("");
			return;
		}
		parent::setColor(%obj, %color);
	}
};
activatePackage(DirtWaterColor);
