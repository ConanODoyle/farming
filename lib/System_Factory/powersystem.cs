if (!isObject(PowerSystemGroup))
{
    $PowerSystemGroup = new SimSet(PowerSystemGroup);
}

function registerPowersystem()
{
    %devices = new SimSet();
    %producers = new SimSet();
    %consumers = new SimSet();
    %storage = new SimSet();
    %this = new ScriptObject(){
        class = "PowerSystem";
        devices = %devices;
        consumers = %consumers;
        producers = %producers;
        storage = %storage;
    };
    PowerSystemGroup.add(%this);
    return %this;
}

function PowerSystem::deleteAll(%this)
{
    %this.devices.delete();
    %this.producers.delete();
    %this.consumers.delete();
    %this.storage.delete();
}

function PowerSystem::addConsumer(%this, %obj)
{
    if (!%this.devices.isMember(%obj))
        %this.devices.add(%obj);
    if (!%this.consumers.isMember(%obj))
        %this.consumers.add(%obj);
}

function PowerSystem::addProducer(%this, %obj)
{
    if (!%this.devices.isMember(%obj))
        %this.devices.add(%obj);
    if (!%this.producers.isMember(%obj))
        %this.producers.add(%obj);
}

function PowerSystem::addStorage(%this, %obj)
{
    if (!%this.devices.isMember(%obj))
        %this.devices.add(%obj);
    if (!%this.storage.isMember(%obj))
        %this.storage.add(%obj);
}

function PowerSystem::tick(%this)
{
    %power = 0;
    for (%i = 0; %i < %this.producers.getCount(); %i++)
    {
        %power += %this.producers.getObject(%i).producePower();
    }

    %draw = 0;
    %count = %this.consumers.getCount();
    for (%i = 0; %i < %count; %i++)
    {
        %draw += %this.consumers.getObject(%i).getPowerDraw();
    }

    %storage = 0;
    %storageUsed = 0;
    %storageCount = 0; // keep list of energy storage that has power to withdraw from them all equally
    for (%i = 0; %i < %this.storage.getCount(); %i++)
    {
        %storage += %this.storage.getObject(%i).provideStoredPower();
    }

    %unsupplied = %draw - %power;
    // if we need more power than we can provide; draw from battery storage
    %storageUsed = getMax(getMin(%storage, %unsupplied), 0);
    %ratio = (%power + %storageUsed) / %draw; // each device gets at most %ratio * %power allocated to them
    for (%i = 0; %i < %count; %i++)
    {
        %this.consumers.getObject(%i).powerTick(%ratio, %power, %draw);
    }

    if (%storageUsed <= 0.000001)
    {
        return;
    }
    %this.updateStoredPower((%unsupplied * -1) - storageUsed);
}

function PowerSystem::updateStoredPower(%this, %amount)
{
    %amount = mFloatLength(%amount, 2);

    %count = %this.storage.getCount();
    %storageCount = 0;
    for (%i = 0; %i < %this.storage.getCount(); %i++)
    {
        %obj = %this.storage.getObject(%i);
        if (%obj.getStoredPower() > 0)
        {
            %storageList[%storageCount++ - 1] = %obj;
        }
    }

    %split = %amount / %storageCount;
    %unsupplied = 0;
    for (%i = 0; %i < %storageCount; %i++)
    {
        %unsupplied += %storageList[%i].updateStoredPower(%split);
    }

    if (%unsupplied > 0.01)
    {
        %this.updateStoredPower(%amount, %diff);
    }
}
