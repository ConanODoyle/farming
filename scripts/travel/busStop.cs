
if (!isObject($BusStopSimSet))
{
    $BusStopSimSet = new SimSet(BusStopSimSet);
}

package BusStops
{
    function fxDTSBrick::setNTObjectName(%obj, %name)
    {
        if (strPos(strLwr(%name), "busstop") == 0 && %obj.getGroup().bl_id == 888888
            && !$BusStopSimSet.isMember(%obj))
        {
            $BusStopSimSet.add(%obj);
        }
        return parent::setNTObjectName(%obj, %name);
    }

    function GameConnection::exitCenterprintMenu(%cl)
    {
        if (%cl.centerprintMenu.isBusStopMenu)
        {
            %cl.setControlObject(%cl.player);
            %cl.camera.busStopObj = "";
            %cl.camera.setControlObject(%cl.camera);
        }
        return parent::exitCenterprintMenu(%cl);
    }
};
activatePackage(BusStops);

function findAllBusStops(%idx)
{
    cancel($busStopSearchSchedule);
    if (%idx $= "")
    {
        %idx = 0;
    }

    for (%i = %idx; %i < Brickgroup_888888.getCount(); %i++)
    {
        if (%count++ > 1024)
        {
            break;
        }
        %b = Brickgroup_888888.getObject(%i);
        %name = %b.getName();
        if (strPos(strLwr(%name), "_busstop") == 0 && !$BusStopSimSet.isMember(%b))
        {
            $BusStopSimSet.add(%b);
        }
    }

    if (%i > Brickgroup_888888.getCount())
    {
        return;
    }
    $busStopSearchSchedule = schedule(33, MissionCleanup, findAllBusStops, %i);
}

$busStopChance["ZUH"] = 0.005;

function configureBusStopCenterprintMenu(%menu, %brick)
{
    if ((%menu.lastConfiguredMenu + 20 | 0) > $Sim::Time)
    {
        return;
    }
    findAllBusStops();

    %menu.lastConfiguredMenu = $Sim::Time;
    %oldMenuCount = %menu.menuOptionCount;
    for (%i = 0; %i < %oldMenuCount; %i++)
    {
        %menu.menuOption[%i] = "";
        %menu.menuFunction[%i] = "";
        %menu.menuBrick[%i] = "";
    }
    %menu.isBusStopMenu = 1;

    %menuOptionCount = 0;
    for (%i = $BusStopSimSet.getCount() - 1; %i >= 0; %i--)
    {
        if (%i >= $BusStopSimSet.getCount())
        {
            continue;
        }

        %obj = $BusStopSimSet.getObject(%i);
        %name = %obj.getName();
        if (strPos(strLwr(%name), "_busstop") != 0)
        {
            $BusStopSimSet.remove(%obj);
            %i++;
            continue;
        }
        %name = getSubStr(%name, 8, 20);
        %name = strUpr(strReplace(%name, "_", " "));
        %originalName = %name;
        if ($busStopChance[%originalName] > 0 && getRandom() > $busStopChance[%originalName])
        {
            continue;
        }
        if (%obj == %brick || vectorDist(%obj.position, %brick.position) < 8)
        {
            %name = %name @ " - You are here";
            %here = 1;
        }
        else if (isObject(%brick))
        {
            %dist = mFloor(vectorDist(%brick.position, %obj.position) / 10) / 100;
            %name = %name @ " - " @ %dist @ "km";
        }

        %menu.menuOption[%menuOptionCount] = %name;
        if (%here)
        {
            %menu.menuFunction[%menuOptionCount] = "";
            %here = 0;
        }
        else
        {
            %menu.menuFunction[%menuOptionCount] = "goToBusStop";
        }
        %menu.menuBrick[%menuOptionCount] = %obj;
        %menu.stopName[%menuOptionCount] = %originalName;
        %menu.stopBrick[%menuOptionCount] = %obj;
        %menuOptionCount++;
    }

    %menu.menuOptionCount = %menuOptionCount;
    return %menuOptionCount;
}

function goToBusStop(%cl, %menu, %option)
{
    %brick = %menu.menuBrick[%option];
    %pl = %cl.player;

    if (!isObject(%pl) || !isObject(%brick))
    {
        return;
    }
    else if (%cl.score < 0.5)
    {
        messageClient(%cl, '', "You cannot afford to take the bus! You need $0.50 to ride.");
        return;
    }

    %target = %pl;
    while (isObject(%target.getObjectMount()) && %safety++ < 10)
    {
        %target = %target.getObjectMount();
        if (!(%target.getType() & $Typemasks::PlayerObjectType))
        {
            messageClient(%cl, '', "You cannot take the bus while riding a wheeled vehicle!");
            return;
        }
    }

    %cl.setScore(%cl.score - 0.5);
    %target.setTransform(%brick.getTransform());

    %pl.setWhiteout(1);
    %cl.play3D(BusSound, %pl.getPosition());
    %pl.schedule(100, spawnExplosion, spawnProjectile, "1");
    messageClient(%cl, '', "\c6Arrived at bus stop \"\c3" @ %menu.stopName[%option] @ "\c6\"!");
}

function fxDTSBrick::displayBusStopMenu(%brick, %cl)
{
    %pl = %cl.player;
    if (!isObject(%pl))
    {
        return;
    }

    if (!isObject(%brick.busStopMenu))
    {
        %brick.busStopMenu = new ScriptObject(masterBusStopMenu)
        {
            isCenterprintMenu = 1;
            isBusStopMenu = 1;
            menuName = "-Bus Stops ($0.50)-";
        };
        MissionCleanup.add(%brick.busStopMenu);
    }

    configureBusStopCenterprintMenu(%brick.busStopMenu, %brick);

    %cl.startCenterprintMenu(%brick.busStopMenu);
    busStopLoop(%cl, %brick);
}
registerOutputEvent("fxDTSBrick", "displayBusStopMenu", "", 1);

function busStopLoop(%cl, %obj)
{
    cancel(%cl.busStopSchedule);
    
    if (!isObject(%obj) || !isObject(%cl.player) || !%cl.isInCenterprintMenu)
    {
        %exit = 1;
    }

    if (vectorDist(%obj.getPosition(), %cl.player.getPosition()) > 8)
    {
        %exit = 1;
    }

    %start = %cl.player.getEyePoint();
    %end = vectorAdd(vectorScale(%cl.player.getEyeVector(), 8), %start);
    if (containerRaycast(%start, %end, %obj.getType(), %cl.player) != %obj)
    {
        %exit = 1;
    }

    if (%exit)
    {
        %cl.exitCenterprintMenu();
        return;
    }

    %currBrick = %cl.centerprintMenu.stopBrick[%cl.currOption];
    if (%cl.camera.busStopObj != %currBrick)
    {
        %cl.camera.setFlyMode();
        %pos = vectorAdd(%currBrick.getPosition(), "0 0 8");
        %start = vectorAdd(%pos, vectorScale(%currBrick.getForwardVector(), 5));
        %end = vectorAdd(%pos, "0 0 -3");

        //aim the camera at the target brick
        %delta = vectorSub(%end, %start);
        %deltaX = getWord(%delta, 0);
        %deltaY = getWord(%delta, 1);
        %deltaZ = getWord(%delta, 2);
        %deltaXYHyp = vectorLen(%deltaX SPC %deltaY SPC 0);

        %rotZ = mAtan(%deltaX, %deltaY) * -1; 
        %rotX = mAtan(%deltaZ, %deltaXYHyp);

        %aa = eulerRadToMatrix(%rotX SPC 0 SPC %rotZ); //this function should be called eulerToAngleAxis...

        %cl.camera.setTransform(%start SPC %aa);
        %cl.setControlObject(%cl.camera);
        %cl.camera.setControlObject(%cl.camera);
        %cl.camera.setDollyMode(%start, vectorAdd(%start, "0 0 0.1"));
        %cl.camera.busStopObj = %currBrick;
    }

    %cl.busStopSchedule = schedule(200, %cl, busStopLoop, %cl, %obj);
}