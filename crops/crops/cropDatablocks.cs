exec("./potato.cs");
exec("./carrot.cs");
exec("./tomato.cs");
exec("./corn.cs");
exec("./cabbage.cs");
exec("./onion.cs");
exec("./blueberry.cs");
exec("./turnip.cs");
exec("./appleTree.cs");
exec("./mangoTree.cs");

function foodLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<color:ffff00>-Basket " @ %obj.currTool + 1 @ "- <br>" @ %type @ "<color:ffffff>: " @ %count @ " ", 1);
	}
}