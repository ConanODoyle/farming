
exec("./crops/datablocks.cs");
exec("./crops/growth.cs");
exec("./crops/plantData.cs");
exec("./crops/water.cs");
exec("./crops/sprinklers.cs");
exec("./crops/harvest.cs");
exec("./crops/fertilizer.cs");
exec("./crops/seeds/seedDatablocks.cs");
exec("./crops/crops/cropDatablocks.cs");
exec("./crops/shinyPlants.cs");

exec("./vehicles/cart.cs");

exec("./audio.cs");
exec("./spawn.cs");
exec("./bus.cs");
exec("./sacrifice.cs");
exec("./donator.cs");
exec("./lots.cs");
exec("./lines.cs");
exec("./buildCost.cs");
	exec("./disableWrenchAndBuild.cs");
	exec("./wrenchCost.cs");
exec("./info.cs");
exec("./ipCheck.cs");
exec("./botBuy.cs");
	exec("./prices.cs");
	exec("./randomEvents.cs");
exec("./eventStorage.cs");
exec("./tutorial.cs");
exec("./writePlantData.cs");
// exec("./mailCatalog.cs");

exec("./Support_CenterprintMenuSystem.cs");
exec("./Support_CustomBrickRadius.cs");
exec("./Support_StackableItems.cs");
exec("./Support_MultipleSlots.cs");
exec("./Support_DropItemsOnDeath.cs");

function servercmdclearitems(%cl)
{
	if (%cl.isAdmin)
	{
		%pos = %cl.player.position;
		initcontainerradiussearch(%pos, 1000, $Typemasks::itemobjecttype);
		while (isobject(%next = containerSearchNext()))
		{
			if (!%next.isStatic())
				%next.delete();
		}
	}
	messageAll('MsgClearBricks', "\c2" @%cl.name @ " cleared all dropped items.");
}

function serverCmdStuck(%cl)
{
	if (%cl.nextUnstuckTime < $Sim::Time)
	{
		if (isObject(%pl = %cl.player))
		{
			%pl.setTransform(vectorAdd(%pl.getTransform(), "0 0 2.5") SPC getWords(%pl.getTransform(), 3, 6));
			%cl.nextUnstuckTime = $Sim::Time + 3;
		}
	}
	else
	{
		messageClient(%cl, '', "You have to wait to do /stuck again!");
	}
}

package scoreFix
{
	function GameConnection::setScore(%cl, %score)
	{
		if (%score < 1000000)
		{
			%score = (mFloor(%score * 100) | 0) / 100; //ensure integers if %score value doesnt have decimal
		}
		else
		{
			%score = 1000000;
		}

		return parent::setScore(%cl, %score);
	}
};
activatePackage(scoreFix);

package noDualClienting
{
	function GameConnection::autoAdminCheck(%cl)
	{
		for (%i = 0; %i < Clientgroup.getCount(); %i++)
		{
			%t = Clientgroup.getObject(%i);
			if (%t != %cl && %t.bl_id == %cl.bl_id)
			{
				%cl.delete("No dual clienting allowed!");
				return;
			}
		}

		return parent::autoAdminCheck(%cl);
	}
};
activatePackage(noDualClienting);

package wateringCat
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (isObject(%cl.player))
		{
			for (%i = 0; %i < %cl.player.getDatablock().maxTools; %i++)
			{
				if (%cl.player.tool[%i] == "WateringCatItem".getID())
				{
					%hasWateringCat = 1;
					break;
				}
			}
		}

		if (%hasWateringCat && getRandom() < 0.15 && strPos(%msg, ":0") < 0)
		{
			%msg = %msg SPC ":0";
		}

		return parent::serverCmdMessageSent(%cl, %msg);
	}
};
activatePackage(wateringCat);

JeepVehicle.maxWheelSpeed = 18;
JeepVehicle.uiName = "Jeep - $1000";
JeepVehicle.cost = 1000;
HorseArmor.cost = 250;
HorseArmor.uiName = "Horse - $250";
$Game::Item::PopTime = 90000;

exec("Add-ons/Script_Player_Persistence/server.cs");

RegisterPersistenceVar("farmingExperience", false, "");

RegisterPersistenceVar("toolStackCount0", false, "");
RegisterPersistenceVar("toolStackCount1", false, "");
RegisterPersistenceVar("toolStackCount2", false, "");
RegisterPersistenceVar("toolStackCount3", false, "");
RegisterPersistenceVar("toolStackCount4", false, "");
RegisterPersistenceVar("toolStackCount5", false, "");
RegisterPersistenceVar("toolStackCount6", false, "");
RegisterPersistenceVar("toolStackCount7", false, "");
RegisterPersistenceVar("toolStackCount8", false, "");
RegisterPersistenceVar("toolStackCount9", false, "");
RegisterPersistenceVar("toolStackCount10", false, "");
RegisterPersistenceVar("toolStackCount12", false, "");
RegisterPersistenceVar("toolStackCount13", false, "");
RegisterPersistenceVar("toolStackCount14", false, "");
RegisterPersistenceVar("toolStackCount15", false, "");
RegisterPersistenceVar("toolStackCount16", false, "");
RegisterPersistenceVar("toolStackCount17", false, "");
RegisterPersistenceVar("toolStackCount18", false, "");
RegisterPersistenceVar("toolStackCount19", false, "");

RegisterPersistenceVar("deliveryPackageInfo0", false, "");
RegisterPersistenceVar("deliveryPackageInfo1", false, "");
RegisterPersistenceVar("deliveryPackageInfo2", false, "");
RegisterPersistenceVar("deliveryPackageInfo3", false, "");
RegisterPersistenceVar("deliveryPackageInfo4", false, "");
RegisterPersistenceVar("deliveryPackageInfo5", false, "");
RegisterPersistenceVar("deliveryPackageInfo6", false, "");
RegisterPersistenceVar("deliveryPackageInfo7", false, "");
RegisterPersistenceVar("deliveryPackageInfo8", false, "");
RegisterPersistenceVar("deliveryPackageInfo9", false, "");
RegisterPersistenceVar("deliveryPackageInfo10", false, "");
RegisterPersistenceVar("deliveryPackageInfo12", false, "");
RegisterPersistenceVar("deliveryPackageInfo13", false, "");
RegisterPersistenceVar("deliveryPackageInfo14", false, "");
RegisterPersistenceVar("deliveryPackageInfo15", false, "");
RegisterPersistenceVar("deliveryPackageInfo16", false, "");
RegisterPersistenceVar("deliveryPackageInfo17", false, "");
RegisterPersistenceVar("deliveryPackageInfo18", false, "");
RegisterPersistenceVar("deliveryPackageInfo19", false, "");


function totalWater(%type, %startStage)
{
	%ticks = $Farming::Crops::plantData_[%type, %startStage, "numGrowTicks"];
	%time = $Farming::Crops::plantData_[%type, %startStage, "timePerTick"];
	%water = $Farming::Crops::plantData_[%type, %startStage, "waterPerTick"];
	if (%time <= 0)
	{
		return 0;
	}
	else
	{
		return %water * %ticks + totalWater(%type, %startStage + 1);
	}
}

function totalTime(%type, %startStage)
{
	%ticks = $Farming::Crops::plantData_[%type, %startStage, "numGrowTicks"];
	%time = $Farming::Crops::plantData_[%type, %startStage, "timePerTick"];
	if (%time <= 0)
	{
		return 0;
	}
	else
	{
		return %time * %ticks + totalTime(%type, %startStage + 1);
	}
}

$FakeClient = new ScriptObject(FakeClients) {isAdmin = 1; isSuperAdmin = 1;};

function loadLastAutosave()
{
	fcn(Conan).bypassRestrictions = 1;
	fcn(Zeustal).bypassRestrictions = 1;
	serverCmdLoadAutosave($FakeClient, "last");

	schedule(15000, 0, setAllWaterLevelsFull);
}

function serverCmdBuilder(%cl, %target)
{
	if (%cl.isSuperAdmin)
	{
		if (strlen(%target) > 1 && isObject(%targ = findClientByName(%target)))
		{
			messageClient(%cl, '', "\c6You gave \c3" @ %targ.name @ "\c6 builder mode!");
			%cl = %targ;
		}
		%cl.bypassRestrictions = 1;
		%cl.player.setDatablock(PlayerStandardArmor);
		messageClient(%cl, '', "\c6You are now a builder!");
	}
}

function serverCmdUnbuilder(%cl, %target)
{
	if (%cl.isSuperAdmin)
	{
		if (strlen(%target) > 1 && isObject(%targ = findClientByName(%target)))
		{
			messageClient(%cl, '', "\c6You removed \c3" @ %targ.name @ "\c6's builder mode!");
			%cl = %targ;
		}
		%cl.bypassRestrictions = 0;
		%cl.player.setDatablock(isObject(%cl.playerDatablock) ? %cl.playerDatablock : PlayerNoJet);
		messageClient(%cl, '', "\c6You are not a builder anymore!");
	}
}

schedule(1000, 0, sprinklerTick, 0);
schedule(1000, 0, rainCheckLoop);
schedule(1000, 0, generateInstrumentList);
//DO NOT schedule the growTick call - it will break loading plants!

function setAllWaterLevelsFull()
{
	for (%i = 0; %i < MainBrickgroup.getCount(); %i++)
	{
		%group = MainBrickgroup.getObject(%i);

		for (%j = 0; %j < %group.getCount(); %j++)
		{
			%brick = %group.getObject(%j);
			if (%brick.getDatablock().isWaterTank || %brick.getDatablock().isDirt)
			{
				%brick.setWaterLevel(100000);
			}
		}
	}
}

package TreasureChestReward
{
	function fxDTSBrick::openTreasureChest(%brick, %pl)
	{
		%ret = parent::openTreasureChest(%brick, %pl);
		%cl = %pl.client;
		%cl.setScore(%cl.score + 100);
		messageClient(%cl, '', "\c6You got \c2$100\c6 for finding the chest!");
		return %ret;
	}
};
activatePackage(TreasureChestReward);

package brickDeath
{
	function fxDTSBrick::onDeath(%obj)
	{
		%ret = parent::onDeath(%obj);
		%obj.isDead = 1;
		// if (%obj.getDatablock().isSprinkler)
		// {
		// 	SprinklerSimSet.remove(%obj);
		// }
		return %ret;
	}

	function fxDTSBrick::onRemove(%obj)
	{
		%ret = parent::onRemove(%obj);
		// if (%obj.getDatablock().isSprinkler)
		// {
		// 	SprinklerSimSet.remove(%obj);
		// }
		return %ret;
	}
};
activatePackage(brickDeath);

function talkIsDead(%b)
{
	talk("isDead? " @ %b.isDead);
}

registerloadingscreen("https://i.imgur.com/06fAw4h.png", "png", "", 1);


schedule(1, 0, eval, "$Pref::Server::Password = \"its closed\";");