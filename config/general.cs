$Game::Item::PopTime = 180000;

$startingAmount = 200;
$betaBonus = 100;

// Score grant tracking
if(isFile("config/Farming/scoreGrant.cs"))
	exec("config/Farming/scoreGrant.cs");

// Vehicle Costs
JeepVehicle.maxWheelSpeed = 22;
JeepSpring.force = 2000;
JeepSpring.damping = 5000;
HorseArmor.maxForwardSpeed = 10;
HorseArmor.maxForwardCrouchSpeed = 10;
