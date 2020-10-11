
//Exec plant info
echo("");
echo("  --Loading plant data");
exec("./plants/plantDatablocks.cs");
exec("./seeds/seedDatablocks.cs");
exec("./crops/cropDatablocks.cs");

exec("./plants/plantDataFunctions.cs");

//Exec tools

echo("");
echo("  --Loading farming data");
exec("./sprinklers/sprinklers.cs");
exec("./farm/datablocks.cs");

//Exec scripts
exec("./plantUtils.cs");
exec("./growth.cs");
exec("./water.cs");
exec("./harvest.cs");
exec("./shinyPlants.cs");
exec("./weeds.cs");

exec("./tools.cs");