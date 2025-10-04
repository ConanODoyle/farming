package Cosmetics
{
	function Player::mountImage(%obj, %img, %slot)
	{
		if (%obj.client.isDonator && isObject(%img.donatorImage) && %img.donatorImage.getID() != %img.getID())
		{
			return %obj.mountImage(%img.donatorImage, %slot);
		}
		return parent::mountImage(%obj, %img, %slot);
	}
};
activatePackage(Cosmetics);

function registerCosmetic(%inheritItem, %inheritImage, %model, %icon, %offset, %name)
{
	if (isObject(stripChars(%name, " ") @ "Item"))
	{
		error("    Already registered item " @ %name @ "! Skipping...");
		return;
	}
	%str = %str @ "datablock ItemData(" @ stripChars(%name, " ") @ "Item : " @ %inheritItem @ ") {";
	%str = %str @ "    iconName = \"Add-ons/Server_Farming/icons/" @ %icon @ "\";";
	%str = %str @ "    shapeFile = \"./" @ %model @ "\";";
	%str = %str @ "    uiName = \"" @ %name @ "\";";
	%str = %str @ "    image = \"" @ stripChars(%name, " ") @ "Image\";";
	%str = %str @ "}";

	%str = %str @ "datablock ShapeBaseImageData(" @ stripChars(%name, " ") @ "Image : " @ %inheritImage @ ") {";
	%str = %str @ "    shapeFile = \"./" @ %model @ "\";";
	%str = %str @ "    item = \"" @ stripChars(%name, " ") @ "Item\";";
	%str = %str @ "    offset = \"" @ %offset @ "Item\";";
	%str = %str @ "}";

	eval(%str);
}

registerCosmetic(WateringCatItem, WateringCatImage, "cat_black", "", "wateringCat", "Black Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_blackwhite", "", "wateringCat", "Black&White Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_white", "", "wateringCat", "White Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_orange", "", "wateringCat", "Orange Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_calico", "", "wateringCat", "Calico Cat");
registerCosmetic(WateringCatItem, WateringCatImage, "cat_gray", "", "wateringCat", "Gray Cat");

