function getRandomBrickOrthoRot()
{
	%rand = getRandom(0, 3);
	switch (%rand)
	{
		case 0: return "1 0 0 0";
		case 1: return "0 0 1 " @ 90;
		case 2: return "0 0 1 " @ 180;
		case 3: return "0 0 -1 " @ 90;
	}
}
