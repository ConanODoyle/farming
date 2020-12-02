// jeep_tire.cs

datablock WheeledVehicleTire(duoJeepTire)
{
    // Tires act as springs and generate lateral and longitudinal
    // forces to move the vehicle. These distortion/spring forces
    // are what convert wheel angular velocity into forces that
    // act on the rigid body.
    shapeFile = "./bigTire.dts";

    mass = 10;
    radius = 1;
    staticFriction = 0.5;
    kineticFriction = 4;
    restitution = 0.5;

    // Spring that generates lateral tire forces
    lateralForce = 18000;
    lateralDamping = 4000;
    lateralRelaxation = 0.01;

    // Spring that generates longitudinal tire forces
    longitudinalForce = 14000;
    longitudinalDamping = 2000;
    longitudinalRelaxation = 0.01;
};