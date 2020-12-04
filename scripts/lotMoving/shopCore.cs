function hasSavedShop(%bl_id)
{
	if (!isFile($Pref::Farming::LastShopAutosave[%bl_id]))
	{
		return isFile($Pref::Server::AS_["Directory"] @ $Pref::Farming::LastShopAutosave[%bl_id] @ ".bls");
	}
	return true;
}

function getLoadedShop(%bl_id)
{
	%bg = "Brickgroup_" @ %bl_id;

	if (isObject(%bg) && isObject(%bg.shopLot))
	{
		return %bg.shopLot;
	}
	return 0;
}

function hasLoadedShop(%bl_id)
{
	%shopExists = isObject(getLoadedShop(%bl_id));

	if (hasSavedShop(%bl_id)) //has a lot save, return lot value
	{
		return %shopExists;
	}
	else if (!%shopExists) //no file, no lot at all
	{
		return "noSavedLot";
	}
	else
	{
		return 1;
	}
}

function unloadShop(%bl_id)
{
	if (hasLoadedShop(%bl_id) != 1)
	{
		talk("ERROR: unloadShop - BLID " @ %bl_id @ " does not have a lot loaded!");
		error("ERROR: unloadShop - BLID " @ %bl_id @ " does not have a lot loaded!");
		return -2;
	}

	%bg = "Brickgroup_" @ %bl_id;
	if (!isObject(%bg))
	{
		talk("ERROR: unloadShop - no brickgroup with BLID " @ %bl_id @ " exists!");
		error("ERROR: unloadShop - no brickgroup with BLID " @ %bl_id @ " exists!");
		return;
	}

	if (farmingSaveShop(%bl_id, true) == -1)
	{
		return -1;
	}
}

function loadShop(%bl_id, %lot, %rotation)
{
	if (!isObject(%lot) || !%lot.getDatablock().isShopLot)
	{
		talk("ERROR: loadShop - invalid lot provided! " @ %lot);
		error("ERROR: loadShop - invalid lot provided! " @ %lot);
		return -1;
	}

	if (%lot.getGroup().bl_id != 888888)
	{
		error("ERROR: loadShop - lot is not public! " @ %lot);
		return -1;
	}

	%dataObj = new ScriptObject(ShopLoadingDataObj);
	%dataObj.pos0 = %lot.getPosition();
	%dataObj.count = 1;
	%lot.delete();

	farmingLoadLastAutosave(%bl_id, "Shop", %dataObj, %rotation);
}
