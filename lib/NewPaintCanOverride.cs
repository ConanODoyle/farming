//override NewPaintCan code to increase refire rate (reduce paint skipping)
if (!isPackage(NewPaintCan)) //port's new brick tool isnt enabled
{
    return;
}

package NewPaintCan
{
    //prevent paintcans from ejecting projectiles
    function WeaponImage::onFire(%this, %obj, %slot)
    {
        %img = %obj.getMountedImage(0);
        if($Pref::Server::NPC_Enabled && (%img.item $= "SprayCan" 
            && %img.getName() $= ("color" @ %obj.client.currentColor @ "SprayCanImage") 
            || $NPC_PaintFX[%img.getName()] !$= ""))
            return;
        else
            return Parent::onFire(%this, %obj, %slot);
    }

    function Observer::onTrigger(%this, %obj, %slot, %val)
    {
        %c = %obj.getControllingClient();
        if(!isObject(%p = %c.player) || !$Pref::Server::NPC_Enabled)
        {
            Parent::onTrigger(%this, %obj, %slot, %val);
            return;
        }
        if(!isObject(%img = %p.getMountedImage(0)))
        {
            Parent::onTrigger(%this, %obj, %slot, %val);
            return;
        }
        if(isObject(%p) && !%obj.isOrbitMode() && !%obj.getControlObject() && %slot == 0 && %img.getName() $= ("color" @ %c.currentColor @ "SprayCanImage") || $NPC_PaintFX[%img.getName()] !$= "")
        {
            if(!$Pref::Server::NewPaintCan::AdminOrbPainting)
            {
                Parent::onTrigger(%this, %obj, %slot, %val);
                return;
            }
            if(%slot != 0)
                return;
            %shape = %p.paintLaserShape;
            if(isObject(%shape))
            {
                %p.paintLaserShape = "";
                %shape.repeat = "";
                %shape.NPC_fadeLaser(%shape.startPos, %shape.endPos, %shape.color, 1);
            }
            cancel(%p.PaintLaserRepeat);
            if(%c.NPC_isFillPaint)
            {
                Parent::onTrigger(%this, %obj, %slot, %val);
                return;
            }
            if(%val)
            {
                %text = "<font:palatino linotype:86>\n<lmargin:1><bitmap:base/client/ui/crossHair>";
                commandToClient(%c, 'CenterPrint', %text, 0);
                if($Sim::Time - %p.lastPaintLaserFire > 0.03)
                {
                    NPC_FireLaser(%p, 0);
                    %p.lastPaintLaserFire = $Sim::Time;
                }
                %p.PaintLaserRepeat = %p.schedule(33, "PaintLaserRepeat");
            }
            return;
        }
        Parent::onTrigger(%this, %obj, %slot, %val);
    }

    function Armor::onTrigger(%this, %obj, %slot, %val)
    {
        if(!isObject(%img = %obj.getMountedImage(0)) || !$Pref::Server::NPC_Enabled)
        {
            Parent::onTrigger(%this, %obj, %slot, %val);
            return;
        }
        if(%img.item $= "SprayCan" && %img.getName() $= ("color" @ %obj.client.currentColor @ "SprayCanImage") || $NPC_PaintFX[%img.getName()] !$= "")
        {
            if(%slot != 0)
                return;
            %shape = %obj.paintLaserShape;
            if(isObject(%shape))
            {
                %obj.paintLaserShape = "";
                %shape.repeat = "";
                %shape.NPC_fadeLaser(%shape.startPos, %shape.endPos, %shape.color, 1);
            }
            cancel(%obj.paintLaserRepeat);
            if(%obj.client.NPC_isFillPaint)
            {
                Parent::onTrigger(%this, %obj, %slot, %val);
                return;
            }
            if(%val)
            {
                if($Sim::Time - %obj.lastPaintLaserFire > 0.03)
                {
                    NPC_FireLaser(%obj, 0);
                    %obj.lastPaintLaserFire = $Sim::Time;
                }
                %obj.paintLaserRepeat = %obj.schedule(33, "paintLaserRepeat");
            }
            return;
        }
        Parent::onTrigger(%this, %obj, %slot, %val);
    }
};
activatePackage(NewPaintCan);