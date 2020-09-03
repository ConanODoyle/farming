$Game::Item::PopTime = 120000;
$stackableItemExtraPopTime = 60000;

$startingAmount = 200;
$betaBonus = 100;

// Score grant tracking
if(isFile("config/Farming/scoreGrant.cs"))
	exec("config/Farming/scoreGrant.cs");

// Vehicle Costs
JeepVehicle.maxWheelSpeed = 20;
JeepSpring.force = 2000;
JeepSpring.damping = 5000;
HorseArmor.maxForwardSpeed = 9;
HorseArmor.maxForwardCrouchSpeed = 9;
