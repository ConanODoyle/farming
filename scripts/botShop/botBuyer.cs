if (!isObject($BuyDialogueSet))
{
	$BuyDialogueSet = new SimSet(BuyDialogueSet);
}
$BuyDialogueSet.deleteAll();

$obj = new ScriptObject(BuyDialogueStart)
{
	response["Quit"] = "ExitResponse";

	messageCount = 2;
	message[0] = "Hello!";
	messageTimeout[0] = 1;
	message[1] = "I'm buying %purchase%%factorPhrase% Use Ctrl-W (drop tool) to give me anything you'd like to sell.";
	messageTimeout[1] = 1;

	functionOnStart = "setupBuyDialogue";
};
$BuyDialogueSet.add($obj);

function setupBuyDialogue(%dataObj)
{
	%buyer = %dataObj.speaker;

	if (%buyer.buyItems !$= "")
	{
		for (%i = 0; %i < getWordCount(%buyer.buyItems); %i++)
		{
			%item = getWord(%buyer.buyItems, %i);
			if (%item $= "" || !isObject(%item))
			{
				continue;
			}

			if (%list !$= "")
			{
				%list = %list TAB getPluralWord(%item[%i].uiName);
			}
			else
			{
				%list = getPluralWord(%item[%i].uiName);
			}
			%count++;
		}

		if (%count > 1)
		{
			%list = trim(setField(%list, %count - 1, "and " @ getField(%list, %count - 1)));
			%list = strReplace(%list, "\t", ", ");
			%dataObj.var_purchase = %list;
		}
		else if (%count > 0)
		{
			%dataObj.var_purchase = getPluralWord(%buyer.buyItem.uiName);
		}
	}
	else if (isObject(%buyer.buyItem))
	{
		%dataObj.var_purchase = getPluralWord(%buyer.buyItem.uiName);
	}
	else if (%buyer.buyType !$= "")
	{
		%dataObj.var_purchase = %buyer.buyType;
	}

	if (%buyer.buyPriceMod != 1)
	{
		%dataObj.var_factorPhrase = " for " @ mFloor(%buyer.buyPriceMod * 100) @ "% of its normal price!";
	}
	else
	{
		%dataObj.var_factorPhrase = "!";
	}
}


function AIPlayer::setBuyMode(%bot, %mode, %item, %string, %factor)
{
	%bot.buyType = "";
	%bot.buyItems = "";
	%bot.buyItem = "";
	
	if (%factor > 0) //no ternary cause that casts to int >:(
	{
		%bot.buyPriceMod = %factor;
	}
	else
	{
		%bot.buyPriceMod = 1;
	}

	switch (%mode)
	{
		case 0: //All
			%bot.buyType = "ALL";
		case 1: //Specific item
			%bot.buyItem = %item;
		case 2: //List of items
			%bot.buyItems = %string;
		case 3: //buyType
			%bot.buyType = %string;
	}
}
registerOutputEvent("Bot", "setBuyMode", "list All 0 Item 1 Items 2 ItemType 3" TAB "dataBlock ItemData" TAB "string 200 100" TAB "string 200 20", 1);


function AIPlayer::canBuy(%bot, %item)
{
	if (!isObject(%item) || !isObject(%itemDB = %item.getDatablock()) || (%itemDB.isStackable && %item.count <= 0)
		|| (%bot.buyType $= "" && %bot.buyItems $= "" && %bot.buyItem $= ""))
	{
		return 0;
	}
	else if (%bot.buyItems !$= "")
	{
		for (%i = 0; %i < getWordCount(%bot.buyItems); %i++)
		{
			%buyitem = getWord(%bot.buyItems, %i);
			if (%buyitem $= "" || (!isObject(%buyitem) && %buyitem !$= %itemDB.stackType))
			{
				continue;
			}

			if (%buyitem.getID() == %itemDB || %buyitem $= %itemDB.stackType)
			{
				return 1;
			}
		}
		return 0;
	}
	else if (%bot.buyItem !$= "")
	{
		if (isObject(%bot.buyItem))
		{
			if (%bot.buyItem.getID() == %itemDB)
			{
				return 1;
			}
			else if (%bot.buyItem.isStackable && %itemDB.isStackable && %bot.buyItem.stackType $= %itemDB.stackType)
			{
				return 1;
			}
		}
		else if (%bot.buyItem $= %itemDB.stackType)
		{
			return 1;
		}
	}
	else if (%bot.buyType !$= "")
	{
		if (%bot.buyType $= "ALL")
		{
			return 1;
		}

		%list = "\t" @ $StorageType[%bot.buyType @ "List"] @ "\t";
		%type = %itemDB.stackType $= "" ? %itemDB.uiName : %itemDB.stackType;
		if (strPos(%list, "\t" @ %type @ "\t") >= 0)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}

	return 0;
}

function AIPlayer::attemptBuy(%bot, %item)
{
	if (!%bot.canBuy(%item))
	{
		if (%item.count <= 0 && %item.getDatablock().isStackable)
		{
			%item.delete();
		}
		else
		{
			%pl = findClientByBL_ID(%item.bl_id).player;
			%vec = vectorNormalize(vectorSub(%pl.getEyePoint(), %item.getPosition()));
		}

		%bot.mountImage(%item.getDatablock().image, 0);
		%bot.setAimObject(%pl);
		%item.hideNode("ALL");
		%item.canPickup = 0;
		%item.setVelocity("0 0 0");
		%lastTrans = %item.getTransform();

		%bot.schedule(1000, mountImage, wtfImage, 3);

		%item.schedule(2000, unHideNode, "ALL");
		%item.schedule(1999, setTransform, %lastTrans);
		%item.schedule(2000, setVelocity, vectorAdd(vectorScale(%vec, 6), "0 0 4x"));
		schedule(2000, %item, eval, %item @ ".canPickup = 1;");
		%bot.schedule(2000, playThread, 0, undo);
		%bot.schedule(2000, playThread, 1, root);
		%bot.schedule(2000, unmountImage, 0);
		return 0;
	}

	%count = %item.count;
	if (%count $= "")
	{
		%count = 1;
	}
	if (%item.getDatablock().stackType !$= "")
	{
		%type = %item.getDatablock().stackType;
	}
	else
	{
		%type = %item.getDatablock().getName();
	}
	
	if (%bot.buyCost[%type] <= 0)
	{
		%amount = getSellPrice(%type, %count);
		if (strPos(strLwr(%type), "seed") >= 0) //halve seed prices //come up with a better way to store/change this info
		{
			%amount = %amount / 2;
		}
		if (%bot.buyPriceMod > 0)
		{
			%amount *= %bot.buyPriceMod;
		}
	}
	else
	{
		%amount = %count * %bot.buyCost[%type];
	}

	%blid = %item.bl_id;

	if (isObject(%cl = findClientByBL_ID(%blid)))
	{
		%cl.setScore(%cl.score + %amount);
		%amount = mFloatLength(%amount, 2);
		%bot.setAimObject(%cl.player);
		%bot.playThread(0, plant);
		%bot.playThread(1, activate);
		messageClient(%cl, 'MsgItemPickup');
		%plural = %count > 1 ? "s" : "";
		%type = %item.getDatablock().stackType;
		if (%type $= "")
		{
			%type = %item.getDatablock().uiName;
		}

		%cl.updateSale(%amount, %count, %type);
		%item.delete();
		return 1;
	}

	return 0;
}

function GameConnection::updateSale(%cl, %money, %count, %type)
{
	cancel(%cl.messageSaleSched[%type]);

	%cl.totalMoney[%type] += %money;
	%cl.totalCount[%type] += %count;

	%cl.messageSaleSched[%type] = schedule(1000, %cl, messageSale, %cl, %cl.totalMoney[%type], %cl.totalCount[%type], %type);
}

function messageSale(%cl, %money, %count, %type) 
{
	%cl.totalMoney[%type] = 0;
	%cl.totalCount[%type] = 0;

	%plural = %count > 1 ? "s" : "";
	if (%plural !$= "")
	{
		switch$ (%type)
		{
			case "Tomato": %plural = "es";
			case "Potato": %plural = "es";
			case "Corn": %plural = " Cobs";
		}
	}

	messageClient(%cl, '', "\c6You received \c2$" @ mFloatLength(%money, 2) @ "\c6 for selling \c3" @ %count SPC %type @ %plural @ "\c6!");
}

package BotBuyer
{
	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getClassName() $= "AIPlayer" && !isObject(%obj.client) && %obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0)
		{
			if (%col.getClassName() $= "Item" && %obj.canBuy(%col))
			{
				%success = %obj.attemptBuy(%col);

				if (%success)
				{
					return;
				}
			}
		}

		return parent::onCollision(%db, %obj, %col, %vec, %speed);
	}
};
schedule(2000, 0, activatePackage, BotBuyer);