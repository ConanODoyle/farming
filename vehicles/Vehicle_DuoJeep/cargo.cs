datablock WheeledVehicleData(duoCargoJeepVehicle : duoStandardJeepVehicle)
{
    shapeFile = "./cargoJeep.dts";

    uiName = $AddOn__Server_Farming ? "Cargo Jeep - $4000" : "Cargo Jeep";

    numMountPoints = 3;
    mountThread[0] = "sit";
    mountThread[1] = "sit";
    mountThread[2] = "root";

    cost = 4000;
};