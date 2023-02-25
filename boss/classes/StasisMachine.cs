//-----------------//
// Statis Machine: //
//-----------------//

datablock StaticShapeData(StasisMachineShape)
{
	shapeFile = $Harvester::Root @ "/resources/shapes/stasisMachine.dts";
};

//---------------//
// Statis Brick: //
//---------------//

datablock fxDTSBrickData(BrickStasisMachineData)
{
	//------------//
	// Rendering: //
	//------------//
	
	brickFile = "base/data/bricks/flats/2x2F.blb";

	//-------------//
	// Properties: //
	//-------------//
	
	category = "";
	subCategory = "Interactive";

	uiName = "Stasis Machine";
	// iconName = "Add-Ons/Player_Harvester/icon_stasis_machine";
};

/// @param	this	brick datablock
/// @param	brick	brick
function BrickStasisMachineData::onTrustCheckFinished(%this, %brick)
{
	Parent::onTrustCheckFinished(%this, %brick);
	
	%brick.setColliding(0);
	%brick.setRendering(0);
	// %brick.setRayCasting(0);
	
	if(isObject(%brick.stasisMachine))
		%brick.stasisMachine.delete();
	
	switch(%brick.angleID)
	{
		case 0:
			%rotation = "0.0 0.0 1.0 90.0";
		case 1:
			%rotation = "0.0 0.0 1.0 180.0";
		case 2:
			%rotation = "0.0 0.0 -1.0 90.0";
		default:
			%rotation = "1.0 0.0 0.0 0.0";
	}
	
	%brick.stasisMachine = new StaticShape()
	{
		dataBlock = StasisMachineShape;
		position = vectorAdd(%brick.getPosition(), "0.0 0.0 -0.1");
		rotation = %rotation;
	};
	
	if(isObject(%brick.stasisMachine))
	{
		MissionCleanup.add(%brick.stasisMachine);
		%brick.stasisMachine.setNodeColor("ALL", getColorIDTable(%brick.getColorID()));
		%brick.stasisMachine.playThread(0, "rotate");
	}
}

/// @param	this	brick datablock
/// @param	brick	brick
function BrickStasisMachineData::onRemove(%this, %brick)
{
	if(isObject(%brick.stasisMachine))
	{
		%brick.stasisMachine.delete();
	}
	
	Parent::onRemove(%this, %brick);
}

/// @param	this	brick datablock
/// @param	brick	brick
function BrickStasisMachineData::onDeath(%this, %brick)
{
	if(isObject(%brick.stasisMachine))
	{
		%brick.stasisMachine.delete();
	}
	
	Parent::onDeath(%this, %brick);
}

/// @param	this	brick datablock
/// @param	brick	brick
function BrickStasisMachineData::onColorChange(%this, %brick)
{
	Parent::onColorChange(%this, %brick);
	
	if(isObject(%brick.stasisMachine))
	{
		%brick.stasisMachine.setNodeColor("ALL", getColorIDTable(%brick.getColorID()));
	}
}

/// @param	this	brick
function fxDTSBrick::deactivateStasisMachine(%this)
{
	if(isObject(%this.stasisMachine))
	{
		%this.stasisMachine.playThread(0, "ready");
		%this.stasisMachine.schedule(625, playThread, 0, "unlock");
		%this.stasisMachine.schedule(2042, playThread, 0, "deactivate");
		// Spawn item at 2,667 MS.
	}
}

/// @param	this	brick
function fxDTSBrick::resetStasisMachine(%this)
{
	if(isObject(%this.stasisMachine))
	{
		%this.stasisMachine.playThread(0, "rotate");
	}
}

registerOutputEvent(fxDTSBrick, deactivateStasisMachine);
registerOutputEvent(fxDTSBrick, resetStasisMachine);