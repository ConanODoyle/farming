datablock fxLightData(FillYellow1)
{
	uiName = "Fill Yellow (250)";

	LightOn = true;
	radius = 250;
	brightness = 1.5;
	color = "1 0.8 0.65 1";

	flareOn = false;
	flarecolor = "1 1 1";
	flarebitmap = "";
	ConstantSizeOn = true;
	ConstantSize = 2;
	NearSize = 1;
	FarSize = 1;
};

datablock fxLightData(FillYellow2 : FillYellow1)
{
	uiName = "Fill Yellow (500)";
	radius = 500;
};