$count = 0;
if (isObject($BotDialogue1))
{
	for (%i = 0; %i < 30; %i++)
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
	message[1] = "Powered water pumps to fill tanks, and indoor lights that allow you to grow stuff indoors!";
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
	message[1] = "Thankfully our local toolsmith can help repair them! He can be found in the workshop in town.";
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

$BotDialogue[$count++] = new ScriptObject(ToolDialogue3)
{
	messageCount = 2;
	message[0] = "Have you used a trowel on buried crops? They drop more produce when you use them to harvest!";
	messageTimeout[0] = 2;
	message[1] = "On the other hand, hoes let you harvest a bunch of belowground crops at once. Fast!";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(ToolDialogue4)
{
	messageCount = 2;
	message[0] = "Clippers are a really handy tool that boosts your harvest for aboveground crops!";
	messageTimeout[0] = 2;
	message[1] = "But if you want to harvest those fields of wheat faster, use a sickle instead!";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(ToolDialogue5)
{
	messageCount = 2;
	message[0] = "Tree Clippers let you boost the harvest from trees significantly. Big profits!";
	messageTimeout[0] = 2;
	message[1] = "You can also use them to prune apple trees before harvest, increasing yield!";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(ToolDialogue6)
{
	messageCount = 2;
	message[0] = "Watering cans come in 3 tiers, and are great for watering dirt! On the other hand, hoses are great for tanks!";
	messageTimeout[0] = 2;
	message[1] = "Check the tool seller regularly to see what he's got!";
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

$BotDialogue[$count++] = new ScriptObject(NutrientDialogue)
{
	messageCount = 2;
	message[0] = "Some crops need nutrients - you can figure out what they need by using an Organic Analyzer on them.";
	messageTimeout[0] = 2;
	message[1] = "Phosphate can be mined from some areas, and nitrogen comes from compost made from Compost Bins.";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(CompostBinDialogue1)
{
	messageCount = 2;
	message[0] = "Buy your compost bins here! To make compost, you just toss in any full basket of crops.";
	messageTimeout[0] = 2;
	message[1] = "Over time, the produce will decompose and generate compost!";
	messageTimeout[1] = 2;
};

$BotDialogue[$count++] = new ScriptObject(CompostBinDialogue2)
{
	messageCount = 2;
	message[0] = "Big compost bins store more crops and compost, on top of generating compost faster.";
	messageTimeout[0] = 2;
	message[1] = "Quite useful when you want to save storage space on compost!";
	messageTimeout[1] = 2;
};