datablock PlayerData(duoJeepCargoArmor : PlayerStandardArmor) {
    shapeFile = "./cargo.dts";

    uiName = "";

    boundingBox = vectorScale("3.9 4.3 3.0", 4);
    crouchBoundingBox = vectorScale("3.9 4.3 3.0", 4);

    useCustomPainEffects = true;
    PainHighImage = "";
    PainMidImage = "";
    PainLowImage = "";
    painSound = "";
    deathSound = "";

    // extra fields for conan's farming
    isStorageVehicle = 1;
    storageSlotCount = 4;
    storageMultiplier = 10;
    itemStackCount = 2;
    menuName = "Cargo Jeep";

    hasStorageNodes = 1;
    numStorageNodes = 5;
    storageNodePrefix = "storage";
    storageNodeType = 1; // 0 is exclusive, 1 is additive
};

function duoCargoJeepVehicle::onAdd(%this, %obj)
{
    Parent::onAdd(%this, %obj);

    %s = new AIPlayer()
    {
        dataBlock = duoJeepCargoArmor;
    };

    if (isObject(%s)) //creation can fail due to quota limit
    {
        MissionCleanup.add(%s);
        %obj.mountObject(%s, 2);
        %obj.storageBot = %s;
        %s.schedule(10,"rigStorageBot");
    }
}

function duoCargoJeepVehicle::onRemove(%this, %obj)
{
    if (isObject(%obj.storageBot)) %obj.storageBot.delete();
}

function Player::rigStorageBot(%obj)
{
    if(%obj.dataBlock !$= duoJeepCargoArmor) return;

    if(!isObject(%obj)) return;

    %parent = %obj.getObjectMount();
    if(!isObject(%parent)) return;

    %obj.spawnBrick = %parent.spawnBrick;
    %obj.brickGroup = %parent.brickGroup;

    if ($AddOnLoaded__Server_Farming)
    {
        cartAddEvent(%obj);
        %obj.updateStorageNodes(%obj.spawnBrick.eventOutputParameter0_1);
    }
}

package duoJeepCargoPackage
{
    function Player::burn(%obj, %time)
    {
        if (%obj.dataBlock $= duoJeepCargoArmor) return;
        else Parent::burn(%obj, %time);
    }

    function ShapeBase::setNodeColor(%obj, %node, %color)
    {
        Parent::setNodeColor(%obj, %node, %color);
        if (isObject(%obj.storageBot))
        {
            %obj.storageBot.setNodeColor("ALL", %color);
        }
    }

    function duoCargoJeepVehicle::damage(%this, %obj, %source, %pos, %amt, %type)
    {
        if ((%obj.getDamageLevel() + %amt) >= %this.maxDamage)
        {
            if(%obj.destroyed) return;

            %obj.setNodeColor("ALL","0 0 0 1");
            if(isObject(%obj.storageBot))
            {
                %obj.storageBot.delete();
            }

            %p = new Projectile()
            {
                dataBlock = vehicleExplosionProjectile;
                initialPosition = vectorAdd(%obj.getPosition(),"0 0" SPC %this.initialExplosionOffset);
                initialVelocity = "0 0 1";
                client = %obj.lastDamageClient;
                sourceClient = %obj.lastDamageClient;
            };
            MissionCleanup.add(%p);

            %obj.setDamageLevel(%this.maxDamage);
            %obj.destroyed = 1;
            %obj.schedule(%this.burnTime,"finalExplosion");
            if(isObject(%obj.spawnBrick.client.minigame))
            %respawn = %obj.spawnBrick.client.minigame.vehicleReSpawnTime;
            %obj.spawnBrick.schedule(%respawn,"spawnVehicle");
        }
        else Parent::Damage(%this, %obj, %source, %pos, %amt, %type);
    }

    function Player::emote(%obj, %emote)
    {
        if(%obj.dataBlock $= duoJeepCargoArmor) return;
        Parent::emote(%obj, %emote);
    }

    function duoJeepCargoArmor::onDisabled(%this, %obj, %state)
    {
        %obj.schedule(10,"delete");
    }

    function duoJeepCargoArmor::Damage(%data, %obj, %source, %pos, %amt, %type)
    {
        // don't take damage from conventional sources - probably will break stuff but WHATEVER
        return;
    }

    function Vehicle::onActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec)
    {
        if (%vehicle.getDatablock() != duoCargoJeepVehicle.getID() || !isObject(%vehicle.spawnBrick))
        {
            return Parent::onActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec);
        }

        %start = %activatingObj.getEyePoint();

        %vec = %activatingObj.getEyeVector();
        %scale = getWord (%activatingObj.getScale(), 2);
        %end = VectorAdd (%start, VectorScale(%vec, 10 * %scale));

        %mask = $TypeMasks::PlayerObjectType;

        %hit = containerRayCast(%start, %end, %mask, %activatingObj);

        if (%hit == %vehicle.storageBot)
        {
            return;
        }

        return Parent::onActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec);
    }

    function Armor::onMount(%this, %obj, %col, %slot)
    {
        Parent::onMount(%this, %obj, %col, %slot);

        if (!isObject(%col) || %slot != 0 || (%col.getDatablock() != duoCargoJeepVehicle.getID() && %col.getDatablock() != duoStandardJeepVehicle.getID()))
            return;

        %obj.playThread(1, armReadyBoth);
    }

    function Armor::onUnMount(%this, %obj, %col, %slot)
    {
        Parent::onUnMount(%this, %obj, %col, %slot);

        if (!isObject(%col) || %slot != 0 || (%col.getDatablock() != duoCargoJeepVehicle.getID() && %col.getDatablock() != duoStandardJeepVehicle.getID()))
            return;

        %oldImage = %obj.getMountedImage(0);

        if (isObject(%oldImage))
        {
            %obj.unmountImage(0);
            %obj.updateArm();
            %obj.mountImage(%oldImage, 0);
        }
        else
        {
            %obj.stopThread(1);
        }
    }
};
activatepackage(duoJeepCargoPackage);