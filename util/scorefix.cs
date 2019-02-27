package scoreFix
{
	function GameConnection::setScore(%cl, %score)
	{
		if (%score < 1000000)
		{
			%score = (mFloor(%score * 100) | 0) / 100; //ensure integers if %score value doesnt have decimal
		}
		else
		{
			%score = 1000000;
		}

		return parent::setScore(%cl, %score);
	}
};
activatePackage(scoreFix);
