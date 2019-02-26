//centerprint menu system
//modular system written by conan


if (!isObject(exampleCenterprintMenu))
{
	new ScriptObject(exampleCenterprintMenu)
	{
		isCenterprintMenu = 1;
		menuName = "Test Menu";

		menuOption[0] = "Option 1";
		menuFunction[0] = "option1Func";
		menuOption[1] = "Option 2";
		menuFunction[1] = "option2Func";
		menuOption[2] = "Option 3";
		menuFunction[2] = "option3Func";

		//deleteOnExit = 1;

		menuOptionCount = 3;
	};
	// MissionCleanup.add(exampleCenterprintMenu);
}

if (!isObject(exampleCenterprintMenu2))
{
	new ScriptObject(exampleCenterprintMenu2)
	{
		isCenterprintMenu = 1;
		menuName = "Test Menu2";

		menuOption[0] = "Option 0";
		menuFunction[0] = "option0Func";
		menuOption[1] = "Option 1";
		menuFunction[1] = "option1Func";
		menuOption[2] = "Option 2";
		menuFunction[2] = "option2Func";
		menuOption[3] = "Option 3";
		menuFunction[3] = "option3Func";
		menuOption[4] = "Option 4";
		menuFunction[4] = "option4Func";
		menuOption[5] = "Option 5";
		menuFunction[5] = "option5Func";
		menuOption[6] = "Option 6";
		menuFunction[6] = "option6Func";

		menuOptionCount = 7;
	};
	// MissionCleanup.add(exampleCenterprintMenu2);
}

//core//

function GameConnection::startCenterprintMenu(%cl, %menuObj)
{
	if (!isObject(%menuObj))
	{
		error("ERROR: startCenterprintMenu cannot find menu \"" @ %menuObj @ "\" to display!");
		return;
	}
	else if (!%menuObj.isCenterprintMenu)
	{
		error("ERROR: %menuObj not a centerprint menu!");
		return;
	}

	if (%cl.lastMessagedBrickControls + 5 < $Sim::Time)
	{
		messageClient(%cl, '', "\c5Use brick controls to navigate the centerprint menu.");
	}
	%cl.lastMessagedBrickControls = $Sim::Time;

	%cl.isInCenterprintMenu = 1;
	%cl.centerprintMenu = %menuObj.getID();
	%cl.currOption = 0;

	%cl.displayCenterprintMenu();
}

function GameConnection::exitCenterprintMenu(%cl)
{
	%cl.centerprint("", 1);
	%cl.isInCenterprintMenu = 0;
	%menu = %cl.centerprintMenu;
	%cl.centerprintMenu = "";

	if (%menu.deleteOnExit)
	{
		%menu.schedule(1000, delete);
	}
}

function GameConnection::displayCenterprintMenu(%cl, %diff)
{
	if (!isObject(%cl.centerprintMenu) || !%cl.isInCenterprintMenu)
	{
		%cl.centerprint("", 1);
		return;
	}

	%menu = %cl.centerprintMenu;

	%menuName = %menu.menuName;
	%optionIDX = %cl.currOption = %cl.currOption + 0; //make sure its a number and not empty string
	if (%diff < 0)
	{
		%diff = %menu.menuOptionCount + %diff;
	}
	%optionIDX = %cl.currOption = (%optionIDX + %diff) % %menu.menuOptionCount;

	if (%diff != 0)
	{
		messageClient(%cl, 'MsgUploadEnd');
	}

	//display up to 5 options
	%min = %optionIDX;
	%max = %optionIDX;
	%dist = %max - %min;
	while (%dist < 4)
	{
		if (%min > 0)
			%min--;
		if (%max < %menu.menuOptionCount - 1)
			%max++;

		if (%min == 0 && %max == %menu.menuOptionCount - 1) //menu is less than 5 options
			%dist = 50;
		else
			%dist = %max - %min;
	}

	%fontA = "<font:Arial:18><color:ffffff>";
	%fontB = "<font:Arial Bold:18><color:ffff00>";

	%line = "<just:right><color:ffff00>" @ %menuName @ " <br>" @ %fontA;
	for (%i = 0; %i <= getMin(%dist, 6); %i++)
	{
		%option = %i + %min;
		if (%option == %optionIDX)
		{
			%line = %line @ %fontB @ %menu.menuOption[%option] @ " <br>" @ %fontA;
		}
		else
		{
			%line = %line @ %menu.menuOption[%option] @ " <br>";
		}
	}

	%cl.centerprint(%line);
}

function GameConnection::confirmCenterprintMenu(%cl)
{
	if (!%cl.isInCenterprintMenu)
	{
		return;
	}

	%menu = %cl.centerprintMenu;
	%option = %cl.currOption;

	%cl.exitCenterprintMenu();
	%func = %menu.menuFunction[%option];
	// messageClient(%cl, 'MsgAdminForce');
	if (!isFunction(%func))
	{
		// error("ERROR: confirmCenterprintMenu: cannot find function " @ %func @ "!");
		return;
	}
	else
	{
		call(%func, %cl, %menu, %option);
	}
}


//package//

//leave menus on death/spawn
package centerprintMenuSystem
{
	function GameConnection::onDeath(%cl, %a, %b, %c, %d, %e, %f)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.exitCenterprintMenu();
		}

		return parent::onDeath(%cl, %a, %b, %c, %d, %e, %f);
	}

	function GameConnection::spawnPlayer(%cl)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.exitCenterprintMenu();
		}

		return parent::spawnPlayer(%cl);
	}

	function serverCmdShiftBrick(%cl, %x, %y, %z)
	{
		if (%cl.isInCenterprintMenu && %x != 0)
		{
			if (%cl.lastCenterprintShift + 0.1 > $Sim::Time && mAbs(%cl.repeatCenterprintShift) > 30)
			{
				%x *= 5;
			}
			else if (%cl.lastCenterprintShift + 0.1 > $Sim::Time)
			{
				%cl.repeatCenterprintShift += -1 * %x;
			}
			else
			{
				%cl.repeatCenterprintShift = 0;
			}
			%cl.lastCenterprintShift = $Sim::Time;
			%cl.displayCenterprintMenu(-1 * %x);
			return;
		}

		return parent::serverCmdShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdSuperShiftBrick(%cl, %x, %y, %z)
	{
		if (%cl.isInCenterprintMenu && %x != 0)
		{
			if (%cl.lastCenterprintShift + 0.1 > $Sim::Time && mAbs(%cl.repeatCenterprintShift) > 30)
			{
				%x *= 5;
			}
			else if (%cl.lastCenterprintShift + 0.1 > $Sim::Time)
			{
				%cl.repeatCenterprintShift += -1 * %x;
			}
			else
			{
				%cl.repeatCenterprintShift = 0;
			}
			%cl.lastCenterprintShift = $Sim::Time;
			%cl.displayCenterprintMenu(-1 * %x);
			return;
		}

		return parent::serverCmdSuperShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdPlantBrick(%cl)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.confirmCenterprintMenu();
			return;
		}

		return parent::serverCmdPlantBrick(%cl);
	}

	function serverCmdCancelBrick(%cl)
	{
		if (%cl.isInCenterprintMenu)
		{
			%cl.exitCenterprintMenu();
			return;
		}

		return parent::serverCmdCancelBrick(%cl);
	}
};
activatePackage(centerprintMenuSystem);