//override package to add builder-only-support for newbricktool
if (!isPackage(NewBrickTool)) //port's new brick tool isnt enabled
{
    return;
}

package NewBrickTool
{
    function brickImage::onFire(%this, %obj, %slot, %repeat)
    {
        if (!%obj.client.isBuilder)
        {
            return parent::onFire(%this, %obj, %slot, %repeat);
        }
    }

    function brickImage::onMount(%this, %obj, %slot)
    {
        Parent::onMount(%this, %obj, %slot);

        if (!%obj.client.isBuilder)
        {
            return;
        }
        %client = %obj.client;

        if (!isObject(%client))
            return;

        %control = %client.getControlObject();

        if (%control.getClassName() $= "Camera" && $Pref::Server::NewBrickTool::AdminOrb)
        {
            %text = "<font:palatino linotype:86>\n<bitmap:base/client/ui/crossHair>";
            commandToClient(%client, 'CenterPrint', %text, 0);
        }
    }

    function brickImage::onUnMount(%this, %obj, %slot)
    {
        Parent::onUnMount(%this, %obj, %slot);

        %shape = %obj.brickImageRepeatShape;

        if (isObject(%shape) && %shape.repeat)
        {
            %shape.newBrickToolDisableRepeat();
            %obj.brickImageRepeatShape = "";
        }

        %client = %obj.client;

        if (!isObject(%client))
            return;

        %control = %client.getControlObject();

        if (%control.getClassName() $= "Camera" && $Pref::Server::NewBrickTool::AdminOrb)
            commandToClient(%client, 'ClearCenterPrint');
    }

    function GameConnection::setControlObject(%this, %object)
    {
        %previous = %this.getControlObject();
        Parent::setControlObject(%this, %object);
        %player = %this.player;

        if ($Pref::Server::NewBrickTool::AdminOrb && isObject(%player) &&
            %player.getMountedImage(0) == nameToID("brickImage"))
        {
            if (%object && %object.getClassName() $= "Camera" && %this.isBuilder)
            {
                %text = "<font:palatino linotype:86>\n<lmargin:1><bitmap:base/client/ui/crossHair>";
                commandToClient(%this, 'CenterPrint', %text, 0);
            }
            else if (%previous && %previous.getClassName() $= "Camera")
                commandToClient(%this, 'ClearCenterPrint');
        }
    }

    function Observer::onTrigger(%this, %obj, %slot, %state)
    {
        %client = %obj.getControllingClient();
        %player = %client.player;

        if ($Pref::Server::NewBrickTool::AdminOrb && isObject(%player) &&
            !%obj.isOrbitMode() && !%obj.getControlObject() && %slot == 0 &&
            %player.getMountedImage(0) == nameToID("brickImage") && %client.isBuilder)
        {
            %shape = %player.brickImageRepeatShape;

            if (isObject(%shape))
            {
                %player.brickImageRepeatShape = "";
                %shape.repeat = "";
                %shape.newBrickToolFade(%shape.a, %shape.b, %shape.color, 1);
            }

            cancel(%player.brickImageRepeat);

            if (%state)
            {
                %text = "<font:palatino linotype:86>\n<lmargin:1><bitmap:base/client/ui/crossHair>";
                commandToClient(%client, 'CenterPrint', %text, 0);

                if ($Sim::Time - %player.lastBrickImageFire > 0.25)
                {
                    newBrickToolFire(%player, false);
                    %player.lastBrickImageFire = $Sim::Time;
                }

                if ($Pref::Server::NewBrickTool::AllowRepeat)
                    %player.brickImageRepeat = %player.schedule(250, "brickImageRepeat");
            }

            return;
        }

        Parent::onTrigger(%this, %obj, %slot, %state);
    }

    function Armor::onTrigger(%this, %obj, %slot, %state)
    {
        if (%obj.getMountedImage(0) == nameToID("brickImage") && %slot == 0 && %obj.client.isBuilder)
        {
            %shape = %obj.brickImageRepeatShape;

            if (isObject(%shape))
            {
                %obj.brickImageRepeatShape = "";
                %shape.repeat = "";
                %shape.newBrickToolFade(%shape.a, %shape.b, %shape.color, 1);
            }

            cancel(%obj.brickImageRepeat);

            if (%state)
            {
                if ($Sim::Time - %obj.lastBrickImageFire > 0.25)
                {
                    newBrickToolFire(%obj, false);
                    %obj.lastBrickImageFire = $Sim::Time;
                }

                if ($Pref::Server::NewBrickTool::AllowRepeat)
                    %obj.brickImageRepeat = %obj.schedule(250, "brickImageRepeat");
            }

            return;
        }

        Parent::onTrigger(%this, %obj, %slot, %state);
    }

    function Armor::onRemove(%this, %obj)
    {
        if (isObject(%obj.brickImageRepeatShape))
            %obj.brickImageRepeatShape.delete();

        Parent::onRemove(%this, %obj);
    }

    function Armor::onDisabled(%this, %obj, %state)
    {
        %shape = %obj.brickImageRepeatShape;

        if (isObject(%shape) && %shape.repeat)
        {
            %shape.newBrickToolDisableRepeat();
            %obj.brickImageRepeatShape = "";
        }

        Parent::onDisabled(%this, %obj, %state);
    }
};
activatePackage("NewBrickTool");
