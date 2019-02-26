function swolSet::findNextOpenIndx(%set)
{
    for(%i=%set.low;%i<%set.high;%i++)
        if(!%set.obj[%i])
            break;
    return %i;
}
function swolSet::addObj(%set,%obj)
{
    if(!isObject(%obj))
        return;
    %i = %set.findNextOpenIndx();
    %set.rta[%obj] = %i;
    %set.obj[%i] = %obj;
    if(%i >= %set.high)
        %set.low = %set.high = %i+1;
    else
        %set.low++;
}
function swolSet::remObj(%set,%obj)
{
    %i = %set.rta[%obj];
    if(%i $= "")
        return;
    %set.obj[%i] = "";
    %set.rta[%obj] = "";
    if(%i < %set.low)
        %set.low = %i;
}
function swolSet::getObj(%set,%i)
{
    if(!isObject(%o = %set.obj[%i]))
    {
        %set.remObj(%o);
        return "";
    }
    return %o;
}
function swolSet::getPotentialCount(%set)
{
    return %set.high;
}
function swolSet::ini(%set)
{
    %set.high = 0;
    %set.low = 0;
}
function loopThroughTestSet(%ind)
{
    %set = $testSwolSet;
    %max = %set.getPotentialCount();
    if(%ind < %max-1)
    {
        %top = %ind+50;
        if(%top > %max)
            %top = %max;
        for(%i=%ind;%i<%top;%i++)
        {
            if(!%obj = %set.getObj(%i))
                continue;
            //do something with obj
        }
    }
    else
    {
        %ind = 0;
    }
    cancel(%set.sched);
    %set.sched = schedule(33,%set,loopThroughTestSet,%ind);
}
function addBrickToSet(%obj)
{
    if(!isObject(%obj))
        return;
    $testSwolSet.addObj(%obj);
    if(!isEventPending($testSwolSet.sched))
        loopThroughTestSet(0);
}
package swol_test
{
    function fxDTSBrick::onAdd(%obj)
    {
        %ret = parent::onAdd(%obj);
        if (%obj.getDatablock().isSprinkler && %obj.isPlanted)
        {
            schedule(33,%obj,addBrickToSet,%obj);
        }
        return %ret;
    }
};
activatePackage(swol_test);

if (!isObject($testSwolSet))
    ($testSwolSet = new scriptObject(swolSet)).ini();