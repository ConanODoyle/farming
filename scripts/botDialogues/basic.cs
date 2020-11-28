if (!isObject($BasicDialogueSet))
{
	$BasicDialogueSet = new SimSet(BasicDialogueSet);
}
$BasicDialogueSet.deleteAll();

$obj = new ScriptObject(BusInfoDialogue)
{
	messageCount = 3;
	message[0] = "Bus stops are really cool! They only cost $0.50 to use, and bring you around really fast.";
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
	message[0] = "To use electrical equipment, hook them up to a Power Control Box. Mind your fingers, wouldn't want to get fried before the sun can do it to ya. Heh.";
	messageTimeout[0] = 2;
	message[1] = "Make sure to hook up a generator or some charged batteries to power them! Don't forget to put in some coal to burn!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PowerSystemDialogue3)
{
	messageCount = 2;
	message[0] = "Duhhaa... You can use da eclectrical cable to connect da eclectical things togetha! Dey give it away for free rite heea...";
	messageTimeout[0] = 2;
	message[1] = "You shoulddda probablys use it on da eclectical brick, then on to da power box thinger to connect tha two.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue1)
{
	messageCount = 2;
	message[0] = "Ever since we got here, our tools have kept breaking... Stupid manufacturers don't know how to make them last!";
	messageTimeout[0] = 2;
	message[1] = "Luckily our local toolsmith knows how to repair them... You can find him in town, you'll need to get your tools fixed eventually.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue2)
{
	messageCount = 2;
	message[0] = "We've been occasionally getting tools that inexplicably have microchips embedded in them.";
	messageTimeout[0] = 2;
	message[1] = "They seem to keep track of something... Our crops, maybe? If I were you, I'd wrap all my tools in tinfoil.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue3)
{
	messageCount = 2;
	message[0] = "Have you used a trowel on buried crops? It'll give you more produce when you use them to harvest!";
	messageTimeout[0] = 2;
	message[1] = "On the other hand, hoes let you harvest a bunch of belowground crops at once. Saves you lots of time!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue4)
{
	messageCount = 2;
	message[0] = "Clippers are a really handy tool to get you more from harvesting surface crops like corn and wheat!";
	messageTimeout[0] = 2;
	message[1] = "But if you want to harvest those seas of grain faster, use a sickle! Cuts 'em all down in an instant!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolDialogue5)
{
	messageCount = 2;
	message[0] = "Tree Clippers let you harvest significantly more from trees. Big profits!";
	messageTimeout[0] = 2;
	message[1] = "You can also use them to prune flowering apple trees before harvest! Pruned trees bear way more fruit!";
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
	message[0] = "Have you seen this analyzer before? It's so cool! Tells you everything about the things you look at!";
	messageTimeout[0] = 2;
	message[1] = "Doesn't seem to work on anything inorganic though, so I really can only gave it a solid 5/7.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(NutrientDialogue)
{
	messageCount = 4;
	message[0] = "Some crops are demanding little buggers - they won't grow without nutrients!";
	messageTimeout[0] = 2;
	message[1] = "If you got yourself an Organic Analyzer, you can use it to see what they need. They draw nutrients from the soil they're on.";
	messageTimeout[1] = 2;
	message[2] = "Nitrogen comes from compost, which you can buy right here, while phosphate can be mined from the phosphate mines.";
	messageTimeout[2] = 2;
	message[3] = "In addition, flowers add nutrients to the soil they're growing on. Make sure to pick them before they wilt though!";
	messageTimeout[3] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ShovelDialogue)
{
	messageCount = 2;
	message[0] = "With shovels, you can retrieve nutrients from soil. Some of it will be lost though!";
	messageTimeout[0] = 2;
	message[1] = "You can use an organic analyzer to detect what nutrients, and how much, are in the soil.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(CompostBinDialogue1)
{
	messageCount = 2;
	message[0] = "Compost bins make compost that provide nitrogen to soil! To make compost, you just toss in full baskets of crops.";
	messageTimeout[0] = 2;
	message[1] = "Over time, the produce will decompose and generate compost! With the Combiner, you can combine it with phosphate to make fertilizer!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(CompostBinDialogue2)
{
	messageCount = 2;
	message[0] = "Big compost bins store more crops and compost, on top of generating more compost faster.";
	messageTimeout[0] = 2;
	message[1] = "Quite useful when you want to save storage space on compost!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolSellerDialogue)
{
	messageCount = 2;
	message[0] = "Need tools? The Tool Seller works at the [COPYRIGHT INFRINGEMENT] Depot, near the bus stop TRW.";
	messageTimeout[0] = 2;
	message[1] = "He sells all kinds of tools! He also offers quests for tools, check back regularly to see what he's got available!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(SeedSellerDialogue)
{
	messageCount = 2;
	message[0] = "The Seed Seller works at his greenhouse, at the pond near the OFL bus stop.";
	messageTimeout[0] = 2;
	message[1] = "He's a real big guy, sells all of his seeds for half price! He's also got quests for rare seeds - you should check it often for crops you want!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PostOfficeDialogue)
{
	messageCount = 3;
	message[0] = "Welcome to the Post Office! Here you deliver quest materials and get your rewards!";
	messageTimeout[0] = 2;
	message[1] = "You can get Quest Slips from the notepads near special bots - once you have one, drop it into a post office box, followed by the requested items.";
	messageTimeout[1] = 2;
	message[2] = "The quest slip will show you your progress and quest rewards. When you're done, drop it into the box to get your reward!";
	messageTimeout[2] = 2;
	message[2] = "If you lose your slip, click the delivery box it's linked to to get a new copy of the quest slip.";
	messageTimeout[2] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(TaskManagerDialogue)
{
	messageCount = 3;
	message[0] = "Need work? I take produce orders from lands far away, and pass them on to you farmers as quests!";
	messageTimeout[0] = 2;
	message[1] = "Check the quest pads to see what's available! You can store quests in crates, toolboxes, and quest cabinets.";
	messageTimeout[1] = 2;
	message[2] = "Once you got the requests together, head over to a Post Office to deliver them! I'm only a middleman, so I can't take your stuff.";
	messageTimeout[2] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(CoalMiningDialogue)
{
	messageCount = 2;
	message[0] = "This here's a coal mine. If you're lookin' for work, or some quick cash, you can mine some coal to fill out coal quests.";
	messageTimeout[0] = 2;
	message[1] = "You can get coal quests right over there, in... Coal's? Cole's? Kohl's? No idea how you kids say it.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PhosphateMiningDialogue)
{
	messageCount = 2;
	message[0] = "Hey you, you can mine phosphate here! Some crops need it to grow, you can talk to the guys at the barn to find out which ones.";
	messageTimeout[0] = 2;
	message[1] = "Each phosphate item gives 5 phosphate to dirt bricks it's applied to.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(MiningQuestsDialogue)
{
	messageCount = 2;
	message[0] = "Lookin' for work? I got mineral orders to fill and not enough people to do 'em!";
	messageTimeout[0] = 2;
	message[1] = "Pick up an order from the quest pads over there, it'll tell you what needs doin'. Once you got it, you can deliver the stuff at the drop box here.";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(PlantLayerDialogue)
{
	messageCount = 2;
	message[0] = "Not all crops interact with each other when determining crop spacing! For example, you can plant potatoes right up against tomatoes!";
	messageTimeout[0] = 2;
	message[1] = "It's a neat trick to make your farm space more effective! Test crop combinations out to see what works and what doesnt!";
	messageTimeout[1] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(GemDialogue)
{
	messageCount = 1;
	message[0] = "Got any shiny gems? I buy them!";
	messageTimeout[0] = 2;
};
$BasicDialogueSet.add($obj);

$obj = new ScriptObject(ToolBuyerDialogue)
{
	messageCount = 1;
	message[0] = "I buy tools! Yes, even broken ones!";
	messageTimeout[0] = 2;
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
	message[0] = "Hi, I'm selling potsherds! For every potsherd you buy, you'll get two later!";
	messageTimeout[0] = 3;
	message[1] = "You can then sell these back to me, to purchase more potsherds! Or after someone else buys some more from me!";
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
