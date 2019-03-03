package noDualClienting
{
	function GameConnection::autoAdminCheck(%cl)
	{
		for (%i = 0; %i < Clientgroup.getCount(); %i++)
		{
			%t = Clientgroup.getObject(%i);
			if (%t != %cl && %t.bl_id == %cl.bl_id)
			{
				%t.savePersistence();
				%t.delete("No dual clienting allowed!");
				return;
			}
		}

		return parent::autoAdminCheck(%cl);
	}
};
activatePackage(noDualClienting);
