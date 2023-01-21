function Player::getItemsInSlots(%this, %item)
{
	if(%item + 0 == 0)
	{
		%item = %item.getID();
	}
	
	%slots = "";
	
	for(%i = 0; %i < %this.getDataBlock().maxTools ; %i++)
	{
		if(!isObject(%this.tool[%i]))
		{
			continue;
		}
		
		%tool = %this.tool[%i].getID();
		
		if(%tool != %item)
		{
			continue;
		}

		%slots = %slots SPC %i;
	}
	
	return trim(%slots);
}

function Player::getItemCount(%this, %item)
{
	if(%item + 0 == 0)
	{
		%item = %item.getID();
	}
	
	return getWordCount(%this.getItemsInSlots(%item));
}

function Player::hasItem(%this, %item)
{
	if(%item + 0 == 0)
	{
		%item = %item.getID();
	}
	
	return %this.getItemCount(%item) > 0;
}