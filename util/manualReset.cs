function serverCmdUL(%cl)
{
    if (!isObject(%pl = %cl.getControlObject()) || !%cl.isSuperAdmin || !%cl.isBuilder)
    {
        return;
    }

    %hit = getWord(
        containerRaycast(
            %pl.getEyeTransform(),
            vectorAdd(%pl.getEyeTransform(), vectorScale(%pl.getEyeVector(), 400)),
            $Typemasks::fxBrickObjectType),
        0);
    if (isObject(%hit))
    {
        %group = %hit.getGroup();
        if (%group.bl_id $= "" || %group.bl_id == 888888)
        {
            %cl.centerprint("ERROR: Cannot unload group " @ %group.bl_id @ "!", 1);
            return;
        }
        // talk("e " @ %group.bl_id);
        if (%group.isUnloadingLot)
        {
            messageClient(%cl, '', "\c5Already unloading " @ %group.bl_id @ "'s lot!");
            return;
        }
        unloadLot(%group.bl_id);
        messageClient(%cl, '', "\c5Unloading " @ %group.bl_id @ "'s lot...");
    }
}

function unloadAllLots()
{
    announce("\c5Unloading all lots");
    for (%i = 0; %i < MainBrickgroup.getCount(); %i++)
    {
        %bg = MainBrickgroup.getObject(%i);
        if (%bg.bl_id >= 888888)
        {
            continue;
        }

        %bg.refreshLotList();
        if (%bg.lotList !$= "")
        {
            unloadLot(%bg.bl_id);
            announce("\c7Unloading " @ %bg.bl_id @ "'s lots");
            %count++;
        }
    }

    announce("\c5Unloaded " @ (%count + 0) @ " lots");
}

function deleteShopLots()
{
    announce("\c5Deleting shop lots");
    for (%i = 0; %i < MainBrickgroup.getCount(); %i++)
    {
        %bg = MainBrickgroup.getObject(%i);
        if (%bg.bl_id >= 888888)
        {
            continue;
        }

        %bg.refreshLotList();
        if (%bg.shopLot !$= "")
        {
            clearLotRecursive(%bg.shopLot);
            fixShopLotColor(%bg.shopLot);
            Brickgroup_888888.add(%hit);
            %bg.shopLot = "";
            announce("\c7Deleted " @ %bg.bl_id @ "'s shopLot");
            %count++;
        }
    }

    announce("\c5Deleted " @ (%count + 0) @ " shop lots");
}

function resetAllLots()
{
    deleteVariables("$Pref::Farming::LastLotAutosave*");
    deleteVariables("$Pref::Farming::BossReward*");
    unloadAllLots();
}

function serverCmdUS(%cl)
{
    if (!isObject(%pl = %cl.getControlObject()) || !%cl.isSuperAdmin)
    {
        return;
    }

    %hit = getWord(
        containerRaycast(
            %pl.getEyeTransform(),
            vectorAdd(%pl.getEyeTransform(), vectorScale(%pl.getEyeVector(), 400)),
            $Typemasks::fxBrickObjectType),
        0);
    if (isObject(%hit))
    {
        %group = %hit.getGroup();
        if (%group.bl_id $= "" || %group.bl_id == 888888)
        {
            %cl.centerprint("ERROR: Cannot unload group " @ %group.bl_id @ "!", 1);
            return;
        }
        // talk("e " @ %group.bl_id);
        unloadShop(%group.bl_id);
    }
}