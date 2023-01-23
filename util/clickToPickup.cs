
package ClickToPickup_Farming
{
	function Player::activateStuff(%pl)
	{
		if (%pl.getMountedImage(0) == 0)
		{
			%start = %pl.getEyePoint();
			%end = vectorScale(%pl.getEyeVector(), 4 * getWord(%pl.getScale(), 2));
			%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::ItemObjectType;
			%hit = containerRayCast(%start, vectorAdd(%start, %end), %mask);
			if(isObject(%hit) && %hit.getClassName() $= "Item")
			{
				%pl.dataBlock.onCollision(%pl, %hit, "0 0 0", 0);
			}
		}
		return parent::activateStuff(%pl);
	}
};
activatePackage(ClickToPickup_Farming);