function serverCmdUL(%cl)
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
        unloadLot(%group.bl_id);
    }
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