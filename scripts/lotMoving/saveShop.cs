function farmingSaveShop(%bl_id, %delete)
{
	%bg = "brickGroup_" @ %bl_id;
	if (!isObject(%bg))
	{
		error("ERROR: farmingSaveShop - brickGroup doesn't exist! " @ %bg);
		return 0;
	}
	%collection = new SimSet();
	%queue = new SimSet();
	%visited = new SimSet();
	if (%delete)
	{
		%collection.callbackOnComplete = "postSaveClearLot";
	}

	%collection.bl_id = %bg.bl_id;
	%collection.brickGroup = %bg;
	%collection.lotList = %bg.shopLot;
	%collection.type = "Shop"; //required for farmingSaveWriteSave/farmingSaveInitFile
	%queue.lotList = %bg.shopLot;

	farmingSaveRecursiveCollectBricks(%collection, %queue, %visited);
}