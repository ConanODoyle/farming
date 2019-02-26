
function serverCmdContinueTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial";
			%text = "Welcome to Farming!<br><br>Would you like a quick tutorial on how to play?<br><br>(Press YES on all additional prompts to continue through the tutorial)";
		case 1:
			%head = "Tutorial - Basics";
			%text = "Farming is a freebuild server with farming mechanics, where the goal is to build a farm, explore, and make money.";
		case 2:
			%head = "Tutorial - Basics";
			%text = "Buy your first lot for free with /buyLot on an empty single lot!<br><br>Buy dirt bricks, plant seeds, and water the dirt so your crops grow!";
		case 3:
			%head = "Tutorial - Basics";
			%text = "You buy seeds and sell produce at the shop.<br><br>Click bots to find out what they do or sell! Some bots have limited time deals.";
		case 4:
			%head = "Tutorial - Basics";
			%text = "If you don't know what something does, do /tutorial [name] to find out more.<br><br>Examples: storage, water, shop, money";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the basic tutorial. Have fun!<br><br>Do /tutorial if you wish to view this again.");
			return;
	}

	commandToClient(%client, 'messageBoxYesNo', %head, %text, 'continueTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueStorageTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Storage";
			%text = "You can store items in storage crates and carts by dropping the item in them. (Default Ctrl-W)";
		case 1:
			%head = "Tutorial - Storage";
			%text = "To take items out, click the storage container and use the brick controls + enter.";
		case 2:
			%head = "Tutorial - Storage";
			%text = "Stackable items will stack in storage containers. Silos stack 6x as much as crates.<br><br>Normal items will always take up 1 slot.";
		case 3:
			%head = "Tutorial - Storage";
			%text = "The storage cart stores the same amount as a crate. The large storage cart stores 2x as much.";
		case 4:
			%head = "Tutorial - Storage";
			%text = "Storage containers (and vehicle spawns) will drop all the items in them if broken.";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the storage tutorial. <br><br>Do /tutorial Storage if you wish to view this again.");
			return;
	}

	commandToClient(%client, 'messageBoxYesNo', %head, %text, 'continueStorageTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueShopTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Shop";
			%text = "To buy items from sellers, click them then say the number you want to buy in chat.<br><br>If you don't have enough inventory space, you won't be able to purchase.";
		case 1:
			%head = "Tutorial - Shop";
			%text = "The Tool Seller, Seed Seller, Shop Clerk, and Big Buyer will announce special deals occasionally.<br><br>These special offers last a few minutes, and each bot has a different range of selections.";
		case 2:
			%head = "Tutorial - Shop";
			%text = "Some valuable seeds can only be bought through the Seed Seller and Shop Clerk, so keep an eye out for them!";
		case 3:
			%head = "Tutorial - Shop";
			%text = "Watering can upgrades, seed row planters, harvesting equipment, and other useful tools can be bought from the Tool Seller or Shop Clerk.";
		case 4:
			%head = "Tutorial - Shop";
			%text = "Hoe/Sickle: area harvest<br>Clipper/Trowel: +1 harvest bonus<br>Hose: fill tanks fast<br>Planter: plant multiple seeds in a row<br>Reclaimer: reclaim seeds from fully-grown plants";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Shop tutorial. <br><br>Do /tutorial Shop if you wish to view this again.");
			return;
	}

	commandToClient(%client, 'messageBoxYesNo', %head, %text, 'continueShopTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueWaterTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Water";
			%text = "Water dirt to let crops on them grow.<br><br>Crops take water from the dirt under them.";
		case 1:
			%head = "Tutorial - Water";
			%text = "Sprinklers show their radius in their ghost brick.<br><br>The sprinkler has to cover any part of a dirt brick to water it.";
		case 2:
			%head = "Tutorial - Water";
			%text = "Sprinklers must be linked to water tanks with the Sprinkler Hose (free).<br><br>Water tanks have a maximum link range and maximum number of supported sprinklers.";
		case 3:
			%head = "Tutorial - Water";
			%text = "Sometimes it will rain. Water usage of plants are halved during rain.<br><br>Rain also waters dirt and fills up water tanks that aren't covered.";
		case 4:
			%head = "Tutorial - Water";
			%text = "Sometimes there will be heat waves.<br><br>Heat waves double water consumption of all crops.";
		case 5:
			%head = "Tutorial - Water";
			%text = "You can spam click watering cans to water faster!<br><br>Hoses water tanks extremely quickly compared to watering cans.";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Water tutorial. <br><br>Do /tutorial Shop if you wish to view this again.");
			return;
	}

	commandToClient(%client, 'messageBoxYesNo', %head, %text, 'continueWaterTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinueMoneyTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Money";
			%text = "Use /giveMoney amount name to give money to people.<br><br>You can only give up to $1000 at a time.";
		case 1:
			%head = "Tutorial - Money";
			%text = "To see the cost of a farming brick, place a ghost brick and check your bottomprint display.";
		case 2:
			%head = "Tutorial - Money";
			%text = "If you ever get broke, hunt for treasure chests.<br><br>They contain $100 each! There's " @ $TreasureChest::NumChests @ " on the server...";
		case 3:
			%head = "Tutorial - Money";
			%text = "Everything stackable can be sold for money.<br><br>You can only make a decent profit selling produce, though.";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Money tutorial. <br><br>Do /tutorial Shop if you wish to view this again.");
			return;
	}

	commandToClient(%client, 'messageBoxYesNo', %head, %text, 'continueMoneyTutorial');
	%client.tutorialPosition++;
}

function serverCmdContinuePlantingTutorial(%client)
{
	switch(%client.tutorialPosition)
	{
		case 0:
			%head = "Tutorial - Planting";
			%text = "Plants grow with water and sunlight, and do not die if dehydrated.<br><br>They will stop using water when done growing.";
		case 1:
			%head = "Tutorial - Planting";
			%text = "To harvest, click on the plant - some crops will drop. You can use tools to speed this up or add a bonus.<br><br>Just-harvested crops cannot be stolen, you must have full trust to pick it up.";
		case 2:
			%head = "Tutorial - Planting";
			%text = "The seed reclaimer tool allows you to reclaim seeds from harvestable plants.<br><br>You get a 50% EXP cost refund, if the plant costed EXP to plant.";
		case 3:
			%head = "Tutorial - Planting";
			%text = "Seed planters allow you to plant rows of seeds at once.<br><br>Sickles/hoes allow you to area harvest plants at once.<br><br>Clippers/Trowels give a +1 to harvest amount.";
		default:
			commandToClient(%client, 'messageBoxOK', "Tutorial - End", "This is the end of the Planting tutorial. <br><br>Do /tutorial Shop if you wish to view this again.");
			return;
	}

	commandToClient(%client, 'messageBoxYesNo', %head, %text, 'continuePlantingTutorial');
	%client.tutorialPosition++;
}

function serverCmdTutorial(%client, %type, %t1, %t2, %t3)
{
	%client.tutorialPosition = 0;
	if (%type $= "" || %type $= "Basic")
	{
		serverCmdContinueTutorial(%client);
		return;
	}

	%type = strLwr(trim(%type SPC %t1 SPC %t2 SPC %t3));

	if (%type $= "Storage")
	{
		serverCmdContinueStorageTutorial(%client);
	}
	else if (%type $= "Shop")
	{
		serverCmdContinueShopTutorial(%client);
	}
	else if (%type $= "Water")
	{
		serverCmdContinueWaterTutorial(%client);
	}
	else if (%type $= "Money")
	{
		serverCmdContinueMoneyTutorial(%client);
	}
	else if (%type $= "Planting" || %type $= "Harvest")
	{
		serverCmdContinuePlantingTutorial(%client);
	}
	else if (%type $= "Greenhouse")
	{
		%head = "Tutorial - Greenhouse";
		%text = "Greenhouses make plants inside them grow 2x as fast and use 1/2 as much water.<br><br>They also increase yield by +2";
		commandToClient(%client, 'messageBoxOK', %head, %text);
	}
	else if (%type $= "Experience")
	{
		%head = "Tutorial - Experience";
		%text = "You gain experience by planting crops. Potatoes give +2, while trees, turnips, and blueberries cost EXP.<br><br>You can also sacrifice yourself for experience by jumping into the volcano with crops and tools in your inventory.";
		commandToClient(%client, 'messageBoxOK', %head, %text);
	}
	else
	{
		%head = "Tutorial types:";
		%text = "basic, storage, shop, water<br>money, greenhouse, experience<br>planting/harvest";
		commandToClient(%client, 'messageBoxOK', %head, %text);
	}
}

function serverCmdHelp(%client, %a, %b, %c, %d)
{
	serverCmdTutorial(%client, %a, %b, %c, %d);
}