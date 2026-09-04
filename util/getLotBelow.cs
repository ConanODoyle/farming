function getLotBelow(%start)
{
	%end = vectorSub(%start, "0 0 " @ ($maxLotBuildHeight - 0.2 * 27));

	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickAlwaysObjectType);

	while (isObject(%hit = getWord(%ray, 0)) && %safety++ < 100)
	{
		if (%hit.getDatablock().isLot || %hit.getDatablock().isShopLot)
		{
			%owner = getBrickgroupFromObject(%hit).name;
			%bl_id = getBrickgroupFromObject(%hit).bl_id;
			if (%hit.getDatablock().isSingle)
			{
				%prefix = "Center ";
			}

			if (%hit.getDatablock().isShopLot)
			{
				%prefix = "Shop ";
			}

			return %hit;
		}
		else if (%hit.getGroup().bl_id == 888888) //not a lot AND public brick, so we're done
		{
			return 0;
		}
		%ray = containerRaycast(vectorSub(getWords(%ray, 1, 3), "0 0 0.1"), %end, $Typemasks::fxBrickAlwaysObjectType, %hit);
	}
	return 0;
}