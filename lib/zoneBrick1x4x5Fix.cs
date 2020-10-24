schedule(100, 0, eval,
	"package ZoneBricks
	{
		function brickzone1x4x5::onLoadplant(%data,%this)
		{	
			Parent::onLoadPlant(%this);
			brickzone1x4x5::LoadParameters(%this);
		}
	};"
);