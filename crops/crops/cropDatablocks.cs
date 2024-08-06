HammerItem.friction = 0.2;
exec("./potato.cs");
exec("./carrot.cs");
exec("./tomato.cs");
exec("./corn.cs");
exec("./wheat.cs");
exec("./cabbage.cs");
exec("./onion.cs");
exec("./blueberry.cs");
exec("./turnip.cs");
exec("./portobello.cs");
exec("./appleTree.cs");
exec("./mangoTree.cs");

exec("./chili.cs");
exec("./cactus.cs");
exec("./watermelon.cs");
exec("./peachTree.cs");
exec("./dateTree.cs");

exec("./weed.cs");
HammerItem.friction = 0.6;

function foodLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl) && !%cl.isInCenterprintMenu)
	{
		%cl.centerprint("<just:right>\c3-Basket " @ %obj.currTool + 1 @ "- \n\c3" @ %type @ "\c6: " @ %count @ " ", 1);
	}
}
