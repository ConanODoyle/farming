//building & brickgroup
$Farming::ShopLotPrice = "150 1000";

datablock fxDTSBrickData(brick16x16ShopLotData : brick16x16fData)
{
	category = "Baseplates";
	subcategory = "Lots";
	uiName = "16x16 Shop Lot";

	cost = -1;
	isShopLot = 1;
};

function fixShopLotColor(%brick)
{
	%xEven = mAbs(mFloor(getWord(%brick.getPosition(), 0) / 8)) % 2;
	%yEven = mAbs(mFloor(getWord(%brick.getPosition(), 1) / 8)) % 2;

	if (%xEven == %yEven)
	{
		%brick.setColor(57);
	}
	else
	{
		%brick.setColor(58);
	}
	%brick.setShapeFX(0);
	%brick.setColorFX(0);
}

function fxDTSBrick::updateShopSet(%this, %removed)
{
	if (!isObject($ShopSet))
	{
		$ShopSet = new SimSet();
	}

	if (!%this.getDatablock().isShopLot)
	{
		error("ERROR: fxDTSBrick::updateShopSet - attempted to " @ (%removed ? "remove non-shop lot from" : "add non-shop lot to") @ " shop set!");
		return;
	}

	if (%removed)
	{
		if (!$ShopSet.isMember(%this))
		{
			error("ERROR: fxDTSBrick::updateShopSet - brick does not exist in shop set!");
			talk("ERROR: fxDTSBrick::updateShopSet - brick does not exist in shop set!");
			return;
		}

		$ShopSet.remove(%this);
	}
	else
	{
		$ShopSet.add(%this);
	}

	%bg = getBrickgroupFromObject(%this);
	if (%bg.getName() $= "BrickGroup_888888")
	{
		fixShopLotColor(%this);
	}
}

package shopLotBuild
{
	function serverCmdClearBricks(%cl)
	{
		if (isObject(%cl.brickgroup.shopLot))
		{
			messageClient(%cl, '', "Cannot clear bricks - sell your shop first!");
			return;
		}
		return parent::serverCmdClearBricks(%cl);
	}

	function fxDTSBrick::onAdd(%this)
	{
		if (%this.isPlanted && %this.getDatablock().isShopLot)
		{
			if (%this.getGroup() != BrickGroup_888888)
			{
				%this.getGroup().shopLot = %this;
			}

			%this.schedule(100, updateShopSet, 0);
		}

		return parent::onAdd(%this);
	}

	function fxDTSBrick::onRemove(%this)
	{
		if (%this.isPlanted && %this.getDatablock().isShopLot)
		{
			if (%this.getGroup() != BrickGroup_888888)
			{
				%this.getGroup().shopLot = "";
			}

			%this.updateShopSet(%this, 1);
		}
		return parent::onRemove(%this);
	}

	function ndTrustCheckSelect(%brick, %group, %bl_id, %admin)
	{
		if (%brick.getDatablock().isShopLot && !findClientByBL_ID(%bl_id).isBuilder)
		{
			return 0;
		}

		return parent::ndTrustCheckSelect(%brick, %group, %bl_id, %admin);
	}

	function getTrustLevel(%obj1, %obj2)
	{
		if ((%obj1.getClassName() $= "fxDTSBrick" && %obj1.getDatablock().isShopLot)
			|| (%obj2.getClassName() $= "fxDTSBrick" && %obj2.getDatablock().isShopLot))
		{
			if (%obj1.getClassName() $= "Player" || %obj2.getClassName() $= "Player")
			{
				return 0;
			}
		}
		return parent::getTrustLevel(%obj1, %obj2);
	}

	function hammerImage::onHitObject(%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
	{
		if (%hitObj.getDatablock().isShopLot && !%hitObj.willCauseChainKill() && isObject(%player.client))
		{
			%player.client.centerprint("You cannot hammer lot bricks!", 3);
			return;
		}
		return parent::onHitObject(%this, %player, %slot, %hitObj, %hitPos, %hitNormal);
	}
};
schedule(1000, 0, activatePackage, shopLotBuild);











//purchasing

function serverCmdLoadShopLot(%cl, %rotation)
{
	serverCmdLoadShop(%cl, %rotation);
}

function serverCmdLoadShop(%cl, %rotation)
{
	%load = hasLoadedShopLot(%cl.bl_id);
	if (%load $= "noSavedLot")
	{
		messageClient(%cl, '', "You don't have a shop, or a shop saved! Use /buyShop to buy a shop.");
		return;
	}
	else if (%load == 1)
	{
		messageClient(%cl, '', "Your shop is already loaded! You can unload your shop for free at any market.");
		return;
	}
	serverCmdBuyShop(%cl, %rotation);
}

function serverCmdBuyShopLot(%cl, %rotation)
{
	serverCmdBuyShop(%cl, %rotation);
}

function serverCmdBuyShop(%cl, %rotation)
{
	if (!isObject(%pl = %cl.player))
	{
		messageClient(%cl, '', "You are dead!");
		return;
	}

	%rotation = %rotation | 0;

	%start = %cl.player.getHackPosition();
	%end = vectorAdd(%start, "0 0 -10");
	%ray = containerRaycast(%start, %end, $TypeMasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	%costMoney = getWord($Farming::ShopLotPrice, 0);
	%costExp = getWord($Farming::ShopLotPrice, 1);
	%costString = "$" @ %costMoney @ (%costExp > 0 ? ", " @ %costExp @ " EXP" : "");

	if (!%hit.getDatablock().isShopLot)
	{
		messageClient(%cl, '', "You have to be standing on a shop lot!");
		return;
	}

	if (%hit.getGroup() == %cl.brickGroup)
	{
		messageClient(%cl, '', "You already own this shop lot!");
		return;
	}

	if (%hit.getGroup().getName() !$= "BrickGroup_888888")
	{
		messageClient(%cl, '', "This lot is claimed by " @ %hit.getGroup().name @ "\c0!");
		return;
	}

	if (!%cl.checkMoney(%costMoney) || %cl.farmingExperience < %costExp)
	{
		messageClient(%cl, '', "You cannot afford a shop lot! (Cost: " @ %costString @ ")");
		return;
	}

	if (hasLoadedShop(%cl.bl_id))
	{
		messageClient(%cl, '', "You cannot buy more than one shop lot!");
		return;
	}

	if (%cl.repeatBuyShopLot != %hit)
	{
		if (hasSavedShop(%cl.bl_id) && !hasLoadedShop(%cl.bl_id))
		{
			messageClient(%cl, '', "\c5Are you sure you want to load your shop here? Repeat /loadShop to confirm.");
			if (%rotation $= "")
			{
				messageClient(%cl, '', "\c5You can rotate your lot by any number of 90 degree counterclockwise turns with /loadShop [rotation].");
			}
		}
		else
		{
			messageClient(%cl, '', "\c5This lot will cost you \c3" @ %costString @ "\c5. Repeat /buyShop to confirm.");
		}
		%cl.repeatBuyShopLot = %hit;
		cancel(%cl.clearRepeatBuyShopLotSched);
		%cl.clearRepeatBuyShopLotSched = schedule(5000, %cl, eval, %cl @ ".repeatBuyShopLot = 0;");
		return;
	}

	if (hasSavedShop(%cl.bl_id) && !hasLoadedShop(%cl.bl_id))
	{
		loadShop(%cl.bl_id, %hit, %rotation);
		%cl.repeatBuyShopLot = 0;
		cancel(%cl.clearRepeatBuyShopLotSched);
		return;
	}

	%cl.subMoney(%costMoney);
	%cl.addExperience(-1 * %costExp);
	clearLotRecursive(%hit, %cl);
	%cl.brickGroup.add(%hit);
	%cl.brickGroup.shopLot = %hit;
	%hit.setTrusted(1);
	%cl.repeatBuyLot = 0;

	messageClient(%cl, '', "\c5You bought a shop lot for \c0" @ %costString @ "\c5!");
}

function serverCmdRotateShopLot(%cl, %rotation)
{
	serverCmdRotateShop(%cl, %rotation);
}

function serverCmdRotateShop(%cl, %rotation)
{
	if ($nextShopRotation[%cl.bl_id] > $Sim::Time)
	{
		messageClient(%cl, '', "You need to wait " @ convTime($nextLotRotation[%cl.bl_id]) @ " before rotating your shop again.");
		return;
	}

	if (%rotation == 0)
	{
		messageClient(%cl, '', "Please provide a number to rotate your shop.");
		messageClient(%cl, '', "Put in a number to rotate your shop by that many 90 degree increments, counterclockwise.");
		return;
	}

	if (%cl.repeatRotateShopLot != %rotation)
	{
		if (hasLoadedShop(%cl.bl_id))
		{
			messageClient(%cl, '', "\c5Are you sure you want to rotate your shop by " @ mAbs(%rotation) * 90 @ " degrees " @ ((%rotation >= 0) ? "counter" : "") @ "clockwise?");
			messageClient(%cl, '', "\c5This will cost \c3$" @ $Farming::LotRotatePrice @ "\c5. Type /rotateShop " @ %rotation @ " again to confirm.");
			%cl.repeatRotateShopLot = %rotation;
			%cl.clearRepeatRotateShopLotSched = schedule(5000, %cl, eval, %cl @ ".repeatRotateShopLot = \"\";");
			return;
		}
		else
		{
			messageClient(%cl, '', "You don't currently have a shop to rotate!");
			if (hasSavedShopLot(%cl.bl_id))
			{
				messageClient(%cl, '', "However, you have a saved shop. If you want to load and rotate this shop, use /loadShop [rotation].");
				messageClient(%cl, '', "Put in a number to rotate your shop by that many 90 degree increments, counterclockwise.");
			}
			else
			{
				messageClient(%cl, '', "You can buy an unclaimed shop lot with /buyShop.");
				messageClient(%cl, '', "Once you've claimed a shop lot, you can use this function to rotate your shop by 90 degree counterclockwise increments.");
			}
			return;
		}
	}
	else
	{
		%lot = getLoadedShop(%cl.bl_id);
		$Farming::ReloadShop[%cl.bl_id] = %lot SPC %rotation;
		%unloadResult = unloadShop(%cl.bl_id);

		if (%unloadResult == -1)
		{
			messageClient(%cl, '', "Please wait for a few moments. Either the autosaver is currently running or another player is unloading their shop.");
			deleteVariables("Farming::ReloadShop" @ %cl.bl_id);
			return;
		}
		else if (%unloadResult == -2)
		{
			messageClient(%cl, '', "Something went wrong. Your shop lot brick no longer exists. Please inform an admin.");
			deleteVariables("Farming::ReloadShop" @ %cl.bl_id);
			return;
		}

		messageClient(%cl, '', "\c5You have rotated your shop for \c0$" @ $Farming::LotRotatePrice @ "\c5. Please wait while your shop reloads.");
		%cl.subMoney($Farming::LotRotatePrice);
		%cl.repeatRotateShopLot = "";
		cancel(%cl.clearRepeatRotateShopLotSched);
		return;
	}
}

function serverCmdSellShopLot(%cl, %force)
{
	serverCmdSellShop(%cl, %force);
}

function serverCmdSellShop(%cl, %force)
{
	if (!isObject(%pl = %cl.player))
	{
		messageClient(%cl, '', "You are dead!");
		return;
	}

	if (!%cl.isSuperAdmin)
	{
		%force = 0;
	}

	%start = %pl.getHackPosition();
	%end = vectorAdd(%start, "0 0 -10");
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%hit = getWord(%ray, 0);

	if (!isObject(%hit) || !%hit.getDatablock().isShopLot)
	{
		messageClient(%cl, '', "You have to be standing on a shop lot!");
		return;
	}
	else if (%hit.getGroup() != %cl.brickGroup && !%force)
	{
		messageClient(%cl, '', "You do not own this shop lot!");
		return;
	}
	else if (%cl.repeatSellShopLot != %hit)
	{
		%costMoney = getWord($Farming::ShopLotPrice, 0);
		%costExp = getWord($Farming::ShopLotPrice, 1);
		%costString = "$" @ %costMoney @ (%costExp > 0 ? ", " @ %costExp @ " EXP" : "");
		%cl.sellPrice = %costMoney;

		if (!%force)
		{
			messageClient(%cl, '', "\c5Are you sure you want to sell this shop lot? Any bricks above it will be removed. Repeat this command to confirm.");
			getSellPriceSingleWrapper(%hit, %cl, %costExp);
		}
		else
			messageClient(%cl, '', "\c5Are you sure you want to force sell this shop lot? Any bricks above it will be removed. Type /sellShop 1 to confirm.");
		%cl.repeatSellShopLot = %hit;
		cancel(%cl.clearRepeatSellShopLotSched);
		%cl.clearRepeatSellShopLotSched = schedule(5000, %cl, eval, %cl @ ".repeatSellShopLot = 0;");
		return;
	}

	cancel(%cl.clearRepeatSellShopLotSched);

	%costMoney = getWord($Farming::ShopLotPrice, 0);
	%costExp = getWord($Farming::ShopLotPrice, 1);
	%costString = "$" @ %costMoney @ (%costExp > 0 ? ", " @ %costExp @ " EXP" : "");

	if (!%force)
	{
		%cl.addMoney(%costMoney);
		%cl.addExperience(%costExp);
	}

	%hit.getGroup().shopLot = "";
	BrickGroup_888888.add(%hit);
	clearLotRecursive(%hit, %cl);
	fixShopLotColor(%hit);
	// %cl.refundRatio = 0;
	%cl.repeatSellShopLot = 0;

	if (!%force)
	{
		messageClient(%cl, '', "\c5You sold a lot for \c2" @ %costString @ "\c5!");
	}
	else
	{
		%owner = getBrickgroupFromObject(%hit).name;
		messageClient(%cl, '', "\c5You \c0force\c5 sold \c3" @ %owner @ "\c5's lot!");
	}
}