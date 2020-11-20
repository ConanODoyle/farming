// Load dts shapes and merge animations 
datablock TSShapeConstructor(RealCatDts)
{
	baseShape  = "./cat.dts";
	sequence0  = "./cat_root.dsq root";

	sequence1  = "./cat_root.dsq run";
	sequence2  = "./cat_root.dsq walk";
	sequence3  = "./cat_root.dsq back";
	sequence4  = "./cat_root.dsq side";

	sequence5  = "./cat_root.dsq crouch";
	sequence6  = "./cat_root.dsq crouchRun";
	sequence7  = "./cat_root.dsq crouchBack";
	sequence8  = "./cat_root.dsq crouchSide";

	sequence9  = "./cat_root.dsq look";
	sequence10 = "./cat_root.dsq headside";
	sequence11 = "./cat_root.dsq headUp";

	sequence12 = "./cat_root.dsq jump";
	sequence13 = "./cat_root.dsq standjump";
	sequence14 = "./cat_root.dsq fall";
	sequence15 = "./cat_root.dsq land";

	sequence16 = "./cat_root.dsq armAttack";
	sequence17 = "./cat_root.dsq armReadyLeft";
	sequence18 = "./cat_root.dsq armReadyRight";
	sequence19 = "./cat_root.dsq armReadyBoth";
	sequence20 = "./cat_root.dsq spearready";  
	sequence21 = "./cat_root.dsq spearThrow";

	sequence22 = "./cat_root.dsq talk";  

	sequence23 = "./cat_root.dsq death1"; 
	
	sequence24 = "./cat_root.dsq shiftUp";
	sequence25 = "./cat_root.dsq shiftDown";
	sequence26 = "./cat_root.dsq shiftAway";
	sequence27 = "./cat_root.dsq shiftTo";
	sequence28 = "./cat_root.dsq shiftLeft";
	sequence29 = "./cat_root.dsq shiftRight";
	sequence30 = "./cat_root.dsq rotCW";
	sequence31 = "./cat_root.dsq rotCCW";

	sequence32 = "./cat_root.dsq undo";
	sequence33 = "./cat_root.dsq plant";

	sequence34 = "./cat_root.dsq sit";

	sequence35 = "./cat_root.dsq wrench";

	sequence36 = "./cat_root.dsq activate";
	sequence37 = "./cat_root.dsq activate2";

	sequence38 = "./cat_root.dsq leftrecoil";

//   sequence39 = "./cat_flap.dsq fly";
//   sequence40 = "./cat_idle1.dsq Idle";

//   sequence40 = "./cat_sit.dsq sit2";
//   sequence41 = "./cat_sit3.dsq sit3";
//   sequence42 = "./cat_sleep.dsq sleep";
//   sequence43 = "./cat_stand.dsq stand";
//   sequence43 = "./cat_rest.dsq rest";
//   sequence44 = "./cat_boost.dsq boost";
};

datablock PlayerData(RealCatArmor : PlayerStandardArmor)
{
	renderFirstPerson = false;
//   emap = false;
   
//   className = Armor;
	shapeFile = "./cat.dts";
	cameraMaxDist = 8;
	cameraTilt = 0.261;//0.174 * 2.5; //~25 degrees
	cameraVerticalOffset = 2.3;
//   computeCRC = false;

	cameraDefaultFov = 90.0;
	cameraMinFov = 5.0;
	cameraMaxFov = 120.0;

   //debrisShapeName = "~/data/shapes/player/debris_player.dts";
   //debris = horseDebris;

	aiAvoidThis = true;

	minLookAngle = -1.5708;
	maxLookAngle = 1.5708;
	maxFreelookAngle = 2.0;

//   mass = 80;
//   drag = 0.028;          //0.5
//   density = 0.7;     //0.1
	maxDamage = 100;     //240
//   maxEnergy =  130;
//   repairRate = 0.33;
//  airControl = 0.25;

//   rechargeRate = 0.4;

	runForce = 80 * 80;  //60 * 80
	runEnergyDrain = 0;
	minRunEnergy = 0;
	maxForwardSpeed = 8;
	maxBackwardSpeed = 6;
	maxSideSpeed = 7;

	maxForwardCrouchSpeed = 4;
	maxBackwardCrouchSpeed = 3.5;
	maxSideCrouchSpeed = 4;

//   maxForwardProneSpeed = 0;
//   maxBackwardProneSpeed = 0;
//   maxSideProneSpeed = 0;

	maxForwardWalkSpeed = 5;
	maxBackwardWalkSpeed = 5;
	maxSideWalkSpeed = 5;

	maxUnderwaterForwardSpeed = 4;
	maxUnderwaterBackwardSpeed = 3.5;
	maxUnderwaterSideSpeed = 3.5;

	jumpForce = 15 * 80; //20 * 80;
	jumpEnergyDrain = 0;
	minJumpEnergy = 0;
	jumpDelay = 0;

	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;

	minImpactSpeed = 250;        //250
	speedDamageScale = 1;        //3.8

	boundingBox          = vectorScale("5 5 4.5", 1);    //4.25 4.25 1.9
	crouchBoundingBox    = vectorScale("5 5 4.5", 1);    //4.25 4.25 1.4

//   pickupRadius = 0.75;
   
   // Foot Prints
   //decalData   = SwiftDragonFootprint;
   //decalOffset = 0.25;

	jetEmitter = "";
	jetGroundEmitter = "";
	jetGroundDistance = 4;

   //footPuffEmitter = LightPuffEmitter;
//   footPuffNumParts = 10;
//   footPuffRadius = 0.25;

   //dustEmitter = LiftoffDustEmitter;

//   splash = PlayerSplash;
//   splashVelocity = 4.0;
//   splashAngle = 67.0;
//   splashFreqMod = 300.0;
//   splashVelEpsilon = 0.60;
//   bubbleEmitTime = 0.1;
//   splashEmitter[0] = PlayerFoamDropletsEmitter;
//   splashEmitter[1] = PlayerFoamEmitter;
//   splashEmitter[2] = PlayerBubbleEmitter;
//   mediumSplashSoundVelocity = 10.0;   
//   hardSplashSoundVelocity = 20.0;   
//   exitSplashSoundVelocity = 5.0;

   // Controls over slope of runnable/jumpable surfaces
//   runSurfaceAngle  = 85;
//   jumpSurfaceAngle = 90;

//   minJumpSpeed = 20;
//   maxJumpSpeed = 30;

//   horizMaxSpeed = 250;
//   horizResistSpeed = 180;
//   horizResistFactor = 0.1;

//   upMaxSpeed = 200;
//   upResistSpeed = 50;
//   upResistFactor = 0.1;
   
//   footstepSplashHeight = 0.35;

   //NOTE:  some sounds commented out until wav's are available

	JumpSound           = "";

   // Footstep Sounds
//   FootSoftSound        = SwiftDragonFootFallSound;
//   FootHardSound        = SwiftDragonFootFallSound;
//   FootMetalSound       = SwiftDragonFootFallSound;
//   FootSnowSound        = SwiftDragonFootFallSound;
//   FootShallowSound     = SwiftDragonFootFallSound;
//   FootWadingSound      = SwiftDragonFootFallSound;
//   FootUnderwaterSound  = SwiftDragonFootFallSound;
   //FootBubblesSound     = FootLightBubblesSound;
   //movingBubblesSound   = ArmorMoveBubblesSound;
   //waterBreathSound     = WaterBreathMaleSound;

   //impactSoftSound      = ImpactLightSoftSound;
   //impactHardSound      = ImpactLightHardSound;
   //impactMetalSound     = ImpactLightMetalSound;
   //impactSnowSound      = ImpactLightSnowSound;
   
//   impactWaterEasy      = Splash1Sound;
//   impactWaterMedium    = Splash1Sound;
//   impactWaterHard      = Splash1Sound;
   
//   groundImpactMinSpeed    = 10.0;
//   groundImpactShakeFreq   = "4.0 4.0 4.0";
//  groundImpactShakeAmp    = "1.0 1.0 1.0";
//   groundImpactShakeDuration = 0.8;
//   groundImpactShakeFalloff = 10.0;
   
   //exitingWater         = ExitingWaterLightSound;

   // Inventory Items
	maxItems   = 10;    //total number of bricks you can carry
	maxWeapons = 5;     //this will be controlled by mini-game code
	maxTools = 5;
	
	uiName = "Cat.";
	rideable = false;
	lookUpLimit = 0.6;
	lookDownLimit = 0.1;

	canRide = false;
	showEnergyBar = false;
	paintable = true;

//  brickImage = horseBrickImage;   //the imageData to use for brick deployment

	numMountPoints = 0;
//   mountThread[0] = "root";
//   mountNode[0] = 2;

//  useCustomPainEffects = true;
//  PainHighImage = "";
//  PainMidImage  = "";
//  PainLowImage  = "";
//  painSound     = "";
//  deathSound    = "";
};

