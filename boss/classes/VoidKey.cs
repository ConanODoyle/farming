//-------------//
// Void Light: //
//-------------//

datablock fxLightData(VoidLight)
{
	//------------//
	// Rendering: //
	//------------//
	
	lightOn = true;
	
	radius = 10.0;
	brightness = -5.0;
	color = "1.0 1.0 1.0";
	
	flareOn = true;
	
	flareBitmap = $Harvester::Root @ "/resources/shapes/darkCorona";
	nearSize = 1.5;
	farSize = 0.75;
	linkFlare = true;
	linkFlareSize = false;
	blendMode = 1;
	
	//-------------//
	// Properties: //
	//-------------//
	
	animRadius = true;
	lerpRadius = true;
	minRadius = 5.0;
	maxRadius = 10.0;
	radiusTime = 4.0;
	radiusKeys = "AZA";
};