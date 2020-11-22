function maxAllQuotas()
{
	for (%i = 0; %i < QuotaGroup.getCount(); %i++)
	{
		%obj = QuotaGroup.getObject(%i);
		%obj.setAllocs_Schedules(9999, 5465489);
		%obj.setAllocs_Misc(9999, 5465489);
		%obj.setAllocs_Projectile(9999, 5465489);
		%obj.setAllocs_Item(9999, 5465489);
		%obj.setAllocs_Environment(9999, 5465489);
		%obj.setAllocs_Player(9999, 5465489);
		%obj.setAllocs_Vehicle(9999, 5465489);
	}
}