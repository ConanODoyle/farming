registerOutputEvent("Player", "setWhiteout", "float 0 1 0.1 0.5", 0);

package fixWhiteoutEvent
{
	function ShapeBase::setWhiteout(%obj, %val, %extra)
	{
		parent::setWhiteout(%obj, %val);
		return %val;	
	}
};
activatePackage(fixWhiteoutEvent);