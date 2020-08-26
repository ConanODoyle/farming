function servercmdclearitems(%cl)
{
	if (%cl.isAdmin)
	{
		%pos = %cl.player.getPosition();
		initContainerRadiusSearch(%pos, 300, $Typemasks::itemobjecttype);
		while (isObject(%next = containerSearchNext()))
		{
			if (!%next.isStatic())
				%next.delete();
		}
		messageAll('MsgClearBricks', "\c2" @%cl.name @ " cleared nearby items.");
	}
}
