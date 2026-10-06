$Farming::CosmeticList = "";
$Farming::ReskinnableTools = "";

exec("./toolExchanger.cs");

package Cosmetics
{
	function Player::mountImage(%obj, %img, %slot)
	{
		if (isObject(%img) && %img.hasSkin) //assumed that the item will have a data id
		{
			%tool = %obj.currTool;
			%skin = getDataIDArrayTagValue(%obj.toolDataID[%obj.tool], "skin");
			%obj.unmountImage(%slot);
			return %obj.mountImage(%img, %slot, %skin);
		}
		return parent::mountImage(%obj, %img, %slot);
	}
};
activatePackage(Cosmetics);

function registerCosmetic(%inheritItem, %inheritImage, %itemmodel, %imagemodel, %icon, %offset, %name)
{
	%itemName = "Cosmetic__" @ stripChars(%name, " ") @ "Item";	
	%itemNameListing = %inheritItem @ "_" @ stripChars(%name, " ") @ "Item";
	
	if (isObject(%itemName))
	{
		if (strpos($Farming::CosmeticList, %itemNameListing) == -1)
		{
			error("    WARNING: Registered item " @ %name @ " missing from faulty cosmetic list. Re-listing...");
			$Farming::CosmeticList = ltrim($Farming::CosmeticList TAB %inheritItem @ "_" @ stripChars(%name, " ") @ "Item");
			%inheritItem.makeReskinnable();
			return;
		}
		else
		{
			error("    Already registered item " @ %name @ "! Skipping...");
			return;
		}
	}

	if (strpos($Farming::CosmeticList, %itemNameListing) >= 0)
	{
		error("    WARNING: Aborted registration of item " @ %name @ " due to faulty cosmetic list.");
		talk("    WARNING: Aborted registration of item " @ %name @ " due to faulty cosmetic list.");
		return;
	}

	if (%imagemodel $= "")
	{
		%imagemodel = %itemmodel;
	}

	%str = %str @ "datablock ItemData(Cosmetic__" @ stripChars(%name, " ") @ "Item : " @ %inheritItem @ ") {";
	%str = %str @ "    iconName = \"Add-ons/Server_Farming/icons/" @ %icon @ "\";";
	%str = %str @ "    shapeFile = \"Add-ons/Server_Farming/crops/tools/cosmetics/" @ %itemmodel @ ".dts\";";
	%str = %str @ "    uiName = \"" @ %name @ "\";";
	%str = %str @ "    image = \"Cosmetic__" @ stripChars(%name, " ") @ "Image\";";
	%str = %str @ "};";

	%str = %str @ "datablock ShapeBaseImageData(Cosmetic__" @ stripChars(%name, " ") @ "Image : " @ %inheritImage @ ") {";
	%str = %str @ "    shapeFile = \"Add-ons/Server_Farming/crops/tools/cosmetics/" @ %imagemodel @ ".dts\";";
	%str = %str @ "    item = \"Cosmetic__" @ stripChars(%name, " ") @ "Item\";";
	%str = %str @ "    offset = \"" @ %offset @ "\";";
	%str = %str @ "};";

	eval(%str);

	$Farming::CosmeticList = ltrim($Farming::CosmeticList TAB %inheritItem @ "_" @ stripChars(%name, " ") @ "Item");
	echo("	Registered " @ "Cosmetic__" @ stripChars(%name, " ") @ "Item for " @ %inheritItem);
	%inheritItem.makeReskinnable();
}

// tbh i dont really know why im doing string arrays not that i know any alternatives

function ItemData::makeReskinnable(%item)
{
	if (strPos($Farming::ReskinnableTools, %item) == -1)
	{
		$Farming::ReskinnableTools = ltrim($Farming::ReskinnableTools TAB %item);
		echo("	Registered " @ %item @ " as a reskinnable tool");
	}
}

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

	for (%i = 0; %i < getFieldCount($Farming::CosmeticList); %i++)
	{
		%cosmetic = getField($Farming::CosmeticList, %i);
		%underscore = strstr(%cosmetic, "_");
		%cosmeticBase = getSubStr(%cosmetic, 0, %underscore);

		if (%cosmeticBase $= %itemName)
		{
			%cosmeticName = getSubStr(%cosmetic, %underscore + 1, strLen(%cosmetic) - %underscore);
			%reskinOptions = %reskinOptions TAB %cosmeticName;
		}
	}									

	%reskinOptions = ltrim(%reskinOptions);

	return %reskinOptions;
}

function ItemData::getNumReskins(%item)
{
	if (!isObject(%item))
	{
		return;
	}

	return getFieldCount(%item.getReskinOptions());
}

registerCosmetic(WateringCatItem, WateringCatImage, "cat_black", "", 			"no_icon", "", "Black Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_blackwhite", "", 		"no_icon", "", "BlackWhite Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_white", "", 			"no_icon", "", "White Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_orange", "", 			"no_icon", "", "Orange Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_calico", "", 			"no_icon", "", "Calico Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_gray", "", 			"no_icon", "", "Gray Cat");

registerCosmetic(WateringCatItem, WateringCatImage, "cup", "", 					"no_icon", "", "Mug");
MugImage.hasSkin = 1;

registerCosmetic(ClipperItem, ClipperImage, 		"scissors", "scissorsopen",	"no_icon", "", "Scissors");

registerCosmetic(TrowelItem, TrowelImage, 			"entrenchingtool", "",		"no_icon", "", "Entrenching Tool");

registerCosmetic(SickleItem, SickleImage, 			"communismsickle", "",		"no_icon", "", "Proletariat Sickle");

registerCosmetic(hoeItem, hoeImage, 				"snowplow", "",				"no_icon", "", "Snowplow");