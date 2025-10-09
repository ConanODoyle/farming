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
	if (isObject("Cosmetic__" @ stripChars(%name, " ") @ "Item"))
	{
		error("    Already registered item " @ %name @ "! Skipping...");
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
}

registerCosmetic(WateringCatItem, WateringCatImage, "cat_black", "", 			"no_icon", "", "Black Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_blackwhite", "", 		"no_icon", "", "Black&White Cat");
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