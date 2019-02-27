function isProduce(%cropType)
{
	if (isObject(%cropType))
	{
		%cropType = %cropType.stackType;
	}

	for (%i = 0; %i < $ProduceCount; %i++)
	{
		%produce = getWord($ProduceList_[%i], 0);
		if (%cropType $= %produce)
		{
			return = 1;
		}
	}
	return 0;
}