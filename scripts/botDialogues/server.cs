
$count = 0;
if (isObject($BotDialogue1))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($BotDialogue[%i]))
		{
			$BotDialogue[%i].delete();
		}
	}
}

$BotDialogue[$count++] = new ScriptObject(LotMovingDialogue)
{
	messageCount = 2;
	message[0] = "With the new lots, you can now move, or load your unloaded lot in any empty spot!";
	messageTimeout[0] = 2;
	message[1] = "Talk to the Lot Manager in City Hall for help!";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(PowerSystemDialogue1)
{
	messageCount = 2;
	message[0] = "A shipment of electrical equipment has arrived lately... I hear it'll help a lot with optimizing your farm.";
	messageTimeout[0] = 2;
	message[1] = "Powered indoor lights and water pumps allow you to grow stuff indoors!";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(PowerSystemDialogue2)
{
	messageCount = 2;
	message[0] = "To use electrical equipment, hook them up to a Power Control Box.";
	messageTimeout[0] = 2;
	message[1] = "Then hook up a generator or some charged batteries to power them! Don't forget to put in some coal!";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(PowerSystemDialogue3)
{
	messageCount = 2;
	message[0] = "Use Electrical Cable to connect electrical devices. You can get it free in town.";
	messageTimeout[0] = 2;
	message[1] = "Use it on an electrical brick, then on the power control box to connect the two.";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(ToolDialogue1)
{
	messageCount = 2;
	message[0] = "Ever since we moved to this new area, we've been getting worse tools... they keep breaking down!";
	messageTimeout[0] = 2;
	message[1] = "Thankfully our local toolsmith can help repair them! He can be found in the workshop in town";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(ToolDialogue2)
{
	messageCount = 2;
	message[0] = "We've been occasionally getting tools that inexplicably have microchips embedded in them.";
	messageTimeout[0] = 2;
	message[1] = "They seem to keep track of something... I wonder what? Our crops, maybe?";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(OrganicAnalyzerDialogue)
{
	messageCount = 2;
	message[0] = "Have you seen this tool before? It's so cool! Tells you everything about the things you look at!";
	messageTimeout[0] = 2;
	message[1] = "Doesn't seem to work on anything inorganic though, so I only gave it a 2 out of 5 stars.";
	messageTimeout[1] = 2;
};