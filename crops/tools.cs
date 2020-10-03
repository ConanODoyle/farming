exec("./tools/harvestTools.cs");
exec("./tools/planter.cs");
exec("./tools/reclaimer.cs");
exec("./tools/weedcutter.cs");
exec("./tools/waterTools.cs");


function repairDurability(%dataID, %amount)
{
	if (%dataID $= "")
	{
		return;
	}

	%max = getDataIDArrayTagValue(%dataID, "maxDurability");
	%durability = getDataIDArrayTagValue(%dataID, "durability") + %amount | 0);
	%final = %durability > %max : %max ? %durability; //ternaries auto apply | 0
	setDataIDArrayTagValue(%dataID, "durability", %final);
}

function getDurability(%this, %obj, %slot)
{
	%dataID = %obj.toolDataID[%obj.currTool];
	if (%dataID $= "")
	{
		return 0;
	}
	return getDataIDArrayTagValue(%dataID, "durability");
}

function useDurability(%this, %obj, %slot)
{
	%dataID = %obj.toolDataID[%obj.currTool];
	if (%dataID $= "")
	{
		return 0;
	}
	%durability = getDataIDArrayTagValue(%dataID, "durability");
	if (%durability > 0)
	{
		setDataIDArrayTagValue(%dataID, "durability", %durability - 1 | 0);
	}
	return getMax(%durability, 0);
}

function generateToolDataID(%item)
{
	if (!isObject(%item))
	{
		return "";
	}

	%dataID = getRandomHash() @ "Tool";
	setDataIDArrayTagValue(%dataID, "datablock", %item.getName());

	if (isFunction(%item.durabilityFunction))
	{
		%durability = call(%item.durabilityFunction, %item) | 0;
	}
	else
	{
		%durability = %item.durability > 0 ? %item.durability : 100;
	}

	setDataIDArrayTagValue(%dataID, "durability", %durability | 0);
	setDataIDArrayTagValue(%dataID, "maxDurability", %durability | 0);

	if (isFunction(%item.modifiersFunction))
	{
		%modifiers = call(%item.modifiersFunction);
		for (%i = 0; %i < getFieldCount(%modifiers); %i++)
		{
			setDataIDArrayValue(%dataID, %i, getField(%modifiers, %i));
		}
	}

	return %dataID;
}

package DataIDTools
{
	function serverCmdUseTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%tool = %pl.tool[%slot];
			if (%tool.hasDataID && %tool.isDataIDTool && %pl.toolDataID[%slot] $= "")
			{
				%pl.toolDataID[%slot] = generateToolDataID(%tool);
			}
		}
		return parent::serverCmdUseTool(%cl, %slot);
	}

	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%tool = %pl.tool[%slot];
			if (%tool.hasDataID && %tool.isDataIDTool && %pl.toolDataID[%slot] $= "")
			{
				%pl.toolDataID[%slot] = generateToolDataID(%tool);
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}
};
activatePackage(DataIDTools);