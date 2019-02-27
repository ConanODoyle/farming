//Allows bricks to define functionality when people click them or drop items into them.

//How to use: 	brickDatablock.processorFunction = "functionName";
//				brickDatablock.activateFunction = "functionName";

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
};
activatePackage(Processors);