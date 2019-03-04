function getEmptyToolSlot(%pl)
{
	%db = %pl.getDatablock();
	for(%i=0; %i < %db.maxTools; %i++)
	{
		%item = %pl.tool[%i];
		if(%item $= 0 || %item $= "")
		{
			return %i;
		}
	}
	return -1;
}

package Support_PickupDuplicateItems
{
	function Armor::onCollision(%this, %obj, %col, %vec, %speed)
	{	
		if(%col.getDatablock().canPickupMultiple && %col.canPickup)
		{
			%freeSlot = getEmptyToolSlot(%obj);

			if (%freeSlot >= 0)
			{
				%obj.pickup(%col);
				return;
			}
		}
		return parent::onCollision(%this, %obj, %col, %vec, %speed);
	}
};
activatePackage(Support_PickupDuplicateItems);