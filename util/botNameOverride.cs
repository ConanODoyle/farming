package BotNameShapename
{
	function AIPlayer::setBotName(%bot, %name, %cl)
	{
		%ret = parent::setBotName(%bot, %name, %cl);
		%bot.setShapeName(%name, 8564862);
		%bot.setShapeNameDistance(20);
	}
};
activatePackage(BotNameShapename);