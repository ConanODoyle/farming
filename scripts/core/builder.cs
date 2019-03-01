function serverCmdBuilder(%cl, %target)
{
	if (%cl.isSuperAdmin)
	{
		if (strlen(%target) > 1 && isObject(%targ = findClientByName(%target)))
		{
			messageClient(%cl, '', "\c6You gave \c3" @ %targ.name @ "\c6 builder mode!");
			%cl = %targ;
		}
		%cl.bypassRestrictions = 1;
		%cl.player.setDatablock(PlayerStandardArmor);
		messageClient(%cl, '', "\c6You are now a builder!");
	}
}

function serverCmdUnbuilder(%cl, %target)
{
	if (%cl.isSuperAdmin)
	{
		if (strlen(%target) > 1 && isObject(%targ = findClientByName(%target)))
		{
			messageClient(%cl, '', "\c6You removed \c3" @ %targ.name @ "\c6's builder mode!");
			%cl = %targ;
		}
		%cl.bypassRestrictions = 0;
		%cl.player.setDatablock(isObject(%cl.playerDatablock) ? %cl.playerDatablock : PlayerNoJet);
		messageClient(%cl, '', "\c6You are not a builder anymore!");
	}
}

datablock ParticleData(BuilderOrbCameraParticle : CameraParticleA)
{
	colors[0] = "0.3 0.0 0.3 0.0";
	colors[1] = "0 0.4 1.0 1.0";
	colors[2] = "0 0.4 1.0 0.0";
	useInvAlpha = 0;
};
datablock ParticleEmitterData(BuilderOrbCameraEmitter : CameraEmitterA)
{
	 particles = "BuilderOrbCameraParticle";
	 uiName = "Builder Orb Glow";
};
datablock ShapeBaseImageData(BuilderOrbCameraImage : CameraImage)
{
	 stateEmitter[1] = "BuilderOrbCameraEmitter";
	 stateEmitter[2] = "BuilderOrbCameraEmitter";
};

if(isPackage(BuilderOrb))
{
	deactivatePackage(BuilderOrb);
}

package BuilderOrb
{
	function serverCmdDropCameraAtPlayer(%cl)
	{
		if (!%cl.isAdmin && %cl.bypassRestrictions && isObject(%cl.player)) 
		{
			%cl.camera.mountImage("BuilderOrbCameraImage",0);
			%cl.camera.setTransform(%cl.player.getEyePoint() SPC getWords(%cl.player.getTransform(),3,6));
			%cl.setControlObject(%cl.camera);
		} 
		else
		{
			parent::serverCmdDropCameraAtPlayer(%cl);
		}
	}
	
	function serverCmdDropPlayerAtCamera(%cl)
	{
		if (!%cl.isAdmin && %cl.bypassRestrictions && isObject(%cl.player)) 
		{
			%cl.camera.unMountImage(0);
			%cl.setControlObject(%cl.player);
			%cl.player.setTransform(%cl.camera.getTransform());
			%cl.player.teleportEffect();
		}
		else
		{
			parent::serverCmdDropPlayerAtCamera(%cl);
		}
	}
};
activatePackage(BuilderOrb);
