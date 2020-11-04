if (!isObject($BasicDialogueSet))
{
	$BasicDialogueSet = new SimSet(BasicDialogueSet);
}
$BasicDialogueSet.deleteAll();

$obj = new ScriptObject(BusInfoDialogue)
{
	messageCount = 3;
	message[0] = "Bus stops are really cool! They're free, and bring you around really fast.";
	messageTimeout[0] = 2;
	message[1] = "The bus line info shows all the stops available to travel to when you click it!";
	messageTimeout[1] = 3;
	message[2] = "That said, I haven't seen a bus in all the time I've been standing here...";
	messageTimeout[2] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(LotMovingDialogue)
{
	messageCount = 2;
	message[0] = "With the new lots, you can now move, or load your unloaded lot in any empty spot!";
	messageTimeout[0] = 2;
	message[1] = "Talk to the Lot Manager in City Hall for help!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PowerSystemDialogue1)
{
	messageCount = 2;
	message[0] = "A shipment of electrical equipment has arrived lately... I hear it'll help a lot with optimizing your farm.";
	messageTimeout[0] = 2;
	message[1] = "Powered water pumps to fill tanks, and indoor lights that allow you to grow stuff indoors!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PowerSystemDialogue2)
{
	messageCount = 2;
	message[0] = "To use electrical equipment, hook them up to a Power Control Box.";
	messageTimeout[0] = 2;
	message[1] = "Then hook up a generator or some charged batteries to power them! Don't forget to put in some coal!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PowerSystemDialogue3)
{
	messageCount = 2;
	message[0] = "Use Electrical Cable to connect electrical devices. You can get it free in town.";
	messageTimeout[0] = 2;
	message[1] = "Use it on an electrical brick, then on the power control box to connect the two.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue1)
{
	messageCount = 2;
	message[0] = "Ever since we moved to this new area, we've been getting worse tools... they keep breaking down!";
	messageTimeout[0] = 2;
	message[1] = "Thankfully our local toolsmith can help repair them! He can be found in the workshop in town.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue2)
{
	messageCount = 2;
	message[0] = "We've been occasionally getting tools that inexplicably have microchips embedded in them.";
	messageTimeout[0] = 2;
	message[1] = "They seem to keep track of something... I wonder what? Our crops, maybe?";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue3)
{
	messageCount = 2;
	message[0] = "Have you used a trowel on buried crops? They drop more produce when you use them to harvest!";
	messageTimeout[0] = 2;
	message[1] = "On the other hand, hoes let you harvest a bunch of belowground crops at once. Fast!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue4)
{
	messageCount = 2;
	message[0] = "Clippers are a really handy tool that boosts your harvest for aboveground crops!";
	messageTimeout[0] = 2;
	message[1] = "But if you want to harvest those fields of wheat faster, use a sickle instead!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue5)
{
	messageCount = 2;
	message[0] = "Tree Clippers let you boost the harvest from trees significantly. Big profits!";
	messageTimeout[0] = 2;
	message[1] = "You can also use them to prune apple trees before harvest, increasing yield!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue6)
{
	messageCount = 2;
	message[0] = "Watering cans come in 3 tiers, and are great for watering dirt! On the other hand, hoses are great for tanks!";
	messageTimeout[0] = 2;
	message[1] = "Check the tool seller regularly to see what he's got!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(OrganicAnalyzerDialogue)
{
	messageCount = 2;
	message[0] = "Have you seen this tool before? It's so cool! Tells you everything about the things you look at!";
	messageTimeout[0] = 2;
	message[1] = "Doesn't seem to work on anything inorganic though, so I only gave it a 2 out of 5 stars.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(NutrientDialogue)
{
	messageCount = 3;
	message[0] = "Some crops need nutrients - you can figure out what they need by using an Organic Analyzer on them.";
	messageTimeout[0] = 2;
	message[1] = "Phosphate can be mined from phosphate mines, and nitrogen comes from compost made from Compost Bins.";
	messageTimeout[1] = 2;
	message[2] = "Flowers also give nutrients to the dirt brick they're growing on! Make sure to pick them before they wilt.";
	messageTimeout[2] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ShovelDialogue)
{
	messageCount = 2;
	message[0] = "With shovels, you can retrieve nutrients from soil. Some of it will be lost though!";
	messageTimeout[0] = 2;
	message[1] = "You can use an organic analyzer to detect what nutrients (and how much) are in the soil.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(CompostBinDialogue1)
{
	messageCount = 2;
	message[0] = "Buy your compost bins here! To make compost, you just toss in any full basket of crops.";
	messageTimeout[0] = 2;
	message[1] = "Over time, the produce will decompose and generate compost!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(CompostBinDialogue2)
{
	messageCount = 2;
	message[0] = "Big compost bins store more crops and compost, on top of generating compost faster.";
	messageTimeout[0] = 2;
	message[1] = "Quite useful when you want to save storage space on compost!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolSellerDialogue)
{
	messageCount = 2;
	message[0] = "The Tool Seller works at the Tool Depot, near the bus stop TRW.";
	messageTimeout[0] = 2;
	message[1] = "He sells all kinds of tools! Check back regularly to see what he's offering.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(SeedSellerDialogue)
{
	messageCount = 2;
	message[0] = "The Seed Seller works at his greenhouse, at the pond near the OFL bus stop.";
	messageTimeout[0] = 2;
	message[1] = "He sells all kinds of seeds for half price! Check back regularly to see what he's offering.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PostOfficeDialogue)
{
	messageCount = 3;
	message[0] = "The Post Office is where you go to complete quests and receive quest rewards!";
	messageTimeout[0] = 2;
	message[1] = "Get a Quest Slip from the Task Manager, drop it into a post office box, then drop your deliveries into it.";
	messageTimeout[1] = 2;
	message[2] = "The quest slip will show you the progress and quest rewards. When you're done, drop it into the box to get your reward!";
	messageTimeout[2] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(TaskManagerDialogue)
{
	messageCount = 3;
	message[0] = "Hi, I'm the Task Manager! I get orders from people, and pass them on to you as quests!";
	messageTimeout[0] = 2;
	message[1] = "Check the quest notepads to see what's available! You can store quests in crates, toolboxes, and quest cabinets.";
	messageTimeout[1] = 2;
	message[2] = "Talk to the post office clerk to get information on how to complete quests. Good day to you!";
	messageTimeout[2] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(FakeSellerDialogue)
{
	messageCount = 2;
	message[0] = "Hi, I'm selling Rocket Launchers!";
	messageTimeout[0] = 5;
	message[1] = "...Unfortunately I'm out of stock. Check back later!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PyramidSchemeDialogue)
{
	messageCount = 2;
	message[0] = "Hi, I'm selling pyramids! For every pyramid you buy, you'll get two later!";
	messageTimeout[0] = 3;
	message[1] = "You can then sell these back to me for their original price!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);
