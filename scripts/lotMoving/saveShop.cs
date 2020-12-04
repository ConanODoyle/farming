function farmingSaveShop(%bl_id, %delete)
{
	%brickGroup = "BrickGroup_" @ %bl_id;

	if (!isObject(%brickGroup))
	{
		error("ERROR: farmingSaveShop - Client has no brickgroup! " @ %client);
		echo("ERROR: farmingSaveShop - Client has no brickgroup! " @ %client);
		return -1;
	}

	%lot = %brickGroup.shopLot;

	if (!isObject(%lot))
	{
		error("ERROR: farmingSaveShop - Client's brickgroup doesn't have a shop lot!");
		echo("ERROR: farmingSaveShop - Client's brickgroup doesn't have a shop lot!");
		return -1;
	}

	%brickGroup.isSaveClearingLot = true;

	%file = farmingSaveInitFile(%bl_id, "Shop");

	$Farming::Temp::Origin[%file] = %lot.position;
	$Farming::Temp::BrickQueue[%file] = new SimSet();
	$Farming::Temp::BrickSet[%file] = new SimSet();
	farmingSaveGatherBricks(%file, "Shop", %lot, %delete);
}