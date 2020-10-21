function GameConnection::hasLicense(%cl, %type)
{
	%type = strLwr(trim(%type));
	if (%type $= "")
	{
		return 0;
	}
	if (getSubStr(%type, strLen(%type) - 4, 4) $= "seed")
	{
		%type = getSubStr(%type, strLen(%type) - 4);
	}

	%licenseList = " " @ strLwr($Pref::Farming::License[%cl.bl_id]) @ " ";

	return strPos(%licenseList, " " @ %type @ " ") >= 0;
}

function GameConnection::buyLicense(%cl, %type)
{
	%type = strLwr(trim(%type));
	if (%cl.hasLicense(%type) || %type $= "")
	{
		return 1;
	}
	if (getSubStr(%type, strLen(%type) - 4, 4) $= "seed")
	{
		%type = getSubStr(%type, strLen(%type) - 4);
	}

	%price = getPlantData(%type, "licenseCost");
	if (%price <= 0)
	{
		return 2;
	}

	if (%cl.farmingExperience < %price)
	{
		return 3;
	}
	else
	{
		%cl.farmingExperience = %cl.farmingExperience - %price | 0;
		%Pref::Farming::License[%cl.bl_id] = trim($Pref::Farming::License[%cl.bl_id] SPC %type);
		return 0;
	}
}

function getLicenseCost(%type)
{
	%type = strLwr(trim(%type));
	if (getSubStr(%type, strLen(%type) - 4, 4) $= "seed")
	{
		%type = getSubStr(%type, strLen(%type) - 4);
	}
	
	%price = getPlantData(%type, "licenseCost");
	return %price + 0;
}