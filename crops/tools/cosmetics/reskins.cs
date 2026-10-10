// DEBUG
// REMOVE LATER ------------------------------------------------------------

function Player::cattest(%pl)
{
    if (!%pl.client.isSuperAdmin)
    {
        return;
    }

    %pl.farmingadditem(trowelitem);
	%pl.farmingadditem(sickleitem);
    %pl.farmingadditem(wateringcatitem);
}

function Player::buxlol(%pl)
{
    if (!%pl.client.isSuperAdmin)
    {
        return;
    }

    %pl.farmingaddstackableitem(bux0item,200);
}

function Player::reskinHeld(%p, %reskin)
{
	talk("Tool:" SPC %p.tool[%p.currtool] SPC ": DataID:" SPC %p.tooldataid[%p.currtool] SPC ": Reskin:" SPC %reskin);
	talk("Result:" SPC %p.reskinItem(%p.tool[%p.currtool], %p.tooldataid[%p.currtool], %reskin));
}

function Player::unskinHeld(%p)
{
	talk("Tool:" SPC %p.tool[%p.currtool] SPC ": DataID:" SPC %p.tooldataid[%p.currtool]);
	talk("Result:" SPC %p.removeItemReskin(%p.tool[%p.currtool], %p.tooldataid[%p.currtool]));
}


// -----------------------------------------------------------------------


$Farming::ReskinList = "";
$Farming::ReskinnableTools = "";

function registerReskin(%inheritItem, %inheritImage, %itemmodel, %imagemodel, %icon, %offset, %name)
{
	%itemName = "Reskin__" @ stripChars(%name, " ") @ "Item";	
	%itemNameListing = %inheritItem @ "_" @ stripChars(%name, " ") @ "Item";
	
	if (isObject(%itemName))
	{
		if (strpos($Farming::ReskinList, %itemNameListing) == -1)
		{
			error("    WARNING: Registered item " @ %name @ " missing from faulty reskin list. Re-listing...");
			$Farming::ReskinList = ltrim($Farming::ReskinList TAB %inheritItem @ "_" @ stripChars(%name, " ") @ "Item");
			%inheritItem.makeReskinnable();
			return;
		}
		else
		{
			error("    Already registered item " @ %name @ "! Skipping...");
			return;
		}
	}

	if (strpos($Farming::ReskinList, %itemNameListing) >= 0)
	{
		error("    WARNING: Aborted registration of item " @ %name @ " due to faulty reskin list.");
		return;
	}

	if (%imagemodel $= "")
	{
		%imagemodel = %itemmodel;
	}

	%str = %str @ "datablock ItemData(Reskin__" @ stripChars(%name, " ") @ "Item : " @ %inheritItem @ ") {";
	%str = %str @ "    iconName = \"Add-ons/Server_Farming/icons/" @ %icon @ "\";";
	%str = %str @ "    shapeFile = \"Add-ons/Server_Farming/crops/tools/cosmetics/" @ %itemmodel @ ".dts\";";
	%str = %str @ "    uiName = \"" @ stripChars(%name, " ") @ "\";";
	%str = %str @ "    image = \"Reskin__" @ stripChars(%name, " ") @ "Image\";";
	%str = %str @ "    skinBase = " @ %inheritItem @";";
	%str = %str @ "};";

	%str = %str @ "datablock ShapeBaseImageData(Reskin__" @ stripChars(%name, " ") @ "Image : " @ %inheritImage @ ") {";
	%str = %str @ "    shapeFile = \"Add-ons/Server_Farming/crops/tools/cosmetics/" @ %imagemodel @ ".dts\";";
	%str = %str @ "    item =   \"" @ %inheritItem @ "\";";
	%str = %str @ "    offset = \"" @ %offset @ "\";";
	%str = %str @ "};";

	eval(%str);

	$Farming::ReskinList = ltrim($Farming::ReskinList TAB %inheritItem @ "_" @ stripChars(%name, " ") @ "Item");
	echo("	Registered " @ "Reskin__" @ stripChars(%name, " ") @ "Item for " @ %inheritItem);
	%inheritItem.makeReskinnable();
}

// player function cause idk how to get item holder from itemdatas
function Player::reskinItem(%pl, %item, %dataID, %reskin)
{
	if (!isObject(%item) || %dataID $= "")
	{
		return;
	}

	%foundTool = false;
	
	for (%i = 0; %i < %pl.getDatablock().maxTools; %i++)
	{
		%currDataID = %pl.toolDataID[%i];
		if (%currDataID $= %dataID)
		{
			%foundTool = true;
			%slot = %i;
			break;
		}
	}

	if (!%foundTool)
	{
		return;
	}

	if (!%item.ownsReskin(%reskin))
	{
		return;
	}

	%pl.farmingRemoveItem(%slot);
	%pl.farmingAddItem(%reskin, %dataID);

	return 1;
}

function Player::removeItemReskin(%pl, %item, %dataID)
{
	if (!isObject(%item) || %dataID $= "" || %item.skinBase $= "")
	{
		return;
	}

	%foundTool = false;
	
	for (%i = 0; %i < %pl.getDatablock().maxTools; %i++)
	{
		%currDataID = %pl.toolDataID[%i];
		if (%currDataID $= %dataID)
		{
			%foundTool = true;
			%slot = %i;
			break;
		}
	}

	if (!%foundTool)
	{
		return;
	}

	%pl.farmingRemoveItem(%slot);
	%pl.farmingAddItem(%item.skinBase, %dataID);

	return 1;
}

function ItemData::makeReskinnable(%item)
{
	if (strPos($Farming::ReskinnableTools, %item) == -1)
	{
		$Farming::ReskinnableTools = ltrim($Farming::ReskinnableTools TAB %item);
		echo("	Registered " @ %item @ " as a reskinnable tool");
	}
}

// Returns an item's reskin options separated by field
function ItemData::getReskinOptions(%item)
{
	if (!isObject(%item))
	{
		return;
	}

	%itemName = %item.getName();

	%isSkinnable = false;

	for (%i = 0; %i < getFieldCount($Farming::ReskinnableTools); %i++)
	{
		%tool = getField($Farming::ReskinnableTools, %i);
		if (%tool $= %itemName)
		{
			%isSkinnable = true;
			break;
		}
	}

	if (!%isSkinnable)
	{	
		return %isSkinnable;
	}

	for (%i = 0; %i < getFieldCount($Farming::ReskinList); %i++)
	{
		%cosmetic = getField($Farming::ReskinList, %i);
		%underscore = strstr(%cosmetic, "_");
		%cosmeticBase = getSubStr(%cosmetic, 0, %underscore);

		if (%cosmeticBase $= %itemName)
		{
			
			%cosmeticName = getSubStr(%cosmetic, %underscore + 1, strLen(%cosmetic) - %underscore);
			%cosmeticName = "Reskin__" @ %cosmeticName;
			%reskinOptions = %reskinOptions TAB %cosmeticName;
		}
	}									

	%reskinOptions = ltrim(%reskinOptions);

	return %reskinOptions;
}

function ItemData::getReskinCount(%item)
{
	if (!isObject(%item))
	{
		return;
	}

	if (strpos(%item.getReskinOptions(), "Reskin__") == -1)
	{
		return 0;
	}

	return getFieldCount(%item.getReskinOptions());
}

function ItemData::ownsReskin(%item, %reskin)
{
	if (!isObject(%item))
	{
		return 0;
	}

	if (%reskin.skinBase $= %item.getName())
	{
		return true;
	}

	return false;
}

registerReskin(WateringCatItem, WateringCatImage, "cat_black", "", 			"no_icon", "", "Black Cat");
registerReskin(WateringCatItem, WateringCatImage, "cat_blackwhite", "", 		"no_icon", "", "BlackWhite Cat");
registerReskin(WateringCatItem, WateringCatImage, "cat_white", "", 			"no_icon", "", "White Cat");
registerReskin(WateringCatItem, WateringCatImage, "cat_orange", "", 			"no_icon", "", "Orange Cat");
registerReskin(WateringCatItem, WateringCatImage, "cat_calico", "", 			"no_icon", "", "Calico Cat");
registerReskin(WateringCatItem, WateringCatImage, "cat_gray", "", 			"no_icon", "", "Gray Cat");

registerReskin(WateringCatItem, WateringCatImage, "cup", "", 					"no_icon", "", "Mug");

registerReskin(ClipperItem, ClipperImage, 		"scissors", "scissorsopen",	"no_icon", "", "Scissors");

registerReskin(TrowelItem, TrowelImage, 			"entrenchingtool", "",		"no_icon", "", "Entrenching Tool");

registerReskin(SickleItem, SickleImage, 			"communismsickle", "",		"no_icon", "", "Proletariat Sickle");

registerReskin(hoeItem, hoeImage, 				"snowplow", "",				"no_icon", "", "Snowplow");