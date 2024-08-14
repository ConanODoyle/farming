//tips
function registerInfo(%str)
{
	$InfoString[$InfoStringCount++ - 1] = "[INFO] \c6" @ %str;
}

$tutorialTopicList = "basic, tools, storage, water, shop, money, planting/harvest, greenhouse fishing";

function infoLoop()
{
	cancel($InfoLoopSchedule);

	$InfoIDX = ($InfoIDX + 1) % $InfoStringCount;
	announce($InfoString[$InfoIDX]);

	$InfoLoopSchedule = schedule(300000, 0, infoLoop);
}

$InfoStringCount = 0;
registerInfo("Use \c5/tutorial\c6 to see information on a lot of things, like greenhouses, fishing, and fertilizer!");
registerInfo("Talk to the Lot Manager if you need to find an empty lot to buy! Use \c5/buyLot\c6 to buy a red lot - your first lot is free!");
registerInfo("Use \c5/home\c6 to instantly teleport to town!");
registerInfo("Explore the map to find secrets! Treasure chests give \c2$100\c6 each!");
registerInfo("Bus stops let you travel for just $0.50! Click the destination sheet to select a bus stop using brick controls!");
registerInfo("New to farming? Start with potatoes and carrots!");
registerInfo("Join the Discord here: <a:https://discord.gg/R3R3Vfj>https://discord.gg/R3R3Vfj");

package TutorialTrigger
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (stripos(%msg, "If you needed a helpful tip bot,") >= 0)
		{
			schedule(1, 0, infoLoop);
		}
		return parent::serverCmdMessageSent(%cl, %msg);
	}
};
activatePackage(TutorialTrigger);


//tutorial
$header = "<font:Palatino Linotype:24>";
$body = "<font:Arial:16>";

function serverCmdContinueTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial";
			%text = $header @ "Welcome to Farming!\n\n" @ $body @ "Would you like a quick tutorial on how to play?\n\n(Press YES on all additional prompts to continue through the tutorial)";
		case 1:
			%head = "Tutorial - Basics";
			%text = $header @ "- Overview -\n\n" 
				@ $body @ "Farming is a freebuild and exploration server with farming mechanics. Build a farm, fish for loot, and explore for secrets!\nFinding chests gives you $100!\n\n" 
				@ "Talk with bots scattered around the map for information, tips, and tutorials, and interact with them via chat.\n\n"
				@ "Bus stops scattered around the map can be used to travel around quickly, for only $0.50 per ride.";
		case 2:
			%head = "Tutorial - Basics";
			%text = $header @ "- Overview -\n\n" 
				@ $body @ "Your first land plot is free - talk to the Lot Manager in City Hall for help in getting your first lot!\n\n"
				@ "Buy seeds from seed sellers in the Barn or at Bapps, plant them on dirt bricks (found in the Farming tab), then water them and wait for them to grow. Crops do not die unless harvested, so don't worry about losing progress!\n\n"
				@ "Sell your produce to the Supermarket Buyer, found next to Bapps and the Barn. Some crops will drop seeds on harvest, letting you replant them for free!";
		case 3:
			%head = "Tutorial - Basics";
			%text = $header @ "- Special Offers -\n\n" 
				@ $body @ "Some special bots on the map have limited time buy/sell offers! Some will announce their offers, while others will have to be talked to directly.\n\n"
				@ "You can repair tools at the Repairs tent near the Town Hall, sell tools at the Tool Seller, buy seeds cheap at the Seed Seller, and sell produce for a bonus at the Big Buyer!";
		default:
			%head = "Tutorial - Basics";
			%text = $header @ "- More Info -\n\n" 
			@ $body @ "That's it for the basics!\nDo /tutorial if you wish to view this again.\n\n"
			@ "If you would like to know more, do /tutorial [topic] for specific information.\nOptions: " @ $tutorialTopicList;
			%client.messageBoxOKLong(%head, %text);
			%client.hasReadBasicTutorial = 1;
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continueTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueStorageTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Storage";
			%text = $header @ "- Storing -\n\n" 
				@ $body @ "Storage include crates, toolboxes, storage carts, quest cabinets, silos, and more. These can store many items in each slot!\n\n"
				@ "To drop an item into a storage brick, use the drop tool keybind to drop the item while facing the brick (default is Ctrl+W)\n\n"
				@ "Certain bricks are limited to specific types of items, and bricks like silos can contain a lot more of a single type of item.";
		case 1:
			%head = "Tutorial - Storage";
			%text = $header @ "- Withdrawing -\n\n" 
				@ $body @ "Click a storage brick to view and access its storage. Use the brick control keybinds to select and remove items (keybinds are in Options).\n\n"
				@ "Items that have many of a certain number, like crops, will be taken out in full item stacks at a time.\n\n"
				@ "If your inventory is full, items taken out will be dropped at your feet.";
		case 3:
			%head = "Tutorial - Storage";
			%text = $header @ "- Carts -\n\n" 
				@ $body @ "The storage cart stores the same amount as a crate. The large storage cart stores 2x as much.\n\n"
				@ "The horse cart and storage jeep store the same amount as a crate, but move significantly faster. The storage truck holds the same amount as a large storage cart.\n\n"
				@ "Vehicle bricks that have storage vehicles spawned on them will drop their contents when broken. The vehicle will be refunded at full price too!";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the storage tutorial. \n\nDo /tutorial Storage if you wish to view this again.");
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continueStorageTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueShopTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Shop";
			%text = $header @ "- Bots -\n\n" 
				@ $body @ "Click bots to start talking with them, to buy things or to learn what you can sell to them.\n\n"
				@ "If you don't have enough inventory space, you won't be able to purchase new items.";
		case 1:
			%head = "Tutorial - Shop";
			%text = $header @ "- Limited Offers -\n\n" 
				@ $body @ "The Tool Seller, Seed Seller, Bapps Shop Clerk, and Big Buyer will announce special deals occasionally.\n\n"
				@ "These special offers last a few minutes, and each bot has a different range of selections.\n\n"
				@ "Some valuable seeds can only be bought through the Seed Seller and Bapps shop clerks, so keep an eye out for them!\n\n"
				@ "Watering can upgrades, seed row planters, harvesting equipment, and other useful tools can be bought from the Tool Seller and specific sellers.";
		default:
			%client.messageBoxOKLong("Tutorial - End", "This is the end of the Shop tutorial. \n\nDo /tutorial Shop if you wish to view this again.\n\n You can find information on tools by doing /tutorial Tools");
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continueShopTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueWaterTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Water";
			%text = $header @ "- Watering -\n\n" 
				@ $body @ "Water dirt to provide water to crops growing on them. Crops take water and nutrients from the dirt they're planted on.\n\n"
				@ "Click water-storing bricks, like dirt and tanks, to see how much water is in them. Dirt will change color to match the level of water they have.";
		case 1:
			%head = "Tutorial - Water";
			%text = $header @ "- Watering Cans -\n\n" 
				@ $body @ "You can spam click watering cans to water faster! This is intended behavior.\n\n"
				@ "Hoses water tanks extremely quickly compared to watering cans.";
		case 2:
			%head = "Tutorial - Water";
			%text = "Sprinklers show their radius in their ghost brick.\n\nThe sprinkler has to cover any part of a dirt brick to water it.";
			%text = $header @ "- Sprinklers -\n\n" 
				@ $body @ "Sprinklers automatically water bricks around them, taking water from tanks they are attached to. They show their water area in their ghost brick.\n\n"
				@ "The sprinkler only has to cover any part of a dirt brick to water it, which means a 4x4 sprinkler can cover up to 4 8x8 dirt bricks.\n\n"
				@ "Sprinklers must be linked to water tanks with the Sprinkler Hose (free).\n\nWater tanks have a maximum link range and maximum number of supported sprinklers.";
		case 3:
			%head = "Tutorial - Water";
			%text = $header @ "- Rain -\n\n" 
				@ $body @ "Sometimes it will rain! The water usage of most plants are halved during rain.\n\n"
				@ "Rain also waters uncovered dirt and water tanks. The chances of weeds spawning is also increased";
		case 4:
			%head = "Tutorial - Water";
			%text = $header @ "- Heat Waves -\n\n" 
				@ $body @ "Also, sometimes there will be heat waves. The water usage of most plants will be doubled during heat waves.\n\n"
				@ "Dirt and tanks outside of greenhouses will lose water even without crops growing. The chances of weeds spawning is also decreased";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Water tutorial. \n\nDo /tutorial Water if you wish to view this again.");
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continueWaterTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueToolsTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Tools";
			%text = $header @ "- Planting -\n\n" 
				@ $body @ "Seed reclaimers allow you to reclaim seeds from harvestable plants. You get some of the experience used to plant the seed back.\n\n"
				@ "Seed planters allow you to plant full rows of seeds at once! Seeds are taken from your other inventory slots.\n\n"
				@ "Harvesting tools give bonuses to harvest, or allow you to harvest in an area. Trowels and hoes are used for belowground crops, while sickles and clippers and tree clippers are used for aboveground.";
		case 1:
			%head = "Tutorial - Tools";
			%text = $header @ "- Utility -\n\n" 
				@ $body @ "Upgrade tools allow you to upgrade another tool's maximum durability.\n\n"
				@ "Repair tools increase the durability of an item by 25% on use.\n\n"
				@ "Sprinkler lines allow you to connect water tanks and sprinklers together.\n\n"
				@ "Electrical cable allow you to connect electrical devices to a control box.\n\n"
				@ "CropTrak\x99 Kits apply a new CropTrak\x99 on a given harvesting tool. With enough tracked harvests, you can get permanent harvesting bonuses!";
		case 2:
			%head = "Tutorial - Tools";
			%text = $header @ "- Farming -\n\n" 
				@ $body @ "There are 3 tiers of watering cans and hoses. Hoses fill water tanks very quickly but dirt very slowly, while watering cans fill dirt quickly.\n\n"
				@ "Weedkiller prevents dirt from spawning weeds for a while.\n\n"
				@ "Organic Analyzers inform you the current requirements and conditions of a plant.";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Tools tutorial. \n\nDo /tutorial Tools if you wish to view this again.");
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continueToolsTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueFishingTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Fishing";
			%text = $header @ "- Fishing -\n\n" 
				@ $body @ "Fishing poles cast out a line and bobber, and can fish from any pool of water.\n\n"
				@ "The bobber will dip once a fish starts nibbling, then get pulled down strongly when the fish bites.\n\n"
				@ "Reel only when the fish bites on! The quicker you do it, the better quality loot you will get!";
		case 1:
			%head = "Tutorial - Fishing";
			%text = $header @ "- Items -\n\n" 
				@ $body @ "Buy fishing poles and accessories at the Bass Pro Shop, out in the desert!\n\n"
				@ "Each higher tier of fishing pole increases the overall quality of the loot.\n\n"
				@ "Tackle Boxes and Fish Finders give you more information about your fishing and how well you do.\n\n";
		case 2:
			%head = "Tutorial - Fishing";
			%text = $header @ "- Fish -\n\n" 
				@ $body @ "There are a large variety of fish, and the better you fish the better rewards you get.\n\n"
				@ "Most rewards from fishing can be sold. You can even sometimes get seeds for farming!\n\n"
				@ "Sell fishing loot at Supermarket Buyers, found at the barn and at Bapps.";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Fishing tutorial. \n\nDo /tutorial Fishing if you wish to view this again.");
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continueFishingTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueMoneyTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Money";
			%text = $header @ "- Bricks -\n\n" 
				@ $body @ "To see the cost of a farming brick, place its ghost brick and check your bottomprint display.\n\n"
				@ "Bricks that cost money can be broken for a full refund, and if they contain anything, those items will drop on the ground. The only exceptions are queued compost items and dirt nutrients.\n\n"
				@ "Special bricks that are placed with items also turn back into their item form upon being broken.";
		case 1:
			%head = "Tutorial - Money";
			%text = $header @ "- Commands -\n\n" 
				@ $body @ "Use /giveMoney amount name to give money to people.\n\nYou can only give up to $10000 at a time.\n\n";
		case 2:
			%head = "Tutorial - Money";
			%text = $header @ "- Out of money? -\n\n" 
				@ $body @ "If you ever go broke, hunt for treasure chests. They contain $100 each! There's " @ $TreasureChest::NumChests @ " on the server.\n\n"
				@ "You can also do mining quests at Coal's if you're out of chests. In addition, money quests that take crops give more money than selling the crops individually - the quest paper will show you how much additional you will make!";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Money tutorial. \n\nDo /tutorial Money if you wish to view this again.");
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continueMoneyTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinuePlantingTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Planting";
			%text = $header @ "- Planting -\n\n" 
				@ $body @ "Plant crops using seeds you buy from the store. Plants can only be planted on dirt, planter boxes, and pots.\n\n"
				@ "Different crops have different spacing requirements, and some can be planted nearby each other, allowing for more crop density.\n\n"
				@ "Buying certain seeds requires licenses, which can be purchased at the Licensor. Each license costs experience.";
		case 1:
			%head = "Tutorial - Planting";
			%text = $header @ "- Growth -\n\n" 
				@ $body @ "Plants grow with water and sunlight, and sometimes nutrients.\n\n"
				@ "They will stop using water and nutrients when fully grown, and will never wilt or die from dehydration.\n\n"
				@ "You can find detailed information on each crop by talking to the Cropmaster";
		case 2:
			%head = "Tutorial - Planting";
			%text = $header @ "- Harvesting -\n\n" 
				@ $body @ "Harvest crops by clicking on them - they can only be harvested with full trust. Dropped crops also require trust to pick up.\n\n"
				@ "Crop bricks will show the crop visible when harvestable. Not all harvestable stages are optimal - look carefully to see if the crop is at its peak harvest stage or not.\n\n"
				@ "Some crops can be harvested multiple times before dying, such as tomatoes. Other crops have a chance to drop their seed when harvested, including potatoes and carrots.";
		case 3:
			%head = "Tutorial - Planting";
			%text = $header @ "- Tools -\n\n" 
				@ $body @ "Seed reclaimers allow you to reclaim seeds from harvestable plants. You get some of the experience used to plant the seed back.\n\n"
				@ "Seed planters allow you to plant full rows of seeds at once! Seeds are taken from your other inventory slots.\n\n"
				@ "Harvesting tools give bonuses to harvest, or allow you to harvest in an area. Trowels and hoes are used for belowground crops, while sickles and clippers and tree clippers are used for aboveground.";
		case 4:
			%head = "Tutorial - Planting";
			%text = $header @ "- Trees -\n\n" 
				@ $body @ "Trees are special large crops that can be harvested many, many times. Other crops can be grown under them, with a reduction of light level.\n\n"
				@ "They all require some nutrients, both to grow and to bear fruit.\n\n"
				@ "You can find detailed information on each crop by talking to the Cropmaster";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Planting tutorial. \n\nDo /tutorial Planting if you wish to view this again.");
			return;
	}
	%client.messageBoxYesNoLong(%head, %text, 'continuePlantingTutorial');
	%client.tutorialPosition++;
}

function serverCmdTutorial(%client, %type, %t1, %t2, %t3)
{
	%client.tutorialPosition = 0;
	if ((%type $= "" && !%client.hasReadBasicTutorial) || %type $= "Basic")
	{
		serverCmdContinueTutorial(%client);
		return;
	}

	%type = strLwr(trim(%type SPC %t1 SPC %t2 SPC %t3));

	if (stripos(%type, "Storage") == 0)
	{
		serverCmdContinueStorageTutorial(%client);
	}
	else if (stripos(%type, "Shop") == 0)
	{
		serverCmdContinueShopTutorial(%client);
	}
	else if (stripos(%type, "Water") == 0)
	{
		serverCmdContinueWaterTutorial(%client);
	}
	else if (stripos(%type, "Fishing") == 0)
	{
		serverCmdContinueFishingTutorial(%client);
	}
	else if (stripos(%type, "Money") == 0)
	{
		serverCmdContinueMoneyTutorial(%client);
	}
	else if (stripos(%type, "Plant") == 0 || stripos(%type, "Harvest") == 0)
	{
		serverCmdContinuePlantingTutorial(%client);
	}
	else if (stripos(%type, "Tool") == 0)
	{
		serverCmdContinueToolsTutorial(%client);
	}
	else if (stripos(%type, "Greenhouse") == 0)
	{
		%head = "Tutorial - Greenhouse";
		%text = $header @ "- Greenhouses -\n\n" 
			@ $body @ "Greenhouses are special bricks that make plants inside them grow 2x as fast and use 1/2 as much water, as well as reducing the required spacing for each crop by 1.\n\n"
			@ "Crops will still require the same amount of nutrients, and the limited space means trees cannot grow inside them. That said, greenhouses increase maximum possible yield by +2!\n\n"
			@ "Weeds also cannot grow under greenhouses, and won't affect plants inside them.";
		%client.messageBoxOKLong(%head, %text);
	}
	else if (stripos(%type, "Experience") == 0)
	{
		%head = "Tutorial - Experience";
		%text = $header @ "- Experience -\n\n" 
			@ $body @ "Experience is gained by harvesting crops and weeds. Basic crops - potatoes and carrots - don't require any experience to plant.\n\n"
			@ "Nearly all other crops require experience to plant - some give more than they require, like onions, while others don't but in turn produce more profit.\n\n"
			@ "You can also sacrifice yourself at the hole above town for experience based on the value of the items in your inventory, if needed.\n\n";
		%client.messageBoxOKLong(%head, %text);
	}
	else
	{
		%head = "Tutorial List";
		%text = $header @ "- More Info -\n\n" 
			@ $body @ "That's it for the basics!\nDo /tutorial if you wish to view this again.\n\n"
			@ "If you would like to know more, do /tutorial [topic] for specific information.\nOptions: " @ $tutorialTopicList;
		%client.messageBoxOKLong(%head, %text);
	}
}

function serverCmdHelp(%client, %a, %b, %c, %d)
{
	serverCmdTutorial(%client, %a, %b, %c, %d);
}