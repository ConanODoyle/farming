datablock ItemData(ShippingPackageItem)
{
    category = "Item";
    className = "Item";

    shapeFile = "./resources/Package.dts";
    rotate = false;
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    uiName = "Package Slip";
    iconName = "";
    doColorShift = true;
    colorShiftColor = "0.71 0.56 0.38 1";

    image = ShippingPackageImage;
    canDrop = true;

    hasDataID = true;
    canPickupMultiple = true;
};

datablock ShapeBaseImageData(ShippingPackageImage)
{
    shapeFile = "./resources/Package.dts";
    emap = true;

    mountPoint = 0;
    offset = "-0.56 0.1 -0.155";

    item = ShippingPackageItem;

    armReady = true;

    doColorShift = ShippingPackageItem.doColorShift;
    colorShiftColor = ShippingPackageItem.colorShiftColor;

    toolTip = "Click to open package";

    stateName[0] = "init";
    stateTimeoutValue[0] = 0.15;
    stateTransitionOnTimeout[0] = "displayPackage";
    stateScript[0] = "displayPackages";

    stateName[1] = "displayPackage";
    stateTimeoutValue[1] = 1;
    stateWaitForTimeout[1] = false;
    stateTransitionOnTimeout[1] = "delay";
    stateTransitionOnTriggerDown[1] = "buffer";
    stateScript[1] = "displayPackage";

    stateName[2] = "delay";
    stateTimeoutValue[2] = 1;
    stateWaitForTimeout[2] = false;
    stateTransitionOnTimeout[2] = "displayPackage";
    stateTransitionOnTriggerDown[2] = "buffer";

    stateName[3] = "buffer";
    stateWaitForTimeout[3] = false;
    stateTransitionOnTriggerUp[3] = "open";

    stateName[4] = "open";
    stateTimeoutValue[4] = 0.15;
    stateScript[4] = "openPackage";
};

function ShippingPackageImage::onMount(%image, %player, %slot)
{
    %player.playThread(1, armReadyBoth);   
}

function ShippingPackageImage::onUnMount(%image, %player, %slot)
{
    %client = %player.client;
    if (isObject(%client))
    {
        %client.centerPrint("");
    }
}

function ShippingPackageImage::displayPackage(%image, %player, %slot)
{
    %client = %player.client;
    %itemSlot = %player.currTool;

    if (%client.getClassName() !$= "GameConnection")
    {
        return;
    }

    %client.displayPackage(%player.toolDataID[%itemSlot], %player.PackageDisplayMode);
}

function ShippingPackageImage::openPackage(%image, %player, %slot)
{
    %player.playThread(0, "plant");
    serverPlay3D(brickChangeSound, %player.getHackPosition());

    openPackage(%player.toolDataID[%player.currTool]);

    %player.tool[%player.currTool] = 0;
    messageClient(%player.client, 'MsgItemPickup', "", %player.currTool, 0);
    %player.unmountImage(%slot);
}

function GameConnection::displayQuest(%client, %packageID, %displayRewards) {
    if (!getDataIDArrayValue(%packageID, "isPackage")) {
        %client.centerPrint("<just:right>\c6The package is empty... ", 1);
        return;
    }

    %count = getDataIDArrayCount(%packageID);
    %displayString = "<just:right>\c3-Package- \n\c3";
    for (%i = 0; %i < %count; %i++) {
        %reward = getDataIDArrayValue(%packageID, %i);
        %item = getWord(%reward, 0);
        %count = getWord(%reward, 1);

        %displayString = %displayString @ %item.uiName @ "\c6: " @ %count @ " \n\c3";
    }

    %cashReward = getDataIDArrayTagValue(%packageID, "cashReward");
    if (%cashReward > 0) {
        %displayString = %displayString @ "Money\c6: $" @ mFloatLength(%cashReward, 2);
    }

    %client.centerPrint(trim(%displayString), "3");
}

function openPackage(%packageID, %player)
{
    if (!getDataIDArrayValue(%packageID, "isPackage")) {
        error("ERROR: Invalid packageID!");
        return false;
    }

    if (!isObject(%player)) {
        error("ERROR: No player to assign package items to!");
        return false;
    }

    %count = getDataIDArrayCount(%packageID);
    for (%i = 0; %i < %count; %i++) {
        %reward = getDataIDArrayValue(%packageID, %i);
        %item = getWord(%reward, 0);
        %count = getWord(%reward, 1);

        if (%item.isStackable) {
            %player.farmingAddStackableItem(%item, %count);
        } else {
            if (%item.hasDataID) //dataid item
            {
                %player.farmingAddItem(%item, %count); 
            }
            else //possibly multiple normal items
            {
                for (%j = 0; %j < %count; %j++) { 
                    %player.farmingAddItem(%item);
                }
            }
        }
    }

    %cashReward = getDataIDArrayTagValue(%packageID, "cashReward");
    %client.incScore(%cashReward);
    deleteDataIDArray(%packageID);
}

function createPackageArray(%packageID, %o0, %o1, %o2, %o3, %o4, %o5, %o6, %o7, %o8, %o9, %o10, %o11, %o12)
{
    if (%packageID $= "")
    {
        %packageID = "Package_" @ getRandomHash("package");
    }
    setDataIDArrayTagValue(%packageID, "isPackage", 1);

    %count = 0;
    for (%i = 0; %i < 13; %i++)
    {
        if (%o[%i] $= "")
        {
            continue;
        }
        else if ((%str = getWord(%o[%i], 0)) $= "cashReward" || %str $= "score")
        {
            setDataIDArrayTagValue(%packageID, "cashReward", getWord(%o[%i], 1));
        }
        else
        {
            setDataIDArrayValue(%packageID, %count, %o[%i]);
            %count++;
        }
    }

    return %packageID;
}

function addToPackageArray(%packageID, %o)
{
    if (%packageID $= "")
    {
        %packageID = "Package_" @ getRandomHash("package");
    }
    setDataIDArrayTagValue(%packageID, "isPackage", 1);

    %count = getDataIDArrayCount(%packageID);
    if ((%str = getWord(%o, 0)) $= "cashReward" || %str $= "score")
    {
        setDataIDArrayTagValue(%packageID, "cashReward", getWord(%o[%i], 1));
    }
    else
    {
        setDataIDArrayValue(%packageID, %count, %o[%i]);
        %count++;
    }

    return %packageID;
}

function createPackage(%packageID, %player, %pos)
{
    if (%pos $= "" && isObject(%player))
    {
        %pos = %player.getTransform();
    }

    %item = new Item() {
        dataBlock = ShippingPackageItem;
        bl_id = %player.client.bl_id;
        dataID = %packageID;
        miniGame = %player.client.minigame;
    };
    MissionCleanup.add(%item);
    %item.setCollisionTimeout(%player);
    %item.schedulePop();
    %item.setNodeColor("ALL", ShippingPackageItem.colorShiftColor);
    %item.setTransform(%pos);
    return %item;
}