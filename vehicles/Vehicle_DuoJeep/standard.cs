datablock WheeledVehicleData(duoStandardJeepVehicle)
{
    category = "Vehicles";
    displayName = " ";
    shapeFile = "./standardJeep.dts";
    emap = true;
    minMountDist = 3;

    numMountPoints = 3;
    mountThread[0] = "sit";
    mountThread[1] = "sit";
    mountThread[2] = "sit";

    maxDamage = 200.00;
    destroyedLevel = 200.00;
    speedDamageScale = 1.04;
    collDamageThresholdVel = 20.0;
    collDamageMultiplier   = 0.02;

    massCenter = "0 0 0";

    maxSteeringAngle = 0.6981317; // Maximum steering angle, should match animation
    integration = 4; // Force integration time: TickSec/Rate
    tireEmitter = VehicleTireEmitter; // All the tires use the same dust emitter

    // 3rd person camera settings
    cameraRoll = false; // Roll the camera with the vehicle
    cameraMaxDist = 6.5; // Far distance from vehicle
    cameraOffset = 4; // Vertical offset from camera mount point
    cameraLag = 0.0; // Velocity lag of camera
    cameraDecay = 0.75; // Decay per sec. rate of velocity lag
    cameraTilt = 0.2;
    collisionTol = 0.1; // Collision distance tolerance
    contactTol = 0.1;

    useEyePoint = false;

    defaultTire = duoJeepTire;
    defaultSpring = duoJeepSpring;

    numWheels = 4;

    // Rigid Body
    mass = 300;
    density = 5.0;
    drag = 1.6;
    bodyFriction = 0.6;
    bodyRestitution = 0.6;
    minImpactSpeed = 10; // Impacts over this invoke the script callback
    softImpactSpeed = 10; // Play SoftImpact Sound
    hardImpactSpeed = 15; // Play HardImpact Sound
    groundImpactMinSpeed = 10.0;

    // Engine
    engineTorque = 12000; // Engine power
    engineBrake = 2000; // Braking when throttle is 0
    brakeTorque = 50000; // When brakes are applied
    maxWheelSpeed = $AddOn__Server_Farming == 1 ? 22 : 30; // Engine scale by current speed / max speed

    rollForce = 900;
    yawForce = 600;
    pitchForce = 1000;
    rotationalDrag = 0.2;

    // Advanced Steering
    steeringAutoReturn = true;
    steeringAutoReturnRate = 0.9;
    steeringAutoReturnMaxSpeed = 10;
    steeringUseStrafeSteering = true;
    steeringStrafeSteeringRate = 0.1;

    // Energy
    maxEnergy = 100;
    jetForce = 3000;
    minJetEnergy = 30;
    jetEnergyDrain = 2;

    splash = vehicleSplash;
    splashVelocity = 4.0;
    splashAngle = 67.0;
    splashFreqMod = 300.0;
    splashVelEpsilon = 0.60;
    bubbleEmitTime = 1.4;
    splashEmitter[0] = vehicleFoamDropletsEmitter;
    splashEmitter[1] = vehicleFoamEmitter;
    splashEmitter[2] = vehicleBubbleEmitter;
    mediumSplashSoundVelocity = 10.0;
    hardSplashSoundVelocity = 20.0;
    exitSplashSoundVelocity = 5.0;

    softImpactSound = slowImpactSound;
    hardImpactSound = fastImpactSound;

    justcollided = 0;

    uiName = $AddOn__Server_Farming ? "Standard Jeep - $800" : "Standard Jeep";
    rideable = true;
    lookUpLimit = 0.65;
    lookDownLimit = 0.45;

    paintable = true;

    damageEmitter[0] = VehicleBurnEmitter;
    damageEmitterOffset[0] = "0.0 0.0 0.0 ";
    damageLevelTolerance[0] = 0.99;

    damageEmitter[1] = VehicleBurnEmitter;
    damageEmitterOffset[1] = "0.0 0.0 0.0 ";
    damageLevelTolerance[1] = 1.0;

    numDmgEmitterAreas = 1;

    initialExplosionProjectile = vehicleExplosionProjectile;
    initialExplosionOffset = 0; //offset only uses a z value for now

    burnTime = 4000;

    finalExplosionProjectile = vehicleFinalExplosionProjectile;
    finalExplosionOffset = 0.5; //offset only uses a z value for now

    minRunOverSpeed    = 4; //how fast you need to be going to run someone over (do damage)
    runOverDamageScale = 8; //when you run over someone, speed * runoverdamagescale = damage amt
    runOverPushScale   = 1.2; //how hard a person you're running over gets pushed

    //protection for passengers
    protectPassengersBurn   = false; //protect passengers from the burning effect of explosions?
    protectPassengersRadius = true; //protect passengers from radius damage (explosions) ?
    protectPassengersDirect = false; //protect passengers from direct damage (bullets) ?

    cost = 800;
};