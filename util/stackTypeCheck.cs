function isProduce(%cropType)
{
	if (isObject(%cropType))
	{
		%cropType = %cropType.stackType;
	}

	if ($ProduceCheck_[%cropType])
	{
		return 1;
	}

	for (%i = 0; %i < $ProduceCount; %i++)
	{
		%produce = getWord($ProduceList_[%i], 0);
		if (%cropType $= %produce)
		{
			$ProduceCheck_[%cropType] = 1;
			return 1;
		}
	}
	return 0;
}