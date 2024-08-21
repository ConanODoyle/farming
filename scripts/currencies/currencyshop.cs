

function Player::hasAmountCurrency(%pl, %amountNeeded)
{
	for (%i = 0; %i < getFieldCount(%amountNeeded); %i++)
	{
		%field = getField(%amountNeeded, %i);
		%type = getWord(%field, 0);
		%amount = getWord(%field, 1);

		if (%pl.getStackableItemTotal(%type) < %amount)
		{
			return 0;
		}
	}
	return 1;
}