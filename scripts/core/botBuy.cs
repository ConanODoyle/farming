
function AIPlayer::setProduceData(%bot, %type, %data, %shapename, %dist)
{
	switch (%type)
	{
		case 0: %bot.saleType = "Buy";
		case 1: %bot.saleType = "Sell";
		case 2: %bot.saleType = ""; %bot.produceType = ""; %bot.produceCost = ""; %bot.setShapeNameDistance(0); return;
	}

	%bot.produceType = getWord(%data, 0);
	%bot.produceCost = getWord(%data, 1);
	if (%bot.produceCost $= "")
	{
		%bot.produceCost = %bot.produceType.cost;
	}

	%bot.setShapeName(%shapename, 8564862);
	%bot.setShapeNameDistance(%dist);
}

registerOutputEvent("Bot", "setProduceData", "list Buy 0 Sell 1 None 2" TAB "string 200 100" TAB "string 200 100" TAB "int 0 10000 10", 1);

function AIPlayer::talkProduceInfo(%bot, %cl)
{
	%star = "<bitmap:base/client/ui/ci/star> ";
	%timeLeft = mFloor(%bot.nextEventTime - $Sim::Time);
	if (%timeLeft < 60)
	{
		%timeLeftMsg = "This deal will end very soon!";
		%newDealMsg = "I'll check for a new deal very soon!";
	}
	else
	{
		%timeLeftMsg = "This deal will end in " @ mCeil(%timeLeft / 60) @ " minutes!";
		%newDealMsg = "I'll check for a new deal in " @ mCeil(%timeLeft / 60) @ " minutes!";
	}

	if (%bot.saleType $= "")
	{
		%bot.chatRandomMessage(%cl);
		messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %newDealMsg);
		return;
	}

	if (%bot.saleType $= "Sell")
	{
		%bot.startSell("", %cl);
		messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %timeLeftMsg);
		return;
	}
	else if (%bot.saleType $= "Buy")
	{
		if (%bot.produceType $= "" && %bot.produceCost $= "")
		{
			messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: I'm buying everything at standard prices! Check the listing nearby for the values." );
			return;
		}

		%cropType = %bot.produceType;
		%cost = %bot.produceCost;
		%cost = mFloatLength(%cost, 2);
		
		%message = "I'm buying \c3" @ %cropType @ "\c6 for \c2$" @ %cost @ "\c6 each!";
	}
	messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);
	messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %timeLeftMsg);
	%bot.setAimObject(%cl.player);
}

registerOutputEvent("Bot", "talkProduceInfo", "", 1);

//%brick.centerprintMenu is deleted on onRemove() in eventStorage.cs
function fxDTSBrick::listProducePrices(%brick, %cl)
{
	if (!isObject(%brick.centerprintMenu))
	{
		%brick.centerprintMenu = new ScriptObject(ProduceMenus)
		{
			isCenterprintMenu = 1;
			menuName = "Produce Prices";
		};

		for (%i = 0; %i < $ProduceCount; %i++)
		{
			%produce = getField($ProduceList_[%i], 0);
			%cost = $Produce::BuyCost_[%produce];
			%brick.centerprintMenu.menuOption[%i] = %produce @ " - $" @ mFloatLength(%cost, 2);
		}
		%brick.centerprintMenu.menuOptionCount = %i;
		MissionCleanup.add(%brick.centerprintMenu);
	}

	%cl.startCenterprintMenu(%brick.centerprintMenu);
}

registerOutputEvent("fxDTSBrick", "listProducePrices", "", 1);

function AIPlayer::attemptBuy(%bot, %item)
{
	if (%bot.saleType !$= "Buy" 
		|| (%item.getDatablock().stackType !$= %bot.produceType && %bot.produceType !$= "")
		|| %item.count <= 0)
	{
		if (%item.count <= 0)
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
	%type = %item.getDatablock().stackType;
	if (%bot.produceCost <= 0)
	{
		%money = %count * $Produce::BuyCost_[%type];
		if (strPos(strLwr(%type), "seed") >= 0) //halve seed prices //come up with a better way to store/change this info
		{
			%money = %money / 2;
		}
	}
	else
	{
		%money = %count * %bot.produceCost;
	}
	if (%bot.nextEventTime > $Sim::Time) //reduce time left on seller
	{
		%bot.nextEventTime -= 6;
		%bot.refreshTime -= 6;
	}
	%blid = %item.bl_id;

	if (isObject(%cl = findClientByBL_ID(%blid)))
	{
		%cl.setScore(%cl.score + %money);
		%money = mFloatLength(%money, 2);
		%bot.setAimObject(%cl.player);
		%bot.playThread(0, plant);
		%bot.playThread(1, activate);
		messageClient(%cl, 'MsgItemPickup');
		%plural = %count > 1 ? "s" : "";
		%type = %item.getDatablock().stackType;

		%cl.updateSale(%money, %count, %type);
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

function AIPlayer::startSell(%bot, %welcome, %cl)
{
	if (%bot.saleType !$= "Sell" || !isObject(%cl.player))
	{
		return;
	}

	%star = "<bitmap:base/client/ui/ci/star> ";

	if (!isObject(%bot.produceType) || %bot.produceCost $= "")
	{
		messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: Sorry, I got nothing to sell right now." );
		return;
	}

	%itemDB = %bot.produceType;
	%cost = %bot.produceCost;

	%cropType = %itemDB.image.cropType;
	%expRequirement = $Farming::Crops::PlantData_[%cropType, "experienceRequired"];
	if (%expRequirement !$= "" && %cl.farmingExperience < %expRequirement)
	{
		%message = "I'd sell you these " @ %itemDB.uiName @ "s, but you need " @ %expRequirement @ " experience to plant 'em! Sorry!";
		messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);
		%bot.setAimObject(%cl.player);
		return;
	}

	if (%cl.talkingTo != %bot)
	{	
		%cl.talkingTo.talkingToCount--;
		if (isObject(%cl.talkingTo) && %cl.talkingTo.talkingToCount <= 0)
		{
			%cl.talkingTo.talkingToCount = 0;
			%cl.talkingTo.startHoleLoop();
			%message = "Never mind...";
			messageClient(%cl, '', %star @ "\c3" @ (%cl.talkingTo.name $= "" ? "Farmer" : %cl.talkingTo.name) @ "\c6: " @ %message);
		}
		%cl.talkingTo = %bot;
		%bot.talkingToCount++;
	}

	%cost = mFloatLength(%cost, 2);
	if (%welcome $= "")
	{
		%welcome = "Howdy!";
	}
	%message = trim(%welcome) SPC "I'm selling " @ %itemDB.uiName @ "s for $" @ %cost @ " each! How many would ya like?";
	%cl.talkingToDist = vectorDist(%cl.player.getHackPosition(), %bot.getHackPosition());
	%cl.talkingType = "Buy" TAB %itemDB;
	%cl.talkingToLoop(%bot);

	%bot.setMoveX(0);
	%bot.setMoveY(0);
	%bot.stopHoleLoop();
	%bot.setAimObject(%cl.player);

	messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);
}

registerOutputEvent("Bot", "startSell", "string 200 100", 1);

function GameConnection::talkingToLoop(%cl, %bot) 
{
	cancel(%cl.talkingToSchedule);
	if (!isObject(%bot))
	{
		%cl.talkingTo = "";
		%cl.talkingToDist = "";
		%cl.talkingType = "";

		return;
	}

	%dist = vectorDist(%cl.player.getHackPosition(), %bot.getHackPosition());
	if (%dist > %cl.talkingToDist + 1.5) //exit
	{
		%star = "<bitmap:base/client/ui/ci/star> ";

		%message = "See ya!";
		%cl.talkingTo = "";
		%cl.talkingToDist = "";
		%cl.talkingType = "";

		%bot.talkingToCount--;
		if (%bot.talkingToCount <= 0)
		{
			%bot.talkingToCount = 0;
			%bot.startHoleLoop();
		}

		%bot.clearAim();

		messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);
		return;
	}

	%cl.talkingToSchedule = %cl.schedule(100, talkingToLoop, %bot);
}

function attemptBuy(%cl, %num)
{
	%bot = %cl.talkingTo;
	if (!isObject(%bot) || !isObject(%cl.player))
	{
		%cl.talkingTo = "";
		%cl.talkingToDist = "";
		%cl.talkingType = "";
		return;
	}

	%item = getWord(%cl.talkingType, 1);
	%itemType = %item.stackType;
	%max = getMaxPickup(%cl.player, %itemType);
	%star = "<bitmap:base/client/ui/ci/star> ";

	%bot.setAimObject(%cl.player);

	if (%bot.produceType !$= %item || %bot.produceCost $= "")
	{
		%message = "Sale's over, sorry!";
		messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);
		return;
	}

	if (%max < %num)
	{
		%message = "You can't hold that many! Your inventory only has space for " @ %max @ "!";
		messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);
		return;
	}
	else
	{
		%total = %bot.produceCost * %num;
		if (%total > %cl.score)
		{
			%message = "You don't have $" @ mFloatLength(%total, 2) @ "!";
			messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);
			return;
		}
		else
		{
			%cl.setScore(%cl.score - %total);
			%plural = %num > 1 ? "s" : "";
			%message = "$" @ mFloatLength(%total, 2) @ " for " @ %num SPC %item.uiName @ %plural @ "! Thanks!";
			messageClient(%cl, '', %star @ "\c3" @ (%bot.name $= "" ? "Farmer" : %bot.name) @ "\c6: " @ %message);

			%numItems = 1;
			if (getField(%max, 1))
			{
				%numItems = getMin(%num, getField(%max, 0));
			}

			for (%ind = 0; %ind < %numItems; %ind++)
			{
				%i = new Item(purchasedItems)
				{
					dataBlock = %item;
					count = %num;
					client = %cl;
					bl_id = %cl.bl_id;
				};
				%i.setTransform(%cl.player.getHackPosition() SPC getWords(%bot.getTransform(), 3, 6));
				%i.setCollisionTimeout(%bot);
				%i.schedule(60000, schedulePop);
			}
			%bot.playThread(0, activate);

			%pos = %cl.player.getHackPosition();
			for (%ind = 0; %ind < 10; %ind++)
			{
				if (!isObject(%i))
				{
					break;
				}
				Armor::onCollision(%cl.player.getDatablock(), %cl.player, %i, %pos, 0, 0);
			}

			%cl.talkingTo = "";
			%cl.talkingToDist = "";
			%cl.talkingType = "";

			%bot.talkingToCount--;
			if (%bot.talkingToCount <= 0)
			{
				%bot.talkingToCount = 0;
				%bot.startHoleLoop();
			}
			%cl.talkingToLoop();

			%bot.clearAim();
		}
	}
}

if(isPackage(BotBuyProduce))
	deactivatePackage(BotBuyProduce);
package BotBuyProduce
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (%cl.talkingTo && isObject(%cl.player) && getWordCount(%msg) == 1)
		{
			%num = mFloor(%msg);

			if (%num > 0)
			{
				%talkType = %cl.talkingType;

				switch$ (getWord(%talkType, 0))
				{
					case "Buy":
						cancel(%cl.talkingToSched);
						%cl.talkingToSched = schedule(1000, %cl, attemptBuy, %cl, %num);
						%skipParent = 1;
					default:
						cancel(%cl.talkingToSched);
				}
			}
		}

		if (%skipParent)
		{
			%star = "<bitmap:base/client/ui/ci/star>";
			messageClient(%cl, '', %star @ " \c7" @ %cl.clanPrefix @ "\c3" @ %cl.name @ "\c7" @ %clanSuffix @ "\c6: " @ stripMLControlChars(%msg));
			return;
		}

		return parent::serverCmdMessageSent(%cl, %msg);
	}

	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getClassName() $= "AIPlayer" && !isObject(%obj.client) && %obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0)
		{
			%itemDB = %col.getDatablock();
			if (%col.getClassName() $= "Item" && (%itemDB.isStackable || %itemDB.isSellable) && %obj.saleType $= "Buy")
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

	function GameConnection::onDrop(%cl)
	{
		if (isObject(%cl.talkingTo))
		{
			%bot = %cl.talkingTo;
			%bot.talkingToCount--;
			if (%bot.talkingToCount <= 0)
			{
				%bot.talkingToCount = 0;
				%bot.startHoleLoop();
			}
		}

		return parent::onDrop(%cl);
	}
};
schedule(2000, 0, activatePackage, BotBuyProduce);