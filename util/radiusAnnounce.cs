function radiusAnnounce(%msg, %pos, %radius)
{
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%cl = ClientGroup.getObject(%i);
		if (isObject(%pl = %cl.player) && vectorDist(%pl.getPosition(), %pos) < %radius)
		{
			messageClient(%cl, '', %msg);
		}
	}
}