function totalWater(%type, %startStage)
{
  %ticks = $Farming::Crops::plantData_[%type, %startStage, "numGrowTicks"];
  %time = $Farming::Crops::plantData_[%type, %startStage, "timePerTick"];
  %water = $Farming::Crops::plantData_[%type, %startStage, "waterPerTick"];
  if (%time <= 0)
  {
    return 0;
  }
  else
  {
    return %water * %ticks + totalWater(%type, %startStage + 1);
  }
}

function totalTime(%type, %startStage)
{
  %ticks = $Farming::Crops::plantData_[%type, %startStage, "numGrowTicks"];
  %time = $Farming::Crops::plantData_[%type, %startStage, "timePerTick"];
  if (%time <= 0)
  {
    return 0;
  }
  else
  {
    return %time * %ticks + totalTime(%type, %startStage + 1);
  }
}