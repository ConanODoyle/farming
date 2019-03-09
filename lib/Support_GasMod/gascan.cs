//########## Gas Can

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$RTB::RTBR_ServerControl_Hook) exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	RTB_registerPref("Fuel amount","Gas Can","$gc_gasCan::Amount","int 0 500","Item_GasCan",200,0,0);
}
else
{
	$gc_gasCan::Amount = 50;
}

//### Sounds

datablock AudioProfile(gc_gasCanRefuelSound)
{
	filename = "./refuel.wav";
	description = AudioClosest3d;
	preload = true;
};

//### Item

datablock ItemData(gc_gasCanItem)
{
	uiName = "Gas Canister";
	iconName = "./icon_gascan";
	image = gc_gasCanImage;
	category = "Tools";
	className = "Weapon";
	shapeFile = "./gascan.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0;
	friction = 0.6;
	emap = true;
	doColorShift = true;
	colorShiftColor = "1 1 1 1";
	canDrop = true;
};

//### Item Image

datablock ShapeBaseImageData(gc_gasCanImage)
{
	shapeFile = "./gascan.dts";
	emap = true;
	correctMuzzleVector = true;
	className = "WeaponImage";
	item = gc_gasCanItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "1 1 1 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Fire";
	stateTransitionOnTimeOut[2] = "Ready";
	stateTimeoutValue[2] = "0.2";
	stateFire[2] = true;
	stateAllowImageChange[2] = true;
	stateScript[2] = "onFire";
};

function gc_gasCanImage::onFire(%this,%obj,%slot)
{
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 3));
	%raycast = containerRayCast(%start, %end, $TypeMasks::VehicleObjectType | $TypeMasks::fxBrickObjectType);
	%thing = firstWord(%raycast);
	if(isObject(%thing) && %thing.getClassName() !$= "fxDTSBrick")
	{
		%thing.VGM_Gas += $gc_gasCan::Amount; //200
		serverPlay3D(gc_gasCanRefuelSound, %obj.getTransform());
		centerPrint(%obj.client,"Fuel: \c3" @ mCeil(%thing.VGM_Gas) @ " ", 1);

		//remove item
		%currSlot = %obj.currTool;
		%obj.tool[%currSlot] = 0;
		%obj.weaponCount--;
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
}

package gc_gasCan
{
	function Armor::onCollision(%self, %obj, %col, %vec, %speed) 
	{
		if(%col.dataBlock $= "gc_gasCanItem" && %col.canPickup) 
		{
			for(%i=0;%i<%this.maxTools;%i++) 
			{
				%item = %obj.tool[%i];
				if(%item $= 0 || %item $= "") 
				{ 
					%freeSlot = 1;
					break; 
				} 
			}
			if(%freeSlot) 
			{ 
				%obj.pickup(%col); return; 
			} 
		}
		parent::onCollision(%self, %obj, %col, %vec, %speed); 
	}
};
activatePackage(gc_gasCan);
