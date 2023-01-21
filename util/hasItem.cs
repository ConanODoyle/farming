function Player::getItemsInSlots(%this, %item)
{
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
	return getWordCount(%this.getItemsInSlots(%item));
}

function Player::hasItem(%this, %item)
{
	return %this.getItemCount(%item) > 0;
}