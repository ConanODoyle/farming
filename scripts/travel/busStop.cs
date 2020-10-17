
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
};
activatePackage(BusStops);

function configureBusStopCenterprintMenu(%menu, %brick)
{
    if ((%menu.lastConfiguredMenu + 20 | 0) > $Sim::Time)
    {
        return;
    }

    %menu.lastConfiguredMenu = $Sim::Time;
    %oldMenuCount = %menu.menuOptionCount;
    for (%i = 0; %i < %oldMenuCount; %i++)
    {
        %menu.menuOption[%i] = "";
        %menu.menuFunction[%i] = "";
        %menu.menuBrick[%i] = "";
    }

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
        if (%obj == %brick || vectorDist(%obj.position, %brick.position) < 8)
        {
            %name = %name @ " - You are here";
        }
        else if (isObject(%brick))
        {
            %dist = mFloor(vectorDist(%brick.position, %obj.position) / 10) / 100;
            %name = %name @ " - " @ %dist @ "km";
        }

        %menu.menuOption[%i] = %name;
        %menu.menuFunction[%i] = "goToBusStop";
        %menu.menuBrick[%i] = %obj;
        %menu.stopName[%i] = %originalName;
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

    %pl.setTransform(%brick.getTransform());
    %pl.setWhiteout(1);
    %cl.play2D(BusSound);
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
            menuName = "-Bus Stops-";
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

    if (vectorDist(%obj.getPosition(), %cl.player.getPosition()) > 4)
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

    %cl.busStopSchedule = schedule(200, %cl, busStopLoop, %cl, %obj);
}