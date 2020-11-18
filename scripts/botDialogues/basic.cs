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
	message[0] = "A boatload of electrical whatchamacallits just arrived 'ere... If yer' lookin' to expand, yous should look into thems.";
	messageTimeout[0] = 2;
	message[1] = "Looks like yous'll get water pumps to fill up yer' tanks, an' some inside lights that'll let yous grow stuff indoors...";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PowerSystemDialogue2)
{
	messageCount = 2;
	message[0] = "To use electrical equipment, hook them up to a Power Control Box. Mind your fingers, wouldn't want to get fried before your time in the sun does it. Heh.";
	messageTimeout[0] = 2;
	message[1] = "Afterwards, you can hook up a generator or some charged batteries to power them! Don't forget to put in some coal if you're using the former.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PowerSystemDialogue3)
{
	messageCount = 2;
	message[0] = "Duhhaa... You can use da eclectrical cable to connect da eclectical things togetha! Dey give it away for free in da town...";
	messageTimeout[0] = 2;
	message[1] = "You shoulddda probablys use it on da eclectical brick, then on to da power box thinger to connect tha two.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue1)
{
	messageCount = 2;
	message[0] = "Ever since we got here, our tools have kept breaking... Stupid manufacturers don't know how to make them right!";
	messageTimeout[0] = 2;
	message[1] = "Luckily our local toolsmith knows how to repair them... You should find him in town, you'll need to get your tools fixed eventually.";
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
	message[0] = "Compost bins make compost that provide nitrogen to soil! To make compost, you just toss in full baskets of crops.";
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

$obj = new ScriptObject(CoalMiningDialogue)
{
	messageCount = 2;
	message[0] = "Hey you, you can mine coal here! Use it to fuel coal generators, or sell it in town for a small profit.";
	messageTimeout[0] = 2;
	message[1] = "Grab a pickaxe and get to it! Each coal takes 100 seconds to burn.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PhosphateMiningDialogue)
{
	messageCount = 2;
	message[0] = "Hey you, you can mine phosphate here! Use it to fertilize dirt for crops that need it.";
	messageTimeout[0] = 2;
	message[1] = "Grab a pickaxe and get to it! Each phosphate ore gives 5 phosphate to dirt it's applied to.";
	messageTimeout[1] = 2;
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

$obj = new ScriptObject(MailEnhancementDialogue)
{
	messageCount = 2;
	message[0] = "We've got a special deal going on!";
	messageTimeout[0] = 3;
	message[1] = "Sign up for our mail enhancement program and gain 5\" of mailbox space!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);
