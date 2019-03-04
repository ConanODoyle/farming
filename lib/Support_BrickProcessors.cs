//Allows bricks to define functionality when people click them or drop items into them.

//How to use: 	brickDatablock.processorFunction = "functionName";
//				brickDatablock.activateFunction = "functionName";
//				brickDatablock.placerItem = "datablockName"; //defined if the brick must be placed with a special item

package Processors
{
	function ServerCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%start = %pl.getEyePoint();
			%end = vectorAdd(vectorScale(%pl.getEyeVector(), 6), %start);
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && %hit.getDatablock().isProcessor)
			{
				%func = %hit.getDatablock().processorFunction;
				if (isFunction(%func))
				{
					call(%func, %hit, %cl, %slot);
					return;
				}
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}

	function Player::activateStuff(%obj)
	{
		if (isObject(%cl = %obj.client))
		{
			%start = %obj.getEyeTransform();
			%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && ((%db = %hit.getDatablock()).isProcessor))
			{
				%func = %hit.getDatablock().activateFunction;
				if (isFunction(%func))
				{
					call(%func, %hit, %obj);
					return;
				}
			}
		}

		return parent::activateStuff(%obj);
	}

	function fxDTSBrick::onDeath(%this, %obj)
	{
		talk(%this.getDatablock().placerItem);
		if (isObject(%this.getDatablock().placerItem))
		{
			%i = new Item(BrickPlacers) {
				dataBlock = %this.getDatablock().placerItem;
				harvestedBG = getBrickgroupFromObject(%this);
			};
			%i.setTransform(%this.getTransform());
			%i.setVelocity("0 0 4");
			MissionCleanup.add(%i);
			%i.schedule(60000, schedulePop);
		}	
		return parent::onDeath(%this, %obj);
	}

	function Armor::onDisabled(%this, %obj, %state)
	{
		if (isObject(%obj.client))
		{
			%obj.client.itemBrickSelection = 0;
			%obj.itemBrickSelection = 0;
		}
		return parent::onDisabled(%this, %obj, %state);
	}

	function serverCmdPlantBrick(%cl)
	{
		if (isObject(%pl = %cl.player) && isObject(%pl.tempBrick)) //&& !%cl.bypassRestrictions)
		{
			%db = %pl.tempBrick.getDatablock();
			if (%db.isProcessor && isObject(%db.placerItem))
			{
				if (%cl.itemBrickSelection != %db)
				{
					messageClient(%cl, '', "You cannot plant " @ %db.uiname @ " bricks without the item!");
					serverCmdCancelBrick(%cl);
					return;
				}
				else
				{
					%item = %pl.tool[%pl.currTool];
					if (%item.image.placeBrick.getID() != %db)
					{
						messageClient(%cl, '', "You cannot plant " @ %db.uiname @ " bricks without the item!");
						serverCmdCancelBrick(%cl);
						return;
					}

					%pl.tool[%pl.currTool] = "";
					messageClient(%cl, 'MsgItemPickup', '', %pl.currTool, 0);
					%ret = parent::serverCmdPlantBrick(%cl);
					serverCmdUnuseTool(%cl);
					return %ret;
				}
			}
		}

		return parent::serverCmdPlantBrick(%cl);
	}
};
activatePackage(Processors);